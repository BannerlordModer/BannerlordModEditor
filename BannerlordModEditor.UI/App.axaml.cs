using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System;
using Avalonia.Markup.Xaml;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Views;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // 设置依赖注入
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            
            // 创建主窗口并设置服务提供器
            var mainWindow = new MainWindow();
            var mainViewModel = new MainWindowViewModel(serviceProvider);
            mainWindow.DataContext = mainViewModel;
            
            // 设置ViewLocator的服务提供器
            var viewLocator = new ViewLocator(serviceProvider);
            mainWindow.Resources["ViewLocator"] = viewLocator;
            
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 配置依赖注入服务
    /// 
    /// 这个方法使用新的扩展方法来配置所有EditorManager相关的服务，
    /// 解决了依赖注入歧义问题，并提供了统一的配置接口。
    /// 
    /// 主要改进：
    /// - 使用扩展方法简化服务注册
    /// - 提供配置验证和错误处理
    /// - 支持性能监控和健康检查
    /// - 确保服务注册的一致性
    /// </summary>
    /// <returns>配置完成的服务集合</returns>
    private IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // 使用扩展方法注册EditorManager相关服务
        services.AddEditorManagerServices(options =>
        {
            // 启用健康检查
            options.EnableHealthChecks = true;
            
            // 启用性能监控（可选）
            options.EnablePerformanceMonitoring = false;
            
            // 启用诊断功能（可选）
            options.EnableDiagnostics = false;
            
            // 确保所有核心服务都启用
            options.UseLogService = true;
            options.UseErrorHandlerService = true;
            options.UseValidationService = true;
            options.UseDataBindingService = true;
            options.UseEditorFactory = true;
        });
        
        // 注册Common层服务
        services.AddTransient<BannerlordModEditor.Common.Services.IFileDiscoveryService, BannerlordModEditor.Common.Services.FileDiscoveryService>();
        
        // 注册所有编辑器ViewModel和View
        RegisterEditorServices(services);
        
        // 验证服务注册
        ValidateServiceRegistration(services);
        
        return services;
    }

    /// <summary>
    /// 注册所有编辑器相关的服务
    /// </summary>
    /// <param name="services">服务集合</param>
    private void RegisterEditorServices(IServiceCollection services)
    {
        // 注册编辑器ViewModel和View
        services.AddTransient<AttributeEditorViewModel>();
        services.AddTransient<AttributeEditorView>();
        services.AddTransient<SkillEditorViewModel>();
        services.AddTransient<SkillEditorView>();
        services.AddTransient<BoneBodyTypeEditorViewModel>();
        services.AddTransient<BoneBodyTypeEditorView>();
        services.AddTransient<CraftingPieceEditorViewModel>();
        services.AddTransient<CraftingPieceEditorView>();
        services.AddTransient<ItemModifierEditorViewModel>();
        services.AddTransient<ItemModifierEditorView>();
        
        // 注册战斗参数编辑器
        services.AddTransient<CombatParameterEditorViewModel>();
        services.AddTransient<CombatParameterEditorView>();
        
        // 注册物品编辑器
        services.AddTransient<ItemEditorViewModel>();
        services.AddTransient<ItemEditorView>();
    }

    /// <summary>
    /// 验证服务注册是否正确
    /// </summary>
    /// <param name="services">服务集合</param>
    private void ValidateServiceRegistration(IServiceCollection services)
    {
        try
        {
            var validationResult = services.ValidateEditorManagerServices();
            
            if (!validationResult.IsValid)
            {
                // 在调试模式下，输出详细的验证信息
                #if DEBUG
                System.Diagnostics.Debug.WriteLine($"Service registration validation failed:{Environment.NewLine}{validationResult}");
                #endif
                
                // 对于生产环境，记录错误但继续运行（因为有些服务可能是可选的）
                // 在实际项目中，这里应该使用日志服务记录错误
                System.Console.WriteLine($"Warning: Some required services may not be properly registered");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("All services registered successfully");
            }
        }
        catch (Exception ex)
        {
            // 验证过程中出现异常，记录错误但继续运行
            System.Console.WriteLine($"Error during service validation: {ex.Message}");
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}