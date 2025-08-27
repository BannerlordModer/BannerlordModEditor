# 架构修复实施计划

## 概述

本文档提供了Bannerlord Mod Editor项目架构修复的详细实施计划。基于之前分析的依赖注入配置不一致、ViewModel工厂模式问题、数据模型兼容性等问题，我们制定了一个分阶段的实施策略。

## 实施原则

### 1. 渐进式重构
- 保持现有功能正常工作
- 逐步替换和重构组件
- 避免大规模重写

### 2. 向后兼容性
- 保持现有API接口
- 确保现有测试通过
- 渐进式迁移到新架构

### 3. 测试驱动
- 先编写测试验证现有行为
- 重构后确保测试通过
- 添加新的测试覆盖

### 4. 风险控制
- 每个阶段都有明确的回滚计划
- 定期备份和版本控制
- 持续监控和问题跟踪

## 实施阶段

### 阶段1：核心基础设施修复（高优先级）

#### 1.1 依赖注入配置统一

**目标**：解决依赖注入配置不一致问题

**任务清单**：
1. 创建统一的服务注册扩展方法
2. 实现服务生命周期管理
3. 修复BaseEditorViewModel构造函数参数不匹配问题
4. 创建测试友好的服务提供器

**实施步骤**：

```csharp
// 1. 创建服务注册扩展方法
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

**风险缓解**：
- 创建备份配置
- 逐步迁移服务注册
- 验证现有功能正常

**成功标准**：
- 所有服务正确注册
- 应用程序正常启动
- 现有测试通过

#### 1.2 ViewModel工厂模式重构

**目标**：统一ViewModel创建机制

**任务清单**：
1. 创建统一的ViewModel工厂接口
2. 实现ViewModel工厂
3. 重构现有编辑器ViewModel
4. 创建Mock工厂用于测试

**实施步骤**：

```csharp
// 1. 创建ViewModel工厂接口
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

// 2. 实现ViewModel工厂
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
}
```

**风险缓解**：
- 保留现有ViewModel创建方式
- 逐步迁移到新工厂
- 确保测试覆盖

**成功标准**：
- 所有ViewModel正常创建
- 工厂模式工作正常
- 测试通过

#### 1.3 服务层架构优化

**目标**：统一服务接口和实现

**任务清单**：
1. 创建统一的服务接口
2. 实现基础服务类
3. 重构现有服务
4. 添加错误处理和日志记录

**实施步骤**：

```csharp
// 1. 创建服务基类
public abstract class BaseService : ICoreService
{
    protected readonly ILogService LogService;
    protected readonly IErrorHandlerService ErrorHandlerService;
    
    public bool IsInitialized { get; protected set; }
    
    protected BaseService(ILogService logService, IErrorHandlerService errorHandlerService)
    {
        LogService = logService;
        ErrorHandlerService = errorHandlerService;
    }
    
    public virtual async Task InitializeAsync()
    {
        try
        {
            await OnInitializeAsync();
            IsInitialized = true;
            LogService.LogInfo($"{GetType().Name} initialized successfully", "Service");
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, $"Failed to initialize {GetType().Name}");
            throw;
        }
    }
    
    protected virtual Task OnInitializeAsync() => Task.CompletedTask;
}

// 2. 重构现有服务
public class LogService : BaseService, ILogService
{
    public LogService(IErrorHandlerService errorHandlerService) 
        : base(new LogService(errorHandlerService), errorHandlerService)
    {
    }
    
    public void LogInfo(string message, string category = "General")
    {
        Console.WriteLine($"[INFO] {category}: {message}");
    }
    
    // ... 其他方法实现
}
```

**风险缓解**：
- 逐步重构服务
- 保持接口兼容性
- 充分测试

**成功标准**：
- 服务正常工作
- 错误处理完善
- 日志记录正常

### 阶段2：数据层重构（中优先级）

#### 2.1 DO/DTO模式实施

**目标**：解决数据模型兼容性问题

**任务清单**：
1. 识别需要转换的数据模型
2. 创建领域对象和数据传输对象
3. 实现对象映射器
4. 更新现有代码使用新模型

**实施步骤**：

```csharp
// 1. 创建领域对象基类
public abstract class DomainObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

