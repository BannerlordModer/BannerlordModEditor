# 修复后的API规范文档

## API概述

本文档定义了BannerlordModEditor项目修复后的API规范，包括XML处理、测试工具、异步操作和错误处理等核心接口。这些API设计旨在解决当前测试失败问题，提供更好的可维护性和扩展性。

## 核心接口

### 1. XML处理接口

#### 1.1 IEnhancedXmlLoader接口

```yaml
openapi: 3.0.0
info:
  title: Enhanced XML Loader API
  version: 1.0.0
  description: 增强的XML加载器接口，提供统一的XML处理功能

paths:
  /xml/load:
    get:
      summary: 加载XML文件
      operationId: loadXml
      parameters:
        - name: filePath
          in: query
          required: true
          schema:
            type: string
          description: XML文件路径
        - name: options
          in: query
          required: false
          schema:
            $ref: '#/components/schemas/XmlLoadOptions'
          description: XML加载选项
      responses:
        '200':
          description: 成功加载XML文件
          content:
            application/json:
              schema:
                type: object
                properties:
                  success:
                    type: boolean
                  data:
                    type: object
                    description: 反序列化的对象
                  warnings:
                    type: array
                    items:
                      type: string
        '400':
          description: 请求参数错误
        '500':
          description: 内部服务器错误

  /xml/save:
    post:
      summary: 保存对象到XML
      operationId: saveXml
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              properties:
                data:
                  type: object
                  description: 要保存的对象
                filePath:
                  type: string
                  description: 保存路径
                options:
                  $ref: '#/components/schemas/XmlSaveOptions'
      responses:
        '200':
          description: 成功保存XML文件
          content:
            application/json:
              schema:
                type: object
                properties:
                  success:
                    type: boolean
                  filePath:
                    type: string
                  fileSize:
                    type: integer
        '400':
          description: 请求参数错误
        '500':
          description: 内部服务器错误

  /xml/validate:
    post:
      summary: 验证XML对象
      operationId: validateXml
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              properties:
                data:
                  type: object
                  description: 要验证的对象
                options:
                  $ref: '#/components/schemas/XmlValidationOptions'
      responses:
        '200':
          description: 验证结果
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/XmlValidationResult'
        '400':
          description: 请求参数错误
        '500':
          description: 内部服务器错误

  /xml/compare:
    post:
      summary: 比较两个XML对象的结构
      operationId: compareXml
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              properties:
                obj1:
                  type: object
                  description: 第一个对象
                obj2:
                  type: object
                  description: 第二个对象
                options:
                  $ref: '#/components/schemas/XmlComparisonOptions'
      responses:
        '200':
          description: 比较结果
          content:
            application/json:
              schema:
                type: object
                properties:
                  equal:
                    type: boolean
                  differences:
                    type: array
                    items:
                      type: string
        '400':
          description: 请求参数错误
        '500':
          description: 内部服务器错误

components:
  schemas:
    XmlLoadOptions:
      type: object
      properties:
        preserveWhitespace:
          type: boolean
          default: true
          description: 是否保留空白字符
        handleEmptyElements:
          type: boolean
          default: true
          description: 是否处理空元素
        normalizeLineEndings:
          type: boolean
          default: true
          description: 是否标准化行结束符
        encodingHandling:
          $ref: '#/components/schemas/XmlEncodingHandling'
          default: PreserveOriginal

    XmlSaveOptions:
      type: object
      properties:
        preserveOriginalFormat:
          type: boolean
          default: true
          description: 是否保留原始格式
        handleEmptyElements:
          type: boolean
          default: true
          description: 是否处理空元素
        normalizeNamespaces:
          type: boolean
          default: true
          description: 是否标准化命名空间
        originalXml:
          type: string
          description: 原始XML内容

    XmlValidationOptions:
      type: object
      properties:
        validateSchema:
          type: boolean
          default: true
          description: 是否验证XML模式
        validateStructure:
          type: boolean
          default: true
          description: 是否验证XML结构
        checkRequiredElements:
          type: boolean
          default: true
          description: 是否检查必需元素
        checkEmptyElements:
          type: boolean
          default: true
          description: 是否检查空元素处理

    XmlValidationResult:
      type: object
      properties:
        isValid:
          type: boolean
          description: 验证是否通过
        errors:
          type: array
          items:
            type: string
          description: 错误信息列表
        warnings:
          type: array
          items:
            type: string
          description: 警告信息列表
        details:
          type: object
          description: 验证详细信息

    XmlComparisonOptions:
      type: object
      properties:
        mode:
          $ref: '#/components/schemas/ComparisonMode'
          default: Logical
        ignoreComments:
          type: boolean
          default: true
          description: 是否忽略注释
        ignoreWhitespace:
          type: boolean
          default: true
          description: 是否忽略空白字符
        ignoreAttributeOrder:
          type: boolean
          default: true
          description: 是否忽略属性顺序
        allowCaseInsensitiveBooleans:
          type: boolean
          default: true
          description: 是否允许布尔值大小写不敏感
        allowNumericTolerance:
          type: boolean
          default: true
          description: 是否允许数值容差
        numericTolerance:
          type: number
          default: 0.0001
          description: 数值容差

    ComparisonMode:
      type: string
      enum:
        - Strict
        - Logical
        - Loose
      default: Logical

    XmlEncodingHandling:
      type: string
      enum:
        - PreserveOriginal
        - ForceUTF8
        - AutoDetect
      default: PreserveOriginal
```

