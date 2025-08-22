using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.UI.Services;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试辅助类，提供依赖注入服务
    /// </summary>
    public class TestServiceProvider
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider GetServiceProvider()
        {
            if (_serviceProvider == null)
            {
                var services = new ServiceCollection();
                
                // 注册核心服务
                services.AddSingleton<ILogService, LogService>();
                services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
                services.AddSingleton<IValidationService, ValidationService>();
                services.AddSingleton<IDataBindingService, DataBindingService>();
                
                // 注册编辑器工厂 - 使用统一的编辑器工厂
                services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
                
                // 注册所有编辑器ViewModel
                services.AddTransient<AttributeEditorViewModel>();
                services.AddTransient<SkillEditorViewModel>();
                services.AddTransient<CraftingPieceEditorViewModel>();
                services.AddTransient<ItemModifierEditorViewModel>();
                services.AddTransient<BoneBodyTypeEditorViewModel>();
                services.AddTransient<CombatParameterEditorViewModel>();
                services.AddTransient<ItemEditorViewModel>();
                
                // 注册其他服务
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<EditorManagerViewModel>();
                services.AddTransient<EditorCategoryViewModel>();
                
                // 注册Common层服务
                services.AddSingleton<IFileDiscoveryService, FileDiscoveryService>();
                
                _serviceProvider = services.BuildServiceProvider();
            }
            
            return _serviceProvider;
        }

        public static T GetService<T>() where T : notnull
        {
            return GetServiceProvider().GetRequiredService<T>();
        }

        public static void Reset()
        {
            _serviceProvider = null;
        }
    }
}