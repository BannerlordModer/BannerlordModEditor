# CI/CD修复架构设计

## 执行摘要

本文档详细说明了如何修复BannerlordModEditor项目的CI/CD问题。主要问题包括：
1. **接口定义不完整**：IEditorFactory接口存在但定义不完善
2. **工厂模式冲突**：EditorFactory和EnhancedEditorFactory同时存在
3. **服务注册问题**：依赖注入配置不完整
4. **缺失类型和引用**：多个ViewModel和View类型缺失
5. **测试环境配置**：测试服务注册不完整

## 问题分析

### 当前状态

根据代码分析，发现以下核心问题：

#### 1. 工厂模式冲突
- `EditorFactory.cs`：基础工厂实现，包含基本功能
- `EnhancedEditorFactory.cs`：增强工厂实现，包含反射和高级功能
- 两个类都实现了`IEditorFactory`接口，但功能有重叠

#### 2. 接口定义不完整
```csharp
// 当前IEditorFactory接口
public interface IEditorFactory
{
    ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName);
    BaseEditorView? CreateEditorView(string editorType);
    void RegisterEditor<TViewModel, TView>(string editorType);
    IEnumerable<string> GetRegisteredEditorTypes();
}
```

#### 3. 服务注册问题
- 缺少必要的UI服务注册
- 测试环境中的服务提供器配置不完整
- 缺少ViewModel和View类型的注册

#### 4. 缺失类型
- 多个编辑器View类型缺失
- 部分ViewModel类型引用问题
- 服务接口实现不完整

## 修复架构设计

### 1. 统一工厂模式

#### 1.1 工厂接口完善
```csharp
/// <summary>
/// 统一的编辑器工厂接口
/// </summary>
public interface IEditorFactory
{
    // 基础功能
    ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName);
    BaseEditorView? CreateEditorView(string editorType);
    void RegisterEditor<TViewModel, TView>(string editorType);
    IEnumerable<string> GetRegisteredEditorTypes();
    
    // 增强功能
    EditorTypeInfo? GetEditorTypeInfo(string editorType);
    IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category);
    IEnumerable<string> GetCategories();
    void RegisterEditor<TViewModel, TView>(string editorType, string displayName, string description, string xmlFileName, string category = "General");
    
    // 反射功能
    void RegisterEditorsByReflection();
    void RegisterEditorsFromAssembly(Assembly assembly);
}
```

#### 1.2 工厂实现统一
```csharp
/// <summary>
/// 统一的编辑器工厂实现
/// </summary>
public class UnifiedEditorFactory : IEditorFactory
{
    private readonly Dictionary<string, EditorTypeInfo> _editorTypes = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;
    
    public UnifiedEditorFactory(
        IServiceProvider serviceProvider,
        IValidationService validationService,
        IDataBindingService dataBindingService)
    {
        _serviceProvider = serviceProvider;
        _validationService = validationService;
        _dataBindingService = dataBindingService;
        
        RegisterDefaultEditors();
        RegisterEditorsByReflection();
    }
    
    // 实现所有接口方法...
}
```

### 2. 服务注册架构

#### 2.1 服务注册扩展
```csharp
/// <summary>
/// 编辑器服务注册扩展
/// </summary>
public static class EditorServiceExtensions
{
    /// <summary>
    /// 注册编辑器核心服务
    /// </summary>
    public static IServiceCollection AddEditorServices(this IServiceCollection services)
    {
        // 注册工厂
        services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
        
        // 注册UI服务
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<ILogService, LogService>();
        
        // 注册所有编辑器ViewModel
        services.RegisterEditorViewModels();
        
        // 注册所有编辑器View
        services.RegisterEditorViews();
        
        return services;
    }
    
    /// <summary>
    /// 注册编辑器ViewModel
    /// </summary>
    public static IServiceCollection RegisterEditorViewModels(this IServiceCollection services)
    {
        // 基础编辑器
        services.AddTransient<AttributeEditorViewModel>();
        services.AddTransient<SkillEditorViewModel>();
        services.AddTransient<BoneBodyTypeEditorViewModel>();
        services.AddTransient<CraftingPieceEditorViewModel>();
        services.AddTransient<ItemModifierEditorViewModel>();
        
        // 通用编辑器
        services.AddTransient<GenericEditorViewModel>();
        services.AddTransient<SimpleEditorViewModel>();
        
        // 新增编辑器
        services.AddTransient<CombatParameterEditorViewModel>();
        services.AddTransient<ItemEditorViewModel>();
        
        return services;
    }
    
    /// <summary>
    /// 注册编辑器View
    /// </summary>
    public static IServiceCollection RegisterEditorViews(this IServiceCollection services)
    {
        // 基础编辑器View
        services.AddTransient<AttributeEditorView>();
        services.AddTransient<SkillEditorView>();
        services.AddTransient<BoneBodyTypeEditorView>();
        services.AddTransient<CraftingPieceEditorView>();
        services.AddTransient<ItemModifierEditorView>();
        
        // 新增编辑器View
        services.AddTransient<CombatParameterEditorView>();
        services.AddTransient<ItemEditorView>();
        
        return services;
    }
}
```

