using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("ArrayOfAttributeData")]
    public class AttributesDTO
    {
        [XmlElement("AttributeData")]
        public List<AttributeDataDTO> Attributes { get; set; } = new List<AttributeDataDTO>();

        public bool ShouldSerializeAttributes() => Attributes != null && Attributes.Count > 0;
    }

    public class AttributeDataDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Source")]
        public string? Source { get; set; }

        [XmlElement("Documentation")]
        public string? Documentation { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);
        public bool ShouldSerializeDocumentation() => !string.IsNullOrEmpty(Documentation);
    }
}