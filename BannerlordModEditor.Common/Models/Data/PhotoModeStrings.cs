using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("strings")]
    public class PhotoModeStrings
    {
        [XmlElement("string")]
        public List<LocalizedString> Strings { get; set; } = new List<LocalizedString>();
    }

    public class LocalizedString
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;
    }
} 