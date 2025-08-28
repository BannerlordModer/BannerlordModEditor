# EditorManagerViewModel 依赖注入解决方案技术栈

## 技术栈概览

本文档详细描述了解决 EditorManagerViewModel 依赖注入歧义问题的技术栈选择和实现方案。该方案基于现有的 .NET 9 和 Avalonia UI 架构，采用工厂模式来解决依赖注入歧义问题。

## 核心技术栈

### 前端技术栈
| 技术 | 版本 | 选择理由 | 用途 |
|------|------|----------|------|
| **.NET 9** | 9.0.100 | 最新 LTS 版本，性能优化 | 运行时和基础框架 |
| **Avalonia UI** | 11.3.0 | 跨平台桌面 UI 框架 | 用户界面 |
| **CommunityToolkit.Mvvm** | 8.2.2 | 官方 MVVM 工具包 | MVVM 模式支持 |
| **Microsoft.Extensions.DependencyInjection** | 9.0.0 | 官方依赖注入容器 | 服务注册和解析 |

### 后端服务技术栈
| 技术 | 版本 | 选择理由 | 用途 |
|------|------|----------|------|
| **Microsoft.Extensions.Logging** | 9.0.0 | 官方日志框架 | 日志记录 |
| **System.Text.Json** | 9.0.0 | 高性能 JSON 处理 | 配置序列化 |
| **System.Reactive** | 6.0.0 | 响应式编程支持 | 事件处理 |

### 测试技术栈
| 技术 | 版本 | 选择理由 | 用途 |
|------|------|----------|------|
| **xUnit** | 2.9.2 | 现代单元测试框架 | 单元测试 |
| **Moq** | 4.20.70 | 模拟框架 | 模拟依赖 |
| **FluentAssertions** | 6.12.1 | 断言库 | 测试断言 |
| **coverlet.collector** | 6.0.2 | 代码覆盖率 | 测试覆盖率 |

## 架构决策记录 (ADR)

### ADR-001: 工厂模式选择

**状态**: 已接受
**上下文**: 需要解决 EditorManagerViewModel 的依赖注入歧义问题

**决策**: 采用工厂模式来解决依赖注入歧义

**后果**: 
- ✅ 消除了构造函数歧义
- ✅ 提供了清晰的创建路径
- ✅ 支持多种创建策略
- ❌ 增加了少量代码复杂度

**替代方案考虑**:
1. **构造函数注入标记**: 使用 [ActivatorUtilitiesConstructor] 特性
   - 优点: 简单直接
   - 缺点: 不够灵活，难以扩展

2. **属性注入**: 移除构造函数，使用属性注入
   - 优点: 避免歧义
   - 缺点: 违反依赖注入最佳实践

3. **多个接口**: 为每种创建方式定义不同接口
   - 优点: 类型安全
   - 缺点: 接口爆炸，维护困难

### ADR-002: 生命周期选择

**状态**: 已接受
**上下文**: 需要确定工厂和 ViewModel 的生命周期

**决策**: 
- **EditorManagerFactory**: Singleton
- **EditorManagerViewModel**: Transient

**理由**:
- Factory 无状态，可以共享
- ViewModel 有状态，每次创建新实例

**性能影响**:
- Factory 创建开销: 低
- ViewModel 创建开销: 中等
- 内存使用: 适中

### ADR-003: 错误处理策略

**状态**: 已接受
**上下文**: 需要确定异常处理和错误恢复策略

**决策**: 
- 使用自定义异常类型
- 提供详细的错误信息
- 支持错误恢复机制

**实现**:
```csharp
public class EditorManagerCreationException : Exception
{
    public string ErrorCode { get; }
    public EditorManagerCreationFailureReason FailureReason { get; }
}
```

## 实现方案详解

### 1. 核心组件实现

#### 1.1 EditorManagerFactory 实现

