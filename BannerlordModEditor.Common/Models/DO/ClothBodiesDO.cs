using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("bodies")]
    public class ClothBodiesDO
    {
        [XmlElement("body")]
        public List<ClothBodyDO> Bodies { get; set; } = new List<ClothBodyDO>();

        public bool ShouldSerializeBodies() => Bodies != null && Bodies.Count > 0;
    }

    public class ClothBodyDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("owner_skeleton")]
        public string OwnerSkeleton { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeOwnerSkeleton() => !string.IsNullOrEmpty(OwnerSkeleton);

        [XmlElement("capsules")]
        public ClothCapsulesDO Capsules { get; set; }

        public bool ShouldSerializeCapsules() => Capsules != null;
    }

    public class ClothCapsulesDO
    {
        [XmlElement("capsule")]
        public List<ClothCapsuleDO> CapsuleList { get; set; } = new List<ClothCapsuleDO>();

        public bool ShouldSerializeCapsuleList() => CapsuleList != null && CapsuleList.Count > 0;
    }

    public class ClothCapsuleDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("length")]
        public string Length { get; set; } = string.Empty;

        [XmlAttribute("origin")]
        public string Origin { get; set; } = string.Empty;

        [XmlAttribute("frame")]
        public string Frame { get; set; } = string.Empty;

        [XmlElement("points")]
        public ClothPointsDO Points { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeLength() => !string.IsNullOrEmpty(Length);
        public bool ShouldSerializeOrigin() => !string.IsNullOrEmpty(Origin);
        public bool ShouldSerializeFrame() => !string.IsNullOrEmpty(Frame);
        public bool ShouldSerializePoints() => Points != null;
    }

    public class ClothPointsDO
    {
        [XmlElement("point")]
        public List<ClothPointDO> PointList { get; set; } = new List<ClothPointDO>();

        public bool ShouldSerializePointList() => PointList != null && PointList.Count > 0;
    }

    public class ClothPointDO
    {
        [XmlAttribute("radius")]
        public string Radius { get; set; } = string.Empty;

        [XmlElement("bones")]
        public ClothBonesDO Bones { get; set; }

        public bool ShouldSerializeRadius() => !string.IsNullOrEmpty(Radius);
        public bool ShouldSerializeBones() => Bones != null;
    }

    public class ClothBonesDO
    {
        [XmlElement("bone")]
        public List<ClothBoneDO> BoneList { get; set; } = new List<ClothBoneDO>();

        public bool ShouldSerializeBoneList() => BoneList != null && BoneList.Count > 0;
    }

    public class ClothBoneDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
    }
}