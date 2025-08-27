using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.UI.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using System.Linq;

namespace BannerlordModEditor.UI.Tests.DependencyInjection
{
    /// <summary>
    /// 依赖注入配置验证测试套件
    /// 
    /// 这个测试套件专门验证TestServiceProvider的正确配置和所有服务的注册状态。
    /// 主要功能：
    /// - 验证所有核心服务的正确注册
    /// - 测试服务生命周期管理
    /// - 确保依赖注入配置的一致性
    /// - 验证服务解析的正确性
    /// 
    /// 测试覆盖范围：
    /// 1. 核心基础设施服务（日志、错误处理、验证等）
    /// 2. 编辑器工厂服务
    /// 3. 所有编辑器ViewModel
    /// 4. 主要ViewModel
    /// 5. Common层服务
    /// 6. 测试专用模拟服务
    /// </summary>
    public class DependencyInjectionTests
    {
        [Fact]
        public void TestServiceProvider_Should_Be_Configured_Correctly()
        {
            // Arrange & Act
            var isValid = TestServiceProvider.ValidateConfiguration();

            // Assert
            Assert.True(isValid, "TestServiceProvider配置验证失败");
        }

        [Fact]
        public void CoreServices_Should_Be_Registered_As_Singleton()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            // 验证日志服务
            var logService1 = serviceProvider.GetService<ILogService>();
            var logService2 = serviceProvider.GetService<ILogService>();
            Assert.NotNull(logService1);
            Assert.Same(logService1, logService2);

            // 验证错误处理服务
            var errorHandler1 = serviceProvider.GetService<IErrorHandlerService>();
            var errorHandler2 = serviceProvider.GetService<IErrorHandlerService>();
            Assert.NotNull(errorHandler1);
            Assert.Same(errorHandler1, errorHandler2);

            // 验证验证服务
            var validationService1 = serviceProvider.GetService<IValidationService>();
            var validationService2 = serviceProvider.GetService<IValidationService>();
            Assert.NotNull(validationService1);
            Assert.Same(validationService1, validationService2);

            // 验证数据绑定服务
            var dataBindingService1 = serviceProvider.GetService<IDataBindingService>();
            var dataBindingService2 = serviceProvider.GetService<IDataBindingService>();
            Assert.NotNull(dataBindingService1);
            Assert.Same(dataBindingService1, dataBindingService2);
        }

        [Fact]
        public void EditorFactory_Should_Be_Registered_As_Singleton()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act
            var factory1 = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            var factory2 = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();

