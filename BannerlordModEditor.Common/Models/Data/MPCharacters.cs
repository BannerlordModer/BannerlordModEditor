using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Represents the mpcharacters.xml file structure containing multiplayer character definitions
    /// </summary>
    [XmlRoot("MPCharacters")]
    public class MPCharacters
    {
        /// <summary>
        /// Collection of NPC character definitions
        /// </summary>
        [XmlElement("NPCCharacter")]
        public List<NPCCharacter> Characters { get; set; } = new List<NPCCharacter>();
    }

    /// <summary>
    /// Represents a multiplayer NPC character definition
    /// </summary>
    public class NPCCharacter
    {
        /// <summary>
        /// Unique identifier for the character
        /// </summary>
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Character level
        /// </summary>
        [XmlAttribute("level")]
        public string? Level { get; set; }

        /// <summary>
        /// Display name of the character (may include localization keys)
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Character age
        /// </summary>
        [XmlAttribute("age")]
        public string? Age { get; set; }

        /// <summary>
        /// Character voice type
        /// </summary>
        [XmlAttribute("voice")]
        public string? Voice { get; set; }

        /// <summary>
        /// Default group for the character (Infantry, Cavalry, etc.)
        /// </summary>
        [XmlAttribute("default_group")]
        public string? DefaultGroup { get; set; }

        /// <summary>
        /// Whether the character is a hero
        /// </summary>
        [XmlAttribute("is_hero")]
        public string? IsHero { get; set; }

        /// <summary>
        /// Character's culture
        /// </summary>
        [XmlAttribute("culture")]
        public string? Culture { get; set; }

        /// <summary>
        /// Character's occupation
        /// </summary>
        [XmlAttribute("occupation")]
        public string? Occupation { get; set; }

        /// <summary>
        /// Character's face properties and appearance
        /// </summary>
        [XmlElement("face")]
        public CharacterFace? Face { get; set; }

        /// <summary>
        /// Character's skills collection
        /// </summary>
        [XmlElement("skills")]
        public CharacterSkills? Skills { get; set; }

        /// <summary>
        /// Character's equipment sets
        /// </summary>
        [XmlElement("Equipments")]
        public CharacterEquipments? Equipments { get; set; }

        /// <summary>
        /// Character's resistances
        /// </summary>
        [XmlElement("Resistances")]
        public CharacterResistances? Resistances { get; set; }
    }

    /// <summary>
    /// Represents a character's face properties and appearance
    /// </summary>
    public class CharacterFace
    {
        /// <summary>
        /// Body properties defining physical appearance
        /// </summary>
        [XmlElement("BodyProperties")]
        public BodyProperties? BodyProperties { get; set; }

        /// <summary>
        /// Maximum body properties for randomization
        /// </summary>
        [XmlElement("BodyPropertiesMax")]
        public BodyProperties? BodyPropertiesMax { get; set; }
    }

    /// <summary>
    /// Represents body properties for character appearance
    /// </summary>
    public class BodyProperties
    {
        /// <summary>
        /// Version of the body properties format
        /// </summary>
        [XmlAttribute("version")]
        public string? Version { get; set; }

        /// <summary>
        /// Character age
        /// </summary>
        [XmlAttribute("age")]
        public string? Age { get; set; }

        /// <summary>
        /// Character weight (0.0 to 1.0)
        /// </summary>
        [XmlAttribute("weight")]
        public string? Weight { get; set; }

        /// <summary>
        /// Character build/muscle (0.0 to 1.0)
        /// </summary>
        [XmlAttribute("build")]
        public string? Build { get; set; }

        /// <summary>
        /// Encoded appearance key
        /// </summary>
        [XmlAttribute("key")]
        public string? Key { get; set; }
    }

    /// <summary>
    /// Represents a character's skills collection
    /// </summary>
    public class CharacterSkills
    {
        /// <summary>
        /// List of individual skills
        /// </summary>
        [XmlElement("skill")]
        public List<CharacterSkill> SkillList { get; set; } = new List<CharacterSkill>();
    }

    /// <summary>
    /// Represents an individual character skill
    /// </summary>
    public class CharacterSkill
    {
        /// <summary>
        /// Skill identifier (Riding, OneHanded, etc.)
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Skill value/level
        /// </summary>
        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Represents a character's equipment collection
    /// </summary>
    public class CharacterEquipments
    {
        /// <summary>
        /// List of equipment rosters (multiple equipment sets)
        /// </summary>
        [XmlElement("EquipmentRoster")]
        public List<EquipmentRoster> Rosters { get; set; } = new List<EquipmentRoster>();
    }

    /// <summary>
    /// Represents a single equipment roster (one equipment set)
    /// </summary>
    public class EquipmentRoster
    {
        /// <summary>
        /// List of equipment pieces in this roster
        /// </summary>
        [XmlElement("equipment")]
        public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();
    }

    /// <summary>
    /// Represents a single piece of equipment
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// Equipment slot (Item0, Item1, Body, Head, etc.)
        /// </summary>
        [XmlAttribute("slot")]
        public string? Slot { get; set; }

        /// <summary>
        /// Item identifier
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }
    }

    /// <summary>
    /// Represents a character's resistances
    /// </summary>
    public class CharacterResistances
    {
        /// <summary>
        /// Dismount resistance value
        /// </summary>
        [XmlAttribute("dismount")]
        public string? Dismount { get; set; }
    }
} 