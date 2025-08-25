# Bannerlord XML 适配技术实现指南

## 概述

本文档为BannerlordModEditor项目中XML文件适配工作提供详细的技术实现建议和优先级排序。基于对项目架构和未适配XML文件的深入分析，本文档提供了具体的实现策略、代码示例和最佳实践。

## 技术架构建议

### 1. DO/DTO 架构模式选择

#### 1.1 何时使用DO/DTO模式
**使用场景**:
- 复杂的嵌套XML结构 (如MPClassDivisions)
- 需要精确控制序列化行为的文件
- 包含大量业务逻辑的XML类型
- 大型文件 (>100KB)

**简化实现示例**:
```csharp
// 简化实现：对于简单结构，直接使用基础模型
[XmlRoot("terrain_materials")]
public class TerrainMaterials
{
    [XmlElement("terrain_material")]
    public List<TerrainMaterial> Materials { get; set; } = new();
}

// 对于复杂结构，使用DO/DTO模式
public class MPClassDivisionsDO
{
    [XmlElement("MPClassDivision")]
    public List<MPClassDivisionDO> Divisions { get; set; } = new();
    
    [XmlIgnore]
    public bool HasDivisions => Divisions.Count > 0;
    
    public bool ShouldSerializeDivisions() => HasDivisions;
}
```

#### 1.2 命名空间组织策略
```
BannerlordModEditor.Common.Models/
├── Multiplayer/              # 多人游戏相关
│   ├── MPClassDivisions/
│   │   ├── MPClassDivisionsDO.cs
│   │   ├── MPClassDivisionsDTO.cs
│   │   └── MPClassDivisionsMapper.cs
│   └── ...
├── Engine/Layouts/          # 编辑器UI布局
│   ├── SkeletonsLayout/
│   │   ├── SkeletonsLayoutDO.cs
│   │   ├── SkeletonsLayoutDTO.cs
│   │   └── SkeletonsLayoutMapper.cs
│   └── ...
├── Map/Terrain/            # 地形系统
│   ├── TerrainMaterials.cs
│   └── ...
└── Data/                   # 游戏数据
    ├── MovementSets.cs
    ├── SkeletonScales.cs
    └── ...
```

### 2. 具体实现建议

#### 2.1 MPClassDivisions.xml 实现

**技术挑战**:
- 复杂的嵌套结构 (MPClassDivision → Perks → Perk → Effects)
- 多种效果类型 (OnSpawnEffect, RandomOnSpawnEffect)
- 条件逻辑和游戏模式特定配置

**实现策略**:
```csharp
// DO模型
public class MPClassDivisionDO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
    
    [XmlAttribute("hero")]
    public string Hero { get; set; } = string.Empty;
    
    [XmlAttribute("troop")]
    public string Troop { get; set; } = string.Empty;
    
    [XmlElement("Perks")]
    public PerksDO Perks { get; set; } = new();
    
    [XmlIgnore]
    public bool HasPerks => Perks != null && Perks.Perks.Count > 0;
    
    public bool ShouldSerializePerks() => HasPerks;
}

public class PerksDO
{
    [XmlElement("Perk")]
    public List<PerkDO> Perks { get; set; } = new();
}

public class PerkDO
{
    [XmlAttribute("game_mode")]
    public string GameMode { get; set; } = "all";
    
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    
    [XmlElement("OnSpawnEffect")]
    public List<OnSpawnEffectDO> OnSpawnEffects { get; set; } = new();
    
    [XmlElement("RandomOnSpawnEffect")]
    public List<RandomOnSpawnEffectDO> RandomOnSpawnEffects { get; set; } = new();
}

// 映射器
public static class MPClassDivisionsMapper
{
    public static MPClassDivisionsDTO ToDTO(MPClassDivisionsDO source)
    {
        if (source == null) return null;
        
        return new MPClassDivisionsDTO
        {
            Divisions = source.Divisions?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<MPClassDivisionDTO>()
        };
    }
    
    public static MPClassDivisionsDO ToDO(MPClassDivisionsDTO source)
    {
        if (source == null) return null;
        
        return new MPClassDivisionsDO
        {
            Divisions = source.Divisions?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<MPClassDivisionDO>()
        };
    }
}
```

#### 2.2 Layouts文件实现

**技术挑战**:
- 元数据定义结构，不是普通数据
- 复杂的默认节点配置
- 多种关节类型 (hinge_joint, d6_joint)

