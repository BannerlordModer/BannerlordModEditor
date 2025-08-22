using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models;
using System.Collections.ObjectModel;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 属性编辑器视图模型 - 使用新的架构
/// </summary>
[EditorType(
    EditorType = "AttributeEditor",
    DisplayName = "属性编辑器",
    Description = "编辑角色属性",
    XmlFileName = "attributes.xml",
    Category = "Character")]
public partial class AttributeEditorViewModel : SimpleEditorViewModel<ArrayOfAttributeData, AttributeData, AttributeDataViewModel>
{
    [ObservableProperty]
    private ObservableCollection<AttributeDataViewModel> attributes = new();

    [ObservableProperty]
    private AttributeDataViewModel? selectedAttribute;

    private readonly IValidationService _validationService;

    public AttributeEditorViewModel(IValidationService? validationService = null) 
        : base("attributes.xml", "属性编辑器")
    {
        _validationService = validationService ?? new ValidationService();
        
        // 初始化时添加示例数据
        Attributes.Add(new AttributeDataViewModel 
        { 
            Id = "NewAttribute", 
            Name = "New Attribute", 
            Source = "Character",
            Documentation = "Enter documentation here..."
        });
        
        Items = Attributes;
    }

    protected override bool ItemMatchesFilter(AttributeDataViewModel item, string filterText)
    {
        return item.Id.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
               item.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
               item.Documentation.Contains(filterText, StringComparison.OrdinalIgnoreCase);
    }

    protected override AttributeDataViewModel CreateNewItemViewModel()
    {
        return new AttributeDataViewModel 
        { 
            Id = $"NewAttribute{Attributes.Count + 1}", 
            Name = "New Attribute", 
            Source = "Character",
            Documentation = "Enter documentation here..."
        };
    }

    protected override AttributeDataViewModel DuplicateItemViewModel(AttributeDataViewModel source)
    {
        return new AttributeDataViewModel
        {
            Id = $"{source.Id}_Copy",
            Name = $"{source.Name} (Copy)",
            Source = source.Source,
            Documentation = source.Documentation,
            DefaultValue = source.DefaultValue
        };
    }

    protected override AttributeData ConvertToItemModel(AttributeDataViewModel viewModel)
    {
        return new AttributeData
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Source = viewModel.Source,
            Documentation = string.IsNullOrWhiteSpace(viewModel.Documentation) ? null : viewModel.Documentation,
            DefaultValue = string.IsNullOrWhiteSpace(viewModel.DefaultValue) ? null : viewModel.DefaultValue
        };
    }

    protected override AttributeDataViewModel ConvertToItemViewModel(AttributeData itemModel)
    {
        return new AttributeDataViewModel
        {
            Id = itemModel.Id,
            Name = itemModel.Name,
            Source = itemModel.Source,
            Documentation = itemModel.Documentation ?? string.Empty,
            DefaultValue = itemModel.DefaultValue ?? string.Empty
        };
    }

    protected override ArrayOfAttributeData ConvertToRootModel(ObservableCollection<AttributeDataViewModel> items)
    {
        var data = new ArrayOfAttributeData();
        foreach (var attr in items)
        {
            data.AttributeData.Add(ConvertToItemModel(attr));
        }
        return data;
    }

    protected override ObservableCollection<AttributeDataViewModel> ConvertFromRootModel(ArrayOfAttributeData rootModel)
    {
        var result = new ObservableCollection<AttributeDataViewModel>();
        foreach (var attr in rootModel.AttributeData)
        {
            result.Add(ConvertToItemViewModel(attr));
        }
        return result;
    }

    [RelayCommand]
    public void AddAttribute()
    {
        AddItem();
    }

    [RelayCommand]
    public void RemoveAttribute(AttributeDataViewModel? attribute)
    {
        if (attribute != null)
        {
            RemoveItem(attribute);
        }
    }

    [RelayCommand]
    public void DuplicateAttribute(AttributeDataViewModel? attribute)
    {
        if (attribute != null)
        {
            DuplicateItem(attribute);
        }
    }

    [RelayCommand]
    public void ValidateAll()
    {
        var isValid = true;
        foreach (var attr in Attributes)
        {
            if (!attr.IsValid)
            {
                isValid = false;
                break;
            }
        }
        
        StatusMessage = isValid ? "所有属性验证通过" : "存在验证错误";
    }

    [RelayCommand]
    private async Task LoadFile()
    {
        try
        {
            var samplePath = @"TestData\attributes.xml";
            await LoadXmlFileAsync(samplePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveFile()
    {
        await SaveXmlFileAsync();
    }
}

/// <summary>
/// 属性数据视图模型
/// </summary>
public partial class AttributeDataViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "ID不能为空")]
    [StringLength(50, ErrorMessage = "ID长度不能超过50个字符")]
    private string id = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "名称不能为空")]
    [StringLength(100, ErrorMessage = "名称长度不能超过100个字符")]
    private string name = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "来源不能为空")]
    private string source = "Character";

    [ObservableProperty]
    [StringLength(1000, ErrorMessage = "文档长度不能超过1000个字符")]
    private string documentation = string.Empty;

    [ObservableProperty]
    private string defaultValue = string.Empty;

    private readonly List<string> _validationErrors = new();

    public AttributeDataViewModel()
    {
        // 初始化验证
        ValidateAllProperties();
    }

    partial void OnIdChanged(string value)
    {
        ValidateProperty(nameof(Id));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    partial void OnNameChanged(string value)
    {
        ValidateProperty(nameof(Name));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    partial void OnSourceChanged(string value)
    {
        ValidateProperty(nameof(Source));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    partial void OnDocumentationChanged(string value)
    {
        ValidateProperty(nameof(Documentation));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    partial void OnDefaultValueChanged(string value)
    {
        ValidateProperty(nameof(DefaultValue));
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    private void ValidateProperty(string propertyName)
    {
        var validationService = new ValidationService();
        var errors = validationService.ValidateProperty(this, propertyName);
        
        // 更新验证错误
        _validationErrors.RemoveAll(e => e.StartsWith($"{propertyName}:"));
        _validationErrors.AddRange(errors.Select(e => $"{propertyName}: {e}"));
        
        OnPropertyChanged(nameof(ValidationErrors));
    }

    private void ValidateAllProperties()
    {
        var validationService = new ValidationService();
        var result = validationService.Validate(this);
        
        _validationErrors.Clear();
        foreach (var error in result.Errors)
        {
            _validationErrors.Add(error);
        }
        
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(ValidationErrors));
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Id) && 
                           !string.IsNullOrWhiteSpace(Name) && 
                           !string.IsNullOrWhiteSpace(Source) &&
                           _validationErrors.Count == 0;

    public IReadOnlyList<string> ValidationErrors => _validationErrors.AsReadOnly();

    public IEnumerable<string> SourceOptions => new[] { "Character", "WieldedWeapon", "WieldedShield", "SumEquipment" };

    /// <summary>
    /// 克隆当前对象
    /// </summary>
    public AttributeDataViewModel Clone()
    {
        return new AttributeDataViewModel
        {
            Id = this.Id,
            Name = this.Name,
            Source = this.Source,
            Documentation = this.Documentation,
            DefaultValue = this.DefaultValue
        };
    }
} 