### 2. 测试工具接口

#### 2.1 ITestDataManager接口

```csharp
/// <summary>
/// 测试数据管理器接口
/// </summary>
public interface ITestDataManager
{
    /// <summary>
    /// 获取测试数据文件路径
    /// </summary>
    /// <param name="testDataName">测试数据名称</param>
    /// <returns>测试数据文件路径</returns>
    string GetTestDataPath(string testDataName);
    
    /// <summary>
    /// 加载测试数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="testDataName">测试数据名称</param>
    /// <returns>加载的数据对象</returns>
    T LoadTestData<T>(string testDataName) where T : class;
    
    /// <summary>
    /// 从XML文件加载测试数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="testDataName">测试数据名称</param>
    /// <returns>加载的数据对象</returns>
    Task<T> LoadTestDataFromXmlAsync<T>(string testDataName) where T : class;
    
    /// <summary>
    /// 生成测试数据
    /// </summary>
    /// <param name="templateName">模板名称</param>
    /// <param name="replacements">替换参数</param>
    /// <param name="outputPath">输出路径</param>
    void GenerateTestData(string templateName, Dictionary<string, string> replacements, string outputPath);
    
    /// <summary>
    /// 清理测试数据缓存
    /// </summary>
    void ClearCache();
    
    /// <summary>
    /// 获取所有可用的测试数据
    /// </summary>
    /// <returns>测试数据列表</returns>
    List<string> GetAvailableTestData();
    
    /// <summary>
    /// 验证测试数据完整性
    /// </summary>
    /// <param name="testDataName">测试数据名称</param>
    /// <returns>验证结果</returns>
    Task<TestDataValidationResult> ValidateTestDataAsync(string testDataName);
}

/// <summary>
/// 测试数据验证结果
/// </summary>
public class TestDataValidationResult
{
    /// <summary>
    /// 验证是否通过
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// 错误信息列表
    /// </summary>
    public List<string> Errors { get; set; } = new();
    
    /// <summary>
    /// 警告信息列表
    /// </summary>
    public List<string> Warnings { get; set; } = new();
    
    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime LastModified { get; set; }
    
    /// <summary>
    /// XML格式是否有效
    /// </summary>
    public bool IsXmlValid { get; set; }
    
    /// <summary>
    /// 验证耗时（毫秒）
    /// </summary>
    public long ValidationTimeMs { get; set; }
}
```

#### 2.2 IAsyncTestHelper接口

