# BannerlordModEditor-CLI 测试策略文档

## 文档概述

本文档定义了BannerlordModEditor-CLI项目的全面测试策略，确保项目的长期稳定性和质量保证。该策略基于xUnit测试框架，覆盖了从单元测试到系统测试的各个层次。

## 1. 测试目标和范围

### 1.1 测试目标
- **功能完整性**: 确保所有XML适配功能正确实现
- **数据完整性**: 保证XML往返测试的准确性
- **性能可靠性**: 验证大型XML文件处理的性能
- **错误处理**: 确保异常情况下的优雅处理
- **长期维护**: 建立可持续的测试体系

### 1.2 测试范围
- **核心功能**: XML序列化/反序列化
- **数据处理**: DO/DTO模式转换
- **文件系统**: 文件发现和管理
- **用户界面**: CLI和TUI交互
- **集成场景**: 端到端工作流

## 2. 测试级别和类型

### 2.1 单元测试 (Unit Tests)
**覆盖率要求**: ≥95%

**测试范围**:
- XML模型类的序列化/反序列化
- 数据转换器(Mappers)
- 服务层逻辑
- 工具类和辅助方法

**测试框架**: xUnit 2.5

**示例结构**:
```csharp
[Theory]
[InlineData("test_data.xml")]
public async Task XmlModel_ShouldPassRoundTripTest(string xmlFileName)
{
    // Arrange
    var xmlPath = Path.Combine("TestData", xmlFileName);
    
    // Act
    var result = await XmlTestUtils.RoundTripTest<XmlModel>(xmlPath);
    
    // Assert
    Assert.True(result.IsSuccess);
}
```

### 2.2 集成测试 (Integration Tests)
**覆盖率要求**: ≥90%

**测试范围**:
- 文件发现服务集成
- XML适配流程
- 数据库操作集成
- 外部依赖集成

**测试标记**: `[Trait("Category", "Integration")]`

### 2.3 系统测试 (System Tests)
**覆盖率要求**: ≥85%

**测试范围**:
- 完整的XML处理工作流
- CLI命令行界面
- TUI用户界面
- 端到端用户场景

### 2.4 性能测试 (Performance Tests)
**覆盖率要求**: ≥80%

**测试范围**:
- 大型XML文件处理
- 内存使用监控
- 并发处理能力
- 响应时间测量

## 3. 测试环境配置

### 3.1 开发环境
- **操作系统**: Linux (Ubuntu 22.04+)
- **.NET版本**: 9.0.x
- **测试框架**: xUnit 2.5
- **覆盖率工具**: coverlet
- **报告工具**: ReportGenerator

### 3.2 测试数据管理
- **测试数据目录**: `BannerlordModEditor.Common.Tests/TestData/`
- **数据分类**: 按功能模块组织
- **数据版本**: 与项目版本同步
- **数据更新**: 定期同步真实游戏数据

### 3.3 测试依赖
```xml
<!-- 测试框架 -->
<PackageReference Include="xunit" Version="2.5.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
<PackageReference Include="xunit.extensibility.execution" Version="2.5.3" />
<PackageReference Include="xunit.extensibility.core" Version="2.5.3" />

<!-- 覆盖率工具 -->
<PackageReference Include="coverlet.collector" Version="6.0.0" />
<PackageReference Include="coverlet.msbuild" Version="6.0.0" />

<!-- 测试辅助工具 -->
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Moq" Version="4.20.69" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
```

## 4. 测试质量标准

### 4.1 代码覆盖率要求
- **单元测试**: ≥95% 行覆盖率
- **集成测试**: ≥90% 功能覆盖率
- **系统测试**: ≥85% 场景覆盖率
- **性能测试**: ≥80% 性能关键路径覆盖率

### 4.2 测试质量指标
- **测试通过率**: 100%
- **测试稳定性**: 无随机失败
- **测试执行时间**: < 5分钟（单元测试）
- **内存泄漏**: 0个检测到的泄漏

### 4.3 测试数据要求
- **真实性**: 使用真实的骑砍2XML数据
- **完整性**: 覆盖所有XML类型和结构
- **边界情况**: 包含异常和边界数据
- **版本控制**: 测试数据纳入版本管理

## 5. 测试策略实施

### 5.1 XML往返测试策略
```csharp
// 核心测试策略
public class XmlRoundTripTests
{
    [Theory]
    [InlineData("Credits.xml")]
    [InlineData("Adjustables.xml")]
    [InlineData("AchievementData/gog_achievement_data.xml")]
    // ... 475个XML文件
    public async Task XmlFile_ShouldPassRoundTripTest(string xmlFileName)
    {
        // 1. 读取原始XML
        var originalXml = await File.ReadAllTextAsync(xmlPath);
        
        // 2. 反序列化为模型
        var model = XmlTestUtils.Deserialize<XmlModel>(originalXml);
        
        // 3. 序列化回XML
        var serializedXml = XmlTestUtils.Serialize(model);
        
        // 4. 验证结构一致性
        var result = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
        Assert.True(result.IsSuccess, result.ErrorMessage);
    }
}
```

### 5.2 DO/DTO转换测试策略
```csharp
public class MapperTests
{
    [Fact]
    public void Mapper_ShouldHandleAllProperties()
    {
        // Arrange
        var source = new SourceModel { /* 初始化所有属性 */ };
        
        // Act
        var dto = Mapper.ToDTO(source);
        var result = Mapper.ToDO(dto);
        
        // Assert
        Assert.Equivalent(source, result);
    }
    
    [Fact]
    public void Mapper_ShouldHandleNullValues()
    {
        // 测试空值处理
        Assert.Null(Mapper.ToDTO(null));
        Assert.Null(Mapper.ToDO(null));
    }
}
```

