using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("base")]
    public class PostfxGraphsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("postfx_graphs")]
        public PostfxGraphsContainer PostfxGraphs { get; set; } = new PostfxGraphsContainer();
    }

    public class PostfxGraphsContainer
    {
        [XmlElement("postfx_graph")]
        public List<PostfxGraph> PostfxGraphList { get; set; } = new List<PostfxGraph>();
    }

    public class PostfxGraph
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("postfx_node")]
        public List<PostfxNode> PostfxNodeList { get; set; } = new List<PostfxNode>();
    }

    public class PostfxNode
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("class")]
        public string? Class { get; set; }

        [XmlAttribute("shader")]
        public string Shader { get; set; } = string.Empty;

        [XmlAttribute("format")]
        public string Format { get; set; } = string.Empty;

        [XmlAttribute("size")]
        public string Size { get; set; } = string.Empty;

        [XmlAttribute("width")]
        public string Width { get; set; } = string.Empty;

        [XmlAttribute("height")]
        public string Height { get; set; } = string.Empty;

        [XmlAttribute("compute")]
        public string? Compute { get; set; }

        [XmlAttribute("compute_tg_size_x")]
        public string? ComputeTgSizeX { get; set; }

        [XmlAttribute("compute_tg_size_y")]
        public string? ComputeTgSizeY { get; set; }

        [XmlElement("input")]
        public List<PostfxInput> Inputs { get; set; } = new List<PostfxInput>();

        [XmlElement("output")]
        public List<PostfxOutput> Outputs { get; set; } = new List<PostfxOutput>();

        [XmlElement("preconditions")]
        public PostfxPreconditions? Preconditions { get; set; }
    }

    public class PostfxInput
    {
        [XmlAttribute("index")]
        public string Index { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("source")]
        public string Source { get; set; } = string.Empty;
    }

    public class PostfxOutput
    {
        [XmlAttribute("index")]
        public string Index { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class PostfxPreconditions
    {
        [XmlElement("config")]
        public PostfxConfig? Config { get; set; }
    }

    public class PostfxConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 