using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace BannerlordModEditor.Common.Validation.DependencyAnalysis
{
    /// <summary>
    /// XML文件依赖关系分析器
    /// 基于MBObjectManager的依赖机制实现
    /// </summary>
    public interface IXmlDependencyAnalyzer
    {
        Task<DependencyAnalysisResult> AnalyzeDependenciesAsync(XmlDocument xmlDocument, string xmlPath);
        Task<LoadingOrder> DetermineLoadingOrderAsync(IEnumerable<string> xmlFiles);
        IEnumerable<DependencyEdge> FindCircularDependencies(IEnumerable<string> xmlFiles);
    }

    /// <summary>
    /// 依赖分析结果
    /// </summary>
    public class DependencyAnalysisResult
    {
        public string XmlPath { get; set; }
        public List<XmlDependency> Dependencies { get; set; } = new();
        public List<XmlDependency> Dependents { get; set; } = new();
        public Dictionary<string, List<ReferenceInfo>> ObjectReferences { get; set; } = new();
        public bool HasCircularDependencies { get; set; }
        public AnalysisComplexity Complexity { get; set; }
    }

    /// <summary>
    /// XML依赖关系
    /// </summary>
    public class XmlDependency
    {
        public string SourceXml { get; set; }
        public string TargetXml { get; set; }
        public DependencyType Type { get; set; }
        public ReferenceStrength Strength { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 引用信息
    /// </summary>
    public class ReferenceInfo
    {
        public string ReferencedType { get; set; }
        public string ReferencedObject { get; set; }
        public string ReferenceSource { get; set; }
        public ReferenceCategory Category { get; set; }
        public bool IsRequired { get; set; }
    }

    /// <summary>
    /// 依赖类型枚举
    /// </summary>
    public enum DependencyType
    {
        ObjectReference,    // 对象引用
        TypeInheritance,     // 类型继承
        ResourceDependency, // 资源依赖
        SchemaImport,       // Schema导入
        LogicalDependency   // 逻辑依赖
    }

    /// <summary>
    /// 引用强度
    /// </summary>
    public enum ReferenceStrength
    {
        Weak,      // 弱引用，可选
        Strong,    // 强引用，必需
        Critical   // 关键引用，系统核心
    }

    /// <summary>
    /// 引用类别
    /// </summary>
    public enum ReferenceCategory
    {
        DirectReference,    // 直接引用
        IndirectReference,  // 间接引用
        CollectionReference, // 集合引用
        PolymorphicReference // 多态引用
    }

    /// <summary>
    /// 分析复杂度
    /// </summary>
    public enum AnalysisComplexity
    {
        Simple,     // 简单依赖
        Moderate,   // 中等复杂度
        Complex,    // 复杂依赖
        VeryComplex // 非常复杂
    }

    /// <summary>
    /// 加载顺序
    /// </summary>
    public class LoadingOrder
    {
        public List<LoadingPhase> Phases { get; set; } = new();
        public List<string> ParallelLoadableGroups { get; set; } = new();
        public List<string> CircularDependencyWarnings { get; set; } = new();
    }

    /// <summary>
    /// 加载阶段
    /// </summary>
    public class LoadingPhase
    {
        public int Phase { get; set; }
        public List<string> XmlFiles { get; set; } = new();
        public string Description { get; set; }
        public bool CanLoadInParallel { get; set; }
    }

    /// <summary>
    /// 依赖边（用于图分析）
    /// </summary>
    public class DependencyEdge
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public DependencyType Type { get; set; }
        public List<string> Path { get; set; } = new();
    }

    /// <summary>
    /// 基于MBObjectManager的依赖分析器实现
    /// </summary>
    public class MbObjectDependencyAnalyzer : IXmlDependencyAnalyzer
    {
        private readonly IXmlTypeResolver _typeResolver;
        private readonly IReferenceExtractor _referenceExtractor;

        public MbObjectDependencyAnalyzer(
            IXmlTypeResolver typeResolver,
            IReferenceExtractor referenceExtractor)
        {
            _typeResolver = typeResolver;
            _referenceExtractor = referenceExtractor;
        }

        public async Task<DependencyAnalysisResult> AnalyzeDependenciesAsync(XmlDocument xmlDocument, string xmlPath)
        {
            var result = new DependencyAnalysisResult { XmlPath = xmlPath };
            
            // 识别XML类型
            var dataType = await _typeResolver.ResolveDataTypeAsync(xmlDocument, xmlPath);
            
            // 提取对象引用
            var references = await _referenceExtractor.ExtractReferencesAsync(xmlDocument, dataType);
            
            // 分析依赖关系
            foreach (var reference in references)
            {
                var dependency = new XmlDependency
                {
                    SourceXml = xmlPath,
                    TargetXml = reference.TargetXml,
                    Type = DetermineDependencyType(reference),
                    Strength = DetermineReferenceStrength(reference),
                    Description = $"Reference to {reference.ReferencedType}.{reference.ReferencedObject}"
                };
                
                result.Dependencies.Add(dependency);
                
                // 记录对象引用信息
                if (!result.ObjectReferences.ContainsKey(reference.TargetXml))
                {
                    result.ObjectReferences[reference.TargetXml] = new List<ReferenceInfo>();
                }
                
                result.ObjectReferences[reference.TargetXml].Add(new ReferenceInfo
                {
                    ReferencedType = reference.ReferencedType,
                    ReferencedObject = reference.ReferencedObject,
                    ReferenceSource = reference.SourceNode,
                    Category = DetermineReferenceCategory(reference),
                    IsRequired = reference.IsRequired
                });
            }
            
            // 计算复杂度
            result.Complexity = CalculateComplexity(result);
            
            return result;
        }

        public async Task<LoadingOrder> DetermineLoadingOrderAsync(IEnumerable<string> xmlFiles)
        {
            var dependencyGraph = new Dictionary<string, List<string>>();
            var allFiles = xmlFiles.ToList();
            
            // 构建依赖图
            foreach (var xmlFile in allFiles)
            {
                dependencyGraph[xmlFile] = new List<string>();
                
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(xmlFile);
                    var analysis = await AnalyzeDependenciesAsync(doc, xmlFile);
                    
                    foreach (var dependency in analysis.Dependencies)
                    {
                        if (allFiles.Contains(dependency.TargetXml))
                        {
                            dependencyGraph[xmlFile].Add(dependency.TargetXml);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理
                    System.Diagnostics.Debug.WriteLine($"Error analyzing {xmlFile}: {ex.Message}");
                }
            }
            
            // 拓扑排序确定加载顺序
            return TopologicalSort(dependencyGraph);
        }

        public IEnumerable<DependencyEdge> FindCircularDependencies(IEnumerable<string> xmlFiles)
        {
            var dependencyGraph = new Dictionary<string, List<string>>();
            var allFiles = xmlFiles.ToList();
            
            // 构建依赖图
            foreach (var xmlFile in allFiles)
            {
                dependencyGraph[xmlFile] = new List<string>();
                
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(xmlFile);
                    var analysis = AnalyzeDependenciesAsync(doc, xmlFile).Result;
                    
                    foreach (var dependency in analysis.Dependencies)
                    {
                        if (allFiles.Contains(dependency.TargetXml))
                        {
                            dependencyGraph[xmlFile].Add(dependency.TargetXml);
                        }
                    }
                }
                catch
                {
                    // 忽略错误
                }
            }
            
            return FindCycles(dependencyGraph);
        }

        private DependencyType DetermineDependencyType(ExtractedReference reference)
        {
            return reference.ReferencedType switch
            {
                "CraftingPiece" => DependencyType.ObjectReference,
                "ItemObject" => DependencyType.ObjectReference,
                "PhysicsMaterial" => DependencyType.ResourceDependency,
                "ActionSet" => DependencyType.LogicalDependency,
                _ => DependencyType.ObjectReference
            };
        }

        private ReferenceStrength DetermineReferenceStrength(ExtractedReference reference)
        {
            return reference.IsRequired switch
            {
                true => ReferenceStrength.Strong,
                false => ReferenceStrength.Weak
            };
        }

        private ReferenceCategory DetermineReferenceCategory(ExtractedReference reference)
        {
            if (reference.IsCollection)
                return ReferenceCategory.CollectionReference;
            
            return reference.IsDirect switch
            {
                true => ReferenceCategory.DirectReference,
                false => ReferenceCategory.IndirectReference
            };
        }

        private AnalysisComplexity CalculateComplexity(DependencyAnalysisResult result)
        {
            var totalDependencies = result.Dependencies.Count;
            var totalReferences = result.ObjectReferences.Values.Sum(v => v.Count);
            
            if (totalDependencies > 50 || totalReferences > 100)
                return AnalysisComplexity.VeryComplex;
            
            if (totalDependencies > 20 || totalReferences > 50)
                return AnalysisComplexity.Complex;
            
            if (totalDependencies > 10 || totalReferences > 20)
                return AnalysisComplexity.Moderate;
            
            return AnalysisComplexity.Simple;
        }

        private LoadingOrder TopologicalSort(Dictionary<string, List<string>> graph)
        {
            var order = new LoadingOrder();
            var phase = 0;
            var visited = new HashSet<string>();
            var tempVisited = new HashSet<string>();
            
            var phases = new Dictionary<string, int>();
            
            foreach (var node in graph.Keys)
            {
                if (!visited.Contains(node))
                {
                    TopologicalSortVisit(node, graph, visited, tempVisited, phases, ref phase);
                }
            }
            
            // 按阶段分组
            var phaseGroups = phases.GroupBy(p => p.Value).OrderBy(g => g.Key);
            
            foreach (var group in phaseGroups)
            {
                order.Phases.Add(new LoadingPhase
                {
                    Phase = group.Key,
                    XmlFiles = group.Select(g => g.Key).ToList(),
                    Description = $"Phase {group.Key}",
                    CanLoadInParallel = group.Key > 0 // 非第一阶段的文件可以并行加载
                });
            }
            
            return order;
        }

        private void TopologicalSortVisit(string node, Dictionary<string, List<string>> graph, 
            HashSet<string> visited, HashSet<string> tempVisited, Dictionary<string, int> phases, ref int phase)
        {
            if (tempVisited.Contains(node))
            {
                // 检测到循环依赖
                return;
            }
            
            if (visited.Contains(node))
            {
                return;
            }
            
            tempVisited.Add(node);
            
            if (graph.ContainsKey(node))
            {
                var maxDependencyPhase = -1;
                
                foreach (var dependency in graph[node])
                {
                    TopologicalSortVisit(dependency, graph, visited, tempVisited, phases, ref phase);
                    
                    if (phases.ContainsKey(dependency))
                    {
                        maxDependencyPhase = Math.Max(maxDependencyPhase, phases[dependency]);
                    }
                }
                
                phases[node] = maxDependencyPhase + 1;
            }
            else
            {
                phases[node] = 0;
            }
            
            tempVisited.Remove(node);
            visited.Add(node);
        }

        private List<DependencyEdge> FindCycles(Dictionary<string, List<string>> graph)
        {
            var cycles = new List<DependencyEdge>();
            var visited = new HashSet<string>();
            var recursionStack = new HashSet<string>();
            var path = new List<string>();
            
            foreach (var node in graph.Keys)
            {
                if (!visited.Contains(node))
                {
                    FindCyclesVisit(node, graph, visited, recursionStack, path, cycles);
                }
            }
            
            return cycles;
        }

        private void FindCyclesVisit(string node, Dictionary<string, List<string>> graph, 
            HashSet<string> visited, HashSet<string> recursionStack, List<string> path, List<DependencyEdge> cycles)
        {
            visited.Add(node);
            recursionStack.Add(node);
            path.Add(node);
            
            if (graph.ContainsKey(node))
            {
                foreach (var neighbor in graph[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        FindCyclesVisit(neighbor, graph, visited, recursionStack, path, cycles);
                    }
                    else if (recursionStack.Contains(neighbor))
                    {
                        // 找到循环
                        var cyclePath = path.SkipWhile(p => p != neighbor).Append(neighbor).ToList();
                        for (int i = 0; i < cyclePath.Count - 1; i++)
                        {
                            cycles.Add(new DependencyEdge
                            {
                                Source = cyclePath[i],
                                Target = cyclePath[i + 1],
                                Type = DependencyType.ObjectReference,
                                Path = cyclePath.ToList()
                            });
                        }
                    }
                }
            }
            
            recursionStack.Remove(node);
            path.RemoveAt(path.Count - 1);
        }
    }

    /// <summary>
    /// 提取的引用信息
    /// </summary>
    public class ExtractedReference
    {
        public string ReferencedType { get; set; }
        public string ReferencedObject { get; set; }
        public string TargetXml { get; set; }
        public string SourceNode { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDirect { get; set; }
        public bool IsCollection { get; set; }
    }
}