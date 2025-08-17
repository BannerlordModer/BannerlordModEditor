using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base", Namespace = "")]
    [XmlType(Namespace = "")]
    public class CraftingPiecesDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "crafting_piece";

        [XmlElement("crafting_pieces")]
        public CraftingPiecesContainerDO CraftingPiecesContainer { get; set; } = new CraftingPiecesContainerDO();

        [XmlIgnore]
        public bool HasEmptyCraftingPiecesContainer { get; set; } = false;

        public bool ShouldSerializeCraftingPiecesContainer() => HasEmptyCraftingPiecesContainer || 
            (CraftingPiecesContainer != null && CraftingPiecesContainer.Pieces.Count > 0);
    }

    public class CraftingPiecesContainerDO
    {
        [XmlElement("crafting_piece")]
        public List<CraftingPieceDO> Pieces { get; set; } = new List<CraftingPieceDO>();

        [XmlIgnore]
        public bool HasEmptyPieces { get; set; } = false;

        public bool ShouldSerializePieces() => HasEmptyPieces || (Pieces != null && Pieces.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class CraftingPieceDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        [XmlAttribute("piece_type")]
        public string? PieceType { get; set; }
        
        [XmlAttribute("tier")]
        public string? Tier { get; set; }
        
        [XmlAttribute("piece_tier")]
        public string? PieceTier { get; set; }
        
        [XmlAttribute("culture")]
        public string? Culture { get; set; }
        
        [XmlAttribute("mesh")]
        public string? Mesh { get; set; }
        
        [XmlAttribute("physics_material")]
        public string? PhysicsMaterial { get; set; }
        
        [XmlAttribute("is_hidden")]
        public string? IsHidden { get; set; }
        
        [XmlAttribute("scale_factor")]
        public string? ScaleFactor { get; set; }
        
        [XmlAttribute("crafting_piece_type")]
        public string? CraftingPieceType { get; set; }

        [XmlElement("piece_data")]
        public CraftingPieceDataDO? PieceData { get; set; }

        [XmlElement("materials")]
        public CpMaterialsDO? Materials { get; set; }

        [XmlElement("modifiers")]
        public CraftingModifiersDO? Modifiers { get; set; }

        [XmlElement("flags")]
        public FlagsDO? Flags { get; set; }

        [XmlElement("availability")]
        public AvailabilityDO? Availability { get; set; }

        // ShouldSerialize 方法用于精确控制序列化
        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializePieceType() => !string.IsNullOrWhiteSpace(PieceType);
        public bool ShouldSerializeTier() => !string.IsNullOrWhiteSpace(Tier);
        public bool ShouldSerializePieceTier() => !string.IsNullOrWhiteSpace(PieceTier);
        public bool ShouldSerializeCulture() => !string.IsNullOrWhiteSpace(Culture);
        public bool ShouldSerializeMesh() => !string.IsNullOrWhiteSpace(Mesh);
        public bool ShouldSerializePhysicsMaterial() => !string.IsNullOrWhiteSpace(PhysicsMaterial);
        public bool ShouldSerializeIsHidden() => !string.IsNullOrWhiteSpace(IsHidden);
        public bool ShouldSerializeScaleFactor() => !string.IsNullOrWhiteSpace(ScaleFactor);
        public bool ShouldSerializeCraftingPieceType() => !string.IsNullOrWhiteSpace(CraftingPieceType);
        public bool ShouldSerializePieceData() => PieceData != null;
        public bool ShouldSerializeMaterials() => Materials != null && Materials.MaterialList.Count > 0;
        public bool ShouldSerializeModifiers() => Modifiers != null && Modifiers.ModifierList.Count > 0;
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList.Count > 0;
        public bool ShouldSerializeAvailability() => Availability != null && Availability.RequirementList.Count > 0;

        // 便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasName => !string.IsNullOrEmpty(Name);
        public bool HasPieceType => !string.IsNullOrEmpty(PieceType);
        public bool HasTier => !string.IsNullOrEmpty(Tier);
        public bool HasPieceTier => !string.IsNullOrEmpty(PieceTier);
        public bool HasCulture => !string.IsNullOrEmpty(Culture);
        public bool HasMesh => !string.IsNullOrEmpty(Mesh);
        public bool HasPhysicsMaterial => !string.IsNullOrEmpty(PhysicsMaterial);
        public bool HasIsHidden => !string.IsNullOrEmpty(IsHidden);
        public bool HasScaleFactor => !string.IsNullOrEmpty(ScaleFactor);
        public bool HasCraftingPieceType => !string.IsNullOrEmpty(CraftingPieceType);
    }

    [XmlType(Namespace = "")]
    public class CraftingPieceDataDO
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
        
        [XmlAttribute("weapon_length")]
        public string? WeaponLength { get; set; }
        
        [XmlAttribute("weapon_balance")]
        public string? WeaponBalance { get; set; }
        
        [XmlAttribute("weight")]
        public string? Weight { get; set; }
        
        [XmlAttribute("hit_points")]
        public string? HitPoints { get; set; }
        
        [XmlAttribute("handling")]
        public string? Handling { get; set; }
        
        [XmlAttribute("missile_speed")]
        public string? MissileSpeed { get; set; }
        
        [XmlAttribute("accuracy")]
        public string? Accuracy { get; set; }
        
        [XmlAttribute("body_armor")]
        public string? BodyArmor { get; set; }

        // ShouldSerialize 方法
        public bool ShouldSerializeThrustDamage() => !string.IsNullOrWhiteSpace(ThrustDamage);
        public bool ShouldSerializeThrustDamageType() => !string.IsNullOrWhiteSpace(ThrustDamageType);
        public bool ShouldSerializeSwingDamage() => !string.IsNullOrWhiteSpace(SwingDamage);
        public bool ShouldSerializeSwingDamageType() => !string.IsNullOrWhiteSpace(SwingDamageType);
        public bool ShouldSerializeThrustSpeed() => !string.IsNullOrWhiteSpace(ThrustSpeed);
        public bool ShouldSerializeSwingSpeed() => !string.IsNullOrWhiteSpace(SwingSpeed);
        public bool ShouldSerializeWeaponLength() => !string.IsNullOrWhiteSpace(WeaponLength);
        public bool ShouldSerializeWeaponBalance() => !string.IsNullOrWhiteSpace(WeaponBalance);
        public bool ShouldSerializeWeight() => !string.IsNullOrWhiteSpace(Weight);
        public bool ShouldSerializeHitPoints() => !string.IsNullOrWhiteSpace(HitPoints);
        public bool ShouldSerializeHandling() => !string.IsNullOrWhiteSpace(Handling);
        public bool ShouldSerializeMissileSpeed() => !string.IsNullOrWhiteSpace(MissileSpeed);
        public bool ShouldSerializeAccuracy() => !string.IsNullOrWhiteSpace(Accuracy);
        public bool ShouldSerializeBodyArmor() => !string.IsNullOrWhiteSpace(BodyArmor);
    }

    [XmlType(Namespace = "")]
    public class CpMaterialsDO
    {
        [XmlElement("material")]
        public List<CpMaterialDO> MaterialList { get; set; } = new List<CpMaterialDO>();

        [XmlIgnore]
        public bool HasEmptyMaterialList { get; set; } = false;

        public bool ShouldSerializeMaterialList() => HasEmptyMaterialList || (MaterialList != null && MaterialList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class CpMaterialDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("count")]
        public string? Count { get; set; }
        
        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeCount() => !string.IsNullOrWhiteSpace(Count);
        public bool ShouldSerializeMaterialType() => !string.IsNullOrWhiteSpace(MaterialType);
    }

    [XmlType(Namespace = "")]
    public class CraftingModifiersDO
    {
        [XmlElement("modifier")]
        public List<CraftingModifierDO> ModifierList { get; set; } = new List<CraftingModifierDO>();

        [XmlIgnore]
        public bool HasEmptyModifierList { get; set; } = false;

        public bool ShouldSerializeModifierList() => HasEmptyModifierList || (ModifierList != null && ModifierList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class CraftingModifierDO
    {
        [XmlAttribute("attribute")]
        public string? Attribute { get; set; }
        
        [XmlAttribute("operation")]
        public string? Operation { get; set; }
        
        [XmlAttribute("value")]
        public string? Value { get; set; }
        
        [XmlAttribute("condition")]
        public string? Condition { get; set; }

        public bool ShouldSerializeAttribute() => !string.IsNullOrWhiteSpace(Attribute);
        public bool ShouldSerializeOperation() => !string.IsNullOrWhiteSpace(Operation);
        public bool ShouldSerializeValue() => !string.IsNullOrWhiteSpace(Value);
        public bool ShouldSerializeCondition() => !string.IsNullOrWhiteSpace(Condition);
    }

    [XmlType(Namespace = "")]
    public class FlagsDO
    {
        [XmlElement("flag")]
        public List<FlagDO> FlagList { get; set; } = new List<FlagDO>();

        [XmlIgnore]
        public bool HasEmptyFlagList { get; set; } = false;

        public bool ShouldSerializeFlagList() => HasEmptyFlagList || (FlagList != null && FlagList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class FlagDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrWhiteSpace(Value);
    }

    [XmlType(Namespace = "")]
    public class AvailabilityDO
    {
        [XmlElement("requirement")]
        public List<RequirementDO> RequirementList { get; set; } = new List<RequirementDO>();

        [XmlIgnore]
        public bool HasEmptyRequirementList { get; set; } = false;

        public bool ShouldSerializeRequirementList() => HasEmptyRequirementList || (RequirementList != null && RequirementList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class RequirementDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("level")]
        public string? Level { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrWhiteSpace(Type);
        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeLevel() => !string.IsNullOrWhiteSpace(Level);
    }
}