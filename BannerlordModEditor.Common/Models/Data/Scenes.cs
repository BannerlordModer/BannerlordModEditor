using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ScenesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("sites")]
        public Sites Sites { get; set; } = new Sites();
    }

    public class Sites
    {
        [XmlElement("site")]
        public List<Site> SiteList { get; set; } = new List<Site>();
    }

    public class Site
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 