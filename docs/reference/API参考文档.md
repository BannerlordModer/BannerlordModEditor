# Bannerlord Mod Editor TUI - API参考

## 概述

本文档提供了Bannerlord Mod Editor TUI项目的完整API参考，包括所有公共类、方法、属性和事件的详细说明。

## 命名空间结构

### BannerlordModEditor.TUI.Services

提供TUI应用程序的核心服务功能。

#### FormatConversionService

负责文件格式转换的核心服务类。

```csharp
public class FormatConversionService : IFormatConversionService
{
    public FormatConversionService(IFileDiscoveryService fileDiscoveryService);
    
    // 转换方法
    public Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null);
    public Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath, ConversionOptions? options = null);
    
    // 检测方法
    public Task<FileFormatInfo> DetectFileFormatAsync(string filePath);
    
    // 验证方法
    public Task<ValidationResult> ValidateConversionAsync(string sourceFilePath, string targetFilePath, ConversionDirection direction);
}
```

**方法详细说明**：

##### ExcelToXmlAsync

将Excel文件转换为XML格式。

**参数**：
- `excelFilePath` (string): Excel文件路径
- `xmlFilePath` (string): XML文件路径
- `options` (ConversionOptions?): 转换选项，可为null

**返回值**：
- `Task<ConversionResult>`: 转换结果对象

**异常**：
- `ArgumentNullException`: 当excelFilePath或xmlFilePath为null时抛出
- `FileNotFoundException`: 当Excel文件不存在时抛出
- `UnauthorizedAccessException`: 当没有文件访问权限时抛出
- `IOException`: 当发生I/O错误时抛出
- `XmlException`: 当XML处理错误时抛出

**示例**：
```csharp
var service = new FormatConversionService(fileDiscoveryService);
var result = await service.ExcelToXmlAsync("data.xlsx", "output.xml");

if (result.Success)
{
    Console.WriteLine($"转换成功，处理了 {result.RecordsProcessed} 条记录");
}
else
{
    Console.WriteLine($"转换失败：{result.Message}");
}
```

##### XmlToExcelAsync

将XML文件转换为Excel格式。

**参数**：
- `xmlFilePath` (string): XML文件路径
- `excelFilePath` (string): Excel文件路径
- `options` (ConversionOptions?): 转换选项，可为null

**返回值**：
- `Task<ConversionResult>`: 转换结果对象

**异常**：
- `ArgumentNullException`: 当xmlFilePath或excelFilePath为null时抛出
- `FileNotFoundException`: 当XML文件不存在时抛出
- `UnauthorizedAccessException`: 当没有文件访问权限时抛出
- `IOException`: 当发生I/O错误时抛出
- `XmlException`: 当XML格式错误时抛出

##### DetectFileFormatAsync

检测文件的格式类型。

**参数**：
- `filePath` (string): 文件路径

**返回值**：
- `Task<FileFormatInfo>`: 文件格式信息对象

**异常**：
- `ArgumentNullException`: 当filePath为null时抛出
- `FileNotFoundException`: 当文件不存在时抛出

##### ValidateConversionAsync

验证转换结果的有效性。

**参数**：
- `sourceFilePath` (string): 源文件路径
- `targetFilePath` (string): 目标文件路径
- `direction` (ConversionDirection): 转换方向

**返回值**：
- `Task<ValidationResult>`: 验证结果对象

**异常**：
- `ArgumentNullException`: 当sourceFilePath或targetFilePath为null时抛出
- `FileNotFoundException`: 当源文件或目标文件不存在时抛出

### BannerlordModEditor.TUI.ViewModels

提供MVVM模式的视图模型实现。

#### MainViewModel

主窗口的视图模型，处理用户交互和业务逻辑。