// 2. 创建数据传输对象基类
public abstract class DataTransferObject
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// 3. 创建映射器基类
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

// 4. 具体实现
public class AttributeDO : DomainObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DefaultValue { get; set; }
}

public class AttributeDTO : DataTransferObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DefaultValue { get; set; }
}

public class AttributeMapper : BaseMapper<AttributeDO, AttributeDTO>
{
    public override AttributeDTO ToDto(AttributeDO domain)
    {
        return new AttributeDTO
        {
            Id = domain.Id,
            Name = domain.Name,
            Description = domain.Description,
            DefaultValue = domain.DefaultValue,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }
    
    public override AttributeDO ToDomain(AttributeDTO dto)
    {
        return new AttributeDO
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            DefaultValue = dto.DefaultValue,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
```

**风险缓解**：
- 先分析现有数据模型
- 创建映射测试
- 逐步迁移代码

**成功标准**：
- 数据模型转换正确
- 现有功能正常
- 测试通过

#### 2.2 XML序列化统一

**目标**：统一XML序列化机制

**任务清单**：
1. 创建统一的XML序列化接口
2. 实现基础序列化器
3. 添加数据验证功能
4. 重构现有序列化代码

**实施步骤**：

```csharp
// 1. 创建XML序列化接口
public interface IXmlSerializer<T>
{
    Task<T> DeserializeAsync(string xmlContent);
    Task<T> DeserializeFromFileAsync(string filePath);
    Task<string> SerializeAsync(T obj);
    Task SerializeToFileAsync(T obj, string filePath);
    Task<bool> ValidateAsync(string xmlContent);
}

// 2. 实现基础序列化器
public abstract class BaseXmlSerializer<T> : IXmlSerializer<T>
{
    protected readonly ILogService LogService;
    protected readonly IErrorHandlerService ErrorHandlerService;
    
    protected BaseXmlSerializer(ILogService logService, IErrorHandlerService errorHandlerService)
    {
        LogService = logService;
        ErrorHandlerService = errorHandlerService;
    }
    
    public virtual async Task<T> DeserializeAsync(string xmlContent)
    {
        try
        {
            LogService.LogInfo($"Deserializing {typeof(T).Name} from XML", "XmlSerializer");
            
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlContent);
            var result = (T)serializer.Deserialize(reader);
            
            LogService.LogInfo($"Successfully deserialized {typeof(T).Name}", "XmlSerializer");
            return result;
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, $"Failed to deserialize {typeof(T).Name}");
            throw;
        }
    }
    
    // ... 其他方法实现
}

// 3. 具体实现
public class AttributeDataSerializer : BaseXmlSerializer<AttributeData>
{
    public AttributeDataSerializer(ILogService logService, IErrorHandlerService errorHandlerService)
        : base(logService, errorHandlerService)
    {
    }
    
    public override async Task<bool> ValidateAsync(string xmlContent)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            
            // 验证必需的元素
            var root = xmlDoc.DocumentElement;
            if (root == null || root.Name != "attributes")
            {
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, "XML validation failed");
            return false;
        }
    }
}
```

**风险缓解**：
- 保留现有序列化逻辑
- 创建序列化测试
- 验证数据完整性

**成功标准**：
- XML序列化正常工作
- 数据验证正确
- 性能无明显下降

#### 2.3 数据流管理

**目标**：统一数据流管理机制

**任务清单**：
1. 创建数据流管理器
2. 实现数据管道
3. 添加错误恢复机制
4. 重构现有数据流代码

**实施步骤**：

```csharp
// 1. 创建数据流管理器接口
public interface IDataFlowManager
{
    Task<TData> LoadDataAsync<TData>(string filePath) where TData : class;
    Task SaveDataAsync<TData>(TData data, string filePath) where TData : class;
    Task<IEnumerable<TItem>> ProcessDataAsync<TData, TItem>(TData data, Func<TData, IEnumerable<TItem>> processor);
    Task<TData> CreateDataAsync<TData, TItem>(IEnumerable<TItem> items, Func<IEnumerable<TItem>, TData> creator);
}

