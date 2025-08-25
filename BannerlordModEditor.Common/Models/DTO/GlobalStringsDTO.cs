using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("strings")]
    public class GlobalStringsDTO
    {
        [XmlElement("string")]
        public List<StringItemDTO> Strings { get; set; } = new List<StringItemDTO>();
    }

    public class StringItemDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;
    }
}