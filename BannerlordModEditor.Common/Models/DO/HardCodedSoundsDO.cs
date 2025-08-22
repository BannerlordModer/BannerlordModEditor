using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class HardCodedSoundsRootDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("hard_coded_sounds")]
        public HardCodedSoundsDO? HardCodedSounds { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeHardCodedSounds() => HardCodedSounds != null && HardCodedSounds.HardCodedSound?.Length > 0;
    }

    public class HardCodedSoundsDO
    {
        [XmlElement("hard_coded_sound")]
        public HardCodedSoundDO[]? HardCodedSound { get; set; }

        public bool ShouldSerializeHardCodedSound() => HardCodedSound?.Length > 0;
    }

    public class HardCodedSoundDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("path")]
        public string? Path { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializePath() => !string.IsNullOrEmpty(Path);
    }
}