// 2. 实现数据流管理器
public class DataFlowManager : IDataFlowManager
{
    private readonly IXmlSerializerFactory _serializerFactory;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;
    
    public DataFlowManager(
        IXmlSerializerFactory serializerFactory,
        ILogService logService,
        IErrorHandlerService errorHandlerService)
    {
        _serializerFactory = serializerFactory;
        _logService = logService;
        _errorHandlerService = errorHandlerService;
    }
    
    public async Task<TData> LoadDataAsync<TData>(string filePath) where TData : class
    {
        try
        {
            _logService.LogInfo($"Loading data from {filePath}", "DataFlow");
            
            var serializer = _serializerFactory.GetSerializer<TData>();
            var data = await serializer.DeserializeFromFileAsync(filePath);
            
            _logService.LogInfo($"Successfully loaded data from {filePath}", "DataFlow");
            return data;
        }
        catch (Exception ex)
        {
            await _errorHandlerService.HandleExceptionAsync(ex, $"Failed to load data from {filePath}");
            throw;
        }
    }
    
    // ... 其他方法实现
}
```

**风险缓解**：
- 逐步迁移数据流逻辑
- 保持数据一致性
- 充分测试

**成功标准**：
- 数据流管理正常
- 错误恢复机制工作
- 性能无明显下降

### 阶段3：测试架构完善（中优先级）

#### 3.1 单元测试架构

**目标**：完善单元测试框架

**任务清单**：
1. 创建测试基础设置
2. 实现ViewModel测试框架
3. 添加服务测试
4. 重构现有测试

**实施步骤**：

```csharp
// 1. 创建测试服务提供器
public static class TestServiceProvider
{
    private static IServiceProvider _serviceProvider;
    
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
            services.AddSingleton<IViewModelFactory, MockViewModelFactory>();
            
            // 注册实际的服务（用于集成测试）
            services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
            
            _serviceProvider = services.BuildServiceProvider();
        }
        
        return _serviceProvider;
    }
    
    public static void Reset()
    {
        _serviceProvider = null;
    }
}

// 2. 创建测试基类
public abstract class TestBase
{
    protected IServiceProvider ServiceProvider { get; }
    protected ILogService LogService { get; }
    protected IErrorHandlerService ErrorHandlerService { get; }
    protected IValidationService ValidationService { get; }
    protected IDataBindingService DataBindingService { get; }
    protected IEditorFactory EditorFactory { get; }
    protected IViewModelFactory ViewModelFactory { get; }
    
    protected TestBase()
    {
        ServiceProvider = TestServiceProvider.GetServiceProvider();
        LogService = ServiceProvider.GetRequiredService<ILogService>();
        ErrorHandlerService = ServiceProvider.GetRequiredService<IErrorHandlerService>();
        ValidationService = ServiceProvider.GetRequiredService<IValidationService>();
        DataBindingService = ServiceProvider.GetRequiredService<IDataBindingService>();
        EditorFactory = ServiceProvider.GetRequiredService<IEditorFactory>();
        ViewModelFactory = ServiceProvider.GetRequiredService<IViewModelFactory>();
    }
    
    [TearDown]
    public virtual void TearDown()
    {
        // 清理测试状态
    }
}

// 3. ViewModel测试基类
public abstract class ViewModelTestBase : TestBase
{
    protected TViewModel CreateViewModel<TViewModel>(params object[] parameters)
        where TViewModel : ViewModelBase
    {
        return ViewModelFactory.CreateViewModel<TViewModel>(parameters);
    }
    
