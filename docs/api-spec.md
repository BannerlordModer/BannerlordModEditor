# BannerlordModEditor XML适配系统API规范

## 执行摘要

本文档定义了BannerlordModEditor XML适配系统的关键接口和方法签名。系统采用DO/DTO架构模式，提供了完整的XML序列化/反序列化功能，支持大型文件处理和性能优化。

## 核心接口定义

### 1. 基础接口

#### IXmlProcessor
```csharp
/// <summary>
/// XML处理器基础接口
/// </summary>
public interface IXmlProcessor
{
    /// <summary>
    /// 反序列化XML字符串为DO对象
    /// </summary>
    T Deserialize<T>(string xml) where T : class, new();
    
    /// <summary>
    /// 序列化DO对象为XML字符串
    /// </summary>
    string Serialize<T>(T obj) where T : class, new();
    
    /// <summary>
    /// 从文件加载XML并反序列化
    /// </summary>
    T LoadFromFile<T>(string filePath) where T : class, new();
    
    /// <summary>
    /// 序列化对象并保存到文件
    /// </summary>
    void SaveToFile<T>(T obj, string filePath) where T : class, new();
}
```

#### IMapper<TDO, TDTO>
```csharp
/// <summary>
/// DO/DTO映射器通用接口
/// </summary>
public interface IMapper<TDO, TDTO>
    where TDO : class, new()
    where TDTO : class, new()
{
    /// <summary>
    /// 将DO转换为DTO
    /// </summary>
    TDTO ToDTO(TDO source);
    
    /// <summary>
    /// 将DTO转换为DO
    /// </summary>
    TDO ToDO(TDTO source);
    
    /// <summary>
    /// 批量转换DO列表为DTO列表
    /// </summary>
    List<TDTO> ToDTOList(List<TDO> sourceList);
    
    /// <summary>
    /// 批量转换DTO列表为DO列表
    /// </summary>
    List<TDO> ToDOList(List<TDTO> sourceList);
}
```

### 2. 大型文件处理接口

#### ILargeXmlFileProcessor
```csharp
/// <summary>
/// 大型XML文件处理器接口
/// </summary>
public interface ILargeXmlFileProcessor
{
    /// <summary>
    /// 处理大型XML文件的通用方法
    /// </summary>
    Task<T> ProcessLargeXmlFileAsync<T>(
        string filePath,
        ProcessingStrategy strategy = ProcessingStrategy.Auto,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default) where T : class, new();
    
    /// <summary>
    /// 获取文件处理策略建议
    /// </summary>
    ProcessingStrategy GetRecommendedStrategy(string filePath);
    
    /// <summary>
    /// 分析文件结构
    /// </summary>
    Task<FileStructureInfo> AnalyzeFileStructureAsync(string filePath, CancellationToken cancellationToken = default);
}
```

#### IXmlObjectPool
```csharp
/// <summary>
/// XML对象池接口
/// </summary>
public interface IXmlObjectPool
{
    /// <summary>
    /// 获取对象
    /// </summary>
    T Get<T>() where T : class, new();
    
    /// <summary>
    /// 返回对象
    /// </summary>
    void Return<T>(T obj) where T : class, new();
    
    /// <summary>
    /// 清空所有对象池
    /// </summary>
    void ClearAll();
    
    /// <summary>
    /// 获取统计信息
    /// </summary>
    PoolStatistics GetStatistics();
}
```

### 3. 性能监控接口

#### IXmlPerformanceMonitor
```csharp
/// <summary>
/// XML处理性能监控接口
/// </summary>
public interface IXmlPerformanceMonitor
{
    /// <summary>
    /// 开始监控操作
    /// </summary>
    IDisposable StartOperation(string operationName);
    
    /// <summary>
    /// 记录操作完成
    /// </summary>
    void RecordOperation(string operationName, TimeSpan duration, long bytesProcessed = 0);
    
    /// <summary>
    /// 获取性能报告
    /// </summary>
    PerformanceReport GetReport();
    
    /// <summary>
    /// 重置统计信息
    /// </summary>
    void Reset();
}
```

