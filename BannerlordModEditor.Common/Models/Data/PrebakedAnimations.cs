using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class PrebakedAnimations
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("prebaked_animations")]
        public PrebakedAnimationsList PrebakedAnimationsList { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePrebakedAnimationsList() => PrebakedAnimationsList != null;
    }

    public class PrebakedAnimationsList
    {
        [XmlElement("animation")]
        public List<Animation> Animations { get; set; }

        public bool ShouldSerializeAnimations() => Animations != null && Animations.Count > 0;
    }

    public class Animation
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("skeleton_name")]
        public string SkeletonName { get; set; }

        [XmlElement("bone")]
        public List<Bone> Bones { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkeletonName() => !string.IsNullOrEmpty(SkeletonName);
        public bool ShouldSerializeBones() => Bones != null && Bones.Count > 0;
    }

    public class Bone
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("key_list")]
        public string KeyList { get; set; }

        public bool ShouldSerializeIndex() => true;
        public bool ShouldSerializeKeyList() => !string.IsNullOrEmpty(KeyList);
    }
}