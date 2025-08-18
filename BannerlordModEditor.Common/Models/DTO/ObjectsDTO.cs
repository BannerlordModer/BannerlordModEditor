using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 对象配置的数据传输对象 - objects.xml
    /// 基于Objects模型，用于DO/DTO架构模式转换
    /// </summary>
    [XmlRoot("objects")]
    public class ObjectsDTO
    {
        [XmlElement("Faction")]
        public FactionDTO Faction { get; set; } = new FactionDTO();

        [XmlElement("Item")]
        public ObjectItemDTO Item { get; set; } = new ObjectItemDTO();

        [XmlElement("NPCCharacter")]
        public NPCCharacterDTO NPCCharacter { get; set; } = new NPCCharacterDTO();

        [XmlElement("PlayerCharacter")]
        public PlayerCharacterDTO PlayerCharacter { get; set; } = new PlayerCharacterDTO();

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

    public class FactionDTO
    {
        // 空的Faction元素
    }

    public class NPCCharacterDTO
    {
        // 空的NPCCharacter元素
    }

    public class PlayerCharacterDTO
    {
        [XmlElement("object")]
        public List<PlayerCharacterObjectDTO> Object { get; set; } = new List<PlayerCharacterObjectDTO>();
    }

    public class PlayerCharacterObjectDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public PlayerCharacterAttributesDTO Attributes { get; set; } = new PlayerCharacterAttributesDTO();

        [XmlElement("skills")]
        public PlayerCharacterSkillsDTO Skills { get; set; } = new PlayerCharacterSkillsDTO();
    }

    public class PlayerCharacterAttributesDTO
    {
        [XmlElement("attribute")]
        public List<PlayerCharacterAttributeDTO> Attribute { get; set; } = new List<PlayerCharacterAttributeDTO>();
    }

    public class PlayerCharacterAttributeDTO
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class PlayerCharacterSkillsDTO
    {
        [XmlElement("skill")]
        public List<PlayerCharacterSkillDTO> Skill { get; set; } = new List<PlayerCharacterSkillDTO>();
    }

    public class PlayerCharacterSkillDTO
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;
    }

    public class ObjectItemDTO
    {
        [XmlElement("object")]
        public List<ItemObjectDTO> Object { get; set; } = new List<ItemObjectDTO>();
    }

    public class ItemObjectDTO
    {
        [XmlAttribute("itemkind")]
        public string ItemKind { get; set; } = string.Empty;

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public ObjectAttributesDTO Attributes { get; set; } = new ObjectAttributesDTO();
    }

    public class ObjectAttributesDTO
    {
        [XmlElement("attribute")]
        public List<ObjectAttributeDTO> Attribute { get; set; } = new List<ObjectAttributeDTO>();
    }

    public class ObjectAttributeDTO
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}