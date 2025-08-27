# XML映射类实现文档

## 概述

本文档详细列出了BannerlordModEditor项目中所有XML映射类的实现方式。项目采用DO/DTO（Domain Object/Data Transfer Object）架构模式，确保XML序列化/反序列化过程中数据的一致性和完整性。

## 架构模式

### DO/DTO模式
- **DO (Domain Object)**: 领域对象，包含业务逻辑和验证规则
- **DTO (Data Transfer Object)**: 数据传输对象，专门用于XML序列化/反序列化
- **Mapper**: 对象映射器，负责DO和DTO之间的双向转换

### 核心优势
1. **关注点分离**: 业务逻辑与数据表示分离
2. **精确控制**: 对XML序列化行为进行细粒度控制
3. **数据一致性**: 确保序列化/反序列化后数据不发生变化
4. **可维护性**: 便于修改和扩展

## XML映射类列表

### 1. CombatParameters (战斗参数)

**文件路径**: 
- DO: `BannerlordModEditor.Common/Models/DO/CombatParametersDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/CombatParametersDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/CombatParametersMapper.cs`

**XML结构**:
```xml
<base type="combat_parameters">
    <definitions>
        <def name="param1" val="value1"/>
        <def name="param2" val="value2"/>
    </definitions>
    <combat_parameters>
        <combat_parameter id="param1" collision_check_starting_percent="0.1"/>
        <combat_parameter id="param2" collision_damage_starting_percent="0.2"/>
    </combat_parameters>
</base>
```

**实现特点**:
- 使用`ShouldSerialize*`方法精确控制属性序列化
- 支持空元素保留（`HasEmptyCombatParameters`标记）
- 包含复杂的嵌套结构（definitions > def > combat_parameter）
- 支持自定义碰撞体（`CustomCollisionCapsule`）

### 2. Credits (制作人员名单)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/CreditsDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/CreditsDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/CreditsMapper.cs`

**XML结构**:
```xml
<Credits>
    <Category Text="Programming">
        <Entry Text="John Doe"/>
        <Section Text="Lead Programmers">
            <Entry Text="Jane Smith"/>
            <EmptyLine/>
        </Section>
        <EmptyLine/>
    </Category>
    <LoadFromFile Name="additional_credits.xml"/>
</Credits>
```

**实现特点**:
- 使用`List<object> Elements`保持元素原始顺序
- 支持多种元素类型（Section、Entry、EmptyLine、LoadFromFile、Image）
- 通过`XmlElement`的`Type`属性实现多态序列化
- 提供便捷属性（`Sections`、`Entries`、`EmptyLines`等）用于业务访问

### 3. ActionTypes (动作类型)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/ActionTypesDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/ActionTypesDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/ActionTypesMapper.cs`

**XML结构**:
```xml
<action_types>
    <action name="attack" type="offensive" usage_direction="forward" action_stage="start"/>
    <action name="defend" type="defensive" usage_direction="backward"/>
</action_types>
```

**实现特点**:
- 简单的列表结构
- 所有属性都使用`ShouldSerialize*`方法控制序列化
- 支持可选属性（有些action可能没有某些属性）

### 4. BoneBodyTypes (骨骼身体类型)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/BoneBodyTypesDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/BoneBodyTypesDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/BoneBodyTypesMapper.cs`

**XML结构**:
```xml
<bone_body_types>
    <bone_body_type name="human" skeleton="human_skeleton">
        <bone name="head" physics_material="flesh"/>
        <bone name="torso" physics_material="bone"/>
    </bone_body_type>
</bone_body_types>
```

**实现特点**:
- 包含枚举类型和复杂属性
- 支持嵌套的骨骼定义
- 使用`ShouldSerialize*`方法控制属性序列化

### 5. ActionSets (动作集合)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/ActionSetsDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/ActionSetsDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/ActionSetsMapper.cs`

**XML结构**:
```xml
<action_sets>
    <action_set name="infantry">
        <action name="walk" anim="walk_anim"/>
        <action name="run" anim="run_anim"/>
    </action_set>
</action_sets>
```

**实现特点**:
- 动画和动作的复杂关系映射
- 支持动作集合的嵌套结构
- 精确控制动画引用的序列化

### 6. CollisionInfos (碰撞信息)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/CollisionInfosDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/CollisionInfosDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/CollisionInfosMapper.cs`

