using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("ArrayOfAttributeData")]
    public class ArrayOfAttributeData
    {
        [XmlElement("AttributeData")]
        public List<AttributeData> AttributeData { get; set; } = new List<AttributeData>();
    }

    public class AttributeData : XmlModelBase
    {
        [XmlAttribute("id")]
        public NullableStringProperty Id { get; set; } = new();

        [XmlAttribute("Name")]
        public NullableStringProperty Name { get; set; } = new();

        [XmlAttribute("Source")]
        public NullableStringProperty Source { get; set; } = new();

        [XmlElement("Documentation")]
        public NullableStringProperty Documentation { get; set; } = new();
    }
}