```csharp
/// <summary>
/// 异步测试辅助工具接口
/// </summary>
public interface IAsyncTestHelper
{
    /// <summary>
    /// 执行异步操作并验证在指定时间内完成
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="operation">异步操作</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="operationName">操作名称</param>
    /// <returns>操作结果</returns>
    Task<T> ExecuteWithTimeoutAsync<T>(
        Func<Task<T>> operation,
        TimeSpan timeout,
        string operationName = "AsyncOperation");
    
    /// <summary>
    /// 验证异步操作抛出指定异常
    /// </summary>
    /// <typeparam name="T">异常类型</typeparam>
    /// <param name="action">异步操作</param>
    /// <param name="because">失败原因</param>
    /// <returns>任务</returns>
    Task ShouldThrowAsync<T>(
        Func<Task> action,
        string because = "") where T : Exception;
    
    /// <summary>
    /// 验证异步操作不抛出异常
    /// </summary>
    /// <param name="action">异步操作</param>
    /// <param name="because">失败原因</param>
    /// <returns>任务</returns>
    Task ShouldNotThrowAsync(
        Func<Task> action,
        string because = "");
    
    /// <summary>
    /// 执行多个异步操作并收集结果
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="operations">操作列表</param>
    /// <param name="maxConcurrency">最大并发数</param>
    /// <returns>操作结果列表</returns>
    Task<List<AsyncOperationResult<T>>> ExecuteAsyncOperations<T>(
        IEnumerable<Func<Task<T>>> operations,
        int maxConcurrency = 4);
    
    /// <summary>
    /// 创建重试策略
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="operation">异步操作</param>
    /// <param name="maxRetries">最大重试次数</param>
    /// <param name="retryDelay">重试延迟</param>
    /// <returns>操作结果</returns>
    Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> operation,
        int maxRetries = 3,
        TimeSpan? retryDelay = null);
}

/// <summary>
/// 异步操作结果
/// </summary>
public class AsyncOperationResult<T>
{
    /// <summary>
    /// 操作是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 操作结果
    /// </summary>
    public T? Result { get; set; }
    
    /// <summary>
    /// 异常信息
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// 执行时间（毫秒）
    /// </summary>
    public long ExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 重试次数
    /// </summary>
    public int RetryCount { get; set; }
}
```

### 3. 错误处理接口

#### 3.1 ITestExceptionHandler接口

```csharp
/// <summary>
/// 测试异常处理器接口
/// </summary>
public interface ITestExceptionHandler
{
    /// <summary>
    /// 执行带有异常处理的操作
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="operation">操作</param>
    /// <param name="operationName">操作名称</param>
    /// <returns>操作结果</returns>
    Task<T> ExecuteWithExceptionHandlingAsync<T>(
        Func<Task<T>> operation,
        string operationName);
    
    /// <summary>
    /// 处理测试失败
    /// </summary>
    /// <param name="ex">异常</param>
    /// <param name="testName">测试名称</param>
    /// <param name="context">上下文信息</param>
    void HandleTestFailure(Exception ex, string testName, object? context = null);
    
    /// <summary>
    /// 记录警告信息
    /// </summary>
    /// <param name="message">警告消息</param>
    /// <param name="context">上下文信息</param>
    void LogWarning(string message, object? context = null);
    
    /// <summary>
    /// 记录错误信息
    /// </summary>
    /// <param name="ex">异常</param>
    /// <param name="context">上下文信息</param>
    void LogError(Exception ex, object? context = null);
    
    /// <summary>
    /// 创建详细的错误报告
    /// </summary>
    /// <param name="ex">异常</param>
    /// <param name="testName">测试名称</param>
    /// <returns>错误报告</returns>
    TestErrorReport CreateErrorReport(Exception ex, string testName);
}

/// <summary>
/// 测试错误报告
/// </summary>
public class TestErrorReport
{
    /// <summary>
    /// 报告ID
    /// </summary>
    public string ReportId { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// 测试名称
    /// </summary>
    public string TestName { get; set; } = string.Empty;
    
    /// <summary>
    /// 异常类型
    /// </summary>
    public string ExceptionType { get; set; } = string.Empty;
    
    /// <summary>
    /// 异常消息
    /// </summary>
    public string ExceptionMessage { get; set; } = string.Empty;
    
    /// <summary>
    /// 堆栈跟踪
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
    
    /// <summary>
    /// 错误发生时间
    /// </summary>
    public DateTime ErrorTime { get; set; } = DateTime.Now;
    
    /// <summary>
    /// 环境信息
    /// </summary>
    public TestEnvironmentInfo Environment { get; set; } = new();
    
    /// <summary>
    /// 错误分类
    /// </summary>
    public ErrorCategory Category { get; set; } = ErrorCategory.Unknown;
    
    /// <summary>
    /// 严重程度
    /// </summary>
    public ErrorSeverity Severity { get; set; } = ErrorSeverity.Medium;
    
    /// <summary>
    /// 建议的解决方案
    /// </summary>
    public List<string> SuggestedSolutions { get; set; } = new();
    
    /// <summary>
    /// 相关文件
    /// </summary>
    public List<string> RelatedFiles { get; set; } = new();
}

/// <summary>
/// 测试环境信息
/// </summary>
public class TestEnvironmentInfo
{
    /// <summary>
    /// .NET版本
    /// </summary>
    public string DotNetVersion { get; set; } = string.Empty;
    
    /// <summary>
    /// 操作系统
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;
    
    /// <summary>
    /// 当前目录
    /// </summary>
    public string CurrentDirectory { get; set; } = string.Empty;
    
    /// <summary>
    /// 测试数据目录
    /// </summary>
    public string TestDataDirectory { get; set; } = string.Empty;
    
    /// <summary>
    /// 内存使用情况
    /// </summary>
    public MemoryInfo Memory { get; set; } = new();
    
    /// <summary>
    /// CPU使用情况
    /// </summary>
    public CpuInfo Cpu { get; set; } = new();
}

/// <summary>
/// 内存信息
/// </summary>
public class MemoryInfo
{
    /// <summary>
    /// 总内存（MB）
    /// </summary>
    public long TotalMemoryMB { get; set; }
    
    /// <summary>
    /// 已用内存（MB）
    /// </summary>
    public long UsedMemoryMB { get; set; }
    
    /// <summary>
    /// 可用内存（MB）
    /// </summary>
    public long AvailableMemoryMB { get; set; }
    
    /// <summary>
    /// 内存使用率
    /// </summary>
    public double MemoryUsagePercent { get; set; }
}

/// <summary>
/// CPU信息
/// </summary>
public class CpuInfo
{
    /// <summary>
    /// CPU核心数
    /// </summary>
    public int CoreCount { get; set; }
    
    /// <summary>
    /// CPU使用率
    /// </summary>
    public double CpuUsagePercent { get; set; }
    
    /// <summary>
    /// CPU频率
    /// </summary>
    public double CpuFrequencyGHz { get; set; }
}

/// <summary>
/// 错误分类
/// </summary>
public enum ErrorCategory
{
    Unknown,
    XmlProcessing,
    FileIo,
    Serialization,
    Deserialization,
    Network,
    Configuration,
    Timeout,
    Validation,
    BusinessLogic
}

/// <summary>
/// 错误严重程度
/// </summary>
public enum ErrorSeverity
{
    Low,
    Medium,
    High,
    Critical
}
```