**XML结构**:
```xml
<collision_infos>
    <collision_info name="wall">
        <mesh name="wall_mesh"/>
        <physics_material name="stone"/>
    </collision_info>
</collision_infos>
```

**实现特点**:
- 碰撞检测的复杂配置
- 支持网格和物理材质的引用
- 多层嵌套的碰撞参数

### 7. MapIcons (地图图标)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/MapIconsDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/MapIconsDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/MapIconsMapper.cs`

**XML结构**:
```xml
<map_icons>
    <map_icon name="castle">
        <mesh name="castle_mesh"/>
        <scale value="1.0"/>
        <color r="1.0" g="1.0" b="1.0" a="1.0"/>
    </map_icon>
</map_icons>
```

**实现特点**:
- 地图图标的多层嵌套结构
- 支持颜色和缩放参数
- 复杂的网格引用系统

### 8. ItemHolsters (物品挂载点)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/ItemHolstersDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/ItemHolstersDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/ItemHolstersMapper.cs`

**XML结构**:
```xml
<item_holsters>
    <item_holster name="sword_holster">
        <bone name="hip_bone"/>
        <offset x="0.0" y="0.0" z="0.0"/>
        <rotation x="0.0" y="0.0" z="0.0"/>
    </item_holster>
</item_holsters>
```

**实现特点**:
- 物品挂载点的3D位置和旋转
- 支持骨骼引用和偏移量
- 精确的空间变换参数

### 9. MpItems (多人游戏物品)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/MpItemsDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/MpItemsDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/MpItemsMapper.cs`

**XML结构**:
```xml
<mp_items>
    <mp_item name="sword_mp">
        <item name="sword"/>
        <multiplayer_class name="infantry"/>
        <cost value="100"/>
    </mp_item>
</mp_items>
```

**实现特点**:
- 多人游戏特有的物品属性
- 支持职业分类和成本系统
- 复杂的多人游戏平衡参数

### 10. MpCraftingPieces (多人游戏制作部件)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/MpCraftingPiecesDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/MpCraftingPiecesDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/MpCraftingPiecesMapper.cs`

**XML结构**:
```xml
<mp_crafting_pieces>
    <mp_crafting_piece name="blade_mp">
        <crafting_piece name="blade"/>
        <unlock_level value="10"/>
        <cost value="50"/>
    </mp_crafting_piece>
</mp_crafting_pieces>
```

**实现特点**:
- 多人游戏制作系统的特殊要求
- 支持解锁等级和成本系统
- 制作部件的引用关系

### 11. ParticleSystems (粒子系统)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/ParticleSystemsDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/ParticleSystemsDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/ParticleSystemsMapper.cs`

**XML结构**:
```xml
<particle_systems>
    <particle_system name="blood_splash">
        <emission_rate value="100"/>
        <lifetime value="2.0"/>
        <size value="0.1"/>
    </particle_system>
</particle_systems>
```

**实现特点**:
- 粒子系统的复杂参数
- 支持发射率、生命周期、大小等属性
- 视觉效果的精确控制

### 12. Looknfeel (外观感受)

**文件路径**:
- DO: `BannerlordModEditor.Common/Models/DO/LooknfeelDO.cs`
- DTO: `BannerlordModEditor.Common/Models/DTO/LooknfeelDTO.cs`
- Mapper: `BannerlordModEditor.Common/Mappers/LooknfeelMapper.cs`

**XML结构**:
```xml
<looknfeel>
    <widget_look name="button">
        <state name="normal">
            <colour property="Colour" r="1.0" g="1.0" b="1.0" a="1.0"/>
        </state>
        <state name="hover">
            <colour property="Colour" r="0.9" g="0.9" b="0.9" a="1.0"/>
        </state>
    </widget_look>
</looknfeel>
```

**实现特点**:
- UI外观的复杂状态管理
- 支持多种状态（normal、hover、disabled等）
- 颜色和属性的精确控制
- 混合XML架构的特殊处理

## 通用实现模式

### 1. 属性序列化控制
```csharp
// 使用ShouldSerialize*方法控制属性序列化
public bool ShouldSerializePropertyName() => !string.IsNullOrEmpty(PropertyName);
public bool ShouldSerializeListProperty() => ListProperty?.Count > 0;
```

### 2. 空元素处理
```csharp
// 使用标记属性控制空元素序列化
[XmlIgnore]
public bool HasEmptyElement { get; set; } = false;

public bool ShouldSerializeElement() => HasEmptyElement || (Element != null && Element.Count > 0);
```

