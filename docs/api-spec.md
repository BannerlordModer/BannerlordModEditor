# EditorManagerViewModel 依赖注入解决方案 API 规范

## 概述

本文档定义了 EditorManagerViewModel 依赖注入歧义问题解决方案的 API 规范和接口契约。该规范确保了组件间的清晰交互和向后兼容性。

## 版本信息

- **API 版本**: 1.0.0
- **状态**: 设计中
- **最后更新**: 2025-08-28

## 核心接口定义

### 1. IEditorManagerFactory 接口

```csharp
namespace BannerlordModEditor.UI.Factories
{
    /// <summary>
    /// EditorManagerViewModel 工厂接口
    /// </summary>
    public interface IEditorManagerFactory
    {
        /// <summary>
        /// 创建 EditorManagerViewModel 实例（使用 Options 模式）
        /// </summary>
        /// <param name="options">配置选项，如果为 null 则使用默认配置</param>
        /// <returns>EditorManagerViewModel 实例</returns>
        /// <exception cref="EditorManagerCreationException">创建失败时抛出</exception>
        EditorManagerViewModel CreateEditorManager(EditorManagerOptions? options = null);

        /// <summary>
        /// 创建 EditorManagerViewModel 实例（使用依赖注入服务）
        /// </summary>
        /// <param name="editorFactory">编辑器工厂，可选</param>
        /// <param name="logService">日志服务，可选</param>
        /// <param name="errorHandlerService">错误处理服务，可选</param>
        /// <param name="validationService">验证服务，可选</param>
        /// <param name="serviceProvider">服务提供者，可选</param>
        /// <returns>EditorManagerViewModel 实例</returns>
        /// <exception cref="EditorManagerCreationException">创建失败时抛出</exception>
        /// <exception cref="ArgumentNullException">当必需参数为 null 时抛出</exception>
        EditorManagerViewModel CreateEditorManagerFromServices(
            IEditorFactory? editorFactory = null,
            ILogService? logService = null,
            IErrorHandlerService? errorHandlerService = null,
            IValidationService? validationService = null,
            IServiceProvider? serviceProvider = null);

        /// <summary>
        /// 创建默认配置的 EditorManagerViewModel
        /// </summary>
        /// <returns>使用默认配置的 EditorManagerViewModel 实例</returns>
        /// <exception cref="EditorManagerCreationException">创建失败时抛出</exception>
        EditorManagerViewModel CreateDefaultEditorManager();

        /// <summary>
        /// 验证工厂配置是否有效
        /// </summary>
        /// <returns>如果配置有效返回 true，否则返回 false</returns>
        bool ValidateConfiguration();

        /// <summary>
        /// 获取工厂配置信息
        /// </summary>
        /// <returns>工厂配置信息</returns>
        EditorManagerFactoryInfo GetFactoryInfo();
    }
}
```

### 2. EditorManagerOptions 类

