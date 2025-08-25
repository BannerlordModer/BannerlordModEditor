# XML转换框架测试套件

## 概述

这是一个为通用XML转换框架创建的完整测试套件，覆盖了所有核心功能和边界情况。

## 测试文件结构

```
BannerlordModEditor.Common.Tests/Conversion/
├── XmlConversionFrameworkTests.cs          # 基础测试类
├── XmlToTableConversionTests.cs             # XmlToTable转换测试
├── TableToXmlConversionTests.cs             # TableToXml转换测试
├── XmlCsvConversionTests.cs                  # XML与CSV互转测试
├── XmlJsonConversionTests.cs                # XML与JSON互转测试
├── BatchConversionTests.cs                  # 批量转换测试
├── ConversionStrategyTests.cs               # 转换策略测试
├── IntegrationAndPerformanceTests.cs        # 集成和性能测试
├── ComprehensiveConversionTests.cs          # 综合测试
├── XmlConversionTestHelpers.cs              # 测试辅助方法
└── CompilationVerificationTests.cs           # 编译验证测试
```

## 测试数据文件

```
BannerlordModEditor.Common.Tests/TestData/Conversion/
├── simple_attributes.xml                    # 简单属性XML
├── nested_skills.xml                         # 嵌套技能XML
├── complex_combat_parameters.xml            # 复杂战斗参数XML
├── game_items.xml                           # 游戏物品XML
├── game_characters.xml                      # 游戏角色XML
└── game_quests.xml                         # 游戏任务XML
```

## 测试覆盖范围

### 1. 核心转换功能
- **XmlToTableAsync**: XML转表格格式
- **TableToXmlAsync**: 表格格式转XML
- **XmlToCsvAsync**: XML转CSV格式
- **CsvToXmlAsync**: CSV格式转XML
- **XmlToJsonAsync**: XML转JSON格式
- **JsonToXmlAsync**: JSON格式转XML

### 2. 批量转换功能
- 并行处理
- 顺序处理
- 全局选项应用
- 错误处理和恢复
- 性能优化

### 3. 转换策略
- 简单XML结构策略
- 中等复杂度策略
- 复杂XML结构策略
- 深度限制策略
- 属性过滤策略
- 嵌套元素处理策略

### 4. 集成测试
- 端到端转换流程
- 往返转换一致性
- 多格式转换链
- 错误处理和恢复

### 5. 性能测试
- 小文件处理性能
- 大文件处理性能
- 内存使用监控
- 并发处理性能
- 批量转换性能

### 6. 边界情况测试
- 空文件处理
- 无效XML处理
- 特殊字符处理
- 大数据集处理
- 编码问题处理

## 测试分类

### 单元测试
- 各个转换方法的独立测试
- 参数验证测试
- 边界条件测试

### 集成测试
- 完整转换流程测试
- 多格式互转测试
- 往返转换一致性测试

### 性能测试
- 执行时间测试
- 内存使用测试
- 并发处理测试

### 综合测试
- 复杂场景测试
- 错误恢复测试
- 数据完整性测试

## 使用方法

### 运行所有测试
```bash
dotnet test BannerlordModEditor.Common.Tests --filter "Conversion"
```

### 运行特定测试类别
```bash
# 运行转换功能测试
dotnet test BannerlordModEditor.Common.Tests --filter "XmlToTable"

# 运行批量转换测试
dotnet test BannerlordModEditor.Common.Tests --filter "Batch"

# 运行性能测试
dotnet test BannerlordModEditor.Common.Tests --filter "Performance"
```

### 运行单个测试文件
```bash
dotnet test BannerlordModEditor.Common.Tests --filter "XmlToTableConversionTests"
```

## 测试数据

测试数据包含多种类型的XML文件：

1. **simple_attributes.xml**: 简单的属性列表，用于测试基本转换功能
2. **nested_skills.xml**: 包含嵌套结构的技能数据，用于测试复杂转换
3. **complex_combat_parameters.xml**: 复杂的战斗参数数据，包含多层嵌套
4. **game_items.xml**: 游戏物品数据，包含多种属性类型
5. **game_characters.xml**: 游戏角色数据，包含复杂的嵌套结构
6. **game_quests.xml**: 游戏任务数据，包含多层嵌套和数组

## 辅助方法

`XmlConversionTestHelpers`类提供了多种辅助方法：

- `CreateSimpleTestXml()`: 创建简单测试XML
- `CreateNestedTestXml()`: 创建嵌套测试XML
- `CreateComplexTestXml()`: 创建复杂测试XML
- `ValidateTableData()`: 验证表格数据完整性
- `ValidateXmlStructure()`: 验证XML结构
- `MeasureExecutionTime()`: 测量执行时间

## 测试断言

测试使用了多种断言方法：

- `AssertTableConversionResult()`: 验证表格转换结果
- `AssertConversionResult()`: 验证通用转换结果
- `AssertBatchConversionResult()`: 验证批量转换结果
- `ValidateTableData()`: 验证表格数据结构
- `ValidateXmlStructure()`: 验证XML结构

## 性能基准

测试套件包含性能基准：

- 小文件转换应在100ms内完成
- 中等文件转换应在200ms内完成
- 大文件转换应在300ms内完成
- 批量转换应能高效处理
- 内存使用应保持在合理范围内

## 错误处理

测试覆盖了多种错误情况：

- 文件不存在
- 无效XML格式
- 权限问题
- 磁盘空间不足
- 并发访问冲突

## 扩展指南

### 添加新的测试用例

1. 在相应的测试类中添加新的测试方法
2. 使用`[Fact]`或`[Theory]`特性标记
3. 使用`Arrange-Act-Assert`模式组织代码
4. 添加适当的断言验证结果

### 添加新的测试数据

1. 在`TestData/Conversion/`目录下创建新的XML文件
2. 确保文件格式正确
3. 在测试中引用新的测试文件

### 添加新的转换格式

1. 创建新的测试类继承`XmlConversionFrameworkTests`
2. 实现相应的转换测试方法
3. 添加往返转换测试
4. 添加性能测试

## 注意事项

1. **临时文件**: 测试会创建临时文件，测试结束后会自动清理
2. **测试数据**: 测试数据文件应该保持小而专注
3. **性能**: 性能测试应该在适当的环境中运行
4. **并发**: 并发测试应该考虑资源限制
5. **错误处理**: 应该测试各种错误情况的处理

## 故障排除

### 测试失败
- 检查测试数据文件是否存在
- 确认XML格式是否正确
- 检查文件权限
- 查看详细的错误信息

### 性能问题
- 确保测试环境没有其他资源占用
- 检查内存使用情况
- 考虑调整性能阈值

### 编译错误
- 确认所有依赖项都已正确引用
- 检查命名空间引用
- 确认方法签名正确

## 贡献指南

1. 遵循现有的测试命名约定
2. 保持测试的独立性和可重复性
3. 添加适当的注释和文档
4. 确保测试覆盖新的功能
5. 运行所有测试确保没有回归