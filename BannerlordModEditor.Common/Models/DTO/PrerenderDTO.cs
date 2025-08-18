using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 预渲染图形配置的数据传输对象
    /// 用于序列化/反序列化操作
    /// </summary>
    [XmlRoot("base", Namespace = "")]
    public class PrerenderDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("postfx_graphs")]
        public PrerenderPostfxGraphsDTO PostfxGraphs { get; set; } = new PrerenderPostfxGraphsDTO();

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePostfxGraphs() => PostfxGraphs != null && PostfxGraphs.PostfxGraphList.Count > 0;
    }

    /// <summary>
    /// 后处理图形容器的DTO
    /// </summary>
    public class PrerenderPostfxGraphsDTO
    {
        [XmlElement("postfx_graph")]
        public List<PrerenderPostfxGraphDTO> PostfxGraphList { get; set; } = new List<PrerenderPostfxGraphDTO>();

        public bool ShouldSerializePostfxGraphList() => PostfxGraphList != null && PostfxGraphList.Count > 0;
    }

    /// <summary>
    /// 单个后处理图形的DTO
    /// </summary>
    public class PrerenderPostfxGraphDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("postfx_node")]
        public List<PrerenderPostfxNodeDTO> PostfxNodes { get; set; } = new List<PrerenderPostfxNodeDTO>();

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializePostfxNodes() => PostfxNodes != null && PostfxNodes.Count > 0;
    }

    /// <summary>
    /// 后处理节点的DTO
    /// </summary>
    public class PrerenderPostfxNodeDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("class")]
        public string Class { get; set; } = string.Empty;

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
        public string Compute { get; set; } = string.Empty;

        [XmlAttribute("compute_tg_size_x")]
        public string ComputeTgSizeX { get; set; } = string.Empty;

        [XmlAttribute("compute_tg_size_y")]
        public string ComputeTgSizeY { get; set; } = string.Empty;

        [XmlElement("input")]
        public List<InputDTO> Inputs { get; set; } = new List<InputDTO>();

        [XmlElement("preconditions")]
        public PreconditionsDTO Preconditions { get; set; } = new PreconditionsDTO();

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
        public bool ShouldSerializePreconditions() => Preconditions != null && Preconditions.Configs.Count > 0;
    }

    /// <summary>
    /// 输入配置的DTO
    /// </summary>
    public class InputDTO
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

    /// <summary>
    /// 预条件配置的DTO
    /// </summary>
    public class PreconditionsDTO
    {
        [XmlElement("config")]
        public List<ConfigDTO> Configs { get; set; } = new List<ConfigDTO>();

        public bool ShouldSerializeConfigs() => Configs != null && Configs.Count > 0;
    }

    /// <summary>
    /// 配置项的DTO
    /// </summary>
    public class ConfigDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}