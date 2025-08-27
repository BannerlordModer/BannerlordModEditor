using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 物品编辑器视图模型
/// </summary>
[EditorType(
    EditorType = "ItemEditor",
    DisplayName = "物品编辑器",
    Description = "编辑游戏物品",
    XmlFileName = "spitems.xml",
    Category = "Items")]
public partial class ItemEditorViewModel : SimpleEditorViewModel<MpItems, Item, ItemViewModel>
{
    [ObservableProperty]
    private ObservableCollection<ItemViewModel> items = new();

    [ObservableProperty]
    private ItemViewModel? selectedItem;

    [ObservableProperty]
    private ObservableCollection<string> itemTypes = new();

    [ObservableProperty]
    private ObservableCollection<string> itemCategories = new();

    [ObservableProperty]
    private ObservableCollection<string> cultures = new();

    [ObservableProperty]
    private string selectedTypeFilter = "All";

    [ObservableProperty]
    private string selectedCategoryFilter = "All";

    [ObservableProperty]
    private string selectedCultureFilter = "All";

    private readonly IValidationService _validationService;

    public ItemEditorViewModel(IValidationService? validationService = null) 
        : base("spitems.xml", "物品编辑器")
    {
        _validationService = validationService ?? new ValidationService();
        
        // 初始化过滤器选项
        InitializeFilterOptions();
        
        // 初始化示例数据
        InitializeSampleData();
        
        Items = this.items;
    }

    private void InitializeFilterOptions()
    {
        // 物品类型
        foreach (var type in new[] { "All", "Goods", "Armor", "Weapon", "Horse", "Banner" })
        {
            ItemTypes.Add(type);
        }
        
        // 物品类别
        foreach (var category in new[] { "All", "OneHandedWeapon", "TwoHandedWeapon", "Polearm", "Bow", "Crossbow", "Thrown", "Shield", "HeadArmor", "BodyArmor", "LegArmor", "HandArmor", "Mount", "Goods" })
        {
            ItemCategories.Add(category);
        }
        
        // 文化
        foreach (var culture in new[] { "All", "empire", "vlandia", "sturgia", "aserai", "khuzait", "battania" })
        {
            Cultures.Add(culture);
        }
    }

    private void InitializeSampleData()
    {
        Items.Add(new ItemViewModel
        {
            Id = "sample_sword",
            Name = "Sample Sword",
            Type = "Weapon",
            Subtype = "OneHandedSword",
            ItemCategory = "OneHandedWeapon",
            Culture = "empire",
            Weight = "1.5",
            Value = "100",
            Difficulty = "10",
            IsMerchandise = true,
            MultiplayerItem = true
        });

        Items.Add(new ItemViewModel
        {
            Id = "sample_armor",
            Name = "Sample Armor",
            Type = "Armor",
            Subtype = "BodyArmor",
            ItemCategory = "BodyArmor",
            Culture = "vlandia",
            Weight = "5.0",
            Value = "500",
            Difficulty = "20",
            IsMerchandise = true,
            MultiplayerItem = false
        });
    }

    protected override bool ItemMatchesFilter(ItemViewModel item, string filterText)
    {
        var matchesText = string.IsNullOrWhiteSpace(filterText) ||
                          item.Id?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true ||
                          item.Name?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true;

        var matchesType = SelectedTypeFilter == "All" || item.Type == SelectedTypeFilter;
        var matchesCategory = SelectedCategoryFilter == "All" || item.ItemCategory == SelectedCategoryFilter;
        var matchesCulture = SelectedCultureFilter == "All" || item.Culture == SelectedCultureFilter;

        return matchesText && matchesType && matchesCategory && matchesCulture;
    }

    protected override ItemViewModel CreateNewItemViewModel()
    {
        return new ItemViewModel
        {
            Id = $"item_{Items.Count + 1}",
            Name = "New Item",
            Type = "Goods",
            Culture = "empire",
            IsMerchandise = true
        };
    }