```csharp
public class MainViewModel : ViewModelBase
{
    // 构造函数
    public MainViewModel(IFormatConversionService conversionService);
    
    // 属性
    public string SourceFilePath { get; set; }
    public string TargetFilePath { get; set; }
    public ConversionDirection ConversionDirection { get; set; }
    public FileFormatInfo? SourceFileInfo { get; set; }
    public bool IsBusy { get; set; }
    public string StatusMessage { get; set; }
    public ConversionOptions ConversionOptions { get; set; }
    
    // 命令
    public ICommand BrowseSourceFileCommand { get; }
    public ICommand BrowseTargetFileCommand { get; }
    public ICommand AnalyzeSourceFileCommand { get; }
    public ICommand ConvertCommand { get; }
    public ICommand ToggleDirectionCommand { get; }
    public ICommand ShowOptionsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ExitCommand { get; }
}
```

**属性详细说明**：

##### SourceFilePath

获取或设置源文件路径。

**类型**：`string`

**默认值**：`string.Empty`

**事件**：当值改变时触发`PropertyChanged`事件

##### TargetFilePath

获取或设置目标文件路径。

**类型**：`string`

**默认值**：`string.Empty`

**事件**：当值改变时触发`PropertyChanged`事件

##### ConversionDirection

获取或设置转换方向。

**类型**：`ConversionDirection`

**默认值**：`ConversionDirection.ExcelToXml`

**事件**：当值改变时触发`PropertyChanged`事件

##### SourceFileInfo

获取或设置源文件信息。

**类型**：`FileFormatInfo?`

**默认值**：`null`

**事件**：当值改变时触发`PropertyChanged`事件

##### IsBusy

获取或设置应用程序忙碌状态。

**类型**：`bool`

**默认值**：`false`

**事件**：当值改变时触发`PropertyChanged`事件`

##### StatusMessage

获取或设置状态消息。

**类型**：`string`

**默认值**：`"就绪"`

**事件**：当值改变时触发`PropertyChanged`事件

**命令详细说明**：

##### BrowseSourceFileCommand

浏览源文件命令。

**类型**：`ICommand`

**执行方法**：`BrowseSourceFileAsync()`

**可执行条件**：总是可执行，但在忙碌状态时会显示提示

##### BrowseTargetFileCommand

浏览目标文件命令。

**类型**：`ICommand`

**执行方法**：`BrowseTargetFileAsync()`

**可执行条件**：总是可执行，但在忙碌状态时会显示提示

##### AnalyzeSourceFileCommand

分析源文件命令。

**类型**：`ICommand`

**执行方法**：`AnalyzeSourceFileAsync()`

**可执行条件**：当`SourceFilePath`不为空且不忙碌时可执行

##### ConvertCommand

执行转换命令。

**类型**：`ICommand`

**执行方法**：`ConvertAsync()`

**可执行条件**：当`SourceFilePath`和`TargetFilePath`都不为空，`SourceFileInfo`不为null且不忙碌时可执行

##### ToggleDirectionCommand

切换转换方向命令。

**类型**：`ICommand`

**执行方法**：`ToggleDirection()`

**可执行条件**：总是可执行

##### ShowOptionsCommand

显示选项对话框命令。

**类型**：`ICommand`

**执行方法**：`ShowOptions()`

**可执行条件**：总是可执行

##### ClearCommand

清空表单命令。

**类型**：`ICommand`

**执行方法**：`Clear()`

**可执行条件**：当不忙碌时可执行

##### ExitCommand

退出应用程序命令。

**类型**：`ICommand`

**执行方法**：`Exit()`

**可执行条件**：总是可执行

### BannerlordModEditor.TUI.Views

提供TUI界面组件的实现。

#### MainWindow

主窗口类，继承自`Window`。

```csharp
public class MainWindow : Window
{
    // 构造函数
    public MainWindow();
    
    // 初始化方法
    private void InitializeComponent();
    
