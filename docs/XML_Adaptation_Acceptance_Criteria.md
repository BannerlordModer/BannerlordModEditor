# BannerlordModEditor XML适配验收标准

## 1. 总体验收标准

### 1.1 功能完整性验收
**验收标准**: 所有三个XML类型的适配必须完全实现并通过测试

**具体要求**:
- [ ] BannerIcons DO/DTO架构完全实现
- [ ] ItemModifiers属性数量问题完全修复
- [ ] ParticleSystems编译警告完全消除
- [ ] 所有相关单元测试通过（100%通过率）
- [ ] XML序列化往返测试通过
- [ ] 结构化比较测试通过

**验收方法**:
```bash
# 运行所有相关测试
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIcons|ItemModifiers|ParticleSystems" --verbosity normal

# 检查测试结果
# 期望输出：所有测试通过，没有失败
```

### 1.2 代码质量验收
**验收标准**: 代码必须符合项目的质量标准

**具体要求**:
- [ ] 代码编译无警告
- [ ] 代码覆盖率 > 80%
- [ ] 所有公共API都有XML文档注释
- [ ] 遵循项目编码规范
- [ ] 无性能问题
- [ ] 无内存泄漏

**验收方法**:
```bash
# 编译检查
dotnet build --no-restore

# 代码覆盖率检查
dotnet test BannerlordModEditor.Common.Tests --collect:"XPlat Code Coverage"

# 静态代码分析
dotnet format --verify-no-changes
```

### 1.3 性能验收
**验收标准**: XML处理性能必须满足要求

**具体要求**:
- [ ] 大型XML文件（>10MB）处理时间 < 5秒
- [ ] 内存使用峰值 < 100MB
- [ ] 支持异步处理
- [ ] 无内存泄漏
- [ ] CPU使用率合理

**验收方法**:
```bash
# 性能测试（需要创建性能测试用例）
dotnet test --filter "PerformanceTest"
```

## 2. BannerIcons专项验收标准

### 2.1 DO/DTO架构验收
**验收标准**: BannerIcons必须实现完整的DO/DTO架构

**文件检查**:
- [ ] `/Models/DO/BannerIconsDO.cs` 存在且符合规范
- [ ] `/Models/DTO/BannerIconsDTO.cs` 存在且符合规范
- [ ] `/Mappers/BannerIconsMapper.cs` 存在且符合规范

**代码结构检查**:
```csharp
// BannerIconsDO.cs 必须包含
[XmlRoot("base")]
public class BannerIconsDO
{
    [XmlAttribute("type")]
    public string Type { get; set; }
    
    [XmlElement("BannerIconData")]
    public BannerIconDataDO BannerIconData { get; set; }
    
    // 必须包含ShouldSerialize方法
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeBannerIconData() => BannerIconData != null;
}

// 必须包含所有嵌套对象
public class BannerIconDataDO { ... }
public class BannerIconGroupDO { ... }
public class BackgroundDO { ... }
public class IconDO { ... }
public class BannerColorsDO { ... }
public class ColorEntryDO { ... }
```

### 2.2 XML结构一致性验收
**验收标准**: 序列化前后XML结构必须完全一致

**测试数据**: `TestData/banner_icons.xml`

**验收测试**:
```csharp
[Fact]
public void BannerIcons_RoundTrip_StructuralEquality()
{
    var xml = File.ReadAllText("TestData/banner_icons.xml");
    var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
    var serialized = XmlTestUtils.Serialize(model, xml);
    
    // 节点数量必须完全一致
    var (origNodes, origAttrs) = XmlTestUtils.CountNodesAndAttributes(xml);
    var (serNodes, serAttrs) = XmlTestUtils.CountNodesAndAttributes(serialized);
    
    Assert.Equal(origNodes, serNodes);
    Assert.Equal(origAttrs, serAttrs);
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
}
```

### 2.3 特殊元素处理验收
**验收标准**: 必须正确处理空元素和可选元素

**XmlTestUtils增强检查**:
```csharp
// 必须在XmlTestUtils.Deserialize中添加
if (obj is BannerIconsDO bannerIcons)
{
    var doc = XDocument.Parse(xml);
    var bannerIconDataElement = doc.Root?.Element("BannerIconData");
    bannerIcons.HasEmptyBannerIconData = bannerIconDataElement != null && 
        !bannerIconDataElement.Elements().Any();
    
    // 检查BannerColors是否存在
    bannerIcons.HasBannerColors = bannerIconDataElement?.Element("BannerColors") != null;
}
```

## 3. ItemModifiers专项验收标准

### 3.1 属性数量一致性验收
**验收标准**: 序列化前后属性数量必须完全一致（840个）