#### IXmlCacheManager
```csharp
/// <summary>
/// XML缓存管理接口
/// </summary>
public interface IXmlCacheManager
{
    /// <summary>
    /// 获取缓存项
    /// </summary>
    T Get<T>(string key) where T : class;
    
    /// <summary>
    /// 设置缓存项
    /// </summary>
    void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class;
    
    /// <summary>
    /// 清空缓存
    /// </summary>
    void Clear();
    
    /// <summary>
    /// 获取缓存统计
    /// </summary>
    CacheStatistics GetStatistics();
}
```

## 核心类接口定义

### 1. MPClassDivisions 相关接口

#### IMPClassDivisionsProcessor
```csharp
/// <summary>
/// MPClassDivisions处理器接口
/// </summary>
public interface IMPClassDivisionsProcessor
{
    /// <summary>
    /// 处理MPClassDivisions XML文件
    /// </summary>
    Task<MPClassDivisionsDO> ProcessMPClassDivisionsAsync(
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 按文化获取分类
    /// </summary>
    List<MPClassDivisionDO> GetClassDivisionsByCulture(MPClassDivisionsDO data, string culture);
    
    /// <summary>
    /// 按游戏模式获取分类
    /// </summary>
    List<MPClassDivisionDO> GetClassDivisionsByGameMode(MPClassDivisionsDO data, string gameMode);
    
    /// <summary>
    /// 验证游戏平衡性
    /// </summary>
    List<string> ValidateGameBalance(MPClassDivisionsDO data);
}
```

#### IMPClassDivisionValidator
```csharp
/// <summary>
/// MPClassDivision验证器接口
/// </summary>
public interface IMPClassDivisionValidator
{
    /// <summary>
    /// 验证单个分类
    /// </summary>
    ValidationResult ValidateClassDivision(MPClassDivisionDO division);
    
    /// <summary>
    /// 验证所有分类
    /// </summary>
    List<ValidationResult> ValidateAllClassDivisions(MPClassDivisionsDO data);
    
    /// <summary>
    /// 检查平衡性
    /// </summary>
    bool IsBalanced(MPClassDivisionDO division);
}
```

### 2. TerrainMaterials 相关接口

#### ITerrainMaterialsProcessor
```csharp
/// <summary>
/// TerrainMaterials处理器接口
/// </summary>
public interface ITerrainMaterialsProcessor
{
    /// <summary>
    /// 处理TerrainMaterials XML文件
    /// </summary>
    Task<TerrainMaterialsDO> ProcessTerrainMaterialsAsync(
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 按名称获取材质
    /// </summary>
    TerrainMaterialDO? GetMaterialByName(TerrainMaterialsDO data, string name);
    
    /// <summary>
    /// 获取所有植被材质
    /// </summary>
    List<TerrainMaterialDO> GetFloraMaterials(TerrainMaterialsDO data);
    
    /// <summary>
    /// 获取所有网格混合材质
    /// </summary>
    List<TerrainMaterialDO> GetMeshBlendMaterials(TerrainMaterialsDO data);
}
```

#### ITerrainMaterialValidator
```csharp
/// <summary>
/// TerrainMaterial验证器接口
/// </summary>
public interface ITerrainMaterialValidator
{
    /// <summary>
    /// 验证单个材质
    /// </summary>
    ValidationResult ValidateTerrainMaterial(TerrainMaterialDO material);
    
    /// <summary>
    /// 验证所有材质
    /// </summary>
    List<ValidationResult> ValidateAllTerrainMaterials(TerrainMaterialsDO data);
    
    /// <summary>
    /// 检查材质完整性
    /// </summary>
    bool IsComplete(TerrainMaterialDO material);
}
```

### 3. Layouts 相关接口