    protected override ItemViewModel DuplicateItemViewModel(ItemViewModel source)
    {
        return new ItemViewModel
        {
            Id = $"{source.Id}_Copy",
            Name = $"{source.Name} (Copy)",
            Type = source.Type,
            Subtype = source.Subtype,
            ItemCategory = source.ItemCategory,
            Culture = source.Culture,
            Mesh = source.Mesh,
            HolsterMesh = source.HolsterMesh,
            BodyName = source.BodyName,
            ShieldBodyName = source.ShieldBodyName,
            HolsterBodyName = source.HolsterBodyName,
            Weight = source.Weight,
            Value = source.Value,
            Difficulty = source.Difficulty,
            Appearance = source.Appearance,
            IsMerchandise = source.IsMerchandise,
            MultiplayerItem = source.MultiplayerItem,
            UsingTableau = source.UsingTableau,
            RecalculateBody = source.RecalculateBody,
            HasLowerHolsterPriority = source.HasLowerHolsterPriority,
            HolsterPositionShift = source.HolsterPositionShift,
            FlyingMesh = source.FlyingMesh,
            HolsterMeshWithWeapon = source.HolsterMeshWithWeapon,
            AmmoOffset = source.AmmoOffset,
            Prefab = source.Prefab,
            LodAtlasIndex = source.LodAtlasIndex,
            ItemHolsters = source.ItemHolsters
        };
    }

