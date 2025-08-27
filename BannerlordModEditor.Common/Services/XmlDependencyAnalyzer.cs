using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// XML文件依赖关系分析器
    /// 基于Mount & Blade源码分析，实现XML文件之间的依赖关系检测和验证
    /// </summary>
    public class XmlDependencyAnalyzer
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        
        // 预定义的依赖关系规则（基于mountblade-code分析）
        private readonly Dictionary<string, List<string>> _predefinedDependencies = new()
        {
            // 基础层依赖
            { "managed_core_parameters", new List<string> { "physics_materials" } },
            { "managed_campaign_parameters", new List<string> { "managed_core_parameters" } },
            { "native_parameters", new List<string> { "managed_campaign_parameters" } },
            
            // 制作系统依赖链
            { "crafting_templates", new List<string> { "skills", "item_modifiers_groups" } },
            { "crafting_pieces", new List<string> { "crafting_templates", "item_usage_sets" } },
            { "items", new List<string> { "crafting_pieces", "item_modifiers", "item_usage_sets" } },
            
            // 角色系统依赖
            { "bone_body_types", new List<string> { "skeletons" } },
            { "faces", new List<string> { "bone_body_types" } },
            { "characters", new List<string> { "bone_body_types", "faces", "attributes", "skills" } },
            
            // 动作系统依赖
            { "action_types", new List<string> { "item_usage_sets", "combat_parameters" } },
            { "action_sets", new List<string> { "action_types", "animations" } },
            { "full_movement_sets", new List<string> { "action_sets" } },
            { "movement_sets", new List<string> { "full_movement_sets" } },
            
            // 装备系统依赖
            { "item_holsters", new List<string> { "items", "skeletons" } },
            { "equipment_sets", new List<string> { "items", "item_modifiers" } },
            
            // 多人游戏依赖
            { "mpcharacters", new List<string> { "characters", "mpcultures", "mpbadges" } },
            { "mpitems", new List<string> { "items", "mpclassdivisions" } },
            { "mpcultures", new List<string> { "cultures" } },
            
            // 战斗系统依赖
            { "combat_parameters", new List<string> { "physics_materials", "collision_infos" } },
            { "collision_infos", new List<string> { "physics_materials" } },
            
            // 粒子系统依赖
            { "particle_systems", new List<string> { "physics_materials", "textures" } },
            { "gpu_particle_systems", new List<string> { "particle_systems" } },
            
            // 声音系统依赖
            { "module_sounds", new List<string> { "soundfiles" } },
            { "voice_definitions", new List<string> { "module_sounds" } },
            
            // UI系统依赖
            { "looknfeel", new List<string> { "managed_core_parameters" } },
            
            // 地图系统依赖
            { "map_icons", new List<string> { "map_tree_types", "flora_kinds" } },
            { "scenes", new List<string> { "map_icons", "atmospheres" } }
        };

        public XmlDependencyAnalyzer(
            IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
        }

        /// <summary>
        /// 分析指定目录中所有XML文件的依赖关系
        /// </summary>
        public XmlDependencyAnalysisResult AnalyzeDependencies(string moduleDataPath)
        {
            var result = new XmlDependencyAnalysisResult();
            
            try
            {
                // 获取所有XML文件
                var xmlFiles = _fileDiscoveryService.GetAllXmlFiles(moduleDataPath);
                result.TotalFiles = xmlFiles.Count;
                
                // 分析每个文件的依赖关系
                foreach (var xmlFile in xmlFiles)
                {
                    var fileResult = AnalyzeFileDependencies(xmlFile, moduleDataPath);
                    result.FileResults.Add(fileResult);
                }
                
                // 检测循环依赖
                result.CircularDependencies = DetectCircularDependencies(result.FileResults);
                
                // 计算加载顺序
                result.LoadOrder = CalculateLoadOrder(result.FileResults);
                
                result.IsValid = result.CircularDependencies.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"分析过程中发生错误: {ex.Message}");
                result.IsValid = false;
            }
            
            return result;
        }

        /// <summary>
        /// 分析单个XML文件的依赖关系
        /// </summary>
        private XmlFileDependencyResult AnalyzeFileDependencies(string xmlFilePath, string moduleDataPath)
        {
            var result = new XmlFileDependencyResult
            {
                FilePath = xmlFilePath,
                FileName = System.IO.Path.GetFileName(xmlFilePath)
            };
            
            try
            {
                // 获取文件的基础名称（不含扩展名）
                var fileBaseName = System.IO.Path.GetFileNameWithoutExtension(xmlFilePath);
                
                // 1. 检查预定义依赖关系
                if (_predefinedDependencies.TryGetValue(fileBaseName, out var predefinedDeps))
                {
                    result.PredefinedDependencies.AddRange(predefinedDeps);
                }
                
                // 2. 分析XML内容中的引用依赖
                var contentDependencies = AnalyzeContentReferences(xmlFilePath);
                result.ContentDependencies.AddRange(contentDependencies);
                
                // 3. 分析Schema依赖
                var schemaDependencies = AnalyzeSchemaReferences(xmlFilePath);
                result.SchemaDependencies.AddRange(schemaDependencies);
                
                // 4. 检查依赖文件是否存在
                CheckDependencyExistence(result, moduleDataPath);
                
                // 5. 合并所有依赖关系
                result.AllDependencies = result.PredefinedDependencies
                    .Union(result.ContentDependencies)
                    .Union(result.SchemaDependencies)
                    .Distinct()
                    .ToList();
                
                result.IsValid = result.MissingDependencies.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"分析文件依赖时发生错误: {ex.Message}");
                result.IsValid = false;
            }
            
            return result;
        }

        /// <summary>
        /// 分析XML内容中的引用依赖
        /// </summary>
        private List<string> AnalyzeContentReferences(string xmlFilePath)
        {
            var dependencies = new List<string>();
            
            try
            {
                var doc = XDocument.Load(xmlFilePath);
                
                // 分析常见的引用模式
                var referencePatterns = new[]
                {
                    // ID引用模式
                    "//@id",
                    "//@ref",
                    "//@reference",
                    "//@target",
                    
                    // 对象引用模式
                    "//@item",
                    "//@character",
                    "//@culture",
                    "//@faction",
                    "//@item_modifier",
                    "//@skill",
                    "//@attribute",
                    "//@template",
                    "//@sound",
                    "//@mesh",
                    "//@texture",
                    "//@material",
                    
                    // 特定元素的引用
                    "//ItemUsageSet",
                    "//CraftingPiece",
                    "//ItemModifier",
                    "//SkillObject",
                    "//CharacterObject",
                    "//CultureObject",
                    "//BannerEffect",
                    "//PhysicsMaterial",
                    "//SoundEvent",
                    "//Mesh",
                    "//Texture"
                };
                
                foreach (var pattern in referencePatterns)
                {
                    IEnumerable<string> references;
                    try
                    {
                        references = doc.XPathSelectElements(pattern)
                            .Select(e => e.Value)
                            .Where(v => !string.IsNullOrEmpty(v))
                            .Distinct();
                    }
                    catch
                    {
                        // 如果XPathSelectElements失败，尝试其他方法
                        references = new List<string>();
                    }
                    
                    foreach (var reference in references)
                    {
                        // 从引用中提取文件名
                        var dependency = ExtractDependencyFromReference(reference);
                        if (dependency != null && !dependencies.Contains(dependency))
                        {
                            dependencies.Add(dependency);
                        }
                    }
                }
                
                // 分析特定类型的引用关系
                AnalyzeSpecificReferences(doc, dependencies);
            }
            catch (Exception ex)
            {
                // 记录错误但不中断流程
                Console.WriteLine($"分析XML内容引用时发生错误: {ex.Message}");
            }
            
            return dependencies;
        }

        /// <summary>
        /// 分析特定类型的引用关系
        /// </summary>
        private void AnalyzeSpecificReferences(XDocument doc, List<string> dependencies)
        {
            // 分析制作相关引用
            var craftingPieces = doc.Descendants("CraftingPiece")
                .Select(e => e.Attribute("id")?.Value)
                .Where(v => !string.IsNullOrEmpty(v));
            
            if (craftingPieces.Any())
            {
                dependencies.Add("crafting_pieces");
            }
            
            // 分析物品相关引用
            var items = doc.Descendants("Item")
                .Select(e => e.Attribute("id")?.Value)
                .Where(v => !string.IsNullOrEmpty(v));
            
            if (items.Any())
            {
                dependencies.Add("items");
            }
            
            // 分析角色相关引用
            var characters = doc.Descendants("Character")
                .Select(e => e.Attribute("id")?.Value)
                .Where(v => !string.IsNullOrEmpty(v));
            
            if (characters.Any())
            {
                dependencies.Add("characters");
            }
            
            // 分析文化相关引用
            var cultures = doc.Descendants("Culture")
                .Select(e => e.Attribute("id")?.Value)
                .Where(v => !string.IsNullOrEmpty(v));
            
            if (cultures.Any())
            {
                dependencies.Add("cultures");
            }
            
            // 分析技能相关引用
            var skills = doc.Descendants("Skill")
                .Select(e => e.Attribute("id")?.Value)
                .Where(v => !string.IsNullOrEmpty(v));
            
            if (skills.Any())
            {
                dependencies.Add("skills");
            }
        }

        /// <summary>
        /// 从引用字符串中提取依赖文件名
        /// </summary>
        private string ExtractDependencyFromReference(string reference)
        {
            if (string.IsNullOrEmpty(reference))
                return null;
            
            // 处理命名空间引用格式：namespace.object
            if (reference.Contains('.'))
            {
                var parts = reference.Split('.');
                if (parts.Length == 2)
                {
                    // 根据引用类型推断依赖文件
                    return InferDependencyFromReference(parts[0], parts[1]);
                }
            }
            
            // 处理直接ID引用
            return InferDependencyFromReference(reference);
        }

        /// <summary>
        /// 根据引用推断依赖文件
        /// </summary>
        private string InferDependencyFromReference(string typeHint, string objectId)
        {
            var typeLower = typeHint.ToLower();
            
            // 基于mountblade-code分析的引用映射
            var referenceMap = new Dictionary<string, string>
            {
                // 基础对象
                { "item", "items" },
                { "character", "characters" },
                { "culture", "cultures" },
                { "faction", "factions" },
                { "clan", "clans" },
                { "kingdom", "kingdoms" },
                
                // 游戏对象
                { "skill", "skills" },
                { "attribute", "attributes" },
                { "trait", "traits" },
                { "perk", "perks" },
                
                // 装备相关
                { "itemmodifier", "item_modifiers" },
                { "itemmodifiergroup", "item_modifiers_groups" },
                { "craftingpiece", "crafting_pieces" },
                { "craftingtemplate", "crafting_templates" },
                { "itemusage", "item_usage_sets" },
                { "equipment", "equipment_sets" },
                
                // 战斗相关
                { "combatparameter", "combat_parameters" },
                { "collisioninfo", "collision_infos" },
                { "actiontype", "action_types" },
                { "actionset", "action_sets" },
                { "movementset", "movement_sets" },
                
                // 视觉效果
                { "mesh", "meshes" },
                { "texture", "textures" },
                { "material", "materials" },
                { "particle", "particle_systems" },
                { "sound", "soundfiles" },
                { "voice", "voice_definitions" },
                
                // 地图相关
                { "mapicon", "map_icons" },
                { "scene", "scenes" },
                { "flora", "flora_kinds" },
                { "terrain", "terrain_materials" },
                
                // UI相关
                { "banner", "banners" },
                { "bannericon", "banner_icons" },
                { "looknfeel", "looknfeel" }
            };
            
            if (referenceMap.TryGetValue(typeLower, out var dependency))
            {
                return dependency;
            }
            
            // 如果没有匹配的类型映射，尝试根据对象ID推断
            return InferDependencyFromReference(objectId);
        }

        /// <summary>
        /// 根据对象ID推断依赖文件
        /// </summary>
        private string InferDependencyFromReference(string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
                return null;
            
            // 基于常见的ID前缀推断
            var idLower = objectId.ToLower();
            
            if (idLower.StartsWith("item_")) return "items";
            if (idLower.StartsWith("char_")) return "characters";
            if (idLower.StartsWith("cult_")) return "cultures";
            if (idLower.StartsWith("skill_")) return "skills";
            if (idLower.StartsWith("attr_")) return "attributes";
            if (idLower.StartsWith("craft_")) return "crafting_pieces";
            if (idLower.StartsWith("modifier_")) return "item_modifiers";
            if (idLower.StartsWith("action_")) return "action_types";
            if (idLower.StartsWith("combat_")) return "combat_parameters";
            if (idLower.StartsWith("sound_")) return "soundfiles";
            if (idLower.StartsWith("mesh_")) return "meshes";
            if (idLower.StartsWith("texture_")) return "textures";
            if (idLower.StartsWith("material_")) return "materials";
            if (idLower.StartsWith("particle_")) return "particle_systems";
            if (idLower.StartsWith("banner_")) return "banner_icons";
            if (idLower.StartsWith("map_")) return "map_icons";
            if (idLower.StartsWith("scene_")) return "scenes";
            
            return null;
        }

        /// <summary>
        /// 分析Schema引用
        /// </summary>
        private List<string> AnalyzeSchemaReferences(string xmlFilePath)
        {
            var dependencies = new List<string>();
            
            try
            {
                var doc = XDocument.Load(xmlFilePath);
                
                // 查找schema引用
                var schemaLocation = doc.Root?.Attribute("xsi:schemaLocation")?.Value;
                if (!string.IsNullOrEmpty(schemaLocation))
                {
                    // 从schema location中提取依赖信息
                    var schemaFiles = schemaLocation.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var schemaFile in schemaFiles)
                    {
                        if (schemaFile.EndsWith(".xsd"))
                        {
                            var schemaName = System.IO.Path.GetFileNameWithoutExtension(schemaFile);
                            dependencies.Add(schemaName);
                        }
                    }
                }
                
                // 查找noNamespaceSchemaLocation
                var noNamespaceSchema = doc.Root?.Attribute("xsi:noNamespaceSchemaLocation")?.Value;
                if (!string.IsNullOrEmpty(noNamespaceSchema))
                {
                    var schemaName = System.IO.Path.GetFileNameWithoutExtension(noNamespaceSchema);
                    dependencies.Add(schemaName);
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不中断流程
                Console.WriteLine($"分析Schema引用时发生错误: {ex.Message}");
            }
            
            return dependencies;
        }

        /// <summary>
        /// 检查依赖文件是否存在
        /// </summary>
        private void CheckDependencyExistence(XmlFileDependencyResult result, string moduleDataPath)
        {
            foreach (var dependency in result.AllDependencies)
            {
                var expectedPath = System.IO.Path.Combine(moduleDataPath, $"{dependency}.xml");
                if (!System.IO.File.Exists(expectedPath))
                {
                    result.MissingDependencies.Add(dependency);
                }
            }
        }

        /// <summary>
        /// 检测循环依赖
        /// </summary>
        private List<CircularDependencyInfo> DetectCircularDependencies(List<XmlFileDependencyResult> fileResults)
        {
            var circularDependencies = new List<CircularDependencyInfo>();
            
            // 构建依赖图
            var graph = new Dictionary<string, List<string>>();
            foreach (var result in fileResults)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(result.FileName);
                graph[fileName] = result.AllDependencies;
            }
            
            // 使用深度优先搜索检测循环依赖
            var visited = new HashSet<string>();
            var recursionStack = new HashSet<string>();
            
            foreach (var node in graph.Keys)
            {
                if (!visited.Contains(node))
                {
                    var cycle = new List<string>();
                    if (HasCycle(graph, node, visited, recursionStack, cycle))
                    {
                        circularDependencies.Add(new CircularDependencyInfo
                        {
                            Cycle = cycle,
                            Description = $"检测到循环依赖: {string.Join(" -> ", cycle)} -> {cycle[0]}"
                        });
                    }
                }
            }
            
            return circularDependencies;
        }

        /// <summary>
        /// 深度优先搜索检测循环依赖
        /// </summary>
        private bool HasCycle(Dictionary<string, List<string>> graph, string node, 
            HashSet<string> visited, HashSet<string> recursionStack, List<string> cycle)
        {
            visited.Add(node);
            recursionStack.Add(node);
            cycle.Add(node);
            
            if (graph.TryGetValue(node, out var dependencies))
            {
                foreach (var dependency in dependencies)
                {
                    if (!visited.Contains(dependency))
                    {
                        if (HasCycle(graph, dependency, visited, recursionStack, cycle))
                            return true;
                    }
                    else if (recursionStack.Contains(dependency))
                    {
                        // 找到循环，构建循环路径
                        var cycleStart = cycle.IndexOf(dependency);
                        var actualCycle = cycle.GetRange(cycleStart, cycle.Count - cycleStart);
                        cycle.Clear();
                        cycle.AddRange(actualCycle);
                        return true;
                    }
                }
            }
            
            recursionStack.Remove(node);
            cycle.RemoveAt(cycle.Count - 1);
            return false;
        }

        /// <summary>
        /// 计算加载顺序
        /// </summary>
        private List<string> CalculateLoadOrder(List<XmlFileDependencyResult> fileResults)
        {
            var loadOrder = new List<string>();
            var graph = new Dictionary<string, List<string>>();
            var inDegree = new Dictionary<string, int>();
            
            // 构建图和入度
            foreach (var result in fileResults)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(result.FileName);
                graph[fileName] = result.AllDependencies;
                inDegree[fileName] = 0;
            }
            
            // 计算入度
            foreach (var node in graph.Keys)
            {
                foreach (var dependency in graph[node])
                {
                    if (inDegree.ContainsKey(dependency))
                    {
                        inDegree[dependency]++;
                    }
                }
            }
            
            // 拓扑排序
            var queue = new Queue<string>(inDegree.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key));
            
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                loadOrder.Add(node);
                
                foreach (var dependency in graph[node])
                {
                    if (inDegree.ContainsKey(dependency))
                    {
                        inDegree[dependency]--;
                        if (inDegree[dependency] == 0)
                        {
                            queue.Enqueue(dependency);
                        }
                    }
                }
            }
            
            return loadOrder;
        }

        /// <summary>
        /// 获取推荐的加载顺序
        /// </summary>
        public List<string> GetRecommendedLoadOrder(string moduleDataPath)
        {
            var dependencyResult = AnalyzeDependencies(moduleDataPath);
            return dependencyResult.LoadOrder;
        }

        /// <summary>
        /// 获取依赖关系图
        /// </summary>
        public Dictionary<string, List<string>> GetDependencyGraph(string moduleDataPath)
        {
            var dependencyResult = AnalyzeDependencies(moduleDataPath);
            var graph = new Dictionary<string, List<string>>();
            
            foreach (var fileResult in dependencyResult.FileResults)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileResult.FileName);
                graph[fileName] = fileResult.AllDependencies;
            }
            
            return graph;
        }
    }

    /// <summary>
    /// XML依赖分析结果
    /// </summary>
    public class XmlDependencyAnalysisResult
    {
        public int TotalFiles { get; set; }
        public List<XmlFileDependencyResult> FileResults { get; set; } = new();
        public List<CircularDependencyInfo> CircularDependencies { get; set; } = new();
        public List<string> LoadOrder { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 单个XML文件的依赖分析结果
    /// </summary>
    public class XmlFileDependencyResult
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public List<string> PredefinedDependencies { get; set; } = new();
        public List<string> ContentDependencies { get; set; } = new();
        public List<string> SchemaDependencies { get; set; } = new();
        public List<string> AllDependencies { get; set; } = new();
        public List<string> MissingDependencies { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 循环依赖信息
    /// </summary>
    public class CircularDependencyInfo
    {
        public List<string> Cycle { get; set; } = new();
        public string Description { get; set; }
    }
}