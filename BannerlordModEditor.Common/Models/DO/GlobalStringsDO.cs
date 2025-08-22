using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("strings")]
    public class GlobalStringsDO
    {
        [XmlElement("string")]
        public List<StringItemDO> Strings { get; set; } = new List<StringItemDO>();

        [XmlIgnore]
        public bool HasStrings { get; set; } = false;

        public bool ShouldSerializeStrings() => HasStrings && Strings != null && Strings.Count > 0;
    }

    public class StringItemDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
}