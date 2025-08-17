# ParticleSystems XML序列化API规范

## API概览

本文档定义了ParticleSystems XML序列化系统的完整API接口规范，包括数据模型、序列化接口、映射接口和测试接口。

## 接口定义

### 1. 核心数据模型接口

#### ParticleSystemsDO (领域对象)
```csharp
namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 粒子系统根对象 - 领域模型
    /// </summary>
    [XmlRoot("particle_effects")]
    public class ParticleSystemsDO
    {
        /// <summary>
        /// 粒子效果列表
        /// </summary>
        [XmlElement("effect")]
        public List<EffectDO> Effects { get; set; } = new List<EffectDO>();

        /// <summary>
        /// 序列化控制：仅在Effects不为空时序列化
        /// </summary>
        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }
}
```

#### EffectDO (效果对象)
```csharp
/// <summary>
/// 粒子效果对象
/// </summary>
public class EffectDO
{
    /// <summary>
    /// 效果名称
    /// </summary>
    [XmlAttribute("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 全局唯一标识符
    /// </summary>
    [XmlAttribute("guid")]
    public string? Guid { get; set; }

    /// <summary>
    /// 音效代码
    /// </summary>
    [XmlAttribute("sound_code")]
    public string? SoundCode { get; set; }

    /// <summary>
    /// 发射器集合
    /// </summary>
    [XmlElement("emitters")]
    public EmittersDO? Emitters { get; set; }

    // 序列化控制方法
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
    public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
    public bool ShouldSerializeEmitters() => Emitters != null;
}
```

#### EmitterDO (发射器对象)
```csharp
/// <summary>
/// 粒子发射器对象
/// </summary>
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

    // 运行时标记属性
    [XmlIgnore]
    public bool HasEmptyChildren { get; set; } = false;

    [XmlIgnore]
    public bool HasEmptyFlags { get; set; } = false;

    [XmlIgnore]
    public bool HasEmptyParameters { get; set; } = false;

    // 序列化控制方法
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeChildren() => Children != null || HasEmptyChildren;
    public bool ShouldSerializeFlags() => Flags != null || HasEmptyFlags;
    public bool ShouldSerializeParameters() => Parameters != null || HasEmptyParameters;
}
```

#### ParametersDO (参数容器)
```csharp
/// <summary>
/// 参数容器对象
/// </summary>
public class ParametersDO
{
    [XmlElement("parameter", Order = 1)]
    public List<ParameterDO> ParameterList { get; set; } = new List<ParameterDO>();

    [XmlElement("decal_materials", Order = 2)]
    public DecalMaterialsDO? DecalMaterials { get; set; }

    // 运行时标记属性
    [XmlIgnore]
    public bool HasDecalMaterials { get; set; } = false;

    [XmlIgnore]
    public bool HasEmptyParameters { get; set; } = false;

    // 序列化控制方法
    public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
    public bool ShouldSerializeDecalMaterials() => DecalMaterials != null || HasDecalMaterials;
}
```

#### ParameterDO (参数对象)
```csharp
/// <summary>
/// 参数对象 - 支持简单参数和曲线参数
/// </summary>
public class ParameterDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlAttribute("base")]
    public string? Base { get; set; }

    [XmlAttribute("bias")]
    public string? Bias { get; set; }

    [XmlAttribute("curve")]
    public string? Curve { get; set; }

    [XmlElement("curve", Order = 1)]
    public CurveDO? ParameterCurve { get; set; }

    [XmlElement("color", Order = 2)]
    public ColorDO? ColorElement { get; set; }

    [XmlElement("alpha", Order = 3)]
    public AlphaDO? AlphaElement { get; set; }

    // 序列化控制方法
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeValue() => Value != null;
    public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
    public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
    public bool ShouldSerializeCurve() => !string.IsNullOrEmpty(Curve);
    public bool ShouldSerializeParameterCurve() => ParameterCurve != null;
    public bool ShouldSerializeColorElement() => ColorElement != null;
    public bool ShouldSerializeAlphaElement() => AlphaElement != null;
}
```

