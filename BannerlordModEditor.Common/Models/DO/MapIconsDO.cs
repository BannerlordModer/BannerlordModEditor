using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class MapIconsDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("map_icons")]
        public MapIconsContainerDO MapIconsContainer { get; set; } = new MapIconsContainerDO();

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }

    public class MapIconsContainerDO
    {
        [XmlElement("map_icon")]
        public List<MapIconDO> MapIconList { get; set; } = new List<MapIconDO>();
    }

    public class MapIconDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("id_str")]
        public string? IdStr { get; set; }

        [XmlAttribute("flags")]
        public string? Flags { get; set; }

        [XmlAttribute("mesh_name")]
        public string? MeshName { get; set; }

        [XmlAttribute("mesh_scale")]
        public string? MeshScale { get; set; }

        [XmlAttribute("sound_no")]
        public string? SoundNo { get; set; }

        [XmlAttribute("offset_pos")]
        public string? OffsetPos { get; set; }

        [XmlAttribute("dirt_name")]
        public string? DirtName { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeIdStr() => !string.IsNullOrEmpty(IdStr);
        public bool ShouldSerializeFlags() => !string.IsNullOrEmpty(Flags);
        public bool ShouldSerializeMeshName() => !string.IsNullOrEmpty(MeshName);
        public bool ShouldSerializeMeshScale() => !string.IsNullOrEmpty(MeshScale);
        public bool ShouldSerializeSoundNo() => !string.IsNullOrEmpty(SoundNo);
        public bool ShouldSerializeOffsetPos() => !string.IsNullOrEmpty(OffsetPos);
        public bool ShouldSerializeDirtName() => !string.IsNullOrEmpty(DirtName);
    }
}