```csharp
public class EditorManagerFactory : IEditorManagerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EditorManagerFactory> _logger;
    private readonly IEditorManagerPerformanceMonitor? _performanceMonitor;

    public EditorManagerFactory(
        IServiceProvider serviceProvider,
        ILogger<EditorManagerFactory> logger,
        IEditorManagerPerformanceMonitor? performanceMonitor = null)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _performanceMonitor = performanceMonitor;
    }

    public EditorManagerViewModel CreateEditorManager(EditorManagerOptions? options = null)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogDebug("Creating EditorManagerViewModel with options");
            
            var effectiveOptions = options ?? CreateDefaultOptions();
            effectiveOptions.Validate();
            
            var result = new EditorManagerViewModel(effectiveOptions);
            
            stopwatch.Stop();
            RecordPerformance(stopwatch.Elapsed, effectiveOptions);
            
            _logger.LogInformation("Successfully created EditorManagerViewModel in {ElapsedMs}ms", 
                stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to create EditorManagerViewModel after {ElapsedMs}ms", 
                stopwatch.ElapsedMilliseconds);
            
            throw new EditorManagerCreationException(
                "Failed to create EditorManagerViewModel", 
                DetermineFailureReason(ex),
                ex);
        }
    }

    private EditorManagerCreationFailureReason DetermineFailureReason(Exception ex)
    {
        return ex switch
        {
            ArgumentNullException => EditorManagerCreationFailureReason.InvalidOptions,
            InvalidOperationException => EditorManagerCreationFailureReason.ConfigurationError,
            TargetInvocationException => EditorManagerCreationFailureReason.ConstructorException,
            _ => EditorManagerCreationFailureReason.Unknown
        };
    }

    private void RecordPerformance(TimeSpan elapsed, EditorManagerOptions options)
    {
        _performanceMonitor?.RecordCreationTime(elapsed, options);
    }
}
```

#### 1.2 EditorManagerViewModel 重构

```csharp
public partial class EditorManagerViewModel : ViewModelBase
{
    // 保持现有字段...
    private readonly IEditorFactory? _editorFactory;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;
    private readonly IValidationService _validationService;
    private readonly IServiceProvider? _serviceProvider;

    /// <summary>
    /// 主要构造函数（推荐使用）
    /// </summary>
    public EditorManagerViewModel(EditorManagerOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        _editorFactory = options.EditorFactory;
        _logService = options.LogService ?? new LogService();
        _errorHandlerService = options.ErrorHandlerService ?? new ErrorHandlerService();
        _validationService = options.ValidationService ?? new ValidationService();
        _serviceProvider = options.ServiceProvider;

        Initialize();
    }

    /// <summary>
    /// 内部构造函数（供工厂使用）
    /// </summary>
    internal EditorManagerViewModel(
        IEditorFactory editorFactory,
        ILogService logService,
        IErrorHandlerService errorHandlerService,
        IValidationService validationService,
        IServiceProvider serviceProvider)
    {
        _editorFactory = editorFactory ?? throw new ArgumentNullException(nameof(editorFactory));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _errorHandlerService = errorHandlerService ?? throw new ArgumentNullException(nameof(errorHandlerService));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        Initialize();
    }

    /// <summary>
    /// 初始化组件（保持现有逻辑）
    /// </summary>
    private void Initialize()
    {
        LoadEditors();
    }

    // 保持现有的其他方法...
}
```

### 2. 性能监控实现

#### 2.1 性能监控器