    // 事件处理方法
    private void OnKeyDown(KeyEventEventArgs args);
    private void SetupKeyboardShortcuts();
}
```

**方法详细说明**：

##### InitializeComponent

初始化主窗口的UI组件。

**功能**：
- 创建和配置所有UI控件
- 设置数据绑定
- 初始化事件处理器
- 设置键盘快捷键

##### OnKeyDown

处理键盘按键事件。

**参数**：
- `args` (KeyEventEventArgs): 按键事件参数

**功能**：
- 处理功能键（F1-F8）
- 处理ESC键
- 处理其他快捷键

##### SetupKeyboardShortcuts

设置键盘快捷键。

**功能**：
- 为所有命令设置快捷键
- 配置快捷键提示文本
- 注册键盘事件处理器

## 数据模型

### ConversionResult

转换结果数据模型。

```csharp
public class ConversionResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? OutputPath { get; set; }
    public int RecordsProcessed { get; set; }
    public TimeSpan Duration { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
}
```

**属性说明**：

- `Success`: 转换是否成功
- `Message`: 转换结果消息
- `OutputPath`: 输出文件路径
- `RecordsProcessed`: 处理的记录数量
- `Duration`: 转换耗时
- `Errors`: 错误信息列表
- `Warnings`: 警告信息列表

### FileFormatInfo

文件格式信息数据模型。

```csharp
public class FileFormatInfo
{
    public FileFormatType FormatType { get; set; }
    public bool IsSupported { get; set; }
    public string FormatDescription { get; set; }
    public string? RootElement { get; set; }
    public int RowCount { get; set; }
    public List<string> ColumnNames { get; set; }
}
```

**属性说明**：

- `FormatType`: 文件格式类型
- `IsSupported`: 是否支持该格式
- `FormatDescription`: 格式描述信息
- `RootElement`: XML根元素名称（仅XML文件）
- `RowCount`: 数据行数
- `ColumnNames`: 列名列表

### ValidationResult

验证结果数据模型。

```csharp
public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public List<ValidationError> Errors { get; set; }
    public List<ValidationWarning> Warnings { get; set; }
}
```

**属性说明**：

- `IsValid`: 验证是否通过
- `Message`: 验证结果消息
- `Errors`: 错误列表
- `Warnings`: 警告列表

### ConversionOptions

转换选项配置。

```csharp
public class ConversionOptions
{
    public bool IncludeSchemaValidation { get; set; }
    public bool PreserveFormatting { get; set; }
    public bool CreateBackup { get; set; }
    public string? WorksheetName { get; set; }
    public string? RootElementName { get; set; }
    public string? RowElementName { get; set; }
    public bool FlattenNestedElements { get; set; }
    public string? NestedElementSeparator { get; set; }
}
```

**属性说明**：

- `IncludeSchemaValidation`: 是否包含架构验证
- `PreserveFormatting`: 是否保留格式
- `CreateBackup`: 是否创建备份
- `WorksheetName`: Excel工作表名称
- `RootElementName`: XML根元素名称
- `RowElementName`: XML行元素名称
- `FlattenNestedElements`: 是否扁平化嵌套元素
- `NestedElementSeparator`: 嵌套元素分隔符

## 枚举类型

### ConversionDirection

转换方向枚举。

```csharp
public enum ConversionDirection
{
    ExcelToXml,
    XmlToExcel
}
```

### FileFormatType

文件格式类型枚举。

```csharp
public enum FileFormatType
{
    Unknown,
    Excel,
    Xml,
    Csv,
    Json
}
```

### ValidationErrorType

验证错误类型枚举。

```csharp
public enum ValidationErrorType
{
    InvalidFormat,
    StructureMismatch,
    DataIntegrity,
    SchemaViolation
}
```

### ValidationWarningType

验证警告类型枚举。

```csharp
public enum ValidationWarningType
{
    EmptyField,
    FormatLoss,
    DataTruncation,
    PerformanceWarning
}
```

## 事件

### PropertyChangedEvent

属性变更事件，继承自`INotifyPropertyChanged`接口。

```csharp
public event PropertyChangedEventHandler? PropertyChanged;
```

**触发条件**：
- 当任何属性值改变时触发
- 用于数据绑定和UI更新

### CommandCanExecuteChanged

命令可执行状态变更事件。

```csharp
public event EventHandler? CanExecuteChanged;
```

**触发条件**：
- 当命令的可执行状态改变时触发
- 用于UI按钮的启用/禁用状态更新

## 扩展接口

### IFormatConversionService

格式转换服务接口。

```csharp
public interface IFormatConversionService
{
    Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null);
    Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath, ConversionOptions? options = null);
    Task<FileFormatInfo> DetectFileFormatAsync(string filePath);
    Task<ValidationResult> ValidateConversionAsync(string sourceFilePath, string targetFilePath, ConversionDirection direction);
}
```

### IFileDiscoveryService

文件发现服务接口。

```csharp
public interface IFileDiscoveryService
{
    Task<string[]> FindSupportedFilesAsync(string directory, string searchPattern);
    Task<bool> IsFileSupportedAsync(string filePath);
}
```

## 使用示例

### 基本转换示例

```csharp
// 创建服务实例
var conversionService = new FormatConversionService(fileDiscoveryService);

