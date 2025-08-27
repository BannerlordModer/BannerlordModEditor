using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

class TestProgram
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("开始测试依赖注入和编辑器工厂...");
            
            // 配置依赖注入服务
            var services = new ServiceCollection();
            
            // 注册编辑器工厂
            services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
            
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
            
            Console.WriteLine("✓ 依赖注入容器创建成功");
            
            // 测试编辑器工厂
            var editorFactory = serviceProvider.GetRequiredService<IEditorFactory>();
            Console.WriteLine("✓ 编辑器工厂创建成功");
            
            // 测试获取所有编辑器
            var allEditors = editorFactory.GetAllEditors();
            Console.WriteLine($"✓ 成功获取 {allEditors.Count()} 个编辑器");
            
            // 测试EditorManagerViewModel
            var editorManager = new EditorManagerViewModel(editorFactory);
            Console.WriteLine("✓ EditorManagerViewModel 创建成功");
            Console.WriteLine($"✓ 加载了 {editorManager.Categories.Count} 个编辑器分类");
            
            // 测试MainWindowViewModel
            var mainViewModel = new MainWindowViewModel(serviceProvider);
            Console.WriteLine("✓ MainWindowViewModel 创建成功");
            Console.WriteLine($"✓ EditorManager 属性: {mainViewModel.EditorManager?.Categories.Count ?? 0} 个分类");
            
            // 列出所有注册的编辑器类型
            var registeredTypes = editorFactory.GetRegisteredEditorTypes();
            Console.WriteLine("\n已注册的编辑器类型:");
            foreach (var type in registeredTypes)
            {
                var info = editorFactory.GetEditorTypeInfo(type);
                Console.WriteLine($"  - {type}: {info?.DisplayName ?? "N/A"} ({info?.Category ?? "N/A"})");
            }
            
            // 列出所有分类
            var categories = editorFactory.GetCategories();
            Console.WriteLine("\n编辑器分类:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  - {category}");
            }
            
            // 测试创建具体编辑器
            var attributeEditor = editorFactory.CreateEditorViewModel("AttributeEditor", "attributes.xml");
            if (attributeEditor != null)
            {
                Console.WriteLine("\n✓ AttributeEditor 创建成功");
                Console.WriteLine($"  - 类型: {attributeEditor.GetType().Name}");
                Console.WriteLine($"  - 编辑器类型: {attributeEditor.EditorType}");
                Console.WriteLine($"  - XML文件: {attributeEditor.XmlFileName}");
            }
            else
            {
                Console.WriteLine("\n✗ AttributeEditor 创建失败");
            }
            
            // 测试默认编辑器
            Console.WriteLine("\n默认编辑器分类:");
            foreach (var category in editorManager.Categories)
            {
                Console.WriteLine($"  - {category.Name} ({category.Description})");
                foreach (var editor in category.Editors)
                {
                    Console.WriteLine($"    - {editor.Name} ({editor.EditorType})");
                }
            }
            
            Console.WriteLine("\n测试完成！");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"测试失败: {ex.Message}");
            Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
        }
    }
}