using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SpecialMeshesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("meshes")]
        public MeshesContainer Meshes { get; set; } = new MeshesContainer();
    }

    public class MeshesContainer
    {
        [XmlElement("mesh")]
        public List<SpecialMesh> MeshList { get; set; } = new List<SpecialMesh>();
    }

    public class SpecialMesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("types")]
        public MeshTypes Types { get; set; } = new MeshTypes();
    }

    public class MeshTypes
    {
        [XmlElement("type")]
        public List<MeshType> TypeList { get; set; } = new List<MeshType>();
    }

    public class MeshType
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 