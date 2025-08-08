using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class CraftingPieces
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "crafting_piece";

        [XmlElement("crafting_pieces")]
        public CraftingPiecesContainer CraftingPiecesContainer { get; set; } = new CraftingPiecesContainer();
    }

    public class CraftingPiecesContainer
    {
        [XmlElement("crafting_piece")]
        public List<CraftingPiece> Pieces { get; set; } = new List<CraftingPiece>();

        public bool ShouldSerializePieces() => Pieces != null && Pieces.Count > 0;
    }

    public class CraftingPiece
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }

        [XmlAttribute("tier")]
        public string Tier { get; set; }

        [XmlAttribute("piece_tier")]
        public string PieceTier { get; set; }
        public bool ShouldSerializePieceTier() => !string.IsNullOrEmpty(PieceTier);

        [XmlAttribute("culture")]
        public string Culture { get; set; }

        [XmlAttribute("mesh")]
        public string Mesh { get; set; }

        [XmlAttribute("physics_material")]
        public string PhysicsMaterial { get; set; }

        [XmlAttribute("is_hidden")]
        public string IsHidden { get; set; }

        [XmlAttribute("scale_factor")]
        public string ScaleFactor { get; set; }
        public bool ShouldSerializeScaleFactor() => !string.IsNullOrEmpty(ScaleFactor);

        [XmlAttribute("crafting_piece_type")]
        public string CraftingPieceType { get; set; }
        public bool ShouldSerializeCraftingPieceType() => !string.IsNullOrEmpty(CraftingPieceType);

        [XmlElement("piece_data")]
        public CraftingPieceData PieceData { get; set; }
        public bool ShouldSerializePieceData() => PieceData != null;

        [XmlElement("materials")]
        public Materials Materials { get; set; }
        public bool ShouldSerializeMaterials() => Materials != null && Materials.MaterialList != null && Materials.MaterialList.Count > 0;

        [XmlElement("modifiers")]
        public CraftingModifiers Modifiers { get; set; }
        public bool ShouldSerializeModifiers() => Modifiers != null && Modifiers.ModifierList != null && Modifiers.ModifierList.Count > 0;

        [XmlElement("flags")]
        public Flags Flags { get; set; }
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList != null && Flags.FlagList.Count > 0;

        [XmlElement("availability")]
        public Availability Availability { get; set; }
        public bool ShouldSerializeAvailability() => Availability != null && Availability.RequirementList != null && Availability.RequirementList.Count > 0;
    }

    public class CraftingPieceData
    {
        [XmlAttribute("thrust_damage")]
        public string ThrustDamage { get; set; }
        public bool ShouldSerializeThrustDamage() => !string.IsNullOrEmpty(ThrustDamage);

        [XmlAttribute("thrust_damage_type")]
        public string ThrustDamageType { get; set; }
        public bool ShouldSerializeThrustDamageType() => !string.IsNullOrEmpty(ThrustDamageType);

        [XmlAttribute("swing_damage")]
        public string SwingDamage { get; set; }
        public bool ShouldSerializeSwingDamage() => !string.IsNullOrEmpty(SwingDamage);

        [XmlAttribute("swing_damage_type")]
        public string SwingDamageType { get; set; }
        public bool ShouldSerializeSwingDamageType() => !string.IsNullOrEmpty(SwingDamageType);

        [XmlAttribute("thrust_speed")]
        public string ThrustSpeed { get; set; }
        public bool ShouldSerializeThrustSpeed() => !string.IsNullOrEmpty(ThrustSpeed);

        [XmlAttribute("swing_speed")]
        public string SwingSpeed { get; set; }
        public bool ShouldSerializeSwingSpeed() => !string.IsNullOrEmpty(SwingSpeed);

        [XmlAttribute("weapon_length")]
        public string WeaponLength { get; set; }
        public bool ShouldSerializeWeaponLength() => !string.IsNullOrEmpty(WeaponLength);

        [XmlAttribute("weapon_balance")]
        public string WeaponBalance { get; set; }
        public bool ShouldSerializeWeaponBalance() => !string.IsNullOrEmpty(WeaponBalance);

        [XmlAttribute("weight")]
        public string Weight { get; set; }
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);

        [XmlAttribute("hit_points")]
        public string HitPoints { get; set; }
        public bool ShouldSerializeHitPoints() => !string.IsNullOrEmpty(HitPoints);

        [XmlAttribute("handling")]
        public string Handling { get; set; }
        public bool ShouldSerializeHandling() => !string.IsNullOrEmpty(Handling);

        [XmlAttribute("missile_speed")]
        public string MissileSpeed { get; set; }
        public bool ShouldSerializeMissileSpeed() => !string.IsNullOrEmpty(MissileSpeed);

        [XmlAttribute("accuracy")]
        public string Accuracy { get; set; }
        public bool ShouldSerializeAccuracy() => !string.IsNullOrEmpty(Accuracy);

        [XmlAttribute("body_armor")]
        public string BodyArmor { get; set; }
        public bool ShouldSerializeBodyArmor() => !string.IsNullOrEmpty(BodyArmor);
    }

    public class Materials
    {
        [XmlElement("material")]
        public List<Material> MaterialList { get; set; } = new List<Material>();

        public bool ShouldSerializeMaterialList() => MaterialList != null && MaterialList.Count > 0;
    }

    public class Material
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("count")]
        public string Count { get; set; }
        public bool ShouldSerializeCount() => !string.IsNullOrEmpty(Count);

        [XmlAttribute("material_type")]
        public string MaterialType { get; set; }
        public bool ShouldSerializeMaterialType() => !string.IsNullOrEmpty(MaterialType);
    }

    public class CraftingModifiers
    {
        [XmlElement("modifier")]
        public List<CraftingModifier> ModifierList { get; set; } = new List<CraftingModifier>();

        public bool ShouldSerializeModifierList() => ModifierList != null && ModifierList.Count > 0;
    }

    public class CraftingModifier
    {
        [XmlAttribute("attribute")]
        public string Attribute { get; set; }

        [XmlAttribute("operation")]
        public string Operation { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("condition")]
        public string Condition { get; set; }
        public bool ShouldSerializeCondition() => !string.IsNullOrEmpty(Condition);
    }

    public class Flags
    {
        [XmlElement("flag")]
        public List<Flag> FlagList { get; set; } = new List<Flag>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class Flag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class Availability
    {
        [XmlElement("requirement")]
        public List<Requirement> RequirementList { get; set; } = new List<Requirement>();

        public bool ShouldSerializeRequirementList() => RequirementList != null && RequirementList.Count > 0;
    }

    public class Requirement
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }
    }
}