```csharp
namespace BannerlordModEditor.UI.ViewModels
{
    /// <summary>
    /// EditorManagerViewModel 的配置选项
    /// </summary>
    public class EditorManagerOptions
    {
        /// <summary>
        /// 编辑器工厂
        /// </summary>
        public IEditorFactory? EditorFactory { get; set; }

        /// <summary>
        /// 日志服务
        /// </summary>
        public ILogService? LogService { get; set; }

        /// <summary>
        /// 错误处理服务
        /// </summary>
        public IErrorHandlerService? ErrorHandlerService { get; set; }

        /// <summary>
        /// 验证服务
        /// </summary>
        public IValidationService? ValidationService { get; set; }

        /// <summary>
        /// 服务提供者
        /// </summary>
        public IServiceProvider? ServiceProvider { get; set; }

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public bool EnableDebugMode { get; set; } = false;

        /// <summary>
        /// 是否启用性能监控
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = false;

        /// <summary>
        /// 创建默认配置
        /// </summary>
        public static EditorManagerOptions Default => new EditorManagerOptions();

        /// <summary>
        /// 创建用于依赖注入的配置
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>配置好的 EditorManagerOptions 实例</returns>
        /// <exception cref="ArgumentNullException">当 serviceProvider 为 null 时抛出</exception>
        public static EditorManagerOptions ForDependencyInjection(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            return new EditorManagerOptions
            {
                EditorFactory = serviceProvider.GetService<IEditorFactory>(),
                LogService = serviceProvider.GetService<ILogService>(),
                ErrorHandlerService = serviceProvider.GetService<IErrorHandlerService>(),
                ValidationService = serviceProvider.GetService<IValidationService>(),
                ServiceProvider = serviceProvider
            };
        }

        /// <summary>
        /// 创建用于测试的配置
        /// </summary>
        /// <param name="editorFactory">编辑器工厂</param>
        /// <param name="logService">日志服务</param>
        /// <param name="errorHandlerService">错误处理服务</param>
        /// <param name="validationService">验证服务</param>
        /// <returns>用于测试的配置</returns>
        public static EditorManagerOptions CreateForTesting(
            IEditorFactory? editorFactory = null,
            ILogService? logService = null,
            IErrorHandlerService? errorHandlerService = null,
            IValidationService? validationService = null)
        {
            return new EditorManagerOptions
            {
                EditorFactory = editorFactory,
                LogService = logService ?? new MockLogService(),
                ErrorHandlerService = errorHandlerService ?? new MockErrorHandlerService(),
                ValidationService = validationService ?? new MockValidationService(),
                EnableDebugMode = true
            };
        }

        /// <summary>
        /// 验证选项配置
        /// </summary>
        /// <exception cref="InvalidOperationException">当配置无效时抛出</exception>
        public void Validate()
        {
            var errors = new List<string>();

            if (LogService == null)
                errors.Add("LogService is required");
            
            if (ErrorHandlerService == null)
                errors.Add("ErrorHandlerService is required");
            
            if (ValidationService == null)
                errors.Add("ValidationService is required");

            if (errors.Count > 0)
                throw new InvalidOperationException($"Invalid EditorManagerOptions: {string.Join(", ", errors)}");
        }

        /// <summary>
        /// 克隆当前配置
        /// </summary>
        /// <returns>克隆的配置实例</returns>
        public EditorManagerOptions Clone()
        {
            return new EditorManagerOptions
            {
                EditorFactory = EditorFactory,
                LogService = LogService,
                ErrorHandlerService = ErrorHandlerService,
                ValidationService = ValidationService,
                ServiceProvider = ServiceProvider,
                EnableDebugMode = EnableDebugMode,
                EnablePerformanceMonitoring = EnablePerformanceMonitoring
            };
        }
    }
}
```

### 3. 服务注册扩展方法

