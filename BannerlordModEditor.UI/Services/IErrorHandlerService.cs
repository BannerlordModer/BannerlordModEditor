using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 错误处理服务接口
/// </summary>
public interface IErrorHandlerService
{
    /// <summary>
    /// 处理异常
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">错误上下文</param>
    /// <returns>处理结果</returns>
    Task<ErrorResult> HandleExceptionAsync(Exception exception, string context = "");

    /// <summary>
    /// 处理异常（同步版本）
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">错误上下文</param>
    void HandleError(Exception exception, string context = "");

    /// <summary>
    /// 显示错误消息
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="title">错误标题</param>
    /// <param name="severity">错误严重程度</param>
    void ShowErrorMessage(string message, string title = "错误", ErrorSeverity severity = ErrorSeverity.Error);

    /// <summary>
    /// 显示警告消息
    /// </summary>
    /// <param name="message">警告消息</param>
    /// <param name="title">警告标题</param>
    void ShowWarningMessage(string message, string title = "警告");

    /// <summary>
    /// 显示信息消息
    /// </summary>
    /// <param name="message">信息消息</param>
    /// <param name="title">信息标题</param>
    void ShowInfoMessage(string message, string title = "信息");

    /// <summary>
    /// 记录错误日志
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">错误上下文</param>
    void LogError(Exception exception, string context = "");

    /// <summary>
    /// 记录信息日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别</param>
    void LogInfo(string message, string category = "General");

    /// <summary>
    /// 获取最近的错误历史
    /// </summary>
    /// <returns>错误历史记录</returns>
    IEnumerable<ErrorLogEntry> GetErrorHistory();
}

/// <summary>
/// 错误严重程度
/// </summary>
public enum ErrorSeverity
{
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
/// 错误处理结果
/// </summary>
public class ErrorResult
{
    /// <summary>
    /// 是否处理成功
    /// </summary>
    public bool IsHandled { get; set; }

    /// <summary>
    /// 用户友好的错误消息
    /// </summary>
    public string UserMessage { get; set; } = string.Empty;

    /// <summary>
    /// 技术错误详情
    /// </summary>
    public string TechnicalDetails { get; set; } = string.Empty;

    /// <summary>
    /// 错误严重程度
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// 建议的恢复操作
    /// </summary>
    public string RecoveryAction { get; set; } = string.Empty;

    /// <summary>
    /// 是否可以恢复
    /// </summary>
    public bool CanRecover { get; set; }
}

/// <summary>
/// 错误日志条目
/// </summary>
public class ErrorLogEntry
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 错误上下文
    /// </summary>
    public string Context { get; set; } = string.Empty;

    /// <summary>
    /// 错误严重程度
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// 异常类型
    /// </summary>
    public string ExceptionType { get; set; } = string.Empty;

    /// <summary>
    /// 堆栈跟踪
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
}