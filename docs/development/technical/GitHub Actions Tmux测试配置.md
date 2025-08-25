# GitHub Actions Tmux集成测试配置

## 概述

本项目配置了完整的GitHub Actions自动化测试流程，专门用于TUI应用程序的Tmux集成测试。

## 工作流文件

### 1. `tui-integration-tests.yml` - TUI集成测试主要工作流

**触发条件：**
- 推送到 `main`, `master`, `feature/*` 分支
- 针对到 `main`, `master` 分支的Pull Request
- 手动触发 (`workflow_dispatch`)

**支持的测试环境：**
- Linux (Ubuntu Latest) - 完整测试
- macOS (macOS Latest) - 完整测试  
- Windows (Windows Latest) - 仅UAT测试

### 2. `comprehensive-test-suite.yml` - 全面测试套件

包含多种测试分类：
- 单元测试
- 集成测试
- 性能测试
- 错误处理测试
- 并发测试
- 回归测试
- 大型XML文件测试
- 内存测试
- UI测试

### 3. `dotnet-desktop.yml` - 主要CI/CD工作流

Windows环境的构建、测试和部署流程。

## Tmux测试特性

### 测试覆盖范围

**Tmux集成测试项目** (`BannerlordModEditor.TUI.TmuxTest`) 包含：

1. **基础设施验证**
   - Tmux可用性检查
   - Tmux会话管理
   - Tmux命令执行
   - 文件操作验证
   - 进程执行验证

2. **TUI工作流测试**
   - 应用程序启动验证
   - 基本文件转换工作流
   - 错误处理和用户反馈
   - 应用程序退出处理
   - 帮助信息显示

**总计：11个专门测试**

### 环境配置

#### Linux环境
```yaml
- name: Install tmux
  run: |
    sudo apt-get update
    sudo apt-get install -y tmux
    
- name: Verify tmux installation
  run: |
    tmux -V
    tmux new-session -d -s test-session
    tmux send-keys -t test-session "echo 'Tmux test session created'" Enter
    tmux capture-pane -t test-session -p
    tmux kill-session -t test-session
```

#### macOS环境
```yaml
- name: Install tmux (macOS)
  run: |
    brew install tmux
```

#### Windows环境
```yaml
- name: Skip Tmux Integration Tests (Windows)
  run: |
    echo "Tmux Integration Tests require tmux, which is not available on Windows"
    echo "Tmux tests will run on Linux and macOS environments only"
```

## 测试矩阵策略

```yaml
strategy:
  matrix:
    dotnet-version: ['9.0.x']
    include:
      - dotnet-version: '9.0.x'
        display-name: '.NET 9.0'
```

## 测试结果和报告

### 测试结果上传
```yaml
- name: Upload Tmux Test Results
  if: always()
  uses: actions/upload-artifact@v4
  with:
    name: tmux-test-results-${{ runner.os }}
    path: |
      TestResults/
      *.trx
```

### 综合测试报告
自动生成包含以下信息的综合报告：
- 测试环境信息
- 各平台测试结果
- 测试覆盖率摘要
- 测试基础设施状态
- 关键功能验证状态

### PR评论集成
针对Pull Request自动添加测试结果评论。

## 验证脚本

项目包含本地验证脚本 `verify-tmux-tests.sh`：

```bash
#!/bin/bash
# 验证tmux测试环境配置
./verify-tmux-tests.sh
```

验证项目：
- ✅ tmux安装和功能
- ✅ .NET环境
- ✅ 项目文件存在
- ✅ 依赖恢复
- ✅ 项目构建
- ✅ 测试运行

## 测试统计

### 总体测试覆盖
- **测试项目总数**: 5个
- **总测试数量**: 219+个测试
- **Tmux集成测试**: 11个测试
- **UAT测试**: 30个测试
- **Common测试**: 58个测试
- **UI测试**: 76个测试
- **TUI测试**: 44个测试

### 测试框架
- **xUnit** - 主要测试框架
- **FluentAssertions** - 断言库
- **Shouldly** - 额外断言支持
- **Moq** - 模拟框架
- **coverlet** - 代码覆盖率

## 运行测试

### 本地运行
```bash
# 运行所有测试
dotnet test

# 运行特定Tmux测试
dotnet test BannerlordModEditor.TUI.TmuxTest

# 运行UAT测试
dotnet test BannerlordModEditor.TUI.UATTests
```

### 验证环境
```bash
# 运行环境验证脚本
./verify-tmux-tests.sh
```

## 注意事项

1. **跨平台兼容性**: Tmux测试仅在Linux和macOS上运行
2. **依赖管理**: 确保所有NuGet包版本兼容
3. **测试隔离**: 每个测试项目职责单一，便于维护
4. **错误处理**: 所有测试都包含适当的错误处理和清理逻辑
5. **性能考虑**: Tmux测试可能需要较长时间，建议并行运行

## 故障排除

### 常见问题
1. **tmux未安装**: 确保在Linux/macOS环境中正确安装tmux
2. **权限问题**: 检查文件权限和执行权限
3. **依赖冲突**: 检查NuGet包版本兼容性
4. **测试超时**: 某些Tmux操作可能需要调整超时时间

### 调试建议
1. 使用本地验证脚本检查环境配置
2. 检查GitHub Actions日志中的详细错误信息
3. 确保所有测试文件路径正确
4. 验证tmux会话管理功能正常

## 未来扩展

### 计划中的改进
1. **更多测试场景**: 添加复杂的TUI交互测试
2. **性能基准**: 建立Tmux测试性能基准
3. **并行测试**: 优化测试并行执行策略
4. **覆盖率报告**: 集成更详细的代码覆盖率分析

### 集成建议
1. **监控集成**: 集成测试结果监控和告警
2. **部署流水线**: 将测试结果与部署流程集成
3. **文档自动化**: 自动生成测试文档和报告

---

此配置确保了Bannerlord Mod Editor TUI应用程序的全面自动化测试覆盖，特别是Tmux集成测试的可靠性和稳定性。