#### 2.2 测试服务注册
```csharp
/// <summary>
/// 测试服务注册扩展
/// </summary>
public static class TestServiceExtensions
{
    /// <summary>
    /// 注册测试服务
    /// </summary>
    public static IServiceCollection AddTestServices(this IServiceCollection services)
    {
        // 注册编辑器服务
        services.AddEditorServices();
        
        // 注册Common层服务
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        
        // 注册测试替身
        services.AddTransient<IValidationService>(sp => 
            new MockValidationService());
        services.AddTransient<IDataBindingService>(sp => 
            new MockDataBindingService());
        
        return services;
    }
}
```

### 3. 缺失类型补全

#### 3.1 缺失的View类型
```csharp
/// <summary>
/// 属性编辑器视图
/// </summary>
public partial class AttributeEditorView : BaseEditorView
{
    public AttributeEditorView()
    {
        InitializeComponent();
    }
}

/// <summary>
/// 技能编辑器视图
/// </summary>
public partial class SkillEditorView : BaseEditorView
{
    public SkillEditorView()
    {
        InitializeComponent();
    }
}

/// <summary>
/// 骨骼类型编辑器视图
/// </summary>
public partial class BoneBodyTypeEditorView : BaseEditorView
{
    public BoneBodyTypeEditorView()
    {
        InitializeComponent();
    }
}

/// <summary>
/// 制作件编辑器视图
/// </summary>
public partial class CraftingPieceEditorView : BaseEditorView
{
    public CraftingPieceEditorView()
    {
        InitializeComponent();
    }
}

/// <summary>
/// 物品修饰器编辑器视图
/// </summary>
public partial class ItemModifierEditorView : BaseEditorView
{
    public ItemModifierEditorView()
    {
        InitializeComponent();
    }
}
```

#### 3.2 基础编辑器视图
```csharp
/// <summary>
/// 基础编辑器视图
/// </summary>
public class BaseEditorView : UserControl
{
    public BaseEditorView()
    {
        // 基础初始化
    }
    
    /// <summary>
    /// 获取关联的ViewModel
    /// </summary>
    public ViewModelBase? ViewModel => DataContext as ViewModelBase;
}
```

### 4. 测试策略

#### 4.1 测试架构
```csharp
/// <summary>
/// 编辑器工厂测试基类
/// </summary>
public abstract class EditorFactoryTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected IEditorFactory EditorFactory { get; private set; }
    
    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTestServices();
        
        ServiceProvider = services.BuildServiceProvider();
        EditorFactory = ServiceProvider.GetRequiredService<IEditorFactory>();
    }
    
    [TearDown]
    public void TearDown()
    {
        ServiceProvider?.Dispose();
    }
}
```

#### 4.2 集成测试
```csharp
/// <summary>
/// 编辑器工厂集成测试
/// </summary>
public class EditorFactoryIntegrationTests : EditorFactoryTestBase
{
    [Test]
    public void CreateEditorViewModel_ShouldCreateAllRegisteredEditors()
    {
        // Arrange
        var editorTypes = EditorFactory.GetRegisteredEditorTypes();
        
        // Act & Assert
        foreach (var editorType in editorTypes)
        {
            var viewModel = EditorFactory.CreateEditorViewModel(editorType, "test.xml");
            Assert.IsNotNull(viewModel, $"Failed to create {editorType}");
        }
    }
    
    [Test]
    public void CreateEditorView_ShouldCreateAllRegisteredViews()
    {
        // Arrange
        var editorTypes = EditorFactory.GetRegisteredEditorTypes();
        
        // Act & Assert
        foreach (var editorType in editorTypes)
        {
            var view = EditorFactory.CreateEditorView(editorType);
            Assert.IsNotNull(view, $"Failed to create {editorType} view");
        }
    }
    
    [Test]
    public void GetEditorTypeInfo_ShouldReturnCorrectInformation()
    {
        // Arrange
        const string editorType = "AttributeEditor";
        
        // Act
        var editorInfo = EditorFactory.GetEditorTypeInfo(editorType);
        
        // Assert
        Assert.IsNotNull(editorInfo);
        Assert.AreEqual(editorType, editorInfo.EditorType);
        Assert.AreEqual("AttributeEditor", editorInfo.DisplayName);
    }
}
```

### 5. 质量保证

