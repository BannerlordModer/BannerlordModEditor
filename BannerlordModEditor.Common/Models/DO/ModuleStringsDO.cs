using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("strings")]
    public class ModuleStringsDO
    {
        [XmlElement("string")]
        public List<ModuleStringItemDO> Strings { get; set; } = new List<ModuleStringItemDO>();
    }

    public class ModuleStringItemDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
}