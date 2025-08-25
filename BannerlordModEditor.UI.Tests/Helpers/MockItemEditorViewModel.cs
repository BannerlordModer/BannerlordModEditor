using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的物品数据模型
/// </summary>
public class MockItemData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ItemCategory { get; set; } = string.Empty;
    public string Culture { get; set; } = string.Empty;
}

/// <summary>
/// 模拟的ItemEditorViewModel用于测试
/// </summary>
public partial class MockItemEditorViewModel : ObservableObject
{
    private string _filterText = string.Empty;
    private MockItemData? _selectedItem;
    private string _selectedTypeFilter = string.Empty;
    private string _selectedCategoryFilter = string.Empty;
    private string _selectedCultureFilter = string.Empty;

    public ObservableCollection<MockItemData> Items { get; } = new();
    public ObservableCollection<string> ItemTypes { get; } = new();
    public ObservableCollection<string> ItemCategories { get; } = new();
    public ObservableCollection<string> Cultures { get; } = new();

    public ObservableCollection<MockItemData> FilteredItems { get; } = new();

    public string FilterText
    {
        get => _filterText;
        set
        {
            SetProperty(ref _filterText, value);
            ApplyFilters();
        }
    }

    public MockItemData? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
        }
    }

    public string SelectedTypeFilter
    {
        get => _selectedTypeFilter;
        set
        {
            SetProperty(ref _selectedTypeFilter, value);
            ApplyFilters();
        }
    }

    public string SelectedCategoryFilter
    {
        get => _selectedCategoryFilter;
        set
        {
            SetProperty(ref _selectedCategoryFilter, value);
            ApplyFilters();
        }
    }

    public string SelectedCultureFilter
    {
        get => _selectedCultureFilter;
        set
        {
            SetProperty(ref _selectedCultureFilter, value);
            ApplyFilters();
        }
    }

    public MockItemEditorViewModel()
    {
        InitializeSampleData();
        InitializeFilters();
    }

    private void InitializeSampleData()
    {
        // 添加示例物品数据
        Items.Add(new MockItemData
        {
            Id = "sword_1",
            Name = "长剑",
            Type = "武器",
            ItemCategory = "近战武器",
            Culture = "帝国"
        });

        Items.Add(new MockItemData
        {
            Id = "bow_1",
            Name = "长弓",
            Type = "武器",
            ItemCategory = "远程武器",
            Culture = "瓦兰迪亚"
        });

        Items.Add(new MockItemData
        {
            Id = "armor_1",
            Name = "板甲",
            Type = "护甲",
            ItemCategory = "重型护甲",
            Culture = "帝国"
        });

        Items.Add(new MockItemData
        {
            Id = "horse_1",
            Name = "战马",
            Type = "坐骑",
            ItemCategory = "马匹",
            Culture = "库赛特"
        });

        // 初始化过滤后的物品列表
        foreach (var item in Items)
        {
            FilteredItems.Add(item);
        }
    }

    private void InitializeFilters()
    {
        // 初始化类型过滤器
        var types = Items.Select(i => i.Type).Distinct().ToList();
        foreach (var type in types)
        {
            ItemTypes.Add(type);
        }

        // 初始化类别过滤器
        var categories = Items.Select(i => i.ItemCategory).Distinct().ToList();
        foreach (var category in categories)
        {
            ItemCategories.Add(category);
        }

        // 初始化文化过滤器
        var cultures = Items.Select(i => i.Culture).Distinct().ToList();
        foreach (var culture in cultures)
        {
            Cultures.Add(culture);
        }
    }

    [RelayCommand]
    public void AddNewItem()
    {
        var newItem = new MockItemData
        {
            Id = $"new_item_{Items.Count + 1}",
            Name = "新物品",
            Type = "未分类",
            ItemCategory = "未分类",
            Culture = "通用"
        };

        Items.Add(newItem);
        ApplyFilters();
    }

    [RelayCommand]
    public void ClearFilters()
    {
        FilterText = string.Empty;
        SelectedTypeFilter = string.Empty;
        SelectedCategoryFilter = string.Empty;
        SelectedCultureFilter = string.Empty;
    }

    [RelayCommand]
    public async Task SaveXmlFile()
    {
        await Task.Delay(100); // 模拟保存操作
    }

    [RelayCommand]
    public async Task LoadFile()
    {
        await Task.Delay(100); // 模拟加载操作
    }

    [RelayCommand]
    public void ValidateAll()
    {
        // 模拟验证所有物品
    }

    private void ApplyFilters()
    {
        FilteredItems.Clear();

        var filtered = Items.AsEnumerable();

        // 应用文本过滤
        if (!string.IsNullOrWhiteSpace(FilterText))
        {
            var filterLower = FilterText.ToLower();
            filtered = filtered.Where(item => 
                item.Id.ToLower().Contains(filterLower) ||
                item.Name.ToLower().Contains(filterLower) ||
                item.Type.ToLower().Contains(filterLower) ||
                item.ItemCategory.ToLower().Contains(filterLower) ||
                item.Culture.ToLower().Contains(filterLower));
        }

        // 应用类型过滤
        if (!string.IsNullOrWhiteSpace(SelectedTypeFilter))
        {
            filtered = filtered.Where(item => item.Type == SelectedTypeFilter);
        }

        // 应用类别过滤
        if (!string.IsNullOrWhiteSpace(SelectedCategoryFilter))
        {
            filtered = filtered.Where(item => item.ItemCategory == SelectedCategoryFilter);
        }

        // 应用文化过滤
        if (!string.IsNullOrWhiteSpace(SelectedCultureFilter))
        {
            filtered = filtered.Where(item => item.Culture == SelectedCultureFilter);
        }

        foreach (var item in filtered)
        {
            FilteredItems.Add(item);
        }
    }
}