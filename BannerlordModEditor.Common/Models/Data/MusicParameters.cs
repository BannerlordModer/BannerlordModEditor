using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("music_parameters")]
    public class MusicParameters
    {
        [XmlElement("music_parameter")]
        public List<MusicParameter> Parameters { get; set; } = new List<MusicParameter>();
    }

    public class MusicParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
} 