### 4. 断言增强接口

#### 4.1 IAssertionHelper接口

```csharp
/// <summary>
/// 增强断言辅助工具接口
/// </summary>
public interface IAssertionHelper
{
    /// <summary>
    /// 验证对象具有有效的XML结构
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要验证的对象</param>
    /// <param name="because">失败原因</param>
    void ShouldHaveValidXmlStructure<T>(T obj, string because = "");
    
    /// <summary>
    /// 验证对象可以被序列化和反序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要验证的对象</param>
    /// <param name="because">失败原因</param>
    void ShouldBeSerializable<T>(T obj, string because = "");
    
    /// <summary>
    /// 验证对象正确处理空元素
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要验证的对象</param>
    /// <param name="because">失败原因</param>
    void ShouldHandleEmptyElements<T>(T obj, string because = "");
    
    /// <summary>
    /// 验证XML往返序列化后结构相等
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要验证的对象</param>
    /// <param name="options">比较选项</param>
    /// <param name="because">失败原因</param>
    void ShouldSurviveRoundTripSerialization<T>(
        T obj, 
        XmlComparisonOptions? options = null, 
        string because = "");
    
    /// <summary>
    /// 验证XML文件加载时间在预期范围内
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="filePath">文件路径</param>
    /// <param name="maxLoadTimeMs">最大加载时间（毫秒）</param>
    /// <param name="because">失败原因</param>
    Task ShouldLoadWithinTimeAsync<T>(
        string filePath, 
        int maxLoadTimeMs, 
        string because = "") where T : class;
    
    /// <summary>
    /// 验证XML文件大小在预期范围内
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="maxSizeBytes">最大文件大小（字节）</param>
    /// <param name="because">失败原因</param>
    void ShouldHaveFileSizeWithinLimit(
        string filePath, 
        long maxSizeBytes, 
        string because = "");
    
    /// <summary>
    /// 验证XML文档结构符合预期
    /// </summary>
    /// <param name="xml">XML内容</param>
    /// <param name="expectedStructure">预期结构</param>
    /// <param name="because">失败原因</param>
    void ShouldHaveExpectedStructure(
        string xml, 
        XmlStructure expectedStructure, 
        string because = "");
}

/// <summary>
/// XML结构定义
/// </summary>
public class XmlStructure
{
    /// <summary>
    /// 根元素名称
    /// </summary>
    public string RootElement { get; set; } = string.Empty;
    
    /// <summary>
    /// 预期的子元素
    /// </summary>
    public List<XmlElementDefinition> ExpectedElements { get; set; } = new();
    
    /// <summary>
    /// 预期的属性
    /// </summary>
    public List<XmlAttributeDefinition> ExpectedAttributes { get; set; } = new();
    
    /// <summary>
    /// 最小深度
    /// </summary>
    public int MinDepth { get; set; }
    
    /// <summary>
    /// 最大深度
    /// </summary>
    public int MaxDepth { get; set; }
    
    /// <summary>
    /// 是否允许注释
    /// </summary>
    public bool AllowComments { get; set; } = true;
    
    /// <summary>
    /// 是否允许CDATA
    /// </summary>
    public bool AllowCData { get; set; } = true;
}

/// <summary>
/// XML元素定义
/// </summary>
public class XmlElementDefinition
{
    /// <summary>
    /// 元素名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否必需
    /// </summary>
    public bool IsRequired { get; set; } = false;
    
    /// <summary>
    /// 最小出现次数
    /// </summary>
    public int MinOccurs { get; set; } = 0;
    
    /// <summary>
    /// 最大出现次数
    /// </summary>
    public int MaxOccurs { get; set; } = int.MaxValue;
    
    /// <summary>
    /// 预期的子元素
    /// </summary>
    public List<XmlElementDefinition> ChildElements { get; set; } = new();
    
    /// <summary>
    /// 预期的属性
    /// </summary>
    public List<XmlAttributeDefinition> Attributes { get; set; } = new();
}

/// <summary>
/// XML属性定义
/// </summary>
public class XmlAttributeDefinition
{
    /// <summary>
    /// 属性名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否必需
    /// </summary>
    public bool IsRequired { get; set; } = false;
    
    /// <summary>
    /// 预期的值类型
    /// </summary>
    public XmlAttributeValueType ValueType { get; set; } = XmlAttributeValueType.String;
    
    /// <summary>
    /// 允许的值列表
    /// </summary>
    public List<string> AllowedValues { get; set; } = new();
    
    /// <summary>
    /// 值模式（正则表达式）
    /// </summary>
    public string? ValuePattern { get; set; }
}

/// <summary>
/// XML属性值类型
/// </summary>
public enum XmlAttributeValueType
{
    String,
    Integer,
    Decimal,
    Boolean,
    DateTime,
    Enum
}
```

