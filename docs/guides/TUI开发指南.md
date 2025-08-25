# Bannerlord Mod Editor TUI - 开发指南

## 项目概述

Bannerlord Mod Editor TUI 是一个基于终端用户界面的工具，用于《骑马与砍杀2：霸主》模组开发者的XML配置文件转换。本文档为开发者提供项目的技术细节、架构设计和开发指南。

## 项目架构

### 整体架构

```
BannerlordModEditor-TUI/
├── BannerlordModEditor.Common/           # 核心业务逻辑层
│   ├── Models/                          # 数据模型
│   │   ├── DO/                         # 领域对象
│   │   ├── DTO/                        # 数据传输对象
│   │   └── Data/                       # 原始数据模型
│   ├── Services/                       # 业务服务
│   └── Loaders/                        # 数据加载器
├── BannerlordModEditor.TUI/               # TUI表现层
│   ├── ViewModels/                     # MVVM视图模型
│   ├── Views/                          # TUI视图
│   └── Services/                       # TUI服务
├── BannerlordModEditor.TUI.Tests/         # TUI单元测试
├── BannerlordModEditor.Common.Tests/     # Common层测试
└── docs/                               # 项目文档
```

### 核心组件

#### 1. FormatConversionService
负责Excel和XML文件格式转换的核心服务。

**主要功能**：
- Excel转XML转换
- XML转Excel转换
- 文件格式检测
- 转换结果验证
- 错误处理和异常捕获

**关键方法**：
```csharp
public async Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null)
public async Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath, ConversionOptions? options = null)
public async Task<FileFormatInfo> DetectFileFormatAsync(string filePath)
public async Task<ValidationResult> ValidateConversionAsync(string sourceFilePath, string targetFilePath, ConversionDirection direction)
```

#### 2. MainViewModel
主视图模型，实现MVVM模式，处理用户交互和业务逻辑。

**主要功能**：
- 文件路径管理
- 转换方向控制
- 命令执行
- 状态管理
- 错误处理

**关键属性**：
```csharp
public string SourceFilePath { get; set; }
public string TargetFilePath { get; set; }
public ConversionDirection ConversionDirection { get; set; }
public FileFormatInfo? SourceFileInfo { get; set; }
public bool IsBusy { get; set; }
public string StatusMessage { get; set; }
```

#### 3. MainWindow
主窗口类，基于Terminal.Gui框架构建TUI界面。

**主要功能**：
- 界面布局管理
- 控件初始化
- 事件处理
- 键盘快捷键

## 技术栈

### 核心技术

- **.NET 9.0**：最新的.NET平台，提供高性能和现代化特性
- **Terminal.Gui**：跨平台TUI框架，支持丰富的终端界面组件
- **CommunityToolkit.Mvvm**：MVVM模式工具包，简化数据绑定和命令处理
- **xUnit**：单元测试框架，提供全面的测试支持
- **Moq**：模拟框架，用于单元测试中的依赖注入

### 数据处理

- **ClosedXML**：Excel文件处理库，支持.xlsx和.xls格式
- **System.Xml.Linq**：XML处理库，提供LINQ to XML功能
- **System.Text.Json**：JSON序列化和反序列化

## 开发环境设置

### 前置要求

- .NET 9.0 SDK
- Visual Studio Code 或 Visual Studio 2022
- Git

### 项目设置

1. **克隆项目**：
```bash
git clone <repository-url>
cd BannerlordModEditor-TUI-Development
```

2. **还原依赖**：
```bash
dotnet restore
```

3. **编译项目**：
```bash
dotnet build
```

4. **运行测试**：
```bash
dotnet test
```

## 编码规范

### C# 编码规范

1. **命名约定**：
   - 类名：PascalCase（`MainViewModel`）
   - 方法名：PascalCase（`ConvertFileAsync`）
   - 属性名：PascalCase（`SourceFilePath`）
   - 变量名：camelCase（`sourceFilePath`）
   - 常量名：PascalCase（`MaxFileSize`）

2. **异步方法**：
   - 所有异步方法应以`Async`结尾
   - 使用`async/await`模式
   - 避免`async void`，使用`async Task`代替

3. **异常处理**：
   - 使用特定的异常类型
   - 提供有意义的错误信息
   - 记录异常日志

