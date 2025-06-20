using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class Voices
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlArray("face_animation_records")]
        [XmlArrayItem("face_animation_record")]
        public FaceAnimationRecord[]? Records { get; set; }
    }

    public class FaceAnimationRecord
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("animation_name")]
        public string? AnimationName { get; set; }

        [XmlArray("flags")]
        [XmlArrayItem("flag")]
        public AnimationFlag[]? Flags { get; set; }
    }

    public class AnimationFlag
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
    }
} 