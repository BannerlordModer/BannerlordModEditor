using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// Layouts文件管理器
    /// 提供Layouts文件的增强功能，包括缓存、验证、批量操作等
    /// </summary>
    public class LayoutsManager
    {
        private readonly Dictionary<string, LayoutsBaseDO> _layoutsCache = new Dictionary<string, LayoutsBaseDO>();
        private readonly Dictionary<string, DateTime> _lastModifiedCache = new Dictionary<string, DateTime>();
        
        /// <summary>
        /// 加载Layouts文件
        /// </summary>
        public LayoutsBaseDO LoadLayoutsFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            
            // 检查缓存
            if (_layoutsCache.ContainsKey(filePath) && 
                _lastModifiedCache.ContainsKey(filePath) && 
                _lastModifiedCache[filePath] == fileInfo.LastWriteTime)
            {
                return _layoutsCache[filePath];
            }
            
            // 加载文件
            var serializer = new XmlSerializer(typeof(LayoutsBaseDO));
            LayoutsBaseDO layouts;
            
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                layouts = (LayoutsBaseDO)serializer.Deserialize(stream)!;
            }
            
            // 初始化索引
            layouts.InitializeIndexes();
            
            // 验证布局
            layouts.Validate();
            
            // 更新缓存
            _layoutsCache[filePath] = layouts;
            _lastModifiedCache[filePath] = fileInfo.LastWriteTime;
            
            return layouts;
        }
        
        /// <summary>
        /// 异步加载Layouts文件
        /// </summary>
        public async Task<LayoutsBaseDO> LoadLayoutsFileAsync(string filePath)
        {
            return await Task.Run(() => LoadLayoutsFile(filePath));
        }
        
        /// <summary>
        /// 保存Layouts文件
        /// </summary>
        public void SaveLayoutsFile(string filePath, LayoutsBaseDO layouts)
        {
            var serializer = new XmlSerializer(typeof(LayoutsBaseDO));
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            using (var writer = new System.IO.StreamWriter(stream))
            {
                var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, layouts, ns);
            }
            
            // 更新缓存
            _layoutsCache[filePath] = layouts;
            _lastModifiedCache[filePath] = DateTime.Now;
        }
        
        /// <summary>
        /// 异步保存Layouts文件
        /// </summary>
        public async Task SaveLayoutsFileAsync(string filePath, LayoutsBaseDO layouts)
        {
            await Task.Run(() => SaveLayoutsFile(filePath, layouts));
        }
        
        /// <summary>
        /// 批量加载Layouts文件
        /// </summary>
        public Dictionary<string, LayoutsBaseDO> LoadLayoutsFilesBatch(string directoryPath, string searchPattern = "*.xml")
        {
            var result = new Dictionary<string, LayoutsBaseDO>();
            var directoryInfo = new DirectoryInfo(directoryPath);
            
            if (!directoryInfo.Exists)
            {
                return result;
            }
            
            var files = directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
            
            foreach (var file in files)
            {
                try
                {
                    var layouts = LoadLayoutsFile(file.FullName);
                    result[file.FullName] = layouts;
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理其他文件
                    Console.WriteLine($"Failed to load {file.FullName}: {ex.Message}");
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 获取所有支持的布局类型
        /// </summary>
        public List<string> GetSupportedLayoutTypes()
        {
            return new List<string>
            {
                "SkeletonsLayout",
                "ParticleSystemLayout", 
                "ItemHolstersLayout",
                "PhysicsMaterialsLayout",
                "FloraKindsLayout",
                "AnimationsLayout"
            };
        }
        
        /// <summary>
        /// 创建新的Layouts配置
        /// </summary>
        public LayoutsBaseDO CreateNewLayouts(string type, string description = "")
        {
            var layouts = new LayoutsBaseDO
            {
                Type = type,
                HasLayouts = true,
                Layouts = new LayoutsContainerDO
                {
                    LayoutList = new List<LayoutDO>
                    {
                        new LayoutDO
                        {
                            Class = $"{type}_class",
                            XmlTag = $"{type}_tag",
                            Version = "1.0",
                            UseInTreeview = "true",
                            HasColumns = true,
                            Columns = new ColumnsDO
                            {
                                ColumnList = new List<ColumnDO>
                                {
                                    new ColumnDO { Id = "col1", Width = "200px" },
                                    new ColumnDO { Id = "col2", Width = "300px" }
                                }
                            },
                            HasItems = true,
                            Items = new ItemsDO
                            {
                                ItemList = new List<ItemDO>
                                {
                                    new ItemDO
                                    {
                                        Name = "name",
                                        Label = "Name",
                                        Type = "string",
                                        Column = "col1",
                                        XmlPath = "@name",
                                        Optional = "false"
                                    },
                                    new ItemDO
                                    {
                                        Name = "id", 
                                        Label = "ID",
                                        Type = "string",
                                        Column = "col2",
                                        XmlPath = "@id",
                                        Optional = "false"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            
            layouts.InitializeIndexes();
            layouts.Validate();
            
            return layouts;
        }
        
        /// <summary>
        /// 克隆Layouts配置
        /// </summary>
        public LayoutsBaseDO CloneLayouts(LayoutsBaseDO source)
        {
            // 序列化然后反序列化实现深拷贝
            var serializer = new XmlSerializer(typeof(LayoutsBaseDO));
            
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, source);
                memoryStream.Position = 0;
                return (LayoutsBaseDO)serializer.Deserialize(memoryStream)!;
            }
        }
        
        /// <summary>
        /// 合并两个Layouts配置
        /// </summary>
        public LayoutsBaseDO MergeLayouts(LayoutsBaseDO primary, LayoutsBaseDO secondary)
        {
            var merged = CloneLayouts(primary);
            
            // 合并布局
            foreach (var secondaryLayout in secondary.Layouts.LayoutList)
            {
                var existingLayout = merged.Layouts.LayoutList.FirstOrDefault(l => l.Class == secondaryLayout.Class);
                
                if (existingLayout == null)
                {
                    // 添加新布局
                    merged.Layouts.LayoutList.Add(CloneLayout(secondaryLayout));
                }
                else
                {
                    // 合并现有布局的项
                    MergeLayoutItems(existingLayout, secondaryLayout);
                }
            }
            
            merged.InitializeIndexes();
            merged.Validate();
            
            return merged;
        }
        
        /// <summary>
        /// 克隆单个布局
        /// </summary>
        private LayoutDO CloneLayout(LayoutDO source)
        {
            return new LayoutDO
            {
                Class = source.Class,
                Version = source.Version,
                XmlTag = source.XmlTag,
                NameAttribute = source.NameAttribute,
                HasNameAttribute = source.HasNameAttribute,
                HasEmptyNameAttribute = source.HasEmptyNameAttribute,
                UseInTreeview = source.UseInTreeview,
                Columns = CloneColumns(source.Columns),
                HasColumns = source.HasColumns,
                InsertionDefinitions = CloneInsertionDefinitions(source.InsertionDefinitions),
                HasInsertionDefinitions = source.HasInsertionDefinitions,
                TreeviewContextMenu = CloneTreeviewContextMenu(source.TreeviewContextMenu),
                HasTreeviewContextMenu = source.HasTreeviewContextMenu,
                Items = CloneItems(source.Items),
                HasItems = source.HasItems
            };
        }
        
        /// <summary>
        /// 克隆列配置
        /// </summary>
        private ColumnsDO CloneColumns(ColumnsDO source)
        {
            return new ColumnsDO
            {
                ColumnList = source.ColumnList.Select(c => new ColumnDO
                {
                    Id = c.Id,
                    Width = c.Width
                }).ToList()
            };
        }
        
        /// <summary>
        /// 克隆插入定义
        /// </summary>
        private InsertionDefinitionsDO CloneInsertionDefinitions(InsertionDefinitionsDO source)
        {
            return new InsertionDefinitionsDO
            {
                InsertionDefinitionList = source.InsertionDefinitionList.Select(d => new InsertionDefinitionDO
                {
                    Label = d.Label,
                    XmlPath = d.XmlPath,
                    DefaultNode = CloneDefaultNode(d.DefaultNode),
                    HasDefaultNode = d.HasDefaultNode
                }).ToList()
            };
        }
        
        /// <summary>
        /// 克隆默认节点
        /// </summary>
        private DefaultNodeDO CloneDefaultNode(DefaultNodeDO source)
        {
            return new DefaultNodeDO
            {
                AnyElements = source.AnyElements,
                HasAnyElements = source.HasAnyElements
            };
        }
        
        /// <summary>
        /// 克隆树形视图上下文菜单
        /// </summary>
        private TreeviewContextMenuDO CloneTreeviewContextMenu(TreeviewContextMenuDO source)
        {
            return new TreeviewContextMenuDO
            {
                ItemList = source.ItemList.Select(i => new ContextMenuItemDO
                {
                    Name = i.Name,
                    ActionCode = i.ActionCode,
                    HasActionCode = i.HasActionCode,
                    TreeviewContextMenu = CloneTreeviewContextMenu(i.TreeviewContextMenu),
                    HasTreeviewContextMenu = i.HasTreeviewContextMenu
                }).ToList()
            };
        }
        
        /// <summary>
        /// 克隆项配置
        /// </summary>
        private ItemsDO CloneItems(ItemsDO source)
        {
            return new ItemsDO
            {
                ItemList = source.ItemList.Select(i => new ItemDO
                {
                    Name = i.Name,
                    Label = i.Label,
                    Type = i.Type,
                    Column = i.Column,
                    XmlPath = i.XmlPath,
                    Optional = i.Optional,
                    HasOptional = i.HasOptional,
                    Properties = CloneProperties(i.Properties),
                    HasProperties = i.HasProperties,
                    DefaultNode = CloneDefaultNode(i.DefaultNode),
                    HasDefaultNode = i.HasDefaultNode
                }).ToList()
            };
        }
        
        /// <summary>
        /// 克隆属性配置
        /// </summary>
        private PropertiesDO CloneProperties(PropertiesDO source)
        {
            return new PropertiesDO
            {
                PropertyList = source.PropertyList.Select(p => new PropertyDO
                {
                    Name = p.Name,
                    Value = p.Value,
                    HasValue = p.HasValue
                }).ToList()
            };
        }
        
        /// <summary>
        /// 合并布局项
        /// </summary>
        private void MergeLayoutItems(LayoutDO target, LayoutDO source)
        {
            foreach (var sourceItem in source.Items.ItemList)
            {
                var existingItem = target.Items.ItemList.FirstOrDefault(i => i.Name == sourceItem.Name);
                
                if (existingItem == null)
                {
                    // 添加新项
                    target.Items.ItemList.Add(CloneItem(sourceItem));
                }
                else
                {
                    // 更新现有项（保留目标项的配置，只更新缺失的）
                    if (string.IsNullOrEmpty(existingItem.Label) && !string.IsNullOrEmpty(sourceItem.Label))
                        existingItem.Label = sourceItem.Label;
                    
                    if (string.IsNullOrEmpty(existingItem.Type) && !string.IsNullOrEmpty(sourceItem.Type))
                        existingItem.Type = sourceItem.Type;
                    
                    if (string.IsNullOrEmpty(existingItem.Column) && !string.IsNullOrEmpty(sourceItem.Column))
                        existingItem.Column = sourceItem.Column;
                    
                    if (string.IsNullOrEmpty(existingItem.XmlPath) && !string.IsNullOrEmpty(sourceItem.XmlPath))
                        existingItem.XmlPath = sourceItem.XmlPath;
                }
            }
        }
        
        /// <summary>
        /// 克隆单个项
        /// </summary>
        private ItemDO CloneItem(ItemDO source)
        {
            return new ItemDO
            {
                Name = source.Name,
                Label = source.Label,
                Type = source.Type,
                Column = source.Column,
                XmlPath = source.XmlPath,
                Optional = source.Optional,
                HasOptional = source.HasOptional,
                Properties = CloneProperties(source.Properties),
                HasProperties = source.HasProperties,
                DefaultNode = CloneDefaultNode(source.DefaultNode),
                HasDefaultNode = source.HasDefaultNode
            };
        }
        
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _layoutsCache.Clear();
            _lastModifiedCache.Clear();
        }
        
        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public LayoutsCacheStatistics GetCacheStatistics()
        {
            return new LayoutsCacheStatistics
            {
                CachedFilesCount = _layoutsCache.Count,
                TotalMemoryUsage = EstimateMemoryUsage(),
                OldestCacheEntry = _lastModifiedCache.Values.DefaultIfEmpty().Min(),
                NewestCacheEntry = _lastModifiedCache.Values.DefaultIfEmpty().Max()
            };
        }
        
        /// <summary>
        /// 估算内存使用量
        /// </summary>
        private long EstimateMemoryUsage()
        {
            // 简单的内存估算
            return _layoutsCache.Values.Sum(layouts => 
                layouts.Layouts.LayoutList.Count * 1000); // 每个布局约1KB
        }
    }
    
    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class LayoutsCacheStatistics
    {
        public int CachedFilesCount { get; set; }
        public long TotalMemoryUsage { get; set; }
        public DateTime OldestCacheEntry { get; set; }
        public DateTime NewestCacheEntry { get; set; }
    }
}