### 5. 性能监控接口

#### 5.1 IPerformanceMonitor接口

```csharp
/// <summary>
/// 性能监控器接口
/// </summary>
public interface IPerformanceMonitor
{
    /// <summary>
    /// 开始性能监控
    /// </summary>
    /// <param name="operationName">操作名称</param>
    /// <returns>性能监控上下文</returns>
    PerformanceMonitorContext StartMonitoring(string operationName);
    
    /// <summary>
    /// 监控异步操作性能
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="operation">异步操作</param>
    /// <param name="operationName">操作名称</param>
    /// <returns>操作结果和性能指标</returns>
    Task<PerformanceResult<T>> MonitorAsync<T>(
        Func<Task<T>> operation,
        string operationName);
    
    /// <summary>
    /// 获取性能报告
    /// </summary>
    /// <param name="timeRange">时间范围</param>
    /// <returns>性能报告</returns>
    PerformanceReport GetPerformanceReport(TimeRange timeRange);
    
    /// <summary>
    /// 检查性能阈值
    /// </summary>
    /// <param name="operationName">操作名称</param>
    /// <param name="threshold">性能阈值</param>
    /// <returns>是否满足阈值要求</returns>
    bool CheckPerformanceThreshold(string operationName, PerformanceThreshold threshold);
    
    /// <summary>
    /// 重置性能统计
    /// </summary>
    void ResetStatistics();
}

/// <summary>
/// 性能监控上下文
/// </summary>
public class PerformanceMonitorContext : IDisposable
{
    /// <summary>
    /// 操作名称
    /// </summary>
    public string OperationName { get; set; } = string.Empty;
    
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.Now;
    
    /// <summary>
    /// 开始内存使用
    /// </summary>
    public long StartMemoryBytes { get; set; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }
    
    /// <summary>
    /// 结束内存使用
    /// </summary>
    public long EndMemoryBytes { get; set; }
    
    /// <summary>
    /// 执行时间（毫秒）
    /// </summary>
    public long ExecutionTimeMs => (long)(EndTime - StartTime).TotalMilliseconds;
    
    /// <summary>
    /// 内存使用变化（字节）
    /// </summary>
    public long MemoryDeltaBytes => EndMemoryBytes - StartMemoryBytes;
    
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; } = true;
    
    /// <summary>
    /// 异常信息
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// 自定义指标
    /// </summary>
    public Dictionary<string, object> CustomMetrics { get; set; } = new();
    
    public void Dispose()
    {
        EndTime = DateTime.Now;
        EndMemoryBytes = GC.GetTotalMemory(false);
    }
}

/// <summary>
/// 性能结果
/// </summary>
public class PerformanceResult<T>
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public T? Result { get; set; }
    
    /// <summary>
    /// 性能指标
    /// </summary>
    public PerformanceMetrics Metrics { get; set; } = new();
    
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; } = true;
    
    /// <summary>
    /// 异常信息
    /// </summary>
    public Exception? Exception { get; set; }
}

/// <summary>
/// 性能指标
/// </summary>
public class PerformanceMetrics
{
    /// <summary>
    /// 执行时间（毫秒）
    /// </summary>
    public long ExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 内存使用变化（字节）
    /// </summary>
    public long MemoryDeltaBytes { get; set; }
    
    /// <summary>
    /// 峰值内存使用（字节）
    /// </summary>
    public long PeakMemoryBytes { get; set; }
    
    /// <summary>
    /// CPU使用率
    /// </summary>
    public double CpuUsagePercent { get; set; }
    
    /// <summary>
    /// 操作吞吐量（每秒操作数）
    /// </summary>
    public double ThroughputPerSecond { get; set; }
    
    /// <summary>
    /// 自定义指标
    /// </summary>
    public Dictionary<string, object> CustomMetrics { get; set; } = new();
}

/// <summary>
/// 性能报告
/// </summary>
public class PerformanceReport
{
    /// <summary>
    /// 报告生成时间
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// 时间范围
    /// </summary>
    public TimeRange TimeRange { get; set; } = new();
    
    /// <summary>
    /// 操作统计
    /// </summary>
    public Dictionary<string, OperationStatistics> OperationStats { get; set; } = new();
    
    /// <summary>
    /// 系统资源使用情况
    /// </summary>
    public SystemResourceUsage ResourceUsage { get; set; } = new();
    
    /// <summary>
    /// 性能警告
    /// </summary>
    public List<PerformanceWarning> Warnings { get; set; } = new();
    
    /// <summary>
    /// 建议
    /// </summary>
    public List<string> Recommendations { get; set; } = new();
}

/// <summary>
/// 操作统计
/// </summary>
public class OperationStatistics
{
    /// <summary>
    /// 操作名称
    /// </summary>
    public string OperationName { get; set; } = string.Empty;
    
    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecutionCount { get; set; }
    
    /// <summary>
    /// 成功次数
    /// </summary>
    public int SuccessCount { get; set; }
    
    /// <summary>
    /// 失败次数
    /// </summary>
    public int FailureCount { get; set; }
    
    /// <summary>
    /// 平均执行时间（毫秒）
    /// </summary>
    public double AverageExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 最小执行时间（毫秒）
    /// </summary>
    public long MinExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 最大执行时间（毫秒）
    /// </summary>
    public long MaxExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 平均内存使用（字节）
    /// </summary>
    public double AverageMemoryBytes { get; set; }
    
    /// <summary>
    /// 成功率
    /// </summary>
    public double SuccessRate => ExecutionCount > 0 ? (double)SuccessCount / ExecutionCount : 0;
}

/// <summary>
/// 系统资源使用情况
/// </summary>
public class SystemResourceUsage
{
    /// <summary>
    /// CPU使用率
    /// </summary>
    public double CpuUsagePercent { get; set; }
    
    /// <summary>
    /// 内存使用率
    /// </summary>
    public double MemoryUsagePercent { get; set; }
    
    /// <summary>
    /// 磁盘使用率
    /// </summary>
    public double DiskUsagePercent { get; set; }
    
    /// <summary>
    /// 网络使用率
    /// </summary>
    public double NetworkUsagePercent { get; set; }
    
    /// <summary>
    /// 可用内存（MB）
    /// </summary>
    public long AvailableMemoryMB { get; set; }
    
    /// <summary>
    /// 可用磁盘空间（GB）
    /// </summary>
    public long AvailableDiskSpaceGB { get; set; }
}

/// <summary>
/// 性能警告
/// </summary>
public class PerformanceWarning
{
    /// <summary>
    /// 警告级别
    /// </summary>
    public PerformanceWarningLevel Level { get; set; }
    
    /// <summary>
    /// 警告消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 相关操作
    /// </summary>
    public string? OperationName { get; set; }
    
    /// <summary>
    /// 警告时间
    /// </summary>
    public DateTime WarningTime { get; set; } = DateTime.Now;
    
    /// <summary>
    /// 建议的解决方案
    /// </summary>
    public List<string> SuggestedActions { get; set; } = new();
}

/// <summary>
/// 性能警告级别
/// </summary>
public enum PerformanceWarningLevel
{
    Info,
    Warning,
    Error,
    Critical
}

/// <summary>
/// 时间范围
/// </summary>
public class TimeRange
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime Start { get; set; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime End { get; set; }
    
    /// <summary>
    /// 持续时间
    /// </summary>
    public TimeSpan Duration => End - Start;
}

/// <summary>
/// 性能阈值
/// </summary>
public class PerformanceThreshold
{
    /// <summary>
    /// 最大执行时间（毫秒）
    /// </summary>
    public long MaxExecutionTimeMs { get; set; }
    
    /// <summary>
    /// 最大内存使用（字节）
    /// </summary>
    public long MaxMemoryBytes { get; set; }
    
    /// <summary>
    /// 最大CPU使用率
    /// </summary>
    public double MaxCpuUsagePercent { get; set; }
    
    /// <summary>
    /// 最小成功率
    /// </summary>
    public double MinSuccessRate { get; set; } = 0.95;
    
    /// <summary>
    /// 最大错误率
    /// </summary>
    public double MaxErrorRate { get; set; } = 0.05;
}
```

