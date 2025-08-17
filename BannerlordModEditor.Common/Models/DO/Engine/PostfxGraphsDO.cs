using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Engine;

[XmlRoot("base")]
public class PostfxGraphsDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement("postfx_graphs")]
    public PostfxGraphsContainerDO PostfxGraphs { get; set; } = new PostfxGraphsContainerDO();

    [XmlIgnore]
    public bool HasPostfxGraphs { get; set; } = false;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializePostfxGraphs() => HasPostfxGraphs && PostfxGraphs != null && PostfxGraphs.Graphs.Count > 0;
}

public class PostfxGraphsContainerDO
{
    [XmlElement("postfx_graph")]
    public List<PostfxGraphDO> Graphs { get; set; } = new List<PostfxGraphDO>();

    public bool ShouldSerializeGraphs() => Graphs != null && Graphs.Count > 0;
}

public class PostfxGraphDO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("postfx_node")]
    public List<PostfxNodeDO> Nodes { get; set; } = new List<PostfxNodeDO>();

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeNodes() => Nodes != null && Nodes.Count > 0;
}

public class PostfxNodeDO
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
    public List<PostfxOutputDO> Outputs { get; set; } = new List<PostfxOutputDO>();

    [XmlElement("input")]
    public List<PostfxInputDO> Inputs { get; set; } = new List<PostfxInputDO>();

    [XmlElement("preconditions")]
    public PostfxPreconditionsDO Preconditions { get; set; } = new PostfxPreconditionsDO();

    [XmlIgnore]
    public bool HasPreconditions { get; set; } = false;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeShader() => !string.IsNullOrEmpty(Shader);
    public bool ShouldSerializeClass() => !string.IsNullOrEmpty(Class);
    public bool ShouldSerializeFormat() => !string.IsNullOrEmpty(Format);
    public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
    public bool ShouldSerializeWidth() => !string.IsNullOrEmpty(Width);
    public bool ShouldSerializeHeight() => !string.IsNullOrEmpty(Height);
    public bool ShouldSerializeOutputs() => Outputs != null && Outputs.Count > 0;
    public bool ShouldSerializeInputs() => Inputs != null && Inputs.Count > 0;
    public bool ShouldSerializePreconditions() => HasPreconditions && Preconditions != null && Preconditions.Configs.Count > 0;
}

public class PostfxOutputDO
{
    [XmlAttribute("index")]
    public string Index { get; set; } = string.Empty;

    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}

public class PostfxInputDO
{
    [XmlAttribute("index")]
    public string Index { get; set; } = string.Empty;

    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("source")]
    public string Source { get; set; } = string.Empty;

    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);
}

public class PostfxPreconditionsDO
{
    [XmlElement("config")]
    public List<PostfxConfigDO> Configs { get; set; } = new List<PostfxConfigDO>();

    public bool ShouldSerializeConfigs() => Configs != null && Configs.Count > 0;
}

public class PostfxConfigDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}