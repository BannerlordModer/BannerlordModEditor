using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 泛型编辑器视图模型基类，支持DO/DTO架构
/// </summary>
/// <typeparam name="TDO">领域对象类型</typeparam>
/// <typeparam name="TDTO">数据传输对象类型</typeparam>
/// <typeparam name="TItemDO">单个项的领域对象类型</typeparam>
/// <typeparam name="TItemDTO">单个项的数据传输对象类型</typeparam>
/// <typeparam name="TItemViewModel">项视图模型类型</typeparam>
public abstract partial class GenericEditorViewModel<TDO, TDTO, TItemDO, TItemDTO, TItemViewModel> : BaseEditorViewModel<TDO, TItemDO>
    where TDO : class, new()
    where TDTO : class, new()
    where TItemDO : class, new()
    where TItemDTO : class, new()
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

    protected GenericEditorViewModel(string xmlFileName, string editorName) 
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

    /// <summary>
    /// 删除视图模型项
    /// </summary>
    protected void RemoveItem(TItemViewModel? item)
    {
        if (item != null)
        {
            Items.Remove(item);
            HasUnsavedChanges = true;
            StatusMessage = "已删除项";
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        FilterText = string.Empty;
    }

    protected abstract TItemViewModel CreateNewItemViewModel();
    protected abstract TItemViewModel DuplicateItemViewModel(TItemViewModel source);
    protected abstract TItemDO ConvertToItemDO(TItemViewModel viewModel);
    protected abstract TItemViewModel ConvertToItemViewModel(TItemDO itemDO);
    protected abstract TDO ConvertToRootDO(ObservableCollection<TItemViewModel> items);
    protected abstract ObservableCollection<TItemViewModel> ConvertFromRootDO(TDO rootDO);
    protected abstract TDO ConvertToRootDO(TDTO dto);
    protected abstract TDTO ConvertToRootDTO(TDO data);

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
                    var loader = new GenericXmlLoader<TDTO>();
                    var dto = loader.Load(foundPath);
                    
                    if (dto != null)
                    {
                        var data = ConvertToRootDO(dto);
                        var itemDOs = GetItemsFromData(data);
                        Items.Clear();
                        foreach (var itemDO in itemDOs)
                        {
                            Items.Add(ConvertToItemViewModel(itemDO));
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
                var itemDOs = new ObservableCollection<TItemDO>();
                foreach (var itemViewModel in Items)
                {
                    itemDOs.Add(ConvertToItemDO(itemViewModel));
                }
                
                var rootDO = CreateDataFromItems(itemDOs);
                var dto = ConvertToRootDTO(rootDO);
                var loader = new GenericXmlLoader<TDTO>();
                loader.Save(dto, FilePath);
                
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

    // 实现BaseEditorViewModel的抽象方法
    protected override TDO LoadDataFromFile(string path)
    {
        var loader = new GenericXmlLoader<TDTO>();
        var dto = loader.Load(path);
        return ConvertToRootDO(dto);
    }

    protected override void SaveDataToFile(TDO data, string path)
    {
        var dto = ConvertToRootDTO(data);
        var loader = new GenericXmlLoader<TDTO>();
        loader.Save(dto, path);
    }

    protected override IEnumerable<TItemDO> GetItemsFromData(TDO data)
    {
        // 需要在子类中实现具体的提取逻辑
        return Enumerable.Empty<TItemDO>();
    }

    protected override TDO CreateDataFromItems(ObservableCollection<TItemDO> items)
    {
        // 需要在子类中实现具体的创建逻辑
        return new TDO();
    }

    protected override TItemDO CreateNewItem()
    {
        // 创建默认的项DO
        return new TItemDO();
    }

    protected override TItemDO CreateErrorItem(string errorMessage)
    {
        // 创建错误项，子类可以重写此方法
        var item = new TItemDO();
        // 这里可以设置错误信息，具体取决于TItemDO的结构
        return item;
    }

    protected override bool MatchesSearchFilter(TItemDO item, string filter)
    {
        // 默认的搜索过滤实现，子类可以重写
        return item.ToString()?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    }