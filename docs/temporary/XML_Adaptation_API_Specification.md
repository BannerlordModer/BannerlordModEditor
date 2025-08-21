# BannerlordModEditor XML适配系统API规范

## 概述

本文档详细描述了BannerlordModEditor XML适配系统的API规范，包括所有公共接口、方法和使用示例。API设计遵循一致性、易用性和可扩展性原则。

## 核心命名空间

### BannerlordModEditor.Common.Models.DO
包含所有领域对象（Domain Objects），提供业务逻辑和类型安全的数据访问。

### BannerlordModEditor.Common.Models.DTO
包含所有数据传输对象（Data Transfer Objects），专门用于XML序列化。

### BannerlordModEditor.Common.Mappers
包含所有对象映射器，负责DO和DTO之间的转换。

### BannerlordModEditor.Common.Services
包含核心服务，如文件发现、XML加载等。

## 核心API组件

### 1. XML加载服务

#### GenericXmlLoader<T>

**命名空间**：`BannerlordModEditor.Common.Services`

**描述**：通用XML序列化/反序列化服务，支持保留原始XML结构信息。

**公共方法**：

```csharp
// 反序列化XML到指定类型
public static T Deserialize<T>(string xmlContent) where T : class, new()

// 序列化对象到XML，保留原始结构信息
public static string Serialize<T>(T obj, string originalXml = null) where T : class

// 检查文件是否已适配
public static bool IsFileAdapted(string filePath)
```

**使用示例**：
```csharp
// 反序列化
string xmlContent = File.ReadAllText("banner_icons.xml");
var bannerIcons = GenericXmlLoader<BannerIconsDO>.Deserialize(xmlContent);

// 序列化（保留原始结构）
string serializedXml = GenericXmlLoader<BannerIconsDO>.Serialize(bannerIcons, xmlContent);

// 检查适配状态
bool isAdapted = GenericXmlLoader.IsFileAdapted("banner_icons.xml");
```

### 2. 文件发现服务

#### FileDiscoveryService

**命名空间**：`BannerlordModEditor.Common.Services`

**描述**：提供XML文件发现和适配状态检查功能。

**公共方法**：

```csharp
// 获取所有XML文件
public static List<string> GetAllXmlFiles(string rootPath)

// 获取已适配的XML文件
public static List<string> GetAdaptedXmlFiles(string rootPath)

// 获取未适配的XML文件
public static List<string> GetUnadaptedXmlFiles(string rootPath)

// 检查文件是否支持
public static bool IsFileSupported(string filePath)
```

**使用示例**：
```csharp
// 获取所有XML文件
var allFiles = FileDiscoveryService.GetAllXmlFiles("ModuleData");

// 获取已适配的文件
var adaptedFiles = FileDiscoveryService.GetAdaptedXmlFiles("ModuleData");

// 检查文件支持
bool supported = FileDiscoveryService.IsFileSupported("spclcharacters.xml");
```

### 3. 命名约定服务

#### NamingConventionMapper

**命名空间**：`BannerlordModEditor.Common.Services`

**描述**：处理不同命名约定之间的转换。

**公共方法**：

```csharp
// 转换文件名到类名
public static string FileNameToClassName(string fileName)

// 转换类名到文件名
public static string ClassNameToFileName(string className)

// 获取XML根元素名
public static string GetXmlRootName(string fileName)

// 获取DO类名
public static string GetDOClassName(string fileName)
```

**使用示例**：
```csharp
// 文件名到类名转换
string className = NamingConventionMapper.FileNameToClassName("banner_icons.xml");
// 返回: "BannerIcons"

// 获取DO类名
string doClassName = NamingConventionMapper.GetDOClassName("item_modifiers.xml");
// 返回: "ItemModifiersDO"
```

## 具体XML类型API

### 1. BannerIcons API

#### DO层API

**BannerIconsDO**
```csharp
namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class BannerIconsDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlElement("BannerIconData")]
        public BannerIconDataDO? BannerIconData { get; set; }
        
        [XmlIgnore]
        public bool HasBannerIconData { get; set; } = false;
        
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeBannerIconData() => HasBannerIconData && BannerIconData != null;
    }
}
```