#### CurveDO (曲线对象)
```csharp
/// <summary>
/// 曲线对象
/// </summary>
public class CurveDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("version")]
    public string? Version { get; set; }

    [XmlAttribute("default")]
    public string? Default { get; set; }

    [XmlAttribute("curve_multiplier")]
    public string? CurveMultiplier { get; set; }

    [XmlElement("keys")]
    public KeysDO? Keys { get; set; }

    // 序列化控制方法
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
    public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);
    public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);
    public bool ShouldSerializeKeys() => Keys != null;
}
```

#### DecalMaterialsDO (贴花材质容器)
```csharp
/// <summary>
/// 贴花材质容器对象
/// </summary>
public class DecalMaterialsDO
{
    [XmlElement("decal_material")]
    public List<DecalMaterialDO> DecalMaterialList { get; set; } = new List<DecalMaterialDO>();

    // 序列化控制方法
    public bool ShouldSerializeDecalMaterialList() => DecalMaterialList != null && DecalMaterialList.Count > 0;
}
```

### 2. DTO层接口

#### ParticleSystemsDTO
```csharp
namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 粒子系统数据传输对象 - 专门用于XML序列化
    /// </summary>
    [XmlRoot("particle_effects")]
    public class ParticleSystemsDTO
    {
        [XmlElement("effect")]
        public List<EffectDTO> Effects { get; set; } = new List<EffectDTO>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }
}
```

#### EffectDTO
```csharp
/// <summary>
/// 粒子效果数据传输对象
/// </summary>
public class EffectDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("guid")]
    public string? Guid { get; set; }

    [XmlAttribute("sound_code")]
    public string? SoundCode { get; set; }

    [XmlElement("emitters")]
    public EmittersDTO? Emitters { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
    public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
    public bool ShouldSerializeEmitters() => Emitters != null;
}
```

### 3. 映射器接口

#### ParticleSystemsMapper
```csharp
namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 粒子系统对象映射器 - 负责DO和DTO之间的转换
    /// </summary>
    public static class ParticleSystemsMapper
    {
        /// <summary>
        /// 将DO对象转换为DTO对象
        /// </summary>
        /// <param name="source">源DO对象</param>
        /// <returns>转换后的DTO对象</returns>
        public static ParticleSystemsDTO ToDTO(ParticleSystemsDO source)
        {
            if (source == null) return null;
            
            return new ParticleSystemsDTO
            {
                Effects = source.Effects?
                    .Select(EffectMapper.ToDTO)
                    .Where(e => e != null)
                    .ToList() ?? new List<EffectDTO>()
            };
        }

        /// <summary>
        /// 将DTO对象转换为DO对象
        /// </summary>
        /// <param name="source">源DTO对象</param>
        /// <returns>转换后的DO对象</returns>
        public static ParticleSystemsDO ToDO(ParticleSystemsDTO source)
        {
            if (source == null) return null;
            
            return new ParticleSystemsDO
            {
                Effects = source.Effects?
                    .Select(EffectMapper.ToDO)
                    .Where(e => e != null)
                    .ToList() ?? new List<EffectDO>()
            };
        }
    }
}
```

#### EffectMapper
```csharp
/// <summary>
/// 效果对象映射器
/// </summary>
public static class EffectMapper
{
    public static EffectDTO ToDTO(EffectDO source)
    {
        if (source == null) return null;
        
        return new EffectDTO
        {
            Name = source.Name,
            Guid = source.Guid,
            SoundCode = source.SoundCode,
            Emitters = EmittersMapper.ToDTO(source.Emitters)
        };
    }

    public static EffectDO ToDO(EffectDTO source)
    {
        if (source == null) return null;
        
        return new EffectDO
        {
            Name = source.Name,
            Guid = source.Guid,
            SoundCode = source.SoundCode,
            Emitters = EmittersMapper.ToDO(source.Emitters)
        };
    }
}
```

### 4. 序列化接口

