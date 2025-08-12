using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("CraftingPieces")]
    public class MpCraftingPiecesDO
    {
        [XmlElement("CraftingPiece")]
        public List<MpCraftingPieceDO> CraftingPieceList { get; set; } = new List<MpCraftingPieceDO>();

        public bool ShouldSerializeCraftingPieceList() => CraftingPieceList != null && CraftingPieceList.Count > 0;
    }

    public class MpCraftingPieceDO
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

        [XmlAttribute("scale")]
        public string Scale { get; set; }

        [XmlAttribute("is_craftable")]
        public string IsCraftable { get; set; }

        [XmlElement("BuildData")]
        public MpBuildDataDO BuildData { get; set; }

        [XmlElement("BladeData")]
        public MpBladeDataDO BladeData { get; set; }

        [XmlElement("Flags")]
        public MpCraftingFlagsDO Flags { get; set; }

        // 修复ShouldSerialize方法：只检查null而不是空字符串
        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeTier() => Tier != null;
        public bool ShouldSerializePieceType() => PieceType != null;
        public bool ShouldSerializeMesh() => Mesh != null;
        public bool ShouldSerializeDistanceToNextPiece() => DistanceToNextPiece != null;
        public bool ShouldSerializeDistanceToPreviousPiece() => DistanceToPreviousPiece != null;
        public bool ShouldSerializeWeight() => Weight != null;
        public bool ShouldSerializeExcludedItemUsageFeatures() => ExcludedItemUsageFeatures != null;
        public bool ShouldSerializeLength() => Length != null;
        public bool ShouldSerializeFullScale() => FullScale != null;
        public bool ShouldSerializeCulture() => Culture != null;
        public bool ShouldSerializeScale() => Scale != null;
        public bool ShouldSerializeIsCraftable() => IsCraftable != null;
        public bool ShouldSerializeBuildData() => BuildData != null;
        public bool ShouldSerializeBladeData() => BladeData != null;
        public bool ShouldSerializeFlags() => Flags != null;
    }

    public class MpBuildDataDO
    {
        [XmlAttribute("piece_offset")]
        public string PieceOffset { get; set; }

        [XmlAttribute("previous_piece_offset")]
        public string PreviousPieceOffset { get; set; }

        public bool ShouldSerializePieceOffset() => PieceOffset != null;
        public bool ShouldSerializePreviousPieceOffset() => PreviousPieceOffset != null;
    }

    public class MpBladeDataDO
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

        [XmlAttribute("holster_mesh_length")]
        public string HolsterMeshLength { get; set; }

        [XmlAttribute("holster_body_name")]
        public string HolsterBodyName { get; set; }

        [XmlElement("Thrust")]
        public MpThrustDO Thrust { get; set; }

        [XmlElement("Swing")]
        public MpSwingDO Swing { get; set; }

        public bool ShouldSerializeStackAmount() => StackAmount != null;
        public bool ShouldSerializeBladeLength() => BladeLength != null;
        public bool ShouldSerializeBladeWidth() => BladeWidth != null;
        public bool ShouldSerializePhysicsMaterial() => PhysicsMaterial != null;
        public bool ShouldSerializeBodyName() => BodyName != null;
        public bool ShouldSerializeHolsterMesh() => HolsterMesh != null;
        public bool ShouldSerializeHolsterMeshLength() => HolsterMeshLength != null;
        public bool ShouldSerializeHolsterBodyName() => HolsterBodyName != null;
        public bool ShouldSerializeThrust() => Thrust != null;
        public bool ShouldSerializeSwing() => Swing != null;
    }

    public class MpThrustDO
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlAttribute("damage_factor")]
        public string DamageFactor { get; set; }

        public bool ShouldSerializeDamageType() => DamageType != null;
        public bool ShouldSerializeDamageFactor() => DamageFactor != null;
    }

    public class MpSwingDO
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlAttribute("damage_factor")]
        public string DamageFactor { get; set; }

        public bool ShouldSerializeDamageType() => DamageType != null;
        public bool ShouldSerializeDamageFactor() => DamageFactor != null;
    }

    public class MpCraftingFlagsDO
    {
        [XmlElement("Flag")]
        public List<MpCraftingFlagDO> FlagList { get; set; } = new List<MpCraftingFlagDO>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class MpCraftingFlagDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeType() => Type != null;
    }
}