**BannerIconGroupDO**
```csharp
public class BannerIconGroupDO
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("is_pattern")]
    public string? IsPattern { get; set; }

    [XmlElement("Background")]
    public List<BackgroundDO> Backgrounds { get; set; } = new List<BackgroundDO>();

    [XmlElement("Icon")]
    public List<IconDO> Icons { get; set; } = new List<IconDO>();
    
    // 类型安全的便捷属性
    public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
    public bool? IsPatternBool => bool.TryParse(IsPattern, out bool pattern) ? pattern : (bool?)null;
}
```

#### DTO层API

**BannerIconsDTO**
```csharp
namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class BannerIconsDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlElement("BannerIconData")]
        public BannerIconDataDTO? BannerIconData { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeBannerIconData() => BannerIconData != null;

        // 便捷属性
        public bool HasType => !string.IsNullOrEmpty(Type);
        public bool HasBannerIconData => BannerIconData != null;
    }
}
```

#### 映射器API

**BannerIconsMapper**
```csharp
namespace BannerlordModEditor.Common.Mappers
{
    public static class BannerIconsMapper
    {
        // DO to DTO 转换
        public static BannerIconsDTO ToDTO(BannerIconsDO source)
        
        // DTO to DO 转换
        public static BannerIconsDO ToDO(BannerIconsDTO source)
    }
}
```

#### 使用示例
```csharp
// 加载XML文件
string xmlContent = File.ReadAllText("banner_icons.xml");
var bannerIconsDO = GenericXmlLoader<BannerIconsDO>.Deserialize(xmlContent);

// 访问数据
foreach (var group in bannerIconsDO.BannerIconData?.BannerIconGroups ?? new List<BannerIconGroupDO>())
{
    Console.WriteLine($"Group: {group.Name} (ID: {group.IdInt})");
    Console.WriteLine($"Is Pattern: {group.IsPatternBool}");
    
    foreach (var background in group.Backgrounds)
    {
        Console.WriteLine($"  Background: {background.MeshName}");
    }
}

// 转换为DTO
var bannerIconsDTO = BannerIconsMapper.ToDTO(bannerIconsDO);

// 序列化回XML
string serializedXml = GenericXmlLoader<BannerIconsDO>.Serialize(bannerIconsDO, xmlContent);
```

### 2. ItemModifiers API

#### DO层API

**ItemModifiersDO**
```csharp
namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("ItemModifiers", Namespace = "")]
    [XmlType(Namespace = "")]
    public class ItemModifiersDO
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifierDO> ItemModifierList { get; set; } = new List<ItemModifierDO>();

        [XmlIgnore]
        public bool HasEmptyItemModifierList { get; set; } = false;

        public bool ShouldSerializeItemModifierList() => 
            HasEmptyItemModifierList || (ItemModifierList != null && ItemModifierList.Count > 0);
    }
}
```

**ItemModifierDO**
```csharp
public class ItemModifierDO
{
    [XmlAttribute("modifier_group")]
    public string? ModifierGroup { get; set; }
    
    [XmlAttribute("id")]
    public string? Id { get; set; }
    
    [XmlAttribute("damage")]
    public string? DamageString
    {
        get => Damage.HasValue ? Damage.Value.ToString() : null;
        set => Damage = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
    }
    [XmlIgnore]
    public int? Damage { get; set; }
    
    [XmlAttribute("price_factor")]
    public string? PriceFactorString
    {
        get => PriceFactor.HasValue ? PriceFactor.Value.ToString() : null;
        set => PriceFactor = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
    }
    [XmlIgnore]
    public float? PriceFactor { get; set; }
    
    // ShouldSerialize 方法
    public bool ShouldSerializeModifierGroup() => !string.IsNullOrWhiteSpace(ModifierGroup);
    public bool ShouldSerializeDamageString() => !string.IsNullOrEmpty(DamageString);
    public bool ShouldSerializePriceFactorString() => !string.IsNullOrEmpty(PriceFactorString);
}
```

#### DTO层API

**ItemModifiersDTO**
```csharp
namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("ItemModifiers")]
    public class ItemModifiersDTO
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifierDTO> ItemModifierList { get; set; } = new List<ItemModifierDTO>();

        public bool ShouldSerializeItemModifierList() => ItemModifierList != null && ItemModifierList.Count > 0;

        // 便捷属性
        public int Count => ItemModifierList?.Count ?? 0;
        public bool HasItemModifiers => ItemModifierList != null && ItemModifierList.Count > 0;
    }
}
```

