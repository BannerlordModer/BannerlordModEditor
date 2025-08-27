using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests.Extensions
{
    /// <summary>
    /// 依赖注入服务注册扩展方法
    /// 
    /// 这个类提供了统一的服务注册方法，确保测试环境和实际应用环境
    /// 使用相同的依赖注入配置。
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册核心基础设施服务
        /// </summary>
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // 注册核心服务
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            // 注册选项模式配置
            services.AddOptions();
            
            return services;
        }

        /// <summary>
        /// 注册编辑器工厂
        /// </summary>
        public static IServiceCollection AddEditorFactory(this IServiceCollection services, bool useMockFactory = true)
        {
            if (useMockFactory)
            {
                services.AddSingleton<IEditorFactory, MockEditorFactory>();
            }
            else
            {
                // 实际应用的编辑器工厂
                // services.AddSingleton<IEditorFactory, EditorFactory>();
            }
            
            return services;
        }

        /// <summary>
        /// 注册所有编辑器ViewModel
        /// </summary>
        public static IServiceCollection AddEditorViewModels(this IServiceCollection services)
        {
            // 基础编辑器
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<CombatParameterEditorViewModel>();
            services.AddTransient<ItemEditorViewModel>();
            
            // 高级编辑器
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            
            // 通用编辑器
            // services.AddTransient<GenericEditorViewModel>();
            // services.AddTransient<SimpleEditorViewModel>();
            
            return services;
        }

        /// <summary>
        /// 注册主要ViewModel
        /// </summary>
        public static IServiceCollection AddMainViewModels(this IServiceCollection services)
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<EditorManagerViewModel>();
            services.AddTransient<EditorCategoryViewModel>();
            services.AddTransient<EditorItemViewModel>();
            
            return services;
        }

        /// <summary>
        /// 注册Common层服务
        /// </summary>
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileDiscoveryService, FileDiscoveryService>();
            
            // 其他Common层服务
            // services.AddSingleton<INamingConventionMapper, NamingConventionMapper>();
            
            return services;
        }

        /// <summary>
        /// 注册测试专用服务
        /// </summary>
        public static IServiceCollection AddTestOnlyServices(this IServiceCollection services)
        {
            // 注册测试用的模拟ViewModel
            // services.AddTransient<MockMainWindowViewModel>();
            // services.AddTransient<MockEditorManager>();
            
            return services;
        }

        /// <summary>
        /// 配置完整的测试服务
        /// </summary>
        public static IServiceCollection AddTestServices(this IServiceCollection services)
        {
            return services.AddTestServices(useMockFactory: true);
        }

        /// <summary>
        /// 配置完整的测试服务（重载方法）
        /// </summary>
        public static IServiceCollection AddTestServices(this IServiceCollection services, bool useMockFactory = true)
        {
            services
                .AddCoreServices()
                .AddEditorFactory(useMockFactory: useMockFactory)
                .AddEditorViewModels()
                .AddMainViewModels()
                .AddCommonServices();
                
            return services;
        }

        /// <summary>
        /// 配置完整的应用服务
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCoreServices()
                .AddEditorFactory(useMockFactory: false)
                .AddEditorViewModels()
                .AddMainViewModels()
                .AddCommonServices();
                
            return services;
        }

        /// <summary>
        /// 验证服务配置
        /// </summary>
        public static bool ValidateServiceConfiguration(this IServiceProvider serviceProvider)
        {
            try
            {
                // 验证核心服务
                var requiredServices = new[]
                {
                    typeof(ILogService),
                    typeof(IErrorHandlerService),
                    typeof(IValidationService),
                    typeof(IDataBindingService),
                    typeof(IEditorFactory)
                };
                
                foreach (var serviceType in requiredServices)
                {
                    var service = serviceProvider.GetService(serviceType);
                    if (service == null)
                    {
                        System.Console.WriteLine($"缺少必需服务: {serviceType.Name}");
                        return false;
                    }
                }
                
                // 验证编辑器ViewModel
                var editorViewModels = new[]
                {
                    typeof(AttributeEditorViewModel),
                    typeof(SkillEditorViewModel),
                    typeof(CombatParameterEditorViewModel),
                    typeof(ItemEditorViewModel)
                };
                
                foreach (var viewModelType in editorViewModels)
                {
                    var viewModel = serviceProvider.GetService(viewModelType);
                    if (viewModel == null)
                    {
                        System.Console.WriteLine($"缺少编辑器ViewModel: {viewModelType.Name}");
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"服务配置验证失败: {ex.Message}");
                return false;
            }
        }
    }
}