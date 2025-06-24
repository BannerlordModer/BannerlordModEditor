using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("strings")]
    public class NativeStrings
    {
        [XmlElement("string")]
        public List<NativeStringItem> Strings { get; set; } = new List<NativeStringItem>();
    }

    public class NativeStringItem
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