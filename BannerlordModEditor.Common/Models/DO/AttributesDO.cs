using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("ArrayOfAttributeData")]
    public class AttributesDO
    {
        [XmlElement("AttributeData")]
        public List<AttributeDataDO> Attributes { get; set; } = new List<AttributeDataDO>();
        
        [XmlIgnore]
        public bool HasEmptyAttributes { get; set; } = false;

        public bool ShouldSerializeAttributes() => HasEmptyAttributes || (Attributes != null && Attributes.Count > 0);
    }

    public class AttributeDataDO
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