**实现策略**:
```csharp
// 基础布局模型
[XmlRoot("base")]
public class LayoutsBaseDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";
    
    [XmlElement("layouts")]
    public LayoutsDO Layouts { get; set; } = new();
    
    [XmlIgnore]
    public bool HasLayouts => Layouts != null && Layouts.Layouts.Count > 0;
    
    public bool ShouldSerializeLayouts() => HasLayouts;
}

public class LayoutsDO
{
    [XmlElement("layout")]
    public List<LayoutDO> Layouts { get; set; } = new();
}

public class LayoutDO
{
    [XmlAttribute("class")]
    public string Class { get; set; } = string.Empty;
    
    [XmlAttribute("version")]
    public string Version { get; set; } = "0.1";
    
    [XmlAttribute("xml_tag")]
    public string XmlTag { get; set; } = string.Empty;
    
    [XmlElement("columns")]
    public ColumnsDO Columns { get; set; } = new();
    
    [XmlElement("insertion_definitions")]
    public InsertionDefinitionsDO InsertionDefinitions { get; set; } = new();
}

// 关节类型的多态处理
public abstract class JointDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    
    [XmlAttribute("bone1")]
    public string Bone1 { get; set; } = string.Empty;
    
    [XmlAttribute("bone2")]
    public string Bone2 { get; set; } = string.Empty;
    
    [XmlAttribute("pos")]
    public string Position { get; set; } = "0.000, 0.000, 0.000";
}

[XmlType("hinge_joint")]
public class HingeJointDO : JointDO
{
    [XmlAttribute("min_angle")]
    public int MinAngle { get; set; } = 0;
    
    [XmlAttribute("max_angle")]
    public int MaxAngle { get; set; } = 90;
}

[XmlType("d6_joint")]
public class D6JointDO : JointDO
{
    [XmlAttribute("axis_lock_x")]
    public string AxisLockX { get; set; } = "locked";
    
    [XmlAttribute("axis_lock_y")]
    public string AxisLockY { get; set; } = "locked";
    
    [XmlAttribute("axis_lock_z")]
    public string AxisLockZ { get; set; } = "locked";
    
    [XmlAttribute("twist_lock")]
    public string TwistLock { get; set; } = "limited";
}
```

#### 2.3 TerrainMaterials.xml 实现

**技术挑战**:
- 多层纹理配置
- 复杂的材质属性
- 物理材质关联

**实现策略**:
```csharp
[XmlRoot("terrain_materials")]
public class TerrainMaterials
{
    [XmlElement("terrain_material")]
    public List<TerrainMaterial> Materials { get; set; } = new();
}

public class TerrainMaterial
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    
    [XmlAttribute("is_enabled")]
    public bool IsEnabled { get; set; } = true;
    
    [XmlAttribute("physics_material")]
    public string PhysicsMaterial { get; set; } = string.Empty;
    
    [XmlElement("textures")]
    public Textures Textures { get; set; } = new();
    
    // 复杂属性的简化实现
    [XmlAttribute("pitch_roll_yaw")]
    public string PitchRollYaw { get; set; } = "0.000, 0.000, 0.000";
    
    [XmlAttribute("scale")]
    public string Scale { get; set; } = "5.000, 5.000";
    
    [XmlIgnore]
    public bool HasTextures => Textures != null && Textures.TextureList.Count > 0;
    
    public bool ShouldSerializeTextures() => HasTextures;
}

public class Textures
{
    [XmlElement("texture")]
    public List<Texture> TextureList { get; set; } = new();
}

public class Texture
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;
    
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
}
```

### 3. 性能优化策略

#### 3.1 大型文件处理优化

**简化实现 - 流式处理**:
```csharp
public class LargeXmlProcessor
{
    public async Task<List<T>> ProcessLargeFileAsync<T>(string filePath, 
        Func<XElement, T> elementProcessor)
    {
        var results = new List<T>();
        
        using var reader = XmlReader.Create(filePath);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(GetElementName<T>()))
            {
                var element = XElement.Load(reader.ReadSubtree());
                var result = elementProcessor(element);
                if (result != null)
                {
                    results.Add(result);
                }
            }
        }
        
        return results;
    }
    
    private string GetElementName<T>()
    {
        // 返回类型对应的XML元素名称
        return typeof(T).Name.Replace("DO", "").ToLowerInvariant();
    }
}
```

#### 3.2 缓存策略

**简化实现 - 内存缓存**:
```csharp
public class XmlCacheService
{
    private readonly Dictionary<string, (object data, DateTime timestamp)> _cache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
    
    public T GetOrLoad<T>(string filePath, Func<string, T> loader)
    {
        if (_cache.TryGetValue(filePath, out var cached))
        {
            if (DateTime.Now - cached.timestamp < _cacheDuration)
            {
                return (T)cached.data;
            }
        }
        
        var result = loader(filePath);
        _cache[filePath] = (result, DateTime.Now);
        return result;
    }
    
    public void InvalidateCache(string filePath)
    {
        _cache.Remove(filePath);
    }
}
```

### 4. 测试策略

#### 4.1 测试数据管理

