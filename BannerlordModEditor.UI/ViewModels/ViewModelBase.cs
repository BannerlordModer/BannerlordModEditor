using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.Services;
using System;
using System.Threading.Tasks;
using System.Reflection;
using BannerlordModEditor.UI.Factories;

namespace BannerlordModEditor.UI.ViewModels;

/// <summary>
/// 基础视图模型类，提供错误处理和日志记录功能
/// 
/// 这个基类为所有视图模型提供了统一的功能：
/// - 异常处理和错误恢复
/// - 日志记录
/// - 用户友好的错误消息显示
/// - 安全执行操作的方法
/// 
/// 使用方式：
/// <code>
/// public class MyViewModel : ViewModelBase
/// {
///     public MyViewModel() : base()
///     {
///         LogInfo("ViewModel initialized", "MyViewModel");
///     }
///     
///     public async Task DoWorkAsync()
///     {
///         await ExecuteSafelyAsync(async () =>
///         {
///             // 危险的操作
///         }, "MyViewModel.DoWorkAsync");
///     }
/// }
/// </code>
/// </summary>
public class ViewModelBase : ObservableObject
{
    public IErrorHandlerService ErrorHandler { get; }
    public ILogService LogService { get; }

    public ViewModelBase(
        IErrorHandlerService? errorHandler = null,
        ILogService? logService = null)
    {
        ErrorHandler = errorHandler ?? new ErrorHandlerService();
        LogService = logService ?? new LogService();
    }

    /// <summary>
    /// 获取编辑器类型（从EditorTypeAttribute获取）
    /// </summary>
    public string EditorType
    {
        get
        {
            var editorTypeAttribute = GetType().GetCustomAttribute<EditorTypeAttribute>();
            return editorTypeAttribute?.EditorType ?? GetType().Name;
        }
    }

    /// <summary>
    /// 获取XML文件名（从EditorTypeAttribute获取）
    /// </summary>
    public string XmlFileName
    {
        get
        {
            var editorTypeAttribute = GetType().GetCustomAttribute<EditorTypeAttribute>();
            return editorTypeAttribute?.XmlFileName ?? $"{GetType().Name}.xml";
        }
    }

    /// <summary>
    /// 安全执行异步操作，自动处理异常
    /// </summary>
    /// <param name="action">要执行的异步操作</param>
    /// <param name="context">操作上下文，用于日志记录</param>
    /// <returns>如果执行成功返回true，否则返回false</returns>
    /// <remarks>
    /// 这个方法会自动捕获异常、记录日志并显示用户友好的错误消息
    /// </remarks>
    protected async Task<bool> ExecuteSafelyAsync(Func<Task> action, string context = "")
    {
        try
        {
            await action();
            return true;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex, context);
            return false;
        }
    }

    /// <summary>
    /// 安全执行同步操作，自动处理异常
    /// </summary>
    /// <param name="action">要执行的同步操作</param>
    /// <param name="context">操作上下文，用于日志记录</param>
    /// <returns>如果执行成功返回true，否则返回false</returns>
    /// <remarks>
    /// 这个方法会自动捕获异常、记录日志并显示用户友好的错误消息
    /// </remarks>
    protected bool ExecuteSafely(Action action, string context = "")
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            HandleError(ex, context);
            return false;
        }
    }

    /// <summary>
    /// 异步处理异常
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">异常上下文</param>
    /// <returns>异步任务</returns>
    /// <remarks>
    /// 这个方法会记录异常日志并调用错误处理器
    /// </remarks>
    protected async Task HandleErrorAsync(Exception exception, string context = "")
    {
        LogService.LogException(exception, context);
        await ErrorHandler.HandleExceptionAsync(exception, context);
    }

    /// <summary>
    /// 同步处理异常
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="context">异常上下文</param>
    /// <remarks>
    /// 这个方法会记录异常日志并显示错误消息
    /// </remarks>
    protected void HandleError(Exception exception, string context = "")
    {
        LogService.LogException(exception, context);
        ErrorHandler.ShowErrorMessage(exception.Message, "错误");
    }

    /// <summary>
    /// 记录信息日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别，默认为"General"</param>
    protected void LogInfo(string message, string category = "General")
    {
        LogService.LogInfo(message, category);
    }

    /// <summary>
    /// 记录警告日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别，默认为"General"</param>
    protected void LogWarning(string message, string category = "General")
    {
        LogService.LogWarning(message, category);
    }

    /// <summary>
    /// 记录错误日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志类别，默认为"General"</param>
    protected void LogError(string message, string category = "General")
    {
        LogService.LogError(message, category);
    }

    /// <summary>
    /// 显示用户友好的错误消息
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="title">错误标题，默认为"错误"</param>
    protected void ShowError(string message, string title = "错误")
    {
        ErrorHandler.ShowErrorMessage(message, title);
    }

    /// <summary>
    /// 显示警告消息
    /// </summary>
    /// <param name="message">警告消息</param>
    /// <param name="title">警告标题，默认为"警告"</param>
    protected void ShowWarning(string message, string title = "警告")
    {
        ErrorHandler.ShowWarningMessage(message, title);
    }

    /// <summary>
    /// 显示信息消息
    /// </summary>
    /// <param name="message">信息消息</param>
    /// <param name="title">信息标题，默认为"信息"</param>
    protected void ShowInfo(string message, string title = "信息")
    {
        ErrorHandler.ShowInfoMessage(message, title);
    }
}