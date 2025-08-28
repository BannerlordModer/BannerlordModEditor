using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI.Extensions;

/// <summary>
/// 依赖注入服务注册扩展方法
/// 
/// 这个类提供了扩展方法来简化EditorManager相关服务的注册，
/// 确保依赖注入配置的一致性和正确性。
/// 
/// 主要功能：
/// - 标准化的服务注册方法
/// - 支持不同配置选项
/// - 提供验证和错误处理
/// - 支持可选服务的注册
/// 
/// 使用方式：
/// <code>
/// // 在ConfigureServices方法中使用
/// services.AddEditorManagerServices();
/// 
/// // 或者使用自定义配置
/// services.AddEditorManagerServices(options =>
/// {
///     options.EnablePerformanceMonitoring = true;
///     options.EnableHealthChecks = true;
/// });
/// </code>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加EditorManager相关服务到依赖注入容器
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合，用于链式调用</returns>
    /// <remarks>
    /// 这个方法使用默认配置注册所有EditorManager相关的服务
    /// </remarks>
    public static IServiceCollection AddEditorManagerServices(this IServiceCollection services)
    {
        return services.AddEditorManagerServices(_ => { });
    }

    /// <summary>
    /// 添加EditorManager相关服务到依赖注入容器，并配置选项
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configureOptions">配置选项的委托</param>
    /// <returns>服务集合，用于链式调用</returns>
    /// <remarks>
    /// 这个方法允许通过委托自定义EditorManager服务的配置
    /// </remarks>
    public static IServiceCollection AddEditorManagerServices(
        this IServiceCollection services,
        Action<EditorManagerServiceOptions> configureOptions)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        // 创建并配置选项
        var options = new EditorManagerServiceOptions();
        configureOptions(options);

        // 验证选项
        options.Validate();

        // 注册核心服务
        RegisterCoreServices(services, options);

        // 注册编辑器工厂服务
        RegisterEditorFactoryServices(services, options);

        // 注册EditorManager相关服务
        RegisterEditorManagerServices(services, options);

        // 注册可选服务
        RegisterOptionalServices(services, options);

        return services;
    }

    /// <summary>
    /// 注册核心服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="options">服务选项</param>
    private static void RegisterCoreServices(IServiceCollection services, EditorManagerServiceOptions options)
    {
        // 注册日志服务
        if (options.UseLogService)
        {
            services.AddSingleton<ILogService, LogService>();
        }

        // 注册错误处理服务
        if (options.UseErrorHandlerService)
        {
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        }

        // 注册验证服务
        if (options.UseValidationService)
        {
            services.AddSingleton<IValidationService, ValidationService>();
        }

        // 注册数据绑定服务
        if (options.UseDataBindingService)
        {
            services.AddSingleton<IDataBindingService, DataBindingService>();
        }
    }

    /// <summary>
    /// 注册编辑器工厂服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="options">服务选项</param>
    private static void RegisterEditorFactoryServices(IServiceCollection services, EditorManagerServiceOptions options)
    {
        // 注册编辑器工厂
        if (options.UseEditorFactory)
        {
            services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
        }
    }

    /// <summary>
    /// 注册EditorManager相关服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="options">服务选项</param>
    private static void RegisterEditorManagerServices(IServiceCollection services, EditorManagerServiceOptions options)
    {
        // 注册EditorManagerFactory
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();

        // 注册EditorManagerViewModel为Transient（每次请求都创建新实例）
        services.AddTransient<EditorManagerViewModel>();
    }

    /// <summary>
    /// 注册可选服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="options">服务选项</param>
    private static void RegisterOptionalServices(IServiceCollection services, EditorManagerServiceOptions options)
    {
        // 注册性能监控服务（如果启用）
        if (options.EnablePerformanceMonitoring)
        {
            // TODO: 注册性能监控服务
            // services.AddSingleton<IPerformanceMonitor, PerformanceMonitor>();
        }

        // 注册健康检查服务（如果启用）
        if (options.EnableHealthChecks)
        {
            // TODO: 注册健康检查服务
            // services.AddSingleton<IHealthChecker, HealthChecker>();
        }

        // 注册诊断服务（如果启用）
        if (options.EnableDiagnostics)
        {
            // TODO: 注册诊断服务
            // services.AddSingleton<IDiagnosticService, DiagnosticService>();
        }
    }

    /// <summary>
    /// 添加最小化的EditorManager服务（仅包含必要服务）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合，用于链式调用</returns>
    /// <remarks>
    /// 这个方法只注册最基本的服务，适用于资源受限的环境
    /// </remarks>
    public static IServiceCollection AddMinimalEditorManagerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // 只注册最核心的服务
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>();

        return services;
    }

    /// <summary>
    /// 添加完整的EditorManager服务（包含所有可选服务）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合，用于链式调用</returns>
    /// <remarks>
    /// 这个方法注册所有可用的服务，适用于功能完整的环境
    /// </remarks>
    public static IServiceCollection AddFullEditorManagerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        return services.AddEditorManagerServices(options =>
        {
            options.UseLogService = true;
            options.UseErrorHandlerService = true;
            options.UseValidationService = true;
            options.UseDataBindingService = true;
            options.UseEditorFactory = true;
            options.EnablePerformanceMonitoring = true;
            options.EnableHealthChecks = true;
            options.EnableDiagnostics = true;
        });
    }

    /// <summary>
    /// 验证服务注册是否完整
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>验证结果</returns>
    /// <remarks>
    /// 这个方法可以用来验证所有必要的EditorManager服务是否已经正确注册
    /// </remarks>
    public static ServiceRegistrationValidationResult ValidateEditorManagerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        var result = new ServiceRegistrationValidationResult();

        // 检查核心服务
        result.IsLogServiceRegistered = services.Any(s => s.ServiceType == typeof(ILogService));
        result.IsErrorHandlerServiceRegistered = services.Any(s => s.ServiceType == typeof(IErrorHandlerService));
        result.IsValidationServiceRegistered = services.Any(s => s.ServiceType == typeof(IValidationService));
        result.IsDataBindingServiceRegistered = services.Any(s => s.ServiceType == typeof(IDataBindingService));
        result.IsEditorFactoryRegistered = services.Any(s => s.ServiceType == typeof(IEditorFactory));
        result.IsEditorManagerFactoryRegistered = services.Any(s => s.ServiceType == typeof(IEditorManagerFactory));
        result.IsEditorManagerViewModelRegistered = services.Any(s => s.ServiceType == typeof(EditorManagerViewModel));

        // 计算总体结果
        result.IsValid = result.IsLogServiceRegistered &&
                        result.IsErrorHandlerServiceRegistered &&
                        result.IsEditorManagerFactoryRegistered &&
                        result.IsEditorManagerViewModelRegistered;

        return result;
    }
}

