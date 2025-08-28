# BannerlordModEditor-CLI 持续集成测试文档

## 文档概述

本文档详细说明了BannerlordModEditor-CLI项目的持续集成测试流程，包括自动化测试、质量监控、报告生成和失败处理机制。基于GitHub Actions平台，确保每次代码提交都经过全面的质量验证。

## 1. CI/CD流水线架构

### 1.1 流水线概览
```
代码提交 → 代码检查 → 构建验证 → 单元测试 → 集成测试 → 性能测试 → 安全扫描 → 质量报告 → 部署准备
```

### 1.2 触发条件
- **推送事件**: 推送到 `master`、`main`、`develop` 分支
- **拉取请求**: 针对主分支的PR
- **定时触发**: 每日凌晨2点执行完整测试套件
- **手动触发**: 管理员手动触发特殊测试

## 2. 测试流水线配置

### 2.1 主要流水线文件
- **`.github/workflows/comprehensive-test-suite.yml`**: 综合测试套件
- **`.github/workflows/dotnet-desktop.yml`**: 桌面应用构建
- **`.github/workflows/tui-integration-tests.yml`**: TUI集成测试

### 2.2 流水线作业详解

#### 2.2.1 单元测试作业
```yaml
unit-tests:
  runs-on: ubuntu-latest
  steps:
    - name: 检出代码
      uses: actions/checkout@v4
      
    - name: 设置 .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
        
    - name: 恢复依赖
      run: dotnet restore
      
    - name: 构建项目
      run: dotnet build --configuration Release --no-restore
      
    - name: 运行单元测试
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --no-build \
          --verbosity normal \
          --collect:"XPlat Code Coverage" \
          --results-directory TestResults \
          --logger "trx;LogFileName=unit_tests.trx"
```

#### 2.2.2 集成测试作业
```yaml
integration-tests:
  runs-on: ubuntu-latest
  needs: unit-tests
  steps:
    - name: 运行集成测试
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --no-build \
          --verbosity normal \
          --filter "Category=Integration" \
          --results-directory TestResults \
          --logger "trx;LogFileName=integration_tests.trx"
```

#### 2.2.3 性能测试作业
```yaml
performance-tests:
  runs-on: ubuntu-latest
  needs: unit-tests
  steps:
    - name: 运行性能测试
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --no-build \
          --verbosity normal \
          --filter "Category=Performance" \
          --results-directory TestResults \
          --logger "trx;LogFileName=performance_tests.trx"
```

## 3. 测试分类和执行策略

### 3.1 测试分类标记
```csharp
// 单元测试
[Fact]
[Trait("Category", "Unit")]

// 集成测试
[Fact]
[Trait("Category", "Integration")]

// 性能测试
[Fact]
[Trait("Category", "Performance")]

// 错误处理测试
[Fact]
[Trait("Category", "ErrorHandling")]

// 并发测试
[Fact]
[Trait("Category", "Concurrency")]

// 回归测试
[Fact]
[Trait("Category", "Regression")]

// 大型XML测试
[Fact]
[Trait("Category", "LargeXml")]

// 内存测试
[Fact]
[Trait("Category", "Memory")]
```

### 3.2 测试执行顺序
1. **单元测试** (必须通过)
2. **集成测试** (依赖单元测试)
3. **性能测试** (依赖单元测试)
4. **错误处理测试** (依赖单元测试)
5. **并发测试** (依赖单元测试)
6. **回归测试** (依赖单元测试)
7. **大型XML测试** (依赖单元测试)
8. **内存测试** (依赖单元测试)
9. **UI测试** (依赖单元测试)
10. **安全扫描** (依赖单元测试)

## 4. 测试报告和监控

### 4.1 测试结果收集
```yaml
- name: 上传测试结果
  uses: actions/upload-artifact@v4
  if: always()
  with:
    name: test-results
    path: |
      TestResults/
      *.trx
```

### 4.2 覆盖率报告生成
```yaml
- name: 生成覆盖率报告
  run: |
    dotnet tool install -g dotnet-reportgenerator-globaltool
    if [ -f "TestResults/**/coverage.cobertura.xml" ]; then
      reportgenerator -reports:TestResults/**/coverage.cobertura.xml \
        -targetdir:TestResults/CoverageReport \
        -reporttypes:Html
    fi
```

