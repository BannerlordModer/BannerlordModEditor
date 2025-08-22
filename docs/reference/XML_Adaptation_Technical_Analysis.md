# Bannerlord Mod Editor - 技术分析和实施指南

## 1. 关键问题分析

### 1.1 物理材质XML结构分析

通过检查`physics_materials.xml`和对应的模型`PhysicsMaterials.cs`，我们发现了几个关键问题：

#### 问题1：布尔属性不匹配
**问题**：XML包含字符串形式的布尔值，但C#模型使用布尔类型
```xml
<!-- XML结构 -->
<physics_material id="pot" dont_stick_missiles="true" attacks_can_pass_through="false" />
<physics_material id="wood" rain_splashes_enabled="true" flammable="true" />
```

```csharp
// 当前模型问题
public class PhysicsMaterial
{
    [XmlAttribute("dont_stick_missiles")]
    [DefaultValue(false)]
    public bool DontStickMissiles { get; set; }  // 序列化为：dont_stick_missiles="true"
    
    [XmlAttribute("attacks_can_pass_through")]
    public string? AttacksCanPassThrough { get; set; }  // 正确：保留字符串格式
}
```

**根本原因**：当`bool`属性序列化时，输出是正确的，但比较逻辑可能因类型转换差异而失败。

#### 问题2：缺少ShouldSerialize方法
**问题**：可选字符串属性缺少适当的序列化控制
```csharp
// 当前模型
[XmlAttribute("override_material_name_for_impact_sounds")]
public string? OverrideMaterialNameForImpactSounds { get; set; }  // 没有ShouldSerialize方法

// 应该是：
[XmlAttribute("override_material_name_for_impact_sounds")]
public string? OverrideMaterialNameForImpactSounds { get; set; }

public bool ShouldSerializeOverrideMaterialNameForImpactSounds() 
    => !string.IsNullOrWhiteSpace(OverrideMaterialNameForImpactSounds);
```

#### 问题3：数值精度和格式化
**问题**：浮点值在往返过程中可能丢失精度或格式
```xml
<!-- 原始XML -->
<physics_material static_friction="0.800" dynamic_friction="0.400" />

<!-- 序列化后可能变成： -->
<physics_material static_friction="0.8" dynamic_friction="0.4" />
```

### 1.2 怪物XML结构分析

#### 问题1：带有可选元素的复杂嵌套结构
```xml
<!-- XML结构 -->
<Monster id="human" weight="80">
    <Capsules>
        <body_capsule radius="0.4" pos1="0, 1.2, 1.5" pos2="0, -0.37, 1.5" />
    </Capsules>
    <Flags Mountable="true" CanRear="true" />
</Monster>
```

```csharp
// 当前模型 - 缺少适当的序列化控制
public class Monster
{
    [XmlElement("Capsules")]
    public MonsterCapsules? Capsules { get; set; }  // 没有ShouldSerialize方法
    
    [XmlElement("Flags")]
    public MonsterFlags? Flags { get; set; }  // 没有ShouldSerialize方法
}
```

#### 问题2：嵌套元素中的布尔属性
```xml
<!-- 嵌套布尔属性 -->
<Flags Mountable="true" CanRear="true" RunsAwayWhenHit="true" />
```

```csharp
// 当前模型 - 不一致的布尔处理
public class MonsterFlags
{
    [XmlAttribute("Mountable")]
    public string? Mountable { get; set; }  // 正确：字符串保留
    
    [XmlAttribute("CanAttack")]
    public string? CanAttack { get; set; }  // 正确：字符串保留
}
```

## 2. 技术实施策略

### 2.1 第一阶段：修复布尔属性处理

#### 策略1：将所有布尔属性转换为字符串
```csharp
// 之前（有问题）
[XmlAttribute("dont_stick_missiles")]
[DefaultValue(false)]
public bool DontStickMissiles { get; set; }

// 之后（正确）
[XmlAttribute("dont_stick_missiles")]
public string? DontStickMissiles { get; set; }  // 保留精确的字符串格式

public bool ShouldSerializeDontStickMissiles() 
    => !string.IsNullOrWhiteSpace(DontStickMissiles);
```

#### 策略2：实现适当的ShouldSerialize方法
```csharp
public class PhysicsMaterial
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;  // 必需 - 不需要ShouldSerialize
    
    [XmlAttribute("override_material_name_for_impact_sounds")]
    public string? OverrideMaterialNameForImpactSounds { get; set; }
    
    [XmlAttribute("dont_stick_missiles")]
    public string? DontStickMissiles { get; set; }
    
    [XmlAttribute("attacks_can_pass_through")]
    public string? AttacksCanPassThrough { get; set; }
    
    [XmlAttribute("rain_splashes_enabled")]
    public string? RainSplashesEnabled { get; set; }
    
    [XmlAttribute("flammable")]
    public string? Flammable { get; set; }
    
    [XmlAttribute("display_color")]
    public string? DisplayColor { get; set; }
    
    // 可选属性的ShouldSerialize方法
    public bool ShouldSerializeOverrideMaterialNameForImpactSounds() 
        => !string.IsNullOrWhiteSpace(OverrideMaterialNameForImpactSounds);
    
    public bool ShouldSerializeDontStickMissiles() 
        => !string.IsNullOrWhiteSpace(DontStickMissiles);
    
    public bool ShouldSerializeAttacksCanPassThrough() 
        => !string.IsNullOrWhiteSpace(AttacksCanPassThrough);
    
    public bool ShouldSerializeRainSplashesEnabled() 
        => !string.IsNullOrWhiteSpace(RainSplashesEnabled);
    
    public bool ShouldSerializeFlammable() 
        => !string.IsNullOrWhiteSpace(Flammable);
    
    public bool ShouldSerializeDisplayColor() 
        => !string.IsNullOrWhiteSpace(DisplayColor);
}
```

