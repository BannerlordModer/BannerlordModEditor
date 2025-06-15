using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("ArrayOfAttributeData")]
    public class ArrayOfAttributeData
    {
        [XmlElement("AttributeData")]
        public List<AttributeData> AttributeData { get; set; } = new List<AttributeData>();
    }

    public class AttributeData
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("Source")]
        public string Source { get; set; } = string.Empty;

        [XmlElement("Documentation")]
        public string? Documentation { get; set; }
    }
} 