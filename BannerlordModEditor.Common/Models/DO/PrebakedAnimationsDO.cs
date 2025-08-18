using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class PrebakedAnimationsDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("prebaked_animations")]
        public PrebakedAnimationsListDO PrebakedAnimationsList { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePrebakedAnimationsList() => PrebakedAnimationsList != null;
    }

    public class PrebakedAnimationsListDO
    {
        [XmlElement("animation")]
        public List<AnimationDO> Animations { get; set; } = new List<AnimationDO>();

        public bool ShouldSerializeAnimations() => Animations != null && Animations.Count > 0;
    }

    public class AnimationDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("skeleton_name")]
        public string SkeletonName { get; set; } = string.Empty;

        [XmlElement("bone")]
        public List<BoneDO> Bones { get; set; } = new List<BoneDO>();

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkeletonName() => !string.IsNullOrEmpty(SkeletonName);
        public bool ShouldSerializeBones() => Bones != null && Bones.Count > 0;
    }

    public class BoneDO
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("key_list")]
        public string KeyList { get; set; } = string.Empty;

        public bool ShouldSerializeIndex() => true;
        public bool ShouldSerializeKeyList() => !string.IsNullOrEmpty(KeyList);
    }
}