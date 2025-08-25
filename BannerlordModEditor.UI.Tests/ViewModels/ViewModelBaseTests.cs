using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Tests.Helpers;
using BannerlordModEditor.UI.Factories;
using System;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Tests.ViewModels;

/// <summary>
/// ViewModelBase 基础功能测试
/// 
/// 测试内容：
/// - 构造函数和依赖注入
/// - EditorType 和 XmlFileName 属性
/// - 异常处理和安全执行
/// - 日志记录功能
/// - 错误消息显示
/// </summary>
public class ViewModelBaseTests
{
    private readonly Mock<IErrorHandlerService> _mockErrorHandler;
    private readonly Mock<ILogService> _mockLogService;

    public ViewModelBaseTests()
    {
        _mockErrorHandler = new Mock<IErrorHandlerService>();
        _mockLogService = new Mock<ILogService>();
    }

    [Fact]
    public void Constructor_WithDefaultServices_ShouldCreateInstance()
    {
        // Arrange & Act
        var viewModel = new TestableViewModelBase();

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.ErrorHandler);
        Assert.NotNull(viewModel.LogService);
    }

    [Fact]
    public void Constructor_WithInjectedServices_ShouldUseInjectedServices()
    {
        // Arrange & Act
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Assert
        Assert.NotNull(viewModel);
        Assert.Same(_mockErrorHandler.Object, viewModel.ErrorHandler);
        Assert.Same(_mockLogService.Object, viewModel.LogService);
    }

    [Fact]
    public void EditorType_WithoutAttribute_ShouldReturnTypeName()
    {
        // Arrange & Act
        var viewModel = new TestableViewModelBase();

        // Assert
        Assert.Equal("TestableViewModelBase", viewModel.EditorType);
    }

    [Fact]
    public void XmlFileName_WithoutAttribute_ShouldReturnTypeNameWithXmlExtension()
    {
        // Arrange & Act
        var viewModel = new TestableViewModelBase();

        // Assert
        Assert.Equal("TestableViewModelBase.xml", viewModel.XmlFileName);
    }

    [Fact]
    public void EditorType_WithAttribute_ShouldReturnAttributeValue()
    {
        // Arrange & Act
        var viewModel = new AttributedTestViewModel();

        // Assert
        Assert.Equal("TestEditor", viewModel.EditorType);
    }

    [Fact]
    public void XmlFileName_WithAttribute_ShouldReturnAttributeValue()
    {
        // Arrange & Act
        var viewModel = new AttributedTestViewModel();

        // Assert
        Assert.Equal("test_file.xml", viewModel.XmlFileName);
    }

    [Fact]
    public async Task ExecuteSafelyAsync_WithSuccessfulOperation_ShouldReturnTrue()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var operationExecuted = false;

        // Act
        var result = await viewModel.TestExecuteSafelyAsync(async () =>
        {
            operationExecuted = true;
            await Task.Delay(10);
        }, "TestContext");

        // Assert
        Assert.True(result);
        Assert.True(operationExecuted);
        _mockErrorHandler.Verify(x => x.HandleExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteSafelyAsync_WithException_ShouldReturnTrueAndHandleError()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var expectedException = new InvalidOperationException("Test exception");

        // Act
        var result = await viewModel.TestExecuteSafelyAsync(async () =>
        {
            await Task.Delay(10);
            throw expectedException;
        }, "TestContext");

        // Assert
        Assert.False(result);
        _mockLogService.Verify(x => x.LogException(expectedException, "TestContext"), Times.Once);
        _mockErrorHandler.Verify(x => x.HandleExceptionAsync(expectedException, "TestContext"), Times.Once);
    }

    [Fact]
    public void ExecuteSafely_WithSuccessfulOperation_ShouldReturnTrue()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var operationExecuted = false;

        // Act
        var result = viewModel.TestExecuteSafely(() =>
        {
            operationExecuted = true;
        }, "TestContext");

        // Assert
        Assert.True(result);
        Assert.True(operationExecuted);
        _mockErrorHandler.Verify(x => x.ShowErrorMessage(It.IsAny<string>(), "错误", It.IsAny<ErrorSeverity>()), Times.Never);
    }

    [Fact]
    public void ExecuteSafely_WithException_ShouldReturnFalseAndHandleError()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var expectedException = new InvalidOperationException("Test exception");

        // Act
        var result = viewModel.TestExecuteSafely(() =>
        {
            throw expectedException;
        }, "TestContext");

        // Assert
        Assert.False(result);
        _mockLogService.Verify(x => x.LogException(expectedException, "TestContext"), Times.Once);
        _mockErrorHandler.Verify(x => x.ShowErrorMessage(expectedException.Message, "错误", It.IsAny<ErrorSeverity>()), Times.Once);
    }

    [Fact]
    public async Task HandleErrorAsync_ShouldLogAndHandleException()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var expectedException = new InvalidOperationException("Test exception");

        // Act
        await viewModel.TestHandleErrorAsync(expectedException, "TestContext");

        // Assert
        _mockLogService.Verify(x => x.LogException(expectedException, "TestContext"), Times.Once);
        _mockErrorHandler.Verify(x => x.HandleExceptionAsync(expectedException, "TestContext"), Times.Once);
    }

    [Fact]
    public void HandleError_ShouldLogAndShowErrorMessage()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var expectedException = new InvalidOperationException("Test exception");

        // Act
        viewModel.TestHandleError(expectedException, "TestContext");

        // Assert
        _mockLogService.Verify(x => x.LogException(expectedException, "TestContext"), Times.Once);
        _mockErrorHandler.Verify(x => x.ShowErrorMessage(expectedException.Message, "错误", It.IsAny<ErrorSeverity>()), Times.Once);
    }

    [Fact]
    public void LogInfo_ShouldCallLogService()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogInfo("Test message", "TestCategory");

        // Assert
        _mockLogService.Verify(x => x.LogInfo("Test message", "TestCategory"), Times.Once);
    }

    [Fact]
    public void LogWarning_ShouldCallLogService()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogWarning("Test warning", "TestCategory");

        // Assert
        _mockLogService.Verify(x => x.LogWarning("Test warning", "TestCategory"), Times.Once);
    }

    [Fact]
    public void LogError_ShouldCallLogService()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogError("Test error", "TestCategory");

        // Assert
        _mockLogService.Verify(x => x.LogError("Test error", "TestCategory"), Times.Once);
    }

    [Fact]
    public void ShowError_ShouldCallErrorHandler()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowError("Test error message", "Test Title");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowErrorMessage("Test error message", "Test Title", It.IsAny<ErrorSeverity>()), Times.Once);
    }

    [Fact]
    public void ShowWarning_ShouldCallErrorHandler()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowWarning("Test warning message", "Test Title");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowWarningMessage("Test warning message", "Test Title"), Times.Once);
    }

    [Fact]
    public void ShowInfo_ShouldCallErrorHandler()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowInfo("Test info message", "Test Title");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowInfoMessage("Test info message", "Test Title"), Times.Once);
    }

    [Fact]
    public void LogInfo_WithDefaultCategory_ShouldUseGeneral()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogInfo("Test message");

        // Assert
        _mockLogService.Verify(x => x.LogInfo("Test message", "General"), Times.Once);
    }

    [Fact]
    public void LogWarning_WithDefaultCategory_ShouldUseGeneral()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogWarning("Test warning");

        // Assert
        _mockLogService.Verify(x => x.LogWarning("Test warning", "General"), Times.Once);
    }

    [Fact]
    public void LogError_WithDefaultCategory_ShouldUseGeneral()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestLogError("Test error");

        // Assert
        _mockLogService.Verify(x => x.LogError("Test error", "General"), Times.Once);
    }

    [Fact]
    public void ShowError_WithDefaultTitle_ShouldUseError()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowError("Test error message");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowErrorMessage("Test error message", "错误", It.IsAny<ErrorSeverity>()), Times.Once);
    }

    [Fact]
    public void ShowWarning_WithDefaultTitle_ShouldUseWarning()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowWarning("Test warning message");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowWarningMessage("Test warning message", "警告"), Times.Once);
    }

    [Fact]
    public void ShowInfo_WithDefaultTitle_ShouldUseInfo()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);

        // Act
        viewModel.TestShowInfo("Test info message");

        // Assert
        _mockErrorHandler.Verify(x => x.ShowInfoMessage("Test info message", "信息"), Times.Once);
    }

    [Fact]
    public void ExecuteSafely_WithoutContext_ShouldStillWork()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var operationExecuted = false;

        // Act
        var result = viewModel.TestExecuteSafely(() =>
        {
            operationExecuted = true;
        });

        // Assert
        Assert.True(result);
        Assert.True(operationExecuted);
    }

    [Fact]
    public async Task ExecuteSafelyAsync_WithoutContext_ShouldStillWork()
    {
        // Arrange
        var viewModel = new TestableViewModelBase(_mockErrorHandler.Object, _mockLogService.Object);
        var operationExecuted = false;

        // Act
        var result = await viewModel.TestExecuteSafelyAsync(async () =>
        {
            operationExecuted = true;
            await Task.Delay(10);
        });

        // Assert
        Assert.True(result);
        Assert.True(operationExecuted);
    }

    #region Test Helper Classes

    /// <summary>
    /// 测试用的ViewModelBase实现，用于访问受保护的方法
    /// </summary>
    private class TestableViewModelBase : ViewModelBase
    {
        public TestableViewModelBase() : base()
        {
        }

        public TestableViewModelBase(IErrorHandlerService errorHandler, ILogService logService) 
            : base(errorHandler, logService)
        {
        }

        public new IErrorHandlerService ErrorHandler => base.ErrorHandler;
        public new ILogService LogService => base.LogService;

        public async Task<bool> TestExecuteSafelyAsync(Func<Task> action, string context = "")
        {
            return await ExecuteSafelyAsync(action, context);
        }

        public bool TestExecuteSafely(Action action, string context = "")
        {
            return ExecuteSafely(action, context);
        }

        public async Task TestHandleErrorAsync(Exception exception, string context = "")
        {
            await HandleErrorAsync(exception, context);
        }

        public void TestHandleError(Exception exception, string context = "")
        {
            HandleError(exception, context);
        }

        public void TestLogInfo(string message, string category = "General")
        {
            LogInfo(message, category);
        }

        public void TestLogWarning(string message, string category = "General")
        {
            LogWarning(message, category);
        }

        public void TestLogError(string message, string category = "General")
        {
            LogError(message, category);
        }

        public void TestShowError(string message, string title = "错误")
        {
            ShowError(message, title);
        }

        public void TestShowWarning(string message, string title = "警告")
        {
            ShowWarning(message, title);
        }

        public void TestShowInfo(string message, string title = "信息")
        {
            ShowInfo(message, title);
        }
    }

    /// <summary>
    /// 带有EditorTypeAttribute的测试ViewModel
    /// </summary>
    [EditorType(EditorType = "TestEditor", XmlFileName = "test_file.xml")]
    private class AttributedTestViewModel : ViewModelBase
    {
        public AttributedTestViewModel() : base()
        {
        }

        public AttributedTestViewModel(IErrorHandlerService errorHandler, ILogService logService) 
            : base(errorHandler, logService)
        {
        }
    }

    #endregion
}