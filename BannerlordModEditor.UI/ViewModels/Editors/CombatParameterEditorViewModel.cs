using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 战斗参数编辑器视图模型
/// </summary>
[EditorType(
    EditorType = "CombatParameterEditor",
    DisplayName = "战斗参数编辑器",
    Description = "编辑战斗参数",
    XmlFileName = "combat_parameters.xml",
    Category = "Combat",
    SupportsDto = true)]
public partial class CombatParameterEditorViewModel : GenericEditorViewModel<
    CombatParametersDO, CombatParametersDTO, BaseCombatParameterDO, BaseCombatParameterDTO, CombatParameterViewModel>
{
    [ObservableProperty]
    private ObservableCollection<CombatParameterViewModel> combatParameters = new();

    [ObservableProperty]
    private CombatParameterViewModel? selectedCombatParameter;

    [ObservableProperty]
    private ObservableCollection<DefinitionViewModel> definitions = new();

    [ObservableProperty]
    private DefinitionViewModel? selectedDefinition;

    [ObservableProperty]
    private string? type;

    [ObservableProperty]
    private bool hasDefinitions;

    [ObservableProperty]
    private bool hasEmptyCombatParameters;

    private readonly IValidationService _validationService;

    public CombatParameterEditorViewModel(IValidationService? validationService = null) 
        : base("combat_parameters.xml", "战斗参数编辑器")
    {
        _validationService = validationService ?? new ValidationService();
        
        // 初始化示例数据
        InitializeSampleData();
        
        Items = CombatParameters;
    }

    private void InitializeSampleData()
    {
        // 添加示例定义
        Definitions.Add(new DefinitionViewModel 
        { 
            Name = "example_def", 
            Value = "1.0" 
        });
        
        // 添加示例战斗参数
        CombatParameters.Add(new CombatParameterViewModel 
        { 
            Id = "example_parameter",
            CollisionCheckStartingPercent = "0.1",
            CollisionDamageStartingPercent = "0.2",
            CollisionCheckEndingPercent = "0.8",
            CollisionRadius = "0.5"
        });
    }

    protected override bool ItemMatchesFilter(CombatParameterViewModel item, string filterText)
    {
        return item.Id?.Contains(filterText, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    protected override CombatParameterViewModel CreateNewItemViewModel()
    {
        return new CombatParameterViewModel 
        { 
            Id = $"parameter_{CombatParameters.Count + 1}" 
        };
    }

    protected override CombatParameterViewModel DuplicateItemViewModel(CombatParameterViewModel source)
    {
        return new CombatParameterViewModel
        {
            Id = $"{source.Id}_Copy",
            CollisionCheckStartingPercent = source.CollisionCheckStartingPercent,
            CollisionDamageStartingPercent = source.CollisionDamageStartingPercent,
            CollisionCheckEndingPercent = source.CollisionCheckEndingPercent,
            VerticalRotLimitMultiplierUp = source.VerticalRotLimitMultiplierUp,
            VerticalRotLimitMultiplierDown = source.VerticalRotLimitMultiplierDown,
            LeftRiderRotLimit = source.LeftRiderRotLimit,
            LeftRiderMinRotLimit = source.LeftRiderMinRotLimit,
            RightRiderRotLimit = source.RightRiderRotLimit,
            RightRiderMinRotLimit = source.RightRiderMinRotLimit,
            RiderLookDownLimit = source.RiderLookDownLimit,
            LeftLadderRotLimit = source.LeftLadderRotLimit,
            RightLadderRotLimit = source.RightLadderRotLimit,
            WeaponOffset = source.WeaponOffset,
            CollisionRadius = source.CollisionRadius,
            AlternativeAttackCooldownPeriod = source.AlternativeAttackCooldownPeriod,
            HitBoneIndex = source.HitBoneIndex,
            ShoulderHitBoneIndex = source.ShoulderHitBoneIndex,
            LookSlopeBlendFactorUpLimit = source.LookSlopeBlendFactorUpLimit,
            LookSlopeBlendFactorDownLimit = source.LookSlopeBlendFactorDownLimit,
            LookSlopeBlendSpeedFactor = source.LookSlopeBlendSpeedFactor,
            HasCustomCollisionCapsule = source.HasCustomCollisionCapsule,
            CustomCollisionCapsuleP1 = source.CustomCollisionCapsuleP1,
            CustomCollisionCapsuleP2 = source.CustomCollisionCapsuleP2,
            CustomCollisionCapsuleR = source.CustomCollisionCapsuleR
        };
    }

    protected override BaseCombatParameterDO ConvertToItemDO(CombatParameterViewModel viewModel)
    {
        var parameter = new BaseCombatParameterDO
        {
            Id = viewModel.Id,
            CollisionCheckStartingPercent = viewModel.CollisionCheckStartingPercent,
            CollisionDamageStartingPercent = viewModel.CollisionDamageStartingPercent,
            CollisionCheckEndingPercent = viewModel.CollisionCheckEndingPercent,
            VerticalRotLimitMultiplierUp = viewModel.VerticalRotLimitMultiplierUp,
            VerticalRotLimitMultiplierDown = viewModel.VerticalRotLimitMultiplierDown,
            LeftRiderRotLimit = viewModel.LeftRiderRotLimit,
            LeftRiderMinRotLimit = viewModel.LeftRiderMinRotLimit,
            RightRiderRotLimit = viewModel.RightRiderRotLimit,
            RightRiderMinRotLimit = viewModel.RightRiderMinRotLimit,
            RiderLookDownLimit = viewModel.RiderLookDownLimit,
            LeftLadderRotLimit = viewModel.LeftLadderRotLimit,
            RightLadderRotLimit = viewModel.RightLadderRotLimit,
            WeaponOffset = viewModel.WeaponOffset,
            CollisionRadius = viewModel.CollisionRadius,
            AlternativeAttackCooldownPeriod = viewModel.AlternativeAttackCooldownPeriod,
            HitBoneIndex = viewModel.HitBoneIndex,
            ShoulderHitBoneIndex = viewModel.ShoulderHitBoneIndex,
            LookSlopeBlendFactorUpLimit = viewModel.LookSlopeBlendFactorUpLimit,
            LookSlopeBlendFactorDownLimit = viewModel.LookSlopeBlendFactorDownLimit,
            LookSlopeBlendSpeedFactor = viewModel.LookSlopeBlendSpeedFactor
        };

        // 处理自定义碰撞胶囊
        if (viewModel.HasCustomCollisionCapsule)
        {
            parameter.CustomCollisionCapsule = new CustomCollisionCapsuleDO
            {
                P1 = viewModel.CustomCollisionCapsuleP1,
                P2 = viewModel.CustomCollisionCapsuleP2,
                R = viewModel.CustomCollisionCapsuleR
            };
        }

        return parameter;
    }

    protected override CombatParameterViewModel ConvertToItemViewModel(BaseCombatParameterDO itemDO)
    {
        var viewModel = new CombatParameterViewModel
        {
            Id = itemDO.Id,
            CollisionCheckStartingPercent = itemDO.CollisionCheckStartingPercent,
            CollisionDamageStartingPercent = itemDO.CollisionDamageStartingPercent,
            CollisionCheckEndingPercent = itemDO.CollisionCheckEndingPercent,
            VerticalRotLimitMultiplierUp = itemDO.VerticalRotLimitMultiplierUp,
            VerticalRotLimitMultiplierDown = itemDO.VerticalRotLimitMultiplierDown,
            LeftRiderRotLimit = itemDO.LeftRiderRotLimit,
            LeftRiderMinRotLimit = itemDO.LeftRiderMinRotLimit,
            RightRiderRotLimit = itemDO.RightRiderRotLimit,
            RightRiderMinRotLimit = itemDO.RightRiderMinRotLimit,
            RiderLookDownLimit = itemDO.RiderLookDownLimit,
            LeftLadderRotLimit = itemDO.LeftLadderRotLimit,
            RightLadderRotLimit = itemDO.RightLadderRotLimit,
            WeaponOffset = itemDO.WeaponOffset,
            CollisionRadius = itemDO.CollisionRadius,
            AlternativeAttackCooldownPeriod = itemDO.AlternativeAttackCooldownPeriod,
            HitBoneIndex = itemDO.HitBoneIndex,
            ShoulderHitBoneIndex = itemDO.ShoulderHitBoneIndex,
            LookSlopeBlendFactorUpLimit = itemDO.LookSlopeBlendFactorUpLimit,
            LookSlopeBlendFactorDownLimit = itemDO.LookSlopeBlendFactorDownLimit,
            LookSlopeBlendSpeedFactor = itemDO.LookSlopeBlendSpeedFactor
        };

        // 处理自定义碰撞胶囊
        if (itemDO.CustomCollisionCapsule != null)
        {
            viewModel.HasCustomCollisionCapsule = true;
            viewModel.CustomCollisionCapsuleP1 = itemDO.CustomCollisionCapsule.P1;
            viewModel.CustomCollisionCapsuleP2 = itemDO.CustomCollisionCapsule.P2;
            viewModel.CustomCollisionCapsuleR = itemDO.CustomCollisionCapsule.R;
        }

        return viewModel;
    }

    protected override CombatParametersDO ConvertToRootDO(ObservableCollection<CombatParameterViewModel> items)
    {
        var root = new CombatParametersDO
        {
            Type = Type,
            HasDefinitions = HasDefinitions,
            HasEmptyCombatParameters = HasEmptyCombatParameters
        };

        // 转换定义
        if (HasDefinitions)
        {
            root.Definitions = new DefinitionsDO();
            foreach (var def in Definitions)
            {
                root.Definitions.Defs.Add(new DefDO
                {
                    Name = def.Name,
                    Value = def.Value
                });
            }
        }

        // 转换战斗参数
        foreach (var item in items)
        {
            root.CombatParametersList.Add(ConvertToItemDO(item));
        }

        return root;
    }

    protected override ObservableCollection<CombatParameterViewModel> ConvertFromRootDO(CombatParametersDO rootDO)
    {
        var result = new ObservableCollection<CombatParameterViewModel>();

        // 设置属性
        Type = rootDO.Type;
        HasDefinitions = rootDO.HasDefinitions;
        HasEmptyCombatParameters = rootDO.HasEmptyCombatParameters;

        // 转换定义
        if (rootDO.HasDefinitions && rootDO.Definitions != null)
        {
            Definitions.Clear();
            foreach (var def in rootDO.Definitions.Defs)
            {
                Definitions.Add(new DefinitionViewModel
                {
                    Name = def.Name,
                    Value = def.Value
                });
            }
        }

        // 转换战斗参数
        if (rootDO.CombatParametersList != null)
        {
            foreach (var param in rootDO.CombatParametersList)
            {
                result.Add(ConvertToItemViewModel(param));
            }
        }

        return result;
    }

    [RelayCommand]
    public void AddCombatParameter()
    {
        AddItem();
    }

    [RelayCommand]
    public void RemoveCombatParameter(CombatParameterViewModel? parameter)
    {
        if (parameter != null)
        {
            RemoveItem(parameter);
        }
    }

    [RelayCommand]
    private void DuplicateCombatParameter(CombatParameterViewModel? parameter)
    {
        if (parameter != null)
        {
            DuplicateItem(parameter);
        }
    }

    [RelayCommand]
    public void AddDefinition()
    {
        var newDefinition = new DefinitionViewModel 
        { 
            Name = $"def_{Definitions.Count + 1}", 
            Value = "1.0" 
        };
        
        Definitions.Add(newDefinition);
        HasDefinitions = true;
        HasUnsavedChanges = true;
        StatusMessage = "已添加新定义";
    }

    [RelayCommand]
    public void RemoveDefinition(DefinitionViewModel? definition)
    {
        if (definition != null)
        {
            Definitions.Remove(definition);
            HasUnsavedChanges = true;
            StatusMessage = "已删除定义";
            
            // 如果没有定义了，设置HasDefinitions为false
            if (Definitions.Count == 0)
            {
                HasDefinitions = false;
            }
        }
    }

    [RelayCommand]
    public void ValidateAll()
    {
        var isValid = true;
        
        // 验证战斗参数
        foreach (var param in CombatParameters)
        {
            if (!param.IsValid)
            {
                isValid = false;
                break;
            }
        }
        
        // 验证定义
        foreach (var def in Definitions)
        {
            if (!def.IsValid)
            {
                isValid = false;
                break;
            }
        }
        
        StatusMessage = isValid ? "所有数据验证通过" : "存在验证错误";
    }

    [RelayCommand]
    public void ToggleEmptyCombatParameters()
    {
        HasEmptyCombatParameters = !HasEmptyCombatParameters;
        HasUnsavedChanges = true;
        StatusMessage = HasEmptyCombatParameters ? "已启用空战斗参数" : "已禁用空战斗参数";
    }

    // 实现GenericEditorViewModel的抽象方法
    protected override CombatParametersDO ConvertToRootDO(CombatParametersDTO dto)
    {
        return CombatParametersMapper.ToDO(dto);
    }

    protected override CombatParametersDTO ConvertToRootDTO(CombatParametersDO data)
    {
        return CombatParametersMapper.ToDTO(data);
    }

    /// <summary>
    /// 获取验证服务（用于测试）
    /// </summary>
    public IValidationService ValidationService => _validationService;
}

/// <summary>
/// 战斗参数视图模型
/// </summary>
public partial class CombatParameterViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "ID不能为空")]
    [StringLength(100, ErrorMessage = "ID长度不能超过100个字符")]
    private string? id;

    [ObservableProperty]
    [Range(0, 1, ErrorMessage = "碰撞检查起始百分比必须在0-1之间")]
    private string? collisionCheckStartingPercent;

    [ObservableProperty]
    [Range(0, 1, ErrorMessage = "碰撞伤害起始百分比必须在0-1之间")]
    private string? collisionDamageStartingPercent;

    [ObservableProperty]
    [Range(0, 1, ErrorMessage = "碰撞检查结束百分比必须在0-1之间")]
    private string? collisionCheckEndingPercent;

    [ObservableProperty]
    private string? verticalRotLimitMultiplierUp;

    [ObservableProperty]
    private string? verticalRotLimitMultiplierDown;

    [ObservableProperty]
    private string? leftRiderRotLimit;

    [ObservableProperty]
    private string? leftRiderMinRotLimit;

    [ObservableProperty]
    private string? rightRiderRotLimit;

    [ObservableProperty]
    private string? rightRiderMinRotLimit;

    [ObservableProperty]
    private string? riderLookDownLimit;

    [ObservableProperty]
    private string? leftLadderRotLimit;

    [ObservableProperty]
    private string? rightLadderRotLimit;

    [ObservableProperty]
    private string? weaponOffset;

    [ObservableProperty]
    [Range(0, double.MaxValue, ErrorMessage = "碰撞半径必须大于等于0")]
    private string? collisionRadius;

    [ObservableProperty]
    private string? alternativeAttackCooldownPeriod;

    [ObservableProperty]
    private string? hitBoneIndex;

    [ObservableProperty]
    private string? shoulderHitBoneIndex;

    [ObservableProperty]
    private string? lookSlopeBlendFactorUpLimit;

    [ObservableProperty]
    private string? lookSlopeBlendFactorDownLimit;

    [ObservableProperty]
    private string? lookSlopeBlendSpeedFactor;

    [ObservableProperty]
    private bool hasCustomCollisionCapsule;

    [ObservableProperty]
    private string? customCollisionCapsuleP1;

    [ObservableProperty]
    private string? customCollisionCapsuleP2;

    [ObservableProperty]
    private string? customCollisionCapsuleR;

    private readonly List<string> _validationErrors = new();

    public bool IsValid => !string.IsNullOrWhiteSpace(Id) && _validationErrors.Count == 0;

    public IReadOnlyList<string> ValidationErrors => _validationErrors.AsReadOnly();

    partial void OnIdChanged(string? value)
    {
        ValidateProperty(nameof(Id));
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnCollisionCheckStartingPercentChanged(string? value)
    {
        ValidateProperty(nameof(CollisionCheckStartingPercent));
        OnPropertyChanged(nameof(IsValid));
    }

    private void ValidateProperty(string propertyName)
    {
        var validationService = new ValidationService();
        var errors = validationService.ValidateProperty(this, propertyName);
        
        _validationErrors.RemoveAll(e => e.StartsWith($"{propertyName}:"));
        _validationErrors.AddRange(errors.Select(e => $"{propertyName}: {e}"));
        
        OnPropertyChanged(nameof(ValidationErrors));
    }
}