#### ILayoutsProcessor
```csharp
/// <summary>
/// Layouts处理器接口
/// </summary>
public interface ILayoutsProcessor
{
    /// <summary>
    /// 处理Layouts XML文件
    /// </summary>
    Task<LayoutsBaseDO> ProcessLayoutsAsync(
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 按类获取布局
    /// </summary>
    LayoutDO? GetLayoutByClass(LayoutsBaseDO data, string className);
    
    /// <summary>
    /// 按XML标签获取布局
    /// </summary>
    LayoutDO? GetLayoutByXmlTag(LayoutsBaseDO data, string xmlTag);
    
    /// <summary>
    /// 获取树形视图布局
    /// </summary>
    List<LayoutDO> GetTreeviewLayouts(LayoutsBaseDO data);
    
    /// <summary>
    /// 生成UI配置
    /// </summary>
    UIConfiguration GenerateUIConfiguration(LayoutsBaseDO data);
}
```

#### ILayoutsValidator
```csharp
/// <summary>
/// Layouts验证器接口
/// </summary>
public interface ILayoutsValidator
{
    /// <summary>
    /// 验证单个布局
    /// </summary>
    ValidationResult ValidateLayout(LayoutDO layout);
    
    /// <summary>
    /// 验证所有布局
    /// </summary>
    List<ValidationResult> ValidateAllLayouts(LayoutsBaseDO data);
    
    /// <summary>
    /// 检查布局完整性
    /// </summary>
    bool IsComplete(LayoutDO layout);
}
```

## 事件接口定义

### 1. 处理进度事件

#### IProcessingProgressHandler
```csharp
/// <summary>
/// 处理进度事件处理器接口
/// </summary>
public interface IProcessingProgressHandler
{
    /// <summary>
    /// 处理进度更新
    /// </summary>
    void OnProgressUpdate(ProcessingProgress progress);
    
    /// <summary>
    /// 处理处理完成
    /// </summary>
    void OnProcessingCompleted(ProcessingResult result);
    
    /// <summary>
    /// 处理处理错误
    /// </summary>
    void OnProcessingError(ProcessingError error);
}
```

### 2. 缓存事件

#### ICacheEventHandler
```csharp
/// <summary>
/// 缓存事件处理器接口
/// </summary>
public interface ICacheEventHandler
{
    /// <summary>
    /// 缓存命中事件
    /// </summary>
    void OnCacheHit(string key, Type type);
    
    /// <summary>
    /// 缓存未命中事件
    /// </summary>
    void OnCacheMiss(string key, Type type);
    
    /// <summary>
    /// 缓存过期事件
    /// </summary>
    void OnCacheExpired(string key, Type type);
}
```

## 工厂接口定义

### 1. 处理器工厂

#### IXmlProcessorFactory
```csharp
/// <summary>
/// XML处理器工厂接口
/// </summary>
public interface IXmlProcessorFactory
{
    /// <summary>
    /// 创建通用XML处理器
    /// </summary>
    IXmlProcessor CreateXmlProcessor();
    
    /// <summary>
    /// 创建大型文件处理器
    /// </summary>
    ILargeXmlFileProcessor CreateLargeXmlFileProcessor();
    
    /// <summary>
    /// 创建MPClassDivisions处理器
    /// </summary>
    IMPClassDivisionsProcessor CreateMPClassDivisionsProcessor();
    
    /// <summary>
    /// 创建TerrainMaterials处理器
    /// </summary>
    ITerrainMaterialsProcessor CreateTerrainMaterialsProcessor();
    
    /// <summary>
    /// 创建Layouts处理器
    /// </summary>
    ILayoutsProcessor CreateLayoutsProcessor();
}
```

### 2. 验证器工厂

#### IValidatorFactory
```csharp
/// <summary>
/// 验证器工厂接口
/// </summary>
public interface IValidatorFactory
{
    /// <summary>
    /// 创建MPClassDivisions验证器
    /// </summary>
    IMPClassDivisionValidator CreateMPClassDivisionValidator();
    
    /// <summary>
    /// 创建TerrainMaterials验证器
    /// </summary>
    ITerrainMaterialValidator CreateTerrainMaterialValidator();
    
    /// <summary>
    /// 创建Layouts验证器
    /// </summary>
    ILayoutsValidator CreateLayoutsValidator();
}
```

## 配置接口定义

### 1. 处理策略配置