**简化实现 - 测试基类**:
```csharp
public abstract class XmlTestBase
{
    protected T LoadTestData<T>(string fileName) where T : class
    {
        var testDataPath = Path.Combine("TestData", fileName);
        var xml = File.ReadAllText(testDataPath);
        return XmlTestUtils.Deserialize<T>(xml);
    }
    
    protected void VerifySerializationRoundtrip<T>(T obj, string originalXml) where T : class
    {
        var serialized = XmlTestUtils.Serialize(obj, originalXml);
        var areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serialized);
        Assert.True(areEqual, "Serialization roundtrip failed");
    }
}
```

#### 4.2 具体测试示例

**MPClassDivisions测试**:
```csharp
public class MPClassDivisionsXmlTests : XmlTestBase
{
    [Fact]
    public void Deserialize_SampleData_ReturnsValidObject()
    {
        // Arrange
        var testData = LoadTestData<MPClassDivisionsDO>("mpclassdivisions.xml");
        
        // Assert
        Assert.NotNull(testData);
        Assert.NotEmpty(testData.Divisions);
        
        var firstDivision = testData.Divisions.First();
        Assert.NotNull(firstDivision.Id);
        Assert.NotNull(firstDivision.Perks);
    }
    
    [Fact]
    public void Serialize_DeserializedData_ProducesEquivalentXml()
    {
        // Arrange
        var originalXml = File.ReadAllText("TestData/mpclassdivisions.xml");
        var obj = XmlTestUtils.Deserialize<MPClassDivisionsDO>(originalXml);
        
        // Act
        var serialized = XmlTestUtils.Serialize(obj, originalXml);
        
        // Assert
        var areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serialized);
        Assert.True(areEqual, "Serialization roundtrip failed");
    }
}
```

## 优先级排序和实施计划

### 第一优先级 (2-3周)

#### 1. MPClassDivisions.xml (多人游戏核心功能)
**工作量**: 5-7天  
**优先级**: 🔴 **高**  
**技术复杂度**: ⭐⭐⭐  

**实施步骤**:
1. 创建DO/DTO模型结构 (2天)
2. 实现映射器 (1天)
3. 编写单元测试 (2天)
4. 集成测试和验证 (1-2天)
5. 文档更新 (0.5天)

**关键挑战**:
- 复杂的嵌套结构处理
- 多种效果类型的统一处理
- 条件逻辑的正确实现

#### 2. SkeletonsLayout.xml (编辑器UI核心)
**工作量**: 6-8天  
**优先级**: 🔴 **高**  
**技术复杂度**: ⭐⭐⭐  

**实施步骤**:
1. 分析XML结构和元数据定义 (2天)
2. 创建多态关节类型模型 (2天)
3. 实现复杂的default_node处理 (2天)
4. 编写测试和验证 (2天)
5. UI集成测试 (1天)

**关键挑战**:
- 元数据定义结构的特殊处理
- 多态关节类型的统一管理
- 复杂的默认节点配置

### 第二优先级 (3-4周)

#### 3. TerrainMaterials.xml (地形编辑功能)
**工作量**: 3-4天  
**优先级**: 🟡 **中**  
**技术复杂度**: ⭐⭐  

**实施步骤**:
1. 创建基础模型 (1天)
2. 实现纹理配置处理 (1天)
3. 物理材质关联 (0.5天)
4. 测试和验证 (1天)
5. UI集成 (0.5天)

#### 4. MovementSets.xml (游戏机制支持)
**工作量**: 3-4天  
**优先级**: 🟡 **中**  
**技术复杂度**: ⭐⭐  

#### 5. SkeletonScales.xml (骨骼系统)
**工作量**: 2-3天  
**优先级**: 🟡 **中**  
**技术复杂度**: ⭐  

### 第三优先级 (4-6周)

#### 6. 其他Layouts文件 (编辑器UI完善)
**工作量**: 8-10天  
**优先级**: 🟡 **中**  
**技术复杂度**: ⭐⭐  

**包含文件**:
- animations_layout.xml
- flora_kinds_layout.xml
- item_holsters_layout.xml
- particle_system_layout.xml
- physics_materials_layout.xml
- skinned_decals_layout.xml

#### 7. 性能优化 (系统性能提升)
**工作量**: 10-14天  
**优先级**: 🟢 **低**  
**技术复杂度**: ⭐⭐⭐  

**优化内容**:
- 大型文件流式处理
- 内存缓存机制
- 增量更新优化
- 性能监控和调优

### 第四优先级 (长期规划)

#### 8. 超大型文件处理 (长期目标)
**工作量**: 4-6周  
**优先级**: 🟢 **低**  
**技术复杂度**: ⭐⭐⭐⭐  

