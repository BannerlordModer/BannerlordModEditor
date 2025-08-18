using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("objects")]
    public class Objects
    {
        [XmlElement("Faction")]
        public Faction Faction { get; set; } = new Faction();

        [XmlElement("Item")]
        public ObjectItem Item { get; set; } = new ObjectItem();
    }

    public class Faction
    {
        // 空的Faction元素
    }

    public class ObjectItem
    {
        [XmlElement("object")]
        public List<ObjectData> ObjectList { get; set; } = new List<ObjectData>();
    }

    public class ObjectData
    {
        [XmlAttribute("itemkind")]
        public string ItemKind { get; set; } = string.Empty;

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("attributes")]
        public Attributes Attributes { get; set; } = new Attributes();
    }

    public class Attributes
    {
        [XmlElement("attribute")]
        public List<Attribute> AttributeList { get; set; } = new List<Attribute>();
    }

    public class Attribute
    {
        [XmlAttribute("code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}