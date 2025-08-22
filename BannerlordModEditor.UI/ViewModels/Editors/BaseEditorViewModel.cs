using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 通用编辑器视图模型基类
/// </summary>
/// <typeparam name="TData">数据模型类型</typeparam>
/// <typeparam name="TItem">数据项类型</typeparam>
public abstract partial class BaseEditorViewModel<TData, TItem> : ViewModelBase
    where TData : class, new()
    where TItem : class, new()
{
    [ObservableProperty]
    private ObservableCollection<TItem> items = new();

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string searchFilter = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TItem> filteredItems = new();

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    private int filteredCount;

    [ObservableProperty]
    private string statusMessage = "就绪";

    protected readonly string XmlFileName;
    protected readonly string EditorName;

    protected BaseEditorViewModel(string xmlFileName, string editorName)
    {
        XmlFileName = xmlFileName;
        EditorName = editorName;
        
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
    /// 加载XML文件
    /// </summary>
    [RelayCommand]
    private async Task LoadFileAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "正在加载文件...";

            var foundPath = await FindFileAsync(XmlFileName);
            if (foundPath != null)
            {
                await LoadDataAsync(foundPath);
                StatusMessage = $"已加载 {Path.GetFileName(foundPath)}";
            }
            else
            {
                await CreateEmptyEditorAsync();
                StatusMessage = $"未找到 {XmlFileName}，创建新文件";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"加载失败: {ex.Message}";
            await CreateErrorEditorAsync(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 保存XML文件
    /// </summary>
    [RelayCommand]
    private async Task SaveFileAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "正在保存文件...";

            if (string.IsNullOrEmpty(FilePath))
            {
                // TODO: 显示文件保存对话框
                FilePath = XmlFileName;
            }

            await SaveDataAsync(FilePath);
            HasUnsavedChanges = false;
            StatusMessage = "保存成功";
        }
        catch (Exception ex)
        {
            StatusMessage = $"保存失败: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 添加新项
    /// </summary>
    [RelayCommand]
    private void AddItem()
    {
        var newItem = CreateNewItem();
        Items.Add(newItem);
        HasUnsavedChanges = true;
        StatusMessage = "已添加新项";
    }

    /// <summary>
    /// 删除选中项
    /// </summary>
    [RelayCommand]
    private void RemoveItem(TItem item)
    {
        if (item != null)
        {
            Items.Remove(item);
            HasUnsavedChanges = true;
            StatusMessage = "已删除项";
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
    /// 查找文件
    /// </summary>
    private async Task<string?> FindFileAsync(string fileName)
    {
        var possiblePaths = new[]
        {
            Path.Combine("TestData", fileName),
            Path.Combine("BannerlordModEditor.Common.Tests", "TestData", fileName),
            Path.Combine("example", "ModuleData", fileName),
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
    /// 加载数据
    /// </summary>
    private async Task LoadDataAsync(string path)
    {
        // 在派生类中实现具体的数据加载逻辑
        await Task.Run(() =>
        {
            var data = LoadDataFromFile(path);
            Items.Clear();
            
            var items = GetItemsFromData(data);
            foreach (var item in items)
            {
                Items.Add(item);
            }
            
            FilePath = path;
            HasUnsavedChanges = false;
        });
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private async Task SaveDataAsync(string path)
    {
        await Task.Run(() =>
        {
            var data = CreateDataFromItems(Items);
            SaveDataToFile(data, path);
        });
    }

    /// <summary>
    /// 创建空编辑器
    /// </summary>
    private async Task CreateEmptyEditorAsync()
    {
        await Task.Run(() =>
        {
            Items.Clear();
            var defaultItem = CreateNewItem();
            Items.Add(defaultItem);
            FilePath = XmlFileName;
            HasUnsavedChanges = false;
        });
    }

    /// <summary>
    /// 创建错误编辑器
    /// </summary>
    private async Task CreateErrorEditorAsync(string errorMessage)
    {
        await Task.Run(() =>
        {
            Items.Clear();
            var errorItem = CreateErrorItem(errorMessage);
            Items.Add(errorItem);
            FilePath = XmlFileName;
            HasUnsavedChanges = false;
        });
    }

    // 抽象方法 - 需要在派生类中实现

    /// <summary>
    /// 从文件加载数据
    /// </summary>
    protected abstract TData LoadDataFromFile(string path);

    /// <summary>
    /// 保存数据到文件
    /// </summary>
    protected abstract void SaveDataToFile(TData data, string path);

    /// <summary>
    /// 从数据中获取项目集合
    /// </summary>
    protected abstract IEnumerable<TItem> GetItemsFromData(TData data);

    /// <summary>
    /// 从项目集合创建数据
    /// </summary>
    protected abstract TData CreateDataFromItems(ObservableCollection<TItem> items);

    /// <summary>
    /// 创建新项目
    /// </summary>
    protected abstract TItem CreateNewItem();

    /// <summary>
    /// 创建错误项目
    /// </summary>
    protected abstract TItem CreateErrorItem(string errorMessage);

    /// <summary>
    /// 检查项目是否匹配搜索过滤条件
    /// </summary>
    protected abstract bool MatchesSearchFilter(TItem item, string filter);
}