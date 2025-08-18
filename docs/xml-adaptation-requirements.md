# Bannerlord XML 文件适配需求文档

## 执行摘要

本文档详细分析了BannerlordModEditor项目中尚未适配的XML文件，提供了完整的适配需求、优先级排序和技术实现建议。基于对项目当前状态的深入分析，我们识别出了多个需要适配的XML文件类型，并按照复杂度和重要性进行了分类。

## 当前状态分析

### 已适配文件统计
- **总XML文件数量**: 90+ 个
- **已适配文件数量**: 45 个
- **适配完成率**: ~49%
- **测试覆盖率**: 830 个测试用例 (100% 通过)

### 项目架构现状
- 采用DO/DTO架构模式处理复杂XML序列化
- 按功能域组织命名空间 (Audio, Engine, Configuration, Data, Multiplayer, Map, Game)
- 完整的单元测试覆盖，确保数据完整性
- 支持大型XML文件的分片测试

## 未适配XML文件分析

### 1. 高优先级未适配文件

#### 1.1 Multiplayer/MPClassDivisions.xml
**文件大小**: 219KB  
**复杂度**: ⭐⭐⭐ **非常复杂**  
**功能域**: Multiplayer

**结构分析**:
```xml
<MPClassDivisions>
  <MPClassDivision id="mp_light_infantry_vlandia">
    <Perks>
      <Perk game_mode="all">
        <OnSpawnEffect type="ArmorOnSpawn" value="9" />
        <RandomOnSpawnEffect type="RandomEquipmentOnSpawn">
          <Group>
            <Item slot="Body" item="mp_cloth_tunic" />
          </Group>
        </RandomOnSpawnEffect>
      </Perk>
    </Perks>
  </MPClassDivision>
</MPClassDivisions>
```

**技术挑战**:
- 复杂的嵌套结构 (MPClassDivision → Perks → Perk → Effects)
- 多种效果类型 (OnSpawnEffect, RandomOnSpawnEffect)
- 条件逻辑和游戏模式特定配置
- 大量的数值属性和字符串配置

**适配优先级**: **高** - 多人游戏核心功能

#### 1.2 Layouts目录文件
**文件数量**: 7个  
**复杂度**: ⭐⭐ **复杂**  
**功能域**: Engine/Layouts

**文件列表**:
- `animations_layout.xml` (1.9KB)
- `flora_kinds_layout.xml` (4.5KB)
- `item_holsters_layout.xml` (7.4KB)
- `particle_system_layout.xml` (20KB)
- `physics_materials_layout.xml` (2.3KB)
- `skeletons_layout.xml` (26KB) - ⭐⭐⭐ **非常复杂**
- `skinned_decals_layout.xml` (638B)

**结构分析** (以skeletons_layout.xml为例):
```xml
<base type="string">
  <layouts>
    <layout class="skeleton" version="0.1">
      <columns>
        <column id="0" width="400" />
      </columns>
      <insertion_definitions>
        <insertion_definition label="Add Hinge Joint">
          <default_node>
            <hinge_joint name="default_hinge">
              <!-- 复杂的关节配置 -->
            </hinge_joint>
          </default_node>
        </insertion_definition>
      </insertion_definitions>
    </layout>
  </layouts>
</base>
```

**技术挑战**:
- 元数据定义结构，不是普通数据
- 复杂的默认节点配置
- 多种关节类型 (hinge_joint, d6_joint)
- 动态UI布局定义

**适配优先级**: **中高** - 编辑器UI核心功能

### 2. 中等优先级未适配文件

#### 2.1 terrain_materials.xml
**文件大小**: 估算 > 50KB  
**复杂度**: ⭐⭐ **复杂**  
**功能域**: Map/Terrain

**结构分析**:
```xml
<terrain_materials>
  <terrain_material name="default">
    <textures>
      <texture type="diffusemap" name="editor_grid_8" />
      <texture type="areamap" name="none" />
      <texture type="normalmap" name="none" />
    </textures>
  </terrain_material>
</terrain_materials>
```

**技术挑战**:
- 多层纹理配置
- 复杂的材质属性
- 物理材质关联
- 地形编辑功能

**适配优先级**: **中** - 地形编辑功能

#### 2.2 大型数据文件
以下文件虽然很大，但结构相对简单：

**文件列表**:
- `full_movement_sets.xml` - 移动动作集定义
- `skeleton_scales.xml` - 骨骼缩放配置
- `monster_usage_sets.xml` - 怪物使用集

**技术挑战**:
- 文件体积大，需要性能优化
- 大量重复结构
- 需要批量处理能力

**适配优先级**: **中** - 游戏机制支持

### 3. 低优先级未适配文件

#### 3.1 可能已过时的文件
根据覆盖报告，以下文件可能已经过时或不需要适配：

- `decal_materials.xml` - 可能不存在或已废弃
- `flora_canopies.xml` - 可能不存在或已废弃
- 其他在报告中提及但实际不存在的文件

#### 3.2 超大型文件
以下文件虽然重要，但由于体积巨大，建议长期规划：

**粒子系统文件**:
- `particle_systems_hardcoded_misc1.xml` (1.7MB)
- `particle_systems2.xml` (1.6MB)
- `particle_systems_hardcoded_misc2.xml` (1.4MB)
- `particle_systems_old.xml` (574KB)
- `particle_systems_general.xml` (429KB)

