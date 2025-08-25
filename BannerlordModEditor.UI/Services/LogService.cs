using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 日志服务实现
/// 
/// 这个服务提供了完整的日志记录功能，包括：
/// - 多级别日志记录（Debug、Info、Warning、Error、Critical）
/// - 分类日志管理
/// - 文件日志记录
/// - 日志历史管理
/// - 日志导出功能
/// 
/// 使用方式：
/// <code>
/// var logService = new LogService();
/// logService.LogInfo("Application started", "Startup");
/// logService.LogError("An error occurred", "Database");
/// </code>
/// </summary>
public partial class LogService : ObservableObject, ILogService
{
    private readonly Queue<LogEntry> _logEntries = new Queue<LogEntry>();
    private readonly object _lock = new object();
    private const int MaxLogEntries = 1000;
    private string _logFilePath = string.Empty;

    [ObservableProperty]
    private LogLevel currentLogLevel = LogLevel.Info;

    [ObservableProperty]
    private bool enableFileLogging = false;

    [ObservableProperty]
    private string logDirectory = "logs";

    public LogService()
    {
        InitializeLogDirectory();
    }

    public void LogInfo(string message, string category = "General")
    {
        AddLogEntry(LogLevel.Info, message, category);
    }

    public void LogWarning(string message, string category = "General")
    {
        AddLogEntry(LogLevel.Warning, message, category);
    }

    public void LogError(string message, string category = "General")
    {
        AddLogEntry(LogLevel.Error, message, category);
    }

    public void LogException(Exception exception, string context = "")
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = LogLevel.Error,
            Message = exception.Message,
            Category = context,
            ExceptionInfo = exception.GetType().Name,
            StackTrace = exception.StackTrace ?? string.Empty
        };

        AddLogEntry(logEntry);
    }

    public void LogDebug(string message, string category = "General")
    {
        AddLogEntry(LogLevel.Debug, message, category);
    }

    public IEnumerable<LogEntry> GetLogHistory(int count = 100)
    {
        lock (_lock)
        {
            return _logEntries.TakeLast(count).ToList();
        }
    }

    public void ClearLogHistory()
    {
        lock (_lock)
        {
            _logEntries.Clear();
        }
    }

    public async Task<bool> SaveLogToFileAsync(string filePath)
    {
        try
        {
            var logContent = new StringBuilder();
            
            lock (_lock)
            {
                foreach (var entry in _logEntries)
                {
                    logContent.AppendLine(FormatLogEntry(entry));
                }
            }

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(filePath, logContent.ToString());
            return true;
        }
        catch (Exception ex)
        {
            LogError($"Failed to save log to file: {ex.Message}", "LogService");
            return false;
        }
    }

    private void AddLogEntry(LogLevel level, string message, string category)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            Category = category
        };

        AddLogEntry(logEntry);
    }

    private void AddLogEntry(LogEntry logEntry)
    {
        // 只记录当前级别或更高级别的日志
        if (logEntry.Level < CurrentLogLevel)
        {
            return;
        }

        lock (_lock)
        {
            _logEntries.Enqueue(logEntry);

            // 限制日志条目数量
            while (_logEntries.Count > MaxLogEntries)
            {
                _logEntries.Dequeue();
            }
        }

        // 输出到控制台
        Console.WriteLine(FormatLogEntry(logEntry));

        // 如果启用了文件日志，写入文件
        if (EnableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            try
            {
                File.AppendAllText(_logFilePath, FormatLogEntry(logEntry) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }

    private string FormatLogEntry(LogEntry entry)
    {
        var levelStr = entry.Level switch
        {
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRITICAL",
            _ => "UNKNOWN"
        };

        var formattedEntry = $"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{levelStr}] [{entry.Category}] {entry.Message}";

        if (!string.IsNullOrEmpty(entry.ExceptionInfo))
        {
            formattedEntry += $" - {entry.ExceptionInfo}";
        }

        return formattedEntry;
    }

    private void InitializeLogDirectory()
    {
        try
        {
            if (!string.IsNullOrEmpty(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
                
                var logFileName = $"BannerlordModEditor_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                _logFilePath = Path.Combine(LogDirectory, logFileName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize log directory: {ex.Message}");
            EnableFileLogging = false;
        }
    }

    partial void OnCurrentLogLevelChanged(LogLevel value)
    {
        LogInfo($"Log level changed to: {value}", "LogService");
    }

    partial void OnEnableFileLoggingChanged(bool value)
    {
        LogInfo($"File logging {(value ? "enabled" : "disabled")}", "LogService");
    }

    partial void OnLogDirectoryChanged(string value)
    {
        LogInfo($"Log directory changed to: {value}", "LogService");
        InitializeLogDirectory();
    }
}