    protected TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new()
    {
        return ViewModelFactory.CreateEditorViewModel<TEditorViewModel, TData, TItem>(xmlFileName, editorName);
    }
}

// 4. 具体测试示例
[TestFixture]
public class AttributeEditorViewModelTests : ViewModelTestBase
{
    [Test]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var xmlFileName = "attributes.xml";
        var editorName = "Attribute Editor";
        
        // Act
        var viewModel = CreateEditorViewModel<AttributeEditorViewModel, AttributeData, Attribute>(
            xmlFileName, editorName);
        
        // Assert
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel.XmlFileName, Is.EqualTo(xmlFileName));
        Assert.That(viewModel.EditorName, Is.EqualTo(editorName));
    }
    
    [Test]
    public async Task LoadFileAsync_WithValidFile_ShouldLoadData()
    {
        // Arrange
        var viewModel = CreateEditorViewModel<AttributeEditorViewModel, AttributeData, Attribute>(
            "attributes.xml", "Attribute Editor");
        
        // Act
        await viewModel.LoadFileAsync();
        
        // Assert
        Assert.That(viewModel.Items, Is.Not.Empty);
        Assert.That(viewModel.HasUnsavedChanges, Is.False);
    }
}
```

**风险缓解**：
- 保留现有测试
- 逐步迁移到新框架
- 确保测试覆盖率

**成功标准**：
- 测试框架正常工作
- 测试覆盖率 > 80%
- 测试运行稳定

#### 3.2 集成测试架构

**目标**：完善集成测试框架

**任务清单**：
1. 创建集成测试基类
2. 实现端到端测试
3. 添加跨平台测试
4. 重构现有集成测试

**实施步骤**：

```csharp
// 1. 创建集成测试基类
public abstract class IntegrationTestBase : TestBase
{
    protected IDataFlowManager DataFlowManager { get; }
    protected IXmlSerializerFactory SerializerFactory { get; }
    
    protected IntegrationTestBase()
    {
        DataFlowManager = ServiceProvider.GetRequiredService<IDataFlowManager>();
        SerializerFactory = ServiceProvider.GetRequiredService<IXmlSerializerFactory>();
    }
    
    protected async Task<TData> LoadTestDataAsync<TData>(string fileName) where TData : class
    {
        var testDataPath = Path.Combine("TestData", fileName);
        return await DataFlowManager.LoadDataAsync<TData>(testDataPath);
    }
    
    protected async Task<string> SaveTestDataAsync<TData>(TData data, string fileName) where TData : class
    {
        var testDataPath = Path.Combine("TestData", fileName);
        await DataFlowManager.SaveDataAsync(data, testDataPath);
        return testDataPath;
    }
}

// 2. 集成测试示例
[TestFixture]
public class EditorIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task LoadAndSaveData_ShouldPreserveDataIntegrity()
    {
        // Arrange
        var originalData = await LoadTestDataAsync<AttributeData>("attributes.xml");
        
        // Act
        await SaveTestDataAsync(originalData, "attributes_test.xml");
        var loadedData = await LoadTestDataAsync<AttributeData>("attributes_test.xml");
        
        // Assert
        Assert.That(loadedData, Is.Not.Null);
        Assert.That(loadedData.Attributes.Count, Is.EqualTo(originalData.Attributes.Count));
        
        // 验证数据完整性
        for (int i = 0; i < originalData.Attributes.Count; i++)
        {
            Assert.That(loadedData.Attributes[i].Id, Is.EqualTo(originalData.Attributes[i].Id));
            Assert.That(loadedData.Attributes[i].Name, Is.EqualTo(originalData.Attributes[i].Name));
        }
    }
}

// 3. 跨平台测试
[TestFixture]
[Platform("Win")]
public class WindowsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task WindowsPathHandling_ShouldWorkCorrectly()
    {
        // Windows特定的路径测试
        var windowsPath = @"C:\TestData\attributes.xml";
        var result = await DataFlowManager.LoadDataAsync<AttributeData>(windowsPath);
        
        Assert.That(result, Is.Not.Null);
    }
}

