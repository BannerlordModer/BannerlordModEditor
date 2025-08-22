using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class MusicDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "music";

        [XmlElement("musics")]
        public MusicsContainerDTO MusicsContainer { get; set; } = new MusicsContainerDTO();
    }

    public class MusicsContainerDTO
    {
        [XmlElement("music")]
        public List<MusicTrackDTO> Music { get; set; } = new List<MusicTrackDTO>();
    }

    public class MusicTrackDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("flags")]
        public string Flags { get; set; } = string.Empty;

        [XmlAttribute("continue_flags")]
        public string ContinueFlags { get; set; } = string.Empty;
    }
}