### 文档规范

1. **XML文档注释**：
```csharp
/// <summary>
/// 将Excel文件转换为XML格式
/// </summary>
/// <param name="excelFilePath">Excel文件路径</param>
/// <param name="xmlFilePath">XML文件路径</param>
/// <param name="options">转换选项</param>
/// <returns>转换结果</returns>
public async Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null)
```

2. **方法注释**：
   - 描述方法的功能
   - 说明参数的含义
   - 说明返回值的含义
   - 列出可能的异常

## 测试策略

### 单元测试

项目使用xUnit进行单元测试，覆盖以下方面：

1. **服务层测试**：
   - FormatConversionService的各种转换场景
   - 错误处理和异常情况
   - 边界条件测试

2. **视图模型测试**：
   - MainViewModel的属性变化
   - 命令执行逻辑
   - 状态管理

3. **集成测试**：
   - 完整的转换流程测试
   - 文件I/O操作测试
   - 用户界面交互测试

### 测试示例

```csharp
[Fact]
public async Task ExcelToXmlAsync_WithValidExcel_ShouldSucceed()
{
    // Arrange
    var excelFilePath = CreateTestExcelFile();
    var xmlFilePath = Path.Combine(_tempDir, "test.xml");

    try
    {
        // Act
        var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

        // Assert
        Assert.True(result.Success);
        Assert.True(File.Exists(xmlFilePath));
        Assert.True(result.RecordsProcessed > 0);
    }
    finally
    {
        // Cleanup
        if (File.Exists(excelFilePath))
        {
            File.Delete(excelFilePath);
        }
        if (File.Exists(xmlFilePath))
        {
            File.Delete(xmlFilePath);
        }
    }
}
```

## 错误处理

### 异常处理策略

1. **输入验证**：
   - 检查参数是否为null或空
   - 验证文件路径格式
   - 验证文件是否存在

2. **文件操作**：
   - 处理文件访问权限问题
   - 处理文件被占用的情况
   - 处理磁盘空间不足的情况

3. **数据格式**：
   - 验证Excel文件格式
   - 验证XML文件格式
   - 处理数据转换错误

### 错误处理示例

```csharp
try
{
    if (string.IsNullOrWhiteSpace(excelFilePath))
    {
        result.Success = false;
        result.Message = "Excel文件路径不能为空";
        result.Errors.Add("Excel文件路径不能为空");
        return result;
    }

    if (!File.Exists(excelFilePath))
    {
        result.Success = false;
        result.Message = $"Excel文件不存在: {excelFilePath}";
        result.Errors.Add($"Excel文件不存在: {excelFilePath}");
        return result;
    }

    // 执行转换逻辑
}
catch (UnauthorizedAccessException ex)
{
    result.Success = false;
    result.Message = $"文件访问权限不足: {ex.Message}";
    result.Errors.Add($"文件访问权限不足: {ex.Message}");
}
catch (IOException ex)
{
    result.Success = false;
    result.Message = $"文件I/O错误: {ex.Message}";
    result.Errors.Add($"文件I/O错误: {ex.Message}");
}
catch (Exception ex)
{
    result.Success = false;
    result.Message = $"转换失败: {ex.Message}";
    result.Errors.Add($"转换失败: {ex.Message}");
}
```

## 性能优化

### 内存管理

1. **大文件处理**：
   - 使用流式处理避免内存溢出
   - 分批处理大量数据
   - 及时释放资源

2. **异步操作**：
   - 使用async/await避免阻塞
   - 合理使用Task并行处理
   - 避免不必要的异步等待

### 性能监控

```csharp
public async Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null)
{
    var startTime = DateTime.UtcNow;
    var result = new ConversionResult();
    
    try
    {
        // 执行转换逻辑
        
        result.Duration = DateTime.UtcNow - startTime;
        return result;
    }
    catch (Exception ex)
    {
        result.Duration = DateTime.UtcNow - startTime;
        // 错误处理
        return result;
    }
}
```

## 扩展开发

### 添加新的文件格式支持

1. **在FileFormatType中添加新的格式**：
```csharp
public enum FileFormatType
{
    Unknown,
    Excel,
    Xml,
    Csv,    // 新增格式
    Json    // 新增格式
}
```