## 使用示例

### 1. 基础XML处理示例

```csharp
// 创建增强的XML加载器
var xmlLoader = new EnhancedXmlLoader<CombatParametersDO>();

// 加载XML文件
var options = new XmlLoadOptions
{
    PreserveWhitespace = true,
    HandleEmptyElements = true,
    NormalizeLineEndings = true
};

var combatParams = await xmlLoader.LoadAsync("combat_parameters.xml", options);

// 验证XML结构
var validationResult = await xmlLoader.ValidateAsync(combatParams);
if (!validationResult.IsValid)
{
    Console.WriteLine($"验证失败: {string.Join(", ", validationResult.Errors)}");
}

// 保存XML文件
var saveOptions = new XmlSaveOptions
{
    PreserveOriginalFormat = true,
    HandleEmptyElements = true,
    OriginalXml = File.ReadAllText("combat_parameters.xml")
};

await xmlLoader.SaveAsync(combatParams, "output.xml", saveOptions);
```

### 2. 测试数据管理示例

```csharp
// 创建测试数据管理器
var testDataManager = new TestDataManager(configuration, testOutputHelper);

// 加载测试数据
var testData = testDataManager.LoadTestData<CombatParametersDO>("combat_parameters.xml");

// 验证测试数据
var validationResult = await testDataManager.ValidateTestDataAsync("combat_parameters.xml");
if (!validationResult.IsValid)
{
    testOutputHelper.WriteLine($"测试数据验证失败: {string.Join(", ", validationResult.Errors)}");
}

// 生成测试数据
var replacements = new Dictionary<string, string>
{
    { "TestId", "test_001" },
    { "TestName", "Test Combat Parameters" }
};

testDataManager.GenerateTestData("combat_parameters_template.xml", replacements, "generated_test.xml");
```