// 转换Excel到XML
var result = await conversionService.ExcelToXmlAsync(
    "input.xlsx", 
    "output.xml",
    new ConversionOptions 
    { 
        CreateBackup = true,
        IncludeSchemaValidation = true 
    });

if (result.Success)
{
    Console.WriteLine($"转换成功！处理了 {result.RecordsProcessed} 条记录");
    Console.WriteLine($"耗时：{result.Duration.TotalMilliseconds}ms");
}
else
{
    Console.WriteLine($"转换失败：{result.Message}");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"错误：{error}");
    }
}
```

### 文件格式检测示例

```csharp
// 检测文件格式
var formatInfo = await conversionService.DetectFileFormatAsync("unknown_file.dat");

if (formatInfo.IsSupported)
{
    Console.WriteLine($"文件格式：{formatInfo.FormatType}");
    Console.WriteLine($"描述：{formatInfo.FormatDescription}");
    Console.WriteLine($"数据行数：{formatInfo.RowCount}");
}
else
{
    Console.WriteLine($"不支持的文件格式：{formatInfo.FormatDescription}");
}
```

### 验证转换结果示例

```csharp
// 验证转换结果
var validationResult = await conversionService.ValidateConversionAsync(
    "input.xlsx",
    "output.xml",
    ConversionDirection.ExcelToXml);

if (validationResult.IsValid)
{
    Console.WriteLine("验证通过！");
}
else
{
    Console.WriteLine("验证失败：");
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"- {error.Message}");
    }
}
```

## 错误处理

### 常见错误类型

1. **ArgumentNullException**
   - 当必需参数为null时抛出
   - 错误消息：参数名 + "不能为空"

2. **FileNotFoundException**
   - 当文件不存在时抛出
   - 错误消息：文件路径 + "不存在"

3. **UnauthorizedAccessException**
   - 当没有文件访问权限时抛出
   - 错误消息：文件路径 + "访问权限不足"

4. **IOException**
   - 当发生I/O错误时抛出
   - 错误消息：具体的I/O错误信息

5. **XmlException**
   - 当XML处理错误时抛出
   - 错误消息：具体的XML错误信息

### 错误处理最佳实践

```csharp
try
{
    var result = await conversionService.ExcelToXmlAsync(
        sourcePath, 
        targetPath, 
        options);
    
    if (result.Success)
    {
        // 处理成功结果
    }
    else
    {
        // 处理失败结果
        LogErrors(result.Errors);
        LogWarnings(result.Warnings);
    }
}
catch (ArgumentNullException ex)
{
    // 处理参数错误
    Console.WriteLine($"参数错误：{ex.Message}");
}
catch (FileNotFoundException ex)
{
    // 处理文件不存在错误
    Console.WriteLine($"文件不存在：{ex.Message}");
}
catch (Exception ex)
{
    // 处理其他未预期的错误
    Console.WriteLine($"未预期的错误：{ex.Message}");
}
```

## 性能考虑

### 内存管理

- 使用`using`语句确保文件资源正确释放
- 避免在内存中同时加载大文件
- 及时清理临时文件和资源

### 异步操作

- 所有I/O操作都使用异步方法
- 避免在UI线程执行耗时操作
- 合理使用`Task.WhenAll`进行并行处理

### 大文件处理

- 对于大文件，考虑分批处理
- 使用流式处理避免内存溢出
- 提供进度反馈给用户

---

**API版本**：1.0.0  
**最后更新**：2025年8月22日  
**维护**：BannerlordModEditor开发团队