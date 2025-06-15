using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("objects")]
    public class Objects
    {
        [XmlElement("Faction")]
        public Faction Faction { get; set; } = new Faction();

        [XmlElement("Item")]
        public ObjectItem Item { get; set; } = new ObjectItem();

        [XmlElement("NPCCharacter")]
        public NPCCharacter NPCCharacter { get; set; } = new NPCCharacter();

        [XmlElement("PlayerCharacter")]
        public PlayerCharacter PlayerCharacter { get; set; } = new PlayerCharacter();
    }

    public class Faction
    {
        // 空元素，可能用于占位
    }

    public class ObjectItem
    {
        [XmlElement("object")]
        public List<ItemObject> Object { get; set; } = new List<ItemObject>();
    }

    public class NPCCharacter
    {
        // 空元素，可能用于占位
    }

    public class PlayerCharacter
    {
        [XmlElement("object")]
        public List<PlayerCharacterObject> Object { get; set; } = new List<PlayerCharacterObject>();
    }

    public class ItemObject
    {
        [XmlAttribute("itemkind")]
        public string ItemKind { get; set; } = string.Empty;

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public ObjectAttributes Attributes { get; set; } = new ObjectAttributes();
    }

    public class PlayerCharacterObject
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public ObjectAttributes Attributes { get; set; } = new ObjectAttributes();

        [XmlElement("skills")]
        public ObjectSkills Skills { get; set; } = new ObjectSkills();
    }

    public class ObjectAttributes
    {
        [XmlElement("attribute")]
        public List<ObjectAttribute> Attribute { get; set; } = new List<ObjectAttribute>();
    }

    public class ObjectAttribute
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class ObjectSkills
    {
        [XmlElement("skill")]
        public List<ObjectSkill> Skill { get; set; } = new List<ObjectSkill>();
    }

    public class ObjectSkill
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;
    }
} 