### 4.3 测试汇总报告
```yaml
- name: 生成测试汇总报告
  run: |
    echo "# 测试执行汇总报告" > test-summary.md
    echo "## 测试执行时间: $(date)" >> test-summary.md
    echo "## 测试覆盖率" >> test-summary.md
    echo "- 单元测试覆盖率: $(cat coverage-summary.txt)" >> test-summary.md
    echo "- 集成测试通过率: $(cat integration-summary.txt)" >> test-summary.md
```

## 5. 质量门禁和失败处理

### 5.1 质量门禁标准
- **单元测试通过率**: 100%
- **代码覆盖率**: ≥95%
- **集成测试通过率**: ≥98%
- **性能测试通过率**: ≥95%
- **安全漏洞**: 0个高危漏洞

### 5.2 失败处理机制
```yaml
# 测试失败时继续执行其他作业
if: always()

# 关键作业失败阻止合并
if: failure()
```

### 5.3 通知机制
```yaml
- name: 发送通知
  uses: 8398a7/action-slack@v3
  with:
    status: ${{ job.status }}
    fields: repo,message,commit,author,action,eventName,ref,workflow
    text: '测试执行完成'
  env:
    SLACK_WEBHOOK_URL: ${{ secrets.SLACK_WEBHOOK }}
```

## 6. 性能基准和监控

### 6.1 性能基准定义
```csharp
public class PerformanceBaselines
{
    public const int MaxXmlProcessingTimeMs = 5000;
    public const int MaxMemoryUsageMB = 512;
    public const int MaxFileDiscoveryTimeMs = 3000;
    public const int MaxSerializationTimeMs = 1000;
    public const int MaxDeserializationTimeMs = 1000;
}
```

### 6.2 性能监控实现
```csharp
[Fact]
[Trait("Category", "Performance")]
public void XmlProcessing_ShouldMeetPerformanceBaselines()
{
    // Arrange
    var largeXml = File.ReadAllText("large_test_data.xml");
    var stopwatch = Stopwatch.StartNew();
    
    // Act
    var result = XmlProcessor.Process(largeXml);
    stopwatch.Stop();
    
    // Assert
    Assert.True(stopwatch.ElapsedMilliseconds < PerformanceBaselines.MaxXmlProcessingTimeMs,
        $"XML处理时间超出基准: {stopwatch.ElapsedMilliseconds}ms");
    Assert.True(GC.GetTotalMemory(false) < PerformanceBaselines.MaxMemoryUsageMB * 1024 * 1024,
        $"内存使用超出基准: {GC.GetTotalMemory(false) / 1024 / 1024}MB");
}
```

## 7. 测试环境管理

### 7.1 环境配置
```yaml
env:
  DOTNET_VERSION: '9.0.x'
  TEST_PROJECT_PATH: 'BannerlordModEditor.Common.Tests'
  UI_TEST_PROJECT_PATH: 'BannerlordModEditor.UI.Tests'
  NODE_VERSION: '18'
```

### 7.2 依赖管理
```yaml
- name: 缓存依赖
  uses: actions/cache@v3
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
    restore-keys: |
      ${{ runner.os }}-nuget-
```

### 7.3 测试数据管理
```yaml
- name: 下载测试数据
  run: |
    if [ ! -d "TestData" ]; then
      git clone https://github.com/your-repo/test-data.git TestData
    fi
    cd TestData && git pull origin main
```

## 8. 安全扫描和合规性

### 8.1 安全扫描配置
```yaml
- name: 运行安全扫描
  run: |
    dotnet list package --vulnerable --include-transitive
    dotnet nuget locals all --clear
    
- name: 运行代码分析
  run: |
    dotnet build --configuration Release --no-restore
```

### 8.2 合规性检查
```yaml
- name: 检查许可证合规性
  run: |
    dotnet list package --format json | dotnet tool run license-check
    
- name: 检查代码规范
  run: |
    dotnet format --verify-no-changes
```

## 9. 部署准备和发布

### 9.1 发布条件
- 所有测试通过
- 覆盖率达到要求
- 安全扫描通过
- 性能基准达标

### 9.2 部署配置
```yaml
deployment-prep:
  runs-on: ubuntu-latest
  needs: [unit-tests, integration-tests, performance-tests, security-scan]
  if: github.ref == 'refs/heads/master' || github.ref == 'refs/heads/main'
  
  steps:
    - name: 构建发布版本
      run: |
        dotnet publish BannerlordModEditor.UI \
          --configuration Release \
          --framework net9.0 \
          --self-contained true \
          --runtime linux-x64 \
          -o publish/linux-x64
```

