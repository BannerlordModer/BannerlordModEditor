# 架构修复实施指南

## 快速开始

本指南提供了Bannerlord Mod Editor架构修复的快速实施路径。

## 问题概览

当前项目存在以下关键问题：
1. **依赖注入配置不一致** - BaseEditorViewModel构造函数参数不匹配
2. **ViewModel工厂模式问题** - MockEditorFactory回退机制复杂
3. **数据模型兼容性问题** - XML序列化测试失败
4. **跨平台兼容性问题** - 路径处理和平台差异

## 核心修复策略

### 1. 依赖注入统一化

#### 问题分析
```csharp
// 当前问题：BaseEditorViewModel构造函数参数不匹配
protected BaseEditorViewModel(string xmlFileName, string editorName,
    IErrorHandlerService? errorHandler = null,
    ILogService? logService = null)
    : base(errorHandler, logService)  // 参数名不匹配
```

#### 解决方案
```csharp
// 修复：统一服务注册和参数传递
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBannerlordServices(this IServiceCollection services)
    {
        // 核心服务 - 单例
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        
        // 工厂服务 - 单例
        services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        
        // 业务服务 - 作用域
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IDataBindingService, DataBindingService>();
        
        // 数据服务 - 瞬态
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        
        // 编辑器ViewModel - 瞬态
        services.AddTransient<AttributeEditorViewModel>();
        services.AddTransient<SkillEditorViewModel>();
        services.AddTransient<BoneBodyTypeEditorViewModel>();
        services.AddTransient<CraftingPieceEditorViewModel>();
        services.AddTransient<ItemModifierEditorViewModel>();
        services.AddTransient<CombatParameterEditorViewModel>();
        services.AddTransient<ItemEditorViewModel>();
        
        return services;
    }
}
```

### 2. ViewModel工厂模式重构

#### 问题分析
```csharp
// 当前问题：MockEditorFactory中复杂的回退机制
try
{
    var attributeEditor = serviceProvider.GetRequiredService<AttributeEditorViewModel>();
    // ... 更多服务创建
}
catch (Exception ex)
{
    // 复杂的回退逻辑
    Console.WriteLine($"警告: 依赖注入创建编辑器失败: {ex.Message}");
    // 手动创建实例
}
```

#### 解决方案
```csharp
// 统一的ViewModel工厂接口
public interface IViewModelFactory
{
    TViewModel CreateViewModel<TViewModel>(params object[] parameters) 
        where TViewModel : ViewModelBase;
    
    TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new();
}

// 简化的工厂实现
public class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;

    public TViewModel CreateViewModel<TViewModel>(params object[] parameters)
        where TViewModel : ViewModelBase
    {
        try
        {
            // 尝试通过服务提供器创建
            var viewModel = _serviceProvider.GetService<TViewModel>();
            if (viewModel != null)
            {
                return viewModel;
            }
            
            // 如果失败，尝试通过反射创建
            return (TViewModel)Activator.CreateInstance(typeof(TViewModel), parameters);
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, $"Failed to create ViewModel: {typeof(TViewModel).Name}");
            throw;
        }
    }
}
```

### 3. 数据模型兼容性修复

#### 问题分析
```csharp
// 当前问题：XML序列化测试失败
// - 空元素处理不当
// - 属性顺序变化
// - 数据结构不一致
```

#### 解决方案
```csharp
// DO/DTO模式实现
public abstract class DomainObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public abstract class DataTransferObject
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// 映射器实现
public abstract class BaseMapper<TDomain, TDto>
    where TDomain : DomainObject
    where TDto : DataTransferObject
{
    public abstract TDto ToDto(TDomain domain);
    public abstract TDomain ToDomain(TDto dto);
    
    public virtual IEnumerable<TDto> ToDto(IEnumerable<TDomain> domains)
    {
        return domains.Select(ToDto);
    }
    
    public virtual IEnumerable<TDomain> ToDomain(IEnumerable<TDto> dtos)
    {
        return dtos.Select(ToDomain);
    }
}
```

### 4. 跨平台兼容性修复

#### 问题分析
```csharp
// 当前问题：路径处理和平台差异
// - Windows使用反斜杠
// - Linux/Mac使用正斜杠
// - 文件系统权限差异
```

#### 解决方案
```csharp
// 跨平台路径处理
public static class PathHelper
{
    public static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;
            
        // 统一使用正斜杠
        return path.Replace('\\', '/');
    }
    
    public static string CombinePaths(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return string.Empty;
            
        var normalizedPaths = paths.Select(NormalizePath).ToArray();
        return string.Join("/", normalizedPaths);
    }
    
    public static bool IsPathValid(string path)
    {
        try
        {
            var fullPath = Path.GetFullPath(path);
            return Path.IsPathRooted(fullPath);
        }
        catch
        {
            return false;
        }
    }
}
```

## 快速实施步骤

### 第一步：修复依赖注入配置

1. **创建服务注册扩展方法**
```csharp
// 在 BannerlordModEditor.UI/Extensions/ServiceCollectionExtensions.cs
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBannerlordServices(this IServiceCollection services)
    {
        // 注册所有服务
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        
        // ... 其他服务注册
        
        return services;
    }
}
```