#### XmlTestUtils 扩展方法
```csharp
namespace BannerlordModEditor.Common.Tests
{
    public static class XmlTestUtils
    {
        /// <summary>
        /// 反序列化ParticleSystems XML
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns>ParticleSystemsDO对象</returns>
        public static ParticleSystemsDO DeserializeParticleSystems(string xml)
        {
            var obj = Deserialize<ParticleSystemsDO>(xml);
            
            // 特殊处理ParticleSystemsDO来检测和保持复杂的XML结构
            if (obj is ParticleSystemsDO particleSystems)
            {
                ProcessParticleSystemsStructure(particleSystems, xml);
            }
            
            return obj;
        }

        /// <summary>
        /// 序列化ParticleSystems对象
        /// </summary>
        /// <param name="obj">ParticleSystemsDO对象</param>
        /// <param name="originalXml">原始XML（用于保留命名空间）</param>
        /// <returns>序列化后的XML字符串</returns>
        public static string SerializeParticleSystems(ParticleSystemsDO obj, string originalXml)
        {
            return Serialize(obj, originalXml);
        }

        /// <summary>
        /// 处理ParticleSystems的复杂结构
        /// </summary>
        /// <param name="particleSystems">粒子系统对象</param>
        /// <param name="xml">原始XML</param>
        private static void ProcessParticleSystemsStructure(ParticleSystemsDO particleSystems, string xml)
        {
            var doc = XDocument.Parse(xml);
            
            // 处理每个effect的复杂结构
            if (particleSystems.Effects != null)
            {
                for (int i = 0; i < particleSystems.Effects.Count; i++)
                {
                    var effect = particleSystems.Effects[i];
                    var effectElement = doc.Root?.Elements("effect").ElementAt(i);
                    
                    if (effectElement != null && effect.Emitters != null)
                    {
                        ProcessEmittersStructure(effect.Emitters, effectElement);
                    }
                }
            }
        }

        /// <summary>
        /// 处理发射器结构
        /// </summary>
        /// <param name="emitters">发射器集合</param>
        /// <param name="effectElement">效果元素</param>
        private static void ProcessEmittersStructure(EmittersDO emitters, XElement effectElement)
        {
            var emittersElement = effectElement.Element("emitters");
            
            if (emittersElement != null)
            {
                for (int j = 0; j < emitters.EmitterList.Count; j++)
                {
                    var emitter = emitters.EmitterList[j];
                    var emitterElement = emittersElement.Elements("emitter").ElementAt(j);
                    
                    if (emitterElement != null)
                    {
                        // 检测空的children元素
                        emitter.HasEmptyChildren = emitterElement.Element("children") != null;
                        
                        // 检测空的flags元素
                        emitter.HasEmptyFlags = emitterElement.Element("flags") != null;
                        
                        // 检测空的parameters元素
                        emitter.HasEmptyParameters = emitterElement.Element("parameters") != null;
                        
                        // 处理parameters中的复杂结构
                        if (emitter.Parameters != null)
                        {
                            ProcessParametersStructure(emitter.Parameters, emitterElement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理参数结构
        /// </summary>
        /// <param name="parameters">参数容器</param>
        /// <param name="emitterElement">发射器元素</param>
        private static void ProcessParametersStructure(ParametersDO parameters, XElement emitterElement)
        {
            var parametersElement = emitterElement.Element("parameters");
            
            if (parametersElement != null)
            {
                // 检测decal_materials元素
                parameters.HasDecalMaterials = parametersElement.Element("decal_materials") != null;
                
                // 检测是否有空的parameters元素（即没有parameter子元素但有decal_materials）
                parameters.HasEmptyParameters = parametersElement.Elements("parameter").Count() == 0 && 
                                                 parametersElement.Element("decal_materials") != null;
            }
        }
    }
}
```

### 5. 分析和诊断接口

