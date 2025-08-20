using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BannerlordModEditor.UI.ViewModels.Editors;
using System;
using BannerlordModEditor.UI.Factories;

namespace BannerlordModEditor.UI.ViewModels;

public partial class EditorManagerViewModel : ViewModelBase
{
    private readonly IEditorFactory _editorFactory;

    [ObservableProperty]
    private ObservableCollection<EditorCategoryViewModel> categories = new();

    [ObservableProperty]
    private EditorItemViewModel? selectedEditor;

    [ObservableProperty]
    private ViewModelBase? currentEditorViewModel;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string currentBreadcrumb = "选择要编辑的XML文件";

    public EditorManagerViewModel(IEditorFactory editorFactory)
    {
        _editorFactory = editorFactory;
        InitializeCategories();
    }

    private void InitializeCategories()
    {
        // 角色相关
        var characterCategory = new EditorCategoryViewModel("角色设定", "角色属性、技能、外观等配置", "👤");
        characterCategory.Editors.Add(new EditorItemViewModel("属性定义", "游戏角色基础属性配置", "attributes.xml", "AttributeEditor", "⚡"));
        characterCategory.Editors.Add(new EditorItemViewModel("技能系统", "角色技能和修饰符配置", "skills.xml", "SkillEditor", "🎯"));
        characterCategory.Editors.Add(new EditorItemViewModel("骨骼体型", "角色骨骼碰撞体配置", "bone_body_types.xml", "BoneBodyTypeEditor", "🦴"));
        
        // 装备相关
        var equipmentCategory = new EditorCategoryViewModel("装备物品", "武器、盔甲、物品等配置", "⚔️");
        equipmentCategory.Editors.Add(new EditorItemViewModel("物品数据", "游戏物品基础数据配置", "mpitems.xml", "ItemEditor", "📦"));
        equipmentCategory.Editors.Add(new EditorItemViewModel("物品修饰符", "装备属性修饰符配置", "item_modifiers.xml", "ItemModifierEditor", "✨"));
        equipmentCategory.Editors.Add(new EditorItemViewModel("制作部件", "装备制作系统部件", "crafting_pieces.xml", "CraftingPieceEditor", "🔧"));
        equipmentCategory.Editors.Add(new EditorItemViewModel("制作模板", "装备制作模板配置", "crafting_templates.xml", "CraftingTemplateEditor", "📋"));

        // 战斗相关
        var combatCategory = new EditorCategoryViewModel("战斗系统", "战斗参数、武器、攻城器械等", "⚔️");
        combatCategory.Editors.Add(new EditorItemViewModel("战斗参数", "战斗系统基础参数", "combat_parameters.xml", "CombatParameterEditor", "🛡️"));
        combatCategory.Editors.Add(new EditorItemViewModel("攻城器械", "攻城战器械配置", "siegeengines.xml", "SiegeEngineEditor", "🏰"));
        combatCategory.Editors.Add(new EditorItemViewModel("武器描述", "武器详细描述数据", "weapon_descriptions.xml", "WeaponDescriptionEditor", "🗡️"));

        // 世界场景
        var worldCategory = new EditorCategoryViewModel("世界场景", "地图、场景、环境等配置", "🌍");
        worldCategory.Editors.Add(new EditorItemViewModel("场景配置", "游戏场景基础配置", "scenes.xml", "SceneEditor", "🏞️"));
        worldCategory.Editors.Add(new EditorItemViewModel("地图图标", "世界地图图标配置", "map_icons.xml", "MapIconEditor", "🗺️"));
        worldCategory.Editors.Add(new EditorItemViewModel("环境对象", "场景环境物体配置", "objects.xml", "ObjectEditor", "🏛️"));

        // 音频系统
        var audioCategory = new EditorCategoryViewModel("音频系统", "声音、音效、音乐等配置", "🎵");
        audioCategory.Editors.Add(new EditorItemViewModel("模组声音", "模组音效配置", "module_sounds.xml", "ModuleSoundEditor", "🔊"));
        audioCategory.Editors.Add(new EditorItemViewModel("声音文件", "音频文件路径配置", "soundfiles.xml", "SoundFileEditor", "🎧"));
        audioCategory.Editors.Add(new EditorItemViewModel("角色声音", "角色语音配置", "voices.xml", "VoiceEditor", "🗣️"));

        // 多人游戏
        var multiplayerCategory = new EditorCategoryViewModel("多人游戏", "多人模式专用配置", "👥");
        multiplayerCategory.Editors.Add(new EditorItemViewModel("多人角色", "多人游戏角色配置", "mpcharacters.xml", "MPCharacterEditor", "🤺"));
        multiplayerCategory.Editors.Add(new EditorItemViewModel("多人文化", "多人游戏文化配置", "mpcultures.xml", "MPCultureEditor", "🏛️"));
        multiplayerCategory.Editors.Add(new EditorItemViewModel("多人徽章", "玩家徽章配置", "mpbadges.xml", "MPBadgeEditor", "🎖️"));
        multiplayerCategory.Editors.Add(new EditorItemViewModel("多人场景", "多人游戏场景", "MultiplayerScenes.xml", "MPSceneEditor", "🏟️"));

        // 引擎系统
        var engineCategory = new EditorCategoryViewModel("引擎系统", "底层引擎和渲染配置", "⚙️");
        engineCategory.Editors.Add(new EditorItemViewModel("物理材质", "物理系统材质配置", "physics_materials.xml", "PhysicsMaterialEditor", "🧲"));
        engineCategory.Editors.Add(new EditorItemViewModel("布料材质", "布料物理材质配置", "cloth_materials.xml", "ClothMaterialEditor", "🧵"));
        engineCategory.Editors.Add(new EditorItemViewModel("粒子系统", "GPU粒子效果配置", "gpu_particle_systems.xml", "ParticleSystemEditor", "✨"));
        engineCategory.Editors.Add(new EditorItemViewModel("后处理图", "后处理效果配置", "before_transparents_graph.xml", "PostfxGraphEditor", "🎨"));

        Categories.Add(characterCategory);
        Categories.Add(equipmentCategory);
        Categories.Add(combatCategory);
        Categories.Add(worldCategory);
        Categories.Add(audioCategory);
        Categories.Add(multiplayerCategory);
        Categories.Add(engineCategory);
    }

