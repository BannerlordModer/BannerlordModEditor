# XML映射适配修复验收标准

## 概述

本文档定义了BannerlordModEditor-CLI项目中XML映射适配修复的具体验收标准和质量指标，确保项目达到95%的质量标准。

## 1. 功能性验收标准

### 1.1 XML往返测试修复

#### 1.1.1 SiegeEngines往返测试

**验收标准:**
- [ ] **测试通过**: `SiegeEngines_XmlSerialization_ShouldBeRoundTripValid` 测试必须通过
- [ ] **根元素一致性**: 序列化后的根元素必须保持为`<SiegeEngineTypes>`
- [ ] **属性完整性**: 所有攻城器械属性必须正确序列化和反序列化
- [ ] **结构一致性**: 往返测试后的XML结构必须与原始XML完全一致

**测试数据验证:**
```xml
<!-- 验证以下关键数据 -->
<SiegeEngineType id="ladder" name="{=G0AWk1rX}Ladder" is_constructible="false"/>
<SiegeEngineType id="ballista" is_ranged="true" damage="160" is_anti_personnel="true"/>
```

**验证方法:**
```csharp
// 执行往返测试
var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
var dto = SiegeEnginesMapper.ToDTO(originalDo);
var roundtripDo = SiegeEnginesMapper.ToDO(dto);
var serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);

// 验证结果
Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
```

#### 1.1.2 SpecialMeshes往返测试

**验收标准:**
- [ ] **测试通过**: `SpecialMeshes_XmlSerialization_ShouldBeRoundTripValid` 测试必须通过
- [ ] **嵌套结构完整性**: mesh->types->type的嵌套结构必须正确保持
- [ ] **空元素处理**: 空的types元素必须正确处理
- [ ] **属性顺序**: 属性顺序必须保持一致

**测试数据验证:**
```xml
<!-- 验证嵌套结构 -->
<mesh name="outer_mesh_forest">
  <types>
    <type name="outer_mesh" />
  </types>
</mesh>
```

#### 1.1.3 LanguageBase往返测试

**验收标准:**
- [ ] **测试通过**: 所有LanguageBase往返测试必须通过
- [ ] **函数体转义**: 特殊字符转义必须正确处理
- [ ] **混合内容**: tags、functions、strings混合内容必须正确处理
- [ ] **布尔值标准化**: 布尔值属性必须正确标准化

**测试数据验证:**
```xml
<!-- 验证函数体转义 -->
<function functionName="MAX" functionBody="{?$0&gt;$1}{$0}{?}{$1}{?}"/>
<!-- 验证混合内容 -->
<base type="string">
  <tags><tag language="English"/></tags>
  <functions>...</functions>
  <strings>...</strings>
</base>
```

### 1.2 XmlTestUtils核心功能

#### 1.2.1 布尔值标准化

**验收标准:**
- [ ] **格式覆盖**: 支持所有常见布尔值格式
- [ ] **标准化一致性**: 所有布尔值必须标准化为"true"/"false"
- [ ] **性能影响**: 标准化过程不能显著影响性能

**支持的布尔值格式:**
```csharp
// True值: "true", "True", "TRUE", "1", "yes", "Yes", "YES", "on", "On", "ON"
// False值: "false", "False", "FALSE", "0", "no", "No", "NO", "off", "Off", "OFF"
```

**验证测试:**
```csharp
[Theory]
[InlineData("true", "true")]
[InlineData("True", "true")]
[InlineData("1", "true")]
[InlineData("false", "false")]
[InlineData("0", "false")]
public void BooleanNormalization_ShouldWorkCorrectly(string input, string expected)
{
    var xml = $"<element boolAttr=\"{input}\"/>";
    var normalized = XmlTestUtils.NormalizeXml(xml);
    Assert.Contains($"boolAttr=\"{expected}\"", normalized);
}
```

#### 1.2.2 属性排序

**验收标准:**
- [ ] **命名空间优先**: 命名空间属性必须排在最前面
- [ ] **字母顺序**: 普通属性必须按字母顺序排序
- [ ] **一致性**: 相同元素的属性顺序必须保持一致

**验证测试:**
```csharp
[Fact]
public void AttributeOrdering_ShouldBeConsistent()
{
    var xml = "<element z=\"3\" a=\"1\" xmlns:ns=\"http://example.com\" b=\"2\"/>";
    var normalized = XmlTestUtils.NormalizeXml(xml);
    
    // 验证命名空间属性在前
    Assert.StartsWith("xmlns:ns", normalized);
    // 验证字母顺序
    Assert.Contains("a=\"1\" b=\"2\" z=\"3\"", normalized);
}
```