**验收测试**:
```csharp
[Fact]
public void ItemModifiers_AttributeCount_ExactMatch()
{
    var xml = File.ReadAllText("TestData/item_modifiers.xml");
    var model = XmlTestUtils.Deserialize<ItemModifiersDO>(xml);
    var serialized = XmlTestUtils.Serialize(model, xml);
    
    var (origNodes, origAttrs) = XmlTestUtils.CountNodesAndAttributes(xml);
    var (serNodes, serAttrs) = XmlTestUtils.CountNodesAndAttributes(serialized);
    
    // 必须完全匹配
    Assert.Equal(840, origAttrs);
    Assert.Equal(840, serAttrs);
    Assert.Equal(origNodes, serNodes);
    Assert.Equal(origAttrs, serAttrs);
}
```

### 3.2 ShouldSerialize方法验收
**验收标准**: 所有ShouldSerialize方法必须正确工作

**检查清单**:
- [ ] ShouldSerializeModifierGroup() - 正确处理空字符串
- [ ] ShouldSerializeId() - 正确处理空字符串
- [ ] ShouldSerializeName() - 正确处理空字符串
- [ ] ShouldSerializeLootDropScoreString() - 正确处理null值
- [ ] ShouldSerializeProductionDropScoreString() - 正确处理null值
- [ ] ShouldSerializeDamageString() - 正确处理null值
- [ ] ShouldSerializeSpeedString() - 正确处理null值
- [ ] ShouldSerializeMissileSpeedString() - 正确处理null值
- [ ] ShouldSerializePriceFactorString() - 正确处理null值
- [ ] ShouldSerializeQuality() - 正确处理空字符串
- [ ] ShouldSerializeHitPointsString() - 正确处理null值
- [ ] ShouldSerializeHorseSpeedString() - 正确处理null值
- [ ] ShouldSerializeStackCountString() - 正确处理null值
- [ ] ShouldSerializeArmorString() - 正确处理null值
- [ ] ShouldSerializeManeuverString() - 正确处理null值
- [ ] ShouldSerializeChargeDamageString() - 正确处理null值
- [ ] ShouldSerializeHorseHitPointsString() - 正确处理null值

### 3.3 数据类型转换验收
**验收标准**: String和Nullable类型之间的双向转换必须正确

**验收测试**:
```csharp
[Fact]
public void ItemModifiers_PropertyTypeConversion_Correct()
{
    var xml = File.ReadAllText("TestData/item_modifiers.xml");
    var model = XmlTestUtils.Deserialize<ItemModifiersDO>(xml);
    
    // 检查第一个ItemModifier的属性转换
    var firstModifier = model.ItemModifierList.First();
    
    // 验证String ↔ Nullable转换
    if (!string.IsNullOrEmpty(firstModifier.DamageString))
    {
        Assert.Equal(firstModifier.Damage.Value, int.Parse(firstModifier.DamageString));
    }
    
    if (!string.IsNullOrEmpty(firstModifier.PriceFactorString))
    {
        Assert.Equal(firstModifier.PriceFactor.Value, float.Parse(firstModifier.PriceFactorString));
    }
    
    // 验证序列化后转换正确
    var serialized = XmlTestUtils.Serialize(model, xml);
    var model2 = XmlTestUtils.Deserialize<ItemModifiersDO>(serialized);
    
    Assert.Equal(model.ItemModifierList.Count, model2.ItemModifierList.Count);
}
```

## 4. ParticleSystems专项验收标准

### 4.1 编译警告消除验收
**验收标准**: 代码编译必须无警告

**编译检查**:
```bash
dotnet build --no-restore --verbosity quiet
# 期望输出：编译成功，无警告
```

### 4.2 Null引用处理验收
**验收标准**: 必须正确处理所有可能为null的引用

**检查清单**:
- [ ] 所有嵌套对象属性都有null检查
- [ ] 集合属性正确初始化为空集合
- [ ] 使用null条件运算符和null合并运算符
- [ ] 没有潜在的NullReferenceException

**代码示例**:
```csharp
// 正确的null处理示例
public class EffectDO
{
    [XmlElement("emitters")]
    public EmittersDO? Emitters { get; set; }
    
    public bool ShouldSerializeEmitters() => Emitters != null;
}

public class EmittersDO
{
    [XmlElement("emitter")]
    public List<EmitterDO> EmitterList { get; set; } = new List<EmitterDO>();
    
    public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
}
```

### 4.3 XML结构处理验收
**验收标准**: 必须正确处理所有ParticleSystems XML文件

**测试文件检查**:
- [ ] `TestData/particle_systems_basic.xml` - 基本粒子系统
- [ ] `TestData/particle_systems_outdoor.xml` - 户外粒子系统
- [ ] `TestData/particle_systems_general.xml` - 通用粒子系统
- [ ] `TestData/particle_systems_hardcoded_misc1.xml` - 硬编码杂项1
- [ ] `TestData/particle_systems_hardcoded_misc2.xml` - 硬编码杂项2

## 5. 通用验收标准

### 5.1 XmlTestUtils增强验收
**验收标准**: XmlTestUtils必须支持所有新的DO类型

**增强检查**:
```csharp
// 必须在XmlTestUtils.Deserialize中添加以下处理
if (obj is BannerIconsDO bannerIcons)
{
    // BannerIcons特殊处理
}

if (obj is ItemModifiersDO itemModifiers)
{
    // ItemModifiers特殊处理（如果需要）
}

if (obj is ParticleSystemsDO particleSystems)
{
    // ParticleSystems特殊处理（如果需要）
}
```

