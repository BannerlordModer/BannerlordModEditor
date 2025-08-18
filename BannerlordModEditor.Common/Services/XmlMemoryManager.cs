using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace BannerlordModEditor.Common.Services;

/// <summary>
/// XML对象池管理器
/// 用于减少GC压力和提高性能
/// </summary>
public class XmlObjectPool
{
    // 对象池
    private readonly ConcurrentDictionary<Type, object> _pools = new ConcurrentDictionary<Type, object>();

    // 池配置
    private readonly int _maxPoolSize;
    private readonly bool _enableTracking;

    /// <summary>
    /// 构造函数
    /// </summary>
    public XmlObjectPool(int maxPoolSize = 1000, bool enableTracking = false)
    {
        _maxPoolSize = maxPoolSize;
        _enableTracking = enableTracking;
    }

    /// <summary>
    /// 获取对象池
    /// </summary>
    public ObjectPool<T> GetPool<T>() where T : class, new()
    {
        return (ObjectPool<T>)_pools.GetOrAdd(typeof(T), _ => 
            new ObjectPool<T>(_maxPoolSize, _enableTracking));
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    public T Get<T>() where T : class, new()
    {
        return GetPool<T>().Get();
    }

    /// <summary>
    /// 返回对象
    /// </summary>
    public void Return<T>(T obj) where T : class, new()
    {
        GetPool<T>().Return(obj);
    }

    /// <summary>
    /// 清空所有对象池
    /// </summary>
    public void ClearAll()
    {
        foreach (var pool in _pools.Values)
        {
            if (pool is IClearable clearable)
            {
                clearable.Clear();
            }
        }
        _pools.Clear();
    }

    /// <summary>
    /// 获取统计信息
    /// </summary>
    public PoolStatistics GetStatistics()
    {
        var stats = new PoolStatistics();
        
        foreach (var kvp in _pools)
        {
            if (kvp.Value is ITrackable trackable)
            {
                var poolStats = trackable.GetStatistics();
                stats.PoolStats[kvp.Key.Name] = poolStats;
            }
        }

        return stats;
    }
}

/// <summary>
/// 通用对象池
/// </summary>
public class ObjectPool<T> where T : class, new()
{
    private readonly ConcurrentBag<T> _pool;
    private readonly int _maxSize;
    private readonly bool _enableTracking;
    private int _createdCount;
    private int _returnedCount;
    private int _rentedCount;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ObjectPool(int maxSize = 1000, bool enableTracking = false)
    {
        _maxSize = maxSize;
        _enableTracking = enableTracking;
        _pool = new ConcurrentBag<T>();
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    public T Get()
    {
        if (_pool.TryTake(out var obj))
        {
            if (_enableTracking)
            {
                Interlocked.Increment(ref _rentedCount);
            }
            return obj;
        }

        // 池为空，创建新对象
        if (_enableTracking)
        {
            Interlocked.Increment(ref _createdCount);
            Interlocked.Increment(ref _rentedCount);
        }
        return new T();
    }

    /// <summary>
    /// 返回对象
    /// </summary>
    public void Return(T obj)
    {
        if (_pool.Count < _maxSize)
        {
            // 重置对象状态
            if (obj is IResettable resettable)
            {
                resettable.Reset();
            }

            _pool.Add(obj);
            
            if (_enableTracking)
            {
                Interlocked.Increment(ref _returnedCount);
            }
        }
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        _pool.Clear();
    }

    /// <summary>
    /// 获取统计信息
    /// </summary>
    public PoolStatisticsItem GetStatistics()
    {
        return new PoolStatisticsItem
        {
            TypeName = typeof(T).Name,
            PoolSize = _pool.Count,
            MaxSize = _maxSize,
            CreatedCount = _createdCount,
            ReturnedCount = _returnedCount,
            RentedCount = _rentedCount,
            InUseCount = _rentedCount - _returnedCount
        };
    }
}

/// <summary>
/// 对象重置接口
/// </summary>
public interface IResettable
{
    void Reset();
}

/// <summary>
/// 可清空接口
/// </summary>
public interface IClearable
{
    void Clear();
}

/// <summary>
/// 可跟踪接口
/// </summary>
public interface ITrackable
{
    PoolStatisticsItem GetStatistics();
}

/// <summary>
/// 池统计信息
/// </summary>
public class PoolStatistics
{
    public Dictionary<string, PoolStatisticsItem> PoolStats { get; set; } = new Dictionary<string, PoolStatisticsItem>();
    public int TotalPoolSize { get; set; }
    public int TotalCreatedCount { get; set; }
    public int TotalReturnedCount { get; set; }
    public int TotalRentedCount { get; set; }

