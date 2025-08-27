# 测试执行指南

## 概述

本指南提供了Bannerlord Mod Editor UI测试套件的完整执行说明，包括本地开发环境和CI环境的配置。

## 测试套件结构

### 测试分类

1. **单元测试** - 测试单个组件的功能
   - `DependencyInjectionTests.cs` - 依赖注入配置验证
   - `EditorViewModelTests.cs` - 编辑器ViewModel功能测试
   - `TestDataManagementTests.cs` - 测试数据管理测试

2. **集成测试** - 测试组件间的交互
   - `EditorManagerIntegrationTests.cs` - 编辑器管理器集成测试
   - `UIWorkflowIntegrationTests.cs` - UI工作流集成测试
   - `CrossPlatformCompatibilityTests.cs` - 跨平台兼容性测试

3. **环境测试** - 测试特定环境下的行为
   - `GitHubActionsEnvironmentTests.cs` - GitHub Actions环境测试
   - `TestDeploymentVerificationTests.cs` - 测试部署验证测试

### 测试文件组织

```
BannerlordModEditor.UI.Tests/
├── DependencyInjection/
│   └── DependencyInjectionTests.cs
├── ViewModels/
│   └── EditorViewModelTests.cs
├── Helpers/
│   ├── TestDataHelper.cs
│   ├── TestServiceProvider.cs
│   └── TestDataManagementTests.cs
├── Integration/
│   ├── EditorManagerIntegrationTests.cs
│   ├── UIWorkflowIntegrationTests.cs
│   └── CrossPlatformCompatibilityTests.cs
├── Environment/
│   ├── GitHubActionsEnvironmentTests.cs
│   └── TestDeploymentVerificationTests.cs
├── TestData/
│   └── (测试数据文件)
├── test-run-settings.json
├── ci-test-profile.json
└── xunit.runner.json
```

## 本地开发环境设置

### 前置条件

- .NET 9.0 SDK
- Visual Studio 2022 或 VS Code
- Git

### 设置步骤

1. **克隆仓库**
   ```bash
   git clone <repository-url>
   cd BannerlordModEditor-GUI-Enhancement
   ```

2. **还原依赖**
   ```bash
   dotnet restore
   ```

3. **构建项目**
   ```bash
   dotnet build
   ```

4. **同步测试数据**
   ```bash
   # Windows
   .\Sync-TestData.bat
   
   # Linux/macOS
   chmod +x ./Sync-TestData.sh
   ./Sync-TestData.sh
   ```

### 运行测试

#### 运行所有测试
```bash
dotnet test
```

#### 运行特定测试项目
```bash
dotnet test BannerlordModEditor.UI.Tests
```

#### 运行特定测试类
```bash
dotnet test --filter "DependencyInjectionTests"
```

#### 运行特定测试方法
```bash
dotnet test --filter "TestServiceProvider_Should_Be_Configured_Correctly"
```

#### 运行测试并生成覆盖率报告
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### 调试测试

1. **在Visual Studio中调试**
   - 打开测试资源管理器
   - 右键点击测试方法
   - 选择"调试测试"

2. **在VS Code中调试**
   - 安装.NET测试扩展
   - 使用测试面板运行和调试测试

## CI环境配置

### GitHub Actions工作流

项目包含预配置的GitHub Actions工作流文件：

```yaml
# .github/workflows/ui-tests.yml
name: UI Tests

on:
  push:
    branches: [ main, feature/* ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
    - name: Sync test data
      run: chmod +x ./Sync-TestData.sh && ./Sync-TestData.sh
    - name: Run tests
      run: dotnet test --configuration Release --collect:"XPlat Code Coverage"
    - name: Generate coverage report
      run: dotnet reportgenerator -reports:coverage.xml -targetdir:coverage-report
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
```

### CI环境变量

| 变量名 | 描述 | 默认值 |
|--------|------|--------|
| `DOTNET_ENVIRONMENT` | .NET环境 | `Testing` |
| `TEST_MODE` | 测试模式标志 | `true` |
| `CI` | CI环境标志 | `true` |
| `GITHUB_ACTIONS` | GitHub Actions标志 | `true` |
| `LOG_LEVEL` | 日志级别 | `Information` |
| `DISABLE_UI` | 禁用UI标志 | `true` |
| `HEADLESS_MODE` | 无头模式标志 | `true` |
| `ENABLE_COVERAGE` | 启用覆盖率标志 | `true` |
| `TEST_TIMEOUT` | 测试超时时间 | `180000` |

## 测试配置

### test-run-settings.json

主要测试运行配置文件，包含：

- 并行执行设置
- 超时配置
- 数据收集配置
- 日志配置
- 测试数据配置
- 环境配置
- 报告配置

### ci-test-profile.json

CI环境专用配置文件，包含：

- CI特定执行模式
- 资源限制
- 数据管理
- 报告格式
- 覆盖率配置
- 性能监控

### xunit.runner.json

xUnit运行器配置：

```json
{
  "$schema": "https://xunit.net/schema/current/xunit.runner.schema.json",
  "parallelizeAssembly": false,
  "parallelizeTestCollections": true,
  "maxParallelThreads": 4
}
```

## 测试数据管理

### 测试数据同步

项目提供了跨平台的测试数据同步脚本：

- `Sync-TestData.bat` (Windows)
- `Sync-TestData.sh` (Linux/macOS)
- `Sync-TestData.ps1` (PowerShell)

### 测试数据结构