            // Assert
            Assert.NotNull(factory1);
            Assert.Same(factory1, factory2);
            Assert.IsType<MockEditorFactory>(factory1);
        }

        [Fact]
        public void EditorViewModels_Should_Be_Registered_As_Transient()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            // 测试AttributeEditorViewModel
            var attributeEditor1 = serviceProvider.GetService<AttributeEditorViewModel>();
            var attributeEditor2 = serviceProvider.GetService<AttributeEditorViewModel>();
            Assert.NotNull(attributeEditor1);
            Assert.NotNull(attributeEditor2);
            Assert.NotSame(attributeEditor1, attributeEditor2);

            // 测试SkillEditorViewModel
            var skillEditor1 = serviceProvider.GetService<SkillEditorViewModel>();
            var skillEditor2 = serviceProvider.GetService<SkillEditorViewModel>();
            Assert.NotNull(skillEditor1);
            Assert.NotNull(skillEditor2);
            Assert.NotSame(skillEditor1, skillEditor2);

            // 测试CombatParameterEditorViewModel
            var combatEditor1 = serviceProvider.GetService<CombatParameterEditorViewModel>();
            var combatEditor2 = serviceProvider.GetService<CombatParameterEditorViewModel>();
            Assert.NotNull(combatEditor1);
            Assert.NotNull(combatEditor2);
            Assert.NotSame(combatEditor1, combatEditor2);

            // 测试ItemEditorViewModel
            var itemEditor1 = serviceProvider.GetService<ItemEditorViewModel>();
            var itemEditor2 = serviceProvider.GetService<ItemEditorViewModel>();
            Assert.NotNull(itemEditor1);
            Assert.NotNull(itemEditor2);
            Assert.NotSame(itemEditor1, itemEditor2);

            // 测试高级编辑器
            var craftingEditor1 = serviceProvider.GetService<CraftingPieceEditorViewModel>();
            var craftingEditor2 = serviceProvider.GetService<CraftingPieceEditorViewModel>();
            Assert.NotNull(craftingEditor1);
            Assert.NotNull(craftingEditor2);
            Assert.NotSame(craftingEditor1, craftingEditor2);

            var boneBodyEditor1 = serviceProvider.GetService<BoneBodyTypeEditorViewModel>();
            var boneBodyEditor2 = serviceProvider.GetService<BoneBodyTypeEditorViewModel>();
            Assert.NotNull(boneBodyEditor1);
            Assert.NotNull(boneBodyEditor2);
            Assert.NotSame(boneBodyEditor1, boneBodyEditor2);
        }

        [Fact]
        public void MainViewModels_Should_Be_Registered_As_Transient()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            // 测试MainWindowViewModel
            var mainWindow1 = serviceProvider.GetService<MainWindowViewModel>();
            var mainWindow2 = serviceProvider.GetService<MainWindowViewModel>();
            Assert.NotNull(mainWindow1);
            Assert.NotNull(mainWindow2);
            Assert.NotSame(mainWindow1, mainWindow2);

            // 测试EditorManagerViewModel
            var editorManager1 = serviceProvider.GetService<EditorManagerViewModel>();
            var editorManager2 = serviceProvider.GetService<EditorManagerViewModel>();
            Assert.NotNull(editorManager1);
            Assert.NotNull(editorManager2);
            Assert.NotSame(editorManager1, editorManager2);

            // 测试EditorCategoryViewModel
            var category1 = serviceProvider.GetService<EditorCategoryViewModel>();
            var category2 = serviceProvider.GetService<EditorCategoryViewModel>();
            Assert.NotNull(category1);
            Assert.NotNull(category2);
            Assert.NotSame(category1, category2);

            // 测试EditorItemViewModel
            var item1 = serviceProvider.GetService<EditorItemViewModel>();
            var item2 = serviceProvider.GetService<EditorItemViewModel>();
            Assert.NotNull(item1);
            Assert.NotNull(item2);
            Assert.NotSame(item1, item2);
        }

        [Fact]
        public void CommonServices_Should_Be_Registered_Correctly()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            var fileDiscoveryService = serviceProvider.GetService<IFileDiscoveryService>();
            Assert.NotNull(fileDiscoveryService);
            Assert.IsType<FileDiscoveryService>(fileDiscoveryService);

            // 验证为单例
            var fileDiscoveryService2 = serviceProvider.GetService<IFileDiscoveryService>();
            Assert.Same(fileDiscoveryService, fileDiscoveryService2);
        }

        [Fact]
        public void ServiceResolution_Should_Work_With_Generic_GetService()
        {
            // Arrange & Act
            var logService = TestServiceProvider.GetService<ILogService>();
            var editorFactory = TestServiceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            var attributeEditor = TestServiceProvider.GetService<AttributeEditorViewModel>();

            // Assert
            Assert.NotNull(logService);
            Assert.NotNull(editorFactory);
            Assert.NotNull(attributeEditor);
            Assert.IsType<LogService>(logService);
            Assert.IsType<MockEditorFactory>(editorFactory);
        }

        [Fact]
        public void ServiceResolution_Should_Work_With_TryGetService()
        {
            // Arrange & Act
            var hasLogService = TestServiceProvider.TryGetService<ILogService>(out var logService);
            var hasNonExistentService = TestServiceProvider.TryGetService<INonExistentService>(out var nonExistentService);

            // Assert
            Assert.True(hasLogService);
            Assert.NotNull(logService);
            Assert.False(hasNonExistentService);
            Assert.Null(nonExistentService);
        }

        [Fact]
        public void ServiceResolution_Should_Work_With_GetServices()
        {
            // Arrange & Act
            var logServices = TestServiceProvider.GetServices<ILogService>().ToList();
            var editorFactories = TestServiceProvider.GetServices<BannerlordModEditor.UI.Factories.IEditorFactory>().ToList();

            // Assert
            Assert.Single(logServices);
            Assert.Single(editorFactories);
            Assert.NotNull(logServices[0]);
            Assert.NotNull(editorFactories[0]);
        }

        [Fact]
        public void Scope_Should_Work_Correctly()
        {
            // Arrange
            var scopeProvider = TestServiceProvider.CreateScope();

            // Act
            var logService1 = scopeProvider.GetService<ILogService>();
            var logService2 = scopeProvider.GetService<ILogService>();

            // Assert
            Assert.NotNull(logService1);
            Assert.NotNull(logService2);
            Assert.Same(logService1, logService2); // 单例服务在作用域内也应该是同一个实例
        }

        [Fact]
        public void Reset_Should_Clear_ServiceProvider()
        {
            // Arrange
            var serviceProvider1 = TestServiceProvider.GetServiceProvider();
            var logService1 = serviceProvider1.GetService<ILogService>();

            // Act
            TestServiceProvider.Reset();
            var serviceProvider2 = TestServiceProvider.GetServiceProvider();
            var logService2 = serviceProvider2.GetService<ILogService>();

            // Assert
            Assert.NotNull(logService1);
            Assert.NotNull(logService2);
            Assert.NotSame(logService1, logService2); // 重置后应该创建新的服务实例
        }

        [Fact]
        public void ServiceDependency_Chain_Should_Be_Resolvable()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act
            var editorManager = serviceProvider.GetService<EditorManagerViewModel>();
            var mainWindow = serviceProvider.GetService<MainWindowViewModel>();

            // Assert
            Assert.NotNull(editorManager);
            Assert.NotNull(mainWindow);
            
            // 验证EditorManager的依赖项
            Assert.NotNull(editorManager.EditorFactory);
            Assert.NotNull(editorManager.LogService);
            Assert.NotNull(editorManager.ErrorHandlerService);
            
            // 验证MainWindow的依赖项
            Assert.NotNull(mainWindow.EditorManager);
            Assert.NotNull(mainWindow.LogService);
        }

        [Fact]
        public void All_Required_Services_Should_Be_Available()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            // 核心服务
            Assert.NotNull(serviceProvider.GetService<ILogService>());
            Assert.NotNull(serviceProvider.GetService<IErrorHandlerService>());
            Assert.NotNull(serviceProvider.GetService<IValidationService>());
            Assert.NotNull(serviceProvider.GetService<IDataBindingService>());
            
            // 工厂服务
            Assert.NotNull(serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>());
            
            // 编辑器ViewModel
            Assert.NotNull(serviceProvider.GetService<AttributeEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<SkillEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<CombatParameterEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<ItemEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<CraftingPieceEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<ItemModifierEditorViewModel>());
            Assert.NotNull(serviceProvider.GetService<BoneBodyTypeEditorViewModel>());
            
            // 主要ViewModel
            Assert.NotNull(serviceProvider.GetService<MainWindowViewModel>());
            Assert.NotNull(serviceProvider.GetService<EditorManagerViewModel>());
            Assert.NotNull(serviceProvider.GetService<EditorCategoryViewModel>());
            Assert.NotNull(serviceProvider.GetService<EditorItemViewModel>());
            
            // Common层服务
            Assert.NotNull(serviceProvider.GetService<IFileDiscoveryService>());
        }

        [Fact]
        public void MockEditorFactory_Should_Have_All_Dependencies_Injected()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var mockFactory = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>() as MockEditorFactory;

            // Act & Assert
            Assert.NotNull(mockFactory);
            Assert.NotNull(mockFactory.ServiceProvider);
            Assert.NotNull(mockFactory.LogService);
            Assert.NotNull(mockFactory.ErrorHandlerService);
            Assert.NotNull(mockFactory.ValidationService);
            Assert.NotNull(mockFactory.DataBindingService);
        }

        [Fact]
        public void Service_Registration_Should_Be_Thread_Safe()
        {
            // Arrange
            const int threadCount = 10;
            var results = new bool[threadCount];
            var exceptions = new System.Exception[threadCount];

            // Act
            Parallel.For(0, threadCount, i =>
            {
                try
                {
                    var serviceProvider = TestServiceProvider.GetServiceProvider();
                    var service = serviceProvider.GetService<ILogService>();
                    results[i] = service != null;
                }
                catch (Exception ex)
                {
                    exceptions[i] = ex;
                    results[i] = false;
                }
            });

            // Assert
            Assert.All(results, result => Assert.True(result));
            Assert.All(exceptions, ex => Assert.Null(ex));
        }

        // 用于测试的接口
        private interface INonExistentService { }
    }
}