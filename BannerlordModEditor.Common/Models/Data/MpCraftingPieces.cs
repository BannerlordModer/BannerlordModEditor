using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("CraftingPieces")]
    public class MpCraftingPiecesModel
    {
        [XmlElement("CraftingPiece")]
        public List<MpCraftingPiece> CraftingPieceList { get; set; } = new List<MpCraftingPiece>();

        public bool ShouldSerializeCraftingPieceList() => CraftingPieceList != null && CraftingPieceList.Count > 0;
    }

    public class MpCraftingPiece
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tier")]
        public string Tier { get; set; }

        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }

        [XmlAttribute("mesh")]
        public string Mesh { get; set; }

        [XmlAttribute("distance_to_next_piece")]
        public string DistanceToNextPiece { get; set; }

        [XmlAttribute("distance_to_previous_piece")]
        public string DistanceToPreviousPiece { get; set; }

        [XmlAttribute("weight")]
        public string Weight { get; set; }

        [XmlAttribute("excluded_item_usage_features")]
        public string ExcludedItemUsageFeatures { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("full_scale")]
        public string FullScale { get; set; }

        [XmlAttribute("culture")]
        public string Culture { get; set; }

        [XmlElement("BuildData")]
        public MpBuildData BuildData { get; set; }

        [XmlElement("BladeData")]
        public MpBladeData BladeData { get; set; }

        [XmlElement("Flags")]
        public MpCraftingFlags Flags { get; set; }

        public bool ShouldSerializeDistanceToNextPiece() => !string.IsNullOrEmpty(DistanceToNextPiece);
        public bool ShouldSerializeDistanceToPreviousPiece() => !string.IsNullOrEmpty(DistanceToPreviousPiece);
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
        public bool ShouldSerializeExcludedItemUsageFeatures() => !string.IsNullOrEmpty(ExcludedItemUsageFeatures);
        public bool ShouldSerializeLength() => !string.IsNullOrEmpty(Length);
        public bool ShouldSerializeFullScale() => !string.IsNullOrEmpty(FullScale);
        public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
        public bool ShouldSerializeBuildData() => BuildData != null;
        public bool ShouldSerializeBladeData() => BladeData != null;
        public bool ShouldSerializeFlags() => Flags != null;
    }

    public class MpBuildData
    {
        [XmlAttribute("piece_offset")]
        public string PieceOffset { get; set; }

        [XmlAttribute("previous_piece_offset")]
        public string PreviousPieceOffset { get; set; }

        public bool ShouldSerializePieceOffset() => !string.IsNullOrEmpty(PieceOffset);
        public bool ShouldSerializePreviousPieceOffset() => !string.IsNullOrEmpty(PreviousPieceOffset);
    }

    public class MpBladeData
    {
        [XmlAttribute("stack_amount")]
        public string StackAmount { get; set; }

        [XmlAttribute("blade_length")]
        public string BladeLength { get; set; }

        [XmlAttribute("blade_width")]
        public string BladeWidth { get; set; }

        [XmlAttribute("physics_material")]
        public string PhysicsMaterial { get; set; }

        [XmlAttribute("body_name")]
        public string BodyName { get; set; }

        [XmlAttribute("holster_mesh")]
        public string HolsterMesh { get; set; }

        [XmlElement("Thrust")]
        public MpThrust Thrust { get; set; }

        [XmlElement("Swing")]
        public MpSwing Swing { get; set; }

        public bool ShouldSerializeStackAmount() => !string.IsNullOrEmpty(StackAmount);
        public bool ShouldSerializeBladeLength() => !string.IsNullOrEmpty(BladeLength);
        public bool ShouldSerializeBladeWidth() => !string.IsNullOrEmpty(BladeWidth);
        public bool ShouldSerializePhysicsMaterial() => !string.IsNullOrEmpty(PhysicsMaterial);
        public bool ShouldSerializeBodyName() => !string.IsNullOrEmpty(BodyName);
        public bool ShouldSerializeHolsterMesh() => !string.IsNullOrEmpty(HolsterMesh);
        public bool ShouldSerializeThrust() => Thrust != null;
        public bool ShouldSerializeSwing() => Swing != null;
    }

    public class MpThrust
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlAttribute("damage_factor")]
        public string DamageFactor { get; set; }

        public bool ShouldSerializeDamageType() => !string.IsNullOrEmpty(DamageType);
        public bool ShouldSerializeDamageFactor() => !string.IsNullOrEmpty(DamageFactor);
    }

    public class MpSwing
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlAttribute("damage_factor")]
        public string DamageFactor { get; set; }

        public bool ShouldSerializeDamageType() => !string.IsNullOrEmpty(DamageType);
        public bool ShouldSerializeDamageFactor() => !string.IsNullOrEmpty(DamageFactor);
    }

    public class MpCraftingFlags
    {
        [XmlElement("Flag")]
        public List<MpCraftingFlag> FlagList { get; set; } = new List<MpCraftingFlag>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class MpCraftingFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }
}