## 10. 测试数据和环境配置

### 10.1 测试数据版本控制
```bash
# 测试数据目录结构
TestData/
├── Credits/
│   ├── Credits.xml
│   ├── CreditsLegalPC.xml
│   └── CreditsExternalPartnersPC.xml
├── AchievementData/
│   └── gog_achievement_data.xml
├── Layouts/
│   ├── animations_layout.xml
│   └── physics_materials_layout.xml
└── Languages/
    ├── std_functions.xml
    └── std_TaleWorlds_Core.xml
```

### 10.2 环境变量配置
```yaml
- name: 设置环境变量
  run: |
    echo "TEST_DATA_PATH=${{ github.workspace }}/TestData" >> $GITHUB_ENV
    echo "LOG_LEVEL=Debug" >> $GITHUB_ENV
    echo "ENABLE_PERFORMANCE_LOGGING=true" >> $GITHUB_ENV
```

## 11. 测试执行监控和报告

### 11.1 实时监控
```yaml
- name: 监控测试执行
  run: |
    echo "开始监控测试执行..."
    timeout 30m dotnet test --logger "console;verbosity=detailed" || \
      echo "测试执行超时或失败"
```

### 11.2 详细报告
```yaml
- name: 生成详细报告
  run: |
    # 生成测试执行时间报告
    echo "## 测试执行时间统计" > timing-report.md
    find TestResults -name "*.trx" -exec grep -h "duration" {} \; >> timing-report.md
    
    # 生成失败测试报告
    echo "## 失败测试详情" > failure-report.md
    find TestResults -name "*.trx" -exec grep -h "failed" {} \; >> failure-report.md
```

## 12. 故障排除和调试

### 12.1 调试模式
```yaml
- name: 启用调试模式
  run: |
    echo "启用详细日志..."
    dotnet test --logger "console;verbosity=detailed" --diag diagnostic.log
```

### 12.2 故障排除脚本
```bash
#!/bin/bash
# 故障排除脚本
echo "=== 系统信息 ==="
uname -a
echo "=== .NET 版本 ==="
dotnet --version
echo "=== 测试数据检查 ==="
ls -la TestData/
echo "=== 内存使用情况 ==="
free -h
echo "=== 磁盘使用情况 ==="
df -h
```

## 13. 持续改进和优化

### 13.1 性能优化
```yaml
- name: 性能优化检查
  run: |
    echo "检查测试执行性能..."
    time dotnet test --configuration Release
    
    echo "优化建议："
    echo "1. 考虑并行执行测试"
    echo "2. 使用测试数据缓存"
    echo "3. 优化大型XML文件处理"
```

### 13.2 测试优化
```yaml
- name: 测试优化建议
  run: |
    echo "分析测试覆盖率..."
    dotnet test --collect:"XPlat Code Coverage"
    
    echo "优化建议："
    echo "1. 增加边界条件测试"
    echo "2. 改进错误处理测试"
    echo "3. 加强并发测试"
```

## 14. 文档和知识管理

### 14.1 自动文档生成
```yaml
- name: 生成测试文档
  run: |
    echo "生成测试文档..."
    dotnet tool install -g docfx
    docfx docs/docfx.json
```

### 14.2 知识库更新
```yaml
- name: 更新知识库
  run: |
    echo "更新测试知识库..."
    # 提取测试结果和经验教训
    echo "## 本次测试执行总结" > test-knowledge.md
    echo "- 执行时间: $(date)" >> test-knowledge.md
    echo "- 通过率: $(cat pass-rate.txt)" >> test-knowledge.md
    echo "- 关键发现: $(cat key-findings.txt)" >> test-knowledge.md
```

## 15. 最佳实践和规范

### 15.1 CI/CD最佳实践
1. **快速反馈**: 保持测试执行时间在可接受范围内
2. **可靠性**: 消除随机失败和不稳定测试
3. **可维护性**: 保持流水线配置的清晰和可维护
4. **安全性**: 确保敏感信息的安全存储

### 15.2 测试最佳实践
1. **独立性**: 测试之间相互独立
2. **可重复性**: 测试结果可重复
3. **自动化**: 完全自动化，无需人工干预
4. **全面性**: 覆盖所有关键功能

---

本文档提供了BannerlordModEditor-CLI项目完整的持续集成测试配置和流程，确保项目的质量和稳定性。所有配置都基于最佳实践，支持长期维护和扩展。