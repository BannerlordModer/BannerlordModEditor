using Xunit;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Tests.Helpers;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Services;

namespace BannerlordModEditor.UI.Tests.Diagnostic
{
    /// <summary>
    /// 诊断UI界面空白问题的测试类
    /// </summary>
    public class UIInterfaceDiagnosticTests
    {
        [Fact]
        public void Diagnostic_EditorManager_Should_Have_NonEmpty_Categories()
        {
            // Arrange & Act
            var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

            // Assert - 基础检查
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            
            // 关键检查：分类集合不应该为空
            Assert.NotEmpty(editorManager.Categories);
            
            // 输出诊断信息
            System.Diagnostics.Debug.WriteLine($"=== UI Interface Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"Total categories: {editorManager.Categories.Count}");
            
            foreach (var category in editorManager.Categories)
            {
                System.Diagnostics.Debug.WriteLine($"Category: {category.Name}");
                System.Diagnostics.Debug.WriteLine($"  - Editors count: {category.Editors.Count}");
                
                foreach (var editor in category.Editors)
                {
                    System.Diagnostics.Debug.WriteLine($"    * Editor: {editor.Name}");
                }
            }
        }

        [Fact]
        public void Diagnostic_EditorManager_Should_Have_Editors_In_Categories()
        {
            // Arrange & Act
            var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

            // Assert
            Assert.NotNull(editorManager);
            Assert.NotEmpty(editorManager.Categories);

            // 关键检查：每个分类都应该有编辑器
            var totalEditors = editorManager.Categories.Sum(c => c.Editors.Count);
            Assert.True(totalEditors > 0);
            
            // 检查是否有任何可见的编辑器
            var visibleEditors = editorManager.Categories
                .SelectMany(c => c.Editors)
                .Where(e => e.IsAvailable)
                .ToList();
            
            Assert.NotEmpty(visibleEditors);
            
            System.Diagnostics.Debug.WriteLine($"=== Editor Visibility Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"Total editors: {totalEditors}");
            System.Diagnostics.Debug.WriteLine($"Visible editors: {visibleEditors.Count}");
        }

        [Fact]
        public void Diagnostic_EditorFactory_Should_Return_Valid_Editors()
        {
            // Arrange
            var editorFactory = TestServiceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            
            // Assert
            Assert.NotNull(editorFactory);
            
            // 检查工厂是否能创建编辑器
            var allEditors = editorFactory.GetAllEditors();
            Assert.NotNull(allEditors);
            
            System.Diagnostics.Debug.WriteLine($"=== Editor Factory Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"Factory type: {editorFactory.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"Total editors from factory: {allEditors.Count()}");
            
            if (!allEditors.Any())
            {
                System.Diagnostics.Debug.WriteLine("WARNING: EditorFactory returned no editors. This is likely causing the blank UI.");
                
                // 检查注册的编辑器类型
                var registeredTypes = editorFactory.GetRegisteredEditorTypes();
                System.Diagnostics.Debug.WriteLine($"Registered editor types: {registeredTypes.Count()}");
                foreach (var type in registeredTypes)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {type}");
                }
            }
        }

        [Fact]
        public void Diagnostic_Dependency_Injection_Should_Be_Configured_Correctly()
        {
            // Arrange
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            
            // Assert - 检查关键服务
            var requiredServices = new[]
            {
                typeof(ILogService),
                typeof(IErrorHandlerService),
                typeof(IValidationService),
                typeof(IDataBindingService),
                typeof(BannerlordModEditor.UI.Factories.IEditorFactory),
                typeof(EditorManagerViewModel),
                typeof(MainWindowViewModel)
            };
            
            foreach (var serviceType in requiredServices)
            {
                var service = serviceProvider.GetService(serviceType);
                Assert.NotNull(service);
                
                System.Diagnostics.Debug.WriteLine($"✓ {serviceType.Name}: {service.GetType().Name}");
            }
        }

        [Fact]
        public void Diagnostic_EditorManager_With_Real_Factory_Should_Work()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // 使用实际的依赖注入配置
            services.AddSingleton<BannerlordModEditor.UI.Factories.IEditorFactory, BannerlordModEditor.UI.Factories.UnifiedEditorFactory>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Act - 创建EditorManagerViewModel
            var editorManager = new EditorManagerViewModel(
                serviceProvider.GetRequiredService<BannerlordModEditor.UI.Factories.IEditorFactory>(),
                serviceProvider.GetRequiredService<ILogService>(),
                serviceProvider.GetRequiredService<IErrorHandlerService>()
            );
            
            // Assert
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            
            System.Diagnostics.Debug.WriteLine($"=== Real EditorManager Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"Categories count: {editorManager.Categories.Count}");
            
            if (editorManager.Categories.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("PROBLEM: EditorManager has no categories. This explains the blank UI.");
            }
            else
            {
                foreach (var category in editorManager.Categories)
                {
                    System.Diagnostics.Debug.WriteLine($"Category: {category.Name} ({category.Editors.Count} editors)");
                }
            }
        }

        [Fact]
        public void Diagnostic_UnifiedEditorFactory_Should_Register_Editors()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            services.AddSingleton<BannerlordModEditor.UI.Factories.IEditorFactory, BannerlordModEditor.UI.Factories.UnifiedEditorFactory>();
            
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetRequiredService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            
            // Act
            var registeredTypes = factory.GetRegisteredEditorTypes();
            var allEditors = factory.GetAllEditors();
            
            // Assert
            Assert.NotNull(registeredTypes);
            Assert.NotNull(allEditors);
            
            System.Diagnostics.Debug.WriteLine($"=== UnifiedEditorFactory Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"Registered types: {registeredTypes.Count()}");
            System.Diagnostics.Debug.WriteLine($"Created editors: {allEditors.Count()}");
            
            if (registeredTypes.Count() == 0)
            {
                System.Diagnostics.Debug.WriteLine("PROBLEM: UnifiedEditorFactory registered no types!");
            }
            
            foreach (var type in registeredTypes)
            {
                System.Diagnostics.Debug.WriteLine($"  - {type}");
            }
        }

        [Fact]
        public void Diagnostic_Check_For_ViewModel_Constructor_Issues()
        {
            // 使用TestServiceProvider来创建ViewModel实例，而不是通过反射
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            
            // 尝试创建每个编辑器ViewModel
            var viewModelTypes = new[]
            {
                typeof(AttributeEditorViewModel),
                typeof(SkillEditorViewModel),
                typeof(BoneBodyTypeEditorViewModel)
            };
            
            // Act & Assert
            foreach (var viewModelType in viewModelTypes)
            {
                try
                {
                    var viewModel = serviceProvider.GetService(viewModelType);
                    Assert.NotNull(viewModel);
                    System.Diagnostics.Debug.WriteLine($"✓ {viewModelType.Name}: Created successfully via DI");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Failed to create {viewModelType.Name} via DI: {ex.Message}");
                }
            }
        }
    }
}