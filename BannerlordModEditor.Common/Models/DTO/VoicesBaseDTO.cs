using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class VoicesBaseDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("face_animation_records")]
        public FaceAnimationRecordsDTO FaceAnimationRecords { get; set; } = new FaceAnimationRecordsDTO();
    }

    public class FaceAnimationRecordsDTO
    {
        [XmlElement("face_animation_record")]
        public List<FaceAnimationRecordDTO> FaceAnimationRecordList { get; set; } = new List<FaceAnimationRecordDTO>();
    }

    public class FaceAnimationRecordDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("animation_name")]
        public string AnimationName { get; set; } = string.Empty;

        [XmlElement("flags")]
        public AnimationFlagsDTO? Flags { get; set; }
    }

    public class AnimationFlagsDTO
    {
        [XmlElement("flag")]
        public List<AnimationFlagDTO> FlagList { get; set; } = new List<AnimationFlagDTO>();
    }

    public class AnimationFlagDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
}