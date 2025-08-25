using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的属性数据模型
/// </summary>
public class MockPropertyData
{
    public string DisplayName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Watermark { get; set; } = string.Empty;
    public string Range { get; set; } = string.Empty;
}

/// <summary>
/// 模拟的枚举属性数据模型
/// </summary>
public class MockEnumPropertyData : MockPropertyData
{
    public ObservableCollection<string> Options { get; } = new();
}

/// <summary>
/// 模拟的PropertyEditorViewModel用于测试
/// </summary>
public partial class MockPropertyEditorViewModel : ObservableObject
{
    private bool _hasErrors;

    public bool HasErrors
    {
        get => _hasErrors;
        set => SetProperty(ref _hasErrors, value);
    }

    public ObservableCollection<MockPropertyData> BasicProperties { get; } = new();
    public ObservableCollection<MockPropertyData> NumericProperties { get; } = new();
    public ObservableCollection<MockPropertyData> BooleanProperties { get; } = new();
    public ObservableCollection<MockEnumPropertyData> EnumProperties { get; } = new();
    public ObservableCollection<MockPropertyData> AdvancedProperties { get; } = new();
    public ObservableCollection<string> ValidationErrors { get; } = new();

    public bool HasBasicProperties => BasicProperties.Count > 0;
    public bool HasNumericProperties => NumericProperties.Count > 0;
    public bool HasBooleanProperties => BooleanProperties.Count > 0;
    public bool HasEnumProperties => EnumProperties.Count > 0;
    public bool HasAdvancedProperties => AdvancedProperties.Count > 0;

    public MockPropertyEditorViewModel()
    {
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        // 基本属性
        BasicProperties.Add(new MockPropertyData
        {
            DisplayName = "名称",
            Value = "测试物品",
            Watermark = "输入物品名称..."
        });

        BasicProperties.Add(new MockPropertyData
        {
            DisplayName = "描述",
            Value = "这是一个测试物品",
            Watermark = "输入物品描述..."
        });

        // 数值属性
        NumericProperties.Add(new MockPropertyData
        {
            DisplayName = "重量",
            Value = "1.5",
            Watermark = "输入重量...",
            Range = "0.0 - 100.0"
        });

        NumericProperties.Add(new MockPropertyData
        {
            DisplayName = "价值",
            Value = "100",
            Watermark = "输入价值...",
            Range = "0 - 10000"
        });

        // 布尔属性
        BooleanProperties.Add(new MockPropertyData
        {
            DisplayName = "可交易",
            Value = "true"
        });

        BooleanProperties.Add(new MockPropertyData
        {
            DisplayName = "可装备",
            Value = "true"
        });

        // 枚举属性
        var itemRarity = new MockEnumPropertyData
        {
            DisplayName = "稀有度",
            Value = "普通",
            Watermark = "选择稀有度..."
        };
        foreach (var option in new[] { "普通", "稀有", "史诗", "传说" })
        {
            itemRarity.Options.Add(option);
        }
        EnumProperties.Add(itemRarity);

        var itemType = new MockEnumPropertyData
        {
            DisplayName = "类型",
            Value = "武器",
            Watermark = "选择类型..."
        };
        foreach (var option in new[] { "武器", "护甲", "消耗品", "任务物品" })
        {
            itemType.Options.Add(option);
        }
        EnumProperties.Add(itemType);

        // 高级属性
        AdvancedProperties.Add(new MockPropertyData
        {
            DisplayName = "内部ID",
            Value = "test_item_001",
            Watermark = "自动生成..."
        });

        AdvancedProperties.Add(new MockPropertyData
        {
            DisplayName = "版本",
            Value = "1.0",
            Watermark = "版本号..."
        });
    }

    [RelayCommand]
    public void ValidateProperties()
    {
        ValidationErrors.Clear();

        // 验证基本属性
        if (string.IsNullOrWhiteSpace(BasicProperties[0].Value))
        {
            ValidationErrors.Add("名称不能为空");
        }

        if (string.IsNullOrWhiteSpace(BasicProperties[1].Value))
        {
            ValidationErrors.Add("描述不能为空");
        }

        // 验证数值属性
        if (!double.TryParse(NumericProperties[0].Value, out double weight) || weight < 0 || weight > 100)
        {
            ValidationErrors.Add("重量必须在0.0到100.0之间");
        }

        if (!int.TryParse(NumericProperties[1].Value, out int value) || value < 0 || value > 10000)
        {
            ValidationErrors.Add("价值必须在0到10000之间");
        }

        HasErrors = ValidationErrors.Count > 0;
    }

    [RelayCommand]
    public void ResetToDefaults()
    {
        BasicProperties[0].Value = "默认物品";
        BasicProperties[1].Value = "这是一个默认物品";
        NumericProperties[0].Value = "1.0";
        NumericProperties[1].Value = "50";
        BooleanProperties[0].Value = "false";
        BooleanProperties[1].Value = "false";
        EnumProperties[0].Value = "普通";
        EnumProperties[1].Value = "消耗品";
        
        ValidationErrors.Clear();
        HasErrors = false;
    }

    [RelayCommand]
    public void ApplyCustomFilter()
    {
        // 模拟应用自定义过滤器
        // 在实际应用中，这里会根据用户输入的过滤条件来过滤属性
    }
}