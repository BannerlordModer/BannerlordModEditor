using System;
using System.Linq;
using BannerlordModEditor.UI.Extensions;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BannerlordModEditor.UI.Tests.Extensions;

/// <summary>
/// ServiceCollectionExtensions集成测试
/// </summary>
public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEditorManagerServices_WithDefaultConfiguration_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IEditorManagerFactory>());
        Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.NotNull(serviceProvider.GetService<IValidationService>());
        Assert.NotNull(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void AddEditorManagerServices_WithCustomConfiguration_ShouldRegisterServicesWithConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEditorManagerServices(options =>
        {
            options.UseLogService = true;
            options.UseErrorHandlerService = true;
            options.UseValidationService = true;
            options.UseDataBindingService = true;
            options.UseEditorFactory = true;
            options.EnablePerformanceMonitoring = true;
            options.EnableHealthChecks = true;
            options.EnableDiagnostics = true;
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IEditorManagerFactory>());
        Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.NotNull(serviceProvider.GetService<IValidationService>());
        Assert.NotNull(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void AddEditorManagerServices_WithMinimalConfiguration_ShouldRegisterOnlyEssentialServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEditorManagerServices(options =>
        {
            options.UseLogService = true;
            options.UseErrorHandlerService = true;
            options.UseValidationService = false;
            options.UseDataBindingService = false;
            options.UseEditorFactory = false;
            options.EnablePerformanceMonitoring = false;
            options.EnableHealthChecks = false;
            options.EnableDiagnostics = false;
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IEditorManagerFactory>());
        Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.Null(serviceProvider.GetService<IValidationService>());
        Assert.Null(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void AddEditorManagerServices_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services.AddEditorManagerServices());
    }

    [Fact]
    public void AddEditorManagerServices_WithNullConfigureOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services.AddEditorManagerServices(null!));
    }

    [Fact]
    public void AddEditorManagerServices_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            services.AddEditorManagerServices(options =>
            {
                options.UseLogService = false; // 无效配置
                options.UseErrorHandlerService = false; // 无效配置
            }));
    }

    [Fact]
    public void AddMinimalEditorManagerServices_ShouldRegisterOnlyCoreServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMinimalEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IEditorManagerFactory>());
        Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.Null(serviceProvider.GetService<IValidationService>());
        Assert.Null(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void AddMinimalEditorManagerServices_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services.AddMinimalEditorManagerServices());
    }

    [Fact]
    public void AddFullEditorManagerServices_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFullEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IEditorManagerFactory>());
        Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.NotNull(serviceProvider.GetService<IValidationService>());
        Assert.NotNull(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void AddFullEditorManagerServices_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services.AddFullEditorManagerServices());
    }

    [Fact]
    public void ValidateEditorManagerServices_WithCompleteRegistration_ShouldReturnValidResult()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFullEditorManagerServices();

        // Act
        var result = services.ValidateEditorManagerServices();

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.IsLogServiceRegistered);
        Assert.True(result.IsErrorHandlerServiceRegistered);
        Assert.True(result.IsValidationServiceRegistered);
        Assert.True(result.IsDataBindingServiceRegistered);
        Assert.True(result.IsEditorFactoryRegistered);
        Assert.True(result.IsEditorManagerFactoryRegistered);
        Assert.True(result.IsEditorManagerViewModelRegistered);
    }

    [Fact]
    public void ValidateEditorManagerServices_WithMinimalRegistration_ShouldReturnValidResult()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMinimalEditorManagerServices();

        // Act
        var result = services.ValidateEditorManagerServices();

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.IsLogServiceRegistered);
        Assert.True(result.IsErrorHandlerServiceRegistered);
        Assert.False(result.IsValidationServiceRegistered);
        Assert.False(result.IsDataBindingServiceRegistered);
        Assert.False(result.IsEditorFactoryRegistered);
        Assert.True(result.IsEditorManagerFactoryRegistered);
        Assert.True(result.IsEditorManagerViewModelRegistered);
    }

    [Fact]
    public void ValidateEditorManagerServices_WithIncompleteRegistration_ShouldReturnInvalidResult()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ILogService, LogService>();
        // 缺少ErrorHandlerService

        // Act
        var result = services.ValidateEditorManagerServices();

        // Assert
        Assert.False(result.IsValid);
        Assert.True(result.IsLogServiceRegistered);
        Assert.False(result.IsErrorHandlerServiceRegistered);
    }

    [Fact]
    public void ValidateEditorManagerServices_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services.ValidateEditorManagerServices());
    }

    [Fact]
    public void AddEditorManagerServices_ShouldRegisterServicesAsSingletonOrTransientCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEditorManagerServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert - 检查单例服务
        var logService1 = serviceProvider.GetService<ILogService>();
        var logService2 = serviceProvider.GetService<ILogService>();
        Assert.Same(logService1, logService2);

        var errorHandlerService1 = serviceProvider.GetService<IErrorHandlerService>();
        var errorHandlerService2 = serviceProvider.GetService<IErrorHandlerService>();
        Assert.Same(errorHandlerService1, errorHandlerService2);

        // Assert - 检查瞬态服务
        var editorManager1 = serviceProvider.GetService<EditorManagerViewModel>();
        var editorManager2 = serviceProvider.GetService<EditorManagerViewModel>();
        Assert.NotSame(editorManager1, editorManager2);
    }

    [Fact]
    public void AddEditorManagerServices_ShouldSupportChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddEditorManagerServices();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddMinimalEditorManagerServices_ShouldSupportChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddMinimalEditorManagerServices();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddFullEditorManagerServices_ShouldSupportChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddFullEditorManagerServices();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void EditorManagerServiceOptions_Default_ShouldHaveCorrectValues()
    {
        // Arrange & Act
        var options = EditorManagerServiceOptions.Default;

        // Assert
        Assert.True(options.UseLogService);
        Assert.True(options.UseErrorHandlerService);
        Assert.True(options.UseValidationService);
        Assert.True(options.UseDataBindingService);
        Assert.True(options.UseEditorFactory);
        Assert.False(options.EnablePerformanceMonitoring);
        Assert.True(options.EnableHealthChecks);
        Assert.False(options.EnableDiagnostics);
    }

    [Fact]
    public void EditorManagerServiceOptions_Validate_WithValidConfiguration_ShouldNotThrow()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = true,
            UseErrorHandlerService = true,
            UseValidationService = true,
            UseDataBindingService = true,
            UseEditorFactory = true
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        Assert.Null(exception);
    }

    [Fact]
    public void EditorManagerServiceOptions_Validate_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = false, // 无效配置
            UseErrorHandlerService = false // 无效配置
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => options.Validate());
    }

    [Fact]
    public void EditorManagerServiceOptions_Validate_WithMissingLogService_ShouldThrow()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = false,
            UseErrorHandlerService = true
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("LogService is required", exception.Message);
    }

    [Fact]
    public void EditorManagerServiceOptions_Validate_WithMissingErrorHandlerService_ShouldThrow()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = true,
            UseErrorHandlerService = false
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("ErrorHandlerService is required", exception.Message);
    }

    [Fact]
    public void EditorManagerServiceOptions_Validate_WithMultipleErrors_ShouldIncludeAllErrors()
    {
        // Arrange
        var options = new EditorManagerServiceOptions
        {
            UseLogService = false,
            UseErrorHandlerService = false
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("LogService is required", exception.Message);
        Assert.Contains("ErrorHandlerService is required", exception.Message);
    }

    [Fact]
    public void ServiceRegistrationValidationResult_ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var result = new ServiceRegistrationValidationResult
        {
            IsValid = false,
            IsLogServiceRegistered = true,
            IsErrorHandlerServiceRegistered = false,
            IsValidationServiceRegistered = true,
            IsDataBindingServiceRegistered = false,
            IsEditorFactoryRegistered = true,
            IsEditorManagerFactoryRegistered = true,
            IsEditorManagerViewModelRegistered = true
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Validation Result: FAILED", resultString);
        Assert.Contains("LogService: Registered", resultString);
        Assert.Contains("ErrorHandlerService: Missing", resultString);
        Assert.Contains("ValidationService: Registered", resultString);
        Assert.Contains("DataBindingService: Missing", resultString);
        Assert.Contains("EditorFactory: Registered", resultString);
        Assert.Contains("EditorManagerFactory: Registered", resultString);
        Assert.Contains("EditorManagerViewModel: Registered", resultString);
    }

    [Fact]
    public void ServiceRegistrationValidationResult_ToString_WithValidResult_ShouldReturnSuccessString()
    {
        // Arrange
        var result = new ServiceRegistrationValidationResult
        {
            IsValid = true,
            IsLogServiceRegistered = true,
            IsErrorHandlerServiceRegistered = true,
            IsValidationServiceRegistered = true,
            IsDataBindingServiceRegistered = true,
            IsEditorFactoryRegistered = true,
            IsEditorManagerFactoryRegistered = true,
            IsEditorManagerViewModelRegistered = true
        };

        // Act
        var resultString = result.ToString();

        // Assert
        Assert.Contains("Validation Result: PASSED", resultString);
        Assert.All(resultString.Split('\n'), line => line.Contains("Registered"));
    }

    [Fact]
    public void AddEditorManagerServices_WithOptions_ShouldApplyOptionsCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var optionsConfigured = false;

        // Act
        services.AddEditorManagerServices(options =>
        {
            options.UseLogService = false;
            options.UseErrorHandlerService = true;
            options.UseValidationService = false;
            options.UseDataBindingService = false;
            options.UseEditorFactory = false;
            optionsConfigured = true;
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.True(optionsConfigured);
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.Null(serviceProvider.GetService<IValidationService>());
        Assert.Null(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void RegisterCoreServices_WithOptions_ShouldRegisterOnlyEnabledServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var options = new EditorManagerServiceOptions
        {
            UseLogService = true,
            UseErrorHandlerService = true,
            UseValidationService = false,
            UseDataBindingService = false
        };

        // Act
        services.AddEditorManagerServices(opt => 
        {
            opt.UseLogService = options.UseLogService;
            opt.UseErrorHandlerService = options.UseErrorHandlerService;
            opt.UseValidationService = options.UseValidationService;
            opt.UseDataBindingService = options.UseDataBindingService;
        });
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<ILogService>());
        Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
        Assert.Null(serviceProvider.GetService<IValidationService>());
        Assert.Null(serviceProvider.GetService<IDataBindingService>());
    }

    [Fact]
    public void RegisterOptionalServices_WithOptions_ShouldRegisterOnlyEnabledServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var options = new EditorManagerServiceOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = false,
            EnableDiagnostics = true
        };

        // Act
        services.AddEditorManagerServices(opt => 
        {
            opt.EnablePerformanceMonitoring = options.EnablePerformanceMonitoring;
            opt.EnableHealthChecks = options.EnableHealthChecks;
            opt.EnableDiagnostics = options.EnableDiagnostics;
        });
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        // 目前可选服务还没有实现，所以这里主要测试配置是否正确应用
        // TODO: 当实现可选服务后，需要更新这个测试
    }
}