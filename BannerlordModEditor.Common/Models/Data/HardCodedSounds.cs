using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// hard_coded_sounds.xml - Hard-coded sound definitions for game events
[XmlRoot("base")]
public class HardCodedSounds
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("hard_coded_sounds")]
    public HardCodedSoundsContainer? HardCodedSoundsContainer { get; set; }
}

public class HardCodedSoundsContainer
{
    [XmlElement("hard_coded_sound")]
    public List<HardCodedSound> HardCodedSound { get; set; } = new();
}

public class HardCodedSound
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("path")]
    public string? Path { get; set; }
} 