### 5.3 性能测试策略
```csharp
[Trait("Category", "Performance")]
public class PerformanceTests
{
    [Fact]
    public void LargeXmlProcessing_ShouldMeetPerformanceRequirements()
    {
        // Arrange
        var largeXml = File.ReadAllText("large_test_data.xml");
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = XmlProcessor.Process(largeXml);
        stopwatch.Stop();
        
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 5000, 
            $"处理时间过长: {stopwatch.ElapsedMilliseconds}ms");
        Assert.NotNull(result);
    }
}
```

## 6. 测试自动化策略

### 6.1 持续集成测试
- **触发条件**: 每次代码提交和PR
- **测试级别**: 全部测试套件
- **失败处理**: 阻止合并到主分支
- **报告生成**: 自动生成测试报告

### 6.2 定期测试
- **每日测试**: 完整回归测试
- **每周测试**: 性能基准测试
- **每月测试**: 全面质量评估

### 6.3 发布前测试
- **候选版本测试**: 完整测试套件
- **回归测试**: 历史问题验证
- **性能测试**: 负载和压力测试
- **安全测试**: 漏洞扫描

## 7. 测试数据管理

### 7.1 测试数据组织
```
TestData/
├── Credits/                    # Credits相关XML
├── AchievementData/            # 成就数据XML
├── Layouts/                    # 布局文件XML
├── Languages/                  # 语言文件XML
├── Multiplayer/                # 多人游戏XML
├── Engine/                     # 引擎参数XML
├── Game/                       # 游戏机制XML
├── Audio/                      # 音频系统XML
└── EdgeCases/                  # 边界情况XML
```

### 7.2 测试数据维护
- **定期更新**: 与游戏版本同步
- **版本控制**: 所有测试数据纳入Git管理
- **数据验证**: 确保测试数据的有效性
- **备份策略**: 重要测试数据多重备份

## 8. 测试报告和监控

### 8.1 测试报告内容
- **执行摘要**: 测试通过率和关键指标
- **覆盖率报告**: 详细的代码覆盖率分析
- **性能报告**: 性能基准和趋势分析
- **问题报告**: 失败测试和错误分析

### 8.2 质量监控指标
- **测试稳定性**: 测试通过率趋势
- **代码质量**: 覆盖率和复杂度
- **性能指标**: 响应时间和资源使用
- **缺陷密度**: 每千行代码的缺陷数

## 9. 测试团队职责

### 9.1 测试工程师职责
- **测试用例设计**: 设计全面的测试用例
- **测试自动化**: 实现自动化测试脚本
- **测试执行**: 执行测试并分析结果
- **质量报告**: 生成质量报告和建议

### 9.2 开发工程师职责
- **单元测试**: 编写高质量的单元测试
- **集成测试**: 配合集成测试的实施
- **测试维护**: 维护现有测试用例
- **缺陷修复**: 及时修复测试发现的缺陷

## 10. 测试工具和技术栈

### 10.1 核心测试工具
- **xUnit**: 单元测试框架
- **coverlet**: 代码覆盖率工具
- **ReportGenerator**: 测试报告生成器
- **FluentAssertions**: 断言库
- **Moq**: 模拟框架

### 10.2 性能测试工具
- **BenchmarkDotNet**: 性能基准测试
- **MemoryProfiler**: 内存分析
- **Visual Studio Diagnostic Tools**: 诊断工具

### 10.3 持续集成工具
- **GitHub Actions**: CI/CD流水线
- **Azure DevOps**: 构建和发布管理
- **SonarQube**: 代码质量分析

## 11. 测试风险和缓解措施

### 11.1 主要测试风险
- **测试数据不完整**: 遗漏某些XML类型
- **性能问题**: 大型文件处理缓慢
- **内存泄漏**: 长时间运行导致内存问题
- **并发问题**: 多线程处理时的竞态条件

### 11.2 风险缓解措施
- **数据完整性**: 建立测试数据清单和验证机制
- **性能优化**: 实施性能监控和优化策略
- **内存管理**: 使用内存分析工具定期检查
- **并发安全**: 实施线程安全设计和测试

## 12. 测试策略评审和改进

### 12.1 定期评审
- **月度评审**: 测试策略有效性评估
- **季度评审**: 测试覆盖率和质量指标分析
- **年度评审**: 全面测试策略优化

### 12.2 持续改进
- **技术更新**: 跟踪新的测试技术和工具
- **最佳实践**: 学习和应用行业最佳实践
- **反馈收集**: 收集团队反馈并改进测试策略

## 13. 附录

### 13.1 测试用例模板
```csharp
[Fact]
[Trait("Category", "Unit")]
public void TestMethodName_ShouldExpectedBehavior_WhenCondition()
{
    // Arrange
    // 准备测试数据和条件
    
    // Act
    // 执行测试操作
    
    // Assert
    // 验证测试结果
}
```

### 13.2 测试数据清单
- **核心XML文件**: 475个测试文件
- **边界情况**: 50个特殊场景
- **性能测试**: 10个大型文件
- **集成测试**: 20个端到端场景

### 13.3 测试环境配置
- **开发环境**: 本地开发测试环境
- **CI环境**: GitHub Actions测试环境
- **性能环境**: 专用性能测试环境
- **生产环境**: 生产环境监控

---

本文档将随着项目的发展而持续更新，确保测试策略与项目需求保持一致。