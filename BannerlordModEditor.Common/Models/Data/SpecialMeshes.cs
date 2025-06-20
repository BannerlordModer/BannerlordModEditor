using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SpecialMeshes
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlArray("meshes")]
        [XmlArrayItem("mesh")]
        public SpecialMesh[]? Meshes { get; set; }
    }

    public class SpecialMesh
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlArray("types")]
        [XmlArrayItem("type")]
        public MeshType[]? Types { get; set; }
    }

    public class MeshType
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
    }
} 