### 3. 异步测试示例

```csharp
// 创建异步测试辅助工具
var asyncTestHelper = new AsyncTestHelper();

// 执行带超时的异步操作
var result = await asyncTestHelper.ExecuteWithTimeoutAsync(
    async () => await xmlLoader.LoadAsync("large_file.xml"),
    TimeSpan.FromSeconds(30),
    "LoadLargeFile"
);

// 验证异步操作抛出异常
await asyncTestHelper.ShouldThrowAsync<XmlException>(
    async () => await xmlLoader.LoadAsync("invalid.xml"),
    "Invalid XML should throw exception"
);

// 执行多个异步操作
var operations = new List<Func<Task<CombatParametersDO>>>
{
    () => xmlLoader.LoadAsync("file1.xml"),
    () => xmlLoader.LoadAsync("file2.xml"),
    () => xmlLoader.LoadAsync("file3.xml")
};

var results = await asyncTestHelper.ExecuteAsyncOperations(operations, maxConcurrency: 2);
```

### 4. 错误处理示例

```csharp
// 创建测试异常处理器
var exceptionHandler = new TestExceptionHandler(testOutputHelper);

// 执行带有异常处理的操作
try
{
    var result = await exceptionHandler.ExecuteWithExceptionHandlingAsync(
        async () => await xmlLoader.LoadAsync("problematic_file.xml"),
        "LoadProblematicFile"
    );
}
catch (TestExecutionException ex)
{
    // 记录详细的错误信息
    exceptionHandler.HandleTestFailure(ex, "LoadProblematicFile", new { FilePath = "problematic_file.xml" });
    
    // 创建错误报告
    var errorReport = exceptionHandler.CreateErrorReport(ex, "LoadProblematicFile");
    testOutputHelper.WriteLine($"错误报告ID: {errorReport.ReportId}");
}
```

