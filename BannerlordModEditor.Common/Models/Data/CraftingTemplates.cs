using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("CraftingTemplates")]
    public class CraftingTemplates
    {
        [XmlElement("CraftingTemplate")]
        public List<CraftingTemplate> CraftingTemplateList { get; set; }

        public bool ShouldSerializeCraftingTemplateList() => CraftingTemplateList != null && CraftingTemplateList.Count > 0;
    }

    public class CraftingTemplate
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("item_type")]
        public string ItemType { get; set; }

        [XmlAttribute("modifier_group")]
        public string ModifierGroup { get; set; }

        [XmlAttribute("item_holsters")]
        public string ItemHolsters { get; set; }

        [XmlAttribute("piece_type_to_scale_holster_with")]
        public string PieceTypeToScaleHolsterWith { get; set; }

        [XmlAttribute("hidden_piece_types_on_holster")]
        public string HiddenPieceTypesOnHolster { get; set; }

        [XmlAttribute("default_item_holster_position_offset")]
        public string DefaultItemHolsterPositionOffset { get; set; }

        [XmlAttribute("always_show_holster_with_weapon")]
        public string AlwaysShowHolsterWithWeapon { get; set; }

        [XmlAttribute("use_weapon_as_holster_mesh")]
        public string UseWeaponAsHolsterMesh { get; set; }

        [XmlElement("PieceDatas")]
        public PieceDatas PieceDatas { get; set; }

        [XmlElement("WeaponDescriptions")]
        public CraftingWeaponDescriptions WeaponDescriptions { get; set; }

        [XmlElement("StatsData")]
        public List<StatsData> StatsDataList { get; set; }

        [XmlElement("UsablePieces")]
        public UsablePieces UsablePieces { get; set; }

        public bool ShouldSerializePieceDatas() => PieceDatas != null;
        public bool ShouldSerializeWeaponDescriptions() => WeaponDescriptions != null;
        public bool ShouldSerializeStatsDataList() => StatsDataList != null && StatsDataList.Count > 0;
        public bool ShouldSerializeUsablePieces() => UsablePieces != null;
    }

    public class PieceDatas
    {
        [XmlElement("PieceData")]
        public List<PieceData> PieceDataList { get; set; }
        public bool ShouldSerializePieceDataList() => PieceDataList != null && PieceDataList.Count > 0;
    }

    public class PieceData
    {
        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }

        [XmlAttribute("build_order")]
        public string BuildOrder { get; set; }
    }

    public class CraftingWeaponDescriptions
    {
        [XmlElement("WeaponDescription")]
        public List<CraftingWeaponDescription> WeaponDescriptionList { get; set; }
        public bool ShouldSerializeWeaponDescriptionList() => WeaponDescriptionList != null && WeaponDescriptionList.Count > 0;
    }

    public class CraftingWeaponDescription
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class StatsData
    {
        [XmlAttribute("weapon_description")]
        public string WeaponDescription { get; set; }

        [XmlElement("StatData")]
        public List<StatData> StatDataList { get; set; }
        public bool ShouldSerializeStatDataList() => StatDataList != null && StatDataList.Count > 0;
        public bool ShouldSerializeWeaponDescription() => !string.IsNullOrEmpty(WeaponDescription);
    }

    public class StatData
    {
        [XmlAttribute("stat_type")]
        public string StatType { get; set; }

        [XmlAttribute("max_value")]
        public string MaxValue { get; set; }
    }

    public class UsablePieces
    {
        [XmlElement("UsablePiece")]
        public List<UsablePiece> UsablePieceList { get; set; }
        public bool ShouldSerializeUsablePieceList() => UsablePieceList != null && UsablePieceList.Count > 0;
    }

    public class UsablePiece
    {
        [XmlAttribute("piece_id")]
        public string PieceId { get; set; }

        [XmlAttribute("mp_piece")]
        public string MpPiece { get; set; }

        public bool ShouldSerializeMpPiece() => !string.IsNullOrEmpty(MpPiece);
    }
}