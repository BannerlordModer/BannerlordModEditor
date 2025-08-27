using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试用的模拟编辑器工厂
    /// </summary>
    public class MockEditorFactory : BannerlordModEditor.UI.Factories.IEditorFactory
    {
        private readonly Dictionary<string, EditorTypeInfo> _editorTypes = new();
        
        // 公开服务属性以供测试访问
        public IServiceProvider ServiceProvider { get; }
        public ILogService LogService { get; }
        public IErrorHandlerService ErrorHandlerService { get; }
        public IValidationService ValidationService { get; }
        public IDataBindingService DataBindingService { get; }

        public MockEditorFactory()
        {
            // 初始化服务
            var services = new ServiceCollection();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            ServiceProvider = services.BuildServiceProvider();
            LogService = ServiceProvider.GetRequiredService<ILogService>();
            ErrorHandlerService = ServiceProvider.GetRequiredService<IErrorHandlerService>();
            ValidationService = ServiceProvider.GetRequiredService<IValidationService>();
            DataBindingService = ServiceProvider.GetRequiredService<IDataBindingService>();
            
            // 初始化一些测试编辑器类型
            _editorTypes["AttributeEditor"] = new EditorTypeInfo 
            { 
                EditorType = "AttributeEditor", 
                DisplayName = "属性编辑器", 
                Category = "角色设定" 
            };
            _editorTypes["SkillEditor"] = new EditorTypeInfo 
            { 
                EditorType = "SkillEditor", 
                DisplayName = "技能编辑器", 
                Category = "角色设定" 
            };
            _editorTypes["BoneBodyTypeEditor"] = new EditorTypeInfo 
            { 
                EditorType = "BoneBodyTypeEditor", 
                DisplayName = "骨骼类型编辑器", 
                Category = "角色设定" 
            };
            _editorTypes["CombatParameterEditor"] = new EditorTypeInfo 
            { 
                EditorType = "CombatParameterEditor", 
                DisplayName = "战斗参数编辑器", 
                Category = "战斗系统" 
            };
            _editorTypes["ItemEditor"] = new EditorTypeInfo 
            { 
                EditorType = "ItemEditor", 
                DisplayName = "物品编辑器", 
                Category = "装备物品" 
            };
        }

        public ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
        {
            return editorType switch
            {
                "AttributeEditor" => new AttributeEditorViewModel(),
                "SkillEditor" => new SkillEditorViewModel(),
                "BoneBodyTypeEditor" => new BoneBodyTypeEditorViewModel(),
                "CombatParameterEditor" => new CombatParameterEditorViewModel(),
                "ItemEditor" => new ItemEditorViewModel(),
                _ => null
            };
        }

        public BaseEditorView? CreateEditorView(string editorType)
        {
            // 对于测试，返回null，因为我们只测试ViewModel
            return null;
        }

        public void RegisterEditor<TViewModel, TView>(string editorType)
            where TViewModel : ViewModelBase
            where TView : BaseEditorView
        {
            // 测试中不需要实现
        }

        public IEnumerable<string> GetRegisteredEditorTypes()
        {
            return _editorTypes.Keys;
        }

        public EditorTypeInfo? GetEditorTypeInfo(string editorType)
        {
            return _editorTypes.TryGetValue(editorType, out var info) ? info : null;
        }

        public IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category)
        {
            return _editorTypes.Values.Where(e => e.Category == category);
        }

        public IEnumerable<string> GetCategories()
        {
            return _editorTypes.Values.Select(e => e.Category).Distinct();
        }

        public void RegisterEditor<TViewModel, TView>(string editorType, 
            string displayName, string description, string xmlFileName, string category = "General")
            where TViewModel : ViewModelBase
            where TView : BaseEditorView
        {
            // 测试中不需要实现
        }

        public IEnumerable<ViewModelBase> GetAllEditors()
        {
            // 返回实际的测试编辑器实例，确保UI测试能够正常运行
            var serviceProvider = TestServiceProvider.GetServiceProvider();
            
            // 创建测试编辑器实例
            var editors = new List<ViewModelBase>();
            
            // 通过依赖注入创建编辑器实例
            try
            {
                var attributeEditor = serviceProvider.GetRequiredService<AttributeEditorViewModel>();
                attributeEditor.FilePath = "attributes.xml";
                editors.Add(attributeEditor);
                
                var skillEditor = serviceProvider.GetRequiredService<SkillEditorViewModel>();
                skillEditor.FilePath = "skills.xml";
                editors.Add(skillEditor);
                
                var boneBodyTypeEditor = serviceProvider.GetRequiredService<BoneBodyTypeEditorViewModel>();
                boneBodyTypeEditor.FilePath = "bone_body_types.xml";
                editors.Add(boneBodyTypeEditor);
                
                var combatParameterEditor = serviceProvider.GetRequiredService<CombatParameterEditorViewModel>();
                combatParameterEditor.FilePath = "combat_parameters.xml";
                editors.Add(combatParameterEditor);
                
                var itemEditor = serviceProvider.GetRequiredService<ItemEditorViewModel>();
                itemEditor.FilePath = "items.xml";
                editors.Add(itemEditor);
            }
            catch (Exception ex)
            {
                // 如果依赖注入失败，回退到手动创建实例
                Console.WriteLine($"警告: 依赖注入创建编辑器失败: {ex.Message}");
                
                // 手动创建编辑器实例作为回退方案
                try
                {
                    editors.Add(new AttributeEditorViewModel { FilePath = "attributes.xml" });
                    editors.Add(new SkillEditorViewModel { FilePath = "skills.xml" });
                    editors.Add(new BoneBodyTypeEditorViewModel { FilePath = "bone_body_types.xml" });
                    editors.Add(new CombatParameterEditorViewModel { FilePath = "combat_parameters.xml" });
                    editors.Add(new ItemEditorViewModel { FilePath = "items.xml" });
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"警告: 手动创建编辑器也失败: {ex2.Message}");
                    
                    // 如果还是失败，返回一些基本的Mock编辑器
                    editors.Add(new MockBaseEditorViewModel { FilePath = "attributes.xml", StatusMessage = "属性定义编辑器" });
                    editors.Add(new MockBaseEditorViewModel { FilePath = "skills.xml", StatusMessage = "技能编辑器" });
                    editors.Add(new MockBaseEditorViewModel { FilePath = "bone_body_types.xml", StatusMessage = "骨骼体型编辑器" });
                    editors.Add(new MockBaseEditorViewModel { FilePath = "combat_parameters.xml", StatusMessage = "战斗参数编辑器" });
                    editors.Add(new MockBaseEditorViewModel { FilePath = "items.xml", StatusMessage = "物品编辑器" });
                }
            }
            
            return editors;
        }
    }
}