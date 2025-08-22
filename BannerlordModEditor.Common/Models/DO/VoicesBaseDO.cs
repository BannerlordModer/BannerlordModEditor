using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class VoicesBaseDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("face_animation_records")]
        public FaceAnimationRecordsDO FaceAnimationRecords { get; set; } = new FaceAnimationRecordsDO();

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeFaceAnimationRecords() => FaceAnimationRecords != null && FaceAnimationRecords.FaceAnimationRecordList.Count > 0;
    }

    public class FaceAnimationRecordsDO
    {
        [XmlElement("face_animation_record")]
        public List<FaceAnimationRecordDO> FaceAnimationRecordList { get; set; } = new List<FaceAnimationRecordDO>();

        public bool ShouldSerializeFaceAnimationRecordList() => FaceAnimationRecordList.Count > 0;
    }

    public class FaceAnimationRecordDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("animation_name")]
        public string AnimationName { get; set; } = string.Empty;

        [XmlElement("flags")]
        public AnimationFlagsDO? Flags { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeAnimationName() => !string.IsNullOrEmpty(AnimationName);
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList.Count > 0;
    }

    public class AnimationFlagsDO
    {
        [XmlElement("flag")]
        public List<AnimationFlagDO> FlagList { get; set; } = new List<AnimationFlagDO>();

        public bool ShouldSerializeFlagList() => FlagList.Count > 0;
    }

    public class AnimationFlagDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}