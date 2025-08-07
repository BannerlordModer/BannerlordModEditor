using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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

        [XmlAttribute("tier")]
        public string TierString
        {
            get => Tier.HasValue ? Tier.Value.ToString() : null;
            set => Tier = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Tier { get; set; }
        
        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }
        
        [XmlAttribute("mesh")]
        public string Mesh { get; set; }
        
        [XmlAttribute("culture")]
        public string Culture { get; set; }
        
        [XmlAttribute("length")]
        public string LengthString
        {
            get => Length.HasValue ? Length.Value.ToString() : null;
            set => Length = string.IsNullOrEmpty(value) ? (double?)null : double.Parse(value);
        }
        [XmlIgnore]
        public double? Length { get; set; }
        [XmlAttribute("weight")]
        public string WeightString
        {
            get => Weight.HasValue ? Weight.Value.ToString() : null;
            set => Weight = string.IsNullOrEmpty(value) ? (double?)null : double.Parse(value);
        }
        [XmlIgnore]
        public double? Weight { get; set; }
        [XmlAttribute("is_hidden")]
        public string IsHiddenString
        {
            get => IsHidden.HasValue ? IsHidden.Value.ToString().ToLower() : null;
            set => IsHidden = string.IsNullOrEmpty(value) ? (bool?)null : bool.Parse(value);
        }
        [XmlIgnore]
        public bool? IsHidden { get; set; }
        [XmlAttribute("is_default")]
        public string IsDefaultString
        {
            get => IsDefault.HasValue ? IsDefault.Value.ToString().ToLower() : null;
            set => IsDefault = string.IsNullOrEmpty(value) ? (bool?)null : bool.Parse(value);
        }
        [XmlIgnore]
        public bool? IsDefault { get; set; }
        [XmlAttribute("CraftingCost")]
        public string CraftingCostString
        {
            get => CraftingCost.HasValue ? CraftingCost.Value.ToString() : null;
            set => CraftingCost = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? CraftingCost { get; set; }

        [XmlElement("BuildData")]
        public BuildData BuildData { get; set; }
        public bool ShouldSerializeBuildData() => BuildData != null;

        [XmlElement("BladeData")]
        public BladeData BladeData { get; set; }
        public bool ShouldSerializeBladeData() => BladeData != null;

        [XmlElement("Flags")]
        public Flags Flags { get; set; }
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList != null && Flags.FlagList.Count > 0;

        [XmlElement("Materials")]
        public Materials Materials { get; set; }
        public bool ShouldSerializeMaterials() => Materials != null && Materials.MaterialList != null && Materials.MaterialList.Count > 0;
    }

    public class BuildData
    {
        [XmlElement("previous_piece_offset")]
        public double? PreviousPieceOffset { get; set; }
        public bool ShouldSerializePreviousPieceOffset() => PreviousPieceOffset.HasValue;
    }

    public class BladeData
    {
        [XmlElement("stack_amount")]
        public int? StackAmount { get; set; }
        public bool ShouldSerializeStackAmount() => StackAmount.HasValue;

        [XmlAttribute("physics_material")]
        public string PhysicsMaterial { get; set; }

        [XmlAttribute("body_name")]
        public string BodyName { get; set; }

        [XmlAttribute("holster_mesh")]
        public string HolsterMesh { get; set; }
        public bool ShouldSerializeHolsterMesh() => !string.IsNullOrEmpty(HolsterMesh);

        [XmlElement("Thrust")]
        public Thrust Thrust { get; set; }
        public bool ShouldSerializeThrust() => Thrust != null;

        [XmlElement("Swing")]
        public Swing Swing { get; set; }
        public bool ShouldSerializeSwing() => Swing != null;
    }

    public class Thrust
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlElement("damage_factor")]
        public double? DamageFactor { get; set; }
        public bool ShouldSerializeDamageFactor() => DamageFactor.HasValue;
    }

    public class Swing
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlElement("damage_factor")]
        public double? DamageFactor { get; set; }
        public bool ShouldSerializeDamageFactor() => DamageFactor.HasValue;
    }

    public class Flags
    {
        [XmlElement("Flag")]
        public List<Flag> FlagList { get; set; }
    }

    public class Flag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }

    public class Materials
    {
        [XmlElement("Material")]
        public List<Material> MaterialList { get; set; }
    }

    public class Material
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("count")]
        public int? Count { get; set; }
        public bool ShouldSerializeCount() => Count.HasValue;
    }
}