using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("bodies")]
    public class ClothBodies
    {
        [XmlElement("body")]
        public List<ClothBody> Body { get; set; } = new List<ClothBody>();
    }

    public class ClothBody
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("owner_skeleton")]
        public string? OwnerSkeleton { get; set; }

        [XmlElement("capsules")]
        public CapsulesContainer? Capsules { get; set; }

        // 确保null值不会序列化
        public bool ShouldSerializeOwnerSkeleton() => OwnerSkeleton != null;
    }

    public class CapsulesContainer
    {
        [XmlElement("capsule")]
        public List<ClothCapsule> Capsule { get; set; } = new List<ClothCapsule>();
    }

    public class ClothCapsule
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("length")]
        public string Length { get; set; } = "0.000";

        [XmlAttribute("origin")]
        public string Origin { get; set; } = "0.000, 0.000, 0.000";

        [XmlAttribute("frame")]
        public string Frame { get; set; } = "1.000000, 0.000000, 0.000000, 0.000000, 0.000000, 1.000000, 0.000000, 0.000000, 0.000000, 0.000000, 1.000000, 0.000000, 0.000000, 0.000000, 0.000000, 1.000000";

        [XmlElement("points")]
        public PointsContainer? Points { get; set; }
    }

    public class PointsContainer
    {
        [XmlElement("point")]
        public List<ClothPoint> Point { get; set; } = new List<ClothPoint>();
    }

    public class ClothPoint
    {
        [XmlAttribute("radius")]
        public string Radius { get; set; } = "0.100";

        [XmlElement("bones")]
        public BonesContainer? Bones { get; set; }
    }

    public class BonesContainer
    {
        [XmlElement("bone")]
        public List<ClothBone> Bone { get; set; } = new List<ClothBone>();
    }

    public class ClothBone
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = "1.000";
    }
} 