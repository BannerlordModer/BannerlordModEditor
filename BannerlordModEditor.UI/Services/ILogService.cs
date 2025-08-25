using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILogService
{
    /// <summary>
    /// 记录信息日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别</param>
    void LogInfo(string message, string category = "General");

    /// <summary>
    /// 记录警告日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别</param>
    void LogWarning(string message, string category = "General");

    /// <summary>
    /// 记录错误日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别</param>
    void LogError(string message, string category = "General");

    /// <summary>
    /// 记录异常日志
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">异常上下文</param>
    void LogException(Exception exception, string context = "");

    /// <summary>
    /// 记录调试日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别</param>
    void LogDebug(string message, string category = "General");

    /// <summary>
    /// 获取日志历史
    /// </summary>
    /// <param name="count">获取的日志数量</param>
    /// <returns>日志条目列表</returns>
    IEnumerable<LogEntry> GetLogHistory(int count = 100);

    /// <summary>
    /// 清除日志历史
    /// </summary>
    void ClearLogHistory();

    /// <summary>
    /// 保存日志到文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>保存是否成功</returns>
    Task<bool> SaveLogToFileAsync(string filePath);
}

/// <summary>
/// 日志级别
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// 调试
    /// </summary>
    Debug,

    /// <summary>
    /// 信息
    /// </summary>
    Info,

    /// <summary>
    /// 警告
    /// </summary>
    Warning,

    /// <summary>
    /// 错误
    /// </summary>
    Error,

    /// <summary>
    /// 严重错误
    /// </summary>
    Critical
}

/// <summary>
/// 日志条目
/// </summary>
public class LogEntry
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 日志类别
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 异常信息
    /// </summary>
    public string ExceptionInfo { get; set; } = string.Empty;

    /// <summary>
    /// 调用堆栈
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
}