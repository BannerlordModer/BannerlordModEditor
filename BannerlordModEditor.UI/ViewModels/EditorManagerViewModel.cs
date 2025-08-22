using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BannerlordModEditor.UI.ViewModels.Editors;
using System;
using System.Reflection;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BannerlordModEditor.UI.ViewModels;

public partial class EditorManagerViewModel : ViewModelBase
{
    private readonly IEditorFactory _editorFactory;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;

    [ObservableProperty]
    private ObservableCollection<EditorCategoryViewModel> categories = new();

    [ObservableProperty]
    private ViewModelBase? selectedEditor;

    [ObservableProperty]
    private string? statusMessage;

    [ObservableProperty]
    private ViewModelBase? currentEditorViewModel;

    [ObservableProperty]
    private string? currentBreadcrumb;

    [ObservableProperty]
    private string? searchText;

    public EditorManagerViewModel(
        IEditorFactory? editorFactory = null,
        ILogService? logService = null,
        IErrorHandlerService? errorHandlerService = null)
    {
        _editorFactory = editorFactory;
        _logService = logService ?? new LogService();
        _errorHandlerService = errorHandlerService ?? new ErrorHandlerService();

        LoadEditors();
    }

    private void LoadEditors()
    {
        try
        {
            if (_editorFactory == null)
            {
                // 创建默认的编辑器分类并添加测试编辑器
                var characterCategory = new EditorCategoryViewModel("角色设定", "角色设定编辑器", "👤");
                characterCategory.Editors.Add(new EditorItemViewModel("属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️"));
                characterCategory.Editors.Add(new EditorItemViewModel("技能系统", "技能系统编辑器", "skills.xml", "SkillEditor", "🎯"));
                
                var equipmentCategory = new EditorCategoryViewModel("装备物品", "装备物品编辑器", "⚔️");
                equipmentCategory.Editors.Add(new EditorItemViewModel("物品编辑", "物品编辑器", "items.xml", "ItemEditor", "📦"));
                
                var combatCategory = new EditorCategoryViewModel("战斗系统", "战斗系统编辑器", "🛡️");
                combatCategory.Editors.Add(new EditorItemViewModel("战斗参数", "战斗参数编辑器", "combat_parameters.xml", "CombatParameterEditor", "⚔️"));
                
                Categories = new ObservableCollection<EditorCategoryViewModel>
                {
                    characterCategory,
                    equipmentCategory,
                    combatCategory,
                    new EditorCategoryViewModel("世界场景", "世界场景编辑器", "🌍"),
                    new EditorCategoryViewModel("音频系统", "音频系统编辑器", "🎵"),
                    new EditorCategoryViewModel("多人游戏", "多人游戏编辑器", "👥"),
                    new EditorCategoryViewModel("游戏配置", "游戏配置编辑器", "⚙️")
                };
                StatusMessage = "已加载默认编辑器分类";
                return;
            }

            var editors = _editorFactory.GetAllEditors();
            var groupedEditors = editors.GroupBy(e => GetEditorCategory(e))
                .Select(g => new EditorCategoryViewModel(g.Key, $"{g.Key} 编辑器", "📁"));

            Categories = new ObservableCollection<EditorCategoryViewModel>(groupedEditors);
            StatusMessage = $"已加载 {editors.Count()} 个编辑器";
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "加载编辑器失败");
            StatusMessage = "加载编辑器失败";
        }
    }

    private string GetEditorCategory(ViewModelBase editor)
    {
        try
        {
            var editorType = editor.GetType();
            var categoryAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
            return categoryAttribute?.Category ?? "其他";
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Unexpected error while getting category name");
            return "错误分类";
        }
    }

    /// <summary>
    /// 自动加载XML文件（如果编辑器支持）
    /// </summary>
    private async Task AutoLoadXmlFileAsync(ViewModelBase editorViewModel, string xmlFileName)
    {
        try
        {
            // 检查编辑器是否有LoadXmlFile方法
            var loadMethod = editorViewModel.GetType().GetMethod("LoadXmlFile");
            if (loadMethod != null)
            {
                // 在后台线程中执行XML加载
                await Task.Run(() => loadMethod.Invoke(editorViewModel, new object[] { xmlFileName }));
                _logService.LogInfo($"Successfully auto-loaded XML file: {xmlFileName}", "EditorManager");
            }
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Failed to auto-load XML file: {xmlFileName}");
        }
    }

    [RelayCommand]
    private void SelectEditor(ViewModelBase editor)
    {
        try
        {
            // 如果传入的是EditorItemViewModel，需要转换为具体的编辑器ViewModel
            ViewModelBase actualEditor = editor;
            if (editor is EditorItemViewModel editorItem)
            {
                actualEditor = CreateEditorViewModel(editorItem);
            }

            SelectedEditor = actualEditor;
            CurrentEditorViewModel = actualEditor;
            StatusMessage = $"已选择编辑器: {actualEditor.GetType().Name}";

            // 更新面包屑导航
            var editorType = actualEditor.GetType();
            var editorAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
            CurrentBreadcrumb = $"{editorAttribute?.Category ?? "其他"} > {editorType.Name}";

            // 获取编辑器的XML文件名并自动加载
            if (editorAttribute?.XmlFileName != null)
            {
                // 异步加载XML文件，不等待完成
                _ = AutoLoadXmlFileAsync(actualEditor, editorAttribute.XmlFileName);
            }
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "选择编辑器失败");
            StatusMessage = "选择编辑器失败";
        }
    }

    private ViewModelBase CreateEditorViewModel(EditorItemViewModel editorItem)
    {
        return editorItem.EditorType switch
        {
            "AttributeEditor" => new AttributeEditorViewModel(),
            "SkillEditor" => new SkillEditorViewModel(),
            "CombatParameterEditor" => new CombatParameterEditorViewModel(),
            "ItemEditor" => new ItemEditorViewModel(),
            "BoneBodyTypeEditor" => new BoneBodyTypeEditorViewModel(),
            "CraftingPieceEditor" => new CraftingPieceEditorViewModel(),
            "ItemModifierEditor" => new ItemModifierEditorViewModel(),
            _ => throw new NotSupportedException($"不支持的编辑器类型: {editorItem.EditorType}")
        };
    }

    [RelayCommand]
    private void RefreshEditors()
    {
        LoadEditors();
    }

    [RelayCommand]
    private void ShowHelp()
    {
        try
        {
            StatusMessage = "查看帮助信息";
            // TODO: 实现帮助对话框
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "显示帮助失败");
        }
    }

    partial void OnSelectedEditorChanged(ViewModelBase? value)
    {
        if (value != null)
        {
            CurrentEditorViewModel = value;
            var editorType = value.GetType();
            var editorAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
            CurrentBreadcrumb = $"{editorAttribute?.Category ?? "其他"} > {editorType.Name}";
        }
        else
        {
            CurrentEditorViewModel = null;
            CurrentBreadcrumb = null;
        }
    }
}