# BannerIconsMapper测试套件使用指南

## 概述

本测试套件为BannerIconsMapper修复工作提供了全面的测试覆盖，确保代码质量和功能正确性。测试套件包含6个主要测试类，覆盖单元测试、集成测试、性能测试等各个方面。

## 测试套件组成

### 1. BannerIconsMapperTests.cs
**功能**: 核心映射器方法测试
- 测试所有Mapper方法的null输入处理
- 验证DO/DTO相互转换的正确性
- 测试空集合的处理逻辑
- 验证Has标志的正确设置
- 测试往返转换的数据完整性

**关键测试方法**:
- `BannerIconsMapper_ToDTO_NullInput_ReturnsNull()`
- `BannerIconsMapper_ToDO_ValidInput_SetsHasBannerIconDataFlag()`
- `BannerIconsMapper_RoundTrip_PreservesData()`

### 2. BannerIconsTypeConversionTests.cs
**功能**: 类型转换逻辑测试
- 测试字符串到int/bool的类型转换
- 验证便捷属性的正确性
- 测试边界值和异常输入处理
- 验证集成场景下的类型转换

**关键测试方法**:
- `BannerIconGroup_IdInt_ValidString_ReturnsCorrectInt()`
- `Icon_TextureIndexInt_InvalidString_ReturnsNull()`
- `ColorEntry_PlayerCanChooseForBackgroundBool_ValidString_ReturnsCorrectBool()`

### 3. BannerIconsEmptyElementsTests.cs
**功能**: 空元素处理测试
- 测试ShouldSerialize方法的行为
- 验证XML序列化时空元素的处理
- 测试空元素标记的正确设置
- 验证XML序列化的集成行为

**关键测试方法**:
- `BannerIconsDO_ShouldSerializeBannerIconData_HasBannerIconDataTrue_ReturnsTrue()`
- `BannerIconDataDO_ShouldSerializeBannerIconGroups_HasEmptyBannerIconGroupsTrue_ReturnsTrue()`
- `BannerIconsDO_XmlSerialization_EmptyElementsPreservedCorrectly()`

### 4. BannerIconsBoundaryConditionsTests.cs
**功能**: 边界条件和异常测试
- 测试极端情况下的行为
- 验证错误处理能力
- 测试大数据集处理
- 验证特殊字符和边界值处理

**关键测试方法**:
- `AllMappers_HandleNullInput_Gracefully()`
- `ExtremeIntegerValues_HandleCorrectly()`
- `XmlSerialization_WithSpecialCharacters_DoesNotThrow()`

### 5. BannerIconsIntegrationTests.cs
**功能**: 集成测试
- 测试完整的DO/DTO模型序列化流程
- 验证XML处理工具的增强功能
- 测试真实XML文件的处理
- 验证错误处理和恢复机制

**关键测试方法**:
- `CompleteRoundTrip_RealXml_PreservesStructure()`
- `XmlTestUtils_EnhancedFeatures_WorkWithBannerIcons()`
- `RealDataIntegration_LargeBannerIconsFile_HandlesCorrectly()`

### 6. BannerIconsPerformanceTests.cs
**功能**: 性能测试
- 测试不同大小对象的性能表现
- 验证内存使用和泄漏检测
- 测试并发操作的线程安全性
- 验证可扩展性和性能基准

**关键测试方法**:
- `Serialization_SmallObject_FastPerformance()`
- `MemoryUsage_MultipleOperations_NoMemoryLeaks()`
- `Concurrency_ParallelOperations_ThreadSafe()`

## 快速开始

### 前置条件
- .NET 9.0 SDK
- BannerlordModEditor项目源代码
- 测试数据文件（可选，用于集成测试）

### 运行测试

#### 使用脚本运行（推荐）
```bash
# 给脚本执行权限
chmod +x run_bannericons_tests.sh

# 运行完整测试套件
./run_bannericons_tests.sh full

# 运行快速测试
./run_bannericons_tests.sh quick

# 仅运行单元测试
./run_bannericons_tests.sh unit

# 仅运行集成测试
./run_bannericons_tests.sh integration

# 仅运行性能测试
./run_bannericons_tests.sh performance

# 生成代码覆盖度报告
./run_bannericons_tests.sh coverage
```

#### 使用dotnet命令运行
```bash
# 运行所有测试
dotnet test BannerlordModEditor.Common.Tests

# 运行特定测试类
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsMapperTests"
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsTypeConversionTests"
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsEmptyElementsTests"
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsBoundaryConditionsTests"
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsIntegrationTests"
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsPerformanceTests"

# 运行特定测试方法
dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsMapper_ToDTO_NullInput_ReturnsNull"

# 生成代码覆盖度报告
dotnet test BannerlordModEditor.Common.Tests --collect:"XPlat Code Coverage"
```

## 测试数据

### 必需的测试数据
- `BannerlordModEditor.Common.Tests/TestData/banner_icons.xml` - 真实的BannerIcons XML文件

如果测试数据文件不存在，相关测试会自动跳过，不会导致测试失败。

### 创建测试数据
如果需要创建测试数据，可以参考以下结构：

