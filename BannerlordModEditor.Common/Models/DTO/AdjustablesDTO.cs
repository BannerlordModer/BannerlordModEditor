using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class AdjustablesDTO
    {
        [XmlElement("Adjustable")]
        public List<AdjustableDTO> Adjustables { get; set; } = new List<AdjustableDTO>();
    }

    public class AdjustableDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}