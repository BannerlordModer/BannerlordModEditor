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
    private string? searchText = string.Empty;

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // 显示所有编辑器
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = true;
                }
            }
        }
        else
        {
            // 过滤编辑器
            var searchLower = value.ToLower();
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = editor.Name.ToLower().Contains(searchLower) ||
                                        editor.Description.ToLower().Contains(searchLower) ||
                                        editor.EditorType.ToLower().Contains(searchLower);
                }
            }
        }
    }

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
                LoadDefaultEditors();
                return;
            }

            var editors = _editorFactory.GetAllEditors();
            if (editors == null || !editors.Any())
            {
                // 如果工厂没有返回编辑器，使用默认配置
                LoadDefaultEditors();
                return;
            }

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

    /// <summary>
    /// 加载默认的编辑器配置
    /// </summary>
    private void LoadDefaultEditors()
    {
        // 创建默认的编辑器分类并添加测试编辑器
        var characterCategory = new EditorCategoryViewModel("角色设定", "角色设定编辑器", "👤");
        characterCategory.Editors.Add(new EditorItemViewModel("属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️"));
        characterCategory.Editors.Add(new EditorItemViewModel("技能系统", "技能系统编辑器", "skills.xml", "SkillEditor", "🎯"));
        characterCategory.Editors.Add(new EditorItemViewModel("骨骼体型", "骨骼体型编辑器", "bone_body_types.xml", "BoneBodyTypeEditor", "🦴"));
        
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
    /// 根据EditorItemViewModel获取分类名称
    /// </summary>
    private string GetCategoryFromEditorItem(EditorItemViewModel editorItem)
    {
        // 根据编辑器类型返回对应的分类名称
        return editorItem.EditorType switch
        {
            "AttributeEditor" => "角色设定",
            "SkillEditor" => "角色设定",
            "BoneBodyTypeEditor" => "角色设定",
            "ItemEditor" => "装备物品",
            "CombatParameterEditor" => "战斗系统",
            "CraftingPieceEditor" => "装备物品",
            "ItemModifierEditor" => "装备物品",
            _ => "其他"
        };
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
            EditorItemViewModel? editorItem = null;
            
            if (editor is EditorItemViewModel item)
            {
                editorItem = item;
                actualEditor = CreateEditorViewModel(editorItem);
            }

            // 保持SelectedEditor为传入的原始对象（可能是EditorItemViewModel）
            SelectedEditor = editor;
            CurrentEditorViewModel = actualEditor;
            StatusMessage = $"已选择编辑器: {actualEditor.GetType().Name}";

            // 更新面包屑导航 - 优先使用EditorItemViewModel的信息
            string categoryName;
            string editorName;
            string xmlFileName;
            
            if (editorItem != null)
            {
                // 使用EditorItemViewModel的信息
                categoryName = GetCategoryFromEditorItem(editorItem);
                editorName = editorItem.Name;
                xmlFileName = editorItem.XmlFileName;
            }
            else
            {
                // 回退到使用ViewModel的属性
                var editorType = actualEditor.GetType();
                var editorAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
                categoryName = editorAttribute?.Category ?? "其他";
                editorName = editorAttribute?.DisplayName ?? editorType.Name.Replace("ViewModel", "");
                xmlFileName = editorAttribute?.XmlFileName ?? "";
                
                if (string.IsNullOrEmpty(xmlFileName) && actualEditor is BaseEditorViewModel baseEditor)
                {
                    xmlFileName = baseEditor.XmlFileName;
                }
            }
            
            CurrentBreadcrumb = $"{categoryName} > {editorName}";
            
            if (!string.IsNullOrEmpty(xmlFileName))
            {
                // 异步加载XML文件，不等待完成
                _ = AutoLoadXmlFileAsync(actualEditor, xmlFileName);
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
        try
        {
            if (_editorFactory != null)
            {
                var viewModel = _editorFactory.CreateEditorViewModel(editorItem.EditorType, editorItem.XmlFileName);
                if (viewModel != null)
                {
                    return viewModel;
                }
                _logService.LogWarning($"Failed to create editor via factory: {editorItem.EditorType}", "EditorManager");
            }

            // 回退到直接创建（用于测试或没有工厂的情况）
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
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Failed to create editor view model: {editorItem.EditorType}");
            throw;
        }
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