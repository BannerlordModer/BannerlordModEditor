using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models.Data;
using System.Collections.ObjectModel;
using BannerlordModEditor.Common.Loaders;
using System.Collections.Generic;
using System.IO;
using System;

namespace BannerlordModEditor.UI.ViewModels.Editors;

public partial class SkillEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<SkillDataViewModel> skills = new();

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    public SkillEditorViewModel()
    {
        // 添加示例数据
        Skills.Add(new SkillDataViewModel 
        { 
            Id = "NewSkill", 
            Name = "New Skill", 
            Documentation = "Enter documentation here..."
        });
    }

    [RelayCommand]
    private void AddSkill()
    {
        var newSkill = new SkillDataViewModel 
        { 
            Id = $"NewSkill{Skills.Count + 1}", 
            Name = "New Skill", 
            Documentation = "Enter documentation here..."
        };
        
        Skills.Add(newSkill);
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveSkill(SkillDataViewModel skill)
    {
        if (skill != null)
        {
            Skills.Remove(skill);
            HasUnsavedChanges = true;
        }
    }

    [RelayCommand]
    private void LoadFile()
    {
        try
        {
            var samplePath = @"TestData\skills.xml";
            LoadXmlFile(samplePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
        }
    }

    public void LoadXmlFile(string fileName)
    {
        try
        {
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
                var loader = new GenericXmlLoader<ArrayOfSkillData>();
                var data = loader.Load(foundPath);
                
                if (data != null)
                {
                    Skills.Clear();
                    foreach (var skill in data.SkillDataList)
                    {
                        Skills.Add(new SkillDataViewModel
                        {
                            Id = skill.Id ?? string.Empty,
                            Name = skill.Name ?? string.Empty,
                            Documentation = skill.Documentation ?? string.Empty
                        });
                    }
                    
                    FilePath = foundPath;
                    HasUnsavedChanges = false;
                }
            }
            else
            {
                Skills.Clear();
                Skills.Add(new SkillDataViewModel 
                { 
                    Id = "NewSkill", 
                    Name = "New Skill", 
                    Documentation = $"正在编辑: {fileName}\n文件不存在，创建新的技能定义..."
                });
                FilePath = fileName;
                HasUnsavedChanges = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadXmlFile failed: {ex.Message}");
            Skills.Clear();
            Skills.Add(new SkillDataViewModel 
            { 
                Id = "Error", 
                Name = "Load Error", 
                Documentation = $"加载 {fileName} 时出错: {ex.Message}"
            });
        }
    }

    [RelayCommand]
    private void SaveFile()
    {
        try
        {
            var data = new ArrayOfSkillData();
            foreach (var skill in Skills)
            {
                data.SkillDataList.Add(new SkillData
                {
                    Id = skill.Id,
                    Name = skill.Name,
                    Documentation = string.IsNullOrWhiteSpace(skill.Documentation) ? string.Empty : skill.Documentation
                });
            }

            var loader = new GenericXmlLoader<ArrayOfSkillData>();
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

public partial class SkillDataViewModel : ObservableObject
{
    [ObservableProperty]
    private string id = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

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
} 