    [RelayCommand]
    private void SelectEditor(EditorItemViewModel editor)
    {
        if (editor == null) return;

        SelectedEditor = editor;
        CurrentBreadcrumb = $"{GetCategoryName(editor)} > {editor.Name}";
        
        // 使用编辑器工厂创建对应的编辑器ViewModel
        CurrentEditorViewModel = _editorFactory.CreateEditorViewModel(editor.EditorType, editor.XmlFileName);
        
        // 如果编辑器支持自动加载，则加载XML文件
        if (CurrentEditorViewModel != null)
        {
            AutoLoadXmlFile(CurrentEditorViewModel, editor.XmlFileName);
        }
    }

    private string GetCategoryName(EditorItemViewModel editor)
    {
        foreach (var category in Categories)
        {
            if (category.Editors.Contains(editor))
                return category.Name;
        }
        return "未知分类";
    }

    /// <summary>
    /// 自动加载XML文件（如果编辑器支持）
    /// </summary>
    private void AutoLoadXmlFile(ViewModelBase editorViewModel, string xmlFileName)
    {
        try
        {
            // 检查编辑器是否有LoadXmlFile方法
            var loadMethod = editorViewModel.GetType().GetMethod("LoadXmlFile");
            if (loadMethod != null)
            {
                loadMethod.Invoke(editorViewModel, new object[] { xmlFileName });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to auto-load XML file: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SearchEditors()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // 显示所有编辑器
            foreach (var category in Categories)
            {
                category.IsExpanded = true;
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = true;
                }
            }
        }
        else
        {
            // 根据搜索文本筛选
            var searchLower = SearchText.ToLower();
            foreach (var category in Categories)
            {
                bool hasVisibleEditors = false;
                foreach (var editor in category.Editors)
                {
                    bool matches = editor.Name.ToLower().Contains(searchLower) ||
                                  editor.Description.ToLower().Contains(searchLower) ||
                                  editor.XmlFileName.ToLower().Contains(searchLower);
                    editor.IsAvailable = matches;
                    if (matches) hasVisibleEditors = true;
                }
                category.IsExpanded = hasVisibleEditors;
            }
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        SearchEditors();
    }
} 