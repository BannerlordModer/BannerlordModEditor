using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("base")]
    public class CollisionInfosBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlArray("collision_infos")]
        [XmlArrayItem("material")]
        public List<CollisionMaterial> Materials { get; set; } = new List<CollisionMaterial>();
    }

    public class CollisionMaterial
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("collision_info")]
        public List<CollisionInfo> CollisionInfos { get; set; } = new List<CollisionInfo>();
    }

    public class CollisionInfo
    {
        [XmlAttribute("second_material")]
        public string SecondMaterial { get; set; } = string.Empty;

        [XmlElement("collision_effect")]
        public CollisionEffect CollisionEffect { get; set; } = new CollisionEffect();
    }

    public class CollisionEffect
    {
        [XmlAttribute("particle")]
        public string Particle { get; set; } = string.Empty;

        [XmlAttribute("decal")]
        public string Decal { get; set; } = string.Empty;
    }
} 