#### IProcessingConfiguration
```csharp
/// <summary>
/// 处理配置接口
/// </summary>
public interface IProcessingConfiguration
{
    /// <summary>
    /// 获取默认处理策略
    /// </summary>
    ProcessingStrategy GetDefaultStrategy();
    
    /// <summary>
    /// 获取文件大小阈值
    /// </summary>
    long GetFileSizeThreshold();
    
    /// <summary>
    /// 获取最大并行度
    /// </summary>
    int GetMaxDegreeOfParallelism();
    
    /// <summary>
    /// 获取缓冲区大小
    /// </summary>
    int GetBufferSize();
    
    /// <summary>
    /// 获取缓存过期时间
    /// </summary>
    TimeSpan GetCacheExpiration();
}
```

### 2. 性能配置

#### IPerformanceConfiguration
```csharp
/// <summary>
/// 性能配置接口
/// </summary>
public interface IPerformanceConfiguration
{
    /// <summary>
    /// 是否启用对象池
    /// </summary>
    bool IsObjectPoolEnabled();
    
    /// <summary>
    /// 是否启用性能监控
    /// </summary>
    bool IsPerformanceMonitoringEnabled();
    
    /// <summary>
    /// 是否启用缓存
    /// </summary>
    bool IsCacheEnabled();
    
    /// <summary>
    /// 获取对象池最大大小
    /// </summary>
    int GetObjectPoolMaxSize();
    
    /// <summary>
    /// 获取缓存最大大小
    /// </summary>
    int GetCacheMaxSize();
}
```

## 扩展接口定义

### 1. 插件接口

#### IXmlProcessorPlugin
```csharp
/// <summary>
/// XML处理器插件接口
/// </summary>
public interface IXmlProcessorPlugin
{
    /// <summary>
    /// 插件名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 插件版本
    /// </summary>
    Version Version { get; }
    
    /// <summary>
    /// 支持的文件类型
    /// </summary>
    IEnumerable<string> SupportedFileTypes { get; }
    
    /// <summary>
    /// 初始化插件
    /// </summary>
    void Initialize(IServiceProvider serviceProvider);
    
    /// <summary>
    /// 处理XML文件
    /// </summary>
    Task<object> ProcessAsync(string filePath, ProcessingContext context);
}
```

### 2. 自定义验证器接口

#### ICustomValidator
```csharp
/// <summary>
/// 自定义验证器接口
/// </summary>
public interface ICustomValidator
{
    /// <summary>
    /// 验证器名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 支持的类型
    /// </summary>
    Type SupportedType { get; }
    
    /// <summary>
    /// 验证对象
    /// </summary>
    ValidationResult Validate(object obj);
    
    /// <summary>
    /// 批量验证
    /// </summary>
    List<ValidationResult> ValidateAll(IEnumerable<object> objects);
}
```

## 数据传输对象定义

### 1. 处理结果

#### ProcessingResult
```csharp
/// <summary>
/// 处理结果
/// </summary>
public class ProcessingResult
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 处理的对象
    /// </summary>
    public object? ResultObject { get; set; }
    
    /// <summary>
    /// 处理时间
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// 处理的字节数
    /// </summary>
    public long BytesProcessed { get; set; }
    
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// 异常信息
    /// </summary>
    public Exception? Exception { get; set; }
}
```

### 2. 验证结果

#### ValidationResult
```csharp
/// <summary>
/// 验证结果
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// 是否有效
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// 验证的消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 验证的级别
    /// </summary>
    public ValidationLevel Level { get; set; } = ValidationLevel.Info;
    
    /// <summary>
    /// 验证的字段
    /// </summary>
    public string? Field { get; set; }
    
    /// <summary>
    /// 建议的修复方案
    /// </summary>
    public string? SuggestedFix { get; set; }
}
```

### 3. 进度信息

#### ProcessingProgress
```csharp
/// <summary>
/// 处理进度信息
/// </summary>
public class ProcessingProgress
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// 文件大小
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// 已处理的字节数
    /// </summary>
    public long ProcessedBytes { get; set; }
    
    /// <summary>
    /// 总字节数
    /// </summary>
    public long TotalBytes { get; set; }
    
    /// <summary>
    /// 进度百分比
    /// </summary>
    public int ProgressPercentage { get; set; }
    
    /// <summary>
    /// 状态信息
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 处理策略
    /// </summary>
    public string Strategy { get; set; } = string.Empty;
}
```

