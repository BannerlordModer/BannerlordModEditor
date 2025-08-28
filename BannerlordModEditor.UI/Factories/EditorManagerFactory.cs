using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// EditorManagerViewModel工厂类，负责创建和配置EditorManagerViewModel实例
/// 
/// 这个工厂类解决了依赖注入歧义问题，通过明确的创建方法和配置选项
/// 来确保EditorManagerViewModel的正确初始化。
/// 
/// 主要功能：
/// - 提供标准化的EditorManagerViewModel创建方法
/// - 支持依赖注入容器的服务解析
/// - 提供性能监控和健康检查
/// - 确保线程安全的实例创建
/// 
/// 使用方式：
/// <code>
/// // 通过依赖注入获取工厂实例
/// var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
/// 
/// // 创建标准的EditorManagerViewModel
/// var editorManager = factory.CreateEditorManager();
/// 
/// // 使用自定义配置创建
/// var options = new EditorManagerCreationOptions
/// {
///     EnablePerformanceMonitoring = true,
///     EnableHealthChecks = true
/// };
/// var customEditorManager = factory.CreateEditorManager(options);
/// </code>
/// </summary>
public class EditorManagerFactory : IEditorManagerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;
    private readonly IEditorFactory _editorFactory;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;

    private readonly SemaphoreSlim _creationLock = new SemaphoreSlim(1, 1);
    private int _instancesCreated = 0;
    private DateTime _lastCreationTime = DateTime.MinValue;

    /// <summary>
    /// 初始化EditorManagerFactory
    /// </summary>
    /// <param name="serviceProvider">依赖注入服务提供器</param>
    /// <param name="logService">日志服务</param>
    /// <param name="errorHandlerService">错误处理服务</param>
    /// <param name="editorFactory">编辑器工厂</param>
    /// <param name="validationService">验证服务</param>
    /// <param name="dataBindingService">数据绑定服务</param>
    /// <exception cref="ArgumentNullException">当必要参数为null时抛出</exception>
    public EditorManagerFactory(
        IServiceProvider serviceProvider,
        ILogService logService,
        IErrorHandlerService errorHandlerService,
        IEditorFactory editorFactory,
        IValidationService validationService,
        IDataBindingService dataBindingService)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _errorHandlerService = errorHandlerService ?? throw new ArgumentNullException(nameof(errorHandlerService));
        _editorFactory = editorFactory ?? throw new ArgumentNullException(nameof(editorFactory));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _dataBindingService = dataBindingService ?? throw new ArgumentNullException(nameof(dataBindingService));

        _logService.LogInfo("EditorManagerFactory initialized", "EditorManagerFactory");
    }

    /// <summary>
    /// 创建标准的EditorManagerViewModel实例
    /// </summary>
    /// <returns>配置完成的EditorManagerViewModel实例</returns>
    /// <remarks>
    /// 这个方法使用默认配置创建EditorManagerViewModel，适用于大多数场景
    /// </remarks>
    public EditorManagerViewModel CreateEditorManager()
    {
        return CreateEditorManager(EditorManagerCreationOptions.Default);
    }

    /// <summary>
    /// 使用自定义选项创建EditorManagerViewModel实例
    /// </summary>
    /// <param name="options">创建选项</param>
    /// <returns>配置完成的EditorManagerViewModel实例</returns>
    /// <exception cref="ArgumentNullException">当options为null时抛出</exception>
    /// <exception cref="EditorManagerCreationException">当创建过程中出现错误时抛出</exception>
    public EditorManagerViewModel CreateEditorManager(EditorManagerCreationOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        return ExecuteSafely(() =>
        {
            var startTime = DateTime.UtcNow;
            _logService.LogDebug("Starting EditorManagerViewModel creation", "EditorManagerFactory");

            // 使用信号量确保线程安全的创建过程
            _creationLock.Wait();

            try
            {
                // 验证服务健康状态
                if (options.EnableHealthChecks)
                {
                    ValidateServiceHealth();
                }

                // 创建EditorManagerOptions
                var editorManagerOptions = CreateEditorManagerOptions(options);

                // 创建EditorManagerViewModel实例
                var editorManager = new EditorManagerViewModel(editorManagerOptions);

                // 执行创建后配置
                if (options.EnablePostCreationConfiguration)
                {
                    ConfigureEditorManager(editorManager, options);
                }

                // 更新性能指标
                UpdateCreationMetrics(DateTime.UtcNow - startTime);

                _logService.LogInfo($"EditorManagerViewModel created successfully (Instance #{_instancesCreated})", "EditorManagerFactory");

                return editorManager;
            }
            finally
            {
                _creationLock.Release();
            }
        }, "CreateEditorManager");
    }

    /// <summary>
    /// 异步创建EditorManagerViewModel实例
    /// </summary>
    /// <param name="options">创建选项（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>异步创建的EditorManagerViewModel实例</returns>
    /// <remarks>
    /// 这个方法适用于需要异步初始化的场景，比如从配置文件加载设置
    /// </remarks>
    public async Task<EditorManagerViewModel> CreateEditorManagerAsync(
        EditorManagerCreationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var creationOptions = options ?? EditorManagerCreationOptions.Default;
        
        return await ExecuteSafelyAsync(async () =>
        {
            var startTime = DateTime.UtcNow;
            _logService.LogDebug("Starting async EditorManagerViewModel creation", "EditorManagerFactory");

            // 使用信号量确保线程安全的创建过程
            await _creationLock.WaitAsync(cancellationToken);

            try
            {
                // 验证服务健康状态
                if (creationOptions.EnableHealthChecks)
                {
                    await ValidateServiceHealthAsync(cancellationToken);
                }

                // 异步创建EditorManagerOptions
                var editorManagerOptions = await CreateEditorManagerOptionsAsync(creationOptions, cancellationToken);

                // 创建EditorManagerViewModel实例
                var editorManager = new EditorManagerViewModel(editorManagerOptions);

                // 执行异步创建后配置
                if (creationOptions.EnablePostCreationConfiguration)
                {
                    await ConfigureEditorManagerAsync(editorManager, creationOptions, cancellationToken);
                }

                // 更新性能指标
                UpdateCreationMetrics(DateTime.UtcNow - startTime);

                _logService.LogInfo($"EditorManagerViewModel created asynchronously successfully (Instance #{_instancesCreated})", "EditorManagerFactory");

                return editorManager;
            }
            finally
            {
                _creationLock.Release();
            }
        }, "CreateEditorManagerAsync");
    }

    /// <summary>
    /// 验证所有依赖服务的健康状态
    /// </summary>
    /// <exception cref="EditorManagerCreationException">当服务健康检查失败时抛出</exception>
    private void ValidateServiceHealth()
    {
        _logService.LogDebug("Validating service health", "EditorManagerFactory");

        try
        {
            // 验证日志服务
            _logService.LogDebug("Testing log service", "EditorManagerFactory.HealthCheck");

            // 验证编辑器工厂
            var registeredTypes = _editorFactory.GetRegisteredEditorTypes();
            _logService.LogDebug($"Editor factory has {registeredTypes.Count()} registered types", "EditorManagerFactory.HealthCheck");

            // 验证验证服务
            var testValidation = _validationService.Validate(new object());
            _logService.LogDebug($"Validation service test result: {testValidation.IsValid}", "EditorManagerFactory.HealthCheck");

            _logService.LogInfo("All services passed health check", "EditorManagerFactory");
        }
        catch (Exception ex)
        {
            var errorEx = new EditorManagerCreationException("Service health check failed", ex);
            _logService.LogException(errorEx, "Service health check failed");
            throw errorEx;
        }
    }

    /// <summary>
    /// 异步验证所有依赖服务的健康状态
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="EditorManagerCreationException">当服务健康检查失败时抛出</exception>
    private async Task ValidateServiceHealthAsync(CancellationToken cancellationToken)
    {
        _logService.LogDebug("Validating service health asynchronously", "EditorManagerFactory");

        try
        {
            // 验证日志服务
            _logService.LogDebug("Testing log service", "EditorManagerFactory.HealthCheck");

            // 验证编辑器工厂
            var registeredTypes = _editorFactory.GetRegisteredEditorTypes();
            _logService.LogDebug($"Editor factory has {registeredTypes.Count()} registered types", "EditorManagerFactory.HealthCheck");

            // 验证验证服务
            var testValidation = _validationService.Validate(new object());
            _logService.LogDebug($"Validation service test result: {testValidation.IsValid}", "EditorManagerFactory.HealthCheck");

            // 模拟异步操作（实际项目中可能有真正的异步健康检查）
            await Task.Delay(1, cancellationToken);

            _logService.LogInfo("All services passed async health check", "EditorManagerFactory");
        }
        catch (Exception ex)
        {
            var errorEx = new EditorManagerCreationException("Async service health check failed", ex);
            _logService.LogException(errorEx, "Async service health check failed");
            throw errorEx;
        }
    }

    /// <summary>
    /// 创建EditorManagerOptions
    /// </summary>
    /// <param name="options">创建选项</param>
    /// <returns>配置完成的EditorManagerOptions</returns>
    private EditorManagerOptions CreateEditorManagerOptions(EditorManagerCreationOptions options)
    {
        return new EditorManagerOptions
        {
            EditorFactory = _editorFactory,
            LogService = _logService,
            ErrorHandlerService = _errorHandlerService,
            ValidationService = _validationService,
            ServiceProvider = _serviceProvider,
            EnablePerformanceMonitoring = options.EnablePerformanceMonitoring,
            EnableHealthChecks = options.EnableHealthChecks,
            EnableDiagnostics = options.EnableDiagnostics,
            CreationTimeout = options.CreationTimeout
        };
    }

    /// <summary>
    /// 异步创建EditorManagerOptions
    /// </summary>
    /// <param name="options">创建选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>异步创建的EditorManagerOptions</returns>
    private async Task<EditorManagerOptions> CreateEditorManagerOptionsAsync(
        EditorManagerCreationOptions options,
        CancellationToken cancellationToken)
    {
        // 模拟异步操作（实际项目中可能需要从配置文件加载设置）
        await Task.Delay(1, cancellationToken);

        return new EditorManagerOptions
        {
            EditorFactory = _editorFactory,
            LogService = _logService,
            ErrorHandlerService = _errorHandlerService,
            ValidationService = _validationService,
            ServiceProvider = _serviceProvider,
            EnablePerformanceMonitoring = options.EnablePerformanceMonitoring,
            EnableHealthChecks = options.EnableHealthChecks,
            EnableDiagnostics = options.EnableDiagnostics,
            CreationTimeout = options.CreationTimeout
        };
    }

    /// <summary>
    /// 配置EditorManagerViewModel实例
    /// </summary>
    /// <param name="editorManager">要配置的EditorManagerViewModel实例</param>
    /// <param name="options">创建选项</param>
    private void ConfigureEditorManager(EditorManagerViewModel editorManager, EditorManagerCreationOptions options)
    {
        try
        {
            _logService.LogDebug("Configuring EditorManagerViewModel", "EditorManagerFactory");

            // 如果启用了性能监控，可以在这里添加性能计数器
            if (options.EnablePerformanceMonitoring)
            {
                // TODO: 添加性能监控配置
                _logService.LogDebug("Performance monitoring enabled", "EditorManagerFactory");
            }

            // 如果启用了诊断，可以在这里添加诊断配置
            if (options.EnableDiagnostics)
            {
                // TODO: 添加诊断配置
                _logService.LogDebug("Diagnostics enabled", "EditorManagerFactory");
            }

            _logService.LogInfo("EditorManagerViewModel configured successfully", "EditorManagerFactory");
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Failed to configure EditorManagerViewModel");
            // 不抛出异常，因为配置失败不应该阻止创建过程
        }
    }

    /// <summary>
    /// 异步配置EditorManagerViewModel实例
    /// </summary>
    /// <param name="editorManager">要配置的EditorManagerViewModel实例</param>
    /// <param name="options">创建选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    private async Task ConfigureEditorManagerAsync(
        EditorManagerViewModel editorManager,
        EditorManagerCreationOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            _logService.LogDebug("Configuring EditorManagerViewModel asynchronously", "EditorManagerFactory");

            // 模拟异步配置操作
            await Task.Delay(1, cancellationToken);

            // 如果启用了性能监控，可以在这里添加性能计数器
            if (options.EnablePerformanceMonitoring)
            {
                // TODO: 添加性能监控配置
                _logService.LogDebug("Performance monitoring enabled", "EditorManagerFactory");
            }

            // 如果启用了诊断，可以在这里添加诊断配置
            if (options.EnableDiagnostics)
            {
                // TODO: 添加诊断配置
                _logService.LogDebug("Diagnostics enabled", "EditorManagerFactory");
            }

            _logService.LogInfo("EditorManagerViewModel configured asynchronously successfully", "EditorManagerFactory");
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Failed to configure EditorManagerViewModel asynchronously");
            // 不抛出异常，因为配置失败不应该阻止创建过程
        }
    }

    /// <summary>
    /// 更新创建性能指标
    /// </summary>
    /// <param name="creationTime">创建耗时</param>
    private void UpdateCreationMetrics(TimeSpan creationTime)
    {
        Interlocked.Increment(ref _instancesCreated);
        _lastCreationTime = DateTime.UtcNow;

        if (creationTime.TotalMilliseconds > 1000)
        {
            _logService.LogWarning($"EditorManagerViewModel creation took {creationTime.TotalMilliseconds:F2}ms", "EditorManagerFactory.Performance");
        }
        else
        {
            _logService.LogDebug($"EditorManagerViewModel creation took {creationTime.TotalMilliseconds:F2}ms", "EditorManagerFactory.Performance");
        }
    }

    /// <summary>
    /// 安全执行操作
    /// </summary>
    /// <param name="action">要执行的操作</param>
    /// <param name="context">操作上下文</param>
    /// <returns>操作结果</returns>
    private T ExecuteSafely<T>(Func<T> action, string context)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            var errorEx = new EditorManagerCreationException($"Failed to {context}", ex);
            _logService.LogException(errorEx, $"Failed to {context}");
            throw errorEx;
        }
    }

    /// <summary>
    /// 安全执行异步操作
    /// </summary>
    /// <param name="action">要执行的异步操作</param>
    /// <param name="context">操作上下文</param>
    /// <returns>异步操作结果</returns>
    private async Task<T> ExecuteSafelyAsync<T>(Func<Task<T>> action, string context)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var errorEx = new EditorManagerCreationException($"Failed to {context}", ex);
            _logService.LogException(errorEx, $"Failed to {context}");
            throw errorEx;
        }
    }

    /// <summary>
    /// 获取工厂统计信息
    /// </summary>
    /// <returns>工厂统计信息</returns>
    public EditorManagerFactoryStatistics GetStatistics()
    {
        return new EditorManagerFactoryStatistics
        {
            InstancesCreated = _instancesCreated,
            LastCreationTime = _lastCreationTime,
            IsCreationInProgress = _creationLock.CurrentCount == 0
        };
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        _creationLock.Dispose();
        _logService.LogInfo("EditorManagerFactory disposed", "EditorManagerFactory");
    }
}

