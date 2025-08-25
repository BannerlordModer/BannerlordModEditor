using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的战斗参数数据模型
/// </summary>
public class MockCombatParameterData
{
    public string Id { get; set; } = string.Empty;
    public string CollisionRadius { get; set; } = string.Empty;
    public string CollisionCheckStartingPercent { get; set; } = string.Empty;
    public string CollisionDamageStartingPercent { get; set; } = string.Empty;
    public string CollisionCheckEndingPercent { get; set; } = string.Empty;
    public string VerticalRotLimitMultiplierUp { get; set; } = string.Empty;
    public string VerticalRotLimitMultiplierDown { get; set; } = string.Empty;
    public string LeftRiderRotLimit { get; set; } = string.Empty;
    public string RightRiderRotLimit { get; set; } = string.Empty;
    public string LeftLadderRotLimit { get; set; } = string.Empty;
    public string RightLadderRotLimit { get; set; } = string.Empty;
    public string WeaponOffset { get; set; } = string.Empty;
    public string AlternativeAttackCooldownPeriod { get; set; } = string.Empty;
    public string HitBoneIndex { get; set; } = string.Empty;
    public string ShoulderHitBoneIndex { get; set; } = string.Empty;
    public string RiderLookDownLimit { get; set; } = string.Empty;
    public bool HasCustomCollisionCapsule { get; set; }
    public string CustomCollisionCapsuleP1 { get; set; } = string.Empty;
    public string CustomCollisionCapsuleP2 { get; set; } = string.Empty;
    public string CustomCollisionCapsuleR { get; set; } = string.Empty;
    public bool IsValid => !string.IsNullOrWhiteSpace(Id);
}

/// <summary>
/// 模拟的定义数据模型
/// </summary>
public class MockDefinitionData
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Value);
}

/// <summary>
/// 模拟的CombatParameterEditorViewModel用于测试
/// </summary>
public partial class MockCombatParameterEditorViewModel : ObservableObject
{
    private string _type = string.Empty;
    private string _filterText = string.Empty;
    private string _statusMessage = "就绪";
    private MockCombatParameterData? _selectedCombatParameter;
    private MockDefinitionData? _selectedDefinition;
    private bool _hasEmptyCombatParameters;

    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    public string FilterText
    {
        get => _filterText;
        set
        {
            SetProperty(ref _filterText, value);
            ApplyFilter();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public MockCombatParameterData? SelectedCombatParameter
    {
        get => _selectedCombatParameter;
        set => SetProperty(ref _selectedCombatParameter, value);
    }

    public MockDefinitionData? SelectedDefinition
    {
        get => _selectedDefinition;
        set => SetProperty(ref _selectedDefinition, value);
    }

    public bool HasEmptyCombatParameters
    {
        get => _hasEmptyCombatParameters;
        set => SetProperty(ref _hasEmptyCombatParameters, value);
    }

    public ObservableCollection<MockCombatParameterData> CombatParameters { get; } = new();
    public ObservableCollection<MockDefinitionData> Definitions { get; } = new();
    public ObservableCollection<MockCombatParameterData> FilteredItems { get; } = new();

    public MockCombatParameterEditorViewModel()
    {
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        // 添加示例战斗参数数据
        CombatParameters.Add(new MockCombatParameterData
        {
            Id = "human_combat_parameters",
            CollisionRadius = "0.5",
            CollisionCheckStartingPercent = "0.1",
            CollisionDamageStartingPercent = "0.2",
            CollisionCheckEndingPercent = "0.8",
            VerticalRotLimitMultiplierUp = "1.0",
            VerticalRotLimitMultiplierDown = "1.0",
            LeftRiderRotLimit = "45",
            RightRiderRotLimit = "45",
            LeftLadderRotLimit = "30",
            RightLadderRotLimit = "30",
            HasCustomCollisionCapsule = false
        });

        CombatParameters.Add(new MockCombatParameterData
        {
            Id = "horse_combat_parameters",
            CollisionRadius = "0.8",
            CollisionCheckStartingPercent = "0.15",
            CollisionDamageStartingPercent = "0.25",
            CollisionCheckEndingPercent = "0.9",
            VerticalRotLimitMultiplierUp = "0.8",
            VerticalRotLimitMultiplierDown = "0.8",
            LeftRiderRotLimit = "60",
            RightRiderRotLimit = "60",
            LeftLadderRotLimit = "40",
            RightLadderRotLimit = "40",
            HasCustomCollisionCapsule = true,
            CustomCollisionCapsuleP1 = "0,0,0",
            CustomCollisionCapsuleP2 = "0,1,0",
            CustomCollisionCapsuleR = "0.5"
        });

        // 添加示例定义数据
        Definitions.Add(new MockDefinitionData
        {
            Name = "default_combat_speed",
            Value = "1.0"
        });

        Definitions.Add(new MockDefinitionData
        {
            Name = "melee_damage_multiplier",
            Value = "1.2"
        });

        // 初始化过滤后的参数列表
        foreach (var param in CombatParameters)
        {
            FilteredItems.Add(param);
        }
    }

    [RelayCommand]
    public void AddCombatParameter()
    {
        var newParameter = new MockCombatParameterData
        {
            Id = $"new_parameter_{CombatParameters.Count + 1}",
            CollisionRadius = "0.5",
            CollisionCheckStartingPercent = "0.1",
            CollisionDamageStartingPercent = "0.2",
            CollisionCheckEndingPercent = "0.8"
        };

        CombatParameters.Add(newParameter);
        ApplyFilter();
        StatusMessage = "已添加新战斗参数";
    }

    [RelayCommand]
    public void AddDefinition()
    {
        var newDefinition = new MockDefinitionData
        {
            Name = $"new_definition_{Definitions.Count + 1}",
            Value = "1.0"
        };

        Definitions.Add(newDefinition);
        StatusMessage = "已添加新定义";
    }

    [RelayCommand]
    public void RemoveDefinition(MockDefinitionData? definition)
    {
        if (definition != null)
        {
            Definitions.Remove(definition);
            StatusMessage = "已删除定义";
        }
    }

    [RelayCommand]
    public void ValidateAll()
    {
        var invalidParams = CombatParameters.Count(p => !p.IsValid);
        var invalidDefs = Definitions.Count(d => !d.IsValid);
        
        StatusMessage = invalidParams == 0 && invalidDefs == 0 
            ? "所有项目验证通过" 
            : $"发现 {invalidParams} 个无效参数和 {invalidDefs} 个无效定义";
    }

    [RelayCommand]
    public void ToggleEmptyCombatParameters()
    {
        HasEmptyCombatParameters = !HasEmptyCombatParameters;
        StatusMessage = HasEmptyCombatParameters ? "已启用空参数" : "已禁用空参数";
    }

    [RelayCommand]
    public async Task LoadFile()
    {
        StatusMessage = "正在加载文件...";
        await Task.Delay(100); // 模拟异步操作
        Type = "combat_parameters";
        StatusMessage = "文件加载完成";
    }

    [RelayCommand]
    public async Task SaveFile()
    {
        StatusMessage = "正在保存文件...";
        await Task.Delay(100); // 模拟异步操作
        StatusMessage = "文件保存完成";
    }

    private void ApplyFilter()
    {
        FilteredItems.Clear();

        var filtered = CombatParameters.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(FilterText))
        {
            var filterLower = FilterText.ToLower();
            filtered = filtered.Where(param => 
                param.Id.ToLower().Contains(filterLower) ||
                param.CollisionRadius.ToLower().Contains(filterLower));
        }

        foreach (var param in filtered)
        {
            FilteredItems.Add(param);
        }
    }
}