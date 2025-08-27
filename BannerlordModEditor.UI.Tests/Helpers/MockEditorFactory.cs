using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试用的模拟编辑器工厂
    /// </summary>
    public class MockEditorFactory : BannerlordModEditor.UI.Factories.IEditorFactory
    {
        private readonly Dictionary<string, EditorTypeInfo> _editorTypes = new();
        private readonly IValidationService _validationService;

        public MockEditorFactory(IValidationService? validationService = null)
        {
            _validationService = validationService ?? new ValidationService();
            
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
                "AttributeEditor" => new AttributeEditorViewModel(_validationService),
                "SkillEditor" => new SkillEditorViewModel(),
                "BoneBodyTypeEditor" => new BoneBodyTypeEditorViewModel(),
                "CombatParameterEditor" => new CombatParameterEditorViewModel(_validationService),
                "ItemEditor" => new ItemEditorViewModel(_validationService),
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
            // 返回一些测试用的编辑器，这些编辑器应该有正确的EditorTypeAttribute
            // 但由于测试环境的限制，我们返回一个空列表，让EditorManager使用默认的编辑器列表
            return new List<ViewModelBase>();
        }
    }
}