### 2.2 第二阶段：修复复杂元素结构处理

#### 策略1：为复杂元素实现适当的ShouldSerialize
```csharp
public class Monster
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;  // 必需
    
    [XmlAttribute("weight")]
    public string? Weight { get; set; }
    
    [XmlElement("Capsules")]
    public MonsterCapsules? Capsules { get; set; }
    
    [XmlElement("Flags")]
    public MonsterFlags? Flags { get; set; }
    
    public bool ShouldSerializeWeight() 
        => !string.IsNullOrWhiteSpace(Weight);
    
    public bool ShouldSerializeCapsules() 
        => Capsules != null;
    
    public bool ShouldSerializeFlags() 
        => Flags != null;
}
```

#### 策略2：正确处理嵌套布尔属性
```csharp
public class MonsterFlags
{
    [XmlAttribute("CanAttack")]
    public string? CanAttack { get; set; }
    
    [XmlAttribute("CanDefend")]
    public string? CanDefend { get; set; }
    
    [XmlAttribute("CanKick")]
    public string? CanKick { get; set; }
    
    [XmlAttribute("CanBeCharged")]
    public string? CanBeCharged { get; set; }
    
    [XmlAttribute("CanCharge")]
    public string? CanCharge { get; set; }
    
    [XmlAttribute("CanClimbLadders")]
    public string? CanClimbLadders { get; set; }
    
    [XmlAttribute("CanSprint")]
    public string? CanSprint { get; set; }
    
    [XmlAttribute("CanCrouch")]
    public string? CanCrouch { get; set; }
    
    [XmlAttribute("CanRetreat")]
    public string? CanRetreat { get; set; }
    
    [XmlAttribute("CanRear")]
    public string? CanRear { get; set; }
    
    [XmlAttribute("CanWander")]
    public string? CanWander { get; set; }
    
    [XmlAttribute("CanBeInGroup")]
    public string? CanBeInGroup { get; set; }
    
    [XmlAttribute("MoveAsHerd")]
    public string? MoveAsHerd { get; set; }
    
    [XmlAttribute("MoveForwardOnly")]
    public string? MoveForwardOnly { get; set; }
    
    [XmlAttribute("IsHumanoid")]
    public string? IsHumanoid { get; set; }
    
    [XmlAttribute("Mountable")]
    public string? Mountable { get; set; }
    
    [XmlAttribute("CanRide")]
    public string? CanRide { get; set; }
    
    [XmlAttribute("CanWieldWeapon")]
    public string? CanWieldWeapon { get; set; }
    
    [XmlAttribute("RunsAwayWhenHit")]
    public string? RunsAwayWhenHit { get; set; }
    
    [XmlAttribute("CanGetScared")]
    public string? CanGetScared { get; set; }
    
    // 所有可选属性的ShouldSerialize方法
    public bool ShouldSerializeCanAttack() 
        => !string.IsNullOrWhiteSpace(CanAttack);
    
    public bool ShouldSerializeCanDefend() 
        => !string.IsNullOrWhiteSpace(CanDefend);
    
    // ... 所有其他属性的类似方法
}
```

### 2.3 第三阶段：数值精度和格式化

#### 策略1：对数值使用字符串属性
```csharp
// 之前（有问题 - 可能丢失精度/格式）
[XmlAttribute("static_friction")]
public float StaticFriction { get; set; }

// 之后（正确 - 保留精确格式）
[XmlAttribute("static_friction")]
public string? StaticFriction { get; set; }

public bool ShouldSerializeStaticFriction() 
    => !string.IsNullOrWhiteSpace(StaticFriction);
```

#### 策略2：为数值字符串实现验证
```csharp
public class PhysicsMaterial
{
    [XmlAttribute("static_friction")]
    public string? StaticFriction { get; set; }
    
    public bool ShouldSerializeStaticFriction() 
        => !string.IsNullOrWhiteSpace(StaticFriction);
    
    public float? GetStaticFrictionValue()
    {
        if (float.TryParse(StaticFriction, out var value))
            return value;
        return null;
    }
    
    public void SetStaticFrictionValue(float? value)
    {
        StaticFriction = value?.ToString("0.000");  // 保留格式
    }
}
```

## 3. 实施检查清单

