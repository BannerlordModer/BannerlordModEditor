# BannerIconsMapper API 规范

## 概述

本文档定义了BannerIconsMapper的API规范，包括接口设计、数据模型、方法签名和使用示例。该API提供BannerIcons配置文件的DO/DTO映射功能，确保XML序列化的准确性和一致性。

## 接口设计

### 1. 主要映射接口

#### BannerIconsMapper
```csharp
namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// BannerIcons对象映射器，负责DO和DTO之间的转换
    /// </summary>
    public static class BannerIconsMapper
    {
        /// <summary>
        /// 将DO对象转换为DTO对象
        /// </summary>
        /// <param name="source">源DO对象</param>
        /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
        public static BannerIconsDTO ToDTO(BannerIconsDO source);
        
        /// <summary>
        /// 将DTO对象转换为DO对象
        /// </summary>
        /// <param name="source">源DTO对象</param>
        /// <returns>转换后的DO对象，如果源为null则返回null</returns>
        public static BannerIconsDO ToDO(BannerIconsDTO source);
    }
}
```

#### BannerIconDataMapper
```csharp
/// <summary>
/// BannerIconData对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class BannerIconDataMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static BannerIconDataDTO ToDTO(BannerIconDataDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static BannerIconDataDO ToDO(BannerIconDataDTO source);
}
```

#### BannerIconGroupMapper
```csharp
/// <summary>
/// BannerIconGroup对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class BannerIconGroupMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static BannerIconGroupDTO ToDTO(BannerIconGroupDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static BannerIconGroupDO ToDO(BannerIconGroupDTO source);
}
```

#### BackgroundMapper
```csharp
/// <summary>
/// Background对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class BackgroundMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static BackgroundDTO ToDTO(BackgroundDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static BackgroundDO ToDO(BackgroundDTO source);
}
```

#### IconMapper
```csharp
/// <summary>
/// Icon对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class IconMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static IconDTO ToDTO(IconDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static IconDO ToDO(IconDTO source);
}
```

#### BannerColorsMapper
```csharp
/// <summary>
/// BannerColors对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class BannerColorsMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static BannerColorsDTO ToDTO(BannerColorsDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static BannerColorsDO ToDO(BannerColorsDTO source);
}
```

#### ColorEntryMapper
```csharp
/// <summary>
/// ColorEntry对象映射器，负责DO和DTO之间的转换
/// </summary>
public static class ColorEntryMapper
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">源DO对象</param>
    /// <returns>转换后的DTO对象，如果源为null则返回null</returns>
    public static ColorEntryDTO ToDTO(ColorEntryDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">源DTO对象</param>
    /// <returns>转换后的DO对象，如果源为null则返回null</returns>
    public static ColorEntryDO ToDO(ColorEntryDTO source);
}
```

## 数据模型

### 1. DO模型

#### BannerIconsDO
```csharp
namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// BannerIcons配置的领域对象
    /// </summary>
    [XmlRoot("base")]
    public class BannerIconsDO
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        /// <summary>
        /// Banner图标数据
        /// </summary>
        [XmlElement("BannerIconData")]
        public BannerIconDataDO? BannerIconData { get; set; }
        
        /// <summary>
        /// 是否存在BannerIconData元素的标记
        /// </summary>
        [XmlIgnore]
        public bool HasBannerIconData { get; set; } = false;
        
        /// <summary>
        /// 序列化控制：是否序列化BannerIconData
        /// </summary>
        public bool ShouldSerializeBannerIconData() => HasBannerIconData && BannerIconData != null;
        
        /// <summary>
        /// 序列化控制：是否序列化Type
        /// </summary>
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }
}
```

