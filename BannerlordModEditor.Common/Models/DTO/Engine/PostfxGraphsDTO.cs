using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Engine;

[XmlRoot("base")]
public class PostfxGraphsDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement("postfx_graphs")]
    public PostfxGraphsContainerDTO PostfxGraphs { get; set; } = new PostfxGraphsContainerDTO();
}

public class PostfxGraphsContainerDTO
{
    [XmlElement("postfx_graph")]
    public List<PostfxGraphDTO> Graphs { get; set; } = new List<PostfxGraphDTO>();
}

public class PostfxGraphDTO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("postfx_node")]
    public List<PostfxNodeDTO> Nodes { get; set; } = new List<PostfxNodeDTO>();
}

public class PostfxNodeDTO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("shader")]
    public string Shader { get; set; } = string.Empty;

    [XmlAttribute("class")]
    public string Class { get; set; } = string.Empty;

    [XmlAttribute("format")]
    public string Format { get; set; } = string.Empty;

    [XmlAttribute("size")]
    public string Size { get; set; } = string.Empty;

    [XmlAttribute("width")]
    public string Width { get; set; } = string.Empty;

    [XmlAttribute("height")]
    public string Height { get; set; } = string.Empty;

    [XmlElement("output")]
    public List<PostfxOutputDTO> Outputs { get; set; } = new List<PostfxOutputDTO>();

    [XmlElement("input")]
    public List<PostfxInputDTO> Inputs { get; set; } = new List<PostfxInputDTO>();

    [XmlElement("preconditions")]
    public PostfxPreconditionsDTO Preconditions { get; set; } = new PostfxPreconditionsDTO();
}

public class PostfxOutputDTO
{
    [XmlAttribute("index")]
    public string Index { get; set; } = string.Empty;

    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
}

public class PostfxInputDTO
{
    [XmlAttribute("index")]
    public string Index { get; set; } = string.Empty;

    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("source")]
    public string Source { get; set; } = string.Empty;
}

public class PostfxPreconditionsDTO
{
    [XmlElement("config")]
    public List<PostfxConfigDTO> Configs { get; set; } = new List<PostfxConfigDTO>();
}

public class PostfxConfigDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
}