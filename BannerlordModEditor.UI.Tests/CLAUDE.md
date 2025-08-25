# CLAUDE.md - BannerlordModEditor.UI.Tests 项目指南

## 项目概述

这是 BannerlordModEditor 项目的 UI 层测试项目，使用 xUnit 测试框架和 Avalonia UI 框架。本项目专门测试 Avalonia UI 组件、ViewModel 交互、编辑器功能以及用户界面行为。

## 测试架构

### 技术栈
- **测试框架**: xUnit 2.5
- **UI框架**: Avalonia UI 11.3
- **MVVM框架**: CommunityToolkit.Mvvm 8.2
- **模拟框架**: Moq (用于模拟依赖)
- **断言库**: xUnit.Assert

### 测试组织结构
```
BannerlordModEditor.UI.Tests/
├── Helpers/                    # 测试辅助工具
│   ├── TestServiceProvider.cs  # 依赖注入服务提供者
│   ├── MockViewModels.cs       # 模拟ViewModel
│   ├── MockEditorFactory.cs    # 模拟编辑器工厂
│   └── TestExtensions.cs       # 测试扩展方法
├── ViewModels/                 # ViewModel测试
│   ├── Editors/               # 编辑器ViewModel测试
│   └── MainWindowViewModelTests.cs
├── Integration/               # 集成测试
│   └── EditorIntegrationTests.cs
├── Services/                  # 服务层测试
│   ├── ValidationServiceTests.cs
│   └── DataBindingServiceTests.cs
├── BasicUITests.cs            # 基础UI交互测试
├── EditorManagerTests.cs      # 编辑器管理器测试
├── UIVisibilityTests.cs       # UI可见性测试
├── XmlLoadingTests.cs         # XML加载测试
└── SimplifiedMainWindowTests.cs # 简化主窗口测试
```

## 测试分类和优先级

### 🔴 高优先级 - 核心功能测试
1. **ViewModel测试** - 确保所有ViewModel正确实现MVVM模式
2. **编辑器功能测试** - 验证各种编辑器的核心功能
3. **服务层测试** - 验证ValidationService、DataBindingService等
4. **集成测试** - 验证组件间的交互

### 🟡 中优先级 - UI交互测试
1. **基础UI测试** - 按钮点击、文本输入等基本交互
2. **UI可见性测试** - 确保UI元素正确显示和隐藏
3. **XML加载测试** - 验证XML文件的加载和保存功能

### 🟢 低优先级 - 边缘情况测试
1. **错误处理测试** - 验证异常情况的处理
2. **性能测试** - 验证UI响应性能
3. **兼容性测试** - 验证不同环境下的兼容性

## 测试最佳实践

### 1. 测试命名约定
```csharp
// 好的测试命名
public void SkillEditorViewModel_AddSkillCommand_ShouldAddNewSkill()
public void MainWindow_Integration_ShouldSelectSkillEditor()
public void ValidationService_ValidateProperty_WithValidValue_ShouldReturnValid()

// 避免的测试命名
public void Test1()
public void SkillEditorTest()
public void CheckValidation()
```

### 2. 测试结构模式 (AAA模式)
```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange - 准备测试数据和环境
    var viewModel = new SkillEditorViewModel();
    var initialCount = viewModel.Skills.Count;
    
    // Act - 执行要测试的操作
    viewModel.AddSkillCommand.Execute(null);
    
    // Assert - 验证结果
    Assert.Equal(initialCount + 1, viewModel.Skills.Count);
}
```

### 3. 依赖注入测试
```csharp
[Fact]
public void Service_ShouldWork_WithDependencyInjection()
{
    // Arrange - 使用TestServiceProvider
    var service = TestServiceProvider.GetService<ValidationService>();
    var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
    
    // Act & Assert
    var result = service.Validate(viewModel);
    Assert.True(result.IsValid);
}
```

### 4. 异步测试处理
```csharp
[Fact]
public async Task AsyncOperation_ShouldCompleteSuccessfully()
{
    // Arrange
    var editor = new SkillEditorViewModel();
    
    // Act - 使用await而不是.Wait()
    await editor.LoadXmlFileAsync("test.xml");
    
    // Assert
    Assert.False(string.IsNullOrEmpty(editor.FilePath));
}
```

### 5. Mock和模拟对象
```csharp
[Fact]
public void Method_ShouldHandleExternalDependency()
{
    // Arrange - 创建模拟对象
    var mockService = new Mock<ILogService>();
    var viewModel = new TestViewModel(mockService.Object);
    
    // Act
    viewModel.TestMethod();
    
    // Assert - 验证交互
    mockService.Verify(x => x.LogInfo(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
}
```

