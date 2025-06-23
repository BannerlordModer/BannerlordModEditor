using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// hard_coded_sounds.xml - Hard-coded sound definitions for game events
[XmlRoot("base")]
public class HardCodedSoundsRoot
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("hard_coded_sounds")]
    public HardCodedSounds? HardCodedSounds { get; set; }
}

public class HardCodedSounds
{
    [XmlElement("hard_coded_sound")]
    public HardCodedSound[]? HardCodedSound { get; set; }
}

public class HardCodedSound
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("path")]
    public string? Path { get; set; }
} 