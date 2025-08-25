# XML适配状态检查工具测试套件

这是一个为XML适配状态检查工具提供的完整测试套件，包含单元测试、集成测试和性能测试。

## 快速开始

### 前置条件
- .NET 9.0 SDK
- Git
- （可选）Visual Studio 2022 或其他支持.NET开发的IDE

### 克隆和构建
```bash
# 克隆项目
git clone <repository-url>
cd BannerlordModEditor-XML-Adaptation/scripts/XmlAdaptationChecker

# 还原依赖
dotnet restore

# 构建项目
dotnet build
```

## 运行测试

### 使用脚本运行（推荐）

#### Linux/macOS
```bash
# 运行所有测试
./run-tests.sh

# 只运行单元测试
./run-tests.sh -u

# 只运行集成测试
./run-tests.sh -i

# 只运行性能测试
./run-tests.sh -p

# 生成覆盖率报告
./run-tests.sh -c

# 清理测试文件
./run-tests.sh --clean

# 显示帮助
./run-tests.sh -h
```

#### Windows
```cmd
# 运行所有测试
run-tests.bat

# 只运行单元测试
run-tests.bat -u

# 只运行集成测试
run-tests.bat -i

# 只运行性能测试
run-tests.bat -p

# 生成覆盖率报告
run-tests.bat -c

# 清理测试文件
run-tests.bat --clean

# 显示帮助
run-tests.bat -h
```

### 使用dotnet命令运行

#### 运行所有测试
```bash
dotnet test
```

#### 运行特定测试类别
```bash
# 单元测试
dotnet test --filter "TestCategory!=Integration&TestCategory!=Performance"

# 集成测试
dotnet test --filter "TestCategory=Integration"

# 性能测试
dotnet test --filter "TestCategory=Performance"
```

#### 运行特定测试方法
```bash
dotnet test --filter "TestMethodName"
```

#### 生成覆盖率报告
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 测试结构

```
Tests/
├── Core/                          # 核心业务逻辑测试
│   └── XmlAdaptationCheckerTests.cs
├── Services/                      # 服务层测试
│   └── OutputFormatServiceTests.cs
├── Validators/                    # 验证器测试
│   └── ConfigurationValidatorTests.cs
├── Integration/                   # 集成测试
│   ├── CompleteCheckFlowIntegrationTests.cs
│   └── OutputFormatServiceIntegrationTests.cs
├── Performance/                   # 性能测试
│   └── PerformanceTests.cs
├── Mocks/                         # 测试数据和模拟对象
│   └── TestDataFactory.cs
├── AdaptationCheckerCLITests.cs   # CLI测试
└── AdaptationCheckerConfigurationTests.cs # 配置测试
```

## 测试覆盖范围

### 核心功能
- ✅ XML文件适配状态检查
- ✅ 多种输出格式支持（控制台、Markdown、CSV、JSON）
- ✅ 配置验证和管理
- ✅ 命令行界面
- ✅ 并行处理支持
- ✅ 复杂度分析
- ✅ 错误处理

### 性能指标
- ✅ 大文件处理性能（< 5秒）
- ✅ 多文件处理性能（< 10秒/100文件）
- ✅ 内存使用控制（< 100MB）
- ✅ 并发访问处理（< 30秒/10任务）
- ✅ 输出生成速度（< 1秒）

### 集成测试
- ✅ 端到端流程测试
- ✅ 文件系统交互
- ✅ 多种输出格式验证
- ✅ 错误场景处理
- ✅ 边界条件测试

## 测试数据和模拟对象

### TestDataFactory
提供测试数据和模拟对象的工厂类：

```csharp
// 创建模拟服务
var mockFileDiscoveryService = TestDataFactory.CreateMockFileDiscoveryService();
var mockValidator = TestDataFactory.CreateMockConfigurationValidator();

// 创建测试配置
var config = TestDataFactory.CreateValidConfiguration();

// 创建测试结果
var result = TestDataFactory.CreateTestResult(totalFiles: 10, adaptedFiles: 7);
```

