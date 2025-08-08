using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("ArrayOfAttributeData")]
    public class AttributesDataModel
    {
        [XmlElement("AttributeData")]
        public List<AttributeDataModel> Attributes { get; set; } = new List<AttributeDataModel>();

        public bool ShouldSerializeAttributes() => Attributes != null && Attributes.Count > 0;
    }

    public class AttributeDataModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);

        [XmlAttribute("Name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);

        [XmlAttribute("Source")]
        public string Source { get; set; }

        public bool ShouldSerializeSource() => !string.IsNullOrWhiteSpace(Source);

        [XmlElement("Documentation")]
        public string Documentation { get; set; }

        public bool ShouldSerializeDocumentation() => !string.IsNullOrWhiteSpace(Documentation);
    }
}