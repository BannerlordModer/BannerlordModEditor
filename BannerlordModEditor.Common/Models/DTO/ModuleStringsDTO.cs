using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("strings")]
    public class ModuleStringsDTO
    {
        [XmlElement("string")]
        public List<ModuleStringItemDTO> Strings { get; set; } = new List<ModuleStringItemDTO>();
    }

    public class ModuleStringItemDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;
    }
}