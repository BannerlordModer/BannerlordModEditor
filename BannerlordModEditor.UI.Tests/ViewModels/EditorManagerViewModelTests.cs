using BannerlordModEditor.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Factories;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BannerlordModEditor.UI.Tests.ViewModels;

/// <summary>
/// EditorManagerViewModel单元测试
/// </summary>
public class EditorManagerViewModelTests
{
    private readonly Mock<ILogService> _mockLogService;
    private readonly Mock<IErrorHandlerService> _mockErrorHandlerService;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<IDataBindingService> _mockDataBindingService;
    private readonly Mock<IEditorFactory> _mockEditorFactory;
    private readonly Mock<IServiceProvider> _mockServiceProvider;

    public EditorManagerViewModelTests()
    {
        _mockLogService = new Mock<ILogService>();
        _mockErrorHandlerService = new Mock<IErrorHandlerService>();
        _mockValidationService = new Mock<IValidationService>();
        _mockDataBindingService = new Mock<IDataBindingService>();
        _mockEditorFactory = new Mock<IEditorFactory>();
        _mockServiceProvider = new Mock<IServiceProvider>();

        // 设置验证服务返回有效结果
        _mockValidationService.Setup(x => x.Validate(It.IsAny<HealthCheckTestObject>()))
            .Returns(new ValidationResult { IsValid = true });

        // 设置编辑器工厂返回一些类型
        _mockEditorFactory.Setup(x => x.GetRegisteredEditorTypes())
            .Returns(new[] { "AttributeEditor", "SkillEditor", "CombatParameterEditor" });
    }

