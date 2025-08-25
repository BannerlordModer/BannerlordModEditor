using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class HardCodedSoundsRootDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("hard_coded_sounds")]
        public HardCodedSoundsDTO? HardCodedSounds { get; set; }
    }

    public class HardCodedSoundsDTO
    {
        [XmlElement("hard_coded_sound")]
        public HardCodedSoundDTO[]? HardCodedSound { get; set; }
    }

    public class HardCodedSoundDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("path")]
        public string? Path { get; set; }
    }
}