    protected override Item ConvertToItemModel(ItemViewModel viewModel)
    {
        return new Item
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Type = viewModel.Type,
            Subtype = viewModel.Subtype,
            ItemCategory = viewModel.ItemCategory,
            Culture = viewModel.Culture,
            Mesh = viewModel.Mesh,
            HolsterMesh = viewModel.HolsterMesh,
            BodyName = viewModel.BodyName,
            ShieldBodyName = viewModel.ShieldBodyName,
            HolsterBodyName = viewModel.HolsterBodyName,
            Weight = viewModel.Weight,
            Value = viewModel.Value,
            Difficulty = viewModel.Difficulty,
            Appearance = viewModel.Appearance,
            IsMerchandise = viewModel.IsMerchandise,
            MultiplayerItem = viewModel.MultiplayerItem,
            UsingTableau = viewModel.UsingTableau,
            RecalculateBody = viewModel.RecalculateBody,
            HasLowerHolsterPriority = viewModel.HasLowerHolsterPriority,
            HolsterPositionShift = viewModel.HolsterPositionShift,
            FlyingMesh = viewModel.FlyingMesh,
            HolsterMeshWithWeapon = viewModel.HolsterMeshWithWeapon,
            AmmoOffset = viewModel.AmmoOffset,
            Prefab = viewModel.Prefab,
            LodAtlasIndex = viewModel.LodAtlasIndex,
            ItemHolsters = viewModel.ItemHolsters,
            
            // 设置Specified属性
            WeightSpecified = !string.IsNullOrWhiteSpace(viewModel.Weight),
            ValueSpecified = !string.IsNullOrWhiteSpace(viewModel.Value),
            DifficultySpecified = !string.IsNullOrWhiteSpace(viewModel.Difficulty),
            AppearanceSpecified = !string.IsNullOrWhiteSpace(viewModel.Appearance),
            LodAtlasIndexSpecified = !string.IsNullOrWhiteSpace(viewModel.LodAtlasIndex)
        };
    }

    protected override ItemViewModel ConvertToItemViewModel(Item itemModel)
    {
        return new ItemViewModel
        {
            Id = itemModel.Id,
            Name = itemModel.Name,
            Type = itemModel.Type,
            Subtype = itemModel.Subtype,
            ItemCategory = itemModel.ItemCategory,
            Culture = itemModel.Culture,
            Mesh = itemModel.Mesh,
            HolsterMesh = itemModel.HolsterMesh,
            BodyName = itemModel.BodyName,
            ShieldBodyName = itemModel.ShieldBodyName,
            HolsterBodyName = itemModel.HolsterBodyName,
            Weight = itemModel.Weight,
            Value = itemModel.Value,
            Difficulty = itemModel.Difficulty,
            Appearance = itemModel.Appearance,
            IsMerchandise = itemModel.IsMerchandise,
            MultiplayerItem = itemModel.MultiplayerItem,
            UsingTableau = itemModel.UsingTableau,
            RecalculateBody = itemModel.RecalculateBody,
            HasLowerHolsterPriority = itemModel.HasLowerHolsterPriority,
            HolsterPositionShift = itemModel.HolsterPositionShift,
            FlyingMesh = itemModel.FlyingMesh,
            HolsterMeshWithWeapon = itemModel.HolsterMeshWithWeapon,
            AmmoOffset = itemModel.AmmoOffset,
            Prefab = itemModel.Prefab,
            LodAtlasIndex = itemModel.LodAtlasIndex,
            ItemHolsters = itemModel.ItemHolsters
        };
    }

    protected override MpItems ConvertToRootModel(ObservableCollection<ItemViewModel> items)
    {
        var root = new MpItems();
        foreach (var item in items)
        {
            root.Items.Add(ConvertToItemModel(item));
        }
        return root;
    }

    protected override ObservableCollection<ItemViewModel> ConvertFromRootModel(MpItems rootModel)
    {
        var result = new ObservableCollection<ItemViewModel>();
        foreach (var item in rootModel.Items)
        {
            result.Add(ConvertToItemViewModel(item));
        }
        return result;
    }

    [RelayCommand]
    private void AddNewItem()
    {
        AddItem();
    }

    [RelayCommand]
    private void RemoveSelectedItem(ItemViewModel? item)
    {
        if (item != null)
        {
            RemoveItem(item);
        }
    }

    [RelayCommand]
    private void DuplicateSelectedItem(ItemViewModel? item)
    {
        if (item != null)
        {
            DuplicateItem(item);
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SelectedTypeFilter = "All";
        SelectedCategoryFilter = "All";
        SelectedCultureFilter = "All";
        FilterText = string.Empty;
    }

    [RelayCommand]
    private void ValidateAll()
    {
        var isValid = true;
        foreach (var item in Items)
        {
            if (!item.IsValid)
            {
                isValid = false;
                break;
            }
        }
        
        StatusMessage = isValid ? "所有物品验证通过" : "存在验证错误";
    }

    [RelayCommand]
    private void ExportToCsv()
    {
        try
        {
            var csvContent = "ID,Name,Type,Category,Culture,Weight,Value,Difficulty\n";
            foreach (var item in Items)
            {
                csvContent += $"{item.Id},{item.Name},{item.Type},{item.ItemCategory},{item.Culture},{item.Weight},{item.Value},{item.Difficulty}\n";
            }
            
            // 这里应该保存到文件，暂时输出到控制台
            System.Diagnostics.Debug.WriteLine("CSV Export:\n" + csvContent);
            StatusMessage = "CSV导出功能已触发（查看控制台）";
        }
        catch (Exception ex)
        {
            StatusMessage = $"CSV导出失败: {ex.Message}";
        }
    }

    // 监听过滤器变化
    partial void OnSelectedTypeFilterChanged(string value)
    {
        UpdateFilteredItems();
    }

    partial void OnSelectedCategoryFilterChanged(string value)
    {
        UpdateFilteredItems();
    }

    partial void OnSelectedCultureFilterChanged(string value)
    {
        UpdateFilteredItems();
    }

    /// <summary>
    /// 获取验证服务（用于测试）
    /// </summary>
    public IValidationService ValidationService => _validationService;
}

