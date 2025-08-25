# TUI 集成测试说明

本文档说明了如何使用基于tmux的集成测试框架来测试Bannerlord Mod Editor TUI应用程序。

## 概述

集成测试框架使用tmux会话自动化来模拟真实用户与TUI应用程序的交互。这些测试验证：

- TUI应用程序的正确启动和关闭
- 用户界面元素的显示和响应
- 文件转换工作流程的完整性
- 错误处理和用户反馈机制
- 键盘输入和文件操作的正确性

## 测试架构

### 基础设施

- **TmuxIntegrationTestBase**: 提供tmux操作的基础功能
- **ProcessResult**: 封装系统命令执行结果
- **测试会话管理**: 自动创建和清理tmux会话

### 测试类别

1. **基础设施验证测试** (`TmuxInfrastructureValidationTests`)
   - 验证tmux可用性
   - 测试会话管理
   - 验证命令执行
   - 测试文件操作

2. **TUI工作流程测试** (`TuiBasicWorkflowIntegrationTests`)
   - 应用程序启动测试
   - 文件转换工作流程
   - 错误处理验证
   - 应用程序退出处理
   - 帮助信息显示

## 运行测试

### 本地运行

确保系统已安装tmux：

```bash
# 检查tmux是否安装
tmux -V

# 如果未安装，在Ubuntu/Debian上安装
sudo apt-get install tmux

# 在macOS上安装
brew install tmux
```

运行测试：

```bash
# 运行所有集成测试
dotnet test BannerlordModEditor.TUI.IntegrationTests

# 运行特定测试类
dotnet test BannerlordModEditor.TUI.IntegrationTests --filter "FullyQualifiedName~TmuxInfrastructureValidationTests"

# 运行特定测试方法
dotnet test BannerlordModEditor.TUI.IntegrationTests --filter "FullyQualifiedName~TmuxInfrastructureValidationTests.TmuxAvailability_TmuxShouldBeAvailable"
```

### GitHub Actions

测试已配置为在以下环境中自动运行：

- **Linux (Ubuntu)**: 完整的UAT和集成测试
- **Windows**: 仅UAT测试（tmux不可用）
- **macOS**: 完整的UAT和集成测试

GitHub Actions会自动：
1. 安装tmux（Linux/macOS）
2. 构建解决方案
3. 运行UAT测试
4. 运行集成测试
5. 生成测试报告

## 编写新测试

### 创建新的集成测试类

```csharp
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using BannerlordModEditor.TUI.IntegrationTests.Common;

namespace BannerlordModEditor.TUI.IntegrationTests.Features
{
    public class MyNewIntegrationTests : TmuxIntegrationTestBase
    {
        public MyNewIntegrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task MyNewTestScenario_ShouldWorkCorrectly()
        {
            // Given - 准备测试环境
            await CreateTmuxSessionAsync();
            
            // When - 执行测试操作
            await ExecuteTmuxCommandAsync("some-command");
            
            // Then - 验证结果
            var output = await CaptureTmuxOutputAsync(10);
            output.Should().Contain("expected-output");
        }
    }
}
```

### 可用的辅助方法

#### tmux会话管理
- `CreateTmuxSessionAsync()`: 创建新的tmux会话
- `IsTmuxSessionActiveAsync()`: 检查会话是否活跃
- `ExecuteTmuxCommandAsync(command, waitForCompletion)`: 在tmux中执行命令

#### 输入模拟
- `SendKeySequenceAsync(keys)`: 发送键盘按键序列
- `SendFilePathAsync(filePath)`: 发送文件路径输入

#### 输出验证
- `CaptureTmuxOutputAsync(lineCount)`: 捕获tmux会话输出
- `WaitForTmuxOutputAsync(expectedText, timeoutSeconds)`: 等待特定文本出现

#### 文件操作
- `CreateTestExcelFile(fileName, content)`: 创建测试Excel文件
- `VerifyOutputFile(filePath, shouldExist)`: 验证输出文件

#### 进程执行
- `ExecuteCommandAsync(fileName, arguments)`: 执行系统命令
- `GetTuiAppPath()`: 获取TUI应用程序路径

## 测试最佳实践

### 1. 测试隔离
- 每个测试使用唯一的tmux会话名称
- 测试完成后自动清理临时文件和会话
- 避免测试之间的相互影响

### 2. 错误处理
- 使用适当的超时设置
- 捕获和记录详细的错误信息
- 提供有意义的断言消息

### 3. 性能考虑
- 避免过长的等待时间
- 使用合理的超时值
- 最小化文件I/O操作

### 4. 可读性
- 使用BDD风格的测试命名
- 提供清晰的测试文档
- 使用描述性的断言

## 故障排除

### 常见问题

1. **tmux不可用**
   - 确保系统已安装tmux
   - 检查tmux版本兼容性
   - 验证PATH环境变量

2. **TUI应用程序路径错误**
   - 确保项目已正确构建
   - 检查GetTuiAppPath()方法的路径解析
   - 验证应用程序文件存在

3. **tmux会话创建失败**
   - 检查系统资源限制
   - 验证tmux配置
   - 确保没有会话名称冲突

4. **输出捕获失败**
   - 增加等待时间
   - 检查tmux权限设置
   - 验证会话活跃状态

### 调试技巧

1. **启用详细输出**
   ```bash
   dotnet test --verbosity normal
   ```

2. **手动测试tmux命令**
   ```bash
   tmux new-session -d -s test_session
   tmux send-keys -t test_session "echo hello" Enter
   tmux capture-pane -t test_session -p
   tmux kill-session -t test_session
   ```

3. **检查tmux会话状态**
   ```bash
   tmux list-sessions
   ```

## 扩展框架

### 添加新的tmux操作

在`TmuxIntegrationTestBase`类中添加新的辅助方法：

```csharp
protected async Task<bool> WaitForApplicationReadyAsync(int timeoutSeconds = 10)
{
    // 实现应用程序就绪状态检查
}

protected async Task SendComplexKeyCombinationAsync(string keyCombo)
{
    // 实现复杂按键组合发送
}
```

### 支持新的测试场景

创建专门的测试类来处理特定的用户场景：

```csharp
public class AdvancedFileConversionTests : TmuxIntegrationTestBase
{
    // 测试复杂的文件转换场景
}

public class UserInterfaceNavigationTests : TmuxIntegrationTestBase
{
    // 测试界面导航和交互
}
```

## 贡献指南

1. 遵循现有的测试命名约定
2. 确保新测试在所有支持的环境中运行
3. 提供充分的测试文档
4. 更新相关的README文档
5. 确保GitHub Actions配置正确

## 参考资源

- [tmux官方文档](https://github.com/tmux/tmux/wiki)
- [xUnit测试框架](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)
- [Shouldly](https://shouldly.readthedocs.io/)