2. **在FormatConversionService中添加转换方法**：
```csharp
public async Task<ConversionResult> CsvToXmlAsync(string csvFilePath, string xmlFilePath, ConversionOptions? options = null)
{
    // 实现CSV转XML逻辑
}
```

3. **更新文件检测逻辑**：
```csharp
switch (extension)
{
    case ".xlsx":
    case ".xls":
        result.FormatType = FileFormatType.Excel;
        break;
    case ".xml":
        result.FormatType = FileFormatType.Xml;
        break;
    case ".csv":
        result.FormatType = FileFormatType.Csv;
        break;
}
```

### 添加新的UI组件

1. **创建新的View**：
```csharp
public class BatchConversionView : View
{
    public BatchConversionView()
    {
        // 初始化UI组件
    }
}
```

2. **创建对应的ViewModel**：
```csharp
public class BatchConversionViewModel : ViewModelBase
{
    // 实现批量转换逻辑
}
```

3. **集成到主窗口**：
```csharp
private void ShowBatchConversionView()
{
    var batchView = new BatchConversionView();
    Application.Run(batchView);
}
```

## 部署和发布

### 构建配置

项目支持多种构建配置：

1. **Debug**：开发调试版本
2. **Release**：发布版本
3. **SelfContained**：自包含版本

### 发布命令

```bash
# 发布为自包含应用
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
dotnet publish -c Release -r osx-x64 --self-contained true

# 发布为框架依赖应用
dotnet publish -c Release -r win-x64 --self-contained false
dotnet publish -c Release -r linux-x64 --self-contained false
dotnet publish -c Release -r osx-x64 --self-contained false
```

### 打包和分发

1. **创建安装包**：
   - Windows：使用Inno Setup或WiX
   - Linux：使用AppImage或deb包
   - macOS：使用dmg包

2. **版本管理**：
   - 使用Git标签标记版本
   - 自动化构建和发布流程
   - 版本号管理

## 贡献指南

### 代码贡献流程

1. **Fork项目仓库**
2. **创建功能分支**：
```bash
git checkout -b feature/new-feature
```

3. **开发并测试**：
```bash
dotnet build
dotnet test
```

4. **提交更改**：
```bash
git add .
git commit -m "Add new feature"
```

5. **推送到分支**：
```bash
git push origin feature/new-feature
```

6. **创建Pull Request**

### 代码审查标准

1. **代码质量**：
   - 遵循编码规范
   - 添加适当的注释
   - 确保代码可读性

2. **测试覆盖**：
   - 新功能必须包含单元测试
   - 测试覆盖率应达到80%以上
   - 集成测试验证功能完整性

3. **文档更新**：
   - 更新相关文档
   - 添加API文档注释
   - 更新用户指南

## 常见问题

### 开发问题

#### 1. 编译错误

**解决方案**：
- 检查.NET版本兼容性
- 确认所有依赖包已安装
- 清理并重新编译项目

#### 2. 测试失败

**解决方案**：
- 检查测试环境配置
- 确认测试数据文件存在
- 查看详细的错误信息

#### 3. UI显示问题

**解决方案**：
- 检查终端兼容性
- 确认终端编码设置
- 调整终端窗口大小

### 调试技巧

1. **日志记录**：
```csharp
Console.WriteLine($"Debug: Processing file {filePath}");
```

2. **断点调试**：
   - 在Visual Studio中设置断点
   - 使用调试器查看变量值
   - 单步执行代码

3. **性能分析**：
   - 使用BenchmarkDotNet进行性能测试
   - 分析内存使用情况
   - 优化热点代码

## 参考资料

### 技术文档

- [.NET 9.0 文档](https://docs.microsoft.com/dotnet/)
- [Terminal.Gui 文档](https://github.com/gui-cs/Terminal.Gui)
- [ClosedXML 文档](https://closedxml.readthedocs.io/)
- [xUnit 文档](https://xunit.net/)

### 相关工具

- **Visual Studio Code**：轻量级代码编辑器
- **Visual Studio 2022**：功能完整的IDE
- **Git**：版本控制工具
- **Postman**：API测试工具

---

**最后更新**：2025年8月22日  
**维护**：BannerlordModEditor开发团队