using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Multiplayer
{
    /// <summary>
    /// Represents the mpcharacters.xml file structure containing multiplayer character definitions
    /// </summary>
    [XmlRoot("MPCharacters")]
    public class MPCharactersDTO
    {
        /// <summary>
        /// Collection of multiplayer character definitions
        /// </summary>
        [XmlElement("NPCCharacter")]
        public List<NPCCharacterDTO> NPCCharacterList { get; set; } = new List<NPCCharacterDTO>();
    }

    /// <summary>
    /// Represents a multiplayer NPC character definition
    /// </summary>
    public class NPCCharacterDTO
    {
        // Basic Identity Properties
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("level")]
        public string? Level { get; set; }

        [XmlAttribute("age")]
        public string? Age { get; set; }

        [XmlAttribute("voice")]
        public string? Voice { get; set; }

        // Character Classification
        [XmlAttribute("default_group")]
        public string? DefaultGroup { get; set; }

        [XmlAttribute("occupation")]
        public string? Occupation { get; set; }

        [XmlAttribute("culture")]
        public string? Culture { get; set; }

        [XmlAttribute("is_hero")]
        public string? IsHero { get; set; }

        // Face and Appearance
        [XmlElement("face")]
        public CharacterFaceDTO? Face { get; set; }

        // Skills
        [XmlElement("skills")]
        public CharacterSkillsDTO? Skills { get; set; }

        // Equipment
        [XmlElement("Equipments")]
        public CharacterEquipmentsDTO? Equipments { get; set; }
    }

    /// <summary>
    /// Represents character face and appearance properties
    /// </summary>
    public class CharacterFaceDTO
    {
        [XmlElement("BodyProperties")]
        public BodyPropertiesDTO? BodyProperties { get; set; }

        [XmlElement("BodyPropertiesMax")]
        public BodyPropertiesDTO? BodyPropertiesMax { get; set; }
    }

    /// <summary>
    /// Represents body properties for character appearance
    /// </summary>
    public class BodyPropertiesDTO
    {
        [XmlAttribute("version")]
        public string? Version { get; set; }

        [XmlAttribute("age")]
        public string? Age { get; set; }

        [XmlAttribute("weight")]
        public string? Weight { get; set; }

        [XmlAttribute("build")]
        public string? Build { get; set; }

        [XmlAttribute("key")]
        public string? Key { get; set; }
    }

    /// <summary>
    /// Represents character skills
    /// </summary>
    public class CharacterSkillsDTO
    {
        [XmlElement("skill")]
        public List<CharacterSkillDTO> SkillList { get; set; } = new List<CharacterSkillDTO>();
    }

    /// <summary>
    /// Represents an individual character skill
    /// </summary>
    public class CharacterSkillDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Represents character equipment configurations
    /// </summary>
    public class CharacterEquipmentsDTO
    {
        [XmlElement("EquipmentRoster")]
        public List<EquipmentRosterDTO> EquipmentRosterList { get; set; } = new List<EquipmentRosterDTO>();
    }

    /// <summary>
    /// Represents an equipment roster for a character
    /// </summary>
    public class EquipmentRosterDTO
    {
        [XmlElement("equipment")]
        public List<CharacterEquipmentDTO> EquipmentList { get; set; } = new List<CharacterEquipmentDTO>();
    }

    /// <summary>
    /// Represents an individual equipment item
    /// </summary>
    public class CharacterEquipmentDTO
    {
        [XmlAttribute("slot")]
        public string? Slot { get; set; }

        [XmlAttribute("id")]
        public string? Id { get; set; }
    }
}