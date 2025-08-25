using Xunit;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Tests.Helpers;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Services;

namespace BannerlordModEditor.UI.Tests.Diagnostic
{
    /// <summary>
    /// 检查真实UI应用程序配置的诊断测试
    /// </summary>
    public class RealUIConfigurationDiagnosticTests
    {
        [Fact]
        public void Diagnostic_Real_App_Dependency_Injection_Should_Work()
        {
            // Arrange - 模拟App.ConfigureServices()的实际配置
            var services = new ServiceCollection();
            
            // 注册编辑器工厂
            services.AddSingleton<BannerlordModEditor.UI.Factories.IEditorFactory, BannerlordModEditor.UI.Factories.UnifiedEditorFactory>();
            
            // 注册日志和错误处理服务
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            
            // 注册验证和数据绑定服务
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            // 注册Common层服务
            services.AddTransient<BannerlordModEditor.Common.Services.IFileDiscoveryService, BannerlordModEditor.Common.Services.FileDiscoveryService>();
            
            // 注册所有编辑器ViewModel
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            services.AddTransient<CombatParameterEditorViewModel>();
            services.AddTransient<ItemEditorViewModel>();
            
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert - 验证关键服务
            var editorFactory = serviceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            Assert.NotNull(editorFactory);
            Assert.IsType<BannerlordModEditor.UI.Factories.UnifiedEditorFactory>(editorFactory);

            // 验证EditorManagerViewModel可以正常创建
            var editorManager = new EditorManagerViewModel(
                editorFactory,
                serviceProvider.GetRequiredService<ILogService>(),
                serviceProvider.GetRequiredService<IErrorHandlerService>()
            );
            
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            
            System.Diagnostics.Debug.WriteLine($"=== Real App Configuration Diagnostic ===");
            System.Diagnostics.Debug.WriteLine($"EditorManager categories: {editorManager.Categories.Count}");
            
            if (editorManager.Categories.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("✓ EditorManager has categories - this should make UI work");
                
                var totalEditors = editorManager.Categories.Sum(c => c.Editors.Count);
                System.Diagnostics.Debug.WriteLine($"Total editors: {totalEditors}");
                
                if (totalEditors == 0)
                {
                    System.Diagnostics.Debug.WriteLine("⚠ Categories exist but no editors - UI might be empty");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("✓ Editors found - UI should show content");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ No categories - this explains blank UI");
            }
        }

        [Fact]
        public void Diagnostic_UnifiedEditorFactory_With_Real_Services_Should_Register_Editors()
        {
            // Arrange - 完整的服务配置
            var services = new ServiceCollection();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            services.AddSingleton<BannerlordModEditor.UI.Factories.IEditorFactory, BannerlordModEditor.UI.Factories.UnifiedEditorFactory>();
            
            // 注册编辑器ViewModel以便工厂可以创建它们
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetRequiredService<BannerlordModEditor.UI.Factories.IEditorFactory>();
            
            // Act
            var registeredTypes = factory.GetRegisteredEditorTypes();
            var allEditors = factory.GetAllEditors();
            
            // Assert
            Assert.NotNull(registeredTypes);
            Assert.NotNull(allEditors);
            
            System.Diagnostics.Debug.WriteLine($"=== UnifiedEditorFactory with Real Services ===");
            System.Diagnostics.Debug.WriteLine($"Registered types: {registeredTypes.Count()}");
            System.Diagnostics.Debug.WriteLine($"Created editors: {allEditors.Count()}");
            
            if (registeredTypes.Count() == 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ CRITICAL: UnifiedEditorFactory registered no types!");
                System.Diagnostics.Debug.WriteLine("This is the root cause of the blank UI.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("✓ Editor types are registered");
                
                if (allEditors.Count() == 0)
                {
                    System.Diagnostics.Debug.WriteLine("⚠ Types are registered but no editor instances created");
                    System.Diagnostics.Debug.WriteLine("This suggests dependency injection issues with editor ViewModels");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("✓ Editor instances created successfully");
                }
            }
        }

        [Fact]
        public void Diagnostic_Check_Editor_ViewModel_Dependencies()
        {
            // Arrange - 检查每个ViewModel的依赖需求
            var viewModelTypes = new[]
            {
                typeof(AttributeEditorViewModel),
                typeof(SkillEditorViewModel),
                typeof(BoneBodyTypeEditorViewModel),
                typeof(CraftingPieceEditorViewModel),
                typeof(ItemModifierEditorViewModel)
            };
            
            System.Diagnostics.Debug.WriteLine($"=== Editor ViewModel Dependencies ===");
            
            foreach (var viewModelType in viewModelTypes)
            {
                var constructors = viewModelType.GetConstructors();
                System.Diagnostics.Debug.WriteLine($"{viewModelType.Name}:");
                
                if (constructors.Length == 0)
                {
                    System.Diagnostics.Debug.WriteLine("  ❌ No public constructors");
                }
                else
                {
                    foreach (var constructor in constructors)
                    {
                        var parameters = constructor.GetParameters();
                        System.Diagnostics.Debug.WriteLine($"  Constructor with {parameters.Length} parameters:");
                        
                        foreach (var param in parameters)
                        {
                            System.Diagnostics.Debug.WriteLine($"    - {param.ParameterType.Name} {param.Name}");
                        }
                        
                        if (parameters.Length == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("    ✓ Parameterless constructor available");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("    ⚠ Requires dependency injection");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Diagnostic_EditorManager_Default_Fallback_Should_Work()
        {
            // Arrange - 创建没有工厂的EditorManager来测试默认配置
            var editorManager = new EditorManagerViewModel(
                null, // No factory
                new LogService(),
                new ErrorHandlerService()
            );
            
            // Assert
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            
            System.Diagnostics.Debug.WriteLine($"=== EditorManager Default Fallback ===");
            System.Diagnostics.Debug.WriteLine($"Categories count: {editorManager.Categories.Count}");
            
            if (editorManager.Categories.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("✓ Default fallback is working");
                
                var totalEditors = editorManager.Categories.Sum(c => c.Editors.Count);
                System.Diagnostics.Debug.WriteLine($"Total editors in fallback: {totalEditors}");
                
                if (totalEditors > 0)
                {
                    System.Diagnostics.Debug.WriteLine("✓ Fallback has editors - UI should work even without factory");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("⚠ Fallback has no editors");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ Even fallback is not working");
            }
        }
    }
}