### 3.1 PhysicsMaterials.cs需要的修复

#### 需要转换为字符串的布尔属性
- [ ] `DontStickMissiles` - 将`bool`转换为`string?`
- [ ] `RainSplashesEnabled` - 将`bool`转换为`string?`
- [ ] `Flammable` - 将`bool`转换为`string?`

#### 需要添加的ShouldSerialize方法
- [ ] `ShouldSerializeOverrideMaterialNameForImpactSounds()`
- [ ] `ShouldSerializeDontStickMissiles()`
- [ ] `ShouldSerializeAttacksCanPassThrough()`
- [ ] `ShouldSerializeRainSplashesEnabled()`
- [ ] `ShouldSerializeFlammable()`
- [ ] `ShouldSerializeDisplayColor()`

#### 需要转换为字符串的数值属性
- [ ] `StaticFriction` - 将`float`转换为`string?`
- [ ] `DynamicFriction` - 将`float`转换为`string?`
- [ ] `Restitution` - 将`float`转换为`string?`
- [ ] `Softness` - 将`float`转换为`string?`
- [ ] `LinearDamping` - 将`float`转换为`string?`
- [ ] `AngularDamping` - 将`float`转换为`string?`

### 3.2 Monsters.cs需要的修复

#### 需要添加的ShouldSerialize方法
- [ ] `ShouldSerializeWeight()` 用于Weight属性
- [ ] `ShouldSerializeCapsules()` 用于Capsules元素
- [ ] `ShouldSerializeFlags()` 用于Flags元素
- [ ] 所有MonsterFlags的ShouldSerialize方法（20+个方法）

#### 字符串属性转换
- [ ] 检查所有数值属性是否需要字符串转换
- [ ] 检查所有布尔属性是否需要一致的字符串处理

### 3.3 需要的通用模型更新

#### 需要审查的模型（基于46个失败）
- [ ] `PhysicsMaterials.cs` - 布尔和数值属性处理
- [ ] `Monsters.cs` - 复杂嵌套结构和布尔处理
- [ ] `AttributesDataModel.cs` - 可选属性序列化
- [ ] `ActionTypesModel.cs` - 可选属性序列化
- [ ] `ProjectConfiguration.cs` - 可选元素处理
- [ ] 所有其他有测试失败的模型

## 4. 测试和验证策略

### 4.1 需要的单元测试更新

#### PhysicsMaterials测试更新
```csharp
[Fact]
public void PhysicsMaterials_RoundTrip_StructuralEquality()
{
    var xml = File.ReadAllText(TestDataPath);
    var model = XmlTestUtils.Deserialize<PhysicsMaterials>(xml);
    var serialized = XmlTestUtils.Serialize(model);
    
    // 测试结构相等性
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
    
    // 测试特定属性保留
    var originalMaterial = model.PhysicsMaterialList.FirstOrDefault(m => m.Id == "pot");
    Assert.NotNull(originalMaterial);
    Assert.Equal("true", originalMaterial.DontStickMissiles);
    Assert.Equal("false", originalMaterial.AttacksCanPassThrough);
}
```

#### Monsters测试更新
```csharp
[Fact]
public void Monsters_RoundTrip_StructuralEquality()
{
    var xml = File.ReadAllText(TestDataPath);
    var model = XmlTestUtils.Deserialize<Monsters>(xml);
    var serialized = XmlTestUtils.Serialize(model);
    
    // 测试结构相等性
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
    
    // 测试复杂嵌套结构保留
    var humanMonster = model.MonsterList.FirstOrDefault(m => m.Id == "human");
    Assert.NotNull(humanMonster);
    Assert.NotNull(humanMonster.Capsules);
    Assert.NotNull(humanMonster.Flags);
    
    // 测试嵌套元素中的布尔属性保留
    Assert.Equal("true", humanMonster.Flags.CanAttack);
    Assert.Equal("true", humanMonster.Flags.CanDefend);
}
```

### 4.2 集成测试策略

#### 测试所有失败的模型
- [ ] 运行所有46个失败的测试以确认修复
- [ ] 验证往返序列化保留精确结构
- [ ] 验证所有可选属性都被正确处理
- [ ] 验证数值精度和格式被保留

#### 性能测试
- [ ] 测试大型XML文件的性能影响
- [ ] 验证内存使用是否可接受
- [ ] 确保序列化速度满足要求

## 5. 推出计划

### 第一阶段：核心模型修复（高优先级）
1. 修复PhysicsMaterials.cs（最高失败率）
2. 修复Monsters.cs（复杂嵌套结构）
3. 修复AttributesDataModel.cs（简单结构，高可见性）

### 第二阶段：额外模型修复（中等优先级）
1. 修复具有类似模式的剩余模型
2. 更新单元测试以反映新的序列化行为
3. 为边缘情况添加集成测试

### 第三阶段：验证和优化（低优先级）
1. 性能测试和优化
2. 文档更新
3. 代码审查和重构

这份技术分析提供了详细的实施策略，用于解决剩余的46个XML适配失败，同时保持与Bannerlord XML格式要求的严格合规性。