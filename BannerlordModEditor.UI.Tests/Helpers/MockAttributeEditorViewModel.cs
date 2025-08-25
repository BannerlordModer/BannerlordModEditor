using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的AttributeDataViewModel用于测试
/// </summary>
public class MockAttributeDataViewModel : ObservableValidator
{
    private string _id = string.Empty;
    private string _name = string.Empty;
    private string _source = "Character";
    private string _documentation = string.Empty;
    private string _defaultValue = string.Empty;

    [Required(ErrorMessage = "ID不能为空")]
    [StringLength(50, ErrorMessage = "ID长度不能超过50个字符")]
    public string Id
    {
        get => _id;
        set
        {
            SetProperty(ref _id, value);
            ValidateProperty(nameof(Id));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    [Required(ErrorMessage = "名称不能为空")]
    [StringLength(100, ErrorMessage = "名称长度不能超过100个字符")]
    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            ValidateProperty(nameof(Name));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    [Required(ErrorMessage = "来源不能为空")]
    public string Source
    {
        get => _source;
        set
        {
            SetProperty(ref _source, value);
            ValidateProperty(nameof(Source));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    [StringLength(1000, ErrorMessage = "文档长度不能超过1000个字符")]
    public string Documentation
    {
        get => _documentation;
        set
        {
            SetProperty(ref _documentation, value);
            ValidateProperty(nameof(Documentation));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public string DefaultValue
    {
        get => _defaultValue;
        set => SetProperty(ref _defaultValue, value);
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Id) && 
                           !string.IsNullOrWhiteSpace(Name) && 
                           !string.IsNullOrWhiteSpace(Source);

    public IEnumerable<string> SourceOptions => new[] { "Character", "WieldedWeapon", "WieldedShield", "SumEquipment" };
}

/// <summary>
/// 模拟的AttributeEditorViewModel用于测试
/// </summary>
public partial class MockAttributeEditorViewModel : ObservableObject
{
    private string _filePath = string.Empty;
    private string _statusMessage = "就绪";
    private bool _hasUnsavedChanges;

    public ObservableCollection<MockAttributeDataViewModel> Attributes { get; } = new();

    public string FilePath
    {
        get => _filePath;
        set => SetProperty(ref _filePath, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        set => SetProperty(ref _hasUnsavedChanges, value);
    }

    public MockAttributeEditorViewModel()
    {
        // 初始化示例数据
        Attributes.Add(new MockAttributeDataViewModel
        {
            Id = "Strength",
            Name = "力量",
            Source = "Character",
            Documentation = "角色的基础力量属性"
        });

        Attributes.Add(new MockAttributeDataViewModel
        {
            Id = "Agility",
            Name = "敏捷",
            Source = "Character",
            Documentation = "角色的基础敏捷属性"
        });
    }

    [RelayCommand]
    public void AddAttribute()
    {
        var newAttribute = new MockAttributeDataViewModel
        {
            Id = $"NewAttribute{Attributes.Count + 1}",
            Name = "New Attribute",
            Source = "Character",
            Documentation = "Enter documentation here..."
        };

        Attributes.Add(newAttribute);
        HasUnsavedChanges = true;
        StatusMessage = "已添加新属性";
    }

    [RelayCommand]
    public void RemoveAttribute(MockAttributeDataViewModel? attribute)
    {
        if (attribute != null)
        {
            Attributes.Remove(attribute);
            HasUnsavedChanges = true;
            StatusMessage = "已删除属性";
        }
    }

    [RelayCommand]
    public void DuplicateAttribute(MockAttributeDataViewModel? attribute)
    {
        if (attribute != null)
        {
            var duplicatedAttribute = new MockAttributeDataViewModel
            {
                Id = $"{attribute.Id}_Copy",
                Name = $"{attribute.Name} (Copy)",
                Source = attribute.Source,
                Documentation = attribute.Documentation,
                DefaultValue = attribute.DefaultValue
            };

            Attributes.Add(duplicatedAttribute);
            HasUnsavedChanges = true;
            StatusMessage = "已复制属性";
        }
    }

    [RelayCommand]
    public void ValidateAll()
    {
        var isValid = Attributes.All(a => a.IsValid);
        StatusMessage = isValid ? "所有属性验证通过" : "存在验证错误";
    }

    [RelayCommand]
    public async Task LoadFile()
    {
        StatusMessage = "正在加载文件...";
        await Task.Delay(100); // 模拟异步操作
        FilePath = "test_attributes.xml";
        StatusMessage = "文件加载完成";
    }

    [RelayCommand]
    public async Task SaveFile()
    {
        StatusMessage = "正在保存文件...";
        await Task.Delay(100); // 模拟异步操作
        HasUnsavedChanges = false;
        StatusMessage = "文件保存完成";
    }
}