```csharp
public class EditorManagerPerformanceMonitor : IEditorManagerPerformanceMonitor
{
    private readonly ILogger<EditorManagerPerformanceMonitor> _logger;
    private readonly object _lock = new();
    private EditorManagerPerformanceStats _stats;

    public EditorManagerPerformanceMonitor(ILogger<EditorManagerPerformanceMonitor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stats = new EditorManagerPerformanceStats
        {
            StartedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            MinCreationTime = double.MaxValue
        };
    }

    public void RecordCreationTime(TimeSpan creationTime, EditorManagerOptions options)
    {
        lock (_lock)
        {
            _stats.TotalCreations++;
            _stats.AverageCreationTime = 
                (_stats.AverageCreationTime * (_stats.TotalCreations - 1) + creationTime.TotalMilliseconds) / _stats.TotalCreations;
            _stats.MaxCreationTime = Math.Max(_stats.MaxCreationTime, creationTime.TotalMilliseconds);
            _stats.MinCreationTime = Math.Min(_stats.MinCreationTime, creationTime.TotalMilliseconds);
            _stats.LastUpdated = DateTime.UtcNow;

            _logger.LogDebug("Recorded creation time: {CreationTime}ms, Average: {AverageTime}ms", 
                creationTime.TotalMilliseconds, _stats.AverageCreationTime);
        }
    }

    public void RecordMemoryUsage(long memoryUsage)
    {
        lock (_lock)
        {
            _stats.MemoryUsage = memoryUsage;
            _stats.LastUpdated = DateTime.UtcNow;
        }
    }

    public void RecordException(Exception exception, string context)
    {
        lock (_lock)
        {
            _stats.ExceptionCount++;
            _stats.LastUpdated = DateTime.UtcNow;
            
            _logger.LogWarning(exception, "Exception recorded in {Context}: {ExceptionMessage}", 
                context, exception.Message);
        }
    }

    public EditorManagerPerformanceStats GetPerformanceStats()
    {
        lock (_lock)
        {
            return _stats with
            {
                // 返回副本以确保线程安全
            };
        }
    }

    public void ResetStats()
    {
        lock (_lock)
        {
            _stats = new EditorManagerPerformanceStats
            {
                StartedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                MinCreationTime = double.MaxValue
            };
            
            _logger.LogInformation("Performance statistics reset");
        }
    }
}
```

### 3. 健康检查实现

#### 3.1 健康检查服务

```csharp
public class EditorManagerHealthCheck : IHealthCheck
{
    private readonly IEditorManagerFactory _factory;
    private readonly ILogger<EditorManagerHealthCheck> _logger;
    private readonly EditorManagerHealthCheckOptions _options;

    public EditorManagerHealthCheck(
        IEditorManagerFactory factory,
        ILogger<EditorManagerHealthCheck> logger,
        IOptions<EditorManagerHealthCheckOptions> options)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new EditorManagerHealthCheckOptions();
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
                var failureMessage = "EditorManager factory configuration is invalid";
                _logger.LogWarning(failureMessage);
                return HealthCheckResult.Unhealthy(failureMessage);
            }

            // 创建测试实例
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_options.Timeout);

            var editorManager = await Task.Run(() => 
            {
                cts.Token.ThrowIfCancellationRequested();
                return _factory.CreateDefaultEditorManager();
            }, cts.Token);

            if (editorManager == null)
            {
                var failureMessage = "Failed to create EditorManager instance";
                _logger.LogError(failureMessage);
                return HealthCheckResult.Unhealthy(failureMessage);
            }

            // 验证基本功能
            var validationIssues = new List<string>();
            
            if (editorManager.Categories == null)
            {
                validationIssues.Add("Categories is null");
            }

            if (editorManager.Categories?.Count == 0)
            {
                validationIssues.Add("No categories loaded");
            }

            if (validationIssues.Count > 0)
            {
                var failureMessage = $"EditorManager validation failed: {string.Join(", ", validationIssues)}";
                _logger.LogWarning(failureMessage);
                return HealthCheckResult.Degraded(failureMessage);
            }

            var data = new Dictionary<string, object>
            {
                { "categories_count", editorManager.Categories?.Count ?? 0 },
                { "factory_type", _factory.GetType().Name },
                { "creation_timeout_ms", _options.Timeout.TotalMilliseconds }
            };

            _logger.LogInformation("EditorManager health check completed successfully");
            return HealthCheckResult.Healthy("EditorManager is working correctly", data: data);
        }
        catch (OperationCanceledException)
        {
            var failureMessage = $"EditorManager health check timed out after {_options.Timeout.TotalMilliseconds}ms";
            _logger.LogWarning(failureMessage);
            return HealthCheckResult.Unhealthy(failureMessage);
        }
        catch (Exception ex)
        {
            var failureMessage = "EditorManager health check failed";
            _logger.LogError(ex, failureMessage);
            return HealthCheckResult.Unhealthy(failureMessage, ex);
        }
    }
}
```