2. **更新App.axaml.cs**
```csharp
// 在 App.axaml.cs 中
private IServiceCollection ConfigureServices()
{
    var services = new ServiceCollection();
    services.AddBannerlordServices(); // 使用扩展方法
    return services;
}
```

### 第二步：重构ViewModel工厂

1. **创建统一的ViewModel工厂接口**
```csharp
// 在 BannerlordModEditor.UI/Factories/IViewModelFactory.cs
public interface IViewModelFactory
{
    TViewModel CreateViewModel<TViewModel>(params object[] parameters) 
        where TViewModel : ViewModelBase;
    
    TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new();
}
```

2. **实现ViewModel工厂**
```csharp
// 在 BannerlordModEditor.UI/Factories/ViewModelFactory.cs
public class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;

    public ViewModelFactory(
        IServiceProvider serviceProvider,
        ILogService logService,
        IErrorHandlerService errorHandlerService)
    {
        _serviceProvider = serviceProvider;
        _logService = logService;
        _errorHandlerService = errorHandlerService;
    }

    public TViewModel CreateViewModel<TViewModel>(params object[] parameters)
        where TViewModel : ViewModelBase
    {
        try
        {
            // 尝试通过服务提供器创建
            var viewModel = _serviceProvider.GetService<TViewModel>();
            if (viewModel != null)
            {
                return viewModel;
            }
            
            // 如果失败，尝试通过反射创建
            return (TViewModel)Activator.CreateInstance(typeof(TViewModel), parameters);
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, $"Failed to create ViewModel: {typeof(TViewModel).Name}");
            throw;
        }
    }
    
    public TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new()
    {
        try
        {
            // 获取必要的服务
            var errorHandler = _serviceProvider.GetService<IErrorHandlerService>();
            var logService = _serviceProvider.GetService<ILogService>();
            
            // 创建ViewModel实例
            var viewModel = (TEditorViewModel)Activator.CreateInstance(
                typeof(TEditorViewModel), 
                xmlFileName, 
                editorName,
                errorHandler,
                logService);
            
            return viewModel;
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, $"Failed to create EditorViewModel: {typeof(TEditorViewModel).Name}");
            throw;
        }
    }
}
```

3. **更新服务注册**
```csharp
// 在 ServiceCollectionExtensions.cs 中添加
services.AddSingleton<IViewModelFactory, ViewModelFactory>();
```

### 第三步：修复BaseEditorViewModel构造函数

1. **修复BaseEditorViewModel构造函数**
```csharp
// 在 BaseEditorViewModel.cs 中
protected BaseEditorViewModel(string xmlFileName, string editorName,
    IErrorHandlerService? errorHandler = null,
    ILogService? logService = null)
    : base(errorHandler, logService)  // 修复参数名
{
    XmlFileName = xmlFileName;
    EditorName = editorName;
    
    LogInfo($"Initialized {EditorName} editor", "BaseEditorViewModel");
    
    // 初始化过滤后的集合
    FilteredItems = new ObservableCollection<TItem>(Items);
    
    // 监听原始集合变化
    Items.CollectionChanged += (s, e) =>
    {
        UpdateFilteredItems();
        UpdateCounts();
        HasUnsavedChanges = true;
    };
}
```

### 第四步：创建简化的Mock工厂

1. **创建简化的Mock工厂**
```csharp
// 在 BannerlordModEditor.UI.Tests/Helpers/SimplifiedMockFactory.cs
public class SimplifiedMockFactory : IViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public SimplifiedMockFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public TViewModel CreateViewModel<TViewModel>(params object[] parameters)
        where TViewModel : ViewModelBase
    {
        try
        {
            // 直接通过服务提供器创建
            return _serviceProvider.GetRequiredService<TViewModel>();
        }
        catch (Exception ex)
        {
            // 如果失败，记录错误并抛出
            throw new InvalidOperationException($"Failed to create ViewModel: {typeof(TViewModel).Name}", ex);
        }
    }
    
    public TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new()
    {
        try
        {
            // 获取必要的服务
            var errorHandler = _serviceProvider.GetRequiredService<IErrorHandlerService>();
            var logService = _serviceProvider.GetRequiredService<ILogService>();
            
            // 创建ViewModel实例
            return (TEditorViewModel)Activator.CreateInstance(
                typeof(TEditorViewModel), 
                xmlFileName, 
                editorName,
                errorHandler,
                logService);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create EditorViewModel: {typeof(TEditorViewModel).Name}", ex);
        }
    }
}
```

2. **更新测试设置**
```csharp
// 在 TestServiceProvider.cs 中
public static IServiceProvider GetServiceProvider()
{
    if (_serviceProvider == null)
    {
        var services = new ServiceCollection();
        
        // 注册测试服务
        services.AddSingleton<ILogService, MockLogService>();
        services.AddSingleton<IErrorHandlerService, MockErrorHandlerService>();
        services.AddSingleton<IValidationService, MockValidationService>();
        services.AddSingleton<IDataBindingService, MockDataBindingService>();
        services.AddSingleton<IEditorFactory, MockEditorFactory>();
        services.AddSingleton<IViewModelFactory, SimplifiedMockFactory>(); // 使用简化的Mock工厂
        
        // 注册实际的服务
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    return _serviceProvider;
}
```

