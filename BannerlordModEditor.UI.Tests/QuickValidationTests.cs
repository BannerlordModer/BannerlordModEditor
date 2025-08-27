using Xunit;
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests
{
    /// <summary>
    /// 快速验证测试套件
    /// 
    /// 这是一个简化的测试套件，用于验证基本的测试功能是否正常工作。
    /// 主要验证：
    /// - 依赖注入配置
    /// - 服务注册
    /// - ViewModel创建
    /// - 基本功能
    /// </summary>
    public class QuickValidationTests
    {
        [Fact]
        public void TestServiceProvider_Should_Be_Available()
        {
            // Arrange & Act
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider);
        }

        [Fact]
        public void CoreServices_Should_Be_Registered()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            var logService = serviceProvider.GetService<ILogService>();
            Assert.NotNull(logService);

            var errorHandlerService = serviceProvider.GetService<IErrorHandlerService>();
            Assert.NotNull(errorHandlerService);

            var validationService = serviceProvider.GetService<IValidationService>();
            Assert.NotNull(validationService);

            var dataBindingService = serviceProvider.GetService<IDataBindingService>();
            Assert.NotNull(dataBindingService);
        }

        [Fact]
        public void EditorFactory_Should_Be_Registered()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act
            var editorFactory = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();

            // Assert
            Assert.NotNull(editorFactory);
        }

        [Fact]
        public void EditorViewModels_Should_Be_Registered()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            var attributeEditor = serviceProvider.GetService<AttributeEditorViewModel>();
            Assert.NotNull(attributeEditor);

            var skillEditor = serviceProvider.GetService<SkillEditorViewModel>();
            Assert.NotNull(skillEditor);

            var mainWindow = serviceProvider.GetService<MainWindowViewModel>();
            Assert.NotNull(mainWindow);

            var editorManager = serviceProvider.GetService<EditorManagerViewModel>();
            Assert.NotNull(editorManager);
        }

        [Fact]
        public void EditorFactory_Should_Create_Editors()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var editorFactory = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();

            // Act
            var attributeEditor = editorFactory.CreateEditorViewModel("AttributeEditor", "attributes.xml");
            var skillEditor = editorFactory.CreateEditorViewModel("SkillEditor", "skills.xml");

            // Assert
            Assert.NotNull(attributeEditor);
            Assert.NotNull(skillEditor);
        }

        [Fact]
        public void ServiceLifetime_Should_Be_Correct()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            // 单例服务
            var logService1 = serviceProvider.GetService<ILogService>();
            var logService2 = serviceProvider.GetService<ILogService>();
            Assert.Same(logService1, logService2);

            var editorFactory1 = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            var editorFactory2 = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            Assert.Same(editorFactory1, editorFactory2);

            // 瞬态服务
            var attributeEditor1 = serviceProvider.GetService<AttributeEditorViewModel>();
            var attributeEditor2 = serviceProvider.GetService<AttributeEditorViewModel>();
            Assert.NotSame(attributeEditor1, attributeEditor2);
        }

        [Fact]
        public void MainWindow_Should_Have_Dependencies()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindowViewModel>();

            // Act & Assert
            Assert.NotNull(mainWindow);
            Assert.NotNull(mainWindow.EditorManager);
            Assert.NotNull(mainWindow.LogService);
        }

        [Fact]
        public void EditorManager_Should_Have_Categories()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var editorManager = serviceProvider.GetService<EditorManagerViewModel>();

            // Act & Assert
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            Assert.True(editorManager.Categories.Count > 0);
        }

        [Fact]
        public void AttributeEditor_Should_Initialize_Correctly()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var attributeEditor = serviceProvider.GetService<AttributeEditorViewModel>();

            // Act & Assert
            Assert.NotNull(attributeEditor);
            Assert.NotNull(attributeEditor.Attributes);
            Assert.True(attributeEditor.Attributes.Count > 0);
        }

        [Fact]
        public void SkillEditor_Should_Initialize_Correctly()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var skillEditor = serviceProvider.GetService<SkillEditorViewModel>();

            // Act & Assert
            Assert.NotNull(skillEditor);
            Assert.NotNull(skillEditor.Skills);
            Assert.True(skillEditor.Skills.Count > 0);
        }

        [Fact]
        public void Validation_Should_Work()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var validationService = serviceProvider.GetService<IValidationService>();

            // Act
            var result = validationService.Validate(new object());

            // Assert
            Assert.NotNull(result);
            // 验证结果的具体属性取决于实际的ValidationService实现
        }

        [Fact]
        public void DataBinding_Should_Work()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            var dataBindingService = serviceProvider.GetService<IDataBindingService>();

            // Act & Assert
            Assert.NotNull(dataBindingService);
            // 数据绑定的具体测试取决于实际的DataBindingService实现
        }

        [Fact]
        public void TestConfiguration_Should_Be_Valid()
        {
            // Arrange & Act
            var isValid = TestServiceProvider.ValidateConfiguration();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void TestHelper_Should_Work()
        {
            // Arrange & Act
            var testDataDir = TestDataHelper.TestDataDirectory;

            // Assert
            Assert.NotNull(testDataDir);
            Assert.True(Directory.Exists(testDataDir));
        }

        [Fact]
        public void All_Required_Services_Should_Be_Available()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();

            // Act & Assert
            var requiredServices = new[]
            {
                typeof(ILogService),
                typeof(IErrorHandlerService),
                typeof(IValidationService),
                typeof(IDataBindingService),
                typeof(BannerlordModEditor.UI.Factories.IEditorFactory),
                typeof(MainWindowViewModel),
                typeof(EditorManagerViewModel),
                typeof(AttributeEditorViewModel),
                typeof(SkillEditorViewModel)
            };

            foreach (var serviceType in requiredServices)
            {
                var service = serviceProvider.GetService(serviceType);
                Assert.True(service != null, $"服务 {serviceType.Name} 未注册");
            }
        }
    }
}