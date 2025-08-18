using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class BeforeTransparentsGraph
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("postfx_graphs")]
        public PostfxGraphs PostfxGraphs { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePostfxGraphs() => PostfxGraphs != null;
    }

    public class PostfxGraphs
    {
        [XmlElement("postfx_graph")]
        public List<PostfxGraph> PostfxGraphList { get; set; }

        public bool ShouldSerializePostfxGraphList() => PostfxGraphList != null && PostfxGraphList.Count > 0;
    }

    public class PostfxGraph
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("postfx_node")]
        public List<PostfxNode> PostfxNodes { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializePostfxNodes() => PostfxNodes != null && PostfxNodes.Count > 0;
    }

    public class PostfxNode
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("shader")]
        public string Shader { get; set; }

        [XmlAttribute("format")]
        public string Format { get; set; }

        [XmlAttribute("size")]
        public string Size { get; set; }

        [XmlAttribute("width")]
        public string Width { get; set; }

        [XmlAttribute("height")]
        public string Height { get; set; }

        [XmlElement("output")]
        public List<PostfxNodeOutput> Outputs { get; set; }

        [XmlElement("input")]
        public List<PostfxNodeInput> Inputs { get; set; }

        [XmlElement("preconditions")]
        public PostfxNodePreconditions Preconditions { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeClass() => !string.IsNullOrEmpty(Class);
        public bool ShouldSerializeShader() => !string.IsNullOrEmpty(Shader);
        public bool ShouldSerializeFormat() => !string.IsNullOrEmpty(Format);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializeWidth() => !string.IsNullOrEmpty(Width);
        public bool ShouldSerializeHeight() => !string.IsNullOrEmpty(Height);
        public bool ShouldSerializeInputs() => Inputs != null && Inputs.Count > 0;
        public bool ShouldSerializeOutputs() => Outputs != null && Outputs.Count > 0;
        public bool ShouldSerializePreconditions() => Preconditions != null;
    }

    public class PostfxNodeInput
    {
        [XmlAttribute("index")]
        public string Index { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);
    }

    public class PostfxNodeOutput
    {
        [XmlAttribute("index")]
        public string Index { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }

    public class PostfxNodePreconditions
    {
        [XmlElement("config")]
        public List<PostfxNodeConfig> Configs { get; set; }

        public bool ShouldSerializeConfigs() => Configs != null && Configs.Count > 0;
    }

    public class PostfxNodeConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}