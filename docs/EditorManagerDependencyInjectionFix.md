# EditorManagerViewModel 依赖注入歧义问题修复方案

## 问题概述

原来的EditorManagerViewModel存在多个构造函数重载，导致依赖注入容器无法确定使用哪个构造函数，从而引发歧义问题。

## 修复方案

### 1. 实现EditorManagerFactory工厂类

**文件位置**: `/BannerlordModEditor.UI/Factories/EditorManagerFactory.cs`

```csharp
public class EditorManagerFactory : IEditorManagerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;
    private readonly IEditorFactory _editorFactory;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;

    public EditorManagerViewModel CreateEditorManager()
    {
        return CreateEditorManager(EditorManagerCreationOptions.Default);
    }

    public EditorManagerViewModel CreateEditorManager(EditorManagerCreationOptions options)
    {
        // 线程安全的创建过程，包含健康检查和性能监控
        // ...
    }
}
```

### 2. 优化EditorManagerOptions配置类

**文件位置**: `/BannerlordModEditor.UI/ViewModels/EditorManagerViewModel.cs`

```csharp
public class EditorManagerOptions
{
    public IEditorFactory? EditorFactory { get; set; }
    public ILogService? LogService { get; set; }
    public IErrorHandlerService? ErrorHandlerService { get; set; }
    public IValidationService? ValidationService { get; set; }
    public IDataBindingService? DataBindingService { get; set; }
    public IServiceProvider? ServiceProvider { get; set; }
    
    // 新增配置选项
    public bool EnablePerformanceMonitoring { get; set; } = false;
    public bool EnableHealthChecks { get; set; } = true;
    public bool EnableDiagnostics { get; set; } = false;
    public bool StrictMode { get; set; } = false;
    
    // 验证方法
    public ConfigurationValidationResult Validate()
    {
        // 配置验证逻辑
    }
}
```

### 3. 修改EditorManagerViewModel构造函数

**文件位置**: `/BannerlordModEditor.UI/ViewModels/EditorManagerViewModel.cs`

```csharp
public EditorManagerViewModel(EditorManagerOptions? options = null)
{
    var config = options ?? EditorManagerOptions.Default;
    
    // 验证配置
    var validationResult = config.Validate();
    if (!validationResult.IsValid)
    {
        throw new InvalidOperationException($"Invalid EditorManagerOptions configuration");
    }
    
    // 分配服务和配置
    _logService = config.LogService ?? new LogService();
    _errorHandlerService = config.ErrorHandlerService ?? new ErrorHandlerService();
    // ...
    
    // 执行健康检查
    if (_enableHealthChecks)
    {
        PerformHealthChecks();
    }
}

// 保留过时的构造函数以向后兼容
[Obsolete("请使用 EditorManagerViewModel(EditorManagerOptions) 构造函数")]
public EditorManagerViewModel(
    IEditorFactory? editorFactory = null,
    ILogService? logService = null,
    // ...
    ) : this(new EditorManagerOptions { ... })
{
}
```

### 4. 更新依赖注入注册

**文件位置**: `/BannerlordModEditor.UI/App.axaml.cs`

```csharp
private IServiceCollection ConfigureServices()
{
    var services = new ServiceCollection();
    
    // 使用扩展方法注册EditorManager相关服务
    services.AddEditorManagerServices(options =>
    {
        options.EnableHealthChecks = true;
        options.UseLogService = true;
        options.UseErrorHandlerService = true;
        options.UseValidationService = true;
        options.UseDataBindingService = true;
        options.UseEditorFactory = true;
    });
    
    // 注册其他服务
    RegisterEditorServices(services);
    
    // 验证服务注册
    ValidateServiceRegistration(services);
    
    return services;
}
```

### 5. 创建依赖注入扩展方法

**文件位置**: `/BannerlordModEditor.UI/Extensions/ServiceCollectionExtensions.cs`

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEditorManagerServices(
        this IServiceCollection services,
        Action<EditorManagerServiceOptions> configureOptions)
    {
        var options = new EditorManagerServiceOptions();
        configureOptions(options);
        options.Validate();
        
        RegisterCoreServices(services, options);
        RegisterEditorFactoryServices(services, options);
        RegisterEditorManagerServices(services, options);
        
        return services;
    }
    
    public static ServiceRegistrationValidationResult ValidateEditorManagerServices(
        this IServiceCollection services)
    {
        // 验证服务注册
    }
}
```

## 使用示例

### 1. 通过依赖注入容器使用

```csharp
// 在ConfigureServices中注册
services.AddEditorManagerServices(options =>
{
    options.EnableHealthChecks = true;
    options.EnablePerformanceMonitoring = false;
});