### 测试文件系统
```csharp
// 创建测试文件系统
var fileSystem = TestDataFactory.CreateTestFileSystem();
fileSystem.CreateDirectory("/test");
fileSystem.CreateFile("/test/file.xml", "<root>test</root>");
```

## 配置选项

### xUnit配置
项目包含 `xunit.runner.json` 配置文件，支持：
- 并行测试执行
- 详细诊断消息
- 长运行测试超时设置

### 性能阈值
性能测试包含以下阈值检查：
- 大文件处理：5秒
- 多文件处理：10秒/100文件
- 内存使用：100MB
- 输出生成：1秒
- 并发处理：30秒/10任务

## CI/CD集成

### GitHub Actions
项目包含完整的GitHub Actions工作流：

- **自动触发**：push、PR、手动触发
- **测试分类**：单元测试、集成测试、性能测试
- **代码质量**：格式检查、安全扫描、依赖检查
- **报告生成**：测试结果、覆盖率报告、性能基准

### 运行特定测试类型
```yaml
# GitHub Actions示例
- name: 运行单元测试
  run: |
    dotnet test \
      --filter "TestCategory!=Integration&TestCategory!=Performance" \
      --collect:"XPlat Code Coverage"
```

## 故障排除

### 常见问题

#### 测试失败
```bash
# 清理和重建
dotnet clean
dotnet restore
dotnet build

# 检查依赖
dotnet list package --vulnerable
dotnet list package --outdated
```

#### 权限问题
```bash
# Linux/macOS
chmod +x run-tests.sh

# Windows
# 确保有写入权限
icacls test-results /grant Everyone:F
```

#### 性能测试失败
```bash
# 检查系统资源
free -h
df -h

# 监控性能
dotnet-counters monitor
```

### 调试技巧

#### 详细日志
```bash
export DOTNET_LOGGING__LOGLEVEL__DEFAULT=Debug
dotnet test --logger "console;verbosity=detailed"
```

#### 调试特定测试
```bash
# 使用Visual Studio
# 在测试方法上设置断点，然后运行测试

# 使用VS Code
# 安装.NET调试扩展，设置断点，启动调试
```

## 贡献指南

### 添加新测试
1. 确定测试类型（单元/集成/性能）
2. 创建对应的测试类
3. 使用TestDataFactory创建测试数据
4. 编写测试方法
5. 验证测试覆盖新功能

### 测试命名约定
- 单元测试：`MethodName_ExpectedBehavior`
- 集成测试：`Scenario_ExpectedOutcome`
- 性能测试：`Performance_Metric_ExpectedThreshold`

### 测试数据管理
- 使用TestDataFactory创建测试数据
- 测试完成后自动清理
- 避免硬编码测试数据

## 报告和指标

### 测试结果位置
- 测试结果：`test-results/`
- 覆盖率报告：`test-results/coverage-report/`
- 性能结果：`test-results/benchmark/`

### 关键指标
- 测试通过率：目标100%
- 代码覆盖率：目标≥85%
- 性能阈值：所有测试必须通过
- 执行时间：单元测试<1分钟，集成测试<5分钟

## 最佳实践

### 编写测试
- 使用AAA模式（Arrange-Act-Assert）
- 测试正常情况和边界情况
- 验证错误处理
- 使用有意义的测试名称
- 保持测试独立和快速

### 维护测试
- 定期更新依赖包
- 清理过时的测试
- 更新性能阈值
- 监控测试覆盖率
- 优化测试执行时间

### 代码审查
- 新功能必须包含测试
- 测试覆盖率必须达标
- 性能测试必须通过
- 代码质量检查必须通过

## 支持

### 文档
- [详细测试覆盖文档](TEST_COVERAGE.md)
- [项目文档](../../docs/)
- [API文档](../../docs/api-spec.md)

### 问题报告
- 创建GitHub Issue
- 包含重现步骤
- 附加测试结果
- 提供系统信息

### 联系方式
- 创建GitHub Discussion
- 提交Pull Request
- 参与代码审查

---

**注意**：这是一个完整的测试套件，确保了XML适配状态检查工具的质量和可靠性。请定期运行测试以验证代码的健康状态。