    public void CalculateTotals()
    {
        TotalPoolSize = 0;
        TotalCreatedCount = 0;
        TotalReturnedCount = 0;
        TotalRentedCount = 0;

        foreach (var stat in PoolStats.Values)
        {
            TotalPoolSize += stat.PoolSize;
            TotalCreatedCount += stat.CreatedCount;
            TotalReturnedCount += stat.ReturnedCount;
            TotalRentedCount += stat.RentedCount;
        }
    }
}

/// <summary>
/// 单个池的统计信息
/// </summary>
public class PoolStatisticsItem
{
    public string TypeName { get; set; } = string.Empty;
    public int PoolSize { get; set; }
    public int MaxSize { get; set; }
    public int CreatedCount { get; set; }
    public int ReturnedCount { get; set; }
    public int RentedCount { get; set; }
    public int InUseCount { get; set; }
    public double HitRate => RentedCount > 0 ? (double)ReturnedCount / RentedCount : 0;
}

/// <summary>
/// XML处理性能监控器
/// </summary>
public class XmlPerformanceMonitor
{
    private readonly Dictionary<string, PerformanceMetrics> _metrics = new Dictionary<string, PerformanceMetrics>();

    /// <summary>
    /// 开始监控操作
    /// </summary>
    public IDisposable StartOperation(string operationName)
    {
        return new PerformanceTracker(this, operationName);
    }

    /// <summary>
    /// 记录操作完成
    /// </summary>
    public void RecordOperation(string operationName, TimeSpan duration, long bytesProcessed = 0)
    {
        lock (_metrics)
        {
            if (!_metrics.ContainsKey(operationName))
            {
                _metrics[operationName] = new PerformanceMetrics();
            }

            var metrics = _metrics[operationName];
            metrics.RecordOperation(duration, bytesProcessed);
        }
    }

    /// <summary>
    /// 获取性能报告
    /// </summary>
    public PerformanceReport GetReport()
    {
        var report = new PerformanceReport();
        
        lock (_metrics)
        {
            foreach (var kvp in _metrics)
            {
                report.Metrics[kvp.Key] = kvp.Value.Clone();
            }
        }

        return report;
    }

    /// <summary>
    /// 重置统计信息
    /// </summary>
    public void Reset()
    {
        lock (_metrics)
        {
            _metrics.Clear();
        }
    }
}

/// <summary>
/// 性能跟踪器
/// </summary>
public class PerformanceTracker : IDisposable
{
    private readonly XmlPerformanceMonitor _monitor;
    private readonly string _operationName;
    private readonly DateTime _startTime;
    private long _bytesProcessed;

    public PerformanceTracker(XmlPerformanceMonitor monitor, string operationName)
    {
        _monitor = monitor;
        _operationName = operationName;
        _startTime = DateTime.UtcNow;
    }

    public void SetBytesProcessed(long bytes)
    {
        _bytesProcessed = bytes;
    }

    public void Dispose()
    {
        var duration = DateTime.UtcNow - _startTime;
        _monitor.RecordOperation(_operationName, duration, _bytesProcessed);
    }
}

/// <summary>
/// 性能指标
/// </summary>
public class PerformanceMetrics
{
    public long OperationCount { get; private set; }
    public TimeSpan TotalDuration { get; private set; }
    public TimeSpan MinDuration { get; private set; }
    public TimeSpan MaxDuration { get; private set; }
    public long TotalBytesProcessed { get; private set; }
    public double AverageBytesPerSecond { get; private set; }

