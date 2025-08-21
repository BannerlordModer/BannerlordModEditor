# BannerlordModEditor DO/DTO 模式转换和测试修复工作 Prompt

## 工作概述

这是一个详细的prompt，用于指导Claude AI继续执行BannerlordModEditor项目中失败的XML序列化测试的修复工作。当前工作已经完成部分DO/DTO模式转换，还有19个失败的测试需要处理。

## 当前状态

### 已完成的工作
1. ✅ **CombatParameters** - 完全修复，3个测试全部通过
2. ✅ **ActionTypes** - 完全修复，测试通过
3. ✅ **BoneBodyTypes** - 完全修复，测试通过
4. ✅ **ActionSets** - 完全修复，测试通过
5. ✅ **CollisionInfos** - 完全修复，测试通过
6. ✅ **MapIcons** - 完全修复，测试通过
7. ✅ **ItemHolsters** - DO/DTO模型已创建，但测试仍失败
8. ✅ **MpCraftingPieces** - DO/DTO模型已创建，但测试仍失败
9. ✅ **DO/DTO经验文档** - 已添加到CLAUDE.md文件

### 当前测试状态
- **失败**: 19个测试
- **通过**: 1062个测试  
- **已跳过**: 2个测试
- **总计**: 1083个测试

### 待处理的失败测试列表
1. __tests__.XmlTestUtils_CompareXmlStructure_Tests.NodeAndAttributeDifferences_ShouldBeReported
2. CreditsXmlTests.CreditsXml_RoundTrip_StructuralEquality
3. ItemHolstersXmlTests.ItemHolsters_RoundTrip_StructuralEquality (已处理但仍失败)
4. LooknfeelXmlTests.Looknfeel_Roundtrip_StructuralEquality
5. CreditsLegalPCXmlTests.CreditsLegalPC_RoundTrip_StructuralEquality
6. DebugMultiplayerScenes.Debug_MultiplayerScenes_Serialization
7. DataTests.BannerIcons_SecondGroupHasCorrectStructure
8. DataTests.BannerIcons_ColorsHaveValidHexValues
9. FloraKindsXmlTests.FloraKinds_RoundTrip_StructuralEquality
10. ParticleSystemsHardcodedMisc2XmlTests.ParticleSystemsHardcodedMisc2_RoundTrip_StructuralEquality
11. MpcosmeticsXmlTests.Mpcosmetics_RoundTrip_StructuralEquality
12. ParticleSystemsHardcodedMisc1XmlTests.ParticleSystemsHardcodedMisc1_RoundTrip_StructuralEquality
13. TempDebugTest.Temp_Debug_Attributes_Format_Comparison
14. CreditsExternalPartnersPlayStationXmlTests.CreditsExternalPartnersPlayStation_RoundTrip_StructuralEquality
15. ParticleSystemsOutdoorXmlTests.ParticleSystemsOutdoor_Roundtrip_StructuralEquality
16. MpCraftingPiecesXmlTests.MpCraftingPieces_RoundTrip_StructuralEquality (已处理但仍失败)
17. BeforeTransparentsGraphXmlTests.BeforeTransparentsGraph_RoundTrip_StructuralEquality

## 核心问题分析

### 已识别的问题模式
1. **XML序列化结构不匹配** - 原始XML与序列化后的XML在结构上存在差异
2. **空元素处理不当** - 空的XML元素在序列化过程中被错误地省略
3. **属性顺序变化** - XML属性在序列化后顺序发生改变
4. **命名空间丢失** - 序列化后XML命名空间信息丢失

### 成功的修复模式
**CombatParameters修复案例**是成功的模板：
- 添加了`HasDefinitions`和`HasEmptyCombatParameters`标记属性
- 在`XmlTestUtils.Deserialize`中添加特殊处理逻辑
- 修改了`ShouldSerialize`方法以精确控制序列化行为

## 工作方法论

### DO/DTO架构模式实施步骤

