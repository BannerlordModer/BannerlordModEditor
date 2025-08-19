using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Xml;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 通用数据网格编辑器视图模型
/// </summary>
public partial class DataGridEditorViewModel<TData, TItem> : BaseEditorViewModel
    where TData : class, new()
    where TItem : class, new()
{
    [ObservableProperty]
    private ObservableCollection<TItem> items = new();

    [ObservableProperty]
    private ObservableCollection<GridColumnInfo> columns = new();

    [ObservableProperty]
    private TItem? selectedItem;

    [ObservableProperty]
    private string searchFilter = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TItem> filteredItems = new();

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    private int filteredCount;

    private readonly Func<TData, IEnumerable<TItem>> _getItemsFunc;
    private readonly Func<IEnumerable<TItem>, TData> _createDataFunc;
    private readonly Action<TItem> _onItemSelected;
    private readonly Action<TItem> _onItemAdded;
    private readonly Action<TItem> _onItemRemoved;

    public DataGridEditorViewModel(
        string xmlFileName,
        string editorName,
        Func<TData, IEnumerable<TItem>> getItemsFunc,
        Func<IEnumerable<TItem>, TData> createDataFunc,
        Action<TItem>? onItemSelected = null,
        Action<TItem>? onItemAdded = null,
        Action<TItem>? onItemRemoved = null)
        : base(xmlFileName, editorName)
    {
        _getItemsFunc = getItemsFunc;
        _createDataFunc = createDataFunc;
        _onItemSelected = onItemSelected ?? (_ => { });
        _onItemAdded = onItemAdded ?? (_ => { });
        _onItemRemoved = onItemRemoved ?? (_ => { });

        // 初始化列信息
        InitializeColumns();
        
        // 初始化过滤后的集合
        FilteredItems = new ObservableCollection<TItem>(Items);
        
        // 监听原始集合变化
        Items.CollectionChanged += (s, e) =>
        {
            UpdateFilteredItems();
            UpdateCounts();
            HasUnsavedChanges = true;
        };
    }

    /// <summary>
    /// 初始化列信息
    /// </summary>
    private void InitializeColumns()
    {
        var itemType = typeof(TItem);
        var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var prop in properties)
        {
            // 跳过复杂的属性类型
            if (IsComplexType(prop.PropertyType))
                continue;

            var column = new GridColumnInfo
            {
                PropertyName = prop.Name,
                Header = GetDisplayName(prop.Name),
                PropertyType = prop.PropertyType,
                IsReadOnly = IsReadOnlyProperty(prop)
            };

            Columns.Add(column);
        }
    }

    /// <summary>
    /// 检查是否为复杂类型
    /// </summary>
    private bool IsComplexType(Type type)
    {
        return type.IsClass && type != typeof(string) || 
               type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) ||
               type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>);
    }

    /// <summary>
    /// 获取显示名称
    /// </summary>
    private string GetDisplayName(string propertyName)
    {
        // 简单的属性名转换逻辑
        return propertyName switch
        {
            "Id" => "ID",
            "Name" => "名称",
            "Description" => "描述",
            "Documentation" => "文档",
            "Type" => "类型",
            "Source" => "来源",
            "Value" => "值",
            _ => propertyName
        };
    }

    /// <summary>
    /// 检查是否为只读属性
    /// </summary>
    private bool IsReadOnlyProperty(PropertyInfo prop)
    {
        return !prop.CanWrite || prop.GetSetMethod() == null;
    }

    /// <summary>
    /// 加载XML文件
    /// </summary>
    public override void LoadXmlFile(string fileName)
    {
        try
        {
            var foundPath = FindFile(fileName);
            if (foundPath != null)
            {
                var data = LoadDataFromFile(foundPath);
                if (data != null)
                {
                    Items.Clear();
                    var items = _getItemsFunc(data);
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                    
                    FilePath = foundPath;
                    HasUnsavedChanges = false;
                    StatusMessage = $"已加载 {Path.GetFileName(foundPath)}";
                }
            }
            else
            {
                CreateEmptyEditor();
                StatusMessage = $"未找到 {fileName}，创建新文件";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"加载失败: {ex.Message}";
            CreateErrorEditor(ex.Message);
        }
    }

    /// <summary>
    /// 保存XML文件
    /// </summary>
    public override void SaveXmlFile()
    {
        try
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                StatusMessage = "请先加载文件";
                return;
            }

            var data = _createDataFunc(Items);
            SaveDataToFile(data, FilePath);
            HasUnsavedChanges = false;
            StatusMessage = "保存成功";
        }
        catch (Exception ex)
        {
            StatusMessage = $"保存失败: {ex.Message}";
        }
    }

    /// <summary>
    /// 添加新项
    /// </summary>
    [RelayCommand]
    private void AddItem()
    {
        var newItem = new TItem();
        Items.Add(newItem);
        SelectedItem = newItem;
        _onItemAdded(newItem);
        StatusMessage = "已添加新项";
    }

    /// <summary>
    /// 删除选中项
    /// </summary>
    [RelayCommand]
    private void RemoveItem()
    {
        if (SelectedItem != null)
        {
            var item = SelectedItem;
            Items.Remove(item);
            SelectedItem = null;
            _onItemRemoved(item);
            StatusMessage = "已删除项";
        }
    }

    /// <summary>
    /// 选中项变化
    /// </summary>
    partial void OnSelectedItemChanged(TItem? value)
    {
        if (value != null)
        {
            _onItemSelected(value);
        }
    }

    /// <summary>
    /// 搜索过滤
    /// </summary>
    partial void OnSearchFilterChanged(string value)
    {
        UpdateFilteredItems();
    }

    /// <summary>
    /// 更新过滤后的项目
    /// </summary>
    private void UpdateFilteredItems()
    {
        if (string.IsNullOrWhiteSpace(SearchFilter))
        {
            FilteredItems.Clear();
            foreach (var item in Items)
            {
                FilteredItems.Add(item);
            }
        }
        else
        {
            var filtered = Items.Where(item => MatchesSearchFilter(item, SearchFilter)).ToList();
            FilteredItems.Clear();
            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }
        }
        
        UpdateCounts();
    }

    /// <summary>
    /// 更新计数
    /// </summary>
    private void UpdateCounts()
    {
        TotalCount = Items.Count;
        FilteredCount = FilteredItems.Count;
    }

    /// <summary>
    /// 检查项目是否匹配搜索过滤条件
    /// </summary>
    private bool MatchesSearchFilter(TItem item, string filter)
    {
        var itemType = typeof(TItem);
        var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var searchLower = filter.ToLower();

        foreach (var prop in properties)
        {
            if (IsComplexType(prop.PropertyType))
                continue;

            var value = prop.GetValue(item);
            if (value != null && value.ToString()!.ToLower().Contains(searchLower))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 查找文件
    /// </summary>
    private string? FindFile(string fileName)
    {
        var possiblePaths = new[]
        {
            Path.Combine("TestData", fileName),
            Path.Combine("BannerlordModEditor.Common.Tests", "TestData", fileName),
            fileName
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        return null;
    }

    /// <summary>
    /// 从文件加载数据
    /// </summary>
    private TData? LoadDataFromFile(string path)
    {
        var loader = new BannerlordModEditor.Common.Loaders.GenericXmlLoader<TData>();
        return loader.Load(path);
    }

    /// <summary>
    /// 保存数据到文件
    /// </summary>
    private void SaveDataToFile(TData data, string path)
    {
        var loader = new BannerlordModEditor.Common.Loaders.GenericXmlLoader<TData>();
        loader.Save(data, path);
    }

    /// <summary>
    /// 创建空编辑器
    /// </summary>
    private void CreateEmptyEditor()
    {
        Items.Clear();
        var defaultItem = new TItem();
        Items.Add(defaultItem);
        SelectedItem = defaultItem;
        FilePath = XmlFileName;
        HasUnsavedChanges = false;
    }

    /// <summary>
    /// 创建错误编辑器
    /// </summary>
    private void CreateErrorEditor(string errorMessage)
    {
        Items.Clear();
        var errorItem = new TItem();
        Items.Add(errorItem);
        SelectedItem = errorItem;
        FilePath = XmlFileName;
        HasUnsavedChanges = false;
    }

    /// <summary>
    /// 刷新编辑器
    /// </summary>
    [RelayCommand]
    private void RefreshCommand()
    {
        UpdateFilteredItems();
        UpdateCounts();
        StatusMessage = "已刷新";
    }

    /// <summary>
    /// 重写基类刷新方法
    /// </summary>
    public override void Refresh()
    {
        UpdateFilteredItems();
        UpdateCounts();
        StatusMessage = "已刷新";
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    [RelayCommand]
    private void Clear()
    {
        Items.Clear();
        FilteredItems.Clear();
        SelectedItem = null;
        TotalCount = 0;
        FilteredCount = 0;
        HasUnsavedChanges = true;
        StatusMessage = "已清空数据";
    }
}

/// <summary>
/// 网格列信息
/// </summary>
public partial class GridColumnInfo : ObservableObject
{
    [ObservableProperty]
    private string propertyName = string.Empty;

    [ObservableProperty]
    private string header = string.Empty;

    [ObservableProperty]
    private Type propertyType = typeof(string);

    [ObservableProperty]
    private bool isReadOnly;

    [ObservableProperty]
    private int width = 100;

    [ObservableProperty]
    private bool isVisible = true;
}