#### 1.2.3 空元素处理

**验收标准:**
- [ ] **格式保持**: 原始格式必须保持（自闭合vs开始/结束标签）
- [ ] **一致性**: 相同类型的空元素处理方式必须一致
- [ ] **性能**: 空元素处理不能显著影响性能

**验证测试:**
```csharp
[Theory]
[InlineData("<empty/>")]  // 自闭合标签
[InlineData("<empty></empty>")]  // 开始/结束标签
public void EmptyElementHandling_ShouldPreserveFormat(string xml)
{
    var normalized = XmlTestUtils.NormalizeXml(xml);
    Assert.Equal(xml, normalized);
}
```

## 2. 非功能性验收标准

### 2.1 性能标准

#### 2.1.1 XML处理性能

**验收标准:**
- [ ] **序列化性能**: 1MB XML文件序列化时间 ≤ 100ms
- [ ] **反序列化性能**: 1MB XML文件反序列化时间 ≤ 100ms
- [ ] **比较性能**: 1MB XML文件比较时间 ≤ 50ms
- [ ] **内存使用**: 峰值内存使用 ≤ 50MB

**性能测试:**
```csharp
[Fact]
public void XmlProcessingPerformance_ShouldMeetRequirements()
{
    var largeXml = GenerateLargeXml(1024 * 1024); // 1MB
    
    // 测试序列化性能
    var sw = Stopwatch.StartNew();
    var obj = XmlTestUtils.Deserialize<TestObject>(largeXml);
    var serialized = XmlTestUtils.Serialize(obj);
    sw.Stop();
    
    Assert.True(sw.ElapsedMilliseconds <= 100, 
        $"Serialization took {sw.ElapsedMilliseconds}ms, expected ≤ 100ms");
}
```

#### 2.1.2 内存管理

**验收标准:**
- [ ] **无内存泄漏**: 长时间运行后不能有内存泄漏
- [ ] **内存使用效率**: 内存使用必须在合理范围内
- [ ] **垃圾回收**: 不能有频繁的垃圾回收

**内存测试:**
```csharp
[Fact]
public void MemoryUsage_ShouldBeEfficient()
{
    var initialMemory = GC.GetTotalMemory(true);
    
    // 执行XML处理操作
    for (int i = 0; i < 1000; i++)
    {
        var xml = "<test><item id=\"1\"/></test>";
        var obj = XmlTestUtils.Deserialize<TestObject>(xml);
        var serialized = XmlTestUtils.Serialize(obj);
    }
    
    var finalMemory = GC.GetTotalMemory(true);
    var memoryIncrease = finalMemory - initialMemory;
    
    Assert.True(memoryIncrease <= 50 * 1024 * 1024, 
        $"Memory increase {memoryIncrease} bytes, expected ≤ 50MB");
}
```

### 2.2 代码质量标准

#### 2.2.1 代码复杂度

**验收标准:**
- [ ] **圈复杂度**: 所有方法的圈复杂度 ≤ 10
- [ ] **认知复杂度**: 所有方法的认知复杂度 ≤ 15
- [ ] **代码行数**: 单个方法不超过50行

**复杂度检查:**
```csharp
// XmlTestUtils.NormalizeXml方法复杂度检查
// 目标: 圈复杂度 ≤ 10
public static string NormalizeXml(string xml, XmlComparisonOptions? options = null)
{
    // 复杂逻辑需要分解为更小的方法
    if (options.IgnoreComments) RemoveComments(doc);
    if (options.IgnoreWhitespace) NormalizeWhitespace(doc);
    if (options.IgnoreAttributeOrder) SortAttributes(doc);
    if (options.AllowCaseInsensitiveBooleans) NormalizeBooleans(doc);
    
    return doc.ToString();
}
```

#### 2.2.2 代码重复率

**验收标准:**
- [ ] **重复率**: 代码重复率 ≤ 5%
- [ ] **相似代码**: 相似代码块需要重构为共享方法
- [ ] **DRY原则**: 遵循Don't Repeat Yourself原则

