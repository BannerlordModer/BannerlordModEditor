using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Game;

namespace BannerlordModEditor.UI.ViewModels.Editors;

public partial class ItemModifierEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<ItemModifierDataViewModel> itemModifiers = new();

    [ObservableProperty]
    private ItemModifierDataViewModel? selectedItemModifier;

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges = false;

    [ObservableProperty]
    private bool isLoading = false;

    [RelayCommand]
    private void LoadFile()
    {
        // 在实际应用中这里会打开文件对话框
        // 现在先使用测试数据路径
        var testFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..", "..", "..", "..", 
            "BannerlordModEditor.Common.Tests", 
            "TestData", 
            "item_modifiers.xml"
        );

        if (File.Exists(testFilePath))
        {
            _ = LoadXmlFileAsync(testFilePath);
        }
    }

    public async Task LoadXmlFileAsync(string xmlFilePath)
    {
        try
        {
            IsLoading = true;
            ItemModifiers.Clear();

            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers? itemModifiersData;

            using (var reader = new StreamReader(xmlFilePath))
            {
                itemModifiersData = serializer.Deserialize(reader) as ItemModifiers;
            }

            if (itemModifiersData?.ItemModifier != null)
            {
                foreach (var modifier in itemModifiersData.ItemModifier)
                {
                    ItemModifiers.Add(new ItemModifierDataViewModel(modifier));
                }
            }

            FilePath = xmlFilePath;
            HasUnsavedChanges = false;
        }
        catch (Exception ex)
        {
            // TODO: 显示错误消息给用户
            System.Diagnostics.Debug.WriteLine($"加载XML文件失败: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void LoadXmlFile(string xmlFileName)
    {
        // 同步版本，用于EditorManager调用
        _ = LoadXmlFileAsync(xmlFileName);
    }

    [RelayCommand]
    private void SaveFile()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            SaveFileAs();
            return;
        }

        try
        {
            var itemModifiersData = new ItemModifiers();
            foreach (var modifierVm in ItemModifiers)
            {
                itemModifiersData.ItemModifier.Add(modifierVm.ToModel());
            }

            var serializer = new XmlSerializer(typeof(ItemModifiers));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter(FilePath))
            using (var xmlWriter = System.Xml.XmlWriter.Create(writer, new System.Xml.XmlWriterSettings 
            { 
                Indent = true, 
                IndentChars = "  ",
                OmitXmlDeclaration = false,
                Encoding = System.Text.Encoding.UTF8
            }))
            {
                serializer.Serialize(xmlWriter, itemModifiersData, ns);
            }

            HasUnsavedChanges = false;
        }
        catch (Exception ex)
        {
            // TODO: 显示错误消息给用户
            System.Diagnostics.Debug.WriteLine($"保存XML文件失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SaveFileAs()
    {
        // TODO: 实现另存为对话框
        SaveFile();
    }

    [RelayCommand]
    private void AddItemModifier()
    {
        var newModifier = new ItemModifierDataViewModel(new ItemModifier 
        { 
            Id = $"new_modifier_{ItemModifiers.Count + 1}",
            Name = "新修饰符",
            ModifierGroup = "ItemModifierGroup.sword",
            Quality = "normal",
            PriceFactor = "1.0",
            LootDropScore = "1.0",
            ProductionDropScore = "1.0"
        });

        ItemModifiers.Add(newModifier);
        SelectedItemModifier = newModifier;
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveItemModifier(ItemModifierDataViewModel? modifier)
    {
        if (modifier != null && ItemModifiers.Contains(modifier))
        {
            ItemModifiers.Remove(modifier);
            if (SelectedItemModifier == modifier)
            {
                SelectedItemModifier = ItemModifiers.FirstOrDefault();
            }
            HasUnsavedChanges = true;
        }
    }

    [RelayCommand]
    private void SelectItemModifier(ItemModifierDataViewModel? modifier)
    {
        SelectedItemModifier = modifier;
    }
}

public partial class ItemModifierDataViewModel : ViewModelBase
{
    [ObservableProperty]
    private string id = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? modifierGroup;

    [ObservableProperty]
    private string quality = string.Empty;

    [ObservableProperty]
    private string priceFactor = string.Empty;

    [ObservableProperty]
    private string lootDropScore = string.Empty;

    [ObservableProperty]
    private string productionDropScore = string.Empty;

    // Weapon stats
    [ObservableProperty]
    private string? damage;

    [ObservableProperty]
    private string? speed;

    [ObservableProperty]
    private string? missileSpeed;

    [ObservableProperty]
    private string? stackCount;

    // Armor stats
    [ObservableProperty]
    private string? armor;

    // Horse stats
    [ObservableProperty]
    private string? horseSpeed;

    [ObservableProperty]
    private string? maneuver;

    [ObservableProperty]
    private string? chargeDamage;

    [ObservableProperty]
    private string? horseHitPoints;

    [ObservableProperty]
    private bool isValid = true;

    public ItemModifierDataViewModel() { }

    public ItemModifierDataViewModel(ItemModifier modifier)
    {
        Id = modifier.Id;
        Name = modifier.Name;
        ModifierGroup = modifier.ModifierGroup;
        Quality = modifier.Quality;
        PriceFactor = modifier.PriceFactor;
        LootDropScore = modifier.LootDropScore;
        ProductionDropScore = modifier.ProductionDropScore;
        Damage = modifier.Damage;
        Speed = modifier.Speed;
        MissileSpeed = modifier.MissileSpeed;
        StackCount = modifier.StackCount;
        Armor = modifier.Armor;
        HorseSpeed = modifier.HorseSpeed;
        Maneuver = modifier.Maneuver;
        ChargeDamage = modifier.ChargeDamage;
        HorseHitPoints = modifier.HorseHitPoints;
        
        ValidateData();
    }

    public ItemModifier ToModel()
    {
        return new ItemModifier
        {
            Id = Id,
            Name = Name,
            ModifierGroup = ModifierGroup,
            Quality = Quality,
            PriceFactor = PriceFactor,
            LootDropScore = LootDropScore,
            ProductionDropScore = ProductionDropScore,
            Damage = Damage,
            Speed = Speed,
            MissileSpeed = MissileSpeed,
            StackCount = StackCount,
            Armor = Armor,
            HorseSpeed = HorseSpeed,
            Maneuver = Maneuver,
            ChargeDamage = ChargeDamage,
            HorseHitPoints = HorseHitPoints
        };
    }

    private void ValidateData()
    {
        IsValid = !string.IsNullOrWhiteSpace(Id) && 
                  !string.IsNullOrWhiteSpace(Name) &&
                  !string.IsNullOrWhiteSpace(Quality);
    }

    partial void OnIdChanged(string value) => ValidateData();
    partial void OnNameChanged(string value) => ValidateData();
    partial void OnQualityChanged(string value) => ValidateData();
} 