**游戏数据文件**:
- `skins.xml` (460KB) - 角色皮肤定义
- `action_sets.xml` (883KB) - 动画动作集
- `action_types.xml` (425KB) - 动画动作类型
- `flora_kinds.xml` (1.5MB) - 植被定义
- `crafting_pieces.xml` (371KB) - 制作部件
- `item_usage_sets.xml` (388KB) - 物品使用集
- `module_strings.xml` (271KB) - 模块字符串本地化
- `prebaked_animations.xml` (569KB) - 预烘焙动画

## 技术实现建议

### 1. 架构模式

#### 1.1 DO/DTO模式（推荐用于复杂文件）
```csharp
// 对于MPClassDivisions等复杂文件
public class MPClassDivisionDO
{
    [XmlElement("MPClassDivision")]
    public List<MPClassDivisionItemDO> Divisions { get; set; }
}

public class MPClassDivisionItemDO
{
    [XmlAttribute("id")]
    public string Id { get; set; }
    
    [XmlElement("Perks")]
    public PerksDO Perks { get; set; }
    
    // ShouldSerialize 方法用于精确控制序列化
    public bool ShouldSerializePerks() => Perks != null && Perks.PerkList.Count > 0;
}
```

#### 1.2 简单模型模式（用于简单文件）
```csharp
// 对于terrain_materials等结构简单的文件
[XmlRoot("terrain_materials")]
public class TerrainMaterials
{
    [XmlElement("terrain_material")]
    public List<TerrainMaterial> Materials { get; set; }
}
```

### 2. 命名空间组织

#### 2.1 建议的命名空间结构
```
BannerlordModEditor.Common.Models/
├── Multiplayer/
│   ├── MPClassDivisions.cs
│   └── MPClassDivisionsDO.cs
├── Engine/
│   ├── Layouts/
│   │   ├── AnimationsLayout.cs
│   │   ├── SkeletonsLayout.cs
│   │   └── ParticleSystemLayout.cs
│   └── Terrain/
│       └── TerrainMaterials.cs
├── Data/
│   ├── FullMovementSets.cs
│   └── SkeletonScales.cs
└── Mappers/
    ├── MPClassDivisionsMapper.cs
    └── TerrainMaterialsMapper.cs
```

### 3. 性能优化策略

#### 3.1 大型文件处理
```csharp
// 对于超大型文件，考虑使用流式处理
public class LargeXmlProcessor
{
    public async Task<List<T>> ProcessLargeFileAsync<T>(string filePath)
    {
        // 使用 XmlReader 进行流式处理
        // 分批处理以避免内存问题
    }
}
```

#### 3.2 缓存策略
```csharp
// 对于频繁访问的文件，实现缓存
public class XmlCacheService
{
    private readonly Dictionary<string, object> _cache = new();
    
    public T GetOrLoad<T>(string filePath, Func<string, T> loader)
    {
        if (_cache.TryGetValue(filePath, out var cached))
            return (T)cached;
            
        var result = loader(filePath);
        _cache[filePath] = result;
        return result;
    }
}
```

### 4. 测试策略

#### 4.1 测试数据管理
```csharp
// 为每个新适配的XML创建测试数据
public class MPClassDivisionsXmlTests
{
    [Fact]
    public void Deserialize_SampleData_ReturnsValidObject()
    {
        // 测试反序列化
    }
    
    [Fact]
    public void Serialize_DeserializedData_ProducesEquivalentXml()
    {
        // 测试序列化一致性
    }
}
```

#### 4.2 性能测试
```csharp
// 对于大型文件，添加性能测试
public class LargeFilePerformanceTests
{
    [Fact]
    public void Load_LargeXml_CompletesWithinTimeLimit()
    {
        // 性能基准测试
    }
}
```

## 优先级排序和实施计划

### 第一阶段：高价值目标 (2-3周)
1. **MPClassDivisions.xml** - 多人游戏核心功能
2. **skeletons_layout.xml** - 编辑器UI核心
3. **terrain_materials.xml** - 地形编辑功能

### 第二阶段：功能完善 (3-4周)
1. **其他Layout文件** - 完善编辑器UI
2. **movement_sets.xml** - 游戏机制支持
3. **skeleton_scales.xml** - 骨骼系统

### 第三阶段：长期优化 (长期规划)
1. **超大型粒子系统文件** - 需要专门的性能优化
2. **其他超大型数据文件** - 根据实际需求逐步适配

## 风险评估和缓解策略

### 1. 技术风险
- **风险**: 超大型文件可能导致内存问题
- **缓解**: 实现流式处理和分批加载

### 2. 兼容性风险
- **风险**: XML结构变化导致适配失败
- **缓解**: 实现灵活的解析器和版本管理

### 3. 性能风险
- **风险**: 大量XML文件影响应用启动速度
- **缓解**: 实现延迟加载和缓存机制

## 成功标准

### 1. 功能标准
- [ ] 所有目标XML文件成功适配
- [ ] 序列化/反序列化测试100%通过
- [ ] 数据完整性验证通过

### 2. 性能标准
- [ ] 大型文件加载时间 < 5秒
- [ ] 内存使用量在合理范围内
- [ ] 支持并发文件处理

### 3. 质量标准
- [ ] 代码覆盖率 > 90%
- [ ] 所有公共API有完整文档
- [ ] 通过代码审查和静态分析

## 总结

BannerlordModEditor项目的XML适配工作已经完成了近一半，取得了显著进展。剩余的未适配文件主要集中在多人游戏、编辑器UI和大型数据文件三个方面。通过采用DO/DTO架构模式、合理的命名空间组织和性能优化策略，我们可以高效地完成剩余的适配工作。

建议优先处理高价值的多人游戏和编辑器UI相关文件，然后根据实际需求逐步适配大型数据文件。整个过程需要注重测试覆盖率和性能优化，确保最终产品的质量和用户体验。