## 枚举定义

### 1. 处理策略

```csharp
/// <summary>
/// 处理策略枚举
/// </summary>
public enum ProcessingStrategy
{
    Auto,           // 自动选择最佳策略
    Streaming,      // 流式处理
    Chunked,        // 分片处理
    Parallel,       // 并行处理
    Hybrid          // 混合处理
}
```

### 2. 验证级别

```csharp
/// <summary>
/// 验证级别枚举
/// </summary>
public enum ValidationLevel
{
    Debug,          // 调试信息
    Info,           // 一般信息
    Warning,        // 警告
    Error,          // 错误
    Critical        // 严重错误
}
```

## 异步方法签名

### 1. 基础异步方法

```csharp
/// <summary>
/// 异步反序列化
/// </summary>
Task<T> DeserializeAsync<T>(string xml, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步序列化
/// </summary>
Task<string> SerializeAsync<T>(T obj, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步从文件加载
/// </summary>
Task<T> LoadFromFileAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步保存到文件
/// </summary>
Task SaveToFileAsync<T>(T obj, string filePath, CancellationToken cancellationToken = default) where T : class, new();
```

### 2. 大型文件处理异步方法

```csharp
/// <summary>
/// 异步分片处理
/// </summary>
Task<T> ProcessInChunksAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步流式处理
/// </summary>
Task<T> ProcessWithStreamingAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步并行处理
/// </summary>
Task<T> ProcessWithParallelAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : class, new();

/// <summary>
/// 异步混合处理
/// </summary>
Task<T> ProcessWithHybridAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : class, new();
```

## 扩展方法

### 1. 处理器扩展

```csharp
/// <summary>
/// 处理器扩展方法
/// </summary>
public static class XmlProcessorExtensions
{
    /// <summary>
    /// 处理MPClassDivisions文件
    /// </summary>
    public static Task<MPClassDivisionsDO> ProcessMPClassDivisionsAsync(
        this ILargeXmlFileProcessor processor,
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 处理TerrainMaterials文件
    /// </summary>
    public static Task<TerrainMaterialsDO> ProcessTerrainMaterialsAsync(
        this ILargeXmlFileProcessor processor,
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 处理Layouts文件
    /// </summary>
    public static Task<LayoutsBaseDO> ProcessLayoutsAsync(
        this ILargeXmlFileProcessor processor,
        string filePath,
        IProgress<ProcessingProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
```

### 2. 验证器扩展

```csharp
/// <summary>
/// 验证器扩展方法
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// 验证并抛出异常
    /// </summary>
    public static void ValidateAndThrow<T>(this IValidator<T> validator, T obj)
    {
        var result = validator.Validate(obj);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Message);
        }
    }
    
    /// <summary>
    /// 验证所有并抛出异常
    /// </summary>
    public static void ValidateAllAndThrow<T>(this IValidator<T> validator, IEnumerable<T> objects)
    {
        var results = validator.ValidateAll(objects);
        var errors = results.Where(r => !r.IsValid).ToList();
        if (errors.Count > 0)
        {
            throw new ValidationAggregateException(errors);
        }
    }
}
```

## 总结

本API规范定义了BannerlordModEditor XML适配系统的完整接口体系，包括：

1. **核心处理接口**：提供XML序列化/反序列化的基础功能
2. **大型文件处理接口**：支持分片、流式、并行等高级处理策略
3. **性能优化接口**：提供对象池、缓存、监控等性能优化功能
4. **验证接口**：确保数据完整性和业务规则符合性
5. **扩展接口**：支持插件和自定义验证器的扩展
6. **配置接口**：提供灵活的配置管理

所有接口都遵循了异步编程模式，支持CancellationToken，并提供了完整的进度报告和错误处理机制。该API设计既考虑了易用性，又保证了高性能和可扩展性。