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

public partial class CraftingPieceEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<CraftingPieceDataViewModel> craftingPieces = new();

    [ObservableProperty]
    private CraftingPieceDataViewModel? selectedCraftingPiece;

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
            "crafting_pieces.xml"
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
            CraftingPieces.Clear();

            var serializer = new XmlSerializer(typeof(CraftingPiecesBase));
            CraftingPiecesBase? craftingPiecesData;

            using (var reader = new StreamReader(xmlFilePath))
            {
                craftingPiecesData = serializer.Deserialize(reader) as CraftingPiecesBase;
            }

            if (craftingPiecesData?.CraftingPieces?.CraftingPiece != null)
            {
                foreach (var piece in craftingPiecesData.CraftingPieces.CraftingPiece)
                {
                    CraftingPieces.Add(new CraftingPieceDataViewModel(piece));
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
            var craftingPiecesData = new CraftingPiecesBase();
            foreach (var pieceVm in CraftingPieces)
            {
                craftingPiecesData.CraftingPieces.CraftingPiece.Add(pieceVm.ToModel());
            }

            var serializer = new XmlSerializer(typeof(CraftingPiecesBase));
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
                serializer.Serialize(xmlWriter, craftingPiecesData, ns);
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
    private void AddCraftingPiece()
    {
        var newPiece = new CraftingPieceDataViewModel(new CraftingPiece 
        { 
            Id = $"new_piece_{CraftingPieces.Count + 1}",
            Name = "新制作部件",
            CraftingPieceType = "blade",
            PieceTier = "1",
            Culture = "culture_empire",
            ScaleFactor = "1.0"
        });

        CraftingPieces.Add(newPiece);
        SelectedCraftingPiece = newPiece;
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveCraftingPiece(CraftingPieceDataViewModel? piece)
    {
        if (piece != null && CraftingPieces.Contains(piece))
        {
            CraftingPieces.Remove(piece);
            if (SelectedCraftingPiece == piece)
            {
                SelectedCraftingPiece = CraftingPieces.FirstOrDefault();
            }
            HasUnsavedChanges = true;
        }
    }

    [RelayCommand]
    private void SelectCraftingPiece(CraftingPieceDataViewModel? piece)
    {
        SelectedCraftingPiece = piece;
    }
}

public partial class CraftingPieceDataViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? craftingPieceType;

    [ObservableProperty]
    private string? pieceTier;

    [ObservableProperty]
    private string? culture;

    [ObservableProperty]
    private string? isHidden;

    [ObservableProperty]
    private string? scaleFactor;

    [ObservableProperty]
    private string? mesh;

    [ObservableProperty]
    private string? physicsMaterial;

    // Piece Data properties
    [ObservableProperty]
    private string? thrustDamage;

    [ObservableProperty]
    private string? thrustDamageType;

    [ObservableProperty]
    private string? swingDamage;

    [ObservableProperty]
    private string? swingDamageType;

    [ObservableProperty]
    private string? thrustSpeed;

    [ObservableProperty]
    private string? swingSpeed;

    [ObservableProperty]
    private string? missileSpeed;

    [ObservableProperty]
    private string? accuracy;

    [ObservableProperty]
    private string? weaponLength;

    [ObservableProperty]
    private string? weaponBalance;

    [ObservableProperty]
    private string? weaponReach;

    [ObservableProperty]
    private string? armorRating;

    [ObservableProperty]
    private string? weight;

    [ObservableProperty]
    private string? hitPoints;

    [ObservableProperty]
    private string? bodyArmor;

    [ObservableProperty]
    private string? legArmor;

    [ObservableProperty]
    private string? armArmor;

    [ObservableProperty]
    private string? stackAmount;

    [ObservableProperty]
    private string? ammoLimit;

    [ObservableProperty]
    private string? ammoOffset;

    [ObservableProperty]
    private string? handling;

    // Collections for sub-entities
    [ObservableProperty]
    private ObservableCollection<CraftingMaterialViewModel> materials = new();

    [ObservableProperty]
    private ObservableCollection<CraftingModifierViewModel> modifiers = new();

    [ObservableProperty]
    private ObservableCollection<CraftingPieceFlagViewModel> flags = new();

    [ObservableProperty]
    private ObservableCollection<CraftingRequirementViewModel> requirements = new();

    [ObservableProperty]
    private bool isValid = true;

    public CraftingPieceDataViewModel() { }

    public CraftingPieceDataViewModel(CraftingPiece piece)
    {
        Id = piece.Id;
        Name = piece.Name;
        CraftingPieceType = piece.CraftingPieceType;
        PieceTier = piece.PieceTier;
        Culture = piece.Culture;
        IsHidden = piece.IsHidden;
        ScaleFactor = piece.ScaleFactor;
        Mesh = piece.Mesh;
        PhysicsMaterial = piece.PhysicsMaterial;

        // Load piece data
        if (piece.PieceData != null)
        {
            ThrustDamage = piece.PieceData.ThrustDamage;
            ThrustDamageType = piece.PieceData.ThrustDamageType;
            SwingDamage = piece.PieceData.SwingDamage;
            SwingDamageType = piece.PieceData.SwingDamageType;
            ThrustSpeed = piece.PieceData.ThrustSpeed;
            SwingSpeed = piece.PieceData.SwingSpeed;
            MissileSpeed = piece.PieceData.MissileSpeed;
            Accuracy = piece.PieceData.Accuracy;
            WeaponLength = piece.PieceData.WeaponLength;
            WeaponBalance = piece.PieceData.WeaponBalance;
            WeaponReach = piece.PieceData.WeaponReach;
            ArmorRating = piece.PieceData.ArmorRating;
            Weight = piece.PieceData.Weight;
            HitPoints = piece.PieceData.HitPoints;
            BodyArmor = piece.PieceData.BodyArmor;
            LegArmor = piece.PieceData.LegArmor;
            ArmArmor = piece.PieceData.ArmArmor;
            StackAmount = piece.PieceData.StackAmount;
            AmmoLimit = piece.PieceData.AmmoLimit;
            AmmoOffset = piece.PieceData.AmmoOffset;
            Handling = piece.PieceData.Handling;
        }

        // Load materials
        if (piece.Materials?.Material != null)
        {
            foreach (var material in piece.Materials.Material)
            {
                Materials.Add(new CraftingMaterialViewModel(material));
            }
        }

        // Load modifiers
        if (piece.Modifiers?.Modifier != null)
        {
            foreach (var modifier in piece.Modifiers.Modifier)
            {
                Modifiers.Add(new CraftingModifierViewModel(modifier));
            }
        }

        // Load flags
        if (piece.Flags?.Flag != null)
        {
            foreach (var flag in piece.Flags.Flag)
            {
                Flags.Add(new CraftingPieceFlagViewModel(flag));
            }
        }

        // Load requirements
        if (piece.Availability?.Requirement != null)
        {
            foreach (var requirement in piece.Availability.Requirement)
            {
                Requirements.Add(new CraftingRequirementViewModel(requirement));
            }
        }

        ValidateData();
    }

    public CraftingPiece ToModel()
    {
        var piece = new CraftingPiece
        {
            Id = Id,
            Name = Name,
            CraftingPieceType = CraftingPieceType,
            PieceTier = PieceTier,
            Culture = Culture,
            IsHidden = IsHidden,
            ScaleFactor = ScaleFactor,
            Mesh = Mesh,
            PhysicsMaterial = PhysicsMaterial
        };

        // Create piece data if we have any data
        if (!string.IsNullOrEmpty(ThrustDamage) || !string.IsNullOrEmpty(SwingDamage) || !string.IsNullOrEmpty(Weight))
        {
            piece.PieceData = new PieceData
            {
                ThrustDamage = ThrustDamage,
                ThrustDamageType = ThrustDamageType,
                SwingDamage = SwingDamage,
                SwingDamageType = SwingDamageType,
                ThrustSpeed = ThrustSpeed,
                SwingSpeed = SwingSpeed,
                MissileSpeed = MissileSpeed,
                Accuracy = Accuracy,
                WeaponLength = WeaponLength,
                WeaponBalance = WeaponBalance,
                WeaponReach = WeaponReach,
                ArmorRating = ArmorRating,
                Weight = Weight,
                HitPoints = HitPoints,
                BodyArmor = BodyArmor,
                LegArmor = LegArmor,
                ArmArmor = ArmArmor,
                StackAmount = StackAmount,
                AmmoLimit = AmmoLimit,
                AmmoOffset = AmmoOffset,
                Handling = Handling
            };
        }

        // Create materials if we have any
        if (Materials.Count > 0)
        {
            piece.Materials = new CraftingMaterials();
            foreach (var materialVm in Materials)
            {
                piece.Materials.Material.Add(materialVm.ToModel());
            }
        }

        // Create modifiers if we have any
        if (Modifiers.Count > 0)
        {
            piece.Modifiers = new CraftingModifiers();
            foreach (var modifierVm in Modifiers)
            {
                piece.Modifiers.Modifier.Add(modifierVm.ToModel());
            }
        }

        // Create flags if we have any
        if (Flags.Count > 0)
        {
            piece.Flags = new CraftingPieceFlags();
            foreach (var flagVm in Flags)
            {
                piece.Flags.Flag.Add(flagVm.ToModel());
            }
        }

        // Create availability if we have any requirements
        if (Requirements.Count > 0)
        {
            piece.Availability = new PieceAvailability();
            foreach (var requirementVm in Requirements)
            {
                piece.Availability.Requirement.Add(requirementVm.ToModel());
            }
        }

        return piece;
    }

    private void ValidateData()
    {
        IsValid = !string.IsNullOrWhiteSpace(Id) && 
                  !string.IsNullOrWhiteSpace(Name) &&
                  !string.IsNullOrWhiteSpace(CraftingPieceType);
    }

    partial void OnIdChanged(string? value) => ValidateData();
    partial void OnNameChanged(string? value) => ValidateData();
    partial void OnCraftingPieceTypeChanged(string? value) => ValidateData();
}

// Supporting ViewModels for sub-entities
public partial class CraftingMaterialViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private string? count;

    [ObservableProperty]
    private string? materialType;

    public CraftingMaterialViewModel() { }

    public CraftingMaterialViewModel(CraftingMaterial material)
    {
        Id = material.Id;
        Count = material.Count;
        MaterialType = material.MaterialType;
    }

    public CraftingMaterial ToModel()
    {
        return new CraftingMaterial
        {
            Id = Id,
            Count = Count,
            MaterialType = MaterialType
        };
    }
}