### 3. 多态元素处理
```csharp
// 使用Type属性实现多态序列化
[XmlElement("Section", Type = typeof(SectionDO))]
[XmlElement("Entry", Type = typeof(EntryDO))]
[XmlElement("EmptyLine", Type = typeof(EmptyLineDO))]
public List<object> Elements { get; set; } = new();
```

### 4. Mapper模式
```csharp
// 标准的Mapper实现模式
public static TargetType ToDTO(SourceType source)
{
    if (source == null) return null;
    
    return new TargetType
    {
        Property1 = source.Property1,
        Property2 = source.Property2,
        ListProperty = source.ListProperty?.Select(ToDTO).ToList() ?? new List<TargetItemType>()
    };
}
```

### 5. 列表映射辅助方法
```csharp
private static List<TTarget> MapList<TSource, TTarget>(List<TSource> source, Func<TSource, TTarget> mapper)
{
    return source?.Select(mapper).ToList() ?? new List<TTarget>();
}
```

## 特殊处理技术

### 1. 命名空间处理
在`XmlTestUtils.Serialize`方法中实现了智能的命名空间处理：
```csharp
// 只有在原始XML有命名空间声明时才保留原始命名空间
// 不添加空命名空间，避免序列化器自动添加xsd和xsi属性
```

### 2. XML结构验证
使用`XmlTestUtils.AreStructurallyEqual`方法进行结构化比较：
```csharp
// 验证序列化前后的XML结构一致性
var result = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
Assert.True(result.IsStructurallyEqual);
```

### 3. 混合XML架构
对于像Looknfeel这样的复杂XML，实现了混合架构：
- 保留原始XML的节点顺序
- 支持动态属性和状态管理
- 处理嵌套的XML结构

## 测试策略

### 1. 单元测试
每个XML映射类都有对应的单元测试：
```csharp
[Fact]
public void XmlSerialization_ShouldPreserveData()
{
    var xml = File.ReadAllText("TestData/Original.xml");
    var obj = XmlTestUtils.Deserialize<T>(xml);
    var serializedXml = XmlTestUtils.Serialize(obj, xml);
    
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serializedXml));
}
```

### 2. 调试测试
为复杂XML创建专门的调试测试：
```csharp
[Fact]
public void Debug_XmlStructure()
{
    // 分析XML结构差异
    // 验证元素顺序
    // 检查属性处理
}
```

### 3. 性能测试
对大型XML文件进行性能测试，确保：
- 序列化/反序列化速度
- 内存使用效率
- 处理大型文件的能力

## 性能优化

### 1. 延迟加载
```csharp
// 使用XmlIgnore标记计算属性
[XmlIgnore]
public List<T> ComputedProperty => Elements.OfType<T>().ToList();
```

### 2. 集合初始化
```csharp
// 确保集合属性始终初始化
public List<T> ListProperty { get; set; } = new();
```

### 3. 空值检查
```csharp
// 在Mapper中进行彻底的空值检查
return source?.Select(mapper).ToList() ?? new List<T>();
```

## 扩展性考虑

### 1. 新XML类型添加
1. 在Models/DO中创建新的DO类
2. 在Models/DTO中创建对应的DTO类
3. 在Mappers中创建对应的Mapper类
4. 添加对应的单元测试

### 2. 现有类型扩展
1. 在DO类中添加新属性
2. 在DTO类中添加对应属性
3. 更新Mapper类中的映射逻辑
4. 更新测试用例

### 3. 自定义序列化逻辑
对于特殊需求，可以实现：
- 自定义的XML序列化器
- 特殊的属性处理逻辑
- 复杂的验证规则

## 总结

BannerlordModEditor项目的XML映射类实现采用了先进的DO/DTO架构模式，确保了：

1. **数据完整性**: 序列化/反序列化过程中数据不发生变化
2. **性能优化**: 高效的XML处理和内存使用
3. **可维护性**: 清晰的代码结构和分离的关注点
4. **扩展性**: 易于添加新的XML类型和扩展现有功能
5. **测试覆盖**: 全面的单元测试和调试工具

这种架构模式不仅满足了项目当前的需求，还为未来的扩展和维护提供了坚实的基础。通过精确的序列化控制和完善的测试策略，确保了XML数据处理的可靠性和一致性。