### 4. 配置选项

#### 4.1 健康检查配置

```csharp
public class EditorManagerHealthCheckOptions
{
    /// <summary>
    /// 健康检查超时时间
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// 是否启用详细日志
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = false;

    /// <summary>
    /// 最大允许的创建时间（毫秒）
    /// </summary>
    public double MaxAllowedCreationTime { get; set; } = 1000;
}
```

## 依赖注入配置

### 1. 完整的服务注册

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEditorManagerServices(
        this IServiceCollection services,
        Action<EditorManagerOptions>? configureOptions = null,
        Action<EditorManagerHealthCheckOptions>? configureHealthCheckOptions = null)
    {
        // 配置基础选项
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // 配置健康检查选项
        if (configureHealthCheckOptions != null)
        {
            services.Configure(configureHealthCheckOptions);
        }

        // 注册核心服务
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>(sp => 
        {
            var factory = sp.GetRequiredService<IEditorManagerFactory>();
            var options = sp.GetService<IOptions<EditorManagerOptions>>()?.Value;
            return factory.CreateEditorManager(options);
        });

        // 注册可选的性能监控
        services.TryAddSingleton<IEditorManagerPerformanceMonitor, EditorManagerPerformanceMonitor>();

        // 注册健康检查
        services.AddHealthChecks()
            .AddCheck<EditorManagerHealthCheck>("editor_manager");

        return services;
    }
}
```

### 2. 在 App.axaml.cs 中的使用

```csharp
private IServiceCollection ConfigureServices()
{
    var services = new ServiceCollection();
    
    // 注册基础服务
    services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
    services.AddSingleton<ILogService, LogService>();
    services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
    services.AddSingleton<IValidationService, ValidationService>();
    services.AddSingleton<IDataBindingService, DataBindingService>();
    
    // 注册 EditorManager 服务（带配置）
    services.AddEditorManagerServices(options =>
    {
        options.EnableDebugMode = true;
        options.EnablePerformanceMonitoring = true;
    }, healthCheckOptions =>
    {
        healthCheckOptions.Timeout = TimeSpan.FromSeconds(3);
        healthCheckOptions.EnableDetailedLogging = true;
    });
    
    // 注册其他服务...
    
    return services;
}
```

## 测试实现

### 1. 单元测试

```csharp
[TestClass]
public class EditorManagerFactoryTests
{
    private Mock<IServiceProvider> _serviceProvider;
    private Mock<ILogger<EditorManagerFactory>> _logger;
    private EditorManagerFactory _factory;

    [TestInitialize]
    public void Setup()
    {
        _serviceProvider = new Mock<IServiceProvider>();
        _logger = new Mock<ILogger<EditorManagerFactory>>();
        _factory = new EditorManagerFactory(_serviceProvider.Object, _logger.Object);
    }

    [TestMethod]
    public void CreateEditorManager_WithValidOptions_ReturnsInstance()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object,
            ValidationService = new Mock<IValidationService>().Object
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<EditorManagerViewModel>();
    }

    [TestMethod]
    public void CreateEditorManager_WithNullOptions_UsesDefaults()
    {
        // Act
        var result = _factory.CreateEditorManager(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<EditorManagerViewModel>();
    }

    [TestMethod]
    public void CreateEditorManager_WithInvalidOptions_ThrowsException()
    {
        // Arrange
        var options = new EditorManagerOptions(); // 缺少必需的服务

        // Act & Assert
        var action = () => _factory.CreateEditorManager(options);
        action.Should().Throw<EditorManagerCreationException>()
            .WithErrorCode("EM_102");
    }
}
```

### 2. 集成测试

```csharp
[TestClass]
public class EditorManagerIntegrationTests
{
    private IServiceProvider _serviceProvider;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        
        // 注册测试服务
        services.AddSingleton<ILogService, MockLogService>();
        services.AddSingleton<IErrorHandlerService, MockErrorHandlerService>();
        services.AddSingleton<IValidationService, MockValidationService>();
        services.AddSingleton<IEditorFactory, MockEditorFactory>();
        