**ItemModifierDTO**
```csharp
public class ItemModifierDTO
{
    [XmlAttribute("modifier_group")]
    public string? ModifierGroup { get; set; }
    
    [XmlAttribute("damage")]
    public string? DamageString { get; set; }
    
    [XmlAttribute("price_factor")]
    public string? PriceFactorString { get; set; }

    // 类型安全的便捷属性
    public int? DamageInt => int.TryParse(DamageString, out int val) ? val : (int?)null;
    public float? PriceFactorFloat => float.TryParse(PriceFactorString, out float val) ? val : (float?)null;

    // 设置方法
    public void SetDamageInt(int? value) => DamageString = value?.ToString();
    public void SetPriceFactorFloat(float? value) => PriceFactorString = value?.ToString();
}
```

#### 使用示例
```csharp
// 加载XML文件
string xmlContent = File.ReadAllText("item_modifiers.xml");
var itemModifiersDO = GenericXmlLoader<ItemModifiersDO>.Deserialize(xmlContent);

// 访问和修改数据
foreach (var modifier in itemModifiersDO.ItemModifierList)
{
    Console.WriteLine($"Modifier: {modifier.Name} ({modifier.Id})");
    Console.WriteLine($"  Damage: {modifier.Damage}");
    Console.WriteLine($"  Price Factor: {modifier.PriceFactor}");
    
    // 修改数据
    if (modifier.Id == "masterwork_sword")
    {
        modifier.Damage = 10; // 自动转换为字符串
        modifier.PriceFactor = 2.0f; // 自动转换为字符串
    }
}

// 转换为DTO
var itemModifiersDTO = ItemModifiersMapper.ToDTO(itemModifiersDO);

// 序列化回XML
string serializedXml = GenericXmlLoader<ItemModifiersDO>.Serialize(itemModifiersDO, xmlContent);
```

### 3. ParticleSystems API

#### DO层API

**ParticleSystemsDO**
```csharp
namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsDO
    {
        [XmlElement("effect")]
        public List<EffectDO> Effects { get; set; } = new List<EffectDO>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }
}
```

**EffectDO**
```csharp
public class EffectDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("guid")]
    public string? Guid { get; set; }

    [XmlAttribute("sound_code")]
    public string? SoundCode { get; set; }

    [XmlElement("emitters")]
    public EmittersDO? Emitters { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
    public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
    public bool ShouldSerializeEmitters() => Emitters != null;
}
```

**EmitterDO**
```csharp
public class EmitterDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("_index_")]
    public string? Index { get; set; }

    [XmlElement("children")]
    public ChildrenDO? Children { get; set; }

    [XmlElement("flags")]
    public ParticleFlagsDO? Flags { get; set; }

    [XmlElement("parameters")]
    public ParametersDO? Parameters { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeChildren() => Children != null;
    public bool ShouldSerializeFlags() => Flags != null;
    public bool ShouldSerializeParameters() => Parameters != null;
}
```

#### DTO层API

**ParticleSystemsDTO**
```csharp
namespace BannerlordModEditor.Common.Models.DTO
{
    public class ParticleSystemsDTO
    {
        public List<EffectDTO> Effects { get; set; } = new List<EffectDTO>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;

        // 便捷属性
        public int EffectsCount => Effects?.Count ?? 0;
    }
}
```

**EmitterDTO**
```csharp
public class EmitterDTO
{
    public string? Name { get; set; }
    public string? Index { get; set; }
    public ChildrenDTO? Children { get; set; }
    public ParticleFlagsDTO? Flags { get; set; }
    public ParametersDTO? Parameters { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeChildren() => Children != null;
    public bool ShouldSerializeFlags() => Flags != null;
    public bool ShouldSerializeParameters() => Parameters != null;

    // 类型安全的便捷属性
    public int? IndexInt => int.TryParse(Index, out int idx) ? idx : (int?)null;
    public bool HasChildren => Children != null;
    public bool HasFlags => Flags != null;
    public bool HasParameters => Parameters != null;
    public int FlagsCount => Flags?.FlagList?.Count ?? 0;
    public int ParametersCount => Parameters?.ParameterList?.Count ?? 0;
    public int ChildrenCount => Children?.EmitterList?.Count ?? 0;

    // 设置方法
    public void SetIndex(int? value) => Index = value?.ToString();
}
```

