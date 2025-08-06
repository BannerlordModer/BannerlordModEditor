using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class MapIcons
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("map_icons")]
        public MapIconsContainer MapIconsContainer { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }

    public class MapIconsContainer
    {
        [XmlElement("map_icon")]
        public List<MapIcon> MapIconList { get; set; } = new List<MapIcon>();
    }

    public class MapIcon
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("id_str")]
        public string IdStr { get; set; }

        [XmlAttribute("flags")]
        public string Flags { get; set; }

        [XmlAttribute("mesh_name")]
        public string MeshName { get; set; }

        [XmlAttribute("mesh_scale")]
        public float MeshScale { get; set; }

        [XmlAttribute("sound_no")]
        public int SoundNo { get; set; }

        [XmlAttribute("offset_pos")]
        public string OffsetPos { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeIdStr() => !string.IsNullOrEmpty(IdStr);
        public bool ShouldSerializeFlags() => !string.IsNullOrEmpty(Flags);
        public bool ShouldSerializeMeshName() => !string.IsNullOrEmpty(MeshName);
        public bool ShouldSerializeMeshScale() => MeshScale != default;
        public bool ShouldSerializeSoundNo() => SoundNo != default;
        public bool ShouldSerializeOffsetPos() => !string.IsNullOrEmpty(OffsetPos);
    }
}