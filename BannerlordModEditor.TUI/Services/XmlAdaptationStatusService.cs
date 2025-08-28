using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// XML适配状态检查服务
    /// 用于检查哪些XML文件已完成Excel互转适配
    /// </summary>
    public class XmlAdaptationStatusService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, AdaptationStatus> _adaptationStatus;

        public XmlAdaptationStatusService(
            IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _adaptationStatus = InitializeAdaptationStatus();
        }

        /// <summary>
        /// 获取XML适配状态报告
        /// </summary>
        /// <returns>适配状态报告</returns>
        public async Task<AdaptationStatusReport> GetAdaptationStatusReportAsync()
        {
            var report = new AdaptationStatusReport();

            try
            {
                // 获取所有支持的XML类型（简化实现）
                var supportedTypes = GetSupportedXmlTypes().Select(t => new BannerlordModEditor.TUI.Models.XmlTypeInfo
                {
                    XmlType = t.XmlType,
                    IsSupported = t.IsSupported,
                    IsAdapted = t.IsAdapted,
                    Description = t.Description,
                    SupportedOperations = t.SupportedOperations.Select(o => (BannerlordModEditor.TUI.Models.SupportedOperation)Enum.Parse(typeof(BannerlordModEditor.TUI.Models.SupportedOperation), o.ToString())).ToList()
                }).ToList();
                
                // 按适配状态分类
                report.AdaptedTypes = supportedTypes.Where(t => t.IsAdapted).ToList();
                report.UnadaptedTypes = supportedTypes.Where(t => !t.IsAdapted).ToList();
                report.PartiallyAdaptedTypes = GetPartiallyAdaptedTypes(supportedTypes);
                
                // 统计信息
                report.TotalTypes = supportedTypes.Count;
                report.AdaptedCount = report.AdaptedTypes.Count;
                report.UnadaptedCount = report.UnadaptedTypes.Count;
                report.PartiallyAdaptedCount = report.PartiallyAdaptedTypes.Count;
                report.AdaptationPercentage = report.TotalTypes > 0 ? 
                    (double)report.AdaptedCount / report.TotalTypes * 100 : 0;

                // 获取优先级建议
                report.PriorityRecommendations = GetPriorityRecommendations();

                // 检查实际XML文件
                await CheckActualXmlFilesAsync(report);
            }
            catch (Exception ex)
            {
                report.Errors.Add($"生成适配状态报告失败: {ex.Message}");
            }

            return report;
        }

        /// <summary>
        /// 检查特定XML类型的适配状态
        /// </summary>
        /// <param name="xmlType">XML类型名称</param>
        /// <returns>适配状态详情</returns>
        public async Task<XmlTypeAdaptationDetail> GetXmlTypeAdaptationDetailAsync(string xmlType)
        {
            var detail = new XmlTypeAdaptationDetail { XmlType = xmlType };

            try
            {
                // 检查是否在已知类型中（简化实现）
                var isSupported = IsXmlTypeSupported(xmlType);
                detail.IsSupported = isSupported;

                if (isSupported)
                {
                    var typeInfo = await GetXmlTypeInfoAsync(xmlType);
                    detail.TypeInfo = typeInfo;
                    detail.IsAdapted = typeInfo.IsAdapted;
                    
                    // 检查模型文件是否存在
                    detail.ModelFileExists = CheckModelFileExists(xmlType);
                    
                    // 检查测试文件是否存在
                    detail.TestFileExists = CheckTestFileExists(xmlType);
                    
                    // 获取适配复杂度
                    detail.AdaptationComplexity = GetAdaptationComplexity(xmlType);
                    
                    // 获取依赖关系
                    detail.Dependencies = GetDependencies(xmlType);
                }
                else
                {
                    detail.Errors.Add($"不支持的XML类型: {xmlType}");
                }
            }
            catch (Exception ex)
            {
                detail.Errors.Add($"检查XML类型适配状态失败: {ex.Message}");
            }

            return detail;
        }

        /// <summary>
        /// 获取适配建议
        /// </summary>
        /// <returns>适配建议列表</returns>
        public async Task<List<AdaptationSuggestion>> GetAdaptationSuggestionsAsync()
        {
            var suggestions = new List<AdaptationSuggestion>();

            try
            {
                var report = await GetAdaptationStatusReportAsync();
                
                // 为未适配的类型生成建议
                foreach (var unadaptedType in report.UnadaptedTypes)
                {
                    var suggestion = new AdaptationSuggestion
                    {
                        XmlType = unadaptedType.XmlType,
                        Priority = GetAdaptationPriority(unadaptedType.XmlType),
                        EstimatedEffort = GetEstimatedEffort(unadaptedType.XmlType),
                        Benefits = GetAdaptationBenefits(unadaptedType.XmlType),
                        Prerequisites = GetPrerequisites(unadaptedType.XmlType)
                    };
                    
                    suggestions.Add(suggestion);
                }
                
                // 按优先级排序
                suggestions = suggestions.OrderBy(s => s.Priority).ToList();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断流程
                Console.WriteLine($"获取适配建议失败: {ex.Message}");
            }

            return suggestions;
        }

        /// <summary>
        /// 检查是否所有XML类型都已适配完成
        /// </summary>
        /// <returns>是否完成适配</returns>
        public async Task<bool> IsAdaptationCompleteAsync()
        {
            try
            {
                var report = await GetAdaptationStatusReportAsync();
                return report.UnadaptedTypes.Count == 0 && report.PartiallyAdaptedTypes.Count == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取适配进度信息
        /// </summary>
        /// <returns>进度信息</returns>
        public async Task<AdaptationProgress> GetAdaptationProgressAsync()
        {
            var progress = new AdaptationProgress();

            try
            {
                var report = await GetAdaptationStatusReportAsync();
                
                progress.TotalTypes = report.TotalTypes;
                progress.AdaptedTypes = report.AdaptedCount;
                progress.UnadaptedTypes = report.UnadaptedCount;
                progress.PartiallyAdaptedTypes = report.PartiallyAdaptedCount;
                progress.PercentageComplete = report.AdaptationPercentage;
                
                // 计算预计完成时间
                progress.EstimatedCompletionDate = CalculateEstimatedCompletion(progress);
                
                // 获取最近适配的类型
                progress.RecentlyAdapted = GetRecentlyAdaptedTypes();
                
                // 获取下一步建议
                progress.NextSteps = GetNextAdaptationSteps(report);
            }
            catch (Exception ex)
            {
                progress.Errors.Add($"获取适配进度失败: {ex.Message}");
            }

            return progress;
        }

        #region 私有方法

        private Dictionary<string, AdaptationStatus> InitializeAdaptationStatus()
        {
            var status = new Dictionary<string, AdaptationStatus>();

            // 已完全适配的XML类型
            var fullyAdapted = new[]
            {
                "ActionTypes", "CombatParameters", "ItemModifiers", "CraftingPieces",
                "ItemHolsters", "ActionSets", "CollisionInfos", "BoneBodyTypes",
                "PhysicsMaterials", "ParticleSystems", "MapIcons", "FloraKinds",
                "Scenes", "Credits", "BannerIcons", "Skins", "ItemUsageSets",
                "MovementSets", "NativeStrings", "Items", "Characters", "Skills", 
                "Factions", "Locations"
            };

            // 部分适配的XML类型
            var partiallyAdapted = Array.Empty<string>();

            // 未适配的XML类型
            var unadapted = new[]
            {
                "BasicObjects", "BodyProperties", "CampaignBehaviors", "CraftingTemplates",
                "FaceGenCharacters", "GameText", "ItemCategories", "Materials",
                "Meshes", "Monsters", "MPPoseNames", "NPCCharacters", "PartyTemplates",
                "QuestTemplates", "SPCultures", "SubModule", "TableauMaterials",
                "TraitGroups", "Traits", "Villages", "WeaponComponents"
            };

            foreach (var type in fullyAdapted)
            {
                status[type] = AdaptationStatus.FullyAdapted;
            }

            foreach (var type in partiallyAdapted)
            {
                status[type] = AdaptationStatus.PartiallyAdapted;
            }

            foreach (var type in unadapted)
            {
                status[type] = AdaptationStatus.Unadapted;
            }

            return status;
        }

        private List<BannerlordModEditor.TUI.Models.XmlTypeInfo> GetPartiallyAdaptedTypes(List<BannerlordModEditor.TUI.Models.XmlTypeInfo> supportedTypes)
        {
            return supportedTypes.Where(t => 
            {
                var status = _adaptationStatus.TryGetValue(t.XmlType, out var adaptationStatus) 
                    ? adaptationStatus 
                    : AdaptationStatus.Unadapted;
                return status == AdaptationStatus.PartiallyAdapted;
            }).ToList();
        }

        private List<PriorityRecommendation> GetPriorityRecommendations()
        {
            return new List<PriorityRecommendation>
            {
                new PriorityRecommendation
                {
                    Priority = AdaptationPriority.High,
                    XmlTypes = new[] { "Items", "Characters", "Skills" },
                    Reason = "核心游戏内容，用户最常编辑的XML类型"
                },
                new PriorityRecommendation
                {
                    Priority = AdaptationPriority.Medium,
                    XmlTypes = new[] { "Factions", "Locations", "QuestTemplates" },
                    Reason = "游戏世界构建的重要元素"
                },
                new PriorityRecommendation
                {
                    Priority = AdaptationPriority.Low,
                    XmlTypes = new[] { "BodyProperties", "Materials", "Meshes" },
                    Reason = "技术性内容，使用频率较低"
                }
            };
        }

        private async Task CheckActualXmlFilesAsync(AdaptationStatusReport report)
        {
            try
            {
                // 搜索example目录中的XML文件
                var examplePath = Path.Combine(Directory.GetCurrentDirectory(), "example");
                if (Directory.Exists(examplePath))
                {
                    var xmlFiles = Directory.GetFiles(examplePath, "*.xml", SearchOption.AllDirectories);
                    
                    foreach (var xmlFile in xmlFiles)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(xmlFile);
                        var fileInfo = new FileInfo(xmlFile);
                        
                        var xmlFileInfo = new XmlFileInfo
                        {
                            FileName = fileName,
                            FilePath = xmlFile,
                            Size = fileInfo.Length,
                            LastModified = fileInfo.LastWriteTime
                        };
                        
                        // 检查适配状态
                        if (_adaptationStatus.TryGetValue(fileName, out var status))
                        {
                            xmlFileInfo.AdaptationStatus = status;
                        }
                        else
                        {
                            xmlFileInfo.AdaptationStatus = AdaptationStatus.Unknown;
                        }
                        
                        report.ActualXmlFiles.Add(xmlFileInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                report.Errors.Add($"检查实际XML文件失败: {ex.Message}");
            }
        }

        private async Task<BannerlordModEditor.TUI.Models.XmlTypeInfo> GetXmlTypeInfoAsync(string xmlType)
        {
            // 这里应该调用XmlTypeDetectionService获取类型信息
            // 为了简化，返回基本信息
            return await Task.FromResult(new BannerlordModEditor.TUI.Models.XmlTypeInfo
            {
                XmlType = xmlType,
                IsSupported = true,
                IsAdapted = _adaptationStatus.TryGetValue(xmlType, out var status) && 
                           (status == AdaptationStatus.FullyAdapted || status == AdaptationStatus.PartiallyAdapted),
                Description = $"{xmlType} XML配置文件"
            });
        }

        private bool CheckModelFileExists(string xmlType)
        {
            try
            {
                var modelPath = $"BannerlordModEditor.Common/Models/DO/{xmlType}DO.cs";
                return File.Exists(modelPath);
            }
            catch
            {
                return false;
            }
        }

        private bool CheckTestFileExists(string xmlType)
        {
            try
            {
                var testPath = $"BannerlordModEditor.Common.Tests/Models/DO/{xmlType}DOTests.cs";
                return File.Exists(testPath);
            }
            catch
            {
                return false;
            }
        }

        private AdaptationComplexity GetAdaptationComplexity(string xmlType)
        {
            // 根据XML类型的复杂度返回评级
            var complexTypes = new[] { "Items", "Characters", "Scenes" };
            var mediumTypes = new[] { "Skills", "Factions", "Locations" };
            
            if (complexTypes.Contains(xmlType))
                return AdaptationComplexity.High;
            else if (mediumTypes.Contains(xmlType))
                return AdaptationComplexity.Medium;
            else
                return AdaptationComplexity.Low;
        }

        private List<string> GetDependencies(string xmlType)
        {
            // 返回XML类型的依赖关系
            var dependencies = new Dictionary<string, List<string>>
            {
                ["Items"] = new List<string> { "ItemCategories", "Materials", "ItemUsageSets" },
                ["Characters"] = new List<string> { "Skills", "BodyProperties", "BasicObjects" },
                ["Scenes"] = new List<string> { "FloraKinds", "Materials", "Meshes" }
            };

            return dependencies.TryGetValue(xmlType, out var deps) ? deps : new List<string>();
        }

        private AdaptationPriority GetAdaptationPriority(string xmlType)
        {
            var highPriority = new[] { "Items", "Characters", "Skills" };
            var mediumPriority = new[] { "Factions", "Locations", "QuestTemplates" };
            
            if (highPriority.Contains(xmlType))
                return AdaptationPriority.High;
            else if (mediumPriority.Contains(xmlType))
                return AdaptationPriority.Medium;
            else
                return AdaptationPriority.Low;
        }

        private int GetEstimatedEffort(string xmlType)
        {
            var complexity = GetAdaptationComplexity(xmlType);
            
            return complexity switch
            {
                AdaptationComplexity.High => 16, // 2人天
                AdaptationComplexity.Medium => 8,  // 1人天
                AdaptationComplexity.Low => 4,     // 0.5人天
                _ => 4
            };
        }

        private List<string> GetAdaptationBenefits(string xmlType)
        {
            return new List<string>
            {
                $"支持{xmlType}的Excel互转功能",
                "提供更好的用户体验",
                "减少手动编辑错误",
                "提高工作效率"
            };
        }

        private List<string> GetPrerequisites(string xmlType)
        {
            var dependencies = GetDependencies(xmlType);
            if (dependencies.Any())
            {
                return new List<string> { $"需要先适配依赖的XML类型: {string.Join(", ", dependencies)}" };
            }
            return new List<string> { "无特殊前提条件" };
        }

        private DateTime? CalculateEstimatedCompletion(AdaptationProgress progress)
        {
            if (progress.UnadaptedTypes == 0)
                return DateTime.Now;

            // 假设每个未适配类型平均需要1天时间
            var estimatedDays = progress.UnadaptedTypes;
            return DateTime.Now.AddDays(estimatedDays);
        }

        private List<string> GetRecentlyAdaptedTypes()
        {
            return new List<string>
            {
                "NativeStrings", "MovementSets", "ItemUsageSets"
            };
        }

        private List<string> GetNextAdaptationSteps(AdaptationStatusReport report)
        {
            var steps = new List<string>();
            
            // 添加高优先级未适配类型
            var highPriorityUnadapted = report.UnadaptedTypes
                .Where(t => GetAdaptationPriority(t.XmlType) == AdaptationPriority.High)
                .Select(t => t.XmlType)
                .ToList();

            if (highPriorityUnadapted.Any())
            {
                steps.Add($"优先适配高优先级XML类型: {string.Join(", ", highPriorityUnadapted)}");
            }

            // 添加部分适配类型的完善建议
            if (report.PartiallyAdaptedTypes.Any())
            {
                steps.Add($"完善部分适配的XML类型: {string.Join(", ", report.PartiallyAdaptedTypes.Select(t => t.XmlType))}");
            }

            // 添加测试建议
            steps.Add("为所有适配的XML类型添加完整的单元测试");

            return steps;
        }

        #endregion

        #region 简化方法

        private List<SimpleXmlTypeInfo> GetSupportedXmlTypes()
        {
            var types = new List<SimpleXmlTypeInfo>();

            // 已完全适配的XML类型
            var fullyAdapted = new[]
            {
                "ActionTypes", "CombatParameters", "ItemModifiers", "CraftingPieces",
                "ItemHolsters", "ActionSets", "CollisionInfos", "BoneBodyTypes",
                "PhysicsMaterials", "ParticleSystems", "MapIcons", "FloraKinds",
                "Scenes", "Credits", "BannerIcons", "Skins", "ItemUsageSets",
                "MovementSets", "NativeStrings"
            };

            // 部分适配的XML类型
            var partiallyAdapted = new[]
            {
                "Items", "Characters", "Skills", "Factions", "Locations"
            };

            // 未适配的XML类型
            var unadapted = new[]
            {
                "BasicObjects", "BodyProperties", "CampaignBehaviors", "CraftingTemplates",
                "FaceGenCharacters", "GameText", "ItemCategories", "Materials",
                "Meshes", "Monsters", "MPPoseNames", "NPCCharacters", "PartyTemplates",
                "QuestTemplates", "SPCultures", "SubModule", "TableauMaterials",
                "TraitGroups", "Traits", "Villages", "WeaponComponents"
            };

            // 添加已适配的类型
            foreach (var type in fullyAdapted)
            {
                types.Add(new SimpleXmlTypeInfo
                {
                    XmlType = type,
                    IsSupported = true,
                    IsAdapted = true,
                    Description = $"{type} XML配置文件（已完全适配）"
                });
            }

            // 添加部分适配的类型
            foreach (var type in partiallyAdapted)
            {
                types.Add(new SimpleXmlTypeInfo
                {
                    XmlType = type,
                    IsSupported = true,
                    IsAdapted = true,
                    Description = $"{type} XML配置文件（部分适配）"
                });
            }

            // 添加未适配的类型
            foreach (var type in unadapted)
            {
                types.Add(new SimpleXmlTypeInfo
                {
                    XmlType = type,
                    IsSupported = true,
                    IsAdapted = false,
                    Description = $"{type} XML配置文件（未适配）"
                });
            }

            return types;
        }

        private bool IsXmlTypeSupported(string xmlType)
        {
            var supportedTypes = GetSupportedXmlTypes();
            return supportedTypes.Any(t => t.XmlType == xmlType);
        }

        #endregion
    }

    #region 数据模型

    /// <summary>
    /// 适配状态报告
    /// </summary>
    public class AdaptationStatusReport
    {
        public List<BannerlordModEditor.TUI.Models.XmlTypeInfo> AdaptedTypes { get; set; } = new();
        public List<BannerlordModEditor.TUI.Models.XmlTypeInfo> UnadaptedTypes { get; set; } = new();
        public List<BannerlordModEditor.TUI.Models.XmlTypeInfo> PartiallyAdaptedTypes { get; set; } = new();
        public List<XmlFileInfo> ActualXmlFiles { get; set; } = new();
        public List<PriorityRecommendation> PriorityRecommendations { get; set; } = new();
        public List<string> Errors { get; set; } = new();

        public int TotalTypes { get; set; }
        public int AdaptedCount { get; set; }
        public int UnadaptedCount { get; set; }
        public int PartiallyAdaptedCount { get; set; }
        public double AdaptationPercentage { get; set; }
    }

    /// <summary>
    /// XML类型适配详情
    /// </summary>
    public class XmlTypeAdaptationDetail
    {
        public string XmlType { get; set; } = string.Empty;
        public bool IsSupported { get; set; }
        public bool IsAdapted { get; set; }
        public BannerlordModEditor.TUI.Models.XmlTypeInfo? TypeInfo { get; set; }
        public bool ModelFileExists { get; set; }
        public bool TestFileExists { get; set; }
        public AdaptationComplexity AdaptationComplexity { get; set; }
        public List<string> Dependencies { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 适配建议
    /// </summary>
    public class AdaptationSuggestion
    {
        public string XmlType { get; set; } = string.Empty;
        public AdaptationPriority Priority { get; set; }
        public int EstimatedEffort { get; set; } // 小时
        public List<string> Benefits { get; set; } = new();
        public List<string> Prerequisites { get; set; } = new();
    }

    /// <summary>
    /// 适配进度
    /// </summary>
    public class AdaptationProgress
    {
        public int TotalTypes { get; set; }
        public int AdaptedTypes { get; set; }
        public int UnadaptedTypes { get; set; }
        public int PartiallyAdaptedTypes { get; set; }
        public double PercentageComplete { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public List<string> RecentlyAdapted { get; set; } = new();
        public List<string> NextSteps { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 优先级建议
    /// </summary>
    public class PriorityRecommendation
    {
        public AdaptationPriority Priority { get; set; }
        public string[] XmlTypes { get; set; } = Array.Empty<string>();
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// XML文件信息
    /// </summary>
    public class XmlFileInfo
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public AdaptationStatus AdaptationStatus { get; set; }
    }

    #endregion

    #region 简化的类型定义

    /// <summary>
    /// 简化的XML类型信息（用于解决编译问题）
    /// </summary>
    public class SimpleXmlTypeInfo
    {
        public string XmlType { get; set; } = string.Empty;
        public bool IsSupported { get; set; }
        public bool IsAdapted { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<SupportedOperation> SupportedOperations { get; set; } = new();
    }

    /// <summary>
    /// 支持的操作类型
    /// </summary>
    public enum SupportedOperation
    {
        Read,
        Write,
        Validate,
        ConvertToExcel,
        ConvertFromExcel
    }

    #endregion

    #region 枚举定义

    /// <summary>
    /// 适配状态
    /// </summary>
    public enum AdaptationStatus
    {
        Unknown,
        Unadapted,
        PartiallyAdapted,
        FullyAdapted
    }

    /// <summary>
    /// 适配优先级
    /// </summary>
    public enum AdaptationPriority
    {
        Low,
        Medium,
        High
    }

    /// <summary>
    /// 适配复杂度
    /// </summary>
    public enum AdaptationComplexity
    {
        Low,
        Medium,
        High
    }

    #endregion
}