/// <summary>
/// 定义视图模型
/// </summary>
public partial class DefinitionViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "名称不能为空")]
    [StringLength(100, ErrorMessage = "名称长度不能超过100个字符")]
    private string? name;

    [ObservableProperty]
    [Required(ErrorMessage = "值不能为空")]
    private string? value;

    private readonly List<string> _validationErrors = new();

    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && 
                           !string.IsNullOrWhiteSpace(Value) && 
                           _validationErrors.Count == 0;

    public IReadOnlyList<string> ValidationErrors => _validationErrors.AsReadOnly();

    partial void OnNameChanged(string? value)
    {
        ValidateProperty(nameof(Name));
        OnPropertyChanged(nameof(IsValid));
    }

    partial void OnValueChanged(string? value)
    {
        ValidateProperty(nameof(Value));
        OnPropertyChanged(nameof(IsValid));
    }

    private void ValidateProperty(string propertyName)
    {
        var validationService = new ValidationService();
        var errors = validationService.ValidateProperty(this, propertyName);
        
        _validationErrors.RemoveAll(e => e.StartsWith($"{propertyName}:"));
        _validationErrors.AddRange(errors.Select(e => $"{propertyName}: {e}"));
        
        OnPropertyChanged(nameof(ValidationErrors));
    }
}