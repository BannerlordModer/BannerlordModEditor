using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine;

// special_meshes.xml - Special mesh definitions
[XmlRoot("base")]
public class SpecialMeshesBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "special_meshes";

    [XmlElement("meshes")]
    public MeshesContainer Meshes { get; set; } = new MeshesContainer();
}

public class MeshesContainer
{
    [XmlElement("mesh")]
    public List<SpecialMesh> Mesh { get; set; } = new List<SpecialMesh>();
}

public class SpecialMesh
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("types")]
    public MeshTypesContainer? Types { get; set; }
}

public class MeshTypesContainer
{
    [XmlElement("type")]
    public List<MeshType> Type { get; set; } = new List<MeshType>();
}

public class MeshType
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
} 