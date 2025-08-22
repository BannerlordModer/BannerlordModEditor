# API规范和接口设计

## API设计原则

### 1. 一致性
- 所有API遵循一致的命名约定
- 参数类型和返回值类型保持一致
- 错误处理机制统一

### 2. 可扩展性
- 支持插件化扩展
- 接口设计考虑未来需求
- 向后兼容性保证

### 3. 类型安全
- 泛型约束确保类型安全
- 明确的接口定义
- 强类型的参数和返回值

### 4. 性能
- 异步操作支持
- 内存使用优化
- 批量处理支持

## 核心API接口

### XML处理接口

#### IXmlLoader<T>
```csharp
public interface IXmlLoader<T>
{
    /// <summary>
    /// 从文件路径加载XML数据
    /// </summary>
    /// <param name="filePath">XML文件路径</param>
    /// <returns>反序列化的对象</returns>
    T Load(string filePath);
    
    /// <summary>
    /// 异步从文件路径加载XML数据
    /// </summary>
    /// <param name="filePath">XML文件路径</param>
    /// <returns>反序列化对象的异步任务</returns>
    Task<T> LoadAsync(string filePath);
    
    /// <summary>
    /// 从XML字符串反序列化对象
    /// </summary>
    /// <param name="xml">XML字符串</param>
    /// <returns>反序列化的对象</returns>
    T Deserialize(string xml);
    
    /// <summary>
    /// 将对象序列化为XML字符串
    /// </summary>
    /// <param name="obj">要序列化的对象</param>
    /// <returns>XML字符串</returns>
    string Serialize(T obj);
    
    /// <summary>
    /// 保存对象到XML文件
    /// </summary>
    /// <param name="obj">要保存的对象</param>
    /// <param name="filePath">目标文件路径</param>
    void Save(T obj, string filePath);
    
    /// <summary>
    /// 异步保存对象到XML文件
    /// </summary>
    /// <param name="obj">要保存的对象</param>
    /// <param name="filePath">目标文件路径</param>
    /// <returns>异步任务</returns>
    Task SaveAsync(T obj, string filePath);
}
```

#### IXmlValidator
```csharp
public interface IXmlValidator
{
    /// <summary>
    /// 验证两个XML字符串的结构相等性
    /// </summary>
    /// <param name="xml1">第一个XML字符串</param>
    /// <param name="xml2">第二个XML字符串</param>
    /// <returns>是否结构相等</returns>
    bool AreStructurallyEqual(string xml1, string xml2);
    
    /// <summary>
    /// 比较两个XML字符串的结构差异
    /// </summary>
    /// <param name="xml1">第一个XML字符串</param>
    /// <param name="xml2">第二个XML字符串</param>
    /// <returns>结构比较结果</returns>
    XmlStructureComparisonResult CompareXmlStructure(string xml1, string xml2);
    
    /// <summary>
    /// 验证XML格式的有效性
    /// </summary>
    /// <param name="xml">XML字符串</param>
    /// <returns>验证结果</returns>
    ValidationResult Validate(string xml);
}
```

### DO/DTO映射接口

#### IMapper<TDO, TDTO>
```csharp
public interface IMapper<TDO, TDTO>
    where TDO : class, new()
    where TDTO : class, new()
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <param name="source">DO源对象</param>
    /// <returns>DTO目标对象</returns>
    TDTO ToDTO(TDO source);
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <param name="source">DTO源对象</param>
    /// <returns>DO目标对象</returns>
    TDO ToDO(TDTO source);
    
    /// <summary>
    /// 批量转换DO对象为DTO对象
    /// </summary>
    /// <param name="sources">DO源对象集合</param>
    /// <returns>DTO目标对象集合</returns>
    List<TDTO> ToDTO(IEnumerable<TDO> sources);
    
    /// <summary>
    /// 批量转换DTO对象为DO对象
    /// </summary>
    /// <param name="sources">DTO源对象集合</param>
    /// <returns>DO目标对象集合</returns>
    List<TDO> ToDO(IEnumerable<TDTO> sources);
}
```