#### BannerIconDataDO
```csharp
/// <summary>
/// Banner图标数据的领域对象
/// </summary>
public class BannerIconDataDO
{
    /// <summary>
    /// Banner图标组列表
    /// </summary>
    [XmlElement("BannerIconGroup")]
    public List<BannerIconGroupDO> BannerIconGroups { get; set; } = new List<BannerIconGroupDO>();

    /// <summary>
    /// Banner颜色配置
    /// </summary>
    [XmlElement("BannerColors")]
    public BannerColorsDO? BannerColors { get; set; }
    
    /// <summary>
    /// 是否存在空BannerIconGroups元素的标记
    /// </summary>
    [XmlIgnore]
    public bool HasEmptyBannerIconGroups { get; set; } = false;
    
    /// <summary>
    /// 是否存在BannerColors元素的标记
    /// </summary>
    [XmlIgnore]
    public bool HasBannerColors { get; set; } = false;

    /// <summary>
    /// 序列化控制：是否序列化BannerIconGroups
    /// </summary>
    public bool ShouldSerializeBannerIconGroups() => HasEmptyBannerIconGroups || (BannerIconGroups != null && BannerIconGroups.Count > 0);
    
    /// <summary>
    /// 序列化控制：是否序列化BannerColors
    /// </summary>
    public bool ShouldSerializeBannerColors() => HasBannerColors && BannerColors != null;
}
```

#### BannerIconGroupDO
```csharp
/// <summary>
/// Banner图标组的领域对象
/// </summary>
public class BannerIconGroupDO
{
    /// <summary>
    /// 组ID
    /// </summary>
    [XmlAttribute("id")]
    public string? Id { get; set; }

    /// <summary>
    /// 组名称
    /// </summary>
    [XmlAttribute("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 是否为图案
    /// </summary>
    [XmlAttribute("is_pattern")]
    public string? IsPattern { get; set; }

    /// <summary>
    /// 背景列表
    /// </summary>
    [XmlElement("Background")]
    public List<BackgroundDO> Backgrounds { get; set; } = new List<BackgroundDO>();

    /// <summary>
    /// 图标列表
    /// </summary>
    [XmlElement("Icon")]
    public List<IconDO> Icons { get; set; } = new List<IconDO>();
    
    /// <summary>
    /// 是否存在空Backgrounds元素的标记
    /// </summary>
    [XmlIgnore]
    public bool HasEmptyBackgrounds { get; set; } = false;
    
    /// <summary>
    /// 是否存在空Icons元素的标记
    /// </summary>
    [XmlIgnore]
    public bool HasEmptyIcons { get; set; } = false;

    /// <summary>
    /// 序列化控制：是否序列化Id
    /// </summary>
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    
    /// <summary>
    /// 序列化控制：是否序列化Name
    /// </summary>
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    
    /// <summary>
    /// 序列化控制：是否序列化IsPattern
    /// </summary>
    public bool ShouldSerializeIsPattern() => !string.IsNullOrEmpty(IsPattern);
    
    /// <summary>
    /// 序列化控制：是否序列化Backgrounds
    /// </summary>
    public bool ShouldSerializeBackgrounds() => HasEmptyBackgrounds || (Backgrounds != null && Backgrounds.Count > 0);
    
    /// <summary>
    /// 序列化控制：是否序列化Icons
    /// </summary>
    public bool ShouldSerializeIcons() => HasEmptyIcons || (Icons != null && Icons.Count > 0);

    /// <summary>
    /// 类型安全的ID属性
    /// </summary>
    public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
    
    /// <summary>
    /// 类型安全的IsPattern属性
    /// </summary>
    public bool? IsPatternBool => bool.TryParse(IsPattern, out bool pattern) ? pattern : (bool?)null;
}
```

### 2. DTO模型

#### BannerIconsDTO
```csharp
namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// BannerIcons配置的数据传输对象
    /// </summary>
    [XmlRoot("base")]
    public class BannerIconsDTO
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        /// <summary>
        /// Banner图标数据
        /// </summary>
        [XmlElement("BannerIconData")]
        public BannerIconDataDTO? BannerIconData { get; set; }

        /// <summary>
        /// 序列化控制：是否序列化Type
        /// </summary>
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        
        /// <summary>
        /// 序列化控制：是否序列化BannerIconData
        /// </summary>
        public bool ShouldSerializeBannerIconData() => BannerIconData != null;

        /// <summary>
        /// 便捷属性：是否有Type
        /// </summary>
        public bool HasType => !string.IsNullOrEmpty(Type);
        
        /// <summary>
        /// 便捷属性：是否有BannerIconData
        /// </summary>
        public bool HasBannerIconData => BannerIconData != null;
    }
}
```