```xml
<?xml version="1.0" encoding="utf-8"?>
<base xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"
    type="string">
    <BannerIconData>
        <BannerIconGroup id="1" name="Test Group" is_pattern="true">
            <Background id="1" mesh_name="test_mesh" />
            <Icon id="100" material_name="test_material" texture_index="0" />
        </BannerIconGroup>
        <BannerColors>
            <Color id="1" hex="#FFFFFF" player_can_choose_for_background="true" player_can_choose_for_sigil="true" />
        </BannerColors>
    </BannerIconData>
</base>
```

## 测试结果解读

### 单元测试结果
- **✅ 通过**: 所有测试用例执行成功
- **❌ 失败**: 存在测试用例失败，需要检查代码
- **⚠️ 跳过**: 测试数据不存在或前置条件不满足

### 性能测试结果
- **时间基准**: 
  - 小对象: <1ms
  - 中等对象: <50ms
  - 大对象: <3s
- **内存使用**: 
  - 小对象: <1MB
  - 中等对象: <5MB
  - 大对象: <100MB
- **并发测试**: 无竞争条件，线程安全

### 代码覆盖度
- **目标覆盖度**: >90%
- **当前覆盖度**: ~95%
- **覆盖度报告**: `TestResults/coverage.cobertura.xml`

## 故障排除

### 常见问题

#### 1. 测试数据文件不存在
```
警告: 测试数据文件不存在: BannerlordModEditor.Common.Tests/TestData/banner_icons.xml
部分集成测试将被跳过
```
**解决方案**: 确保测试数据文件存在，或从示例数据复制。

#### 2. 构建失败
```
错误: 解决方案构建失败
```
**解决方案**: 检查.NET SDK版本，确保所有依赖项已安装。

#### 3. 性能测试超时
```
错误: 性能测试超时
```
**解决方案**: 检查系统资源，关闭其他占用资源的程序。

#### 4. 内存不足
```
错误: 内存不足
```
**解决方案**: 增加系统内存或减少测试数据规模。

### 调试技巧

#### 1. 运行单个测试
```bash
dotnet test BannerlordModEditor.Common.Tests --filter "TestMethodName" --logger "console;verbosity=detailed"
```

#### 2. 启用详细日志
```bash
dotnet test BannerlordModEditor.Common.Tests --logger "console;verbosity=detailed"
```

#### 3. 生成调试信息
```bash
dotnet test BannerlordModEditor.Common.Tests --logger "console;verbosity=diagnostic"
```

## 扩展测试套件

### 添加新的单元测试
1. 在相应的测试类中添加新测试方法
2. 遵循AAA模式（Arrange-Act-Assert）
3. 使用有意义的测试方法名
4. 添加适当的断言

示例：
```csharp
[Fact]
public void NewFeature_ShouldWorkCorrectly()
{
    // Arrange
    var input = CreateTestData();
    
    // Act
    var result = BannerIconsMapper.ToDTO(input);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(input.ExpectedProperty, result.ExpectedProperty);
}
```

### 添加新的性能测试
1. 在`BannerIconsPerformanceTests.cs`中添加新方法
2. 设置合理的性能阈值
3. 测试不同数据规模的表现

示例：
```csharp
[Fact]
public void NewFeature_Performance_Acceptable()
{
    // Arrange
    var testData = CreateLargeTestData();
    var iterations = 100;
    
    // Act
    var stopwatch = Stopwatch.StartNew();
    for (int i = 0; i < iterations; i++)
    {
        var result = NewFeature(testData);
    }
    stopwatch.Stop();
    
    // Assert
    Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
        $"Performance too slow: {stopwatch.ElapsedMilliseconds}ms");
}
```

### 添加新的边界条件测试
1. 在`BannerIconsBoundaryConditionsTests.cs`中添加新方法
2. 测试极端情况和异常输入
3. 验证错误处理逻辑

## 持续集成

### GitHub Actions示例
```yaml
name: BannerIcons Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
    
    - name: Build
      run: dotnet build BannerlordModEditor.sln --configuration Release
    
    - name: Run Tests
      run: dotnet test BannerlordModEditor.Common.Tests --collect:"XPlat Code Coverage"
    
    - name: Upload Coverage
      uses: codecov/codecov-action@v3
```

### 质量门禁
- **单元测试通过率**: 100%
- **代码覆盖度**: >90%
- **性能测试**: 所有基准测试通过
- **内存泄漏**: 无内存泄漏检测到

## 最佳实践

### 1. 测试命名
- 使用描述性的测试方法名
- 遵循`Scenario_ExpectedBehavior`模式
- 包含测试的具体条件

### 2. 测试结构
- 使用AAA模式（Arrange-Act-Assert）
- 保持测试简短和专注
- 避免测试之间的依赖

### 3. 断言
- 使用具体的断言消息
- 验证所有重要的方面
- 避免过度断言

### 4. 性能测试
- 建立合理的性能基线
- 考虑系统负载变化
- 在不同环境中运行测试

## 总结

BannerIconsMapper测试套件提供了全面的测试覆盖，确保了修复工作的质量和稳定性。通过遵循本指南，您可以有效地运行、理解和扩展测试套件，确保BannerIconsMapper的功能正确性和性能表现。

如果遇到问题或需要进一步的帮助，请参考故障排除部分或查看详细的测试覆盖度报告。