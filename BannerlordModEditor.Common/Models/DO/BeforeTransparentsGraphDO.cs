using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO;

// BeforeTransparentsGraph.xml - Post-processing effects graph configuration
[XmlRoot("base")]
public class BeforeTransparentsGraphDO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("postfx_graphs")]
    public PostfxGraphsDO? PostfxGraphs { get; set; }
    
    [XmlIgnore]
    public bool HasPostfxGraphs { get; set; }
    
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializePostfxGraphs() => HasPostfxGraphs;
}

public class PostfxGraphsDO
{
    [XmlElement("postfx_graph")]
    public List<PostfxGraphDO> PostfxGraphList { get; set; } = new List<PostfxGraphDO>();
    
    [XmlIgnore]
    public bool HasEmptyPostfxGraphs { get; set; }
    
    public bool ShouldSerializePostfxGraphList() => HasEmptyPostfxGraphs || (PostfxGraphList != null && PostfxGraphList.Count > 0);
}

public class PostfxGraphDO
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlElement("postfx_node")]
    public List<PostfxNodeDO> PostfxNodes { get; set; } = new List<PostfxNodeDO>();
    
    [XmlIgnore]
    public bool HasEmptyPostfxNodes { get; set; }
    
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializePostfxNodes() => HasEmptyPostfxNodes || (PostfxNodes != null && PostfxNodes.Count > 0);
}

public class PostfxNodeDO
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
    public List<PostfxNodeOutputDO> Outputs { get; set; } = new List<PostfxNodeOutputDO>();
    
    [XmlIgnore]
    public bool HasEmptyOutputs { get; set; }

    [XmlElement("input")]
    public List<PostfxNodeInputDO> Inputs { get; set; } = new List<PostfxNodeInputDO>();
    
    [XmlIgnore]
    public bool HasEmptyInputs { get; set; }

    [XmlElement("preconditions")]
    public PostfxNodePreconditionsDO? Preconditions { get; set; }
    
    [XmlIgnore]
    public bool HasPreconditions { get; set; }
    
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeClass() => !string.IsNullOrEmpty(Class);
    public bool ShouldSerializeShader() => !string.IsNullOrEmpty(Shader);
    public bool ShouldSerializeFormat() => !string.IsNullOrEmpty(Format);
    public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
    public bool ShouldSerializeWidth() => !string.IsNullOrEmpty(Width);
    public bool ShouldSerializeHeight() => !string.IsNullOrEmpty(Height);
    public bool ShouldSerializeInputs() => HasEmptyInputs || (Inputs != null && Inputs.Count > 0);
    public bool ShouldSerializeOutputs() => HasEmptyOutputs || (Outputs != null && Outputs.Count > 0);
    public bool ShouldSerializePreconditions() => HasPreconditions && Preconditions != null;
}

public class PostfxNodeInputDO
{
    [XmlAttribute("index")]
    public string? Index { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("source")]
    public string? Source { get; set; }
    
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);
}

public class PostfxNodeOutputDO
{
    [XmlAttribute("index")]
    public string? Index { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }
    
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}

public class PostfxNodePreconditionsDO
{
    [XmlElement("config")]
    public List<PostfxNodeConfigDO> Configs { get; set; } = new List<PostfxNodeConfigDO>();
    
    [XmlIgnore]
    public bool HasEmptyConfigs { get; set; }
    
    public bool ShouldSerializeConfigs() => HasEmptyConfigs || (Configs != null && Configs.Count > 0);
}

public class PostfxNodeConfigDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}