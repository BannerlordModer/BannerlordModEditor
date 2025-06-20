using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// music.xml - Game music and soundtrack configuration
[XmlRoot("base")]
public class Music
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("musics")]
    public MusicsContainer? MusicsContainer { get; set; }
}

public class MusicsContainer
{
    [XmlElement("music")]
    public List<MusicTrack> Music { get; set; } = new();
}

public class MusicTrack
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("flags")]
    public string? Flags { get; set; }

    [XmlAttribute("continue_flags")]
    public string? ContinueFlags { get; set; }
} 