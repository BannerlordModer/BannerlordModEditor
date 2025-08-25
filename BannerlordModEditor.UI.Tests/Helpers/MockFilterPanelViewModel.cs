using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的FilterPanelViewModel用于测试
/// </summary>
public partial class MockFilterPanelViewModel : ObservableObject
{
    private string _searchText = string.Empty;
    private bool _caseSensitive;
    private bool _wholeWord;
    private string _selectedCategory = string.Empty;
    private string _selectedSubcategory = string.Empty;
    private bool _showValidOnly;
    private bool _showInvalidOnly;
    private bool _showModifiedOnly;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private string _minValue = string.Empty;
    private string _maxValue = string.Empty;
    private string _customFilter = string.Empty;
    private int _totalCount = 100;
    private int _filteredCount = 100;
    private int _validCount = 85;
    private int _invalidCount = 15;

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool CaseSensitive
    {
        get => _caseSensitive;
        set => SetProperty(ref _caseSensitive, value);
    }

    public bool WholeWord
    {
        get => _wholeWord;
        set => SetProperty(ref _wholeWord, value);
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public string SelectedSubcategory
    {
        get => _selectedSubcategory;
        set => SetProperty(ref _selectedSubcategory, value);
    }

    public bool ShowValidOnly
    {
        get => _showValidOnly;
        set => SetProperty(ref _showValidOnly, value);
    }

    public bool ShowInvalidOnly
    {
        get => _showInvalidOnly;
        set => SetProperty(ref _showInvalidOnly, value);
    }

    public bool ShowModifiedOnly
    {
        get => _showModifiedOnly;
        set => SetProperty(ref _showModifiedOnly, value);
    }

    public DateTime? StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    public string MinValue
    {
        get => _minValue;
        set => SetProperty(ref _minValue, value);
    }

    public string MaxValue
    {
        get => _maxValue;
        set => SetProperty(ref _maxValue, value);
    }

    public string CustomFilter
    {
        get => _customFilter;
        set => SetProperty(ref _customFilter, value);
    }

    public int TotalCount
    {
        get => _totalCount;
        set => SetProperty(ref _totalCount, value);
    }

    public int FilteredCount
    {
        get => _filteredCount;
        set => SetProperty(ref _filteredCount, value);
    }

    public int ValidCount
    {
        get => _validCount;
        set => SetProperty(ref _validCount, value);
    }

    public int InvalidCount
    {
        get => _invalidCount;
        set => SetProperty(ref _invalidCount, value);
    }

    public ObservableCollection<string> CategoryOptions { get; } = new();
    public ObservableCollection<string> SubcategoryOptions { get; } = new();

    public MockFilterPanelViewModel()
    {
        InitializeOptions();
    }

    private void InitializeOptions()
    {
        // 初始化分类选项
        foreach (var option in new[] { "角色", "物品", "技能", "任务", "全部" })
        {
            CategoryOptions.Add(option);
        }
        
        // 初始化子分类选项
        foreach (var option in new[] { "基础", "高级", "特殊", "全部" })
        {
            SubcategoryOptions.Add(option);
        }
    }

    [RelayCommand]
    public void ApplyCustomFilter()
    {
        // 模拟应用自定义过滤器
        if (!string.IsNullOrWhiteSpace(CustomFilter))
        {
            FilteredCount = 50; // 模拟过滤结果
        }
        else
        {
            FilteredCount = TotalCount;
        }
    }

    [RelayCommand]
    public void ClearAllFilters()
    {
        SearchText = string.Empty;
        CaseSensitive = false;
        WholeWord = false;
        SelectedCategory = string.Empty;
        SelectedSubcategory = string.Empty;
        ShowValidOnly = false;
        ShowInvalidOnly = false;
        ShowModifiedOnly = false;
        StartDate = null;
        EndDate = null;
        MinValue = string.Empty;
        MaxValue = string.Empty;
        CustomFilter = string.Empty;
        FilteredCount = TotalCount;
    }

    [RelayCommand]
    public void SaveFilterConfig()
    {
        // 模拟保存过滤器配置
        // 在实际应用中，这里会将当前过滤器配置保存到文件或数据库
    }

    [RelayCommand]
    public void LoadFilterConfig()
    {
        // 模拟加载过滤器配置
        // 在实际应用中，这里会从文件或数据库加载过滤器配置
    }

    [RelayCommand]
    public void ResetStatistics()
    {
        TotalCount = 100;
        FilteredCount = 100;
        ValidCount = 85;
        InvalidCount = 15;
    }

    public void UpdateStatistics(int total, int filtered, int valid, int invalid)
    {
        TotalCount = total;
        FilteredCount = filtered;
        ValidCount = valid;
        InvalidCount = invalid;
    }

    public bool HasActiveFilters()
    {
        return !string.IsNullOrWhiteSpace(SearchText) ||
               CaseSensitive ||
               WholeWord ||
               !string.IsNullOrWhiteSpace(SelectedCategory) ||
               !string.IsNullOrWhiteSpace(SelectedSubcategory) ||
               ShowValidOnly ||
               ShowInvalidOnly ||
               ShowModifiedOnly ||
               StartDate.HasValue ||
               EndDate.HasValue ||
               !string.IsNullOrWhiteSpace(MinValue) ||
               !string.IsNullOrWhiteSpace(MaxValue) ||
               !string.IsNullOrWhiteSpace(CustomFilter);
    }
}