```csharp
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务注册扩展方法
    /// </summary>
    public static class EditorManagerServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 EditorManager 相关服务（推荐方式）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        /// <exception cref="ArgumentNullException">当 services 为 null 时抛出</exception>
        public static IServiceCollection AddEditorManagerServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // 注册工厂
            services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
            
            // 注册 EditorManagerViewModel（通过工厂）
            services.AddTransient<EditorManagerViewModel>(sp => 
                sp.GetRequiredService<IEditorManagerFactory>().CreateEditorManager());
            
            return services;
        }

        /// <summary>
        /// 注册 EditorManager 相关服务（带自定义配置）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">配置选项的委托</param>
        /// <returns>服务集合</returns>
        /// <exception cref="ArgumentNullException">当 services 或 configureOptions 为 null 时抛出</exception>
        public static IServiceCollection AddEditorManagerServices(
            this IServiceCollection services,
            Action<EditorManagerOptions> configureOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            // 配置选项
            var options = new EditorManagerOptions();
            configureOptions(options);
            
            // 注册配置的选项
            services.AddSingleton(options);
            
            // 注册工厂
            services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
            
            // 注册 EditorManagerViewModel（使用配置的选项）
            services.AddTransient<EditorManagerViewModel>(sp => 
                sp.GetRequiredService<IEditorManagerFactory>().CreateEditorManager(
                    sp.GetRequiredService<EditorManagerOptions>()));
            
            return services;
        }

        /// <summary>
        /// 注册 EditorManager 相关服务（带预配置选项）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="options">预配置的选项</param>
        /// <returns>服务集合</returns>
        /// <exception cref="ArgumentNullException">当 services 或 options 为 null 时抛出</exception>
        public static IServiceCollection AddEditorManagerServices(
            this IServiceCollection services,
            EditorManagerOptions options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            // 注册配置的选项
            services.AddSingleton(options);
            
            // 注册工厂
            services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
            
            // 注册 EditorManagerViewModel（使用配置的选项）
            services.AddTransient<EditorManagerViewModel>(sp => 
                sp.GetRequiredService<IEditorManagerFactory>().CreateEditorManager(
                    sp.GetRequiredService<EditorManagerOptions>()));
            
            return services;
        }

        /// <summary>
        /// 注册 EditorManager 相关服务（带健康检查）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">配置选项的委托</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddEditorManagerServicesWithHealthChecks(
            this IServiceCollection services,
            Action<EditorManagerOptions>? configureOptions = null)
        {
            // 基础注册
            if (configureOptions != null)
            {
                services.AddEditorManagerServices(configureOptions);
            }
            else
            {
                services.AddEditorManagerServices();
            }
            
            // 注册健康检查
            services.AddHealthChecks()
                .AddCheck<EditorManagerHealthCheck>("editor_manager");
            
            return services;
        }
    }
}
```

### 4. 异常定义

```csharp
namespace BannerlordModEditor.UI.Exceptions
{
    /// <summary>
    /// EditorManager 创建异常
    /// </summary>
    public class EditorManagerCreationException : Exception
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// 创建失败的具体原因
        /// </summary>
        public EditorManagerCreationFailureReason FailureReason { get; }

        public EditorManagerCreationException(string message) 
            : base(message)
        {
            ErrorCode = "EM_001";
            FailureReason = EditorManagerCreationFailureReason.Unknown;
        }

        public EditorManagerCreationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            ErrorCode = "EM_002";
            FailureReason = EditorManagerCreationFailureReason.Unknown;
        }

        public EditorManagerCreationException(
            string message, 
            EditorManagerCreationFailureReason failureReason,
            Exception? innerException = null) 
            : base(message, innerException)
        {
            ErrorCode = GetErrorCodeForReason(failureReason);
            FailureReason = failureReason;
        }

        private static string GetErrorCodeForReason(EditorManagerCreationFailureReason reason)
        {
            return reason switch
            {
                EditorManagerCreationFailureReason.ServiceResolutionFailed => "EM_101",
                EditorManagerCreationFailureReason.InvalidOptions => "EM_102",
                EditorManagerCreationFailureReason.ConstructorException => "EM_103",
                EditorManagerCreationFailureReason.DependencyInjectionError => "EM_104",
                EditorManagerCreationFailureReason.ConfigurationError => "EM_105",
                _ => "EM_999"
            };
        }
    }

    /// <summary>
    /// EditorManager 创建失败原因
    /// </summary>
    public enum EditorManagerCreationFailureReason
    {
        /// <summary>
        /// 未知原因
        /// </summary>
        Unknown,

        /// <summary>
        /// 服务解析失败
        /// </summary>
        ServiceResolutionFailed,

        /// <summary>
        /// 无效的配置选项
        /// </summary>
        InvalidOptions,

        /// <summary>
        /// 构造函数异常
        /// </summary>
        ConstructorException,

        /// <summary>
        /// 依赖注入错误
        /// </summary>
        DependencyInjectionError,

        /// <summary>
        /// 配置错误
        /// </summary>
        ConfigurationError
    }
}
```

