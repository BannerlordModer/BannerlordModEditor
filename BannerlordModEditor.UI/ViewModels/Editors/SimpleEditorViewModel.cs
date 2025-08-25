using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 简化的编辑器视图模型基类，直接使用数据模型
/// </summary>
/// <typeparam name="TModel">数据模型类型</typeparam>
/// <typeparam name="TItemModel">单个项的数据模型类型</typeparam>
/// <typeparam name="TItemViewModel">项视图模型类型</typeparam>
public abstract partial class SimpleEditorViewModel<TModel, TItemModel, TItemViewModel> : BaseEditorViewModel<TModel, TItemModel>
    where TModel : class, new()
    where TItemModel : class, new()
    where TItemViewModel : ObservableObject, new()
{
    [ObservableProperty]
    private ObservableCollection<TItemViewModel> items = new();

    [ObservableProperty]
    private TItemViewModel? selectedItem;

    [ObservableProperty]
    private string filterText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TItemViewModel> filteredItems = new();

    protected SimpleEditorViewModel(string xmlFileName, string editorName) 
        : base(xmlFileName, editorName)
    {
        Items.CollectionChanged += (s, e) => UpdateFilteredItems();
    }

    partial void OnFilterTextChanged(string value)
    {
        UpdateFilteredItems();
    }

    protected virtual void UpdateFilteredItems()
    {
        if (string.IsNullOrWhiteSpace(FilterText))
        {
            FilteredItems = new ObservableCollection<TItemViewModel>(Items);
        }
        else
        {
            var filtered = Items.Where(item => ItemMatchesFilter(item, FilterText));
            FilteredItems = new ObservableCollection<TItemViewModel>(filtered);
        }
    }

    protected virtual bool ItemMatchesFilter(TItemViewModel item, string filterText)
    {
        // 基础过滤实现，子类可以重写
        return true;
    }

    
    [RelayCommand]
    protected virtual void DuplicateItem(TItemViewModel? item)
    {
        if (item != null)
        {
            var duplicatedItem = DuplicateItemViewModel(item);
            Items.Add(duplicatedItem);
            HasUnsavedChanges = true;
            StatusMessage = "已复制项";
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        FilterText = string.Empty;
    }

    protected abstract TItemViewModel CreateNewItemViewModel();
    protected abstract TItemViewModel DuplicateItemViewModel(TItemViewModel source);
    protected abstract TItemModel ConvertToItemModel(TItemViewModel viewModel);
    protected abstract TItemViewModel ConvertToItemViewModel(TItemModel itemModel);
    protected abstract TModel ConvertToRootModel(ObservableCollection<TItemViewModel> items);
    protected abstract ObservableCollection<TItemViewModel> ConvertFromRootModel(TModel rootModel);

    public override async Task LoadXmlFileAsync(string fileName)
    {
        try
        {
            IsLoading = true;
            StatusMessage = "正在加载文件...";
            
            await Task.Run(() =>
            {
                var foundPath = FindXmlFile(fileName);
                if (foundPath != null)
                {
                    var loader = new GenericXmlLoader<TModel>();
                    var data = loader.Load(foundPath);
                    
                    if (data != null)
                    {
                        var itemModels = GetItemsFromData(data);
                        Items.Clear();
                        foreach (var itemModel in itemModels)
                        {
                            Items.Add(ConvertToItemViewModel(itemModel));
                        }
                        FilePath = foundPath;
                        HasUnsavedChanges = false;
                        StatusMessage = $"已加载 {Path.GetFileName(foundPath)}";
                    }
                }
                else
                {
                    HandleFileNotFound(fileName);
                }
            });
        }
        catch (Exception ex)
        {
            HandleLoadError(fileName, ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public override async Task SaveXmlFileAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                StatusMessage = "请先加载文件";
                return;
            }

            IsLoading = true;
            StatusMessage = "正在保存文件...";
            
            await Task.Run(() =>
            {
                var itemModels = new ObservableCollection<TItemModel>();
                foreach (var itemViewModel in Items)
                {
                    itemModels.Add(ConvertToItemModel(itemViewModel));
                }
                
                var rootModel = CreateDataFromItems(itemModels);
                var loader = new GenericXmlLoader<TModel>();
                loader.Save(rootModel, FilePath);
                
                HasUnsavedChanges = false;
                StatusMessage = "保存成功";
            });
        }
        catch (Exception ex)
        {
            HandleSaveError(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private string? FindXmlFile(string fileName)
    {
        var possiblePaths = new[]
        {
            Path.Combine("TestData", fileName),
            Path.Combine("BannerlordModEditor.Common.Tests", "TestData", fileName),
            fileName
        };

        return possiblePaths.FirstOrDefault(File.Exists);
    }

    private void HandleFileNotFound(string fileName)
    {
        Items.Clear();
        Items.Add(CreateNewItemViewModel());
        FilePath = fileName;
        HasUnsavedChanges = false;
        StatusMessage = $"未找到 {fileName}，创建新文件";
    }

    private void HandleLoadError(string fileName, Exception ex)
    {
        StatusMessage = $"加载失败: {ex.Message}";
        System.Diagnostics.Debug.WriteLine($"LoadXmlFile failed: {ex.Message}");
        
        Items.Clear();
        var errorItem = CreateNewItemViewModel();
        // 设置错误信息（需要子类实现具体的错误处理）
        Items.Add(errorItem);
    }

    private void HandleSaveError(Exception ex)
    {
        StatusMessage = $"保存失败: {ex.Message}";
        System.Diagnostics.Debug.WriteLine($"Save failed: {ex.Message}");
    }

    /// <summary>
    /// 添加新项
    /// </summary>
    protected void AddItem()
    {
        ExecuteSafely(() =>
        {
            var newItem = CreateNewItemViewModel();
            Items.Add(newItem);
            HasUnsavedChanges = true;
            StatusMessage = "已添加新项";
            
            LogInfo($"Added new item to {EditorName}", "SimpleEditorViewModel");
        }, "SimpleEditorViewModel.AddItem");
    }

    /// <summary>
    /// 删除选中项
    /// </summary>
    protected void RemoveItem(TItemViewModel item)
    {
        ExecuteSafely(() =>
        {
            if (item != null)
            {
                Items.Remove(item);
                HasUnsavedChanges = true;
                StatusMessage = "已删除项";
                
                LogInfo($"Removed item from {EditorName}", "SimpleEditorViewModel.RemoveItem");
            }
        }, "SimpleEditorViewModel.RemoveItem");
    }

    // 实现BaseEditorViewModel的抽象方法
    protected override TModel LoadDataFromFile(string path)
    {
        var loader = new GenericXmlLoader<TModel>();
        return loader.Load(path) ?? new TModel();
    }

    protected override void SaveDataToFile(TModel data, string path)
    {
        var loader = new GenericXmlLoader<TModel>();
        loader.Save(data, path);
    }

    protected override IEnumerable<TItemModel> GetItemsFromData(TModel data)
    {
        // 需要在子类中实现具体的提取逻辑
        return Enumerable.Empty<TItemModel>();
    }

    protected override TModel CreateDataFromItems(ObservableCollection<TItemModel> items)
    {
        // 需要在子类中实现具体的创建逻辑
        return new TModel();
    }

    protected override TItemModel CreateNewItem()
    {
        // 创建默认的项模型
        return new TItemModel();
    }

    protected override TItemModel CreateErrorItem(string errorMessage)
    {
        // 创建错误项，子类可以重写此方法
        var item = new TItemModel();
        // 这里可以设置错误信息，具体取决于TItemModel的结构
        return item;
    }

    protected override bool MatchesSearchFilter(TItemModel item, string filter)
    {
        // 默认的搜索过滤实现，子类可以重写
        return item.ToString()?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}