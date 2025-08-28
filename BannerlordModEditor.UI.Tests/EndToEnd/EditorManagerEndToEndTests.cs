using System;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BannerlordModEditor.UI.Tests.EndToEnd;

/// <summary>
/// EditorManagerViewModel端到端功能测试
/// </summary>
public class EditorManagerEndToEndTests
{
    [Fact]
    public void CompleteWorkflow_DefaultFactoryToEditorSelection_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();

        // Act
        var categories = editorManager.Categories;
        var characterCategory = categories.FirstOrDefault(c => c.Name == "角色设定");
        var attributeEditor = characterCategory?.Editors.FirstOrDefault(e => e.Name == "属性定义");

        // 选择编辑器
        if (attributeEditor != null)
        {
            editorManager.SelectEditorCommand.Execute(attributeEditor);
        }

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotEmpty(categories);
        Assert.NotNull(characterCategory);
        Assert.NotNull(attributeEditor);
        Assert.Equal(attributeEditor, editorManager.SelectedEditor);
        Assert.NotNull(editorManager.CurrentEditorViewModel);
        Assert.NotNull(editorManager.CurrentBreadcrumb);
        Assert.Contains("属性定义", editorManager.CurrentBreadcrumb);
    }

    [Fact]
    public async Task CompleteWorkflow_AsyncFactoryCreation_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var editorManager = await factory.CreateEditorManagerAsync();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotEmpty(editorManager.Categories);
        Assert.True(editorManager.Categories.Count > 0);
    }

    [Fact]
    public void CompleteWorkflow_WithCustomOptions_ShouldApplyAllOptions()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var options = new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = true,
            EnableDiagnostics = true,
            EnablePostCreationConfiguration = true,
            CreationTimeout = 60000
        };

        // Act
        var editorManager = factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotEmpty(editorManager.Categories);
        // 验证工厂统计信息
        var stats = factory.GetStatistics();
        Assert.Equal(1, stats.InstancesCreated);
        Assert.True(stats.LastCreationTime > DateTime.MinValue);
    }

    [Fact]
    public void CompleteWorkflow_WithSearchAndSelection_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();

        // Act
        // 搜索属性编辑器
        editorManager.SearchText = "属性";
        
        // 找到并选择属性编辑器
        var visibleEditors = editorManager.Categories
            .SelectMany(c => c.Editors)
            .Where(e => e.IsAvailable)
            .ToList();
        
        var attributeEditor = visibleEditors.FirstOrDefault(e => e.Name.Contains("属性"));
        
        if (attributeEditor != null)
        {
            editorManager.SelectEditorCommand.Execute(attributeEditor);
        }

        // 清空搜索
        editorManager.SearchText = "";

        // Assert
        Assert.NotNull(attributeEditor);
        Assert.Equal(attributeEditor, editorManager.SelectedEditor);
        Assert.NotNull(editorManager.CurrentEditorViewModel);
        
        // 验证所有编辑器在清空搜索后都可见
        var allEditors = editorManager.Categories.SelectMany(c => c.Editors);
        Assert.All(allEditors, e => Assert.True(e.IsAvailable));
    }

    [Fact]
    public void CompleteWorkflow_WithRefresh_ShouldReloadEditors()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();
        var initialCategoryCount = editorManager.Categories.Count;

        // Act
        editorManager.RefreshEditorsCommand.Execute(null);

        // Assert
        Assert.Equal(initialCategoryCount, editorManager.Categories.Count);
        Assert.Contains("已加载", editorManager.StatusMessage);
    }

    [Fact]
    public void CompleteWorkflow_WithErrorHandling_ShouldHandleGracefully()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();

        // Act
        // 尝试显示帮助
        editorManager.ShowHelpCommand.Execute(null);

        // Assert
        Assert.Equal("查看帮助信息", editorManager.StatusMessage);
    }

    [Fact]
    public void CompleteWorkflow_WithMultipleEditorSelections_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();

        // Act
        var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");
        var attributeEditor = characterCategory?.Editors.FirstOrDefault(e => e.Name == "属性定义");
        var skillEditor = characterCategory?.Editors.FirstOrDefault(e => e.Name == "技能系统");

        // 选择属性编辑器
        if (attributeEditor != null)
        {
            editorManager.SelectEditorCommand.Execute(attributeEditor);
        }

        var firstSelection = editorManager.SelectedEditor;
        var firstBreadcrumb = editorManager.CurrentBreadcrumb;

        // 选择技能编辑器
        if (skillEditor != null)
        {
            editorManager.SelectEditorCommand.Execute(skillEditor);
        }

        var secondSelection = editorManager.SelectedEditor;
        var secondBreadcrumb = editorManager.CurrentBreadcrumb;

        // Assert
        Assert.Equal(attributeEditor, firstSelection);
        Assert.Equal(skillEditor, secondSelection);
        Assert.Contains("属性定义", firstBreadcrumb);
        Assert.Contains("技能系统", secondBreadcrumb);
        Assert.NotEqual(firstSelection, secondSelection);
        Assert.NotEqual(firstBreadcrumb, secondBreadcrumb);
    }

    [Fact]
    public void CompleteWorkflow_WithDependencyInjection_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();

        // Act
        var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(factory);
        Assert.NotEmpty(editorManager.Categories);
        
        // 验证服务是否正确注入
        var logService = serviceProvider.GetRequiredService<ILogService>();
        var errorHandlerService = serviceProvider.GetRequiredService<IErrorHandlerService>();
        var validationService = serviceProvider.GetRequiredService<IValidationService>();
        
        Assert.NotNull(logService);
        Assert.NotNull(errorHandlerService);
        Assert.NotNull(validationService);
    }

    [Fact]
    public void CompleteWorkflow_WithServiceCollectionExtensions_ShouldWorkCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(factory);
        Assert.NotEmpty(editorManager.Categories);
        
        // 验证所有必需的服务都已注册
        var validationResult = services.ValidateEditorManagerServices();
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public void CompleteWorkflow_WithMinimalServices_ShouldWorkCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMinimalEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(factory);
        Assert.NotEmpty(editorManager.Categories);
        
        // 验证只有核心服务已注册
        var validationResult = services.ValidateEditorManagerServices();
        Assert.True(validationResult.IsValid);
        Assert.True(validationResult.IsLogServiceRegistered);
        Assert.True(validationResult.IsErrorHandlerServiceRegistered);
        Assert.False(validationResult.IsValidationServiceRegistered); // 最小配置不包含验证服务
    }

    [Fact]
    public void CompleteWorkflow_WithFullServices_ShouldWorkCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFullEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(factory);
        Assert.NotEmpty(editorManager.Categories);
        
        // 验证所有服务都已注册
        var validationResult = services.ValidateEditorManagerServices();
        Assert.True(validationResult.IsValid);
        Assert.True(validationResult.IsLogServiceRegistered);
        Assert.True(validationResult.IsErrorHandlerServiceRegistered);
        Assert.True(validationResult.IsValidationServiceRegistered);
        Assert.True(validationResult.IsDataBindingServiceRegistered);
    }

    [Fact]
    public void CompleteWorkflow_WithConfigurationValidation_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var editorManager = factory.CreateEditorManager();

        // Assert
        Assert.NotNull(editorManager);
        
        // 验证配置
        var options = new EditorManagerOptions
        {
            LogService = serviceProvider.GetRequiredService<ILogService>(),
            ErrorHandlerService = serviceProvider.GetRequiredService<IErrorHandlerService>(),
            ValidationService = serviceProvider.GetRequiredService<IValidationService>(),
            EditorFactory = serviceProvider.GetRequiredService<IEditorFactory>(),
            ServiceProvider = serviceProvider
        };

        var validationResult = options.Validate();
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public async Task CompleteWorkflow_WithStatisticsTracking_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var editorManager1 = factory.CreateEditorManager();
        var editorManager2 = factory.CreateEditorManager();
        var editorManager3 = await factory.CreateEditorManagerAsync();

        var stats = factory.GetStatistics();

        // Assert
        Assert.NotNull(editorManager1);
        Assert.NotNull(editorManager2);
        Assert.NotNull(editorManager3);
        Assert.Equal(3, stats.InstancesCreated);
        Assert.True(stats.LastCreationTime > DateTime.MinValue);
        Assert.False(stats.IsCreationInProgress);
    }

    [Fact]
    public void CompleteWorkflow_WithThreadSafety_ShouldWorkCorrectly()
    {
        // Arrange
        var serviceProvider = CreateCompleteServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var tasks = Enumerable.Range(0, 10).Select(async i =>
        {
            await Task.Delay(10); // 模拟异步操作
            return factory.CreateEditorManager();
        });

        var results = Task.WhenAll(tasks).Result;

        // Assert
        Assert.Equal(10, results.Length);
        Assert.All(results, result => Assert.NotNull(result));
        
        var stats = factory.GetStatistics();
        Assert.Equal(10, stats.InstancesCreated);
    }

    private IServiceProvider CreateCompleteServiceProvider()
    {
        var services = new ServiceCollection();
        
        // 注册核心服务
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IEditorFactory, MockEditorFactory>();
        
        // 注册EditorManager服务
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>();
        
        return services.BuildServiceProvider();
    }

    private class MockEditorFactory : IEditorFactory
    {
        public ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
        {
            return new MockEditorViewModel(editorType, xmlFileName);
        }

        public BannerlordModEditor.UI.Views.Editors.BaseEditorView? CreateEditorView(string editorType)
        {
            return null;
        }

        public void RegisterEditor<TViewModel, TView>(string editorType)
            where TViewModel : ViewModelBase
            where TView : BannerlordModEditor.UI.Views.Editors.BaseEditorView
        {
        }

        public IEnumerable<string> GetRegisteredEditorTypes()
        {
            return new[] { "AttributeEditor", "SkillEditor", "CombatParameterEditor" };
        }

        public EditorTypeInfo? GetEditorTypeInfo(string editorType)
        {
            return new EditorTypeInfo
            {
                EditorType = editorType,
                DisplayName = $"{editorType} Display",
                Category = "测试分类"
            };
        }

        public IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category)
        {
            return Enumerable.Empty<EditorTypeInfo>();
        }

        public IEnumerable<string> GetCategories()
        {
            return new[] { "测试分类" };
        }

        public void RegisterEditor<TViewModel, TView>(string editorType, string displayName, string description, string xmlFileName, string category = "General")
            where TViewModel : ViewModelBase
            where TView : BannerlordModEditor.UI.Views.Editors.BaseEditorView
        {
        }

        public IEnumerable<ViewModelBase> GetAllEditors()
        {
            return new List<ViewModelBase>
            {
                new MockEditorViewModel("AttributeEditor", "attributes.xml"),
                new MockEditorViewModel("SkillEditor", "skills.xml"),
                new MockEditorViewModel("CombatParameterEditor", "combat_parameters.xml")
            };
        }
    }

    private class MockEditorViewModel : ViewModelBase
    {
        public new string EditorType { get; }
        public new string XmlFileName { get; }

        public MockEditorViewModel(string editorType, string xmlFileName)
        {
            EditorType = editorType;
            XmlFileName = xmlFileName;
        }
    }
}