### 5. 健康检查接口

```csharp
namespace BannerlordModEditor.UI.HealthChecks
{
    /// <summary>
    /// EditorManager 健康检查
    /// </summary>
    public class EditorManagerHealthCheck : IHealthCheck
    {
        private readonly IEditorManagerFactory _factory;
        private readonly ILogger<EditorManagerHealthCheck> _logger;

        public EditorManagerHealthCheck(
            IEditorManagerFactory factory,
            ILogger<EditorManagerHealthCheck> logger)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Starting EditorManager health check");

                // 检查工厂配置
                if (!_factory.ValidateConfiguration())
                {
                    _logger.LogWarning("EditorManager factory configuration is invalid");
                    return HealthCheckResult.Unhealthy("Factory configuration is invalid");
                }

                // 检查创建实例
                var stopwatch = Stopwatch.StartNew();
                var editorManager = _factory.CreateDefaultEditorManager();
                stopwatch.Stop();

                if (editorManager == null)
                {
                    _logger.LogError("Failed to create EditorManager instance");
                    return HealthCheckResult.Unhealthy("Failed to create EditorManager instance");
                }

                // 检查基本属性
                if (editorManager.Categories == null)
                {
                    _logger.LogError("EditorManager categories are null");
                    return HealthCheckResult.Unhealthy("EditorManager categories are null");
                }

                var creationTime = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation("EditorManager health check completed successfully. Creation time: {CreationTime}ms", creationTime);

                var data = new Dictionary<string, object>
                {
                    { "creation_time_ms", creationTime },
                    { "categories_count", editorManager.Categories.Count },
                    { "factory_info", _factory.GetFactoryInfo() }
                };

                return creationTime > 1000 
                    ? HealthCheckResult.Degraded("EditorManager creation is slow", data: data)
                    : HealthCheckResult.Healthy("EditorManager is working correctly", data: data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EditorManager health check failed");
                return HealthCheckResult.Unhealthy("EditorManager health check failed", ex);
            }
        }
    }

    /// <summary>
    /// 工厂配置信息
    /// </summary>
    public class EditorManagerFactoryInfo
    {
        /// <summary>
        /// 工厂类型
        /// </summary>
        public string FactoryType { get; set; } = string.Empty;

        /// <summary>
        /// 是否配置有效
        /// </summary>
        public bool IsConfigured { get; set; }

        /// <summary>
        /// 支持的服务类型
        /// </summary>
        public List<string> SupportedServices { get; set; } = new();

        /// <summary>
        /// 配置时间
        /// </summary>
        public DateTime ConfiguredAt { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get; set; } = "1.0.0";
    }
}
```

### 6. 性能监控接口

```csharp
namespace BannerlordModEditor.UI.Monitoring
{
    /// <summary>
    /// EditorManager 性能监控器
    /// </summary>
    public interface IEditorManagerPerformanceMonitor
    {
        /// <summary>
        /// 记录创建时间
        /// </summary>
        /// <param name="creationTime">创建耗时</param>
        /// <param name="options">使用的配置选项</param>
        void RecordCreationTime(TimeSpan creationTime, EditorManagerOptions options);

        /// <summary>
        /// 记录内存使用情况
        /// </summary>
        /// <param name="memoryUsage">内存使用量</param>
        void RecordMemoryUsage(long memoryUsage);

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="context">异常上下文</param>
        void RecordException(Exception exception, string context);

        /// <summary>
        /// 获取性能统计信息
        /// </summary>
        /// <returns>性能统计信息</returns>
        EditorManagerPerformanceStats GetPerformanceStats();

        /// <summary>
        /// 重置统计信息
        /// </summary>
        void ResetStats();
    }

    /// <summary>
    /// EditorManager 性能统计信息
    /// </summary>
    public class EditorManagerPerformanceStats
    {
        /// <summary>
        /// 总创建次数
        /// </summary>
        public int TotalCreations { get; set; }

        /// <summary>
        /// 平均创建时间
        /// </summary>
        public double AverageCreationTime { get; set; }

        /// <summary>
        /// 最大创建时间
        /// </summary>
        public double MaxCreationTime { get; set; }

        /// <summary>
        /// 最小创建时间
        /// </summary>
        public double MinCreationTime { get; set; }

        /// <summary>
        /// 异常次数
        /// </summary>
        public int ExceptionCount { get; set; }

        /// <summary>
        /// 内存使用量
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
```

