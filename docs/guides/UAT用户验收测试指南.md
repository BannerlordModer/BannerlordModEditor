# UAT 用户验收测试指南

## 概述

本文档描述了BannerlordModEditor TUI项目的用户验收测试（UAT）框架和测试用例。UAT测试采用BDD（行为驱动开发）风格，使用xUnit框架实现，确保系统满足用户的实际需求和期望。

## 测试架构

### BDD测试结构

```
BannerlordModEditor.UAT.Tests/
├── Features/                    # BDD特性测试
│   ├── FileConversionFeature.cs      # 文件转换功能
│   ├── ErrorHandlingFeature.cs      # 错误处理功能
│   ├── TuiUserInterfaceFeature.cs   # TUI用户界面
│   └── PerformanceAndBoundaryFeature.cs # 性能和边界条件
├── Infrastructure/              # 测试基础设施
│   └── UatTestRunner.cs             # 测试运行器和报告生成
├── Suites/                      # 测试套件
│   └── CompleteUatTestSuite.cs       # 完整测试套件
├── Common/                      # 公共基类
│   └── BddTestBase.cs              # BDD测试基类
└── TestData/                   # 测试数据目录
```

### 测试分类

#### 1. 功能测试 (FileConversionFeature)
- **Excel到XML转换**: 验证基本的文件格式转换功能
- **XML到Excel转换**: 验证反向转换功能
- **大型文件处理**: 验证系统处理大量数据的能力
- **特殊字符处理**: 验证Unicode和特殊字符的支持
- **往返转换**: 验证数据在多次转换中的完整性

#### 2. 错误处理测试 (ErrorHandlingFeature)
- **不存在文件**: 验证对不存在文件的处理
- **无效格式**: 验证对不支持格式的处理
- **权限不足**: 验证权限错误的处理
- **损坏文件**: 验证损坏文件的检测和处理
- **磁盘空间**: 验证磁盘空间不足的处理
- **空输入**: 验证参数验证
- **文件锁定**: 验证文件锁定状态的处理

#### 3. 用户界面测试 (TuiUserInterfaceFeature)
- **基本工作流**: 验证文件选择和转换流程
- **格式检测**: 验证文件格式自动检测
- **方向切换**: 验证转换方向切换
- **状态管理**: 验证忙碌状态和命令禁用
- **清除操作**: 验证状态重置
- **错误恢复**: 验证错误状态处理
- **文件验证**: 验证输入验证逻辑

#### 4. 性能测试 (PerformanceAndBoundaryFeature)
- **大文件性能**: 验证大文件处理的性能基准
- **并发处理**: 验证并发转换的线程安全性
- **内存管理**: 验证内存使用和泄漏
- **边界条件**: 验证各种边界情况
- **文件大小限制**: 验证大文件处理能力
- **重复转换**: 验证多次转换的一致性

## 运行测试

### 前置条件

1. **环境要求**
   - .NET 9.0 SDK
   - 支持的操作系统：Linux、Windows、macOS

2. **依赖项**
   - 所有NuGet包已正确安装
   - 测试数据文件可用

### 运行单个测试

```bash
# 运行特定特性测试
dotnet test BannerlordModEditor.UAT.Tests --filter "FileConversionFeature"

# 运行特定测试方法
dotnet test BannerlordModEditor.UAT.Tests --filter "ExcelToXmlConversion_Success"
```

### 运行测试套件

```bash
# 运行完整UAT测试套件
dotnet test BannerlordModEditor.UAT.Tests --filter "CompleteUatTestSuite"

# 运行核心功能验证
dotnet test BannerlordModEditor.UAT.Tests --filter "CoreFunctionalityValidation"

# 运行性能基准测试
dotnet test BannerlordModEditor.UAT.Tests --filter "PerformanceBenchmark"

# 运行用户体验测试
dotnet test BannerlordModEditor.UAT.Tests --filter "UserExperienceTesting"

# 运行数据完整性测试
dotnet test BannerlordModEditor.UAT.Tests --filter "DataIntegrityTesting"
```

### 运行所有UAT测试

```bash
# 运行所有UAT测试
dotnet test BannerlordModEditor.UAT.Tests

# 生成详细输出
dotnet test BannerlordModEditor.UAT.Tests --logger "console;verbosity=detailed"
```

## 测试报告

### 控制台输出

测试运行时会生成详细的控制台输出，包括：

- 测试执行进度
- 每个测试的结果状态
- 性能指标（执行时间、内存使用）
- 错误信息和堆栈跟踪
- 按特性分组的测试统计

### JSON报告

测试完成后会生成JSON格式的详细报告，包含：

- 测试执行摘要
- 按特性分组的详细结果
- 失败测试的错误信息
- 性能指标和基准数据

报告文件位置：`/tmp/uat_report_yyyyMMdd_HHmmss.json`

### 报告示例

```text
==========================================
         UAT 测试执行报告
==========================================
总测试数: 25
通过测试: 24
失败测试: 1
通过率: 96.0%
总执行时间: 15432ms
平均执行时间: 617.28ms
==========================================

【FileConversion】
  测试数: 5, 通过: 5, 通过率: 100.0%
    ✅ 通过 ExcelToXmlConversion_Success (234ms)
    ✅ 通过 XmlToExcelConversion_Success (156ms)
    ✅ 通过 LargeFileConversion_Performance (8932ms)
    ✅ 通过 SpecialCharacterConversion_Integrity (267ms)
    ✅ 通过 RoundTripConversion_DataIntegrity (445ms)

【ErrorHandling】
  测试数: 8, 通过: 7, 通过率: 87.5%
    ✅ 通过 NonExistentSourceFile_ClearErrorMessage (45ms)
    ✅ 通过 InvalidFileFormat_FormatDetection (123ms)
    ❌ 失败 InsufficientPermissions_PermissionError (67ms)
       错误: 权限测试在当前环境下无法准确模拟
    ✅ 通过 EmptyInputs_ParameterValidation (34ms)
    ...
```

