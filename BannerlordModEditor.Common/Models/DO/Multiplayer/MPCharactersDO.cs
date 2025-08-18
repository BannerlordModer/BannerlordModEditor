using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Multiplayer
{
    /// <summary>
    /// Represents the mpcharacters.xml file structure containing multiplayer character definitions
    /// </summary>
    [XmlRoot("MPCharacters")]
    public class MPCharactersDO
    {
        /// <summary>
        /// Collection of multiplayer character definitions
        /// </summary>
        [XmlElement("NPCCharacter")]
        public List<NPCCharacterDO> NPCCharacterList { get; set; } = new List<NPCCharacterDO>();

        [XmlIgnore]
        public bool HasNPCCharacterList => NPCCharacterList != null && NPCCharacterList.Count > 0;
    }

    /// <summary>
    /// Represents a multiplayer NPC character definition
    /// </summary>
    public class NPCCharacterDO
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
        public CharacterFaceDO? Face { get; set; }

        // Skills
        [XmlElement("skills")]
        public CharacterSkillsDO? Skills { get; set; }

        // Equipment
        [XmlElement("Equipments")]
        public CharacterEquipmentsDO? Equipments { get; set; }

        // Serialization control methods
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeLevel() => !string.IsNullOrEmpty(Level);
        public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
        public bool ShouldSerializeVoice() => !string.IsNullOrEmpty(Voice);
        public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
        public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
        public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
        public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
        public bool ShouldSerializeFace() => Face != null;
        public bool ShouldSerializeSkills() => Skills != null;
        public bool ShouldSerializeEquipments() => Equipments != null;
    }

    /// <summary>
    /// Represents character face and appearance properties
    /// </summary>
    public class CharacterFaceDO
    {
        [XmlElement("BodyProperties")]
        public BodyPropertiesDO? BodyProperties { get; set; }

        [XmlElement("BodyPropertiesMax")]
        public BodyPropertiesDO? BodyPropertiesMax { get; set; }

        public bool ShouldSerializeBodyProperties() => BodyProperties != null;
        public bool ShouldSerializeBodyPropertiesMax() => BodyPropertiesMax != null;
    }

    /// <summary>
    /// Represents body properties for character appearance
    /// </summary>
    public class BodyPropertiesDO
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

        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
        public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
        public bool ShouldSerializeBuild() => !string.IsNullOrEmpty(Build);
        public bool ShouldSerializeKey() => !string.IsNullOrEmpty(Key);
    }

    /// <summary>
    /// Represents character skills
    /// </summary>
    public class CharacterSkillsDO
    {
        [XmlElement("skill")]
        public List<CharacterSkillDO> SkillList { get; set; } = new List<CharacterSkillDO>();

        [XmlIgnore]
        public bool HasSkillList => SkillList != null && SkillList.Count > 0;
    }

    /// <summary>
    /// Represents an individual character skill
    /// </summary>
    public class CharacterSkillDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    /// <summary>
    /// Represents character equipment configurations
    /// </summary>
    public class CharacterEquipmentsDO
    {
        [XmlElement("EquipmentRoster")]
        public List<EquipmentRosterDO> EquipmentRosterList { get; set; } = new List<EquipmentRosterDO>();

        [XmlIgnore]
        public bool HasEquipmentRosterList => EquipmentRosterList != null && EquipmentRosterList.Count > 0;
    }

    /// <summary>
    /// Represents an equipment roster for a character
    /// </summary>
    public class EquipmentRosterDO
    {
        [XmlElement("equipment")]
        public List<CharacterEquipmentDO> EquipmentList { get; set; } = new List<CharacterEquipmentDO>();

        [XmlIgnore]
        public bool HasEquipmentList => EquipmentList != null && EquipmentList.Count > 0;
    }

    /// <summary>
    /// Represents an individual equipment item
    /// </summary>
    public class CharacterEquipmentDO
    {
        [XmlAttribute("slot")]
        public string? Slot { get; set; }

        [XmlAttribute("id")]
        public string? Id { get; set; }

        public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
}