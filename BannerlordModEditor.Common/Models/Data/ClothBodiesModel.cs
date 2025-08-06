using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("bodies")]
    public class ClothBodiesModel
    {
        [XmlElement("body")]
        public List<ClothBody> Bodies { get; set; } = new List<ClothBody>();

        public bool ShouldSerializeBodies() => Bodies != null && Bodies.Count > 0;
    }

    public class ClothBody
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("owner_skeleton")]
        public string OwnerSkeleton { get; set; }

        public bool ShouldSerializeOwnerSkeleton() => !string.IsNullOrEmpty(OwnerSkeleton);

        [XmlElement("capsules")]
        public ClothCapsules Capsules { get; set; }

        public bool ShouldSerializeCapsules() => Capsules != null;
    }

    public class ClothCapsules
    {
        [XmlElement("capsule")]
        public List<ClothCapsule> CapsuleList { get; set; } = new List<ClothCapsule>();

        public bool ShouldSerializeCapsuleList() => CapsuleList != null && CapsuleList.Count > 0;
    }

    public class ClothCapsule
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("origin")]
        public string Origin { get; set; }

        [XmlAttribute("frame")]
        public string Frame { get; set; }

        [XmlElement("points")]
        public ClothPoints Points { get; set; }

        public bool ShouldSerializePoints() => Points != null;
    }

    public class ClothPoints
    {
        [XmlElement("point")]
        public List<ClothPoint> PointList { get; set; } = new List<ClothPoint>();

        public bool ShouldSerializePointList() => PointList != null && PointList.Count > 0;
    }

    public class ClothPoint
    {
        [XmlAttribute("radius")]
        public string Radius { get; set; }

        [XmlElement("bones")]
        public ClothBones Bones { get; set; }

        public bool ShouldSerializeBones() => Bones != null;
    }

    public class ClothBones
    {
        [XmlElement("bone")]
        public List<ClothBone> BoneList { get; set; } = new List<ClothBone>();

        public bool ShouldSerializeBoneList() => BoneList != null && BoneList.Count > 0;
    }

    public class ClothBone
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("weight")]
        public string Weight { get; set; }

        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
    }
}