### 5. 断言增强示例

```csharp
// 创建断言辅助工具
var assertionHelper = new AssertionHelper();

// 验证XML结构
assertionHelper.ShouldHaveValidXmlStructure(combatParams);

// 验证序列化往返
assertionHelper.ShouldBeSerializable(combatParams);

// 验证空元素处理
assertionHelper.ShouldHandleEmptyElements(combatParams);

// 验证性能
await assertionHelper.ShouldLoadWithinTimeAsync<CombatParametersDO>(
    "large_file.xml", 
    1000, 
    "Large file should load within 1 second"
);
```

## 版本兼容性

### 1. 向后兼容性

- 所有现有API保持兼容
- 新功能通过可选参数添加
- 废弃功能标记为[Obsolete]但保持可用

### 2. 版本迁移指南

#### 从旧版本迁移到1.0

1. **XmlTestUtils迁移**:
   ```csharp
   // 旧版本
   var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
   
   // 新版本
   var xmlLoader = new EnhancedXmlLoader<CombatParametersDO>();
   var obj = await xmlLoader.LoadFromXmlStringAsync(xml);
   ```

2. **测试数据管理迁移**:
   ```csharp
   // 旧版本
   var filePath = Path.Combine(testDataPath, "combat_parameters.xml");
   var content = File.ReadAllText(filePath);
   
   // 新版本
   var testDataManager = new TestDataManager(configuration, testOutputHelper);
   var content = testDataManager.LoadTestData<CombatParametersDO>("combat_parameters.xml");
   ```

3. **异步操作迁移**:
   ```csharp
   // 旧版本
   var result = await Task.Run(() => loader.Load(filePath));
   
   // 新版本
   var result = await asyncTestHelper.ExecuteWithTimeoutAsync(
       async () => await loader.LoadAsync(filePath),
       TimeSpan.FromSeconds(30),
       "LoadXmlFile"
   );
   ```

## 最佳实践

### 1. XML处理最佳实践

- 始终使用`EnhancedXmlLoader`而不是直接的`XmlSerializer`
- 为所有XML操作设置适当的超时时间
- 验证XML文件的完整性和结构
- 处理空元素和特殊格式（如`<base>`元素）

### 2. 测试数据管理最佳实践

- 使用`TestDataManager`统一管理测试数据
- 验证测试数据的完整性和有效性
- 为大型测试数据文件提供压缩版本
- 定期清理和更新测试数据

### 3. 异步操作最佳实践

- 使用`AsyncTestHelper`管理异步操作
- 设置合理的超时时间
- 正确处理异步异常
- 避免阻塞异步操作

### 4. 错误处理最佳实践

- 使用`TestExceptionHandler`统一处理异常
- 记录详细的错误信息和上下文
- 提供有用的错误报告和建议
- 实现适当的重试机制

### 5. 性能监控最佳实践

- 监控关键操作的性能指标
- 设置合理的性能阈值
- 定期分析性能报告
- 优化性能瓶颈

## 总结

本文档定义了BannerlordModEditor项目修复后的完整API规范。这些API设计旨在解决当前测试失败问题，提供更好的可维护性、扩展性和性能。通过遵循这些API规范和最佳实践，可以确保项目的稳定性和长期可维护性。