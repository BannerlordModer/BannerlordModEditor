using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base", Namespace = "")]
    [XmlType(Namespace = "")]
    public class CraftingPiecesDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "crafting_piece";

        [XmlElement("crafting_pieces")]
        public CraftingPiecesContainerDTO CraftingPiecesContainer { get; set; } = new CraftingPiecesContainerDTO();
    }

    public class CraftingPiecesContainerDTO
    {
        [XmlElement("crafting_piece")]
        public List<CraftingPieceDTO> Pieces { get; set; } = new List<CraftingPieceDTO>();
    }

    [XmlType(Namespace = "")]
    public class CraftingPieceDTO
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
        public CraftingPieceDataDTO? PieceData { get; set; }

        [XmlElement("materials")]
        public CpMaterialsDTO? Materials { get; set; }

        [XmlElement("modifiers")]
        public CraftingModifiersDTO? Modifiers { get; set; }

        [XmlElement("flags")]
        public FlagsDTO? Flags { get; set; }

        [XmlElement("availability")]
        public AvailabilityDTO? Availability { get; set; }
    }

    [XmlType(Namespace = "")]
    public class CraftingPieceDataDTO
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
    }

    [XmlType(Namespace = "")]
    public class CpMaterialsDTO
    {
        [XmlElement("material")]
        public List<CpMaterialDTO> MaterialList { get; set; } = new List<CpMaterialDTO>();
    }

    [XmlType(Namespace = "")]
    public class CpMaterialDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("count")]
        public string? Count { get; set; }
        
        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }
    }

    [XmlType(Namespace = "")]
    public class CraftingModifiersDTO
    {
        [XmlElement("modifier")]
        public List<CraftingModifierDTO> ModifierList { get; set; } = new List<CraftingModifierDTO>();
    }

    [XmlType(Namespace = "")]
    public class CraftingModifierDTO
    {
        [XmlAttribute("attribute")]
        public string? Attribute { get; set; }
        
        [XmlAttribute("operation")]
        public string? Operation { get; set; }
        
        [XmlAttribute("value")]
        public string? Value { get; set; }
        
        [XmlAttribute("condition")]
        public string? Condition { get; set; }
    }

    [XmlType(Namespace = "")]
    public class FlagsDTO
    {
        [XmlElement("flag")]
        public List<FlagDTO> FlagList { get; set; } = new List<FlagDTO>();
    }

    [XmlType(Namespace = "")]
    public class FlagDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    [XmlType(Namespace = "")]
    public class AvailabilityDTO
    {
        [XmlElement("requirement")]
        public List<RequirementDTO> RequirementList { get; set; } = new List<RequirementDTO>();
    }

    [XmlType(Namespace = "")]
    public class RequirementDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("level")]
        public string? Level { get; set; }
    }
}