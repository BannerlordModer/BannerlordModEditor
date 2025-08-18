using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// Prerender的DO/DTO映射器
    /// 处理预渲染图形配置的领域对象和数据传输对象之间的双向转换
    /// </summary>
    public static class PrerenderMapper
    {
        /// <summary>
        /// 将DO转换为DTO
        /// </summary>
        public static PrerenderDTO ToDTO(PrerenderDO source)
        {
            if (source == null) return null;

            return new PrerenderDTO
            {
                Type = source.Type,
                PostfxGraphs = PrerenderPostfxGraphsMapper.ToDTO(source.PostfxGraphs)
            };
        }

        /// <summary>
        /// 将DTO转换为DO
        /// </summary>
        public static PrerenderDO ToDO(PrerenderDTO source)
        {
            if (source == null) return null;

            var result = new PrerenderDO
            {
                Type = source.Type,
                PostfxGraphs = PrerenderPostfxGraphsMapper.ToDO(source.PostfxGraphs),
                HasPostfxGraphs = source.PostfxGraphs != null && source.PostfxGraphs.PostfxGraphList.Count > 0
            };

            // 初始化索引和业务逻辑
            result.InitializeIndexes();

            return result;
        }

        /// <summary>
        /// 批量转换DO列表为DTO列表
        /// </summary>
        public static List<PrerenderDTO> ToDTO(List<PrerenderDO> source)
        {
            return source?.Select(ToDTO).Where(dto => dto != null).ToList() ?? new List<PrerenderDTO>();
        }

        /// <summary>
        /// 批量转换DTO列表为DO列表
        /// </summary>
        public static List<PrerenderDO> ToDO(List<PrerenderDTO> source)
        {
            return source?.Select(PrerenderMapper.ToDO).Where(dobj => dobj != null).ToList() ?? new List<PrerenderDO>();
        }

        /// <summary>
        /// 深度复制DO
        /// </summary>
        public static PrerenderDO DeepCopy(PrerenderDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        /// <summary>
        /// 验证DO对象数据完整性
        /// </summary>
        public static bool Validate(PrerenderDO source)
        {
            if (source == null) return false;
            return source.IsValid();
        }

        /// <summary>
        /// 获取验证错误信息
        /// </summary>
        public static List<string> GetValidationErrors(PrerenderDO source)
        {
            var errors = new List<string>();

            if (source == null)
            {
                errors.Add("PrerenderDO object is null");
                return errors;
            }

            if (string.IsNullOrEmpty(source.Type))
                errors.Add("Type is required");

            if (source.PostfxGraphs?.PostfxGraphList == null || source.PostfxGraphs.PostfxGraphList.Count == 0)
                errors.Add("At least one postfx_graph is required");

            if (source.PostfxGraphs?.PostfxGraphList != null)
            {
                foreach (var graph in source.PostfxGraphs.PostfxGraphList)
                {
                    if (string.IsNullOrEmpty(graph.Id))
                        errors.Add($"Graph ID is required for graph at index {source.PostfxGraphs.PostfxGraphList.IndexOf(graph)}");

                    if (graph.PostfxNodes == null || graph.PostfxNodes.Count == 0)
                        errors.Add($"Graph '{graph.Id}' must have at least one postfx_node");

                    if (graph.PostfxNodes != null)
                    {
                        foreach (var node in graph.PostfxNodes)
                        {
                            if (string.IsNullOrEmpty(node.Id))
                                errors.Add($"Node ID is required in graph '{graph.Id}'");

                            if (string.IsNullOrEmpty(node.Class))
                                errors.Add($"Node class is required for node '{node.Id}' in graph '{graph.Id}'");
                        }
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// 合并两个DO对象（source的值覆盖target）
        /// </summary>
        public static PrerenderDO Merge(PrerenderDO target, PrerenderDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 保留target的基本属性，除非source提供了新值
            if (!string.IsNullOrEmpty(source.Type))
                target.Type = source.Type;

            // 合并PostfxGraphs
            if (source.PostfxGraphs?.PostfxGraphList != null)
            {
                if (target.PostfxGraphs.PostfxGraphList == null)
                {
                    target.PostfxGraphs.PostfxGraphList = new List<PrerenderPostfxGraphDO>();
                }

                // 按ID合并图形
                foreach (var sourceGraph in source.PostfxGraphs.PostfxGraphList)
                {
                    var targetGraph = target.PostfxGraphs.PostfxGraphList
                        .FirstOrDefault(g => g.Id == sourceGraph.Id);

                    if (targetGraph == null)
                    {
                        // 添加新图形
                        targetGraph = PrerenderPostfxGraphMapper.DeepCopy(sourceGraph);
                        target.PostfxGraphs.PostfxGraphList.Add(targetGraph);
                    }
                    else
                    {
                        // 合并现有图形
                        PrerenderPostfxGraphMapper.Merge(targetGraph, sourceGraph);
                    }
                }
            }

            // 更新运行时标记
            target.HasPostfxGraphs = target.PostfxGraphs.PostfxGraphList.Count > 0;

            // 重新初始化索引
            target.InitializeIndexes();

            return target;
        }
    }

    /// <summary>
    /// PrerenderPostfxGraphs的映射器
    /// </summary>
    internal static class PrerenderPostfxGraphsMapper
    {
        public static PrerenderPostfxGraphsDTO ToDTO(PrerenderPostfxGraphsDO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxGraphsDTO
            {
                PostfxGraphList = source.PostfxGraphList?
                    .Select(PrerenderPostfxGraphMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<PrerenderPostfxGraphDTO>()
            };
        }

        public static PrerenderPostfxGraphsDO ToDO(PrerenderPostfxGraphsDTO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxGraphsDO
            {
                PostfxGraphList = source.PostfxGraphList?
                    .Select(PrerenderPostfxGraphMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<PrerenderPostfxGraphDO>()
            };
        }
    }

    /// <summary>
    /// PrerenderPostfxGraph的映射器
    /// </summary>
    internal static class PrerenderPostfxGraphMapper
    {
        public static PrerenderPostfxGraphDTO ToDTO(PrerenderPostfxGraphDO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxGraphDTO
            {
                Id = source.Id,
                PostfxNodes = source.PostfxNodes?
                    .Select(PrerenderPostfxNodeMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<PrerenderPostfxNodeDTO>()
            };
        }

        public static PrerenderPostfxGraphDO ToDO(PrerenderPostfxGraphDTO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxGraphDO
            {
                Id = source.Id,
                PostfxNodes = source.PostfxNodes?
                    .Select(PrerenderPostfxNodeMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<PrerenderPostfxNodeDO>(),
                HasEmptyPostfxNodes = source.PostfxNodes == null || source.PostfxNodes.Count == 0
            };
        }

        public static PrerenderPostfxGraphDO DeepCopy(PrerenderPostfxGraphDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        public static PrerenderPostfxGraphDO Merge(PrerenderPostfxGraphDO target, PrerenderPostfxGraphDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 保留target的基本属性
            if (!string.IsNullOrEmpty(source.Id))
                target.Id = source.Id;

            // 合并节点
            if (source.PostfxNodes != null)
            {
                if (target.PostfxNodes == null)
                {
                    target.PostfxNodes = new List<PrerenderPostfxNodeDO>();
                }

                // 按ID合并节点
                foreach (var sourceNode in source.PostfxNodes)
                {
                    var targetNode = target.PostfxNodes
                        .FirstOrDefault(n => n.Id == sourceNode.Id);

                    if (targetNode == null)
                    {
                        // 添加新节点
                        targetNode = PrerenderPostfxNodeMapper.DeepCopy(sourceNode);
                        target.PostfxNodes.Add(targetNode);
                    }
                    else
                    {
                        // 合并现有节点
                        PrerenderPostfxNodeMapper.Merge(targetNode, sourceNode);
                    }
                }
            }

            // 更新运行时标记
            target.HasEmptyPostfxNodes = target.PostfxNodes.Count == 0;

            return target;
        }
    }

    /// <summary>
    /// PrerenderPostfxNode的映射器
    /// </summary>
    public static class PrerenderPostfxNodeMapper
    {
        public static PrerenderPostfxNodeDTO ToDTO(PrerenderPostfxNodeDO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxNodeDTO
            {
                Id = source.Id,
                Class = source.Class,
                Shader = source.Shader,
                Format = source.Format,
                Size = source.Size,
                Width = source.Width,
                Height = source.Height,
                Compute = source.Compute,
                ComputeTgSizeX = source.ComputeTgSizeX,
                ComputeTgSizeY = source.ComputeTgSizeY,
                Inputs = source.Inputs?
                    .Select(InputMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<InputDTO>(),
                Preconditions = PreconditionsMapper.ToDTO(source.Preconditions)
            };
        }

        public static PrerenderPostfxNodeDO ToDO(PrerenderPostfxNodeDTO source)
        {
            if (source == null) return null;

            return new PrerenderPostfxNodeDO
            {
                Id = source.Id,
                Class = source.Class,
                Shader = source.Shader,
                Format = source.Format,
                Size = source.Size,
                Width = source.Width,
                Height = source.Height,
                Compute = source.Compute,
                ComputeTgSizeX = source.ComputeTgSizeX,
                ComputeTgSizeY = source.ComputeTgSizeY,
                Inputs = source.Inputs?
                    .Select(InputMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<InputDO>(),
                Preconditions = PreconditionsMapper.ToDO(source.Preconditions),
                HasEmptyInputs = source.Inputs == null || source.Inputs.Count == 0,
                HasPreconditions = source.Preconditions != null && source.Preconditions.Configs.Count > 0
            };
        }

        public static PrerenderPostfxNodeDO DeepCopy(PrerenderPostfxNodeDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        public static PrerenderPostfxNodeDO Merge(PrerenderPostfxNodeDO target, PrerenderPostfxNodeDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 合并所有属性（source覆盖target）
            if (!string.IsNullOrEmpty(source.Id)) target.Id = source.Id;
            if (!string.IsNullOrEmpty(source.Class)) target.Class = source.Class;
            if (!string.IsNullOrEmpty(source.Shader)) target.Shader = source.Shader;
            if (!string.IsNullOrEmpty(source.Format)) target.Format = source.Format;
            if (!string.IsNullOrEmpty(source.Size)) target.Size = source.Size;
            if (!string.IsNullOrEmpty(source.Width)) target.Width = source.Width;
            if (!string.IsNullOrEmpty(source.Height)) target.Height = source.Height;
            if (!string.IsNullOrEmpty(source.Compute)) target.Compute = source.Compute;
            if (!string.IsNullOrEmpty(source.ComputeTgSizeX)) target.ComputeTgSizeX = source.ComputeTgSizeX;
            if (!string.IsNullOrEmpty(source.ComputeTgSizeY)) target.ComputeTgSizeY = source.ComputeTgSizeY;

            // 合并输入列表
            if (source.Inputs != null)
            {
                if (target.Inputs == null)
                {
                    target.Inputs = new List<InputDO>();
                }

                // 清空并重新添加（因为输入列表通常较小且需要保持顺序）
                target.Inputs.Clear();
                foreach (var input in source.Inputs)
                {
                    target.Inputs.Add(InputMapper.DeepCopy(input));
                }
            }

            // 合并预条件
            if (source.Preconditions != null)
            {
                target.Preconditions = PreconditionsMapper.DeepCopy(source.Preconditions);
            }

            // 更新运行时标记
            target.HasEmptyInputs = target.Inputs == null || target.Inputs.Count == 0;
            target.HasPreconditions = target.Preconditions != null && target.Preconditions.Configs.Count > 0;

            return target;
        }
    }

    /// <summary>
    /// Input的映射器
    /// </summary>
    public static class InputMapper
    {
        public static InputDTO ToDTO(InputDO source)
        {
            if (source == null) return null;

            return new InputDTO
            {
                Index = source.Index,
                Type = source.Type,
                Source = source.Source
            };
        }

        public static InputDO ToDO(InputDTO source)
        {
            if (source == null) return null;

            return new InputDO
            {
                Index = source.Index,
                Type = source.Type,
                Source = source.Source
            };
        }

        public static InputDO DeepCopy(InputDO source)
        {
            if (source == null) return null;

            return new InputDO
            {
                Index = source.Index,
                Type = source.Type,
                Source = source.Source
            };
        }
    }

    /// <summary>
    /// Preconditions的映射器
    /// </summary>
    internal static class PreconditionsMapper
    {
        public static PreconditionsDTO ToDTO(PreconditionsDO source)
        {
            if (source == null) return null;

            return new PreconditionsDTO
            {
                Configs = source.Configs?
                    .Select(ConfigMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<ConfigDTO>()
            };
        }

        public static PreconditionsDO ToDO(PreconditionsDTO source)
        {
            if (source == null) return null;

            return new PreconditionsDO
            {
                Configs = source.Configs?
                    .Select(ConfigMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<ConfigDO>()
            };
        }

        public static PreconditionsDO DeepCopy(PreconditionsDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }
    }

    /// <summary>
    /// Config的映射器
    /// </summary>
    internal static class ConfigMapper
    {
        public static ConfigDTO ToDTO(ConfigDO source)
        {
            if (source == null) return null;

            return new ConfigDTO
            {
                Name = source.Name
            };
        }

        public static ConfigDO ToDO(ConfigDTO source)
        {
            if (source == null) return null;

            return new ConfigDO
            {
                Name = source.Name
            };
        }

        public static ConfigDO DeepCopy(ConfigDO source)
        {
            if (source == null) return null;

            return new ConfigDO
            {
                Name = source.Name
            };
        }
    }
}