```
TestData/
├── attributes.xml
├── skills.xml
├── combat_parameters.xml
├── items.xml
├── crafting_pieces.xml
├── item_modifiers.xml
└── bone_body_types.xml
```

### TestDataHelper类

提供测试数据管理的辅助方法：

```csharp
// 获取测试数据路径
var path = TestDataHelper.GetTestDataPath("test.xml");

// 检查文件是否存在
var exists = TestDataHelper.TestDataFileExists("test.xml");

// 获取文件内容
var content = TestDataHelper.GetTestDataContent("test.xml");

// 复制测试数据文件
TestDataHelper.CopyTestDataFile("source.xml", "dest.xml");
```

## 测试执行最佳实践

### 1. 测试隔离

- 每个测试应该独立运行
- 使用`TestServiceProvider.Reset()`在测试间重置状态
- 避免测试间的依赖关系

### 2. 异步测试

- 使用`async/await`模式
- 设置适当的超时时间
- 处理异步操作的异常

### 3. 错误处理

- 使用适当的断言
- 提供有意义的错误消息
- 处理预期的异常情况

### 4. 性能考虑

- 避免在测试中进行不必要的I/O操作
- 使用内存数据库或模拟对象
- 设置合理的性能阈值

### 5. 并行执行

- 配置适当的并行度
- 避免共享状态的测试
- 使用线程安全的测试数据

## 故障排除

### 常见问题

#### 1. 依赖注入失败

**问题**: `TestServiceProvider`无法解析服务

**解决方案**:
```csharp
// 重置服务提供者
TestServiceProvider.Reset();

// 验证配置
var isValid = TestServiceProvider.ValidateConfiguration();
```

#### 2. 测试数据文件缺失

**问题**: 测试数据文件不存在

**解决方案**:
```bash
# 同步测试数据
./Sync-TestData.sh

# 验证文件存在
ls -la TestData/
```

#### 3. 并行测试冲突

**问题**: 并行测试导致资源冲突

**解决方案**:
```json
// 在test-run-settings.json中禁用特定测试的并行执行
"disableParallelizationFor": [
  "GitHubActionsEnvironmentTests",
  "CrossPlatformCompatibilityTests"
]
```

#### 4. 超时问题

**问题**: 测试执行超时

**解决方案**:
```json
// 在test-run-settings.json中调整超时时间
"timeout": {
  "default": 60000,
  "longRunning": 180000
}
```

#### 5. 权限问题

**问题**: 文件访问权限不足

**解决方案**:
```bash
# 设置适当的权限
chmod +x ./Sync-TestData.sh
chmod 755 TestData/
```

### 调试技巧

#### 1. 启用详细日志

```json
{
  "logging": {
    "level": "Debug",
    "enableConsoleOutput": true,
    "enableFileOutput": true
  }
}
```

#### 2. 使用断点调试

- 在IDE中设置断点
- 使用测试调试器
- 检查变量状态

#### 3. 查看测试输出

```bash
# 启用详细输出
dotnet test --logger "console;verbosity=detailed"

# 保存测试结果到文件
dotnet test --logger "trx;LogFileName=test-results.trx"
```

#### 4. 分析覆盖率报告

```bash
# 生成覆盖率报告
dotnet test --collect:"XPlat Code Coverage"

# 使用ReportGenerator生成HTML报告
dotnet reportgenerator -reports:coverage.xml -targetdir:coverage-report
```

## 测试报告

### 报告格式

测试套件支持多种报告格式：

- **JUnit XML** - CI系统集成
- **HTML** - 可视化报告
- **Console** - 命令行输出
- **GitHub Actions** - GitHub集成

### 报告内容

- 测试执行摘要
- 通过/失败/跳过的测试数量
- 执行时间统计
- 覆盖率统计
- 性能指标
- 错误详情

### 报告位置

```
TestResults/
├── test-results.xml
├── test-results.html
├── coverage.xml
├── coverage-report/
│   └── index.html
└── test-results.log
```

## 性能优化

### 1. 测试缓存

```json
{
  "optimizations": {
    "enableTestCaching": true,
    "enableAssemblyCaching": true,
    "enableTestDataCaching": false
  }
}
```

### 2. 并行执行优化

```json
{
  "parallelExecution": {
    "enabled": true,
    "maxParallelThreads": 4,
    "strategy": "test-assembly"
  }
}
```

### 3. 资源监控

```json
{
  "resources": {
    "monitoring": {
      "enableMemoryMonitoring": true,
      "enableCPUMonitoring": true,
      "samplingInterval": 5000
    }
  }
}
```

## 安全考虑

### 1. 测试数据安全

- 不在测试数据中包含敏感信息
- 使用模拟数据代替真实数据
- 清理测试产生的临时文件

### 2. 环境隔离

- 使用独立的测试环境
- 避免测试影响生产数据
- 限制测试的网络访问

### 3. 权限控制

- 最小权限原则
- 定期审计测试权限
- 监控测试执行

## 总结

本测试套件提供了全面的测试覆盖，确保Bannerlord Mod Editor UI的质量和稳定性。通过遵循本指南，您可以有效地执行测试、诊断问题并优化测试性能。

## 支持资源

- [xUnit文档](https://xunit.net/)
- [Moq文档](https://github.com/moq/moq4)
- [.NET测试文档](https://docs.microsoft.com/en-us/dotnet/core/testing/)
- [GitHub Actions文档](https://docs.github.com/en/actions)