### 5.2 向后兼容性验收
**验收标准**: 必须保持与现有代码的兼容性

**兼容性检查**:
- [ ] 不删除现有的Data层模型
- [ ] 现有测试可以继续使用Data层模型
- [ ] 新的DO/DTO架构与现有代码兼容
- [ ] API接口保持不变

### 5.3 错误处理验收
**验收标准**: 必须有健壮的错误处理机制

**错误处理检查**:
- [ ] 提供有意义的错误信息
- [ ] 支持部分失败恢复
- [ ] 记录详细的调试信息
- [ ] 优雅处理异常情况

## 6. 性能验收标准

### 6.1 大型文件处理验收
**验收标准**: 必须能够高效处理大型XML文件

**性能测试**:
```csharp
[Fact]
public void LargeXML_Processing_Performance()
{
    var xml = File.ReadAllText("TestData/large_item_modifiers.xml");
    var stopwatch = Stopwatch.StartNew();
    
    var model = XmlTestUtils.Deserialize<ItemModifiersDO>(xml);
    var serialized = XmlTestUtils.Serialize(model, xml);
    
    stopwatch.Stop();
    
    Assert.True(stopwatch.ElapsedMilliseconds < 5000); // 5秒内完成
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
}
```

### 6.2 内存使用验收
**验收标准**: 内存使用必须在合理范围内

**内存监控**:
- [ ] 处理大型XML文件时内存使用 < 100MB
- [ ] 没有内存泄漏
- [ ] 对象正确释放
- [ ] 垃圾回收正常工作

## 7. 文档验收标准

### 7.1 代码文档验收
**验收标准**: 所有公共API必须有完整的XML文档

**文档检查**:
- [ ] 所有公共类有`<summary>`注释
- [ ] 所有公共属性有`<summary>`注释
- [ ] 所有公共方法有`<summary>`、`<param>`、`<returns>`注释
- [ ] 复杂逻辑有内联注释

### 7.2 项目文档更新验收
**验收标准**: 项目文档必须更新以反映新的架构

**文档更新**:
- [ ] `CLAUDE.md` 更新DO/DTO架构说明
- [ ] 添加新的XML类型适配指南
- [ ] 更新故障排除指南
- [ ] 提供实现示例

## 8. 验收流程

### 8.1 自我验收
开发者必须完成以下步骤：
1. 运行所有测试并确保100%通过
2. 检查代码编译无警告
3. 验证性能要求
4. 更新所有文档
5. 进行代码审查

### 8.2 代码审查
审查者必须检查：
1. 代码质量和规范性
2. 架构设计的合理性
3. 测试覆盖率
4. 文档完整性
5. 性能考虑

### 8.3 集成测试
在集成环境中验证：
1. 与现有系统的兼容性
2. 端到端功能测试
3. 性能测试
4. 回归测试

### 8.4 验收签字
验收完成后，相关方必须签字确认：
- [ ] 开发者签字
- [ ] 代码审查者签字
- [ ] 测试工程师签字
- [ ] 项目经理签字

## 9. 验收标准检查清单

### 9.1 最终检查清单
- [ ] 所有功能需求实现完成
- [ ] 所有测试通过（100%通过率）
- [ ] 代码编译无警告
- [ ] 代码覆盖率 > 80%
- [ ] 性能测试通过
- [ ] 内存使用正常
- [ ] 文档完整更新
- [ ] 向后兼容性验证通过
- [ ] 代码审查通过
- [ ] 集成测试通过

### 9.2 交付物清单
- [ ] BannerIconsDO.cs
- [ ] BannerIconsDTO.cs
- [ ] BannerIconsMapper.cs
- [ ] 修复后的ItemModifiersDO.cs
- [ ] 修复后的ParticleSystemsDO.cs
- [ ] 更新的测试文件
- [ ] 更新的文档
- [ ] 验证报告
- [ ] 性能测试报告

## 10. 验收标准附录

### 10.1 测试数据文件
- `TestData/banner_icons.xml`
- `TestData/item_modifiers.xml`
- `TestData/particle_systems_basic.xml`
- `TestData/particle_systems_outdoor.xml`
- `TestData/particle_systems_general.xml`
- `TestData/particle_systems_hardcoded_misc1.xml`
- `TestData/particle_systems_hardcoded_misc2.xml`

### 10.2 参考实现
- `CombatParametersDO.cs`
- `CombatParametersDTO.cs`
- `CombatParametersMapper.cs`
- `ActionTypesDO.cs`
- `ActionTypesDTO.cs`
- `ActionTypesMapper.cs`

### 10.3 工具和脚本
- `XmlTestUtils.cs`
- 性能测试脚本
- 代码覆盖率工具
- 静态代码分析工具

---

**注意**: 本验收标准必须严格执行，任何未达到标准的项目都必须重新修复直至符合要求。验收标准的任何修改都必须经过项目相关方的同意。