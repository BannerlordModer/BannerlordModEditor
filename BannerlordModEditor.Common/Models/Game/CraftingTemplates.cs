using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game
{
    [XmlRoot("CraftingTemplates")]
    public class CraftingTemplates
    {
        [XmlElement("CraftingTemplate")]
        public List<CraftingTemplate> CraftingTemplateList { get; set; } = new List<CraftingTemplate>();
    }

    public class CraftingTemplate
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("item_type")]
        public string ItemType { get; set; } = string.Empty;

        [XmlAttribute("modifier_group")]
        public string ModifierGroup { get; set; } = string.Empty;

        [XmlAttribute("item_holsters")]
        public string ItemHolsters { get; set; } = string.Empty;

        [XmlAttribute("piece_type_to_scale_holster_with")]
        public string PieceTypeToScaleHolsterWith { get; set; } = string.Empty;

        [XmlAttribute("hidden_piece_types_on_holster")]
        public string HiddenPieceTypesOnHolster { get; set; } = string.Empty;

        [XmlAttribute("default_item_holster_position_offset")]
        public string DefaultItemHolsterPositionOffset { get; set; } = string.Empty;

        [XmlArray("PieceDatas")]
        [XmlArrayItem("PieceData")]
        public List<CraftingTemplatePieceData> PieceDatas { get; set; } = new List<CraftingTemplatePieceData>();

        [XmlArray("WeaponDescriptions")]
        [XmlArrayItem("WeaponDescription")]
        public List<CraftingTemplateWeaponDescription> WeaponDescriptions { get; set; } = new List<CraftingTemplateWeaponDescription>();

        [XmlArray("StatsData")]
        [XmlArrayItem("StatData")]
        public List<StatData> StatsData { get; set; } = new List<StatData>();

        [XmlArray("UsablePieces")]
        [XmlArrayItem("UsablePiece")]
        public List<UsablePiece> UsablePieces { get; set; } = new List<UsablePiece>();
    }

    public class CraftingTemplatePieceData
    {
        [XmlAttribute("piece_type")]
        public string PieceType { get; set; } = string.Empty;

        [XmlAttribute("build_order")]
        public int BuildOrder { get; set; }
    }

    public class CraftingTemplateWeaponDescription
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;
    }

    public class StatData
    {
        [XmlAttribute("stat_type")]
        public string StatType { get; set; } = string.Empty;

        [XmlAttribute("max_value")]
        public float MaxValue { get; set; }
    }

    public class UsablePiece
    {
        [XmlAttribute("piece_id")]
        public string PieceId { get; set; } = string.Empty;
    }
} 