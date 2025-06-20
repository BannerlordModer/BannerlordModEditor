using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models;
using System.Collections.ObjectModel;
using System.Xml;
using BannerlordModEditor.Common.Loaders;
using System.Collections.Generic;
using System.IO;
using System;

namespace BannerlordModEditor.UI.ViewModels.Editors;

public partial class AttributeEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<AttributeDataViewModel> attributes = new();

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    public AttributeEditorViewModel()
    {
        // 添加示例数据
        Attributes.Add(new AttributeDataViewModel 
        { 
            Id = "NewAttribute", 
            Name = "New Attribute", 
            Source = "Character",
            Documentation = "Enter documentation here..."
        });
    }

    [RelayCommand]
    private void AddAttribute()
    {
        var newAttribute = new AttributeDataViewModel 
        { 
            Id = $"NewAttribute{Attributes.Count + 1}", 
            Name = "New Attribute", 
            Source = "Character",
            Documentation = "Enter documentation here..."
        };
        
        Attributes.Add(newAttribute);
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveAttribute(AttributeDataViewModel attribute)
    {
        if (attribute != null)
        {
            Attributes.Remove(attribute);
            HasUnsavedChanges = true;
        }
    }

    [RelayCommand]
    private void LoadFile()
    {
        try
        {
            // 这里应该有文件选择对话框，暂时使用示例路径
            var samplePath = @"TestData\attributes.xml";
            LoadXmlFile(samplePath);
        }
        catch (Exception ex)
        {
            // 显示错误消息
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
        }
    }

    public void LoadXmlFile(string fileName)
    {
        try
        {
            // 尝试多个可能的路径
            var possiblePaths = new[]
            {
                Path.Combine("TestData", fileName),
                Path.Combine("BannerlordModEditor.Common.Tests", "TestData", fileName),
                fileName // 直接使用传入的文件名
            };

            string? foundPath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    foundPath = path;
                    break;
                }
            }

            if (foundPath != null)
            {
                var loader = new GenericXmlLoader<ArrayOfAttributeData>();
                var data = loader.Load(foundPath);
                
                if (data != null)
                {
                    Attributes.Clear();
                    foreach (var attr in data.AttributeData)
                    {
                        Attributes.Add(new AttributeDataViewModel
                        {
                            Id = attr.Id,
                            Name = attr.Name,
                            Source = attr.Source,
                            Documentation = attr.Documentation ?? string.Empty
                        });
                    }
                    
                    FilePath = foundPath;
                    HasUnsavedChanges = false;
                }
            }
            else
            {
                // 如果没有找到文件，创建空的编辑器
                Attributes.Clear();
                Attributes.Add(new AttributeDataViewModel 
                { 
                    Id = "NewAttribute", 
                    Name = "New Attribute", 
                    Source = "Character",
                    Documentation = $"正在编辑: {fileName}\n文件不存在，创建新的属性定义..."
                });
                FilePath = fileName;
                HasUnsavedChanges = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadXmlFile failed: {ex.Message}");
            // 创建空的编辑器以防止崩溃
            Attributes.Clear();
            Attributes.Add(new AttributeDataViewModel 
            { 
                Id = "Error", 
                Name = "Load Error", 
                Source = "Character",
                Documentation = $"加载 {fileName} 时出错: {ex.Message}"
            });
        }
    }

    [RelayCommand]
    private void SaveFile()
    {
        try
        {
            var data = new ArrayOfAttributeData();
            foreach (var attr in Attributes)
            {
                data.AttributeData.Add(new AttributeData
                {
                    Id = attr.Id,
                    Name = attr.Name,
                    Source = attr.Source,
                    Documentation = string.IsNullOrWhiteSpace(attr.Documentation) ? null : attr.Documentation
                });
            }

            var loader = new GenericXmlLoader<ArrayOfAttributeData>();
            if (!string.IsNullOrEmpty(FilePath))
            {
                loader.Save(data, FilePath);
                HasUnsavedChanges = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save failed: {ex.Message}");
        }
    }
}

public partial class AttributeDataViewModel : ObservableObject
{
    [ObservableProperty]
    private string id = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string source = "Character";

    [ObservableProperty]
    private string documentation = string.Empty;

    partial void OnIdChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnNameChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(Name);

    public IEnumerable<string> SourceOptions => new[] { "Character", "WieldedWeapon", "WieldedShield", "SumEquipment" };
} 