    [Fact]
    public void Constructor_WithDefaultOptions_ShouldInitializeSuccessfully()
    {
        // Arrange
        var options = EditorManagerOptions.ForTesting();
        options.LogService = _mockLogService.Object;
        options.ErrorHandlerService = _mockErrorHandlerService.Object;
        options.ValidationService = _mockValidationService.Object;

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Categories);
        Assert.True(viewModel.Categories.Count > 0);
        _mockLogService.Verify(x => x.LogInfo("EditorManagerViewModel initialized successfully", "EditorManagerViewModel"), Times.Once);
    }

    [Fact]
    public void Constructor_WithFullServices_ShouldUseAllServices()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            DataBindingService = _mockDataBindingService.Object,
            EditorFactory = _mockEditorFactory.Object,
            ServiceProvider = _mockServiceProvider.Object
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogInfo("EditorManagerViewModel initialized successfully", "EditorManagerViewModel"), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldUseDefaultOptions()
    {
        // Act
        var viewModel = new EditorManagerViewModel(null);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Categories);
        Assert.True(viewModel.Categories.Count > 0);
    }

    [Fact]
    public void Constructor_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null, // 无效配置
            ErrorHandlerService = null,
            StrictMode = true
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new EditorManagerViewModel(options));
    }

    [Fact]
    public void Constructor_WithWarnings_ShouldLogWarnings()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = null, // 会产生警告
            EditorFactory = null, // 会产生警告
            ServiceProvider = null // 会产生警告
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogWarning("ValidationService is not available - some features may not work", "EditorManagerViewModel.Constructor"), Times.Once);
        _mockLogService.Verify(x => x.LogWarning("EditorFactory is not available - using default editors only", "EditorManagerViewModel.Constructor"), Times.Once);
        _mockLogService.Verify(x => x.LogWarning("ServiceProvider is not available - dependency injection may not work properly", "EditorManagerViewModel.Constructor"), Times.Once);
    }

    [Fact]
    public void Constructor_WithPerformanceMonitoringEnabled_ShouldInitializeMonitoring()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EnablePerformanceMonitoring = true
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogDebug("Performance monitoring initialized", "EditorManagerViewModel.Performance"), Times.Once);
    }

    [Fact]
    public void Constructor_WithHealthChecksEnabled_ShouldPerformHealthChecks()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            EditorFactory = _mockEditorFactory.Object,
            EnableHealthChecks = true
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogDebug("Testing log service", "EditorManagerViewModel.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Editor factory has 3 registered types", "EditorManagerViewModel.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Validation service test: True", "EditorManagerViewModel.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogInfo("All health checks passed", "EditorManagerViewModel"), Times.Once);
    }

    [Fact]
    public void Constructor_WithHealthChecksFailedAndStrictMode_ShouldThrowException()
    {
        // Arrange
        _mockErrorHandlerService.Setup(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ErrorSeverity>()))
            .Throws(new Exception("Test exception"));

        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            EditorFactory = _mockEditorFactory.Object,
            EnableHealthChecks = true,
            StrictMode = true
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new EditorManagerViewModel(options));
    }

    [Fact]
    public void Constructor_WithHealthChecksFailedAndNotStrictMode_ShouldLogWarning()
    {
        // Arrange
        _mockErrorHandlerService.Setup(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ErrorSeverity>()))
            .Throws(new Exception("Test exception"));

        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            EditorFactory = _mockEditorFactory.Object,
            EnableHealthChecks = true,
            StrictMode = false
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogException(It.IsAny<Exception>(), "Health check failed"), Times.Once);
    }

    [Fact]
    public void Constructor_WithDiagnosticsEnabled_ShouldLogDiagnosticInformation()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EnableDiagnostics = true
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        _mockLogService.Verify(x => x.LogDebug("Performance monitoring: False", "EditorManagerViewModel"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Health checks: False", "EditorManagerViewModel"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Diagnostics: True", "EditorManagerViewModel"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Strict mode: False", "EditorManagerViewModel"), Times.Once);
    }

    [Fact]
    public void LoadEditors_WithValidFactory_ShouldLoadEditorsFromFactory()
    {
        // Arrange
        var mockEditors = new List<ViewModelBase>
        {
            new AttributeEditorViewModel(_mockValidationService.Object),
            new SkillEditorViewModel(_mockValidationService.Object)
        };

        _mockEditorFactory.Setup(x => x.GetAllEditors())
            .Returns(mockEditors);

        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EditorFactory = _mockEditorFactory.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.RefreshEditorsCommand.Execute(null);

        // Assert
        Assert.NotNull(viewModel.Categories);
        Assert.True(viewModel.Categories.Count > 0);
        Assert.Equal($"已加载 {mockEditors.Count} 个编辑器", viewModel.StatusMessage);
    }

    [Fact]
    public void LoadEditors_WithFactoryReturningNull_ShouldLoadDefaultEditors()
    {
        // Arrange
        _mockEditorFactory.Setup(x => x.GetAllEditors())
            .Returns((IEnumerable<ViewModelBase>)null!);

        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EditorFactory = _mockEditorFactory.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.RefreshEditorsCommand.Execute(null);

        // Assert
        Assert.NotNull(viewModel.Categories);
        Assert.True(viewModel.Categories.Count > 0);
        Assert.Contains(viewModel.Categories, c => c.Name == "角色设定");
    }

    [Fact]
    public void LoadEditors_WithFactoryThrowingException_ShouldHandleError()
    {
        // Arrange
        _mockEditorFactory.Setup(x => x.GetAllEditors())
            .Throws<Exception>();

        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EditorFactory = _mockEditorFactory.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.RefreshEditorsCommand.Execute(null);

        // Assert
        Assert.NotNull(viewModel.Categories);
        Assert.Equal("加载编辑器失败", viewModel.StatusMessage);
        _mockErrorHandlerService.Verify(x => x.HandleError(It.IsAny<Exception>(), "加载编辑器失败"), Times.Once);
    }

    [Fact]
    public void SearchText_WithValidSearchText_ShouldFilterEditors()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.SearchText = "属性";

        // Assert
        Assert.NotNull(viewModel.Categories);
        var visibleEditors = viewModel.Categories.SelectMany(c => c.Editors).Where(e => e.IsAvailable);
        var hiddenEditors = viewModel.Categories.SelectMany(c => c.Editors).Where(e => !e.IsAvailable);
        
        // 根据实际测试数据验证过滤结果
        Assert.True(visibleEditors.All(e => e.Name.Contains("属性") || e.Description.Contains("属性")));
    }

    [Fact]
    public void SearchText_WithEmptySearchText_ShouldShowAllEditors()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.SearchText = "";
        viewModel.SearchText = null;

        // Assert
        Assert.NotNull(viewModel.Categories);
        var allEditors = viewModel.Categories.SelectMany(c => c.Editors);
        Assert.True(allEditors.All(e => e.IsAvailable));
    }

    [Fact]
    public void SelectEditorCommand_WithValidEditor_ShouldUpdateCurrentEditor()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        var editorItem = new EditorItemViewModel("属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️");

        // Act
        viewModel.SelectEditorCommand.Execute(editorItem);

        // Assert
        Assert.Equal(editorItem, viewModel.SelectedEditor);
        Assert.NotNull(viewModel.CurrentEditorViewModel);
        Assert.NotNull(viewModel.CurrentBreadcrumb);
        Assert.Contains("属性定义", viewModel.CurrentBreadcrumb);
        Assert.Equal("已选择编辑器: AttributeEditorViewModel", viewModel.StatusMessage);
    }

    [Fact]
    public void SelectEditorCommand_WithException_ShouldHandleError()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        var invalidEditorItem = new EditorItemViewModel("无效编辑器", "无效编辑器描述", "invalid.xml", "InvalidEditor", "❌");

        // Act
        viewModel.SelectEditorCommand.Execute(invalidEditorItem);

        // Assert
        Assert.Equal("选择编辑器失败", viewModel.StatusMessage);
        _mockErrorHandlerService.Verify(x => x.HandleError(It.IsAny<Exception>(), "选择编辑器失败"), Times.Once);
    }

    [Fact]
    public void RefreshEditorsCommand_ShouldReloadEditors()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EditorFactory = _mockEditorFactory.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.RefreshEditorsCommand.Execute(null);

        // Assert
        Assert.NotNull(viewModel.Categories);
        _mockEditorFactory.Verify(x => x.GetAllEditors(), Times.AtLeastOnce);
    }

    [Fact]
    public void ShowHelpCommand_ShouldUpdateStatusMessage()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);

        // Act
        viewModel.ShowHelpCommand.Execute(null);

        // Assert
        Assert.Equal("查看帮助信息", viewModel.StatusMessage);
    }

    [Fact]
    public void SelectedEditor_WhenChanged_ShouldUpdateCurrentEditorViewModel()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        var editor = new AttributeEditorViewModel(_mockValidationService.Object);

        // Act
        viewModel.SelectedEditor = editor;

        // Assert
        Assert.Equal(editor, viewModel.CurrentEditorViewModel);
        Assert.NotNull(viewModel.CurrentBreadcrumb);
    }

    [Fact]
    public void SelectedEditor_WhenSetToNull_ShouldClearCurrentEditorViewModel()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        var editor = new AttributeEditorViewModel(_mockValidationService.Object);
        viewModel.SelectedEditor = editor;

        // Act
        viewModel.SelectedEditor = null;

        // Assert
        Assert.Null(viewModel.CurrentEditorViewModel);
        Assert.Null(viewModel.CurrentBreadcrumb);
    }

    [Fact]
    public void Constructor_WithBackwardCompatibility_ShouldWork()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            EditorFactory = _mockEditorFactory.Object,
            ServiceProvider = _mockServiceProvider.Object
        };

        // Act
        var viewModel = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Categories);
        Assert.True(viewModel.Categories.Count > 0);
    }

    [Fact]
    public async Task AutoLoadXmlFile_WithValidEditor_ShouldCallLoadMethod()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        
        // 使用反射来测试private方法
        var autoLoadMethod = typeof(EditorManagerViewModel).GetMethod("AutoLoadXmlFileAsync", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        var mockEditor = new Mock<ViewModelBase>();
        mockEditor.Setup(x => x.GetType())
            .Returns(typeof(AttributeEditorViewModel));

        var loadMethod = typeof(AttributeEditorViewModel).GetMethod("LoadXmlFile");
        mockEditor.Setup(x => x.GetType().GetMethod("LoadXmlFile"))
            .Returns(loadMethod);

        // Act
        var task = (Task)autoLoadMethod.Invoke(viewModel, new object[] { mockEditor.Object, "test.xml" });
        await task; // 等待异步方法完成

        // Assert
        _mockLogService.Verify(x => x.LogInfo("Successfully auto-loaded XML file: test.xml", "EditorManager"), Times.Once);
    }

    [Fact]
    public async Task AutoLoadXmlFile_WithInvalidEditor_ShouldLogError()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object
        };

        var viewModel = new EditorManagerViewModel(options);
        
        // 使用反射来测试private方法
        var autoLoadMethod = typeof(EditorManagerViewModel).GetMethod("AutoLoadXmlFileAsync", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        var mockEditor = new Mock<ViewModelBase>();
        mockEditor.Setup(x => x.GetType())
            .Throws<Exception>();

        // Act
        var task = (Task)autoLoadMethod.Invoke(viewModel, new object[] { mockEditor.Object, "test.xml" });
        await task; // 等待异步方法完成

        // Assert
        _mockLogService.Verify(x => x.LogException(It.IsAny<Exception>(), "Failed to auto-load XML file: test.xml"), Times.Once);
    }
}