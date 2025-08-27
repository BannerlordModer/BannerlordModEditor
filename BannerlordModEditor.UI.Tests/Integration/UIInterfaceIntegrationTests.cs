using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.UI.Tests.Integration;

/// <summary>
/// 集成测试：验证修复后的UI界面是否正常工作
/// </summary>
public class UIInterfaceIntegrationTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dispatcher _dispatcher;

    public UIInterfaceIntegrationTests(ITestOutputHelper output)
    {
        _output = output;
        
        // 配置依赖注入服务
        var services = new ServiceCollection();
        
        // 注册编辑器工厂 - 使用MockEditorFactory进行测试
        services.AddSingleton<IEditorFactory, MockEditorFactory>();
        
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
        
        _serviceProvider = services.BuildServiceProvider();
        
        // 初始化Dispatcher（用于UI测试）
        _dispatcher = Dispatcher.UIThread;
    }

    [Fact]
    public async Task MainWindowViewModel_Should_Load_Editor_Categories()
    {
        await _dispatcher.InvokeAsync(() =>
        {
            // 创建MainWindowViewModel
            var mainWindowViewModel = new MainWindowViewModel(_serviceProvider);
            
            // 验证EditorManager不为空
            Assert.NotNull(mainWindowViewModel.EditorManager);
            
            // 验证Categories被正确加载
            Assert.NotNull(mainWindowViewModel.EditorManager.Categories);
            Assert.NotEmpty(mainWindowViewModel.EditorManager.Categories);
            
            // 验证有角色设定分类
            var characterCategory = mainWindowViewModel.EditorManager.Categories
                .FirstOrDefault(c => c.Name == "角色设定");
            Assert.NotNull(characterCategory);
            
            // 验证该分类下有编辑器
            Assert.NotEmpty(characterCategory.Editors);
            
            _output.WriteLine($"成功加载了 {mainWindowViewModel.EditorManager.Categories.Count} 个分类");
            _output.WriteLine($"角色设定分类包含 {characterCategory.Editors.Count} 个编辑器");
        });
    }

    [Fact]
    public async Task EditorManagerViewModel_Should_Create_Editors_Via_Factory()
    {
        await _dispatcher.InvokeAsync(() =>
        {
            // 创建EditorManagerViewModel
            var editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
            var editorManager = new EditorManagerViewModel(editorFactory);
            
            // 验证Categories被正确加载
            Assert.NotNull(editorManager.Categories);
            Assert.NotEmpty(editorManager.Categories);
            
            // 验证工厂能够创建编辑器
            var characterCategory = editorManager.Categories
                .FirstOrDefault(c => c.Name == "角色设定");
            Assert.NotNull(characterCategory);
            
            // 尝试创建属性编辑器
            var attributeEditor = characterCategory.Editors
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");
            Assert.NotNull(attributeEditor);
            
            // 通过工厂创建编辑器ViewModel
            var viewModel = editorFactory.CreateEditorViewModel(attributeEditor.EditorType, attributeEditor.XmlFileName);
            Assert.NotNull(viewModel);
            Assert.IsType<AttributeEditorViewModel>(viewModel);
            
            _output.WriteLine($"成功创建了编辑器: {viewModel.GetType().Name}");
        });
    }

    [Fact]
    public async Task UnifiedEditorFactory_Should_Register_All_Editors()
    {
        await _dispatcher.InvokeAsync(() =>
        {
            // 创建UnifiedEditorFactory
            var editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
            
            // 验证工厂不为空
            Assert.NotNull(editorFactory);
            
            // 获取所有注册的编辑器类型
            var registeredTypes = editorFactory.GetRegisteredEditorTypes();
            Assert.NotEmpty(registeredTypes);
            
            // 验证特定的编辑器类型已注册
            Assert.Contains("AttributeEditor", registeredTypes);
            Assert.Contains("SkillEditor", registeredTypes);
            Assert.Contains("BoneBodyTypeEditor", registeredTypes);
            
            _output.WriteLine($"已注册的编辑器类型: {string.Join(", ", registeredTypes)}");
        });
    }

    [Fact]
    public async Task EditorItemViewModel_Should_Have_Correct_Properties()
    {
        await _dispatcher.InvokeAsync(() =>
        {
            // 创建EditorItemViewModel
            var editorItem = new EditorItemViewModel(
                "测试编辑器",
                "这是一个测试编辑器",
                "test.xml",
                "TestEditor",
                "🧪"
            );
            
            // 验证属性设置正确
            Assert.Equal("测试编辑器", editorItem.Name);
            Assert.Equal("这是一个测试编辑器", editorItem.Description);
            Assert.Equal("test.xml", editorItem.XmlFileName);
            Assert.Equal("TestEditor", editorItem.EditorType);
            Assert.Equal("🧪", editorItem.Icon);
            Assert.True(editorItem.IsAvailable);
            
            _output.WriteLine($"EditorItemViewModel创建成功: {editorItem.Name}");
        });
    }

    [Fact]
    public async Task Integration_MainWindowViewModel_To_EditorManager_Flow()
    {
        await _dispatcher.InvokeAsync(() =>
        {
            // 完整的集成测试：MainWindowViewModel -> EditorManager -> EditorFactory -> EditorViewModel
            var mainWindowViewModel = new MainWindowViewModel(_serviceProvider);
            
            // 1. 验证MainWindowViewModel创建成功
            Assert.NotNull(mainWindowViewModel);
            Assert.NotNull(mainWindowViewModel.EditorManager);
            
            // 2. 验证EditorManager有分类
            var editorManager = mainWindowViewModel.EditorManager;
            Assert.NotEmpty(editorManager.Categories);
            
            // 3. 选择一个编辑器
            var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");
            Assert.NotNull(characterCategory);
            
            var attributeEditor = characterCategory.Editors.FirstOrDefault(e => e.EditorType == "AttributeEditor");
            Assert.NotNull(attributeEditor);
            
            // 4. 通过EditorManager选择编辑器
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            
            // 5. 验证CurrentEditorViewModel被设置
            Assert.NotNull(editorManager.CurrentEditorViewModel);
            Assert.IsType<AttributeEditorViewModel>(editorManager.CurrentEditorViewModel);
            
            // 6. 验证面包屑导航
            Assert.NotNull(editorManager.CurrentBreadcrumb);
            Assert.Contains("角色设定", editorManager.CurrentBreadcrumb);
            Assert.Contains("属性编辑器", editorManager.CurrentBreadcrumb);
            
            _output.WriteLine($"完整流程测试成功: {editorManager.CurrentBreadcrumb}");
        });
    }

    public void Dispose()
    {
        // 清理资源
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}