#### XmlStructureStats
```csharp
/// <summary>
/// XML结构统计信息
/// </summary>
public class XmlStructureStats
{
    /// <summary>
    /// 总元素数量
    /// </summary>
    public int TotalElements { get; set; }

    /// <summary>
    /// 总属性数量
    /// </summary>
    public int TotalAttributes { get; set; }

    /// <summary>
    /// 元素类型统计
    /// </summary>
    public Dictionary<string, int> ElementTypes { get; set; } = new Dictionary<string, int>();

    /// <summary>
    /// Effect数量
    /// </summary>
    public int EffectCount { get; set; }

    /// <summary>
    /// 第一个Effect是否有DecalMaterials
    /// </summary>
    public bool FirstEffectHasDecalMaterials { get; set; }

    /// <summary>
    /// 第一个Effect的Emitter数量
    /// </summary>
    public int FirstEffectEmitterCount { get; set; }

    /// <summary>
    /// 第一个Emitter是否有Children
    /// </summary>
    public bool FirstEmitterHasChildren { get; set; }

    /// <summary>
    /// 第一个Emitter是否有Flags
    /// </summary>
    public bool FirstEmitterHasFlags { get; set; }

    /// <summary>
    /// 第一个Emitter是否有Parameters
    /// </summary>
    public bool FirstEmitterHasParameters { get; set; }

    /// <summary>
    /// DecalMaterial数量
    /// </summary>
    public int DecalMaterialCount { get; set; }

    /// <summary>
    /// DecalMaterials数量
    /// </summary>
    public int DecalMaterialsCount { get; set; }

    /// <summary>
    /// Curve数量
    /// </summary>
    public int CurveCount { get; set; }

    /// <summary>
    /// Keys数量
    /// </summary>
    public int KeysCount { get; set; }

    /// <summary>
    /// Key数量
    /// </summary>
    public int KeyCount { get; set; }
}
```

