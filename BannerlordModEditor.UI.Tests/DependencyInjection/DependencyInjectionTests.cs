using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BannerlordModEditor.UI.Tests.DependencyInjection
{
    /// <summary>
    /// 测试依赖注入配置是否正确
    /// </summary>
    public class DependencyInjectionTests
    {
        [Fact]
        public void ConfigureServices_ShouldResolveAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // 模拟 App.ConfigureServices() 的配置
            services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            services.AddTransient<BannerlordModEditor.Common.Services.IFileDiscoveryService, BannerlordModEditor.Common.Services.FileDiscoveryService>();
            
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            // 验证 UnifiedEditorFactory 可以被解析
            var editorFactory = serviceProvider.GetService<IEditorFactory>();
            Assert.NotNull(editorFactory);
            Assert.IsType<UnifiedEditorFactory>(editorFactory);

            // 验证所有必需的服务都可以被解析
            Assert.NotNull(serviceProvider.GetService<ILogService>());
            Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
            Assert.NotNull(serviceProvider.GetService<IValidationService>());
            Assert.NotNull(serviceProvider.GetService<IDataBindingService>());
            Assert.NotNull(serviceProvider.GetService<BannerlordModEditor.Common.Services.IFileDiscoveryService>());

            // 验证 UnifiedEditorFactory 的构造函数参数可以正确注入
            var unifiedFactory = editorFactory as UnifiedEditorFactory;
            Assert.NotNull(unifiedFactory);
        }

        [Fact]
        public void LogService_ShouldBeSingleton()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<ILogService, LogService>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var logService1 = serviceProvider.GetService<ILogService>();
            var logService2 = serviceProvider.GetService<ILogService>();

            // Assert
            Assert.Same(logService1, logService2);
        }

        [Fact]
        public void ErrorHandlerService_ShouldBeSingleton()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var errorHandler1 = serviceProvider.GetService<IErrorHandlerService>();
            var errorHandler2 = serviceProvider.GetService<IErrorHandlerService>();

            // Assert
            Assert.Same(errorHandler1, errorHandler2);
        }
    }
}