// 在其他类中使用
public class MyClass
{
    private readonly EditorManagerViewModel _editorManager;
    
    public MyClass(EditorManagerViewModel editorManager)
    {
        _editorManager = editorManager;
    }
}
```

### 2. 通过工厂创建

```csharp
// 通过依赖注入获取工厂
var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

// 创建标准实例
var editorManager = factory.CreateEditorManager();

// 使用自定义配置创建
var options = new EditorManagerCreationOptions
{
    EnablePerformanceMonitoring = true,
    EnableHealthChecks = true,
    EnableDiagnostics = true
};
var customEditorManager = factory.CreateEditorManager(options);
```

### 3. 直接创建（测试场景）

```csharp
// 使用默认配置
var editorManager = new EditorManagerViewModel();

// 使用自定义配置
var options = new EditorManagerOptions
{
    LogService = new LogService(),
    ErrorHandlerService = new ErrorHandlerService(),
    ValidationService = new ValidationService(),
    EnableHealthChecks = true
};
var editorManager = new EditorManagerViewModel(options);

// 使用依赖注入配置
var serviceProvider = BuildServiceProvider();
var options = EditorManagerOptions.ForDependencyInjection(serviceProvider);
var editorManager = new EditorManagerViewModel(options);
```

## 性能监控和健康检查

### 性能监控

```csharp
var options = new EditorManagerCreationOptions
{
    EnablePerformanceMonitoring = true
};

var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
var editorManager = factory.CreateEditorManager(options);

// 获取性能统计
var stats = factory.GetStatistics();
Console.WriteLine($"Instances created: {stats.InstancesCreated}");
Console.WriteLine($"Last creation time: {stats.LastCreationTime}");
```

### 健康检查

```csharp
var options = new EditorManagerCreationOptions
{
    EnableHealthChecks = true,
    StrictMode = true // 在健康检查失败时抛出异常
};

try
{
    var editorManager = factory.CreateEditorManager(options);
}
catch (EditorManagerCreationException ex)
{
    Console.WriteLine($"Failed to create editor manager: {ex.Message}");
}
```

## 配置验证

```csharp
var options = new EditorManagerOptions
{
    LogService = null, // 缺少必要服务
    ErrorHandlerService = null
};

var result = options.Validate();
if (!result.IsValid)
{
    Console.WriteLine("Configuration errors:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"- {error}");
    }
}

if (result.Warnings.Count > 0)
{
    Console.WriteLine("Configuration warnings:");
    foreach (var warning in result.Warnings)
    {
        Console.WriteLine($"- {warning}");
    }
}
```

## 向后兼容性

原有的构造函数仍然可用，但已标记为过时：

```csharp
// 仍然可用，但会产生编译警告
var editorManager = new EditorManagerViewModel(
    editorFactory: factory,
    logService: logService,
    errorHandlerService: errorHandlerService,
    validationService: validationService,
    serviceProvider: serviceProvider);
```

## 错误处理

### 1. 配置验证错误

```csharp
try
{
    var editorManager = new EditorManagerViewModel(invalidOptions);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Configuration error: {ex.Message}");
}
```

### 2. 工厂创建错误

```csharp
try
{
    var editorManager = factory.CreateEditorManager(options);
}
catch (EditorManagerCreationException ex)
{
    Console.WriteLine($"Creation failed: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
    }
}
```

## 测试场景

### 单元测试

```csharp
[Test]
public void TestEditorManagerCreation()
{
    // 使用测试配置
    var options = EditorManagerOptions.ForTesting();
    var editorManager = new EditorManagerViewModel(options);
    
    Assert.IsNotNull(editorManager);
    Assert.IsNotNull(editorManager.Categories);
}
```

### 集成测试

```csharp
[Test]
public void TestDependencyInjection()
{
    var services = new ServiceCollection();
    services.AddEditorManagerServices();
    
    var serviceProvider = services.BuildServiceProvider();
    var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
    
    Assert.IsNotNull(editorManager);
}
```

## 总结

通过这个修复方案，我们解决了以下问题：

1. **依赖注入歧义**: 使用单一构造函数模式，消除了构造函数重载歧义
2. **配置管理**: 通过EditorManagerOptions提供统一的配置接口
3. **错误处理**: 提供详细的配置验证和错误处理机制
4. **性能监控**: 支持创建过程的性能监控和统计
5. **健康检查**: 支持服务健康检查和诊断功能
6. **向后兼容**: 保留了原有构造函数，确保向后兼容性
7. **线程安全**: 工厂类实现了线程安全的创建过程
8. **可扩展性**: 通过配置选项支持功能扩展

这个方案遵循了SOLID原则，提供了清晰的依赖注入契约，并支持多种使用场景。