using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class Prerender
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("postfx_graphs")]
        public PrerenderPostfxGraphs PostfxGraphs { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePostfxGraphs() => PostfxGraphs != null;
    }

    public class PrerenderPostfxGraphs
    {
        [XmlElement("postfx_graph")]
        public List<PrerenderPostfxGraph> PostfxGraphList { get; set; }

        public bool ShouldSerializePostfxGraphList() => PostfxGraphList != null && PostfxGraphList.Count > 0;
    }

    public class PrerenderPostfxGraph
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("postfx_node")]
        public List<PrerenderPostfxNode> PostfxNodes { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializePostfxNodes() => PostfxNodes != null && PostfxNodes.Count > 0;
    }

    public class PrerenderPostfxNode
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

        [XmlAttribute("compute")]
        public string Compute { get; set; }

        [XmlAttribute("compute_tg_size_x")]
        public string ComputeTgSizeX { get; set; }

        [XmlAttribute("compute_tg_size_y")]
        public string ComputeTgSizeY { get; set; }

        [XmlElement("input")]
        public List<Input> Inputs { get; set; }

        [XmlElement("preconditions")]
        public Preconditions Preconditions { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeClass() => !string.IsNullOrEmpty(Class);
        public bool ShouldSerializeShader() => !string.IsNullOrEmpty(Shader);
        public bool ShouldSerializeFormat() => !string.IsNullOrEmpty(Format);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializeWidth() => !string.IsNullOrEmpty(Width);
        public bool ShouldSerializeHeight() => !string.IsNullOrEmpty(Height);
        public bool ShouldSerializeCompute() => !string.IsNullOrEmpty(Compute);
        public bool ShouldSerializeComputeTgSizeX() => !string.IsNullOrEmpty(ComputeTgSizeX);
        public bool ShouldSerializeComputeTgSizeY() => !string.IsNullOrEmpty(ComputeTgSizeY);
        public bool ShouldSerializeInputs() => Inputs != null && Inputs.Count > 0;
        public bool ShouldSerializePreconditions() => Preconditions != null;
    }

    public class Input
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

    public class Preconditions
    {
        [XmlElement("config")]
        public List<Config> Configs { get; set; }

        public bool ShouldSerializeConfigs() => Configs != null && Configs.Count > 0;
    }

    public class Config
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}