#### IMapperFactory
```csharp
public interface IMapperFactory
{
    /// <summary>
    /// 获取指定类型的映射器
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <returns>映射器实例</returns>
    IMapper<TDO, TDTO> GetMapper<TDO, TDTO>()
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 注册自定义映射器
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="mapper">映射器实例</param>
    void RegisterMapper<TDO, TDTO>(IMapper<TDO, TDTO> mapper)
        where TDO : class, new()
        where TDTO : class, new();
}
```

### 文件发现接口

#### IFileDiscoveryService
```csharp
public interface IFileDiscoveryService
{
    /// <summary>
    /// 发现指定目录中的XML文件
    /// </summary>
    /// <param name="directory">搜索目录</param>
    /// <param name="searchPattern">搜索模式</param>
    /// <returns>发现的文件列表</returns>
    List<DiscoveredFile> DiscoverXmlFiles(string directory, string searchPattern = "*.xml");
    
    /// <summary>
    /// 检查文件是否已适配DO/DTO模式
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>是否已适配</returns>
    bool IsFileAdapted(string filePath);
    
    /// <summary>
    /// 获取文件的类型信息
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>文件类型信息</returns>
    TypeInfo GetFileInfo(string filePath);
    
    /// <summary>
    /// 监控目录变化
    /// </summary>
    /// <param name="directory">监控目录</param>
    /// <param name="callback">变化回调</param>
    /// <returns>监控令牌</returns>
    IDisposable WatchDirectory(string directory, Action<FileChangeInfo> callback);
}
```

### 类型转换接口

#### ITypeConverter
```csharp
public interface ITypeConverter<T>
{
    /// <summary>
    /// 从字符串转换为目标类型
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <param name="result">转换结果</param>
    /// <returns>转换是否成功</returns>
    bool TryConvertFromString(string value, out T result);
    
    /// <summary>
    /// 将目标类型转换为字符串
    /// </summary>
    /// <param name="value">目标类型值</param>
    /// <returns>字符串表示</returns>
    string ConvertToString(T value);
    
    /// <summary>
    /// 获取类型支持的所有格式
    /// </summary>
    /// <returns>支持的格式列表</returns>
    List<string> GetSupportedFormats();
}
```

#### ITypeConverterFactory
```csharp
public interface ITypeConverterFactory
{
    /// <summary>
    /// 获取指定类型的转换器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <returns>类型转换器</returns>
    ITypeConverter<T> GetConverter<T>();
    
    /// <summary>
    /// 注册自定义类型转换器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="converter">类型转换器</param>
    void RegisterConverter<T>(ITypeConverter<T> converter);
    
    /// <summary>
    /// 检查是否支持指定类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <returns>是否支持</returns>
    bool Supports<T>();
}
```

### 测试工具接口

#### ITestDataProvider
```csharp
public interface ITestDataProvider
{
    /// <summary>
    /// 获取测试XML文件路径
    /// </summary>
    /// <param name="testName">测试名称</param>
    /// <returns>测试文件路径</returns>
    string GetTestXmlPath(string testName);
    
    /// <summary>
    /// 获取测试XML内容
    /// </summary>
    /// <param name="testName">测试名称</param>
    /// <returns>XML内容</returns>
    string GetTestXmlContent(string testName);
    
    /// <summary>
    /// 获取期望的测试结果
    /// </summary>
    /// <param name="testName">测试名称</param>
    /// <returns>期望结果</returns>
    TestExpectedResult GetExpectedResult(string testName);
    
    /// <summary>
    /// 获取所有测试数据
    /// </summary>
    /// <returns>测试数据集合</returns>
    List<TestData> GetAllTestData();
}
```

#### ITestRunner
```csharp
public interface ITestRunner
{
    /// <summary>
    /// 运行单个测试
    /// </summary>
    /// <param name="testName">测试名称</param>
    /// <returns>测试结果</returns>
    TestResult RunTest(string testName);
    
    /// <summary>
    /// 运行多个测试
    /// </summary>
    /// <param name="testNames">测试名称集合</param>
    /// <returns>测试结果集合</returns>
    List<TestResult> RunTests(IEnumerable<string> testNames);
    
    /// <summary>
    /// 运行所有测试
    /// </summary>
    /// <returns>测试结果集合</returns>
    List<TestResult> RunAllTests();
    
    /// <summary>
    /// 获取测试统计信息
    /// </summary>
    /// <returns>统计信息</returns>
    TestStatistics GetStatistics();
}
```