[TestFixture]
[Platform("Linux")]
public class LinuxIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task LinuxPathHandling_ShouldWorkCorrectly()
    {
        // Linux特定的路径测试
        var linuxPath = "/tmp/testdata/attributes.xml";
        var result = await DataFlowManager.LoadDataAsync<AttributeData>(linuxPath);
        
        Assert.That(result, Is.Not.Null);
    }
}
```

**风险缓解**：
- 创建测试数据
- 使用测试数据库
- 清理测试环境

**成功标准**：
- 集成测试正常工作
- 跨平台测试通过
- 端到端测试覆盖

#### 3.3 性能测试

**目标**：添加性能测试框架

**任务清单**：
1. 创建性能测试框架
2. 实现负载测试
3. 添加内存泄漏检测
4. 建立性能基准

**实施步骤**：

```csharp
// 1. 性能测试基类
public abstract class PerformanceTestBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly Stopwatch Stopwatch;
    
    protected PerformanceTestBase(ITestOutputHelper output)
    {
        Output = output;
        Stopwatch = new Stopwatch();
    }
    
    protected async Task MeasureAsync(string operationName, Func<Task> operation, int iterations = 100)
    {
        var times = new List<long>();
        
        for (int i = 0; i < iterations; i++)
        {
            Stopwatch.Restart();
            await operation();
            Stopwatch.Stop();
            times.Add(Stopwatch.ElapsedMilliseconds);
        }
        
        var averageTime = times.Average();
        var minTime = times.Min();
        var maxTime = times.Max();
        
        Output.WriteLine($"Performance test: {operationName}");
        Output.WriteLine($"Iterations: {iterations}");
        Output.WriteLine($"Average time: {averageTime:F2}ms");
        Output.WriteLine($"Min time: {minTime}ms");
        Output.WriteLine($"Max time: {maxTime}ms");
    }
    
    protected void MeasureMemoryUsage(string operationName, Action operation)
    {
        var initialMemory = GC.GetTotalMemory(true);
        operation();
        var finalMemory = GC.GetTotalMemory(false);
        var memoryUsed = finalMemory - initialMemory;
        
        Output.WriteLine($"Memory test: {operationName}");
        Output.WriteLine($"Memory used: {memoryUsed} bytes");
    }
}