    public void RecordOperation(TimeSpan duration, long bytesProcessed)
    {
        OperationCount++;
        TotalDuration += duration;
        
        if (duration < MinDuration || MinDuration == TimeSpan.Zero)
        {
            MinDuration = duration;
        }
        
        if (duration > MaxDuration)
        {
            MaxDuration = duration;
        }

        TotalBytesProcessed += bytesProcessed;
        
        if (TotalDuration.TotalSeconds > 0)
        {
            AverageBytesPerSecond = TotalBytesProcessed / TotalDuration.TotalSeconds;
        }
    }

    public PerformanceMetrics Clone()
    {
        return new PerformanceMetrics
        {
            OperationCount = OperationCount,
            TotalDuration = TotalDuration,
            MinDuration = MinDuration,
            MaxDuration = MaxDuration,
            TotalBytesProcessed = TotalBytesProcessed,
            AverageBytesPerSecond = AverageBytesPerSecond
        };
    }
}

/// <summary>
/// 性能报告
/// </summary>
public class PerformanceReport
{
    public Dictionary<string, PerformanceMetrics> Metrics { get; set; } = new Dictionary<string, PerformanceMetrics>();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public override string ToString()
    {
        var report = "XML Processing Performance Report\n";
        report += $"Generated at: {GeneratedAt}\n";
        report += new string('=', 50) + "\n";

        foreach (var kvp in Metrics)
        {
            var metrics = kvp.Value;
            report += $"\nOperation: {kvp.Key}\n";
            report += $"  Count: {metrics.OperationCount}\n";
            report += $"  Total Duration: {metrics.TotalDuration}\n";
            report += $"  Average Duration: {TimeSpan.FromMilliseconds(metrics.TotalDuration.TotalMilliseconds / metrics.OperationCount)}\n";
            report += $"  Min Duration: {metrics.MinDuration}\n";
            report += $"  Max Duration: {metrics.MaxDuration}\n";
            report += $"  Total Bytes: {metrics.TotalBytesProcessed:N0}\n";
            report += $"  Average Bytes/sec: {metrics.AverageBytesPerSecond:N0}\n";
        }

        return report;
    }
}

/// <summary>
/// XML缓存管理器
/// </summary>
public class XmlCacheManager
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new ConcurrentDictionary<string, CacheEntry>();
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);
    private readonly int _maxCacheSize = 100;
    private readonly object _cleanupLock = new object();

    /// <summary>
    /// 获取缓存项
    /// </summary>
    public T Get<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (DateTime.UtcNow < entry.Expiration)
            {
                return (T)entry.Value;
            }
            else
            {
                // 过期项清理
                _cache.TryRemove(key, out _);
            }
        }
        return null;
    }

    /// <summary>
    /// 设置缓存项
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        var entry = new CacheEntry
        {
            Value = value,
            Expiration = DateTime.UtcNow + (expiration ?? _defaultExpiration)
        };

        _cache.AddOrUpdate(key, entry, (k, v) => entry);

        // 清理过期项
        CleanupExpiredEntries();
    }

    /// <summary>
    /// 清理过期项
    /// </summary>
    private void CleanupExpiredEntries()
    {
        lock (_cleanupLock)
        {
            if (_cache.Count > _maxCacheSize)
            {
                var expiredKeys = _cache
                    .Where(kvp => kvp.Value.Expiration < DateTime.UtcNow)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    _cache.TryRemove(key, out _);
                }
            }
        }
    }

    /// <summary>
    /// 清空缓存
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
    }

    /// <summary>
    /// 获取缓存统计
    /// </summary>
    public CacheStatistics GetStatistics()
    {
        var now = DateTime.UtcNow;
        var expiredCount = _cache.Count(kvp => kvp.Value.Expiration < now);
        var validCount = _cache.Count - expiredCount;

        return new CacheStatistics
        {
            TotalEntries = _cache.Count,
            ValidEntries = validCount,
            ExpiredEntries = expiredCount,
            MaxSize = _maxCacheSize
        };
    }
}

/// <summary>
/// 缓存项
/// </summary>
public class CacheEntry
{
    public object Value { get; set; }
    public DateTime Expiration { get; set; }
}

/// <summary>
/// 缓存统计
/// </summary>
public class CacheStatistics
{
    public int TotalEntries { get; set; }
    public int ValidEntries { get; set; }
    public int ExpiredEntries { get; set; }
    public int MaxSize { get; set; }
}