using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("strings")]
    public class ModuleStrings
    {
        [XmlElement("string")]
        public List<ModuleStringItem> Strings { get; set; } = new List<ModuleStringItem>();
    }

    public class ModuleStringItem
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