// 2. 具体性能测试
public class EditorPerformanceTests : PerformanceTestBase
{
    public EditorPerformanceTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Test]
    public async Task LoadLargeXmlFile_Performance_ShouldBeAcceptable()
    {
        // Arrange
        var largeXmlPath = "TestData/large_attributes.xml";
        
        // Act & Assert
        await MeasureAsync("Load large XML file", async () =>
        {
            var dataFlowManager = TestServiceProvider.GetServiceProvider()
                .GetRequiredService<IDataFlowManager>();
            await dataFlowManager.LoadDataAsync<AttributeData>(largeXmlPath);
        }, 10);
    }
    
    [Test]
    public void CreateManyViewModels_MemoryUsage_ShouldBeReasonable()
    {
        // Arrange
        var viewModelFactory = TestServiceProvider.GetServiceProvider()
            .GetRequiredService<IViewModelFactory>();
        
        // Act & Assert
        MeasureMemoryUsage("Create 1000 ViewModels", () =>
        {
            for (int i = 0; i < 1000; i++)
            {
                var viewModel = viewModelFactory.CreateEditorViewModel<
                    AttributeEditorViewModel, AttributeData, Attribute>(
                    "attributes.xml", "Attribute Editor");
            }
        });
    }
}
```

**风险缓解**：
- 使用测试数据
- 监控资源使用
- 设置性能基准

**成功标准**：
- 性能测试正常工作
- 性能指标可接受
- 内存使用合理

### 阶段4：部署和监控（低优先级）

#### 4.1 CI/CD优化

**目标**：完善CI/CD流程

**任务清单**：
1. 完善GitHub Actions配置
2. 添加自动化测试
3. 实现自动部署
4. 优化构建性能

**实施步骤**：

```yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop, feature/* ]
  pull_request:
    branches: [ main, develop ]

jobs:
  test:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        dotnet-version: [9.0.x]
        
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ matrix.dotnet-version }}
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Run unit tests
      run: dotnet test BannerlordModEditor.Common.Tests --no-build --verbosity normal
      
    - name: Run UI tests
      run: dotnet test BannerlordModEditor.UI.Tests --no-build --verbosity normal
      
    - name: Run integration tests
      run: dotnet test BannerlordModEditor.IntegrationTests --no-build --verbosity normal
      
    - name: Generate coverage report
      run: |
        dotnet test --collect:"XPlat Code Coverage"
        dotnet reportgenerator -reports:coverage.xml -targetdir:coverage-report
        
    - name: Upload coverage reports
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.xml

  build:
    needs: test
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --configuration Release
      
    - name: Publish
      run: dotnet publish --configuration Release --output ./publish
      
    - name: Upload artifacts
      uses: actions/upload-artifact@v3
      with:
        name: ${{ matrix.os }}-build
        path: ./publish
```

**风险缓解**：
- 分阶段部署
- 监控部署过程
- 准备回滚计划

**成功标准**：
- CI/CD流程正常工作
- 自动化测试通过
- 部署成功

#### 4.2 监控和诊断

**目标**：完善监控和诊断系统

**任务清单**：
1. 实现诊断服务
2. 添加性能监控
3. 创建错误报告系统
4. 建立告警机制

**实施步骤**：

```csharp
// 1. 诊断服务
public class DiagnosticsService : BaseService
{
    private readonly ConfigurationService _configurationService;
    private readonly ConcurrentQueue<DiagnosticEvent> _diagnosticEvents = new();
    
    public DiagnosticsService(
        ConfigurationService configurationService,
        ILogService logService,
        IErrorHandlerService errorHandlerService) 
        : base(logService, errorHandlerService)
    {
        _configurationService = configurationService;
    }
    
    public async Task RecordEventAsync(DiagnosticEvent diagnosticEvent)
    {
        try
        {
            _diagnosticEvents.Enqueue(diagnosticEvent);
            
            // 如果启用诊断，记录到日志
            if (_configurationService.GetSetting("EnableDiagnostics", false))
            {
                LogService.LogInfo($"Diagnostic event: {diagnosticEvent.EventType} - {diagnosticEvent.Message}", "Diagnostics");
            }
            
            // 如果事件严重，立即处理
            if (diagnosticEvent.Severity >= DiagnosticSeverity.Error)
            {
                await ProcessCriticalEventAsync(diagnosticEvent);
            }
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, "Failed to record diagnostic event");
        }
    }
    
    public async Task<DiagnosticReport> GenerateReportAsync()
    {
        try
        {
            var events = await GetEventsAsync();
            
            var report = new DiagnosticReport
            {
                GeneratedAt = DateTime.UtcNow,
                TotalEvents = events.Count(),
                EventsByType = events.GroupBy(e => e.EventType).ToDictionary(g => g.Key, g => g.Count()),
                EventsBySeverity = events.GroupBy(e => e.Severity).ToDictionary(g => g.Key, g => g.Count()),
                RecentErrors = events.Where(e => e.Severity >= DiagnosticSeverity.Error).Take(10).ToList()
            };
            
            return report;
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, "Failed to generate diagnostic report");
            throw;
        }
    }
}

// 2. 性能监控
public class PerformanceMonitor
{
    private readonly ILogger<PerformanceMonitor> _logger;
    private readonly ConcurrentDictionary<string, Stopwatch> _operations = new();
    
    public PerformanceMonitor(ILogger<PerformanceMonitor> logger)
    {
        _logger = logger;
    }
    
    public void StartOperation(string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        _operations[operationName] = stopwatch;
        
        _logger.LogDebug("Started operation {OperationName}", operationName);
    }
    
    public void EndOperation(string operationName)
    {
        if (_operations.TryRemove(operationName, out var stopwatch))
        {
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInformation("Operation {OperationName} completed in {ElapsedMs}ms", 
                operationName, elapsed);
        }
    }
}
```

**风险缓解**：
- 监控系统性能
- 设置合理的告警阈值
- 定期审查监控数据

**成功标准**：
- 监控系统正常工作
- 性能指标可接受
- 错误报告及时

## 时间表

### 阶段1：核心基础设施修复（2-3周）
- 第1周：依赖注入配置统一
- 第2周：ViewModel工厂模式重构
- 第3周：服务层架构优化

### 阶段2：数据层重构（2-3周）
- 第4周：DO/DTO模式实施
- 第5周：XML序列化统一
- 第6周：数据流管理

### 阶段3：测试架构完善（2周）
- 第7周：单元测试架构
- 第8周：集成测试架构和性能测试

### 阶段4：部署和监控（1-2周）
- 第9周：CI/CD优化
- 第10周：监控和诊断

## 资源需求

### 人力资源
- **架构师**：1人，全程参与
- **开发人员**：2-3人，根据阶段需要
- **测试人员**：1人，参与测试架构完善
- **运维人员**：1人，参与部署和监控

### 技术资源
- **开发环境**：Visual Studio 2022, .NET 9.0
- **测试工具**：xUnit, Moq, Coverlet
- **CI/CD工具**：GitHub Actions, Docker
- **监控工具**：Application Insights, ELK Stack

### 硬件资源
- **开发服务器**：至少8GB RAM, 4核CPU
- **测试服务器**：至少16GB RAM, 8核CPU
- **生产服务器**：根据实际需求配置

## 风险管理

### 风险识别
1. **技术风险**：新架构可能影响现有功能
2. **时间风险**：重构可能超出预期时间
3. **资源风险**：人力资源可能不足
4. **质量风险**：重构可能引入新的bug

### 风险缓解
1. **技术风险**：采用渐进式重构，保持向后兼容性
2. **时间风险**：制定详细计划，设置缓冲时间
3. **资源风险**：合理分配资源，必要时寻求外部支持
4. **质量风险**：加强测试，确保代码质量

### 应急计划
1. **回滚计划**：每个阶段都有明确的回滚点
2. **备份计划**：定期备份代码和数据
3. **沟通计划**：建立有效的沟通机制
4. **支持计划**：确保技术支持及时响应

## 成功标准

### 技术指标
- **代码覆盖率**：单元测试覆盖率 > 80%
- **构建时间**：CI/CD构建时间 < 10分钟
- **启动时间**：应用程序启动时间 < 5秒
- **内存使用**：内存使用量 < 100MB

### 业务指标
- **用户满意度**：用户满意度评分 > 4.5/5
- **错误率**：生产环境错误率 < 1%
- **功能完整性**：所有核心功能正常工作
- **性能指标**：响应时间 < 200ms

### 维护指标
- **代码质量**：代码质量评分 > 8/10
- **技术债务**：技术债务评分 > 7/10
- **文档完整性**：API文档覆盖率 > 90%
- **团队生产力**：开发效率提升 > 20%

## 结论

本实施计划提供了一个全面的架构修复方案，通过分阶段实施，我们可以确保系统的稳定性和团队的适应性。关键在于：

1. **渐进式重构**：避免大规模重写，保持系统稳定
2. **测试驱动**：确保重构质量，防止引入新的问题
3. **风险控制**：每个阶段都有明确的风险控制和应急计划
4. **持续改进**：根据实施过程中的反馈不断优化

通过这个实施计划，我们可以成功解决Bannerlord ModEditor项目中的架构问题，构建一个更加稳定、可维护和可扩展的系统。