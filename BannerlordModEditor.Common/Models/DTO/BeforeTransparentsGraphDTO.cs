using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

// BeforeTransparentsGraph.xml - Post-processing effects graph configuration (DTO)
[XmlRoot("base")]
public class BeforeTransparentsGraphDTO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("postfx_graphs")]
    public PostfxGraphsDTO? PostfxGraphs { get; set; }
}

public class PostfxGraphsDTO
{
    [XmlElement("postfx_graph")]
    public List<PostfxGraphDTO> PostfxGraphList { get; set; } = new List<PostfxGraphDTO>();
}

public class PostfxGraphDTO
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlElement("postfx_node")]
    public List<PostfxNodeDTO> PostfxNodes { get; set; } = new List<PostfxNodeDTO>();
}

public class PostfxNodeDTO
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("class")]
    public string? Class { get; set; }

    [XmlAttribute("shader")]
    public string? Shader { get; set; }

    [XmlAttribute("format")]
    public string? Format { get; set; }

    [XmlAttribute("size")]
    public string? Size { get; set; }

    [XmlAttribute("width")]
    public string? Width { get; set; }

    [XmlAttribute("height")]
    public string? Height { get; set; }

    [XmlElement("output")]
    public List<PostfxNodeOutputDTO> Outputs { get; set; } = new List<PostfxNodeOutputDTO>();

    [XmlElement("input")]
    public List<PostfxNodeInputDTO> Inputs { get; set; } = new List<PostfxNodeInputDTO>();

    [XmlElement("preconditions")]
    public PostfxNodePreconditionsDTO? Preconditions { get; set; }
}

public class PostfxNodeInputDTO
{
    [XmlAttribute("index")]
    public string? Index { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("source")]
    public string? Source { get; set; }
}

public class PostfxNodeOutputDTO
{
    [XmlAttribute("index")]
    public string? Index { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }
}

public class PostfxNodePreconditionsDTO
{
    [XmlElement("config")]
    public List<PostfxNodeConfigDTO> Configs { get; set; } = new List<PostfxNodeConfigDTO>();
}

public class PostfxNodeConfigDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
}