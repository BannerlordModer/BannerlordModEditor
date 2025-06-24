using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("strings")]
    public class GlobalStrings
    {
        [XmlElement("string")]
        public List<StringItem> Strings { get; set; } = new List<StringItem>();
    }

    public class StringItem
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;

        // ShouldSerialize methods to control serialization
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
}