using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 错误处理服务实现
/// 
/// 这个服务提供了统一的错误处理机制，包括：
/// - 异常捕获和处理
/// - 用户友好的错误消息显示
/// - 错误日志记录
/// - 错误历史管理
/// - 错误恢复建议
/// 
/// 使用方式：
/// <code>
/// var errorHandler = new ErrorHandlerService();
/// var result = await errorHandler.HandleExceptionAsync(ex, "Context");
/// </code>
/// </summary>
public partial class ErrorHandlerService : ObservableObject, IErrorHandlerService
{
    [ObservableProperty]
    private string currentMessage = string.Empty;

    [ObservableProperty]
    private string currentTitle = string.Empty;

    [ObservableProperty]
    private ErrorSeverity currentSeverity = ErrorSeverity.Info;

    [ObservableProperty]
    private bool isErrorVisible = false;

    private readonly Queue<ErrorLogEntry> _errorHistory = new Queue<ErrorLogEntry>();
    private const int MaxHistorySize = 100;

    public ErrorHandlerService()
    {
        // 初始化错误处理器
    }

    public async Task<ErrorResult> HandleExceptionAsync(Exception exception, string context = "")
    {
        var errorResult = new ErrorResult
        {
            IsHandled = false,
            Severity = ErrorSeverity.Error,
            CanRecover = false
        };

        try
        {
            // 根据异常类型设置错误严重程度
            errorResult.Severity = GetErrorSeverity(exception);
            
            // 生成用户友好的错误消息
            errorResult.UserMessage = GenerateUserMessage(exception);
            errorResult.TechnicalDetails = GenerateTechnicalDetails(exception);
            errorResult.RecoveryAction = GenerateRecoveryAction(exception);
            
            // 记录错误日志
            LogError(exception, context);
            
            // 显示错误消息
            ShowErrorMessage(errorResult.UserMessage, "错误", errorResult.Severity);
            
            errorResult.IsHandled = true;
        }
        catch (Exception ex)
        {
            // 如果错误处理本身出错，至少记录原始错误
            LogError(ex, "ErrorHandlerService.HandleExceptionAsync");
            errorResult.UserMessage = "处理错误时发生了一个异常。";
            errorResult.TechnicalDetails = ex.ToString();
        }

        return await Task.FromResult(errorResult);
    }

    public void ShowErrorMessage(string message, string title = "错误", ErrorSeverity severity = ErrorSeverity.Error)
    {
        CurrentMessage = message;
        CurrentTitle = title;
        CurrentSeverity = severity;
        IsErrorVisible = true;

        // 在实际应用中，这里会显示一个模态对话框
        // 现在只是更新状态供UI绑定
        OnPropertyChanged(nameof(CurrentMessage));
        OnPropertyChanged(nameof(CurrentTitle));
        OnPropertyChanged(nameof(CurrentSeverity));
        OnPropertyChanged(nameof(IsErrorVisible));
    }

    public void ShowWarningMessage(string message, string title = "警告")
    {
        ShowErrorMessage(message, title, ErrorSeverity.Warning);
    }

    public void ShowInfoMessage(string message, string title = "信息")
    {
        ShowErrorMessage(message, title, ErrorSeverity.Info);
    }

    public void LogError(Exception exception, string context = "")
    {
        var logEntry = new ErrorLogEntry
        {
            Timestamp = DateTime.Now,
            Message = exception.Message,
            Context = context,
            Severity = GetErrorSeverity(exception),
            ExceptionType = exception.GetType().Name,
            StackTrace = exception.StackTrace ?? string.Empty
        };

        _errorHistory.Enqueue(logEntry);

        // 限制历史记录大小
        while (_errorHistory.Count > MaxHistorySize)
        {
            _errorHistory.Dequeue();
        }

        // 在实际应用中，这里会写入文件日志系统
        Console.WriteLine($"[{logEntry.Timestamp}] {logEntry.Severity}: {logEntry.Message} (Context: {logEntry.Context})");
    }

    public void LogInfo(string message, string category = "General")
    {
        var logEntry = new ErrorLogEntry
        {
            Timestamp = DateTime.Now,
            Message = message,
            Context = category,
            Severity = ErrorSeverity.Info,
            ExceptionType = "Info",
            StackTrace = string.Empty
        };

        _errorHistory.Enqueue(logEntry);

        // 限制历史记录大小
        while (_errorHistory.Count > MaxHistorySize)
        {
            _errorHistory.Dequeue();
        }

        Console.WriteLine($"[{logEntry.Timestamp}] Info: {logEntry.Message} (Category: {logEntry.Context})");
    }

    public IEnumerable<ErrorLogEntry> GetErrorHistory()
    {
        return _errorHistory.ToList();
    }

    private ErrorSeverity GetErrorSeverity(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => ErrorSeverity.Error,
            ArgumentException => ErrorSeverity.Error,
            InvalidOperationException => ErrorSeverity.Error,
            NotImplementedException => ErrorSeverity.Warning,
            NotSupportedException => ErrorSeverity.Warning,
            System.IO.IOException => ErrorSeverity.Error,
            System.Xml.XmlException => ErrorSeverity.Error,
            System.UnauthorizedAccessException => ErrorSeverity.Critical,
            System.Security.SecurityException => ErrorSeverity.Critical,
            _ => ErrorSeverity.Error
        };
    }

    private string GenerateUserMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "缺少必需的参数。",
            ArgumentException => "提供的参数无效。",
            InvalidOperationException => "操作无效或无法在当前状态下执行。",
            NotImplementedException => "此功能尚未实现。",
            NotSupportedException => "不支持此操作。",
            System.IO.FileNotFoundException => "找不到指定的文件。",
            System.IO.DirectoryNotFoundException => "找不到指定的目录。",
            System.IO.IOException => "文件操作失败。",
            System.Xml.XmlException => "XML解析错误。",
            System.UnauthorizedAccessException => "访问被拒绝。",
            System.Security.SecurityException => "安全验证失败。",
            _ => "发生了一个未知错误。"
        };
    }

    private string GenerateTechnicalDetails(Exception exception)
    {
        return $"异常类型: {exception.GetType().Name}\n" +
               $"消息: {exception.Message}\n" +
               $"堆栈跟踪: {exception.StackTrace}";
    }

    private string GenerateRecoveryAction(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "请检查所有必需的参数是否已提供。",
            ArgumentException => "请检查参数值是否正确。",
            InvalidOperationException => "请确保应用程序处于正确的状态。",
            System.IO.FileNotFoundException => "请检查文件路径是否正确。",
            System.IO.DirectoryNotFoundException => "请检查目录路径是否正确。",
            System.IO.IOException => "请检查文件是否被其他程序占用。",
            System.Xml.XmlException => "请检查XML文件格式是否正确。",
            System.UnauthorizedAccessException => "请检查文件权限。",
            System.Security.SecurityException => "请检查用户权限。",
            _ => "请重试操作。如果问题持续存在，请联系技术支持。"
        };
    }
}