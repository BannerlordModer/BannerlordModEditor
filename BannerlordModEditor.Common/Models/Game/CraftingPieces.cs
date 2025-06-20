using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game;

// crafting_pieces.xml - Weapon crafting piece definitions
[XmlRoot("base")]
public class CraftingPiecesBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "crafting_piece";

    [XmlElement("crafting_pieces")]
    public CraftingPiecesContainer CraftingPieces { get; set; } = new CraftingPiecesContainer();
}

public class CraftingPiecesContainer
{
    [XmlElement("crafting_piece")]
    public List<CraftingPiece> CraftingPiece { get; set; } = new List<CraftingPiece>();
}

public class CraftingPiece
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("crafting_piece_type")]
    public string? CraftingPieceType { get; set; }

    [XmlAttribute("piece_tier")]
    public string? PieceTier { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("is_hidden")]
    public string? IsHidden { get; set; }

    [XmlAttribute("scale_factor")]
    public string? ScaleFactor { get; set; }

    [XmlAttribute("mesh")]
    public string? Mesh { get; set; }

    [XmlAttribute("physics_material")]
    public string? PhysicsMaterial { get; set; }

    [XmlElement("piece_data")]
    public PieceData? PieceData { get; set; }

    [XmlElement("materials")]
    public CraftingMaterials? Materials { get; set; }

    [XmlElement("modifiers")]
    public CraftingModifiers? Modifiers { get; set; }

    [XmlElement("flags")]
    public CraftingPieceFlags? Flags { get; set; }

    [XmlElement("availability")]
    public PieceAvailability? Availability { get; set; }
}

public class PieceData
{
    [XmlAttribute("thrust_damage")]
    public string? ThrustDamage { get; set; }

    [XmlAttribute("thrust_damage_type")]
    public string? ThrustDamageType { get; set; }

    [XmlAttribute("swing_damage")]
    public string? SwingDamage { get; set; }

    [XmlAttribute("swing_damage_type")]
    public string? SwingDamageType { get; set; }

    [XmlAttribute("thrust_speed")]
    public string? ThrustSpeed { get; set; }

    [XmlAttribute("swing_speed")]
    public string? SwingSpeed { get; set; }

    [XmlAttribute("missile_speed")]
    public string? MissileSpeed { get; set; }

    [XmlAttribute("accuracy")]
    public string? Accuracy { get; set; }

    [XmlAttribute("weapon_length")]
    public string? WeaponLength { get; set; }

    [XmlAttribute("weapon_balance")]
    public string? WeaponBalance { get; set; }

    [XmlAttribute("weapon_reach")]
    public string? WeaponReach { get; set; }

    [XmlAttribute("armor_rating")]
    public string? ArmorRating { get; set; }

    [XmlAttribute("weight")]
    public string? Weight { get; set; }

    [XmlAttribute("hit_points")]
    public string? HitPoints { get; set; }

    [XmlAttribute("body_armor")]
    public string? BodyArmor { get; set; }

    [XmlAttribute("leg_armor")]
    public string? LegArmor { get; set; }

    [XmlAttribute("arm_armor")]
    public string? ArmArmor { get; set; }

    [XmlAttribute("stack_amount")]
    public string? StackAmount { get; set; }

    [XmlAttribute("ammo_limit")]
    public string? AmmoLimit { get; set; }

    [XmlAttribute("ammo_offset")]
    public string? AmmoOffset { get; set; }

    [XmlAttribute("handling")]
    public string? Handling { get; set; }
}

public class CraftingMaterials
{
    [XmlElement("material")]
    public List<CraftingMaterial> Material { get; set; } = new List<CraftingMaterial>();
}

public class CraftingMaterial
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("count")]
    public string? Count { get; set; }

    [XmlAttribute("material_type")]
    public string? MaterialType { get; set; }
}

public class CraftingModifiers
{
    [XmlElement("modifier")]
    public List<CraftingModifier> Modifier { get; set; } = new List<CraftingModifier>();
}

public class CraftingModifier
{
    [XmlAttribute("attribute")]
    public string? Attribute { get; set; }

    [XmlAttribute("operation")]
    public string? Operation { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlAttribute("condition")]
    public string? Condition { get; set; }
}

public class CraftingPieceFlags
{
    [XmlElement("flag")]
    public List<CraftingPieceFlag> Flag { get; set; } = new List<CraftingPieceFlag>();
}

public class CraftingPieceFlag
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
}

public class PieceAvailability
{
    [XmlElement("requirement")]
    public List<CraftingRequirement> Requirement { get; set; } = new List<CraftingRequirement>();
}

public class CraftingRequirement
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("level")]
    public string? Level { get; set; }

    [XmlAttribute("operation")]
    public string? Operation { get; set; }
} 