## 特殊测试要求

### Avalonia UI测试注意事项
1. **线程安全**: Avalonia UI测试必须在主线程上运行
2. **生命周期**: 正确处理UI组件的创建和销毁
3. **异步操作**: UI相关的异步操作需要特别小心

### 编辑器测试模式
1. **使用TestServiceProvider**: 通过依赖注入获取编辑器实例
2. **避免直接实例化**: 使用service provider而不是new操作符
3. **状态验证**: 验证编辑器的状态变化和属性通知

```csharp
[Fact]
public void Editor_ShouldNotifyPropertyChanges()
{
    // Arrange
    var editor = TestServiceProvider.GetService<SkillEditorViewModel>();
    var propertyChanged = false;
    editor.PropertyChanged += (s, e) => propertyChanged = true;
    
    // Act
    editor.FilePath = "test.xml";
    
    // Assert
    Assert.True(propertyChanged);
}
```

### 集成测试指南
1. **端到端测试**: 测试完整的用户操作流程
2. **服务组合**: 验证多个服务的协同工作
3. **状态管理**: 确保应用状态在不同操作间正确维护

## 测试数据管理

### 测试数据文件
- 位置: `../BannerlordModEditor.Common.Tests/TestData/`
- 使用真实的XML配置文件进行测试
- 避免在测试代码中硬编码大量测试数据

### 测试数据清理
```csharp
public class TestClass : IDisposable
{
    private readonly string _testFilePath = "test_output.xml";
    
    [Fact]
    public void Test_WithFileCreation()
    {
        // 测试代码...
        File.WriteAllText(_testFilePath, "test data");
    }
    
    public void Dispose()
    {
        // 清理测试文件
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}
```

## 调试和故障排除

### 常见问题
1. **依赖注入失败**: 确保所有服务都在TestServiceProvider中正确注册
2. **线程问题**: 使用`[STAThread]`属性处理UI线程相关测试
3. **异步死锁**: 避免在测试中使用`.Wait()`和`.Result`

### 调试技巧
1. **使用Console.WriteLine**: 在测试中输出调试信息
2. **条件断点**: 在特定条件下设置断点
3. **测试隔离**: 确保测试之间相互独立

```csharp
[Fact]
public void Debug_Example()
{
    // 输出调试信息
    Console.WriteLine($"Starting test: {nameof(Debug_Example)}");
    
    var result = MethodUnderTest();
    
    // 输出中间结果
    Console.WriteLine($"Result: {result}");
    
    Assert.NotNull(result);
}
```

## 性能考虑

### 测试性能优化
1. **共享测试数据**: 使用`IClassFixture`共享昂贵的测试资源
2. **避免IO操作**: 减少文件系统操作，使用内存数据
3. **并行测试**: 使用`[Collection]`属性控制测试并行执行

```csharp
[Collection("Test Collection")]
public class PerformanceTests : IDisposable
{
    private readonly TestFixture _fixture;
    
    public PerformanceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }
    
    // 测试方法...
}
```

## 代码质量要求

### 测试覆盖率目标
- **目标覆盖率**: 95%+
- **关键路径**: 100%覆盖率
- **UI组件**: 90%+覆盖率

### 代码质量检查
- 所有测试必须通过静态代码分析
- 禁止使用`[Obsolete]`的API
- 遵循C#编码规范和命名约定

## 持续集成要求

### 测试运行要求
- 所有测试必须在CI环境中通过
- 测试超时时间: 30秒
- 内存使用限制: 1GB

### 测试报告
- 生成代码覆盖率报告
- 输出详细的测试结果日志
- 失败测试必须提供清晰的错误信息

## 特殊说明

### 中文注释要求
- 所有测试方法和类必须使用中文注释
- 注释应说明测试的目的和预期结果
- 复杂的业务逻辑需要详细的说明文档

### 测试维护
- 定期审查和更新测试用例
- 删除过时或重复的测试
- 保持测试代码的可读性和可维护性

## 相关资源

- [xUnit文档](https://xunit.net/)
- [Avalonia UI文档](https://avaloniaui.net/)
- [CommunityToolkit.Mvvm文档](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [Moq文档](https://github.com/moq/moq4)

---

**重要提示**: 本项目专注于UI层测试，确保所有用户界面交互、ViewModel行为和编辑器功能都能正常工作。测试质量直接影响最终用户体验，请严格按照上述指南编写和维护测试代码。