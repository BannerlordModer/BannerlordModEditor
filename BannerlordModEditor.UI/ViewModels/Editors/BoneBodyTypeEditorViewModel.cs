using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models;
using System.Collections.ObjectModel;
using BannerlordModEditor.Common.Loaders;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Services;

namespace BannerlordModEditor.UI.ViewModels.Editors;

public partial class BoneBodyTypeEditorViewModel : ViewModelBase
{
    private readonly IValidationService _validationService;

    [ObservableProperty]
    private ObservableCollection<BoneBodyTypeViewModel> boneBodyTypes = new();

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    public BoneBodyTypeEditorViewModel(IValidationService? validationService = null)
    {
        _validationService = validationService ?? new ValidationService();
        
        // 添加示例数据
        BoneBodyTypes.Add(new BoneBodyTypeViewModel 
        { 
            Type = "head",
            Priority = "4",
            DoNotScaleAccordingToAgentScale = "true"
        });
    }

    [RelayCommand]
    private void AddBoneBodyType()
    {
        var newBoneType = new BoneBodyTypeViewModel 
        { 
            Type = "new_bone_type",
            Priority = "1"
        };
        
        BoneBodyTypes.Add(newBoneType);
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveBoneBodyType(BoneBodyTypeViewModel boneType)
    {
        if (boneType != null)
        {
            BoneBodyTypes.Remove(boneType);
            HasUnsavedChanges = true;
        }
    }

    [RelayCommand]
    private void LoadFile()
    {
        try
        {
            var samplePath = @"TestData\bone_body_types.xml";
            LoadXmlFile(samplePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
        }
    }

    public async Task LoadXmlFileAsync(string fileName)
    {
        try
        {
            await Task.Run(() =>
            {
                // 尝试多个可能的路径
                var possiblePaths = new[]
                {
                    Path.Combine("TestData", fileName),
                    Path.Combine("BannerlordModEditor.Common.Tests", "TestData", fileName),
                    fileName
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
                    var loader = new GenericXmlLoader<BoneBodyTypes>();
                    var data = loader.Load(foundPath);
                    
                    if (data != null)
                    {
                        BoneBodyTypes.Clear();
                        foreach (var boneType in data.BoneBodyType)
                        {
                            BoneBodyTypes.Add(new BoneBodyTypeViewModel
                            {
                                Type = boneType.Type,
                                Priority = boneType.Priority,
                                ActivateSweep = boneType.ActivateSweep,
                                UseSmallerRadiusMultWhileHoldingShield = boneType.UseSmallerRadiusMultWhileHoldingShield,
                                DoNotScaleAccordingToAgentScale = boneType.DoNotScaleAccordingToAgentScale
                            });
                        }
                        
                        FilePath = foundPath;
                        HasUnsavedChanges = false;
                    }
                }
                else
                {
                    // 如果没有找到文件，创建空的编辑器
                    BoneBodyTypes.Clear();
                    BoneBodyTypes.Add(new BoneBodyTypeViewModel 
                    { 
                        Type = "new_bone_type",
                        Priority = "1"
                    });
                    FilePath = fileName;
                    HasUnsavedChanges = false;
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadXmlFile failed: {ex.Message}");
            // 创建空的编辑器以防止崩溃
            BoneBodyTypes.Clear();
            BoneBodyTypes.Add(new BoneBodyTypeViewModel 
            { 
                Type = "error",
                Priority = "1"
            });
        }
    }

    public void LoadXmlFile(string fileName)
    {
        LoadXmlFileAsync(fileName).Wait();
    }

    [RelayCommand]
    private void SaveFile()
    {
        try
        {
            var data = new BoneBodyTypes();
            foreach (var boneType in BoneBodyTypes)
            {
                data.BoneBodyType.Add(new BoneBodyType
                {
                    Type = boneType.Type,
                    Priority = boneType.Priority,
                    ActivateSweep = string.IsNullOrWhiteSpace(boneType.ActivateSweep) ? null : boneType.ActivateSweep,
                    UseSmallerRadiusMultWhileHoldingShield = string.IsNullOrWhiteSpace(boneType.UseSmallerRadiusMultWhileHoldingShield) ? null : boneType.UseSmallerRadiusMultWhileHoldingShield,
                    DoNotScaleAccordingToAgentScale = string.IsNullOrWhiteSpace(boneType.DoNotScaleAccordingToAgentScale) ? null : boneType.DoNotScaleAccordingToAgentScale
                });
            }

            var loader = new GenericXmlLoader<BoneBodyTypes>();
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

    /// <summary>
    /// 获取验证服务（用于测试）
    /// </summary>
    public IValidationService ValidationService => _validationService;
}

public partial class BoneBodyTypeViewModel : ObservableObject
{
    [ObservableProperty]
    private string type = string.Empty;

    [ObservableProperty]
    private string priority = "1";

    [ObservableProperty]
    private string? activateSweep;

    [ObservableProperty]
    private string? useSmallerRadiusMultWhileHoldingShield;

    [ObservableProperty]
    private string? doNotScaleAccordingToAgentScale;

    partial void OnTypeChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnPriorityChanged(string value)
    {
        OnPropertyChanged(nameof(IsValid));
    }

    public bool IsValid => 
        !string.IsNullOrWhiteSpace(Type) && 
        !string.IsNullOrWhiteSpace(Priority) &&
        int.TryParse(Priority, out var priorityNum) && priorityNum >= 1 && priorityNum <= 5;

    public IEnumerable<string> TypeOptions => new[] 
    { 
        "head", "neck", "chest", "abdomen", "shoulder_left", "shoulder_right", 
        "arm_left", "arm_right", "legs" 
    };

    public IEnumerable<string> PriorityOptions => new[] { "1", "2", "3", "4", "5" };

    public IEnumerable<string> BooleanOptions => new[] { "", "true", "false" };
} 