#### 5.1 验证机制
```csharp
/// <summary>
/// 编辑器工厂验证器
/// </summary>
public class EditorFactoryValidator
{
    private readonly IEditorFactory _factory;
    
    public EditorFactoryValidator(IEditorFactory factory)
    {
        _factory = factory;
    }
    
    /// <summary>
    /// 验证所有编辑器是否能正常创建
    /// </summary>
    public ValidationResult ValidateAllEditors()
    {
        var result = new ValidationResult();
        
        var editorTypes = _factory.GetRegisteredEditorTypes();
        foreach (var editorType in editorTypes)
        {
            try
            {
                var viewModel = _factory.CreateEditorViewModel(editorType, "test.xml");
                var view = _factory.CreateEditorView(editorType);
                
                if (viewModel == null)
                {
                    result.Errors.Add($"Failed to create {editorType} ViewModel");
                }
                
                if (view == null)
                {
                    result.Errors.Add($"Failed to create {editorType} View");
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Exception creating {editorType}: {ex.Message}");
            }
        }
        
        return result;
    }
    
    /// <summary>
    /// 验证服务依赖
    /// </summary>
    public ValidationResult ValidateServiceDependencies()
    {
        var result = new ValidationResult();
        
        // 验证必要的服务是否已注册
        var requiredServices = new[]
        {
            typeof(IEditorFactory),
            typeof(IValidationService),
            typeof(IDataBindingService),
            typeof(IErrorHandlerService),
            typeof(ILogService)
        };
        
        foreach (var serviceType in requiredServices)
        {
            try
            {
                var service = _factory.GetType()
                    .GetField("_serviceProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                    .GetValue(_factory) as IServiceProvider;
                
                if (service?.GetService(serviceType) == null)
                {
                    result.Errors.Add($"Service {serviceType.Name} is not registered");
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error validating {serviceType.Name}: {ex.Message}");
            }
        }
        
        return result;
    }
}
```

#### 5.2 自动化验证
```csharp
/// <summary>
/// 构建验证测试
/// </summary>
[TestFixture]
public class BuildValidationTests
{
    [Test]
    public void ValidateEditorFactory_ShouldPassAllChecks()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEditorServices();
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorFactory>();
        var validator = new EditorFactoryValidator(factory);
        
        // Act
        var editorValidation = validator.ValidateAllEditors();
        var serviceValidation = validator.ValidateServiceDependencies();
        
        // Assert
        Assert.IsEmpty(editorValidation.Errors, 
            $"Editor validation failed: {string.Join(", ", editorValidation.Errors)}");
        Assert.IsEmpty(serviceValidation.Errors, 
            $"Service validation failed: {string.Join(", ", serviceValidation.Errors)}");
    }
}
```

## 实施计划

### 阶段1：接口和工厂统一
1. **完善IEditorFactory接口**
2. **创建UnifiedEditorFactory**
3. **迁移EditorFactory功能**
4. **迁移EnhancedEditorFactory功能**

### 阶段2：服务注册修复
1. **创建EditorServiceExtensions**
2. **完善服务注册**
3. **创建测试服务注册**
4. **验证服务依赖**

### 阶段3：缺失类型补全
1. **创建缺失的View类型**
2. **完善ViewModel引用**
3. **添加必要的接口实现**
4. **验证类型完整性**

### 阶段4：测试优化
1. **创建测试基类**
2. **完善集成测试**
3. **添加验证机制**
4. **优化测试覆盖率**

### 阶段5：质量保证
1. **实现构建验证**
2. **添加自动化检查**
3. **完善错误处理**
4. **文档和示例**

## 风险评估

### 高风险项
1. **破坏性变更**：统一工厂可能影响现有代码
2. **依赖冲突**：服务注册可能产生循环依赖
3. **性能影响**：反射注册可能影响启动性能

### 缓解措施
1. **渐进式迁移**：保持向后兼容性
2. **依赖验证**：添加依赖检查机制
3. **性能优化**：使用缓存和延迟加载

## 预期成果

### 技术成果
1. **统一的工厂模式**：消除代码重复
2. **完整的服务注册**：解决依赖注入问题
3. **完善的测试覆盖**：提高代码质量
4. **自动化验证**：确保构建稳定性

### 业务成果
1. **CI/CD通过**：所有测试和构建成功
2. **开发效率提升**：统一的编辑器创建接口
3. **可维护性改善**：清晰的架构分层
4. **扩展性增强**：易于添加新编辑器类型

## 总结

这个修复架构设计提供了一个系统性的解决方案，通过统一工厂模式、完善服务注册、补全缺失类型和优化测试策略，全面解决CI/CD问题。该方案保持了向后兼容性，同时提供了更好的扩展性和可维护性。