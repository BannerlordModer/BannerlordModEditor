using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class PrebakedAnimationsDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("prebaked_animations")]
        public PrebakedAnimationsListDTO PrebakedAnimationsList { get; set; }
    }

    public class PrebakedAnimationsListDTO
    {
        [XmlElement("animation")]
        public List<AnimationDTO> Animations { get; set; } = new List<AnimationDTO>();
    }

    public class AnimationDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("skeleton_name")]
        public string SkeletonName { get; set; } = string.Empty;

        [XmlElement("bone")]
        public List<BoneDTO> Bones { get; set; } = new List<BoneDTO>();
    }

    public class BoneDTO
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("key_list")]
        public string KeyList { get; set; } = string.Empty;
    }
}