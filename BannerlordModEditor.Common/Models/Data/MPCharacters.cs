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
        public NPCCharacter[]? Characters { get; set; }
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
        public int? Level { get; set; }
        public bool ShouldSerializeLevel() => Level.HasValue;

        /// <summary>
        /// Display name of the character (may include localization keys)
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Character age
        /// </summary>
        [XmlAttribute("age")]
        public int? Age { get; set; }
        public bool ShouldSerializeAge() => Age.HasValue;

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
        public bool? IsHero { get; set; }
        public bool ShouldSerializeIsHero() => IsHero.HasValue;

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

        [XmlArray("skills")]
        [XmlArrayItem("skill")]
        public CharacterSkill[]? Skills { get; set; }

        [XmlArray("Equipments")]
        [XmlArrayItem("EquipmentRoster")]
        public EquipmentRoster[]? Equipments { get; set; }

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
        public int? Version { get; set; }
        public bool ShouldSerializeVersion() => Version.HasValue;

        /// <summary>
        /// Character age
        /// </summary>
        [XmlAttribute("age")]
        public double? Age { get; set; }
        public bool ShouldSerializeAge() => Age.HasValue;

        /// <summary>
        /// Character weight (0.0 to 1.0)
        /// </summary>
        [XmlAttribute("weight")]
        public double? Weight { get; set; }
        public bool ShouldSerializeWeight() => Weight.HasValue;

        /// <summary>
        /// Character build/muscle (0.0 to 1.0)
        /// </summary>
        [XmlAttribute("build")]
        public double? Build { get; set; }
        public bool ShouldSerializeBuild() => Build.HasValue;

        /// <summary>
        /// Encoded appearance key
        /// </summary>
        [XmlAttribute("key")]
        public string? Key { get; set; }
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
        public int? Value { get; set; }
        public bool ShouldSerializeValue() => Value.HasValue;
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
        public Equipment[]? EquipmentList { get; set; }
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
        public int? Dismount { get; set; }
        public bool ShouldSerializeDismount() => Dismount.HasValue;
    }
} 