#### ParticleSystemsAnalyzer
```csharp
/// <summary>
/// 粒子系统分析器
/// </summary>
public static class ParticleSystemsAnalyzer
{
    /// <summary>
    /// 分析粒子系统XML结构
    /// </summary>
    /// <param name="xml">XML字符串</param>
    /// <returns>结构统计信息</returns>
    public static XmlStructureStats AnalyzeStructure(string xml)
    {
        var doc = XDocument.Parse(xml);
        var stats = new XmlStructureStats();
        
        // 基本统计
        stats.TotalElements = doc.Descendants().Count();
        stats.TotalAttributes = doc.Descendants().Sum(e => e.Attributes().Count());
        
        // 元素类型统计
        stats.ElementTypes = doc.Descendants()
            .GroupBy(e => e.Name.LocalName)
            .ToDictionary(g => g.Key, g => g.Count());
        
        // Effect统计
        stats.EffectCount = doc.Root.Elements("effect").Count();
        
        // DecalMaterial统计
        stats.DecalMaterialCount = doc.Descendants("decal_material").Count();
        stats.DecalMaterialsCount = doc.Descendants("decal_materials").Count();
        
        // 曲线元素统计
        stats.CurveCount = doc.Descendants("curve").Count();
        stats.KeysCount = doc.Descendants("keys").Count();
        stats.KeyCount = doc.Descendants("key").Count();
        
        // 第一个Effect的详细统计
        var firstEffect = doc.Root.Elements("effect").FirstOrDefault();
        if (firstEffect != null)
        {
            stats.FirstEffectHasDecalMaterials = firstEffect.Element("emitters")?
                .Elements("emitter").FirstOrDefault()?
                .Element("parameters")?
                .Element("decal_materials") != null;
            
            var firstEffectEmitters = firstEffect.Element("emitters");
            if (firstEffectEmitters != null)
            {
                stats.FirstEffectEmitterCount = firstEffectEmitters.Elements("emitter").Count();
                
                var firstEmitter = firstEffectEmitters.Elements("emitter").FirstOrDefault();
                if (firstEmitter != null)
                {
                    stats.FirstEmitterHasChildren = firstEmitter.Element("children") != null;
                    stats.FirstEmitterHasFlags = firstEmitter.Element("flags") != null;
                    stats.FirstEmitterHasParameters = firstEmitter.Element("parameters") != null;
                }
            }
        }
        
        return stats;
    }

    /// <summary>
    /// 比较两个XML结构的差异
    /// </summary>
    /// <param name="original">原始XML</param>
    /// <param name="serialized">序列化后的XML</param>
    /// <returns>差异报告</returns>
    public static string CompareStructures(string original, string serialized)
    {
        var originalStats = AnalyzeStructure(original);
        var serializedStats = AnalyzeStructure(serialized);
        
        var report = "=== XML结构差异分析 ===\n\n";
        report += $"基本统计:\n";
        report += $"  原始元素数: {originalStats.TotalElements}\n";
        report += $"  序列化元素数: {serializedStats.TotalElements}\n";
        report += $"  元素差异: {originalStats.TotalElements - serializedStats.TotalElements}\n\n";
        
        report += $"  原始属性数: {originalStats.TotalAttributes}\n";
        report += $"  序列化属性数: {serializedStats.TotalAttributes}\n";
        report += $"  属性差异: {originalStats.TotalAttributes - serializedStats.TotalAttributes}\n\n";
        
        report += $"关键元素统计:\n";
        report += $"  Effect: {originalStats.EffectCount} -> {serializedStats.EffectCount}\n";
        report += $"  DecalMaterial: {originalStats.DecalMaterialCount} -> {serializedStats.DecalMaterialCount}\n";
        report += $"  DecalMaterials: {originalStats.DecalMaterialsCount} -> {serializedStats.DecalMaterialsCount}\n";
        report += $"  Curve: {originalStats.CurveCount} -> {serializedStats.CurveCount}\n";
        report += $"  Keys: {originalStats.KeysCount} -> {serializedStats.KeysCount}\n";
        report += $"  Key: {originalStats.KeyCount} -> {serializedStats.KeyCount}\n\n";
        
        report += "元素类型差异:\n";
        var allElementTypes = originalStats.ElementTypes.Keys
            .Union(serializedStats.ElementTypes.Keys)
            .OrderBy(x => x);
        
        foreach (var elementType in allElementTypes)
        {
            var originalCount = originalStats.ElementTypes.ContainsKey(elementType) ? 
                originalStats.ElementTypes[elementType] : 0;
            var serializedCount = serializedStats.ElementTypes.ContainsKey(elementType) ? 
                serializedStats.ElementTypes[elementType] : 0;
            
            if (originalCount != serializedCount)
            {
                report += $"  {elementType}: {originalCount} -> {serializedCount} (差异: {originalCount - serializedCount})\n";
            }
        }
        
        return report;
    }
}
```

### 6. 配置接口

#### ParticleSystemsSerializationOptions
```csharp
/// <summary>
/// 粒子系统序列化配置选项
/// </summary>
public class ParticleSystemsSerializationOptions
{
    /// <summary>
    /// 是否启用并行处理
    /// </summary>
    public bool EnableParallelProcessing { get; set; } = true;

    /// <summary>
    /// 最大内存使用量（MB）
    /// </summary>
    public int MaxMemoryUsageMB { get; set; } = 512;

    /// <summary>
    /// 是否启用详细日志记录
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = false;

    /// <summary>
    /// 是否验证XML结构
    /// </summary>
    public bool ValidateXmlStructure { get; set; } = true;

    /// <summary>
    /// 是否保留原始XML格式
    /// </summary>
    public bool PreserveOriginalFormatting { get; set; } = true;

    /// <summary>
    /// 是否处理空元素
    /// </summary>
    public bool ProcessEmptyElements { get; set; } = true;

    /// <summary>
    /// 是否启用性能优化
    /// </summary>
    public bool EnablePerformanceOptimization { get; set; } = true;
}
```

## 错误处理

### 1. 异常类型

#### ParticleSystemsSerializationException
```csharp
/// <summary>
/// 粒子系统序列化异常
/// </summary>
public class ParticleSystemsSerializationException : Exception
{
    public ParticleSystemsSerializationException(string message) : base(message) { }
    
    public ParticleSystemsSerializationException(string message, Exception innerException) 
        : base(message, innerException) { }
}
```