## 数据模型

### DiscoveredFile
```csharp
public class DiscoveredFile
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsAdapted { get; set; }
    public TypeInfo TypeInfo { get; set; } = new TypeInfo();
}
```

### TypeInfo
```csharp
public class TypeInfo
{
    public string DOType { get; set; } = string.Empty;
    public string DTOType { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public string XmlRoot { get; set; } = string.Empty;
    public bool IsSupported { get; set; }
}
```

### XmlStructureComparisonResult
```csharp
public class XmlStructureComparisonResult
{
    public bool IsStructurallyEqual { get; set; }
    public int NodeCountDifference { get; set; }
    public int AttributeCountDifference { get; set; }
    public List<string> AttributeValueDifferences { get; set; } = new List<string>();
    public List<string> TextDifferences { get; set; } = new List<string>();
}
```

### ValidationResult
```csharp
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public List<string> Warnings { get; set; } = new List<string>();
    public List<string> Infos { get; set; } = new List<string>();
}
```

### TestExpectedResult
```csharp
public class TestExpectedResult
{
    public bool ShouldPass { get; set; }
    public XmlStructureComparisonResult ExpectedComparison { get; set; } = new XmlStructureComparisonResult();
    public Dictionary<string, object> ExpectedProperties { get; set; } = new Dictionary<string, object>();
}
```

### TestResult
```csharp
public class TestResult
{
    public string TestName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public TimeSpan Duration { get; set; }
    public XmlStructureComparisonResult? ComparisonResult { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
}
```

### TestStatistics
```csharp
public class TestStatistics
{
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Skipped { get; set; }
    public double PassRate => Total > 0 ? (double)Passed / Total * 100 : 0;
    public TimeSpan TotalDuration { get; set; }
    public List<TestResult> FailedTests { get; set; } = new List<TestResult>();
}
```

## 事件和通知

### INotificationService
```csharp
public interface INotificationService
{
    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="type">通知类型</param>
    void SendNotification(string message, NotificationType type);
    
    /// <summary>
    /// 注册通知处理器
    /// </summary>
    /// <param name="handler">通知处理器</param>
    /// <returns>处理器ID</returns>
    string RegisterHandler(Action<Notification> handler);
    
    /// <summary>
    /// 取消注册通知处理器
    /// </summary>
    /// <param name="handlerId">处理器ID</param>
    void UnregisterHandler(string handlerId);
}
```

### FileChangeInfo
```csharp
public class FileChangeInfo
{
    public string FilePath { get; set; } = string.Empty;
    public FileChangeType ChangeType { get; set; }
    public DateTime Timestamp { get; set; }
    public string? OldPath { get; set; }
}
```

## 枚举类型

### NotificationType
```csharp
public enum NotificationType
{
    Info,
    Warning,
    Error,
    Success
}
```

### FileChangeType
```csharp
public enum FileChangeType
{
    Created,
    Modified,
    Deleted,
    Renamed
}
```

## 扩展点

### IPlugin
```csharp
public interface IPlugin
{
    /// <summary>
    /// 插件名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 插件版本
    /// </summary>
    Version Version { get; }
    
    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// 插件作者
    /// </summary>
    string Author { get; }
    
    /// <summary>
    /// 初始化插件
    /// </summary>
    /// <param name="services">服务容器</param>
    void Initialize(IServiceProvider services);
    
    /// <summary>
    /// 安装插件
    /// </summary>
    void Install();
    
    /// <summary>
    /// 卸载插件
    /// </summary>
    void Uninstall();
}
```

### ICustomXmlProcessor
```csharp
public interface ICustomXmlProcessor
{
    /// <summary>
    /// 支持的文件扩展名
    /// </summary>
    IEnumerable<string> SupportedExtensions { get; }
    
    /// <summary>
    /// 检查是否支持指定文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>是否支持</returns>
    bool SupportsFile(string filePath);
    
    /// <summary>
    /// 处理XML文件
    /// </summary>
    /// <param name="xml">XML内容</param>
    /// <returns>处理后的XML内容</returns>
    string Process(string xml);
}