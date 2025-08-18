using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class ClothBodiesDTO
    {
        [XmlElement("body")]
        public List<ClothBodyDTO> Bodies { get; set; } = new List<ClothBodyDTO>();
    }

    public class ClothBodyDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("owner_skeleton")]
        public string OwnerSkeleton { get; set; } = string.Empty;

        [XmlElement("capsules")]
        public ClothCapsulesDTO Capsules { get; set; }
    }

    public class ClothCapsulesDTO
    {
        [XmlElement("capsule")]
        public List<ClothCapsuleDTO> CapsuleList { get; set; } = new List<ClothCapsuleDTO>();
    }

    public class ClothCapsuleDTO
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
        public ClothPointsDTO Points { get; set; }
    }

    public class ClothPointsDTO
    {
        [XmlElement("point")]
        public List<ClothPointDTO> PointList { get; set; } = new List<ClothPointDTO>();
    }

    public class ClothPointDTO
    {
        [XmlAttribute("radius")]
        public string Radius { get; set; } = string.Empty;

        [XmlElement("bones")]
        public ClothBonesDTO Bones { get; set; }
    }

    public class ClothBonesDTO
    {
        [XmlElement("bone")]
        public List<ClothBoneDTO> BoneList { get; set; } = new List<ClothBoneDTO>();
    }

    public class ClothBoneDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
}