**重复代码检查:**
```csharp
// 重构前的重复代码
private void ProcessElement1(XElement element)
{
    foreach (var attr in element.Attributes())
    {
        if (attr.Name.LocalName.EndsWith("Global"))
        {
            // 处理逻辑
        }
    }
}

private void ProcessElement2(XElement element)
{
    foreach (var attr in element.Attributes())
    {
        if (attr.Name.LocalName.EndsWith("Global"))
        {
            // 相同的处理逻辑
        }
    }
}

// 重构后的共享代码
private void ProcessGlobalAttributes(XElement element)
{
    foreach (var attr in element.Attributes())
    {
        if (attr.Name.LocalName.EndsWith("Global"))
        {
            // 统一的处理逻辑
        }
    }
}
```

### 2.3 测试质量标准

#### 2.3.1 测试覆盖率

**验收标准:**
- [ ] **行覆盖率**: ≥ 85%
- [ ] **分支覆盖率**: ≥ 80%
- [ [ **方法覆盖率**: ≥ 90%
- [ ] **类覆盖率**: ≥ 95%

**覆盖率验证:**
```bash
# 运行覆盖率测试
dotnet test --collect:"XPlat Code Coverage"

# 验证覆盖率报告
# 确保所有关键路径都有测试覆盖
```

#### 2.3.2 测试质量

**验收标准:**
- [ ] **测试独立性**: 测试之间不能有依赖关系
- [ ] **测试可重复性**: 测试结果必须稳定可重复
- [ ] **测试命名**: 测试名称必须清晰表达测试意图
- [ ] **测试文档**: 复杂测试必须有文档说明

**测试质量检查:**
```csharp
// 好的测试示例
[Fact]
public void XmlTestUtils_NormalizeXml_ShouldHandleBooleanAttributesCorrectly()
{
    // Arrange
    var xml = "<element isActive=\"True\" isVisible=\"1\" isEditable=\"false\"/>";
    
    // Act
    var result = XmlTestUtils.NormalizeXml(xml);
    
    // Assert
    Assert.Contains("isActive=\"true\"", result);
    Assert.Contains("isVisible=\"true\"", result);
    Assert.Contains("isEditable=\"false\"", result);
}
```

## 3. 质量指标监控

### 3.1 持续集成指标

#### 3.1.1 构建成功率

**验收标准:**
- [ ] **构建成功率**: ≥ 95%
- [ ] **构建时间**: ≤ 10分钟
- [ ] **测试执行时间**: ≤ 5分钟

**监控配置:**
```yaml
# GitHub Actions配置
name: Quality Monitoring
on: [push, pull_request]
jobs:
  quality-check:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Setup .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '9.0.x'
      - name: Run tests
        run: dotnet test --collect:"XPlat Code Coverage"
      - name: Upload coverage
        uses: codecov/codecov-action@v1
```

#### 3.1.2 代码质量门禁

**验收标准:**
- [ ] **SonarQube质量门**: 必须通过
- [ ] **代码异味**: 0个阻断级别问题
- [ ] **安全漏洞**: 0个高危漏洞

**质量门禁配置:**
```xml
<!-- SonarQube质量门禁 -->
QualityGate>
  <condition metric="coverage" operator="GT" value="85"/>
  <condition metric="duplicated_lines" operator="LT" value="5"/>
  <condition metric="blocker_violations" operator="EQ" value="0"/>
  <condition metric="critical_violations" operator="EQ" value="0"/>
</QualityGate>
```

### 3.2 性能监控

#### 3.2.1 性能基准

**验收标准:**
- [ ] **性能回归**: 性能下降不超过10%
- [ ] **内存增长**: 内存使用增长不超过5%
- [ ] **响应时间**: 关键操作响应时间保持稳定

**性能基准测试:**
```csharp
[MemoryDiagnoser]
public class XmlProcessingBenchmark
{
    [Benchmark]
    public void SerializeLargeXml()
    {
        var obj = CreateLargeTestObject();
        XmlTestUtils.Serialize(obj);
    }
    
    [Benchmark]
    public void DeserializeLargeXml()
    {
        var xml = GetLargeTestXml();
        XmlTestUtils.Deserialize<TestObject>(xml);
    }
}
```

## 4. 验收流程

### 4.1 验收测试流程

#### 4.1.1 单元测试验收

**流程:**
1. 运行所有单元测试
2. 验证测试通过率 ≥ 98%
3. 检查代码覆盖率 ≥ 85%
4. 验证没有回归问题

**验收命令:**
```bash
# 运行完整测试套件
dotnet test --verbosity normal --collect:"XPlat Code Coverage"

# 验证特定测试
dotnet test --filter "FullyQualifiedName~RoundTrip"
```

#### 4.1.2 集成测试验收

**流程:**
1. 运行所有集成测试
2. 验证往返测试通过率 = 100%
3. 检查性能指标达标
4. 验证XML处理功能正常

**验收命令:**
```bash
# 运行集成测试
dotnet test BannerlordModEditor.Common.Tests --filter "TestCategory=Integration"

# 运行往返测试
dotnet test --filter "FullyQualifiedName~RoundTrip"
```

### 4.2 验收检查清单

#### 4.2.1 功能验收检查清单

**SiegeEngines修复:**
- [ ] 往返测试通过
- [ ] 根元素名称正确
- [ ] 所有属性正确处理
- [ ] 性能指标达标

**SpecialMeshes修复:**
- [ ] 往返测试通过
- [ ] 嵌套结构正确
- [ ] 空元素处理正确
- [ ] 性能指标达标

**LanguageBase修复:**
- [ ] 往返测试通过
- [ ] 函数体转义正确
- [ ] 混合内容处理正确
- [ ] 布尔值标准化正确

#### 4.2.2 质量验收检查清单

**代码质量:**
- [ ] 代码复杂度达标
- [ ] 代码重复率达标
- [ ] 代码风格一致
- [ ] 文档完整

**测试质量:**
- [ ] 测试覆盖率达标
- [ ] 测试质量达标
- [ ] 测试独立性
- [ ] 测试可重复性

**性能质量:**
- [ ] 性能指标达标
- [ ] 内存使用合理
- [ ] 无内存泄漏
- [ ] 响应时间稳定

## 5. 交付标准

### 5.1 代码交付标准

#### 5.1.1 代码审查

**标准:**
- [ ] **代码审查**: 所有代码必须经过至少两人审查
- [ ] **审查通过率**: 审查通过率 ≥ 90%
- [ ] **审查时间**: 审查响应时间 ≤ 24小时

**审查清单:**
```markdown
## 代码审查清单
- [ ] 代码符合编码规范
- [ ] 单元测试覆盖充分
- [ ] 性能考虑充分
- [ ] 错误处理完善
- [ ] 文档完整
- [ ] 安全性考虑
```

#### 5.1.2 文档交付

**标准:**
- [ ] **API文档**: 所有公共API必须有文档
- [ ] **用户文档**: 用户指南必须完整
- [ ] **开发文档**: 开发者文档必须完整
- [ ] **部署文档**: 部署指南必须完整

### 5.2 发布标准

#### 5.2.1 发布准备

**标准:**
- [ ] **版本管理**: 版本号正确
- [ ] **变更日志**: 变更日志完整
- [ ] **依赖管理**: 依赖项更新
- [ ] **兼容性**: 向后兼容性保证

#### 5.2.2 发布验证

**标准:**
- [ ] **环境测试**: 所有环境测试通过
- [ ] **回归测试**: 回归测试通过
- [ ] **性能测试**: 性能测试通过
- [ ] **安全测试**: 安全测试通过

## 6. 质量保证

### 6.1 持续质量监控

#### 6.1.1 监控指标

**指标:**
- [ ] **测试通过率**: 实时监控测试通过率
- [ ] **性能指标**: 实时监控性能指标
- [ ] **错误率**: 实时监控错误率
- [ ] **用户反馈**: 实时监控用户反馈

#### 6.1.2 报警机制

**报警规则:**
- [ ] **测试失败**: 关键测试失败立即报警
- [ ] **性能下降**: 性能下降超过10%报警
- [ ] **错误增加**: 错误率增加超过5%报警
- [ ] **内存泄漏**: 检测到内存泄漏立即报警

### 6.2 持续改进

#### 6.2.1 改进计划

**改进周期:**
- [ ] **每周回顾**: 每周进行质量回顾
- [ ] **每月评估**: 每月评估质量改进效果
- [ ] **季度规划**: 季度规划质量改进方向

#### 6.2.2 知识管理

**知识积累:**
- [ ] **问题记录**: 记录所有问题和解决方案
- [ ] **最佳实践**: 总结最佳实践
- [ ] **经验分享**: 定期分享经验
- [ ] **培训计划**: 制定培训计划

通过严格执行这些验收标准，项目将达到95%的质量标准，并为用户提供稳定可靠的XML编辑功能。