/// <summary>
/// 物品视图模型
/// </summary>
public partial class ItemViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "ID不能为空")]
    [StringLength(100, ErrorMessage = "ID长度不能超过100个字符")]
    private string id = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "名称不能为空")]
    [StringLength(200, ErrorMessage = "名称长度不能超过200个字符")]
    private string name = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "类型不能为空")]
    private string type = "Goods";

    [ObservableProperty]
    private string? subtype;

    [ObservableProperty]
    private string? itemCategory;

    [ObservableProperty]
    private string? culture;

    [ObservableProperty]
    private string? mesh;

    [ObservableProperty]
    private string? holsterMesh;

    [ObservableProperty]
    private string? bodyName;

    [ObservableProperty]
    private string? shieldBodyName;

    [ObservableProperty]
    private string? holsterBodyName;

    [ObservableProperty]
    [Range(0, double.MaxValue, ErrorMessage = "重量必须大于等于0")]
    private string? weight;

    [ObservableProperty]
    [Range(0, int.MaxValue, ErrorMessage = "价值必须大于等于0")]
    private string? value;

    [ObservableProperty]
    [Range(0, int.MaxValue, ErrorMessage = "难度必须大于等于0")]
    private string? difficulty;

    [ObservableProperty]
    [Range(0, 100, ErrorMessage = "外观必须在0-100之间")]
    private string? appearance;

    [ObservableProperty]
    private bool isMerchandise = true;

    [ObservableProperty]
    private bool multiplayerItem;

    [ObservableProperty]
    private bool usingTableau;

    [ObservableProperty]
    private bool recalculateBody;

    [ObservableProperty]
    private bool hasLowerHolsterPriority;

    [ObservableProperty]
    private string? holsterPositionShift;

    [ObservableProperty]
    private string? flyingMesh;

    [ObservableProperty]
    private string? holsterMeshWithWeapon;

    [ObservableProperty]
    private string? ammoOffset;

    [ObservableProperty]
    private string? prefab;

    [ObservableProperty]
    private string? lodAtlasIndex;

    [ObservableProperty]
    private string? itemHolsters;

    private readonly List<string> _validationErrors = new();

    public bool IsValid => !string.IsNullOrWhiteSpace(Id) && 
                           !string.IsNullOrWhiteSpace(Name) && 
                           !string.IsNullOrWhiteSpace(Type) &&
                           _validationErrors.Count == 0;

    public IReadOnlyList<string> ValidationErrors => _validationErrors.AsReadOnly();

    // 便捷属性
    public double? WeightDouble => double.TryParse(Weight, out double weight) ? weight : null;
    public int? ValueInt => int.TryParse(Value, out int val) ? val : null;
    public int? DifficultyInt => int.TryParse(Difficulty, out int diff) ? diff : null;
    public double? AppearanceDouble => double.TryParse(Appearance, out double app) ? app : null;

    partial void OnIdChanged(string value)
    {
        ValidateProperty(nameof(Id));
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnNameChanged(string value)
    {
        ValidateProperty(nameof(Name));
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnTypeChanged(string value)
    {
        ValidateProperty(nameof(Type));
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnWeightChanged(string? value)
    {
        ValidateProperty(nameof(Weight));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(WeightDouble));
    }

    partial void OnValueChanged(string? value)
    {
        ValidateProperty(nameof(Value));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValueInt));
    }

    partial void OnDifficultyChanged(string? value)
    {
        ValidateProperty(nameof(Difficulty));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(DifficultyInt));
    }

    partial void OnAppearanceChanged(string? value)
    {
        ValidateProperty(nameof(Appearance));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(AppearanceDouble));
    }

    private void ValidateProperty(string propertyName)
    {
        var validationService = new ValidationService();
        var errors = validationService.ValidateProperty(this, propertyName);
        
        _validationErrors.RemoveAll(e => e.StartsWith($"{propertyName}:"));
        _validationErrors.AddRange(errors.Select(e => $"{propertyName}: {e}"));
        
        OnPropertyChanged(nameof(ValidationErrors));
    }
}