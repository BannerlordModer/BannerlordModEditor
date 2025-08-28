using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BannerlordModEditor.UI.Tests.Factories;

/// <summary>
/// EditorManagerFactory单元测试
/// </summary>
public class EditorManagerFactoryTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILogService> _mockLogService;
    private readonly Mock<IErrorHandlerService> _mockErrorHandlerService;
    private readonly Mock<IEditorFactory> _mockEditorFactory;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<IDataBindingService> _mockDataBindingService;
    private readonly EditorManagerFactory _factory;

    public EditorManagerFactoryTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLogService = new Mock<ILogService>();
        _mockErrorHandlerService = new Mock<IErrorHandlerService>();
        _mockEditorFactory = new Mock<IEditorFactory>();
        _mockValidationService = new Mock<IValidationService>();
        _mockDataBindingService = new Mock<IDataBindingService>();

        // 设置服务提供器返回模拟服务
        _mockServiceProvider.Setup(x => x.GetService(typeof(ILogService)))
            .Returns(_mockLogService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IErrorHandlerService)))
            .Returns(_mockErrorHandlerService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEditorFactory)))
            .Returns(_mockEditorFactory.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IValidationService)))
            .Returns(_mockValidationService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IDataBindingService)))
            .Returns(_mockDataBindingService.Object);

        // 设置编辑器工厂返回一些类型
        _mockEditorFactory.Setup(x => x.GetRegisteredEditorTypes())
            .Returns(new[] { "AttributeEditor", "SkillEditor", "CombatParameterEditor" });

        // 设置验证服务返回有效结果
        _mockValidationService.Setup(x => x.Validate(It.IsAny<object>()))
            .Returns(new ValidationResult { IsValid = true });

        _factory = new EditorManagerFactory(
            _mockServiceProvider.Object,
            _mockLogService.Object,
            _mockErrorHandlerService.Object,
            _mockEditorFactory.Object,
            _mockValidationService.Object,
            _mockDataBindingService.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_ShouldInitializeFactory()
    {
        // Act
        var factory = new EditorManagerFactory(
            _mockServiceProvider.Object,
            _mockLogService.Object,
            _mockErrorHandlerService.Object,
            _mockEditorFactory.Object,
            _mockValidationService.Object,
            _mockDataBindingService.Object);

        // Assert
        Assert.NotNull(factory);
        _mockLogService.Verify(x => x.LogInfo("EditorManagerFactory initialized", "EditorManagerFactory"), Times.Once);
    }

    [Theory]
    [InlineData(null, "logService")]
    [InlineData("logService", null)]
    [InlineData("logService", "errorHandlerService")]
    [InlineData("logService", "errorHandlerService")]
    [InlineData("logService", "errorHandlerService")]
    [InlineData("logService", "errorHandlerService")]
    public void Constructor_WithNullDependency_ShouldThrowArgumentNullException(
        string notNullService, string nullService)
    {
        // Arrange
        var services = new Dictionary<string, object>
        {
            ["serviceProvider"] = _mockServiceProvider.Object,
            ["logService"] = _mockLogService.Object,
            ["errorHandlerService"] = _mockErrorHandlerService.Object,
            ["editorFactory"] = _mockEditorFactory.Object,
            ["validationService"] = _mockValidationService.Object,
            ["dataBindingService"] = _mockDataBindingService.Object
        };

        if (nullService != "serviceProvider") services["serviceProvider"] = _mockServiceProvider.Object;
        if (nullService != "logService") services["logService"] = _mockLogService.Object;
        if (nullService != "errorHandlerService") services["errorHandlerService"] = _mockErrorHandlerService.Object;
        if (nullService != "editorFactory") services["editorFactory"] = _mockEditorFactory.Object;
        if (nullService != "validationService") services["validationService"] = _mockValidationService.Object;
        if (nullService != "dataBindingService") services["dataBindingService"] = _mockDataBindingService.Object;

        services[nullService] = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EditorManagerFactory(
            (IServiceProvider)services["serviceProvider"],
            (ILogService)services["logService"],
            (IErrorHandlerService)services["errorHandlerService"],
            (IEditorFactory)services["editorFactory"],
            (IValidationService)services["validationService"],
            (IDataBindingService)services["dataBindingService"]));
    }

    [Fact]
    public void CreateEditorManager_WithDefaultOptions_ShouldReturnValidViewModel()
    {
        // Act
        var result = _factory.CreateEditorManager();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EditorManagerViewModel>(result);
        _mockLogService.Verify(x => x.LogDebug("Starting EditorManagerViewModel creation", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogInfo(It.Is<string>(s => s.Contains("created successfully")), "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithCustomOptions_ShouldUseProvidedOptions()
    {
        // Arrange
        var options = new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = true,
            EnableDiagnostics = true,
            EnablePostCreationConfiguration = true,
            CreationTimeout = 60000
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogDebug("Starting EditorManagerViewModel creation", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Validating service health", "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _factory.CreateEditorManager(null!));
    }

    [Fact]
    public async Task CreateEditorManagerAsync_WithDefaultOptions_ShouldReturnValidViewModel()
    {
        // Act
        var result = await _factory.CreateEditorManagerAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EditorManagerViewModel>(result);
        _mockLogService.Verify(x => x.LogDebug("Starting async EditorManagerViewModel creation", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogInfo(It.Is<string>(s => s.Contains("created asynchronously successfully")), "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public async Task CreateEditorManagerAsync_WithCustomOptions_ShouldUseProvidedOptions()
    {
        // Arrange
        var options = new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = true,
            EnableDiagnostics = true,
            CreationTimeout = 60000
        };

        // Act
        var result = await _factory.CreateEditorManagerAsync(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogDebug("Starting async EditorManagerViewModel creation", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Validating service health asynchronously", "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public async Task CreateEditorManagerAsync_WithCancellation_ShouldRespectCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => 
            _factory.CreateEditorManagerAsync(cancellationToken: cts.Token));
    }

    [Fact]
    public void CreateEditorManager_WithHealthChecksEnabled_ShouldValidateServices()
    {
        // Arrange
        var options = new EditorManagerCreationOptions
        {
            EnableHealthChecks = true
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogDebug("Testing log service", "EditorManagerFactory.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Editor factory has 3 registered types", "EditorManagerFactory.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Validation service test result: True", "EditorManagerFactory.HealthCheck"), Times.Once);
        _mockLogService.Verify(x => x.LogInfo("All services passed health check", "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithHealthChecksFailed_ShouldThrowException()
    {
        // Arrange
        _mockEditorFactory.Setup(x => x.GetRegisteredEditorTypes())
            .Throws<Exception>();

        var options = new EditorManagerCreationOptions
        {
            EnableHealthChecks = true
        };

        // Act & Assert
        var exception = Assert.Throws<EditorManagerCreationException>(() => 
            _factory.CreateEditorManager(options));
        
        Assert.Equal("Service health check failed", exception.Message);
        _mockLogService.Verify(x => x.LogException(It.IsAny<EditorManagerCreationException>(), "Service health check failed"), Times.Once);
    }

    [Fact]
    public void GetStatistics_ShouldReturnCorrectStatistics()
    {
        // Arrange
        _factory.CreateEditorManager();
        _factory.CreateEditorManager();

        // Act
        var stats = _factory.GetStatistics();

        // Assert
        Assert.Equal(2, stats.InstancesCreated);
        Assert.True(stats.LastCreationTime > DateTime.MinValue);
        Assert.False(stats.IsCreationInProgress);
    }

    [Fact]
    public void CreateEditorManager_ShouldBeThreadSafe()
    {
        // Arrange
        const int threadCount = 10;
        var results = new List<EditorManagerViewModel>();
        var exceptions = new List<Exception>();

        // Act
        Parallel.For(0, threadCount, i =>
        {
            try
            {
                var result = _factory.CreateEditorManager();
                lock (results)
                {
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(threadCount, results.Count);
        Assert.All(results, result => Assert.NotNull(result));
    }

    [Fact]
    public async Task CreateEditorManagerAsync_ShouldBeThreadSafe()
    {
        // Arrange
        const int threadCount = 10;
        var results = new List<EditorManagerViewModel>();
        var exceptions = new List<Exception>();

        // Act
        var tasks = Enumerable.Range(0, threadCount).Select(async i =>
        {
            try
            {
                var result = await _factory.CreateEditorManagerAsync();
                lock (results)
                {
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(threadCount, results.Count);
        Assert.All(results, result => Assert.NotNull(result));
    }

    [Fact]
    public void CreateEditorManager_WithPerformanceMonitoring_ShouldLogCreationTime()
    {
        // Arrange
        var options = new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogDebug(It.Is<string>(s => s.Contains("creation took")), "EditorManagerFactory.Performance"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithPostCreationConfiguration_ShouldConfigureEditorManager()
    {
        // Arrange
        var options = new EditorManagerCreationOptions
        {
            EnablePostCreationConfiguration = true,
            EnablePerformanceMonitoring = true,
            EnableDiagnostics = true
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogDebug("Configuring EditorManagerViewModel", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Performance monitoring enabled", "EditorManagerFactory"), Times.Once);
        _mockLogService.Verify(x => x.LogDebug("Diagnostics enabled", "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithPostCreationConfigurationFailure_ShouldNotThrow()
    {
        // Arrange
        _mockLogService.Setup(x => x.LogDebug(It.IsAny<string>(), It.IsAny<string>()))
            .Throws<Exception>();

        var options = new EditorManagerCreationOptions
        {
            EnablePostCreationConfiguration = true
        };

        // Act & Assert
        var exception = Record.Exception(() => _factory.CreateEditorManager(options));
        Assert.Null(exception);
    }

    [Fact]
    public void Dispose_ShouldReleaseResources()
    {
        // Arrange
        var factory = new EditorManagerFactory(
            _mockServiceProvider.Object,
            _mockLogService.Object,
            _mockErrorHandlerService.Object,
            _mockEditorFactory.Object,
            _mockValidationService.Object,
            _mockDataBindingService.Object);

        // Act
        factory.Dispose();

        // Assert
        _mockLogService.Verify(x => x.LogInfo("EditorManagerFactory disposed", "EditorManagerFactory"), Times.Once);
    }

    [Fact]
    public void CreateEditorManager_WithSlowCreation_ShouldLogWarning()
    {
        // Arrange
        _mockValidationService.Setup(x => x.Validate(It.IsAny<object>()))
            .Callback(() => Thread.Sleep(1100)); // 模拟慢操作

        var options = new EditorManagerCreationOptions
        {
            EnableHealthChecks = true
        };

        // Act
        var result = _factory.CreateEditorManager(options);

        // Assert
        Assert.NotNull(result);
        _mockLogService.Verify(x => x.LogWarning(It.Is<string>(s => s.Contains("creation took") && s.Contains("ms")), "EditorManagerFactory.Performance"), Times.Once);
    }
}