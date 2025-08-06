using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("CraftingPieces")]
    public class CraftingPieces
    {
        [XmlElement("CraftingPiece")]
        public List<CraftingPiece> Pieces { get; set; }

        public bool ShouldSerializePieces() => Pieces != null && Pieces.Count > 0;
    }

    public class CraftingPiece
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tier")]
        public int? Tier { get; set; }

        public bool ShouldSerializeTier() => Tier.HasValue;

        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }

        [XmlAttribute("mesh")]
        public string Mesh { get; set; }

        [XmlAttribute("culture")]
        public string Culture { get; set; }

        [XmlAttribute("length")]
        public double? Length { get; set; }
        public bool ShouldSerializeLength() => Length.HasValue;

        [XmlAttribute("weight")]
        public double? Weight { get; set; }
        public bool ShouldSerializeWeight() => Weight.HasValue;

        [XmlAttribute("is_hidden")]
        public bool? IsHidden { get; set; }
        public bool ShouldSerializeIsHidden() => IsHidden.HasValue;

        [XmlAttribute("is_default")]
        public bool? IsDefault { get; set; }
        public bool ShouldSerializeIsDefault() => IsDefault.HasValue;

        [XmlAttribute("CraftingCost")]
        public int? CraftingCost { get; set; }
        public bool ShouldSerializeCraftingCost() => CraftingCost.HasValue;

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
        [XmlAttribute("previous_piece_offset")]
        public double? PreviousPieceOffset { get; set; }
        public bool ShouldSerializePreviousPieceOffset() => PreviousPieceOffset.HasValue;
    }

    public class BladeData
    {
        [XmlAttribute("stack_amount")]
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

        [XmlAttribute("damage_factor")]
        public double? DamageFactor { get; set; }
        public bool ShouldSerializeDamageFactor() => DamageFactor.HasValue;
    }

    public class Swing
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlAttribute("damage_factor")]
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

        [XmlAttribute("count")]
        public int? Count { get; set; }
        public bool ShouldSerializeCount() => Count.HasValue;
    }
}