## 使用示例

### 1. 基本映射

```csharp
// 创建DO对象
var bannerIconsDO = new BannerIconsDO
{
    Type = "banner_icons",
    HasBannerIconData = true,
    BannerIconData = new BannerIconDataDO
    {
        HasEmptyBannerIconGroups = false,
        BannerIconGroups = new List<BannerIconGroupDO>
        {
            new BannerIconGroupDO
            {
                Id = "1",
                Name = "Test Group",
                IsPattern = "true",
                HasEmptyBackgrounds = false,
                Backgrounds = new List<BackgroundDO>
                {
                    new BackgroundDO
                    {
                        Id = "1",
                        MeshName = "test_mesh"
                    }
                }
            }
        }
    }
};

// 转换为DTO
var bannerIconsDTO = BannerIconsMapper.ToDTO(bannerIconsDO);

// 转换回DO
var convertedDO = BannerIconsMapper.ToDO(bannerIconsDTO);
```

### 2. XML序列化

```csharp
// 序列化为XML
string xml = XmlTestUtils.Serialize(bannerIconsDO, originalXml);

// 从XML反序列化
var deserializedDO = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
```

### 3. 集合处理

```csharp
// 处理空集合
var emptyGroup = new BannerIconGroupDO
{
    Id = "2",
    Name = "Empty Group",
    HasEmptyBackgrounds = true,  // 标记为空元素
    Backgrounds = new List<BackgroundDO>()  // 空集合
};

// 处理非空集合
var nonEmptyGroup = new BannerIconGroupDO
{
    Id = "3",
    Name = "Non-Empty Group",
    HasEmptyBackgrounds = false,
    Backgrounds = new List<BackgroundDO>
    {
        new BackgroundDO { Id = "1", MeshName = "mesh1" },
        new BackgroundDO { Id = "2", MeshName = "mesh2" }
    }
};
```

## 错误处理

### 1. 参数验证
- 所有映射方法都进行null检查
- 集合属性自动初始化为空列表
- 字符串属性进行空值检查

### 2. 异常处理
- 映射过程中出现的异常会向上抛出
- 建议在调用时使用try-catch块
- 提供详细的错误信息

### 3. 日志记录
- 建议在关键操作点添加日志记录
- 记录映射前后的对象状态
- 记录性能指标

## 性能考虑

### 1. 内存管理
- 大型XML文件建议使用流式处理
- 及时释放XML解析资源
- 避免在循环中创建大量对象

### 2. 批量处理
- 支持批量映射多个对象
- 可以使用并行处理提高性能
- 考虑使用对象池减少GC压力

### 3. 缓存策略
- 可以缓存常用的映射结果
- 对于只读操作可以使用缓存
- 注意缓存失效策略

## 扩展性

### 1. 新增属性
- 可以在DO和DTO中添加新属性
- 确保更新相应的映射方法
- 添加适当的序列化控制

### 2. 新增子类型
- 可以添加新的子类型映射器
- 遵循现有的命名约定
- 保持接口一致性

### 3. 自定义映射
- 支持自定义映射逻辑
- 可以通过配置文件定制映射规则
- 支持插件式扩展

## 版本兼容性

### 1. 向后兼容
- 保持现有API的稳定性
- 新功能通过新方法添加
- 废弃方法标记为Obsolete

### 2. 数据格式兼容
- 支持旧版本的数据格式
- 提供数据迁移工具
- 记录格式变更历史

### 3. 依赖兼容
- 明确依赖的版本要求
- 提供依赖升级指南
- 支持多版本并行运行