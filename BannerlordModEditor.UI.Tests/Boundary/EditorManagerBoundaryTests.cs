using System;
using System.Threading;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BannerlordModEditor.UI.Tests.Boundary;

/// <summary>
/// EditorManagerViewModel边界条件测试
/// </summary>
public class EditorManagerBoundaryTests
{
    [Fact]
    public void EditorManagerFactory_WithNullServiceProvider_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EditorManagerFactory(
            null!,
            mockLogService.Object,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object));
    }

    [Fact]
    public void EditorManagerFactory_WithNullLogService_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EditorManagerFactory(
            mockServiceProvider.Object,
            null!,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object));
    }

    [Fact]
    public void EditorManagerFactory_WithNullErrorHandlerService_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogService = new Mock<ILogService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EditorManagerFactory(
            mockServiceProvider.Object,
            mockLogService.Object,
            null!,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object));
    }

    [Fact]
    public void EditorManagerFactory_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        var factory = new EditorManagerFactory(
            mockServiceProvider.Object,
            mockLogService.Object,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateEditorManager(null!));
    }

    [Fact]
    public void EditorManagerOptions_WithNegativeCreationTimeout_ShouldFailValidation()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object,
            CreationTimeout = -1
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("CreationTimeout must be positive", result.Errors);
    }

    [Fact]
    public void EditorManagerOptions_WithZeroCreationTimeout_ShouldFailValidation()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object,
            CreationTimeout = 0
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("CreationTimeout must be positive", result.Errors);
    }

    [Fact]
    public void EditorManagerOptions_WithIntMaxCreationTimeout_ShouldPassValidation()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object,
            CreationTimeout = int.MaxValue
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void EditorManagerOptions_WithMissingRequiredServices_ShouldFailValidation()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null,
            ErrorHandlerService = null,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("LogService is required", result.Errors);
        Assert.Contains("ErrorHandlerService is required", result.Errors);
    }

    [Fact]
    public void EditorManagerServiceOptions_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = false,
            UseErrorHandlerService = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => options.Validate());
    }

    [Fact]
    public void ServiceCollectionExtensions_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddEditorManagerServices());
        Assert.Throws<ArgumentNullException>(() => services.AddMinimalEditorManagerServices());
        Assert.Throws<ArgumentNullException>(() => services.AddFullEditorManagerServices());
        Assert.Throws<ArgumentNullException>(() => services.ValidateEditorManagerServices());
    }

    [Fact]
    public void ServiceCollectionExtensions_WithNullConfigureOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddEditorManagerServices(null!));
    }

    [Fact]
    public void EditorManagerFactory_WithFailedHealthCheck_ShouldThrowException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        // 设置验证服务抛出异常
        mockValidationService.Setup(x => x.Validate(It.IsAny<object>()))
            .Throws<Exception>();

        var factory = new EditorManagerFactory(
            mockServiceProvider.Object,
            mockLogService.Object,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object);

        var options = new EditorManagerCreationOptions
        {
            EnableHealthChecks = true
        };

        // Act & Assert
        Assert.Throws<EditorManagerCreationException>(() => factory.CreateEditorManager(options));
    }

    [Fact]
    public async Task EditorManagerFactory_WithCancelledAsyncOperation_ShouldThrowTaskCanceledException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        var factory = new EditorManagerFactory(
            mockServiceProvider.Object,
            mockLogService.Object,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object);

        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => 
            factory.CreateEditorManagerAsync(cancellationToken: cts.Token));
    }

    [Fact]
    public void EditorManager_WithStrictModeAndHealthCheckFailure_ShouldThrowException()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockValidationService = new Mock<IValidationService>();

        // 设置错误处理服务抛出异常
        mockErrorHandlerService.Setup(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ErrorSeverity>()))
            .Throws(new Exception("Test exception"));

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object,
            ValidationService = mockValidationService.Object,
            EnableHealthChecks = true,
            StrictMode = true
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new EditorManagerViewModel(options));
    }

    [Fact]
    public void EditorManager_WithInvalidEditorType_ShouldHandleGracefully()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockValidationService = new Mock<IValidationService>();

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object,
            ValidationService = mockValidationService.Object
        };

        var editorManager = new EditorManagerViewModel(options);
        var invalidEditorItem = new EditorItemViewModel("无效编辑器", "无效编辑器描述", "invalid.xml", "InvalidEditorType", "❌");

        // Act & Assert
        // 验证选择无效编辑器不会抛出异常到调用者（异常被内部处理）
        var exception = Record.Exception(() => 
            editorManager.SelectEditorCommand.Execute(invalidEditorItem));
        
        Assert.Null(exception); // 异常应该被内部处理，不会传播到调用者
        mockErrorHandlerService.Verify(x => x.HandleError(It.IsAny<NotSupportedException>(), "选择编辑器失败"), Times.Once);
    }

    [Fact]
    public void EditorManager_WithNullServiceProvider_ShouldUseDefaultServices()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            ServiceProvider = null,
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object
        };

        // Act & Assert
        var exception = Record.Exception(() => new EditorManagerViewModel(options));
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManager_WithEmptySearchText_ShouldShowAllEditors()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object
        };

        var editorManager = new EditorManagerViewModel(options);

        // Act
        editorManager.SearchText = "";
        editorManager.SearchText = null;

        // Assert
        Assert.NotNull(editorManager.Categories);
        var allEditors = editorManager.Categories.SelectMany(c => c.Editors);
        Assert.All(allEditors, e => Assert.True(e.IsAvailable));
    }

    [Fact]
    public void EditorManager_WithLongSearchText_ShouldHandleCorrectly()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object
        };

        var editorManager = new EditorManagerViewModel(options);
        var longSearchText = new string('a', 10000); // 非常长的搜索文本

        // Act & Assert
        var exception = Record.Exception(() => editorManager.SearchText = longSearchText);
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManager_WithSpecialCharactersInSearchText_ShouldHandleCorrectly()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object
        };

        var editorManager = new EditorManagerViewModel(options);

        // Act & Assert
        var exception = Record.Exception(() => editorManager.SearchText = "搜索@#$%^&*()特殊字符");
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManager_WithUnicodeInSearchText_ShouldHandleCorrectly()
    {
        // Arrange
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();

        var options = new EditorManagerOptions
        {
            LogService = mockLogService.Object,
            ErrorHandlerService = mockErrorHandlerService.Object
        };

        var editorManager = new EditorManagerViewModel(options);

        // Act & Assert
        var exception = Record.Exception(() => editorManager.SearchText = "搜索中文和emoji🎯");
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManagerFactory_WithDisposedInstance_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogService = new Mock<ILogService>();
        var mockErrorHandlerService = new Mock<IErrorHandlerService>();
        var mockEditorFactory = new Mock<IEditorFactory>();
        var mockValidationService = new Mock<IValidationService>();
        var mockDataBindingService = new Mock<IDataBindingService>();

        var factory = new EditorManagerFactory(
            mockServiceProvider.Object,
            mockLogService.Object,
            mockErrorHandlerService.Object,
            mockEditorFactory.Object,
            mockValidationService.Object,
            mockDataBindingService.Object);

        // Act
        factory.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => factory.CreateEditorManager());
    }

    [Fact]
    public void EditorManagerOptions_WithNullServiceProviderInForDependencyInjection_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            EditorManagerOptions.ForDependencyInjection(null!));
    }

    [Fact]
    public void EditorManager_WithAllNullServices_ShouldUseDefaults()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null,
            ErrorHandlerService = null,
            ValidationService = null,
            DataBindingService = null,
            EditorFactory = null,
            ServiceProvider = null
        };

        // Act & Assert
        var exception = Record.Exception(() => new EditorManagerViewModel(options));
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManager_WithVeryShortTimeout_ShouldTimeoutGracefully()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = new Mock<ILogService>().Object,
            ErrorHandlerService = new Mock<IErrorHandlerService>().Object,
            CreationTimeout = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => new EditorManagerViewModel(options));
        Assert.Null(exception);
    }

    [Fact]
    public void ServiceRegistrationValidationResult_WithAllMissingServices_ShouldReturnInvalidResult()
    {
        // Arrange
        var result = new ServiceRegistrationValidationResult
        {
            IsLogServiceRegistered = false,
            IsErrorHandlerServiceRegistered = false,
            IsValidationServiceRegistered = false,
            IsDataBindingServiceRegistered = false,
            IsEditorFactoryRegistered = false,
            IsEditorManagerFactoryRegistered = false,
            IsEditorManagerViewModelRegistered = false
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Validation Result: FAILED", resultString);
        Assert.Contains("LogService: Missing", resultString);
        Assert.Contains("ErrorHandlerService: Missing", resultString);
        Assert.Contains("EditorManagerFactory: Missing", resultString);
        Assert.Contains("EditorManagerViewModel: Missing", resultString);
    }

    [Fact]
    public void ConfigurationValidationResult_WithVeryLongErrorMessage_ShouldHandleCorrectly()
    {
        // Arrange
        var longErrorMessage = new string('a', 10000);
        var result = new ConfigurationValidationResult
        {
            IsValid = false,
            Errors = new List<string> { longErrorMessage },
            Warnings = new List<string> { longErrorMessage }
        };

        // Act & Assert
        var exception = Record.Exception(() => result.ToString());
        Assert.Null(exception);
    }
}