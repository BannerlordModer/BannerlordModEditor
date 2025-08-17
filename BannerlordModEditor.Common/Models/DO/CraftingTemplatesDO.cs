using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("CraftingTemplates", Namespace = "")]
    [XmlType(Namespace = "")]
    public class CraftingTemplatesDO
    {
        [XmlElement("CraftingTemplate")]
        public List<CraftingTemplateDO> CraftingTemplateList { get; set; } = new List<CraftingTemplateDO>();

        [XmlIgnore]
        public bool HasEmptyCraftingTemplateList { get; set; } = false;

        public bool ShouldSerializeCraftingTemplateList() => HasEmptyCraftingTemplateList || 
            (CraftingTemplateList != null && CraftingTemplateList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class CraftingTemplateDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("item_type")]
        public string? ItemType { get; set; }
        
        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }
        
        [XmlAttribute("item_holsters")]
        public string? ItemHolsters { get; set; }
        
        [XmlAttribute("piece_type_to_scale_holster_with")]
        public string? PieceTypeToScaleHolsterWith { get; set; }
        
        [XmlAttribute("hidden_piece_types_on_holster")]
        public string? HiddenPieceTypesOnHolster { get; set; }
        
        [XmlAttribute("default_item_holster_position_offset")]
        public string? DefaultItemHolsterPositionOffset { get; set; }
        
        [XmlAttribute("always_show_holster_with_weapon")]
        public string? AlwaysShowHolsterWithWeapon { get; set; }
        
        [XmlAttribute("use_weapon_as_holster_mesh")]
        public string? UseWeaponAsHolsterMesh { get; set; }

        [XmlElement("PieceDatas")]
        public PieceDatasDO? PieceDatas { get; set; }

        [XmlElement("WeaponDescriptions")]
        public CraftingWeaponDescriptionsDO? WeaponDescriptions { get; set; }

        [XmlElement("StatsData")]
        public List<StatsDataDO> StatsDataList { get; set; } = new List<StatsDataDO>();

        [XmlElement("UsablePieces")]
        public UsablePiecesDO? UsablePieces { get; set; }

        // ShouldSerialize 方法用于精确控制序列化
        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeItemType() => !string.IsNullOrWhiteSpace(ItemType);
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrWhiteSpace(ModifierGroup);
        public bool ShouldSerializeItemHolsters() => !string.IsNullOrWhiteSpace(ItemHolsters);
        public bool ShouldSerializePieceTypeToScaleHolsterWith() => !string.IsNullOrWhiteSpace(PieceTypeToScaleHolsterWith);
        public bool ShouldSerializeHiddenPieceTypesOnHolster() => !string.IsNullOrWhiteSpace(HiddenPieceTypesOnHolster);
        public bool ShouldSerializeDefaultItemHolsterPositionOffset() => !string.IsNullOrWhiteSpace(DefaultItemHolsterPositionOffset);
        public bool ShouldSerializeAlwaysShowHolsterWithWeapon() => !string.IsNullOrWhiteSpace(AlwaysShowHolsterWithWeapon);
        public bool ShouldSerializeUseWeaponAsHolsterMesh() => !string.IsNullOrWhiteSpace(UseWeaponAsHolsterMesh);
        public bool ShouldSerializePieceDatas() => PieceDatas != null && PieceDatas.PieceDataList.Count > 0;
        public bool ShouldSerializeWeaponDescriptions() => WeaponDescriptions != null && WeaponDescriptions.WeaponDescriptionList.Count > 0;
        public bool ShouldSerializeStatsDataList() => StatsDataList != null && StatsDataList.Count > 0;
        public bool ShouldSerializeUsablePieces() => UsablePieces != null && UsablePieces.UsablePieceList.Count > 0;

        // 便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasItemType => !string.IsNullOrEmpty(ItemType);
        public bool HasModifierGroup => !string.IsNullOrEmpty(ModifierGroup);
        public bool HasItemHolsters => !string.IsNullOrEmpty(ItemHolsters);
        public bool HasPieceTypeToScaleHolsterWith => !string.IsNullOrEmpty(PieceTypeToScaleHolsterWith);
        public bool HasHiddenPieceTypesOnHolster => !string.IsNullOrEmpty(HiddenPieceTypesOnHolster);
        public bool HasDefaultItemHolsterPositionOffset => !string.IsNullOrEmpty(DefaultItemHolsterPositionOffset);
        public bool HasAlwaysShowHolsterWithWeapon => !string.IsNullOrEmpty(AlwaysShowHolsterWithWeapon);
        public bool HasUseWeaponAsHolsterMesh => !string.IsNullOrEmpty(UseWeaponAsHolsterMesh);
    }

    [XmlType(Namespace = "")]
    public class PieceDatasDO
    {
        [XmlElement("PieceData")]
        public List<PieceDataDO> PieceDataList { get; set; } = new List<PieceDataDO>();

        [XmlIgnore]
        public bool HasEmptyPieceDataList { get; set; } = false;

        public bool ShouldSerializePieceDataList() => HasEmptyPieceDataList || (PieceDataList != null && PieceDataList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class PieceDataDO
    {
        [XmlAttribute("piece_type")]
        public string? PieceType { get; set; }
        
        [XmlAttribute("build_order")]
        public string? BuildOrder { get; set; }

        public bool ShouldSerializePieceType() => !string.IsNullOrWhiteSpace(PieceType);
        public bool ShouldSerializeBuildOrder() => !string.IsNullOrWhiteSpace(BuildOrder);
    }

    [XmlType(Namespace = "")]
    public class CraftingWeaponDescriptionsDO
    {
        [XmlElement("WeaponDescription")]
        public List<CraftingWeaponDescriptionDO> WeaponDescriptionList { get; set; } = new List<CraftingWeaponDescriptionDO>();

        [XmlIgnore]
        public bool HasEmptyWeaponDescriptionList { get; set; } = false;

        public bool ShouldSerializeWeaponDescriptionList() => HasEmptyWeaponDescriptionList || 
            (WeaponDescriptionList != null && WeaponDescriptionList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class CraftingWeaponDescriptionDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
    }

    [XmlType(Namespace = "")]
    public class StatsDataDO
    {
        [XmlAttribute("weapon_description")]
        public string? WeaponDescription { get; set; }

        [XmlElement("StatData")]
        public List<StatDataDO> StatDataList { get; set; } = new List<StatDataDO>();

        [XmlIgnore]
        public bool HasEmptyStatDataList { get; set; } = false;

        public bool ShouldSerializeWeaponDescription() => !string.IsNullOrWhiteSpace(WeaponDescription);
        public bool ShouldSerializeStatDataList() => HasEmptyStatDataList || (StatDataList != null && StatDataList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class StatDataDO
    {
        [XmlAttribute("stat_type")]
        public string? StatType { get; set; }
        
        [XmlAttribute("max_value")]
        public string? MaxValue { get; set; }

        public bool ShouldSerializeStatType() => !string.IsNullOrWhiteSpace(StatType);
        public bool ShouldSerializeMaxValue() => !string.IsNullOrWhiteSpace(MaxValue);
    }

    [XmlType(Namespace = "")]
    public class UsablePiecesDO
    {
        [XmlElement("UsablePiece")]
        public List<UsablePieceDO> UsablePieceList { get; set; } = new List<UsablePieceDO>();

        [XmlIgnore]
        public bool HasEmptyUsablePieceList { get; set; } = false;

        public bool ShouldSerializeUsablePieceList() => HasEmptyUsablePieceList || 
            (UsablePieceList != null && UsablePieceList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class UsablePieceDO
    {
        [XmlAttribute("piece_id")]
        public string? PieceId { get; set; }
        
        [XmlAttribute("mp_piece")]
        public string? MpPiece { get; set; }

        public bool ShouldSerializePieceId() => !string.IsNullOrWhiteSpace(PieceId);
        public bool ShouldSerializeMpPiece() => !string.IsNullOrWhiteSpace(MpPiece);
    }
}