#### 使用示例
```csharp
// 加载XML文件
string xmlContent = File.ReadAllText("particle_systems_basic.xml");
var particleSystemsDO = GenericXmlLoader<ParticleSystemsDO>.Deserialize(xmlContent);

// 递归访问粒子系统数据
foreach (var effect in particleSystemsDO.Effects)
{
    Console.WriteLine($"Effect: {effect.Name}");
    
    foreach (var emitter in effect.Emitters?.EmitterList ?? new List<EmitterDO>())
    {
        Console.WriteLine($"  Emitter: {emitter.Name} (Index: {emitter.Index})");
        
        // 访问子发射器
        if (emitter.Children?.EmitterList != null)
        {
            foreach (var child in emitter.Children.EmitterList)
            {
                Console.WriteLine($"    Child: {child.Name}");
            }
        }
        
        // 访问参数
        if (emitter.Parameters?.ParameterList != null)
        {
            foreach (var param in emitter.Parameters.ParameterList)
            {
                Console.WriteLine($"    Parameter: {param.Name} = {param.Value}");
            }
        }
    }
}

// 转换为DTO
var particleSystemsDTO = ParticleSystemsMapper.ToDTO(particleSystemsDO);

// 序列化回XML
string serializedXml = GenericXmlLoader<ParticleSystemsDO>.Serialize(particleSystemsDO, xmlContent);
```

## 工具类API

### XmlTestUtils

**命名空间**：`BannerlordModEditor.Common.Tests`

**描述**：提供XML测试和验证工具。

**公共方法**：

```csharp
// 验证两个XML是否结构等价
public static bool AreStructurallyEqual(string xml1, string xml2)

// 检查是否没有新增的null属性
public static bool NoNewNullAttributes(string original, string serialized)

// 统计节点和属性数量
public static (int nodeCount, int attrCount) CountNodesAndAttributes(string xml)

// 输出所有节点和属性信息（用于调试）
public static void LogAllNodesAndAttributes(string xml, string title)
```

**使用示例**：
```csharp
// 验证序列化结果
bool isEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
bool noNewNulls = XmlTestUtils.NoNewNullAttributes(originalXml, serializedXml);

// 获取统计信息
var (origNodes, origAttrs) = XmlTestUtils.CountNodesAndAttributes(originalXml);
var (serNodes, serAttrs) = XmlTestUtils.CountNodesAndAttributes(serializedXml);

Console.WriteLine($"Original: {origNodes} nodes, {origAttrs} attributes");
Console.WriteLine($"Serialized: {serNodes} nodes, {serAttrs} attributes");
```

## 异常处理

### 自定义异常类型

#### XmlAdaptationException
```csharp
namespace BannerlordModEditor.Common.Exceptions
{
    public class XmlAdaptationException : Exception
    {
        public XmlAdaptationException(string message) : base(message) { }
        public XmlAdaptationException(string message, Exception inner) : base(message, inner) { }
    }
}
```

#### XmlSerializationException
```csharp
public class XmlSerializationException : XmlAdaptationException
{
    public XmlSerializationException(string message) : base(message) { }
    public XmlSerializationException(string message, Exception inner) : base(message, inner) { }
}
```

### 异常处理示例

```csharp
try
{
    var bannerIcons = GenericXmlLoader<BannerIconsDO>.Deserialize(xmlContent);
    // 处理数据...
}
catch (XmlSerializationException ex)
{
    Console.WriteLine($"XML序列化错误: {ex.Message}");
    // 处理序列化异常
}
catch (XmlAdaptationException ex)
{
    Console.WriteLine($"XML适配错误: {ex.Message}");
    // 处理适配异常
}
catch (Exception ex)
{
    Console.WriteLine($"未知错误: {ex.Message}");
    // 处理其他异常
}
```

## 性能考虑

### 1. 大型文件处理

```csharp
// 使用流式处理大型文件
using (var stream = new FileStream("large_file.xml", FileMode.Open))
using (var reader = new StreamReader(stream))
{
    string xmlContent = reader.ReadToEnd();
    var result = GenericXmlLoader<T>.Deserialize(xmlContent);
    // 处理结果...
}
```

### 2. 批量处理

```csharp
// 批量处理多个文件
var files = FileDiscoveryService.GetAllXmlFiles("ModuleData");
var results = new ConcurrentBag<object>();

Parallel.ForEach(files, file =>
{
    try
    {
        string xmlContent = File.ReadAllText(file);
        var result = ProcessXmlFile(file, xmlContent);
        results.Add(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"处理文件 {file} 时出错: {ex.Message}");
    }
});
```

### 3. 内存优化

