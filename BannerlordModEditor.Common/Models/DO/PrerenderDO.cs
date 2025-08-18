using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 预渲染图形配置的领域对象
    /// 用于Prerender.xml文件的完整处理
    /// 包含后处理图形节点和渲染管线配置
    /// </summary>
    [XmlRoot("base", Namespace = "")]
    public class PrerenderDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("postfx_graphs")]
        public PrerenderPostfxGraphsDO PostfxGraphs { get; set; } = new PrerenderPostfxGraphsDO();

        // 运行时标记
        [XmlIgnore]
        public bool HasPostfxGraphs { get; set; } = false;

        // 业务逻辑：图形索引
        [XmlIgnore]
        public Dictionary<string, PrerenderPostfxGraphDO> GraphsById { get; set; } = new Dictionary<string, PrerenderPostfxGraphDO>();

        // 业务逻辑：节点索引
        [XmlIgnore]
        public Dictionary<string, List<PrerenderPostfxNodeDO>> NodesByGraphId { get; set; } = new Dictionary<string, List<PrerenderPostfxNodeDO>>();

        // 业务逻辑：输入源索引
        [XmlIgnore]
        public Dictionary<string, List<PrerenderPostfxNodeDO>> NodesByInputSource { get; set; } = new Dictionary<string, List<PrerenderPostfxNodeDO>>();

        // 业务逻辑方法
        public void InitializeIndexes()
        {
            GraphsById.Clear();
            NodesByGraphId.Clear();
            NodesByInputSource.Clear();

            if (PostfxGraphs?.PostfxGraphList == null) return;

            foreach (var graph in PostfxGraphs.PostfxGraphList)
            {
                if (!string.IsNullOrEmpty(graph.Id))
                {
                    GraphsById[graph.Id] = graph;
                }

                // 按图形ID索引节点
                if (!string.IsNullOrEmpty(graph.Id))
                {
                    NodesByGraphId[graph.Id] = graph.PostfxNodes ?? new List<PrerenderPostfxNodeDO>();
                }

                // 按输入源索引节点
                if (graph.PostfxNodes != null)
                {
                    foreach (var node in graph.PostfxNodes)
                    {
                        if (node.Inputs != null)
                        {
                            foreach (var input in node.Inputs)
                            {
                                if (!string.IsNullOrEmpty(input.Source))
                                {
                                    if (!NodesByInputSource.ContainsKey(input.Source))
                                    {
                                        NodesByInputSource[input.Source] = new List<PrerenderPostfxNodeDO>();
                                    }
                                    NodesByInputSource[input.Source].Add(node);
                                }
                            }
                        }
                    }
                }
            }
        }

        public PrerenderPostfxGraphDO? GetGraphById(string id)
        {
            return GraphsById.GetValueOrDefault(id);
        }

        public List<PrerenderPostfxNodeDO> GetNodesByGraphId(string graphId)
        {
            return NodesByGraphId.GetValueOrDefault(graphId, new List<PrerenderPostfxNodeDO>());
        }

        public List<PrerenderPostfxNodeDO> GetNodesByInputSource(string source)
        {
            return NodesByInputSource.GetValueOrDefault(source, new List<PrerenderPostfxNodeDO>());
        }

        public List<PrerenderPostfxGraphDO> GetComputeGraphs()
        {
            return PostfxGraphs?.PostfxGraphList?
                .Where(g => g.PostfxNodes?.Any(n => n.IsComputeNode()) ?? false)
                .ToList() ?? new List<PrerenderPostfxGraphDO>();
        }

        public List<PrerenderPostfxNodeDO> GetNodesWithPreconditions()
        {
            return PostfxGraphs?.PostfxGraphList?
                .SelectMany(g => g.PostfxNodes ?? new List<PrerenderPostfxNodeDO>())
                .Where(n => n.HasPreconditions)
                .ToList() ?? new List<PrerenderPostfxNodeDO>();
        }

        // 验证方法
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Type)) return false;
            
            if (PostfxGraphs?.PostfxGraphList == null || PostfxGraphs.PostfxGraphList.Count == 0)
                return false;

            foreach (var graph in PostfxGraphs.PostfxGraphList)
            {
                if (string.IsNullOrEmpty(graph.Id)) return false;
                if (graph.PostfxNodes == null || graph.PostfxNodes.Count == 0) return false;
                
                foreach (var node in graph.PostfxNodes)
                {
                    if (string.IsNullOrEmpty(node.Id)) return false;
                    if (string.IsNullOrEmpty(node.Class)) return false;
                }
            }

            return true;
        }

        // 性能分析
        public int GetTotalNodeCount()
        {
            return PostfxGraphs?.PostfxGraphList?
                .Sum(g => g.PostfxNodes?.Count ?? 0) ?? 0;
        }

        public double GetAverageNodesPerGraph()
        {
            var graphCount = PostfxGraphs?.PostfxGraphList?.Count ?? 0;
            if (graphCount == 0) return 0;
            
            var totalNodes = GetTotalNodeCount();
            return (double)totalNodes / graphCount;
        }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePostfxGraphs() => HasPostfxGraphs && PostfxGraphs != null && PostfxGraphs.PostfxGraphList.Count > 0;
    }

    /// <summary>
    /// 后处理图形容器
    /// </summary>
    public class PrerenderPostfxGraphsDO
    {
        [XmlElement("postfx_graph")]
        public List<PrerenderPostfxGraphDO> PostfxGraphList { get; set; } = new List<PrerenderPostfxGraphDO>();

        public bool ShouldSerializePostfxGraphList() => PostfxGraphList != null && PostfxGraphList.Count > 0;
    }

    /// <summary>
    /// 单个后处理图形
    /// </summary>
    public class PrerenderPostfxGraphDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("postfx_node")]
        public List<PrerenderPostfxNodeDO> PostfxNodes { get; set; } = new List<PrerenderPostfxNodeDO>();

        // 运行时标记
        [XmlIgnore]
        public bool HasEmptyPostfxNodes { get; set; } = false;

        // 业务逻辑方法
        public bool IsComputeGraph()
        {
            return PostfxNodes?.Any(n => n.IsComputeNode()) ?? false;
        }

        public List<PrerenderPostfxNodeDO> GetComputeNodes()
        {
            return PostfxNodes?.Where(n => n.IsComputeNode()).ToList() ?? new List<PrerenderPostfxNodeDO>();
        }

        public List<PrerenderPostfxNodeDO> GetNodesWithPreconditions()
        {
            return PostfxNodes?.Where(n => n.HasPreconditions).ToList() ?? new List<PrerenderPostfxNodeDO>();
        }

        public PrerenderPostfxNodeDO? GetNodeById(string nodeId)
        {
            return PostfxNodes?.FirstOrDefault(n => n.Id == nodeId);
        }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializePostfxNodes() => PostfxNodes != null && PostfxNodes.Count > 0;
    }

    /// <summary>
    /// 后处理节点
    /// </summary>
    public class PrerenderPostfxNodeDO
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
        public List<InputDO> Inputs { get; set; } = new List<InputDO>();

        [XmlElement("preconditions")]
        public PreconditionsDO Preconditions { get; set; } = new PreconditionsDO();

        // 运行时标记
        [XmlIgnore]
        public bool HasEmptyInputs { get; set; } = false;

        [XmlIgnore]
        public bool HasPreconditions { get; set; } = false;

        // 业务逻辑方法
        public bool IsComputeNode()
        {
            return Compute?.ToLower() == "true";
        }

        public bool IsRelativeSize()
        {
            return Size?.ToLower() == "relative";
        }

        public bool IsFixedSize()
        {
            return Size?.ToLower() == "fixed";
        }

        public int? GetComputeGroupSizeX()
        {
            if (int.TryParse(ComputeTgSizeX, out var size))
                return size;
            return null;
        }

        public int? GetComputeGroupSizeY()
        {
            if (int.TryParse(ComputeTgSizeY, out var size))
                return size;
            return null;
        }

        public double? GetWidthValue()
        {
            if (double.TryParse(Width, out var width))
                return width;
            return null;
        }

        public double? GetHeightValue()
        {
            if (double.TryParse(Height, out var height))
                return height;
            return null;
        }

        public List<InputDO> GetProvidedInputs()
        {
            return Inputs?.Where(i => i.Type?.ToLower() == "provided").ToList() ?? new List<InputDO>();
        }

        public List<InputDO> GetNodeInputs()
        {
            return Inputs?.Where(i => i.Type?.ToLower() == "node").ToList() ?? new List<InputDO>();
        }

        public InputDO? GetInputByIndex(int index)
        {
            return Inputs?.FirstOrDefault(i => int.TryParse(i.Index, out var iIndex) && iIndex == index);
        }

        public bool HasInputSource(string source)
        {
            return Inputs?.Any(i => i.Source == source) ?? false;
        }

        // 渲染管线分析
        public bool IsDepthNode()
        {
            return Shader?.Contains("depth") ?? false;
        }

        public bool IsNormalNode()
        {
            return Shader?.Contains("normal") ?? false;
        }

        public bool IsAONode()
        {
            return (Class?.Contains("SAO") ?? false) || (Shader?.Contains("ao") ?? false);
        }

        // 性能估算
        public int GetEstimatedComplexity()
        {
            int complexity = 1;
            
            // 计算着色器通常更复杂
            if (IsComputeNode()) complexity *= 2;
            
            // 输入数量增加复杂度
            complexity += Inputs?.Count ?? 0;
            
            // 有预条件增加复杂度
            if (HasPreconditions) complexity += 2;
            
            return complexity;
        }

        // 序列化控制方法
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
        public bool ShouldSerializePreconditions() => HasPreconditions && Preconditions != null && Preconditions.Configs.Count > 0;
    }

    /// <summary>
    /// 输入配置
    /// </summary>
    public class InputDO
    {
        [XmlAttribute("index")]
        public string Index { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("source")]
        public string Source { get; set; } = string.Empty;

        // 输入类型常量
        public const string ProvidedType = "provided";
        public const string NodeType = "node";
        public const string TextureType = "texture";

        // 业务逻辑方法
        public bool IsProvidedInput() => Type?.ToLower() == ProvidedType.ToLower();
        public bool IsNodeInput() => Type?.ToLower() == NodeType.ToLower();
        public bool IsTextureInput() => Type?.ToLower() == TextureType.ToLower();

        public int? GetIndexValue()
        {
            if (int.TryParse(Index, out var index))
                return index;
            return null;
        }

        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source);
    }

    /// <summary>
    /// 预条件配置
    /// </summary>
    public class PreconditionsDO
    {
        [XmlElement("config")]
        public List<ConfigDO> Configs { get; set; } = new List<ConfigDO>();

        // 业务逻辑方法
        public ConfigDO? GetConfigByName(string name)
        {
            return Configs?.FirstOrDefault(c => c.Name == name);
        }

        public List<ConfigDO> GetQualityConfigs()
        {
            return Configs?.Where(c => c.Name?.Contains("quality", StringComparison.OrdinalIgnoreCase) ?? false).ToList() 
                   ?? new List<ConfigDO>();
        }

        public List<ConfigDO> GetPerformanceConfigs()
        {
            return Configs?.Where(c => c.Name?.Contains("performance", StringComparison.OrdinalIgnoreCase) ?? false).ToList() 
                   ?? new List<ConfigDO>();
        }

        public bool ShouldSerializeConfigs() => Configs != null && Configs.Count > 0;
    }

    /// <summary>
    /// 配置项
    /// </summary>
    public class ConfigDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        // 业务逻辑方法
        public bool IsQualityConfig()
        {
            return Name?.Contains("quality", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool IsPerformanceConfig()
        {
            return Name?.Contains("performance", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool IsEnabledConfig()
        {
            return Name?.Contains("enable", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}