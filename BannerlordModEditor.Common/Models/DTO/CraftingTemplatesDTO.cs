using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("CraftingTemplates", Namespace = "")]
    [XmlType(Namespace = "")]
    public class CraftingTemplatesDTO
    {
        [XmlElement("CraftingTemplate")]
        public List<CraftingTemplateDTO> CraftingTemplateList { get; set; } = new List<CraftingTemplateDTO>();
    }

    [XmlType(Namespace = "")]
    public class CraftingTemplateDTO
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
        public PieceDatasDTO? PieceDatas { get; set; }

        [XmlElement("WeaponDescriptions")]
        public CraftingWeaponDescriptionsDTO? WeaponDescriptions { get; set; }

        [XmlElement("StatsData")]
        public List<StatsDataDTO> StatsDataList { get; set; } = new List<StatsDataDTO>();

        [XmlElement("UsablePieces")]
        public UsablePiecesDTO? UsablePieces { get; set; }
    }

    [XmlType(Namespace = "")]
    public class PieceDatasDTO
    {
        [XmlElement("PieceData")]
        public List<PieceDataDTO> PieceDataList { get; set; } = new List<PieceDataDTO>();
    }

    [XmlType(Namespace = "")]
    public class PieceDataDTO
    {
        [XmlAttribute("piece_type")]
        public string? PieceType { get; set; }
        
        [XmlAttribute("build_order")]
        public string? BuildOrder { get; set; }
    }

    [XmlType(Namespace = "")]
    public class CraftingWeaponDescriptionsDTO
    {
        [XmlElement("WeaponDescription")]
        public List<CraftingWeaponDescriptionDTO> WeaponDescriptionList { get; set; } = new List<CraftingWeaponDescriptionDTO>();
    }

    [XmlType(Namespace = "")]
    public class CraftingWeaponDescriptionDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
    }

    [XmlType(Namespace = "")]
    public class StatsDataDTO
    {
        [XmlAttribute("weapon_description")]
        public string? WeaponDescription { get; set; }

        [XmlElement("StatData")]
        public List<StatDataDTO> StatDataList { get; set; } = new List<StatDataDTO>();
    }

    [XmlType(Namespace = "")]
    public class StatDataDTO
    {
        [XmlAttribute("stat_type")]
        public string? StatType { get; set; }
        
        [XmlAttribute("max_value")]
        public string? MaxValue { get; set; }
    }

    [XmlType(Namespace = "")]
    public class UsablePiecesDTO
    {
        [XmlElement("UsablePiece")]
        public List<UsablePieceDTO> UsablePieceList { get; set; } = new List<UsablePieceDTO>();
    }

    [XmlType(Namespace = "")]
    public class UsablePieceDTO
    {
        [XmlAttribute("piece_id")]
        public string? PieceId { get; set; }
        
        [XmlAttribute("mp_piece")]
        public string? MpPiece { get; set; }
    }
}