## 测试用例详解

### 文件转换功能测试

#### ExcelToXmlConversion_Success
**目标**: 验证Excel到XML的基本转换功能
**步骤**:
1. 创建包含中文和英文的测试Excel文件
2. 执行Excel到XML转换
3. 验证转换成功
4. 验证输出文件包含所有原始数据
5. 验证中文内容正确处理

**验证点**:
- 转换结果Success = true
- 输出XML文件存在且非空
- XML内容包含所有原始数据
- 中文字符正确编码

### 错误处理测试

#### NonExistentSourceFile_ClearErrorMessage
**目标**: 验证对不存在源文件的处理
**步骤**:
1. 指定一个不存在的源文件路径
2. 尝试执行转换操作
3. 验证系统返回适当的错误信息
4. 验证不会创建输出文件

**验证点**:
- 转换结果Success = false
- 错误信息包含"不存在"关键词
- 输出文件未被创建

### TUI用户界面测试

#### BasicFileSelectionAndConversion_Workflow
**目标**: 验证基本的UI工作流程
**步骤**:
1. 创建MainViewModel实例
2. 设置源文件和目标文件路径
3. 模拟文件格式检测
4. 执行转换命令
5. 验证状态更新和结果显示

**验证点**:
- ViewModel状态正确更新
- 转换命令正确执行
- 状态消息显示转换结果
- 忙碌状态正确管理

### 性能测试

#### LargeFilePerformance_Benchmark
**目标**: 验证大文件处理的性能
**步骤**:
1. 创建包含5000条记录的大型Excel文件
2. 执行转换并测量执行时间
3. 监控内存使用情况
4. 验证性能在可接受范围内

**验证点**:
- 转换成功完成
- 执行时间 < 30秒（5000条记录）
- 内存使用 < 100MB
- 每条记录处理时间 < 10ms

## 自定义测试

### 添加新的BDD测试

1. **继承基类**
```csharp
public class MyCustomFeature : UatTestBase
{
    public MyCustomFeature(ITestOutputHelper output) : base(output)
    {
    }
}
```

2. **使用BDD格式编写测试**
```csharp
[Fact]
public async Task MyCustomScenario()
{
    // Given - 准备测试数据
    var testData = CreateTestData();
    
    // When - 执行操作
    var result = await ExecuteAction(testData);
    
    // Then - 验证结果
    result.Should().BeSuccessful();
}
```

3. **使用ExecuteUatTest方法**
```csharp
await ExecuteUatTest("我的自定义测试", async () =>
{
    // 测试逻辑
});
```

### 添加新的测试套件

1. **创建测试套件类**
```csharp
public class MyCustomTestSuite : UatTestBase
{
    public MyCustomTestSuite(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task RunMyCustomTests()
    {
        await ExecuteUatTest("测试1", async () => { /*...*/ });
        await ExecuteUatTest("测试2", async () => { /*...*/ });
        
        GenerateUatReport();
    }
}
```

## 故障排除

### 常见问题

1. **测试失败**
   - 检查测试数据文件是否存在
   - 验证文件权限设置
   - 查看详细错误信息

2. **性能问题**
   - 确保系统资源充足
   - 检查磁盘空间
   - 关闭其他占用资源的程序

3. **权限错误**
   - 以适当权限运行测试
   - 检查文件和目录权限
   - 验证用户访问权限

### 调试技巧

1. **启用详细输出**
```bash
dotnet test BannerlordModEditor.UAT.Tests --logger "console;verbosity=detailed"
```

2. **运行单个测试**
```bash
dotnet test BannerlordModEditor.UAT.Tests --filter "TestMethodName"
```

3. **检查测试数据**
   - 确认TestData目录存在
   - 验证测试文件格式正确
   - 检查文件内容完整性

## 持续集成

### CI/CD配置

在CI/CD流水线中集成UAT测试：

```yaml
# GitHub Actions示例
- name: Run UAT Tests
  run: dotnet test BannerlordModEditor.UAT.Tests --logger "console;verbosity=detailed"
  
- name: Upload Test Reports
  uses: actions/upload-artifact@v2
  with:
    name: uat-test-reports
    path: /tmp/uat_report_*.json
```

### 测试阈值

设置测试通过率阈值：

- **必需通过率**: 95%
- **性能基准**: 基于历史数据
- **内存限制**: 基于系统配置

## 维护和更新

### 添加新功能测试

1. 为每个新功能创建对应的BDD测试
2. 更新测试套件以包含新测试
3. 更新文档说明新测试用例

### 更新性能基准

1. 定期审查性能测试结果
2. 根据系统性能调整基准阈值
3. 更新性能测试报告模板

### 测试数据管理

1. 定期清理临时测试文件
2. 更新测试数据以反映实际使用场景
3. 备份重要的测试数据文件

---

通过遵循本指南，您可以有效地运行和维护BannerlordModEditor TUI项目的UAT测试，确保系统质量和用户满意度。