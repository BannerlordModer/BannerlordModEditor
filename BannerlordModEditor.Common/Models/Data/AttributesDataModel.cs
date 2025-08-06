using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("ArrayOfAttributeData")]
    public class AttributesDataModel
    {
        [XmlElement("AttributeData")]
        public List<AttributeDataModel> Attributes { get; set; }

        public bool ShouldSerializeAttributes() => Attributes != null && Attributes.Count > 0;
    }

    public class AttributeDataModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);

        [XmlAttribute("Name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("Source")]
        public string Source { get; set; }

        public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);

        [XmlElement("Documentation")]
        public string Documentation { get; set; }

        public bool ShouldSerializeDocumentation() => !string.IsNullOrEmpty(Documentation);
    }
}