/// <summary>
/// EditorManagerViewModel创建选项
/// </summary>
public class EditorManagerCreationOptions
{
    /// <summary>
    /// 默认创建选项
    /// </summary>
    public static EditorManagerCreationOptions Default => new();

    /// <summary>
    /// 是否启用性能监控
    /// </summary>
    public bool EnablePerformanceMonitoring { get; set; } = false;

    /// <summary>
    /// 是否启用健康检查
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// 是否启用诊断
    /// </summary>
    public bool EnableDiagnostics { get; set; } = false;

    /// <summary>
    /// 是否启用创建后配置
    /// </summary>
    public bool EnablePostCreationConfiguration { get; set; } = true;

    /// <summary>
    /// 创建超时时间（毫秒）
    /// </summary>
    public int CreationTimeout { get; set; } = 30000;
}

/// <summary>
/// EditorManagerFactory统计信息
/// </summary>
public class EditorManagerFactoryStatistics
{
    /// <summary>
    /// 已创建的实例数量
    /// </summary>
    public int InstancesCreated { get; set; }

    /// <summary>
    /// 最后创建时间
    /// </summary>
    public DateTime LastCreationTime { get; set; }

    /// <summary>
    /// 是否正在创建实例
    /// </summary>
    public bool IsCreationInProgress { get; set; }
}

/// <summary>
/// EditorManager创建异常
/// </summary>
public class EditorManagerCreationException : Exception
{
    public EditorManagerCreationException(string message) : base(message) { }

    public EditorManagerCreationException(string message, Exception innerException) : base(message, innerException) { }
}