#### 1. 准备阶段
- **检查失败测试**: 使用`dotnet test --filter "TestName"`检查具体失败原因
- **分析现有模型**: 检查是否已有DO/DTO模型存在
- **理解XML结构**: 分析原始XML文件的结构特点

#### 2. 实施阶段
- **创建DO模型**: 
  ```csharp
  [XmlRoot("base")]
  public class TestDO
  {
      [XmlElement("element")]
      public TestElementDO Element { get; set; }
      
      [XmlIgnore]
      public bool HasEmptyElement { get; set; } = false;
      
      public bool ShouldSerializeElement() => HasEmptyElement || Element != null;
  }
  ```

- **创建DTO模型**: 与DO结构相同，专门用于序列化

- **创建Mapper**:
  ```csharp
  public static class TestMapper
  {
      public static TestDTO ToDTO(TestDO source)
      {
          if (source == null) return null;
          return new TestDTO { /* 属性映射 */ };
      }
      
      public static TestDO ToDO(TestDTO source)
      {
          if (source == null) return null;
          return new TestDO { /* 反向映射 */ };
      }
  }
  ```

- **更新测试文件**: 将`using`语句和类型引用从Data层改为DO层

- **添加特殊处理逻辑**（如果需要）:
  ```csharp
  // 在XmlTestUtils.Deserialize<T>()中添加
  if (obj is TestDO test)
  {
      var doc = XDocument.Parse(xml);
      var testElement = doc.Root?.Element("test_element");
      test.HasEmptyTestElement = testElement != null && 
          (testElement.Elements().Count() == 0 || testElement.Elements("item").Count() == 0);
  }
  ```

#### 3. 验证阶段
- **运行测试**: `dotnet test --filter "TestName"`
- **检查结果**: 确认测试是否通过
- **提交代码**: 使用git提交并推送到GitHub

### 特殊处理逻辑判断标准

需要添加特殊处理逻辑的情况：
1. **空元素问题** - 原始XML包含空元素但序列化后被省略
2. **复杂嵌套结构** - 包含多层嵌套的XML结构
3. **条件序列化** - 某些元素需要根据条件决定是否序列化
4. **命名空间敏感** - XML包含重要的命名空间信息

## 剩余工作优先级

### 高优先级（需要立即处理）
1. **ItemHolsters** - 已有DO/DTO但测试失败，需要调试
2. **MpCraftingPieces** - 已有DO/DTO但测试失败，需要调试
3. **LooknfeelXmlTests** - 相对简单，可以作为下一个目标
4. **CreditsXmlTests** - 可能是简单的XML结构问题

### 中优先级
1. **FloraKindsXmlTests** - 可能需要复杂的XML处理
2. **ParticleSystems相关测试** - 多个粒子系统测试失败，可能需要批量处理

### 低优先级
1. **DataTests中的测试** - 可能是数据验证问题而非XML序列化问题
2. **DebugMultiplayerScenes** - 可能是特殊的调试逻辑

## 具体执行指导

### 第一步：解决ItemHolsters和MpCraftingPieces的失败问题

这两个测试已经有DO/DTO模型但仍然失败，需要深入调试：

**调试方法**：
1. 运行测试获取详细错误信息
2. 分析XML结构差异
3. 检查是否需要额外的特殊处理逻辑
4. 验证DO/DTO模型是否正确映射所有属性

**可能的问题**：
- 遗漏了某些属性的映射
- 需要更复杂的空元素处理逻辑
- XML属性的处理不当

### 第二步：处理LooknfeelXmlTests

**执行步骤**：
1. 检查Looknfeel的现有Data模型结构
2. 创建对应的DO/DTO模型
3. 更新测试文件
4. 运行测试验证修复

### 第三步：处理CreditsXmlTests

**执行步骤**：
1. 检查Credits的现有Data模型
2. 判断是否需要DO/DTO转换
3. 如果需要，创建DO/DTO模型并更新测试
4. 如果不需要，分析其他失败原因