```csharp
// 及时释放资源
public void ProcessXmlFile(string filePath)
{
    string xmlContent = File.ReadAllText(filePath);
    
    try
    {
        var obj = GenericXmlLoader<T>.Deserialize(xmlContent);
        // 处理对象...
    }
    finally
    {
        // 确保资源被释放
        xmlContent = null;
        GC.Collect();
    }
}
```

## 扩展指南

### 1. 添加新的XML类型支持

#### 步骤1：创建DO模型
```csharp
[XmlRoot("root_element")]
public class NewXmlTypeDO
{
    [XmlAttribute("attribute")]
    public string? Attribute { get; set; }
    
    [XmlElement("element")]
    public List<ElementDO> Elements { get; set; } = new List<ElementDO>();
    
    public bool ShouldSerializeAttribute() => !string.IsNullOrEmpty(Attribute);
}
```

#### 步骤2：创建DTO模型
```csharp
[XmlRoot("root_element")]
public class NewXmlTypeDTO
{
    [XmlAttribute("attribute")]
    public string? Attribute { get; set; }
    
    [XmlElement("element")]
    public List<ElementDTO> Elements { get; set; } = new List<ElementDTO>();
    
    // 便捷属性
    public bool HasAttribute => !string.IsNullOrEmpty(Attribute);
    
    // 类型安全的便捷属性
    public int? AttributeInt => int.TryParse(Attribute, out int val) ? val : (int?)null;
}
```

#### 步骤3：创建映射器
```csharp
public static class NewXmlTypeMapper
{
    public static NewXmlTypeDTO ToDTO(NewXmlTypeDO source)
    {
        if (source == null) return null;
        
        return new NewXmlTypeDTO
        {
            Attribute = source.Attribute,
            Elements = source.Elements?.Select(ToDTO).ToList() ?? new List<ElementDTO>()
        };
    }
    
    public static NewXmlTypeDO ToDO(NewXmlTypeDTO source)
    {
        if (source == null) return null;
        
        return new NewXmlTypeDO
        {
            Attribute = source.Attribute,
            Elements = source.Elements?.Select(ToDo).ToList() ?? new List<ElementDO>()
        };
    }
}
```

#### 步骤4：更新文件发现服务
```csharp
// 在FileDiscoveryService中添加支持
public static bool IsFileSupported(string filePath)
{
    string fileName = Path.GetFileName(filePath).ToLower();
    
    return fileName switch
    {
        "banner_icons.xml" => true,
        "item_modifiers.xml" => true,
        "particle_systems_basic.xml" => true,
        "new_xml_type.xml" => true, // 添加新类型
        _ => false
    };
}
```

### 2. 自定义序列化逻辑

#### 实现自定义ShouldSerialize方法
```csharp
public class CustomDO
{
    private string? _customField;
    
    [XmlAttribute("custom_field")]
    public string? CustomField
    {
        get => _customField;
        set => _customField = value?.Trim();
    }
    
    public bool ShouldSerializeCustomField()
    {
        // 自定义序列化逻辑
        return !string.IsNullOrEmpty(CustomField) && 
               CustomField.Length > 3 && 
               !CustomField.StartsWith("temp_");
    }
}
```

#### 实现自定义类型转换
```csharp
public class CustomDTO
{
    [XmlAttribute("custom_field")]
    public string? CustomField { get; set; }
    
    // 自定义类型转换
    public DateTime? CustomFieldDateTime
    {
        get => DateTime.TryParse(CustomField, out DateTime dt) ? dt : (DateTime?)null;
        set => CustomField = value?.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    public Guid? CustomFieldGuid
    {
        get => Guid.TryParse(CustomField, out Guid guid) ? guid : (Guid?)null;
        set => CustomField = value?.ToString();
    }
}
```

## 总结

BannerlordModEditor XML适配系统的API设计遵循以下原则：

1. **一致性**：所有XML类型都遵循相同的DO/DTO模式
2. **易用性**：提供类型安全的便捷属性和方法
3. **可扩展性**：标准化的架构支持快速添加新类型
4. **健壮性**：完善的错误处理和验证机制
5. **性能**：针对大型文件和批量处理的优化

通过这套API，开发者可以轻松地：
- 加载和解析骑马与砍杀2的XML配置文件
- 以类型安全的方式访问和修改数据
- 确保序列化后的XML与原始文件保持一致
- 扩展系统以支持新的XML类型

该API为骑马与砍杀2模组开发提供了强大而灵活的XML处理能力。