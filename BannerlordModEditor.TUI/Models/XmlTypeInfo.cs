using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// XML类型信息
    /// </summary>
    public class XmlTypeInfo
    {
        /// <summary>
        /// XML类型名称
        /// </summary>
        public string XmlType { get; set; } = string.Empty;

        /// <summary>
        /// 模型类型全名
        /// </summary>
        public string ModelType { get; set; } = string.Empty;

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; } = string.Empty;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 是否已适配
        /// </summary>
        public bool IsAdapted { get; set; }

        /// <summary>
        /// 是否支持
        /// </summary>
        public bool IsSupported { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName => !string.IsNullOrEmpty(Description) ? Description : XmlType;

        /// <summary>
        /// 支持的操作
        /// </summary>
        public List<SupportedOperation> SupportedOperations { get; set; } = new();

        /// <summary>
        /// 预计的记录数
        /// </summary>
        public int? EstimatedRecordCount { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModified { get; set; }
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

    /// <summary>
    /// XML类型检测服务
    /// </summary>
    public interface IXmlTypeDetectionService
    {
        /// <summary>
        /// 检测XML文件类型
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>XML类型信息</returns>
        Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath);

        /// <summary>
        /// 获取所有支持的XML类型
        /// </summary>
        /// <returns>支持的XML类型列表</returns>
        Task<List<XmlTypeInfo>> GetSupportedXmlTypesAsync();

        /// <summary>
        /// 检查是否支持指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型名称</param>
        /// <returns>是否支持</returns>
        Task<bool> IsXmlTypeSupportedAsync(string xmlType);

        /// <summary>
        /// 获取XML类型的模型类型
        /// </summary>
        /// <param name="xmlType">XML类型名称</param>
        /// <returns>模型类型</returns>
        Task<Type?> GetModelTypeAsync(string xmlType);
    }

    /// <summary>
    /// XML类型检测服务实现
    /// </summary>
    public class XmlTypeDetectionService : IXmlTypeDetectionService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, XmlTypeInfo> _supportedTypes;
        private readonly Dictionary<string, XmlTypeInfo> _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public XmlTypeDetectionService(IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _supportedTypes = InitializeSupportedTypes();
            _cache = new Dictionary<string, XmlTypeInfo>();
        }

        public async Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath)
        {
            // 验证输入参数
            if (string.IsNullOrWhiteSpace(xmlFilePath))
            {
                return new XmlTypeInfo
                {
                    IsSupported = false,
                    Description = "XML文件路径不能为空"
                };
            }

            if (!System.IO.File.Exists(xmlFilePath))
            {
                return new XmlTypeInfo
                {
                    IsSupported = false,
                    Description = "XML文件不存在"
                };
            }

            // 检查缓存
            var fileInfo = new System.IO.FileInfo(xmlFilePath);
            var cacheKey = $"{xmlFilePath}_{fileInfo.LastWriteTime.Ticks}";
            
            if (_cache.TryGetValue(cacheKey, out var cachedResult))
            {
                return cachedResult;
            }

            var result = new XmlTypeInfo
            {
                FileSize = fileInfo.Length,
                LastModified = fileInfo.LastWriteTime
            };

            try
            {
                // 使用简化的XML分析逻辑
                var analysisResult = await AnalyzeXmlFileAsync(xmlFilePath);

                if (analysisResult != null)
                {
                    result.XmlType = analysisResult.XmlType;
                    result.ModelType = analysisResult.ModelType;
                    result.Namespace = analysisResult.Namespace;
                    result.IsAdapted = analysisResult.IsAdapted;
                    result.IsSupported = analysisResult.IsAdapted;

                    // 设置描述信息
                    if (_supportedTypes.TryGetValue(analysisResult.XmlType, out var typeInfo))
                    {
                        result.Description = typeInfo.Description;
                        result.SupportedOperations = typeInfo.SupportedOperations;
                    }
                    else
                    {
                        result.Description = $"未知XML类型: {analysisResult.XmlType}";
                        result.SupportedOperations = new List<SupportedOperation>();
                    }
                }
                else
                {
                    // 如果Common层无法识别，尝试通用XML分析
                    await AnalyzeGenericXmlAsync(xmlFilePath, result);
                }

                // 估计记录数
                if (result.IsSupported)
                {
                    result.EstimatedRecordCount = await EstimateRecordCountAsync(xmlFilePath);
                }
            }
            catch (Exception ex)
            {
                result.IsSupported = false;
                result.Description = $"XML类型检测失败: {ex.Message}";
            }

            // 缓存结果
            _cache[cacheKey] = result;
            
            // 清理过期缓存
            CleanupExpiredCache();

            return result;
        }

        /// <summary>
        /// 简化的XML文件分析方法
        /// </summary>
        private async Task<XmlAnalysisResult?> AnalyzeXmlFileAsync(string xmlFilePath)
        {
            try
            {
                // 使用更高效的XML读取方式，只读取根元素
                using var reader = System.Xml.XmlReader.Create(xmlFilePath, new System.Xml.XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    IgnoreComments = true,
                    CloseInput = true
                });

                while (reader.Read())
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        var rootName = reader.Name;
                        
                        // 检查是否是已知的XML类型
                        if (_supportedTypes.TryGetValue(rootName, out var typeInfo))
                        {
                            return new XmlAnalysisResult
                            {
                                XmlType = rootName,
                                ModelType = typeInfo.ModelType,
                                Namespace = typeInfo.Namespace,
                                IsAdapted = typeInfo.IsAdapted
                            };
                        }

                        // 如果不是已知的XML类型，返回通用XML信息
                        return new XmlAnalysisResult
                        {
                            XmlType = rootName,
                            ModelType = "",
                            Namespace = "",
                            IsAdapted = false
                        };
                    }
                }

                return null;
            }
            catch (Exception ex) when (ex is System.Xml.XmlException || ex is System.IO.IOException)
            {
                // 处理XML解析和IO异常
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// XML分析结果
        /// </summary>
        private class XmlAnalysisResult
        {
            public string XmlType { get; set; } = string.Empty;
            public string ModelType { get; set; } = string.Empty;
            public string Namespace { get; set; } = string.Empty;
            public bool IsAdapted { get; set; }
        }

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private void CleanupExpiredCache()
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _cache.Keys
                .Where(key => 
                {
                    var lastModifiedTime = new DateTime(long.Parse(key.Split('_').Last()));
                    return (now - lastModifiedTime) > _cacheExpiration;
                })
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public (int CacheCount, int SupportedTypesCount) GetCacheStats()
        {
            return (_cache.Count, _supportedTypes.Count);
        }

        public async Task<List<XmlTypeInfo>> GetSupportedXmlTypesAsync()
        {
            return await Task.FromResult(_supportedTypes.Values.ToList());
        }

        public async Task<bool> IsXmlTypeSupportedAsync(string xmlType)
        {
            return await Task.FromResult(_supportedTypes.ContainsKey(xmlType));
        }

        public async Task<Type?> GetModelTypeAsync(string xmlType)
        {
            if (_supportedTypes.TryGetValue(xmlType, out var typeInfo))
            {
                try
                {
                    var type = Type.GetType(typeInfo.ModelType);
                    return await Task.FromResult(type);
                }
                catch
                {
                    return await Task.FromResult<Type?>(null);
                }
            }
            return await Task.FromResult<Type?>(null);
        }

        private async Task AnalyzeGenericXmlAsync(string xmlFilePath, XmlTypeInfo result)
        {
            try
            {
                var xmlDoc = System.Xml.Linq.XDocument.Load(xmlFilePath);
                var rootElement = xmlDoc.Root;

                if (rootElement != null)
                {
                    result.XmlType = rootElement.Name.LocalName;
                    result.Description = "通用XML文件";
                    result.IsSupported = true; // 通用XML文件支持基本的读写操作
                    result.SupportedOperations = new List<SupportedOperation>
                    {
                        SupportedOperation.Read,
                        SupportedOperation.Write,
                        SupportedOperation.ConvertToExcel,
                        SupportedOperation.ConvertFromExcel
                    };

                    // 估计记录数
                    result.EstimatedRecordCount = rootElement.Elements().Count();
                }
                else
                {
                    result.IsSupported = false;
                    result.Description = "XML文件没有根元素";
                }
            }
            catch (Exception ex)
            {
                result.IsSupported = false;
                result.Description = $"通用XML分析失败: {ex.Message}";
            }
        }

        private async Task<int> EstimateRecordCountAsync(string xmlFilePath)
        {
            try
            {
                var xmlDoc = System.Xml.Linq.XDocument.Load(xmlFilePath);
                var rootElement = xmlDoc.Root;

                if (rootElement != null)
                {
                    // 对于已知的XML类型，可以更准确地估计记录数
                    return rootElement.Elements().Count();
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private Dictionary<string, XmlTypeInfo> InitializeSupportedTypes()
        {
            var types = new Dictionary<string, XmlTypeInfo>();

            // 核心系统类型
            AddXmlType(types, "ActionTypes", typeof(ActionTypesDO), "动作类型定义", true);
            AddXmlType(types, "CombatParameters", typeof(CombatParametersDO), "战斗参数配置", true);
            AddXmlType(types, "ItemModifiers", typeof(ItemModifiersDO), "物品修饰符", true);
            AddXmlType(types, "CraftingPieces", typeof(CraftingPiecesDO), "制作部件", true);
            AddXmlType(types, "ItemHolsters", typeof(ItemHolstersDO), "物品插槽", true);
            AddXmlType(types, "ActionSets", typeof(ActionSetsDO), "动作集合", true);
            AddXmlType(types, "CollisionInfos", typeof(CollisionInfosDO), "碰撞信息", true);
            AddXmlType(types, "BoneBodyTypes", typeof(BoneBodyTypesDO), "骨骼身体类型", true);
            AddXmlType(types, "PhysicsMaterials", typeof(PhysicsMaterialsDO), "物理材质", true);
            AddXmlType(types, "ParticleSystems", typeof(ParticleSystemsDO), "粒子系统", true);
            
            // 游戏内容类型
            AddXmlType(types, "MapIcons", typeof(MapIconsDO), "地图图标", true);
            AddXmlType(types, "FloraKinds", typeof(FloraKindsDO), "植物类型", true);
            AddXmlType(types, "Scenes", typeof(ScenesDO), "场景定义", true);
            AddXmlType(types, "Credits", typeof(CreditsDO), "制作人员名单", true);
            AddXmlType(types, "BannerIcons", typeof(BannerIconsDO), "旗帜图标", true);
            AddXmlType(types, "Skins", typeof(SkinsDO), "皮肤定义", true);
            AddXmlType(types, "ItemUsageSets", typeof(ItemUsageSetsDO), "物品使用集合", true);
            AddXmlType(types, "MovementSets", typeof(MovementSetsDO), "运动集合", true);
            
            // 添加更多支持的XML类型（基于Common层）
            AddXmlType(types, "BasicObjects", typeof(object), "基础对象", false);
            AddXmlType(types, "BodyProperties", typeof(object), "身体属性", false);
            AddXmlType(types, "CampaignBehaviors", typeof(object), "战役行为", false);
            AddXmlType(types, "Characters", typeof(object), "角色定义", false);
            AddXmlType(types, "CraftingTemplates", typeof(object), "制作模板", false);
            AddXmlType(types, "FaceGenCharacters", typeof(object), "面部生成角色", false);
            AddXmlType(types, "Factions", typeof(object), "阵营定义", false);
            AddXmlType(types, "GameText", typeof(object), "游戏文本", false);
            AddXmlType(types, "ItemCategories", typeof(object), "物品类别", false);
            AddXmlType(types, "Items", typeof(object), "物品定义", false);
            AddXmlType(types, "Locations", typeof(object), "地点定义", false);
            AddXmlType(types, "Materials", typeof(object), "材质定义", false);
            AddXmlType(types, "Meshes", typeof(object), "网格定义", false);
            AddXmlType(types, "Monsters", typeof(object), "怪物定义", false);
            AddXmlType(types, "MPPoseNames", typeof(object), "多人姿势名称", false);
            AddXmlType(types, "NPCCharacters", typeof(object), "NPC角色", false);
            AddXmlType(types, "PartyTemplates", typeof(object), "队伍模板", false);
            AddXmlType(types, "QuestTemplates", typeof(object), "任务模板", false);
            AddXmlType(types, "SPCultures", typeof(object), "单人文化", false);
            AddXmlType(types, "Skills", typeof(object), "技能定义", false);
            AddXmlType(types, "SubModule", typeof(object), "子模块定义", false);
            AddXmlType(types, "TableauMaterials", typeof(object), "表格材质", false);
            AddXmlType(types, "TraitGroups", typeof(object), "特质组", false);
            AddXmlType(types, "Traits", typeof(object), "特质定义", false);
            AddXmlType(types, "Villages", typeof(object), "村庄定义", false);
            AddXmlType(types, "WeaponComponents", typeof(object), "武器组件", false);

            return types;
        }

        private void AddXmlType(Dictionary<string, XmlTypeInfo> types, string xmlType, Type modelType, string description, bool isSupported)
        {
            var operations = new List<SupportedOperation>
            {
                SupportedOperation.Read,
                SupportedOperation.Write,
                SupportedOperation.Validate,
                SupportedOperation.ConvertToExcel,
                SupportedOperation.ConvertFromExcel
            };

            // 对于未适配的类型，限制支持的操作
            if (!isSupported)
            {
                operations = new List<SupportedOperation>
                {
                    SupportedOperation.Read,
                    SupportedOperation.ConvertToExcel,
                    SupportedOperation.ConvertFromExcel
                };
            }

            types[xmlType] = new XmlTypeInfo
            {
                XmlType = xmlType,
                ModelType = modelType.AssemblyQualifiedName ?? string.Empty,
                Namespace = modelType.Namespace ?? string.Empty,
                Description = description,
                IsAdapted = isSupported,
                IsSupported = true, // 所有种类的XML都支持基本操作
                SupportedOperations = operations
            };
        }
    }
}