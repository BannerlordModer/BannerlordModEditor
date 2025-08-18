using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 对象配置的领域对象 - objects.xml
    /// 基于Objects模型，添加DO/DTO架构模式支持
    /// </summary>
    [XmlRoot("objects")]
    public class ObjectsDO
    {
        [XmlElement("Faction")]
        public Faction Faction { get; set; } = new Faction();

        [XmlElement("Item")]
        public ObjectItem Item { get; set; } = new ObjectItem();

        [XmlElement("NPCCharacter")]
        public NPCCharacter NPCCharacter { get; set; } = new NPCCharacter();

        [XmlElement("PlayerCharacter")]
        public PlayerCharacter PlayerCharacter { get; set; } = new PlayerCharacter();

        [XmlIgnore]
        public bool HasFaction { get; set; } = false;

        [XmlIgnore]
        public bool HasItem { get; set; } = false;

        [XmlIgnore]
        public bool HasNPCCharacter { get; set; } = false;

        [XmlIgnore]
        public bool HasPlayerCharacter { get; set; } = false;

        public bool ShouldSerializeFaction() => HasFaction;
        public bool ShouldSerializeItem() => HasItem;
        public bool ShouldSerializeNPCCharacter() => HasNPCCharacter;
        public bool ShouldSerializePlayerCharacter() => HasPlayerCharacter;
    }

    public class NPCCharacter
    {
        // 空的NPCCharacter元素
    }

    public class PlayerCharacter
    {
        [XmlElement("object")]
        public List<PlayerCharacterObject> Object { get; set; } = new List<PlayerCharacterObject>();
    }

    public class PlayerCharacterObject
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public PlayerCharacterAttributes Attributes { get; set; } = new PlayerCharacterAttributes();

        [XmlElement("skills")]
        public PlayerCharacterSkills Skills { get; set; } = new PlayerCharacterSkills();
    }

    public class PlayerCharacterAttributes
    {
        [XmlElement("attribute")]
        public List<PlayerCharacterAttribute> Attribute { get; set; } = new List<PlayerCharacterAttribute>();
    }

    public class PlayerCharacterAttribute
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class PlayerCharacterSkills
    {
        [XmlElement("skill")]
        public List<PlayerCharacterSkill> Skill { get; set; } = new List<PlayerCharacterSkill>();
    }

    public class PlayerCharacterSkill
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;
    }
}