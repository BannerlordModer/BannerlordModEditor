using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine;

// special_meshes.xml - Special mesh definitions
[XmlRoot("base")]
public class SpecialMeshesRoot
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("meshes")]
    public SpecialMeshes? Meshes { get; set; }
}

public class SpecialMeshes
{
    [XmlElement("mesh")]
    public SpecialMesh[]? Mesh { get; set; }
}

public class SpecialMesh
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("types")]
    public MeshTypes? Types { get; set; }
}

public class MeshTypes
{
    [XmlElement("type")]
    public MeshType[]? Type { get; set; }
}

public class MeshType
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
} 