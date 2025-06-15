using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    // parties.xml
    [XmlRoot("base")]
    public class PartiesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "party";

        [XmlElement("parties")]
        public PartiesContainer Parties { get; set; } = new PartiesContainer();
    }

    public class PartiesContainer
    {
        [XmlElement("party")]
        public List<Party> Party { get; set; } = new List<Party>();
    }

    public class Party
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("flags")]
        public string Flags { get; set; } = string.Empty;

        [XmlAttribute("party_template")]
        public string PartyTemplate { get; set; } = string.Empty;

        [XmlAttribute("position")]
        public string Position { get; set; } = string.Empty;

        [XmlAttribute("average_bearing_rot")]
        public string AverageBearingRot { get; set; } = string.Empty;

        [XmlElement("field")]
        public List<PartyField>? Field { get; set; }
    }

    public class PartyField
    {
        [XmlAttribute("field_name")]
        public string FieldName { get; set; } = string.Empty;

        [XmlAttribute("field_value")]
        public string FieldValue { get; set; } = string.Empty;
    }

    // music.xml
    [XmlRoot("base")]
    public class MusicBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "music";

        [XmlElement("musics")]
        public MusicsContainer Musics { get; set; } = new MusicsContainer();
    }

    public class MusicsContainer
    {
        [XmlElement("music")]
        public List<Music> Music { get; set; } = new List<Music>();
    }

    public class Music
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