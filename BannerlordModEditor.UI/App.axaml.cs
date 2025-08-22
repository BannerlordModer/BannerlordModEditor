using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Views;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.Services;
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
    /// </summary>
    private IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // 注册编辑器工厂
        services.AddSingleton<IEditorFactory, EnhancedEditorFactory>();
        
        // 注册验证和数据绑定服务
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        
        // 注册Common层服务
        services.AddTransient<BannerlordModEditor.Common.Services.IFileDiscoveryService, BannerlordModEditor.Common.Services.FileDiscoveryService>();
        
        // 注册所有编辑器ViewModel和View
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
        
        // 注册新的战斗参数编辑器
        services.AddTransient<CombatParameterEditorViewModel>();
        services.AddTransient<CombatParameterEditorView>();
        
        // 注册新的物品编辑器
        services.AddTransient<ItemEditorViewModel>();
        services.AddTransient<ItemEditorView>();
                
        return services;
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