**包含文件**:
- particle_systems_hardcoded_misc1.xml (1.7MB)
- particle_systems2.xml (1.6MB)
- particle_systems_hardcoded_misc2.xml (1.4MB)
- skins.xml (460KB)
- action_sets.xml (883KB)
- action_types.xml (425KB)
- flora_kinds.xml (1.5MB)

## 风险评估和缓解策略

### 高风险项目

#### 1. MPClassDivisions.xml 复杂度风险
**风险**: 嵌套结构过于复杂，可能导致性能问题
**缓解**: 
- 实现懒加载机制
- 分批处理大型数据集
- 添加性能监控

#### 2. Layouts文件架构理解风险
**风险**: 对编辑器UI架构理解不足，实现可能偏离需求
**缓解**:
- 深入研究现有代码结构
- 与架构师充分沟通
- 创建原型验证

#### 3. 超大型文件性能风险
**风险**: 超大型文件可能导致内存溢出或性能严重下降
**缓解**:
- 实现流式处理
- 添加文件大小限制
- 实现渐进式加载

### 中等风险项目

#### 1. 数据完整性风险
**风险**: XML适配过程中可能丢失数据或精度
**缓解**:
- 实现完整的测试覆盖
- 添加数据验证机制
- 实现数据备份功能

#### 2. 向后兼容性风险
**风险**: 新的适配可能破坏现有功能
**缓解**:
- 实现版本控制
- 添加兼容性测试
- 保持现有API不变

## 技术债务和优化建议

### 1. 代码重构建议

#### 1.1 统一模型基类
```csharp
// 建议创建统一的模型基类
public abstract class XmlModelBase
{
    public virtual bool ShouldSerialize<T>(T value) where T : class
    {
        return value != null;
    }
    
    public virtual bool ShouldSerialize<T>(List<T> list) where T : class
    {
        return list != null && list.Count > 0;
    }
}
```

#### 1.2 统一映射器接口
```csharp
public interface IMapper<TDO, TDTO>
{
    TDTO ToDTO(TDO source);
    TDO ToDO(TDTO source);
    List<TDTO> ToDTO(List<TDO> source);
    List<TDO> ToDO(List<TDTO> source);
}
```

### 2. 性能优化建议

#### 2.1 并行处理
```csharp
public class ParallelXmlProcessor
{
    public List<T> ProcessFilesParallel<T>(string[] filePaths, Func<string, T> processor)
    {
        var results = new ConcurrentBag<T>();
        
        Parallel.ForEach(filePaths, filePath =>
        {
            try
            {
                var result = processor(filePath);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不中断处理
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        });
        
        return results.ToList();
    }
}
```

#### 2.2 内存优化
```csharp
public class MemoryOptimizedXmlLoader
{
    public T LoadOptimized<T>(string filePath) where T : class, new()
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = XmlReader.Create(stream, new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true
        });
        
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(reader);
    }
}
```

## 成功标准和质量指标

### 功能标准
- [ ] 所有目标XML文件成功适配
- [ ] 序列化/反序列化测试100%通过
- [ ] 数据完整性验证通过
- [ ] 用户界面功能完整

### 性能标准
- [ ] 大型文件加载时间 < 10秒
- [ ] 内存使用量在文件大小的3倍以内
- [ ] 系统响应时间 < 200ms
- [ ] 支持并发文件处理

### 质量标准
- [ ] 代码覆盖率 > 90%
- [ ] 所有公共API有完整文档
- [ ] 通过静态代码分析
- [ ] 无严重的性能问题

### 用户体验标准
- [ ] 操作流程直观易用
- [ ] 错误信息清晰明了
- [ ] 性能表现良好
- [ ] 功能满足实际需求

## 总结和建议

### 关键建议

1. **优先处理高价值目标**: 集中资源完成MPClassDivisions.xml和Layouts文件的适配，这些是多人游戏和编辑器UI的核心功能。

2. **采用渐进式开发**: 将大型适配任务分解为小的可管理的迭代，每个迭代都有明确的目标和可交付成果。

3. **注重测试质量**: 为每个适配的XML文件创建完整的测试套件，确保数据完整性和功能正确性。

4. **考虑性能优化**: 对于大型文件，提前规划性能优化策略，避免后期重构。

5. **保持代码一致性**: 遵循现有的架构模式和编码规范，确保代码质量和可维护性。

### 实施建议

1. **第一阶段** (2-3周): 专注于核心功能适配，建立技术基础
2. **第二阶段** (3-4周): 完善主要功能，提升用户体验
3. **第三阶段** (4-6周): 优化性能，处理复杂场景
4. **第四阶段** (长期): 处理超大型文件，完善边缘情况

通过按照这个实施计划和技术建议，可以高效地完成BannerlordModEditor项目的XML适配工作，为用户提供功能完整、性能优秀的Mod编辑工具。