        // 注册 EditorManager 服务
        services.AddEditorManagerServices();
        
        _serviceProvider = services.BuildServiceProvider();
    }

    [TestMethod]
    public void ServiceProvider_CanResolveEditorManagerViewModel()
    {
        // Act
        var editorManager = _serviceProvider.GetService<EditorManagerViewModel>();

        // Assert
        editorManager.Should().NotBeNull();
        editorManager.Categories.Should().NotBeNull();
    }

    [TestMethod]
    public void Factory_CanCreateMultipleInstances()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var instance1 = factory.CreateEditorManager();
        var instance2 = factory.CreateEditorManager();

        // Assert
        instance1.Should().NotBeNull();
        instance2.Should().NotBeNull();
        instance1.Should().NotBeSameAs(instance2);
    }
}
```

## 性能优化策略

### 1. 内存优化
- **对象池**: 对频繁创建的对象使用对象池
- **延迟初始化**: 延迟初始化非必要的服务
- **弱引用**: 对大型对象使用弱引用

### 2. 并发优化
- **线程安全**: 确保工厂方法的线程安全性
- **异步支持**: 支持异步创建操作
- **锁优化**: 使用细粒度锁减少竞争

### 3. 缓存策略
- **实例缓存**: 缓存常用的配置和实例
- **服务缓存**: 缓存已解析的服务
- **结果缓存**: 缓存验证结果

## 部署和监控

### 1. 部署考虑
- **兼容性**: 确保与现有部署兼容
- **配置**: 支持配置文件和环境变量
- **日志**: 配置适当的日志级别

### 2. 监控指标
- **创建时间**: 监控实例创建时间
- **错误率**: 监控创建失败率
- **内存使用**: 监控内存使用情况
- **健康状态**: 监控服务健康状态

### 3. 告警规则
- **创建时间过长**: 超过 1 秒告警
- **错误率过高**: 超过 5% 告警
- **内存泄漏**: 内存持续增长告警
- **健康检查失败**: 服务不可用告警

## 迁移指南

### 1. 代码迁移
1. **保持现有构造函数**: 标记为 `[Obsolete]`
2. **引入工厂模式**: 添加工厂服务注册
3. **逐步迁移**: 逐步将现有代码迁移到工厂模式
4. **测试验证**: 确保迁移后功能正常

### 2. 配置迁移
1. **更新服务注册**: 使用新的扩展方法
2. **配置选项**: 迁移到 Options 模式
3. **健康检查**: 添加健康检查配置

### 3. 测试迁移
1. **单元测试**: 更新测试以使用工厂模式
2. **集成测试**: 验证依赖注入集成
3. **性能测试**: 确保性能没有回归

## 总结

这个技术栈方案通过工厂模式成功解决了 EditorManagerViewModel 的依赖注入歧义问题。该方案具有以下特点：

1. **技术成熟**: 基于现有的 .NET 9 和 Avalonia UI 技术栈
2. **架构清晰**: 遵循 SOLID 原则和依赖注入最佳实践
3. **性能优化**: 包含性能监控和优化策略
4. **易于测试**: 提供完整的测试实现
5. **监控完善**: 包含健康检查和监控机制
6. **迁移友好**: 支持渐进式迁移，保持向后兼容

该方案为项目的长期发展提供了坚实的基础，同时解决了当前的技术债务问题。