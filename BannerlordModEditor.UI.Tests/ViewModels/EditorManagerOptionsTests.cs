using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Factories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BannerlordModEditor.UI.Tests.ViewModels;

/// <summary>
/// EditorManagerOptions单元测试
/// </summary>
public class EditorManagerOptionsTests
{
    private readonly Mock<ILogService> _mockLogService;
    private readonly Mock<IErrorHandlerService> _mockErrorHandlerService;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<IDataBindingService> _mockDataBindingService;
    private readonly Mock<IEditorFactory> _mockEditorFactory;
    private readonly Mock<IServiceProvider> _mockServiceProvider;

    public EditorManagerOptionsTests()
    {
        _mockLogService = new Mock<ILogService>();
        _mockErrorHandlerService = new Mock<IErrorHandlerService>();
        _mockValidationService = new Mock<IValidationService>();
        _mockDataBindingService = new Mock<IDataBindingService>();
        _mockEditorFactory = new Mock<IEditorFactory>();
        _mockServiceProvider = new Mock<IServiceProvider>();

        // 设置服务提供器返回模拟服务
        _mockServiceProvider.Setup(x => x.GetService(typeof(ILogService)))
            .Returns(_mockLogService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IErrorHandlerService)))
            .Returns(_mockErrorHandlerService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IValidationService)))
            .Returns(_mockValidationService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IDataBindingService)))
            .Returns(_mockDataBindingService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEditorFactory)))
            .Returns(_mockEditorFactory.Object);
    }

    [Fact]
    public void Default_ShouldReturnValidOptions()
    {
        // Act
        var options = EditorManagerOptions.Default;

        // Assert
        Assert.NotNull(options);
        Assert.False(options.EnablePerformanceMonitoring);
        Assert.True(options.EnableHealthChecks);
        Assert.False(options.EnableDiagnostics);
        Assert.Equal(30000, options.CreationTimeout);
        Assert.False(options.StrictMode);
    }

    [Fact]
    public void ForDependencyInjection_WithValidServiceProvider_ShouldReturnValidOptions()
    {
        // Act
        var options = EditorManagerOptions.ForDependencyInjection(_mockServiceProvider.Object);

        // Assert
        Assert.NotNull(options);
        Assert.Equal(_mockEditorFactory.Object, options.EditorFactory);
        Assert.Equal(_mockLogService.Object, options.LogService);
        Assert.Equal(_mockErrorHandlerService.Object, options.ErrorHandlerService);
        Assert.Equal(_mockValidationService.Object, options.ValidationService);
        Assert.Equal(_mockDataBindingService.Object, options.DataBindingService);
        Assert.Equal(_mockServiceProvider.Object, options.ServiceProvider);
    }

    [Fact]
    public void ForDependencyInjection_WithNullServiceProvider_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            EditorManagerOptions.ForDependencyInjection(null!));
    }

    [Fact]
    public void ForTesting_ShouldReturnValidOptionsForTesting()
    {
        // Act
        var options = EditorManagerOptions.ForTesting();

        // Assert
        Assert.NotNull(options);
        Assert.NotNull(options.LogService);
        Assert.NotNull(options.ErrorHandlerService);
        Assert.NotNull(options.ValidationService);
        Assert.False(options.StrictMode);
    }

    [Fact]
    public void Validate_WithValidConfiguration_ShouldReturnValidResult()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            EditorFactory = _mockEditorFactory.Object,
            ServiceProvider = _mockServiceProvider.Object,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Validate_WithMissingLogService_ShouldReturnInvalidResult()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("LogService is required", result.Errors);
    }

    [Fact]
    public void Validate_WithMissingErrorHandlerService_ShouldReturnInvalidResult()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = null,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("ErrorHandlerService is required", result.Errors);
    }

    [Fact]
    public void Validate_WithMissingValidationService_ShouldReturnWarning()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = null,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Contains("ValidationService is not available - some features may not work", result.Warnings);
    }

    [Fact]
    public void Validate_WithMissingEditorFactory_ShouldReturnWarning()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            EditorFactory = null,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Contains("EditorFactory is not available - using default editors only", result.Warnings);
    }

    [Fact]
    public void Validate_WithMissingServiceProvider_ShouldReturnWarning()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ServiceProvider = null,
            CreationTimeout = 30000
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Contains("ServiceProvider is not available - dependency injection may not work properly", result.Warnings);
    }

    [Fact]
    public void Validate_WithInvalidCreationTimeout_ShouldReturnInvalidResult()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            CreationTimeout = -1
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("CreationTimeout must be positive", result.Errors);
    }

    [Fact]
    public void Validate_WithZeroCreationTimeout_ShouldReturnInvalidResult()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            CreationTimeout = 0
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("CreationTimeout must be positive", result.Errors);
    }

    [Fact]
    public void Validate_WithMultipleIssues_ShouldReturnAllIssues()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null,
            ErrorHandlerService = null,
            ValidationService = null,
            EditorFactory = null,
            ServiceProvider = null,
            CreationTimeout = -1
        };

        // Act
        var result = options.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("LogService is required", result.Errors);
        Assert.Contains("ErrorHandlerService is required", result.Errors);
        Assert.Contains("CreationTimeout must be positive", result.Errors);
        Assert.Contains("ValidationService is not available - some features may not work", result.Warnings);
        Assert.Contains("EditorFactory is not available - using default editors only", result.Warnings);
        Assert.Contains("ServiceProvider is not available - dependency injection may not work properly", result.Warnings);
    }

    [Fact]
    public void EnsureValid_WithValidConfiguration_ShouldNotThrow()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            CreationTimeout = 30000
        };

        // Act & Assert
        var exception = Record.Exception(() => options.EnsureValid());
        Assert.Null(exception);
    }

    [Fact]
    public void EnsureValid_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = null,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            CreationTimeout = 30000
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => options.EnsureValid());
    }

    [Fact]
    public void Clone_ShouldReturnIdenticalCopy()
    {
        // Arrange
        var original = new EditorManagerOptions
        {
            EditorFactory = _mockEditorFactory.Object,
            LogService = _mockLogService.Object,
            ErrorHandlerService = _mockErrorHandlerService.Object,
            ValidationService = _mockValidationService.Object,
            DataBindingService = _mockDataBindingService.Object,
            ServiceProvider = _mockServiceProvider.Object,
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = true,
            EnableDiagnostics = true,
            CreationTimeout = 60000,
            StrictMode = true
        };

        // Act
        var cloned = original.Clone();

        // Assert
        Assert.Equal(original.EditorFactory, cloned.EditorFactory);
        Assert.Equal(original.LogService, cloned.LogService);
        Assert.Equal(original.ErrorHandlerService, cloned.ErrorHandlerService);
        Assert.Equal(original.ValidationService, cloned.ValidationService);
        Assert.Equal(original.DataBindingService, cloned.DataBindingService);
        Assert.Equal(original.ServiceProvider, cloned.ServiceProvider);
        Assert.Equal(original.EnablePerformanceMonitoring, cloned.EnablePerformanceMonitoring);
        Assert.Equal(original.EnableHealthChecks, cloned.EnableHealthChecks);
        Assert.Equal(original.EnableDiagnostics, cloned.EnableDiagnostics);
        Assert.Equal(original.CreationTimeout, cloned.CreationTimeout);
        Assert.Equal(original.StrictMode, cloned.StrictMode);
    }

    [Fact]
    public void Clone_ShouldReturnIndependentCopy()
    {
        // Arrange
        var original = new EditorManagerOptions
        {
            EnablePerformanceMonitoring = false,
            EnableHealthChecks = false,
            CreationTimeout = 10000
        };

        // Act
        var cloned = original.Clone();
        cloned.EnablePerformanceMonitoring = true;
        cloned.EnableHealthChecks = true;
        cloned.CreationTimeout = 20000;

        // Assert
        Assert.False(original.EnablePerformanceMonitoring);
        Assert.False(original.EnableHealthChecks);
        Assert.Equal(10000, original.CreationTimeout);
    }

    [Fact]
    public void WithStrictMode_ShouldEnableStrictMode()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            StrictMode = false
        };

        // Act
        var result = options.WithStrictMode();

        // Assert
        Assert.Same(options, result);
        Assert.True(options.StrictMode);
    }

    [Fact]
    public void WithPerformanceMonitoring_ShouldEnablePerformanceMonitoring()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            EnablePerformanceMonitoring = false
        };

        // Act
        var result = options.WithPerformanceMonitoring();

        // Assert
        Assert.Same(options, result);
        Assert.True(options.EnablePerformanceMonitoring);
    }

    [Fact]
    public void WithHealthChecks_ShouldEnableHealthChecks()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            EnableHealthChecks = false
        };

        // Act
        var result = options.WithHealthChecks();

        // Assert
        Assert.Same(options, result);
        Assert.True(options.EnableHealthChecks);
    }

    [Fact]
    public void WithDiagnostics_ShouldEnableDiagnostics()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            EnableDiagnostics = false
        };

        // Act
        var result = options.WithDiagnostics();

        // Assert
        Assert.Same(options, result);
        Assert.True(options.EnableDiagnostics);
    }

    [Fact]
    public void ConfigurationValidationResult_ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var result = new ConfigurationValidationResult
        {
            IsValid = false,
            Errors = new List<string> { "Error 1", "Error 2" },
            Warnings = new List<string> { "Warning 1", "Warning 2" }
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Configuration Validation: FAILED", resultString);
        Assert.Contains("Errors:", resultString);
        Assert.Contains("- Error 1", resultString);
        Assert.Contains("- Error 2", resultString);
        Assert.Contains("Warnings:", resultString);
        Assert.Contains("- Warning 1", resultString);
        Assert.Contains("- Warning 2", resultString);
    }

    [Fact]
    public void ConfigurationValidationResult_ToString_WithValidResult_ShouldReturnSuccessString()
    {
        // Arrange
        var result = new ConfigurationValidationResult
        {
            IsValid = true,
            Errors = new List<string>(),
            Warnings = new List<string>()
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Configuration Validation: PASSED", resultString);
        Assert.DoesNotContain("Errors:", resultString);
        Assert.DoesNotContain("Warnings:", resultString);
    }

    [Fact]
    public void ConfigurationValidationResult_ToString_WithOnlyWarnings_ShouldIncludeWarnings()
    {
        // Arrange
        var result = new ConfigurationValidationResult
        {
            IsValid = true,
            Errors = new List<string>(),
            Warnings = new List<string> { "Warning 1" }
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Configuration Validation: PASSED", resultString);
        Assert.DoesNotContain("Errors:", resultString);
        Assert.Contains("Warnings:", resultString);
        Assert.Contains("- Warning 1", resultString);
    }

    [Fact]
    public void ConfigurationValidationResult_ToString_WithOnlyErrors_ShouldIncludeErrors()
    {
        // Arrange
        var result = new ConfigurationValidationResult
        {
            IsValid = false,
            Errors = new List<string> { "Error 1" },
            Warnings = new List<string>()
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Configuration Validation: FAILED", resultString);
        Assert.Contains("Errors:", resultString);
        Assert.Contains("- Error 1", resultString);
        Assert.DoesNotContain("Warnings:", resultString);
    }

    [Fact]
    public void Constructor_WithDefaultValues_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var options = new EditorManagerOptions();

        // Assert
        Assert.False(options.EnablePerformanceMonitoring);
        Assert.True(options.EnableHealthChecks);
        Assert.False(options.EnableDiagnostics);
        Assert.Equal(30000, options.CreationTimeout);
        Assert.False(options.StrictMode);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var options = new EditorManagerOptions();

        // Act
        options.EnablePerformanceMonitoring = true;
        options.EnableHealthChecks = false;
        options.EnableDiagnostics = true;
        options.CreationTimeout = 60000;
        options.StrictMode = true;

        // Assert
        Assert.True(options.EnablePerformanceMonitoring);
        Assert.False(options.EnableHealthChecks);
        Assert.True(options.EnableDiagnostics);
        Assert.Equal(60000, options.CreationTimeout);
        Assert.True(options.StrictMode);
    }
}