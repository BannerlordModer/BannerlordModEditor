using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class VoicesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("face_animation_records")]
        public FaceAnimationRecords FaceAnimationRecords { get; set; } = new FaceAnimationRecords();
    }

    public class FaceAnimationRecords
    {
        [XmlElement("face_animation_record")]
        public List<FaceAnimationRecord> FaceAnimationRecordList { get; set; } = new List<FaceAnimationRecord>();
    }

    public class FaceAnimationRecord
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("animation_name")]
        public string AnimationName { get; set; } = string.Empty;

        [XmlElement("flags")]
        public AnimationFlags? Flags { get; set; }
    }

    public class AnimationFlags
    {
        [XmlElement("flag")]
        public List<AnimationFlag> FlagList { get; set; } = new List<AnimationFlag>();
    }

    public class AnimationFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 