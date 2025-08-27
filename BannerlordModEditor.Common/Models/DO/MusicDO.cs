using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class MusicDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "music";

        [XmlElement("musics")]
        public MusicsContainerDO MusicsContainer { get; set; } = new MusicsContainerDO();

        [XmlIgnore]
        public bool HasMusicsContainer { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeMusicsContainer() => HasMusicsContainer && MusicsContainer != null && MusicsContainer.Music.Count > 0;
    }

    public class MusicsContainerDO
    {
        [XmlElement("music")]
        public List<MusicTrackDO> Music { get; set; } = new List<MusicTrackDO>();
    }

    public class MusicTrackDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("flags")]
        public string Flags { get; set; } = string.Empty;

        [XmlAttribute("continue_flags")]
        public string ContinueFlags { get; set; } = string.Empty;

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeFlags() => !string.IsNullOrEmpty(Flags);
        public bool ShouldSerializeContinueFlags() => !string.IsNullOrEmpty(ContinueFlags);
    }
}