### 第四步：批量处理ParticleSystems测试

**执行步骤**：
1. 分析所有ParticleSystems相关测试的共性
2. 创建通用的ParticleSystems DO/DTO模型
3. 批量更新相关测试文件
4. 验证所有修复

## 技术注意事项

### XML序列化最佳实践
1. **ShouldSerialize方法** - 精确控制哪些属性应该被序列化
2. **XmlIgnore属性** - 将运行时标记属性排除在序列化之外
3. **空元素处理** - 使用标记属性来保留原始XML结构中的空元素
4. **命名空间保留** - 在XmlTestUtils.Serialize中传递原始XML以保留命名空间

### 数据验证
1. **null检查** - 在映射器方法中进行彻底的null检查
2. **空集合处理** - 确保空集合被正确初始化
3. **类型安全** - 保持DO和DTO之间的类型一致性

### 向后兼容性
1. **保留原始模型** - 不删除原有的Data层模型
2. **渐进式迁移** - 可以逐步将业务逻辑迁移到DO层
3. **测试更新** - 只需更新测试文件中的using语句和类型引用

## 质量保证

### 测试验证
- 每次修复后必须运行对应的测试
- 确保没有引入回归问题
- 验证修复后的代码质量

### 代码提交
- 每次修复一个适配类和测试都要进行一次git提交
- 提交信息要清晰描述修复内容
- 推送到GitHub以保存进度

### 文档更新
- 更新CLAUDE.md文档记录新的经验
- 记录遇到的问题和解决方案
- 为后续类似工作提供参考

## 成功标准

### 最终目标
- 所有1083个测试都必须通过
- 失败数量必须为0
- 保持代码质量和架构一致性

### 阶段性目标
- 每次修复一个或一组相关测试
- 减少失败测试的数量
- 积累修复经验并应用到后续测试

## 风险管理

### 潜在风险
1. **复杂性增加** - DO/DTO模式可能增加代码复杂性
2. **性能影响** - 多层转换可能影响性能
3. **维护负担** - 需要维护两套模型（Data和DO/DTO）

### 缓解措施
1. **保持简洁** - 只在必要时添加DO/DTO层
2. **性能测试** - 关注性能变化，必要时优化
3. **文档完善** - 提供完整的文档和使用指南

## 工具和命令

### 常用命令
```bash
# 构建项目
dotnet build

# 运行所有测试
dotnet test

# 运行特定测试
dotnet test --filter "TestName"

# 运行测试并保存结果
dotnet test --logger "console;verbosity=minimal" > test_results.txt 2>&1

# 检查测试结果
tail -15 test_results.txt

# Git操作
git add .
git commit -m "描述修复内容"
git push origin branch-name
```

### 调试工具
- **XmlTestUtils.AreStructurallyEqual** - 比较XML结构
- **XDocument.Parse** - 解析XML进行分析
- **XmlTestUtils.Deserialize/Serialize** - 测试序列化功能

## 时间管理建议

### 工作节奏
1. **小批量处理** - 每次处理1-3个相关测试
2. **及时验证** - 每次修改后立即验证结果
3. **定期提交** - 每完成一个修复就提交代码
4. **文档记录** - 及时记录问题和解决方案

### 优先级调整
- 根据修复难度灵活调整优先级
- 先处理容易修复的测试，积累经验
- 对于复杂问题，可以暂时跳过，先处理其他测试

## 总结

这个prompt提供了完整的指导来继续BannerlordModEditor项目中失败的XML序列化测试的修复工作。核心是按照DO/DTO架构模式的成功经验，系统性地处理每个失败的测试，直到所有1083个测试都通过为止。

关键成功因素：
1. 严格遵循已验证的DO/DTO模式
2. 系统性地处理每个失败的测试
3. 及时提交和保存进度
4. 持续学习和应用修复经验

最终目标是实现0个失败测试，同时保持代码质量和架构一致性。