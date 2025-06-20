using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Misc
{
    [XmlRoot("Adjustables")]
    public class Adjustables
    {
        [XmlElement("Adjustable")]
        public List<Adjustable> AdjustableList { get; set; } = new List<Adjustable>();
    }

    public class Adjustable
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
} 