public partial class CraftingModifierViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? attribute;

    [ObservableProperty]
    private string? operation;

    [ObservableProperty]
    private string? value;

    [ObservableProperty]
    private string? condition;

    public CraftingModifierViewModel() { }

    public CraftingModifierViewModel(CraftingModifier modifier)
    {
        Attribute = modifier.Attribute;
        Operation = modifier.Operation;
        Value = modifier.Value;
        Condition = modifier.Condition;
    }

    public CraftingModifier ToModel()
    {
        return new CraftingModifier
        {
            Attribute = Attribute,
            Operation = Operation,
            Value = Value,
            Condition = Condition
        };
    }
}

public partial class CraftingPieceFlagViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? value;

    public CraftingPieceFlagViewModel() { }

    public CraftingPieceFlagViewModel(CraftingPieceFlag flag)
    {
        Name = flag.Name;
        Value = flag.Value;
    }

    public CraftingPieceFlag ToModel()
    {
        return new CraftingPieceFlag
        {
            Name = Name,
            Value = Value
        };
    }
}

public partial class CraftingRequirementViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? type;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private string? level;

    [ObservableProperty]
    private string? operation;

    public CraftingRequirementViewModel() { }

    public CraftingRequirementViewModel(CraftingRequirement requirement)
    {
        Type = requirement.Type;
        Id = requirement.Id;
        Level = requirement.Level;
        Operation = requirement.Operation;
    }

    public CraftingRequirement ToModel()
    {
        return new CraftingRequirement
        {
            Type = Type,
            Id = Id,
            Level = Level,
            Operation = Operation
        };
    }
} 