#### ParticleSystemsStructureException
```csharp
/// <summary>
/// 粒子系统结构异常
/// </summary>
public class ParticleSystemsStructureException : Exception
{
    public ParticleSystemsStructureException(string message) : base(message) { }
    
    public ParticleSystemsStructureException(string message, Exception innerException) 
        : base(message, innerException) { }
}
```

### 2. 错误代码

| 错误代码 | 描述 | 解决方案 |
|---------|------|----------|
| PXS_001 | XML格式错误 | 验证XML格式 |
| PXS_002 | 序列化失败 | 检查对象状态 |
| PXS_003 | 反序列化失败 | 验证XML结构 |
| PXS_004 | 内存不足 | 优化内存使用 |
| PXS_005 | 结构不匹配 | 检查映射逻辑 |

## 使用示例

### 1. 基本序列化
```csharp
// 反序列化
var xml = File.ReadAllText("particle_systems_hardcoded_misc1.xml");
var particleSystems = XmlTestUtils.DeserializeParticleSystems(xml);

// 修改对象
particleSystems.Effects[0].Name = "modified_effect";

// 序列化
var serializedXml = XmlTestUtils.SerializeParticleSystems(particleSystems, xml);
```

### 2. 结构分析
```csharp
// 分析结构
var stats = ParticleSystemsAnalyzer.AnalyzeStructure(xml);
Console.WriteLine($"总元素数: {stats.TotalElements}");
Console.WriteLine($"Effect数量: {stats.EffectCount}");
Console.WriteLine($"DecalMaterial数量: {stats.DecalMaterialCount}");

// 比较结构
var comparison = ParticleSystemsAnalyzer.CompareStructures(originalXml, serializedXml);
Console.WriteLine(comparison);
```

### 3. 配置使用
```csharp
// 创建配置
var options = new ParticleSystemsSerializationOptions
{
    EnableParallelProcessing = true,
    MaxMemoryUsageMB = 1024,
    EnableDetailedLogging = true,
    ValidateXmlStructure = true
};

// 使用配置进行序列化
var serializer = new ParticleSystemsSerializer(options);
var result = serializer.Serialize(particleSystems);
```

## 性能指标

### 1. 序列化性能
- **小文件** (< 100KB): < 100ms
- **中等文件** (100KB - 1MB): < 1s
- **大文件** (1MB - 10MB): < 5s
- **超大文件** (> 10MB): < 30s

### 2. 内存使用
- **基础使用**: < 100MB
- **大文件处理**: < 512MB
- **并行处理**: < 1GB

### 3. 并行处理效率
- **双核**: 1.5x - 1.8x 加速
- **四核**: 2.5x - 3.5x 加速
- **八核**: 4x - 6x 加速

## 版本兼容性

### 1. .NET版本支持
- **.NET 8.0**: 完全支持
- **.NET 9.0**: 完全支持（推荐）
- **.NET 7.0**: 基本支持

### 2. 依赖版本
- **System.Xml**: 4.0.0+
- **System.Linq**: 4.0.0+
- **System.Threading.Tasks**: 4.0.0+

## 安全考虑

### 1. 输入验证
- 验证XML格式和结构
- 检查文件大小限制
- 防止XML注入攻击

### 2. 内存安全
- 限制最大内存使用
- 实现内存监控
- 处理内存不足情况

### 3. 异常安全
- 捕获和处理所有异常
- 提供有意义的错误信息
- 实现资源清理

## 总结

本API规范为ParticleSystems XML序列化系统提供了完整的接口定义，包括数据模型、序列化接口、映射接口、分析接口和配置接口。通过这些接口，用户可以高效地处理复杂的粒子系统XML文件，确保序列化和反序列化的准确性和性能。

该规范具有良好的扩展性，支持未来功能的添加和优化，同时保持了与现有系统的兼容性。