/// <summary>
/// EditorManager服务配置选项
/// </summary>
public class EditorManagerServiceOptions
{
    /// <summary>
    /// 默认配置选项
    /// </summary>
    public static EditorManagerServiceOptions Default => new()
    {
        UseLogService = true,
        UseErrorHandlerService = true,
        UseValidationService = true,
        UseDataBindingService = true,
        UseEditorFactory = true,
        EnablePerformanceMonitoring = false,
        EnableHealthChecks = true,
        EnableDiagnostics = false
    };

    /// <summary>
    /// 是否使用日志服务
    /// </summary>
    public bool UseLogService { get; set; } = true;

    /// <summary>
    /// 是否使用错误处理服务
    /// </summary>
    public bool UseErrorHandlerService { get; set; } = true;

    /// <summary>
    /// 是否使用验证服务
    /// </summary>
    public bool UseValidationService { get; set; } = true;

    /// <summary>
    /// 是否使用数据绑定服务
    /// </summary>
    public bool UseDataBindingService { get; set; } = true;

    /// <summary>
    /// 是否使用编辑器工厂
    /// </summary>
    public bool UseEditorFactory { get; set; } = true;

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
    /// 验证配置选项
    /// </summary>
    /// <exception cref="InvalidOperationException">当配置无效时抛出</exception>
    public void Validate()
    {
        var errors = new List<string>();

        // 检查必要的配置
        if (!UseLogService)
            errors.Add("LogService is required for EditorManager to function properly");

        if (!UseErrorHandlerService)
            errors.Add("ErrorHandlerService is required for EditorManager to function properly");

        if (errors.Count > 0)
        {
            throw new InvalidOperationException($"Invalid EditorManagerServiceOptions configuration:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }
    }
}

/// <summary>
/// 服务注册验证结果
/// </summary>
public class ServiceRegistrationValidationResult
{
    /// <summary>
    /// 验证是否通过
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// 日志服务是否已注册
    /// </summary>
    public bool IsLogServiceRegistered { get; set; }

    /// <summary>
    /// 错误处理服务是否已注册
    /// </summary>
    public bool IsErrorHandlerServiceRegistered { get; set; }

    /// <summary>
    /// 验证服务是否已注册
    /// </summary>
    public bool IsValidationServiceRegistered { get; set; }

    /// <summary>
    /// 数据绑定服务是否已注册
    /// </summary>
    public bool IsDataBindingServiceRegistered { get; set; }

    /// <summary>
    /// 编辑器工厂是否已注册
    /// </summary>
    public bool IsEditorFactoryRegistered { get; set; }

    /// <summary>
    /// EditorManagerFactory是否已注册
    /// </summary>
    public bool IsEditorManagerFactoryRegistered { get; set; }

    /// <summary>
    /// EditorManagerViewModel是否已注册
    /// </summary>
    public bool IsEditorManagerViewModelRegistered { get; set; }

    /// <summary>
    /// 获取详细的验证信息
    /// </summary>
    /// <returns>验证信息字符串</returns>
    public override string ToString()
    {
        var lines = new List<string>
        {
            $"Validation Result: {(IsValid ? "PASSED" : "FAILED")}",
            $"- LogService: {(IsLogServiceRegistered ? "Registered" : "Missing")}",
            $"- ErrorHandlerService: {(IsErrorHandlerServiceRegistered ? "Registered" : "Missing")}",
            $"- ValidationService: {(IsValidationServiceRegistered ? "Registered" : "Missing")}",
            $"- DataBindingService: {(IsDataBindingServiceRegistered ? "Registered" : "Missing")}",
            $"- EditorFactory: {(IsEditorFactoryRegistered ? "Registered" : "Missing")}",
            $"- EditorManagerFactory: {(IsEditorManagerFactoryRegistered ? "Registered" : "Missing")}",
            $"- EditorManagerViewModel: {(IsEditorManagerViewModelRegistered ? "Registered" : "Missing")}"
        };

        return string.Join(Environment.NewLine, lines);
    }
}