## 使用示例

### 1. 基本使用

```csharp
// 在 App.axaml.cs 中
private IServiceCollection ConfigureServices()
{
    var services = new ServiceCollection();
    
    // 注册基础服务
    services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
    services.AddSingleton<ILogService, LogService>();
    services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
    services.AddSingleton<IValidationService, ValidationService>();
    
    // 注册 EditorManager 服务
    services.AddEditorManagerServices();
    
    return services;
}
```

### 2. 自定义配置

```csharp
// 使用自定义配置
services.AddEditorManagerServices(options =>
{
    options.EnableDebugMode = true;
    options.EnablePerformanceMonitoring = true;
    options.LogService = new CustomLogService();
});
```

### 3. 手动使用工厂

```csharp
public class SomeService
{
    private readonly IEditorManagerFactory _factory;

    public SomeService(IEditorManagerFactory factory)
    {
        _factory = factory;
    }

    public void DoWork()
    {
        // 使用工厂创建 EditorManager
        var editorManager = _factory.CreateEditorManager();
        
        // 或者使用自定义选项
        var customOptions = new EditorManagerOptions
        {
            EnableDebugMode = true
        };
        var debugEditorManager = _factory.CreateEditorManager(customOptions);
    }
}
```

### 4. 错误处理

```csharp
try
{
    var editorManager = _factory.CreateEditorManager(options);
}
catch (EditorManagerCreationException ex)
{
    // 根据错误代码处理不同的情况
    switch (ex.ErrorCode)
    {
        case "EM_101":
            // 处理服务解析失败
            break;
        case "EM_102":
            // 处理无效配置
            break;
        default:
            // 处理其他错误
            break;
    }
}
```

## 版本兼容性

### 1. 向后兼容性
- 保持现有 EditorManagerViewModel 构造函数（标记为 Obsolete）
- 支持现有的依赖注入注册方式
- 保持公共 API 的稳定性

### 2. 版本迁移路径
- **版本 1.0.0**: 初始工厂模式实现
- **版本 1.1.0**: 添加健康检查和性能监控
- **版本 1.2.0**: 添加配置验证和错误处理改进
- **版本 2.0.0**: 移除废弃的构造函数（主要版本更新）

### 3. 弃用策略
- 标记废弃的 API 为 `[Obsolete]`
- 提供迁移指南
- 在文档中明确说明替代方案

## 安全考虑

### 1. 输入验证
- 所有公共方法都进行参数验证
- 配置选项在使用前进行验证
- 防止空引用和无效状态

### 2. 异常处理
- 提供详细的错误信息
- 记录异常上下文
- 避免敏感信息泄露

### 3. 资源管理
- 正确处理 IDisposable 资源
- 避免内存泄漏
- 支持异步操作

## 测试契约

### 1. 单元测试要求
- 所有公共方法都需要单元测试
- 异常路径需要专门的测试
- 边界条件需要测试覆盖

### 2. 集成测试要求
- 测试依赖注入容器集成
- 测试服务注册和解析
- 测试健康检查功能

### 3. 性能测试要求
- 测试创建时间性能
- 测试内存使用情况
- 测试并发创建能力

这个 API 规范提供了完整的接口定义和使用指南，确保了 EditorManagerViewModel 依赖注入问题的解决方案具有良好的可用性、可维护性和扩展性。