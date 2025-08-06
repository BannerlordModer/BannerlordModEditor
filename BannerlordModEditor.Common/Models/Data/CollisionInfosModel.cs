using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class CollisionInfosRoot
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("collision_infos")]
        public CollisionInfos CollisionInfos { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null;
    }

    public class CollisionInfos
    {
        [XmlElement("material")]
        public List<CollisionMaterial> Materials { get; set; }

        public bool ShouldSerializeMaterials() => Materials != null && Materials.Count > 0;
    }

    public class CollisionMaterial
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("collision_info")]
        public List<CollisionInfo> CollisionInfos { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionInfo
    {
        [XmlAttribute("second_material")]
        public string SecondMaterial { get; set; }

        [XmlElement("collision_effect")]
        public CollisionEffect CollisionEffect { get; set; }

        public bool ShouldSerializeSecondMaterial() => !string.IsNullOrEmpty(SecondMaterial);
        public bool ShouldSerializeCollisionEffect() => CollisionEffect != null;
    }

    public class CollisionEffect
    {
        [XmlAttribute("particle")]
        public string Particle { get; set; }

        [XmlAttribute("decal")]
        public string Decal { get; set; }

        public bool ShouldSerializeParticle() => !string.IsNullOrEmpty(Particle);
        public bool ShouldSerializeDecal() => !string.IsNullOrEmpty(Decal);
    }
}