### 第五步：添加跨平台支持

1. **创建路径处理工具类**
```csharp
// 在 BannerlordModEditor.Common/Utilities/PathHelper.cs
public static class PathHelper
{
    public static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;
            
        // 统一使用正斜杠
        return path.Replace('\\', '/');
    }
    
    public static string CombinePaths(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return string.Empty;
            
        var normalizedPaths = paths.Select(NormalizePath).ToArray();
        return string.Join("/", normalizedPaths);
    }
    
    public static bool IsPathValid(string path)
    {
        try
        {
            var fullPath = Path.GetFullPath(path);
            return Path.IsPathRooted(fullPath);
        }
        catch
        {
            return false;
        }
    }
    
    public static string GetPlatformPathSeparator()
    {
        return Path.DirectorySeparatorChar.ToString();
    }
}
```

2. **更新文件查找逻辑**
```csharp
// 在 BaseEditorViewModel.cs 中更新文件查找方法
private async Task<string?> FindFileAsync(string fileName)
{
    var possiblePaths = new[]
    {
        PathHelper.CombinePaths("TestData", fileName),
        PathHelper.CombinePaths("BannerlordModEditor.Common.Tests", "TestData", fileName),
        PathHelper.CombinePaths("example", "ModuleData", fileName),
        fileName
    };

    foreach (var path in possiblePaths)
    {
        if (File.Exists(path))
        {
            return path;
        }
    }

    return null;
}
```

## 验证步骤

### 1. 验证依赖注入配置
```bash
# 运行应用程序
dotnet run --project BannerlordModEditor.UI

# 检查是否正常启动
# 检查日志是否有服务注册错误
```

### 2. 验证ViewModel工厂
```bash
# 运行测试
dotnet test BannerlordModEditor.UI.Tests --filter "ViewModelFactory"

# 检查测试是否通过
# 检查是否有工厂创建错误
```

### 3. 验证跨平台兼容性
```bash
# 在不同平台上运行测试
# Windows
dotnet test BannerlordModEditor.UI.Tests

# Linux
dotnet test BannerlordModEditor.UI.Tests

# Mac
dotnet test BannerlordModEditor.UI.Tests
```

### 4. 验证数据模型
```bash
# 运行XML序列化测试
dotnet test BannerlordModEditor.Common.Tests --filter "Xml"

# 检查往返测试是否通过
```

## 常见问题解决方案

### 问题1：依赖注入服务无法解析
```csharp
// 错误：Unable to resolve service for type 'IService'
// 解决：确保服务已正确注册
services.AddSingleton<IService, ServiceImplementation>();
```

### 问题2：ViewModel创建失败
```csharp
// 错误：Failed to create ViewModel
// 解决：检查构造函数参数和服务注册
public MyViewModel(IService service) 
{
    _service = service ?? throw new ArgumentNullException(nameof(service));
}
```

### 问题3：跨平台路径问题
```csharp
// 错误：路径在不同平台表现不一致
// 解决：使用PathHelper处理路径
var path = PathHelper.CombinePaths("TestData", "file.xml");
```

### 问题4：XML序列化失败
```csharp
// 错误：XML序列化往返测试失败
// 解决：使用DO/DTO模式
var domain = mapper.ToDomain(dto);
var result = mapper.ToDto(domain);
```

## 性能优化建议

### 1. 服务生命周期优化
```csharp
// 单例服务：应用程序生命周期
services.AddSingleton<ILogService, LogService>();

// 作用域服务：请求生命周期
services.AddScoped<IValidationService, ValidationService>();

// 瞬态服务：每次请求创建新实例
services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
```

### 2. 异步编程优化
```csharp
// 使用ValueTask优化异步方法
public ValueTask<T> GetAsync<T>(string key)
{
    if (_cache.TryGetValue(key, out var value))
    {
        return new ValueTask<T>((T)value);
    }
    
    return new ValueTask<T>(GetFromSourceAsync(key));
}
```

### 3. 内存管理优化
```csharp
// 正确使用IDisposable
public class DataService : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private bool _disposed;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbConnection?.Dispose();
            }
            _disposed = true;
        }
    }
}
```

## 下一步行动

1. **立即执行**：修复依赖注入配置和ViewModel工厂
2. **短期目标**：完善测试架构和跨平台支持
3. **中期目标**：实施DO/DTO模式和数据流管理
4. **长期目标**：完善监控和CI/CD流程

## 支持资源

- **文档**：查看完整的架构设计文档
- **示例**：参考示例代码实现
- **工具**：使用推荐的开发工具和测试框架
- **社区**：参与社区讨论和问题反馈

通过这个简化的实施指南，你可以快速开始架构修复工作，解决当前项目中的关键问题。