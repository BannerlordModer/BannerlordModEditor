# XML-Excel互转适配系统API规范

## API设计原则

### 1. 一致性原则
- **命名约定**: 所有接口遵循统一的命名规范
- **参数模式**: 标准化的参数传递和返回值格式
- **错误处理**: 统一的异常处理和错误码体系
- **版本控制**: 清晰的API版本管理策略

### 2. 可扩展性原则
- **插件化**: 支持通过插件扩展新的转换器
- **泛型支持**: 强类型的泛型接口设计
- **配置驱动**: 通过配置文件控制行为
- **向后兼容**: 保持与现有API的兼容性

### 3. 性能原则
- **异步操作**: 所有I/O操作支持异步处理
- **流式处理**: 支持大文件的流式转换
- **内存优化**: 对象池和缓存机制
- **并行处理**: 支持批量并行转换

### 4. 类型安全原则
- **强类型**: 使用泛型和类型约束
- **编译时检查**: 减少运行时错误
- **空值安全**: 启用Nullable引用类型
- **接口分离**: 细粒度的接口设计

## 核心接口定义

### 1. 增强的格式转换服务接口

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 增强的格式转换服务接口
    /// 扩展了原有的IFormatConversionService，添加类型化转换支持
    /// </summary>
    public interface IEnhancedFormatConversionService : IFormatConversionService
    {
        #region 类型化转换
        
        /// <summary>
        /// 将类型化XML转换为Excel
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> TypedXmlToExcelAsync<T>(
            string xmlPath, 
            string excelPath, 
            ConversionOptions? options = null) 
            where T : class, new();
        
        /// <summary>
        /// 将Excel转换为类型化XML
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ExcelToTypedXmlAsync<T>(
            string excelPath, 
            string xmlPath, 
            ConversionOptions? options = null) 
            where T : class, new();
        
        /// <summary>
        /// 动态类型化XML转换（基于类型名称）
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="xmlType">XML类型名称</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> DynamicTypedXmlToExcelAsync(
            string xmlPath, 
            string excelPath, 
            string xmlType, 
            ConversionOptions? options = null);
        
        /// <summary>
        /// 动态Excel到类型化XML转换（基于类型名称）
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="xmlType">XML类型名称</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> DynamicExcelToTypedXmlAsync(
            string excelPath, 
            string xmlPath, 
            string xmlType, 
            ConversionOptions? options = null);
        
        #endregion
        
        #region 批量转换
        
        /// <summary>
        /// 批量转换多个文件
        /// </summary>
        /// <param name="tasks">转换任务列表</param>
        /// <param name="options">批量转换选项</param>
        /// <returns>批量转换结果</returns>
        Task<BatchConversionResult> BatchConvertAsync(
            IEnumerable<ConversionTask> tasks, 
            BatchConversionOptions? options = null);
        
        /// <summary>
        /// 批量转换目录中的文件
        /// </summary>
        /// <param name="directory">目录路径</param>
        /// <param name="searchPattern">搜索模式</param>
        /// <param name="direction">转换方向</param>
        /// <param name="options">转换选项</param>
        /// <returns>批量转换结果</returns>
        Task<BatchConversionResult> BatchConvertDirectoryAsync(
            string directory, 
            string searchPattern, 
            ConversionDirection direction, 
            ConversionOptions? options = null);
        
        #endregion
        
        #region 模板生成
        
        /// <summary>
        /// 创建Excel模板文件
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="options">模板选项</param>
        /// <returns>创建结果</returns>
        Task<CreationResult> CreateExcelTemplateAsync(
            string xmlType, 
            string outputPath, 
            ExcelTemplateOptions? options = null);
        
        /// <summary>
        /// 创建XML模板文件
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="options">模板选项</param>
        /// <returns>创建结果</returns>
        Task<CreationResult> CreateXmlTemplateAsync(
            string xmlType, 
            string outputPath, 
            XmlTemplateOptions? options = null);
        
        #endregion
        
        #region 高级功能
        
        /// <summary>
        /// 预览转换结果
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="direction">转换方向</param>
        /// <returns>预览结果</returns>
        Task<ConversionPreviewResult> PreviewConversionAsync(
            string sourcePath, 
            ConversionDirection direction);
        
        /// <summary>
        /// 分析转换可行性
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="direction">转换方向</param>
        /// <returns>分析结果</returns>
        Task<ConversionAnalysisResult> AnalyzeConversionAsync(
            string sourcePath, 
            ConversionDirection direction);
        
        /// <summary>
        /// 验证转换兼容性
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="direction">转换方向</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateConversionCompatibilityAsync(
            string sourcePath, 
            string targetType, 
            ConversionDirection direction);
        
        #endregion
    }
}
```

### 2. 类型化转换器接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// XML-Excel转换器基础接口
    /// </summary>
    public interface IXmlExcelConverter
    {
        /// <summary>
        /// 转换XML到Excel
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> XmlToExcelAsync(
            string xmlPath, 
            string excelPath, 
            ConversionOptions options);
        
        /// <summary>
        /// 转换Excel到XML
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ExcelToXmlAsync(
            string excelPath, 
            string xmlPath, 
            ConversionOptions options);
        
        /// <summary>
        /// 验证XML文件
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateXmlAsync(string xmlPath);
        
        /// <summary>
        /// 验证Excel文件
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateExcelAsync(string excelPath);
        
        /// <summary>
        /// 是否支持指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>是否支持</returns>
        bool SupportsType(string xmlType);
        
        /// <summary>
        /// 获取转换器信息
        /// </summary>
        /// <returns>转换器信息</returns>
        ConverterInfo GetInfo();
    }
    
    /// <summary>
    /// 类型化XML-Excel转换器接口
    /// </summary>
    /// <typeparam name="T">XML模型类型</typeparam>
    public interface ITypedXmlExcelConverter<T> : IXmlExcelConverter
        where T : class, new()
    {
        /// <summary>
        /// 加载XML文件为类型化对象
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>类型化对象</returns>
        Task<T> LoadXmlAsync(string xmlPath);
        
        /// <summary>
        /// 保存类型化对象为XML文件
        /// </summary>
        /// <param name="data">类型化对象</param>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>异步任务</returns>
        Task SaveXmlAsync(T data, string xmlPath);
        
        /// <summary>
        /// 转换类型化对象为Excel数据
        /// </summary>
        /// <param name="data">类型化对象</param>
        /// <returns>Excel数据</returns>
        Task<ExcelData> ConvertToExcelAsync(T data);
        
        /// <summary>
        /// 转换Excel数据为类型化对象
        /// </summary>
        /// <param name="excelData">Excel数据</param>
        /// <returns>类型化对象</returns>
        Task<T> ConvertFromExcelAsync(ExcelData excelData);
        
        /// <summary>
        /// 验证类型化对象
        /// </summary>
        /// <param name="data">类型化对象</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateDataAsync(T data);
    }
    
    /// <summary>
    /// 通用XML-Excel转换器接口
    /// </summary>
    public interface IGenericXmlExcelConverter : IXmlExcelConverter
    {
        /// <summary>
        /// 分析XML结构
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>XML结构分析结果</returns>
        Task<XmlStructureAnalysis> AnalyzeXmlStructureAsync(string xmlPath);
        
        /// <summary>
        /// 分析Excel结构
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <returns>Excel结构分析结果</returns>
        Task<ExcelStructureAnalysis> AnalyzeExcelStructureAsync(string excelPath);
        
        /// <summary>
        /// 推断XML模式
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>推断的模式</returns>
        Task<XmlSchema> InferXmlSchemaAsync(string xmlPath);
    }
}
```

### 3. 转换器工厂接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// XML-Excel转换器工厂接口
    /// </summary>
    public interface IXmlExcelConverterFactory
    {
        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>转换器实例</returns>
        IXmlExcelConverter? GetConverter(string xmlType);
        
        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <returns>转换器实例</returns>
        ITypedXmlExcelConverter<T>? GetConverter<T>() 
            where T : class, new();
        
        /// <summary>
        /// 获取最适合文件的转换器
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>转换器实例</returns>
        IXmlExcelConverter? GetConverterForFile(string filePath);
        
        /// <summary>
        /// 获取所有支持的XML类型
        /// </summary>
        /// <returns>支持的XML类型列表</returns>
        IEnumerable<string> GetSupportedTypes();
        
        /// <summary>
        /// 检查是否支持指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>是否支持</returns>
        bool IsTypeSupported(string xmlType);
        
        /// <summary>
        /// 注册转换器
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <param name="converter">转换器实例</param>
        void RegisterConverter(string xmlType, IXmlExcelConverter converter);
        
        /// <summary>
        /// 注册类型化转换器
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <param name="converter">转换器实例</param>
        void RegisterConverter<T>(ITypedXmlExcelConverter<T> converter)
            where T : class, new();
        
        /// <summary>
        /// 获取转换器信息
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>转换器信息</returns>
        ConverterInfo? GetConverterInfo(string xmlType);
        
        /// <summary>
        /// 获取所有转换器信息
        /// </summary>
        /// <returns>转换器信息列表</returns>
        IEnumerable<ConverterInfo> GetAllConverterInfos();
    }
    
    /// <summary>
    /// 转换器工厂配置接口
    /// </summary>
    public interface IXmlExcelConverterFactoryConfigurator
    {
        /// <summary>
        /// 添加转换器类型
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <typeparam name="TConverter">转换器类型</typeparam>
        /// <returns>配置实例</returns>
        IXmlExcelConverterFactoryConfigurator AddConverter<T, TConverter>()
            where T : class, new()
            where TConverter : class, ITypedXmlExcelConverter<T>;
        
        /// <summary>
        /// 添加通用转换器
        /// </summary>
        /// <typeparam name="TConverter">转换器类型</typeparam>
        /// <returns>配置实例</returns>
        IXmlExcelConverterFactoryConfigurator AddGenericConverter<TConverter>()
            where TConverter : class, IGenericXmlExcelConverter;
        
        /// <summary>
        /// 设置默认转换器
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <param name="converterType">转换器类型</param>
        /// <returns>配置实例</returns>
        IXmlExcelConverterFactoryConfigurator SetDefaultConverter(
            string xmlType, 
            Type converterType);
        
        /// <summary>
        /// 启用/禁用自动检测
        /// </summary>
        /// <param name="enabled">是否启用</param>
        /// <returns>配置实例</returns>
        IXmlExcelConverterFactoryConfigurator EnableAutoDetection(bool enabled = true);
        
        /// <summary>
        /// 构建工厂实例
        /// </summary>
        /// <returns>转换器工厂实例</returns>
        IXmlExcelConverterFactory Build();
    }
}
```

### 4. 增强的类型检测服务接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 增强的XML类型检测服务接口
    /// </summary>
    public interface IEnhancedXmlTypeDetectionService : IXmlTypeDetectionService
    {
        /// <summary>
        /// 从XML文档检测类型
        /// </summary>
        /// <param name="xmlDocument">XML文档</param>
        /// <returns>XML类型信息</returns>
        Task<XmlTypeInfo> DetectXmlTypeAsync(System.Xml.Linq.XDocument xmlDocument);
        
        /// <summary>
        /// 获取转换策略
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>转换策略</returns>
        Task<ConversionStrategy> GetConversionStrategyAsync(string xmlType);
        
        /// <summary>
        /// 注册XML类型
        /// </summary>
        /// <param name="typeInfo">类型信息</param>
        void RegisterXmlType(XmlTypeInfo typeInfo);
        
        /// <summary>
        /// 批量注册XML类型
        /// </summary>
        /// <param name="typeInfos">类型信息列表</param>
        void RegisterXmlTypes(IEnumerable<XmlTypeInfo> typeInfos);
        
        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();
        
        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计</returns>
        (int CacheCount, int SupportedTypesCount) GetCacheStats();
        
        /// <summary>
        /// 预热类型检测
        /// </summary>
        /// <returns>异步任务</returns>
        Task WarmupAsync();
    }
    
    /// <summary>
    /// XML结构分析器接口
    /// </summary>
    public interface IXmlStructureAnalyzer
    {
        /// <summary>
        /// 分析XML文档结构
        /// </summary>
        /// <param name="xmlDocument">XML文档</param>
        /// <returns>分析结果</returns>
        Task<XmlAnalysisResult> AnalyzeStructureAsync(System.Xml.Linq.XDocument xmlDocument);
        
        /// <summary>
        /// 分析XML文件结构
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>分析结果</returns>
        Task<XmlAnalysisResult> AnalyzeStructureAsync(string xmlPath);
        
        /// <summary>
        /// 推断XML模式
        /// </summary>
        /// <param name="rootElement">根元素</param>
        /// <returns>模式信息</returns>
        Task<SchemaInfo> InferSchemaAsync(System.Xml.Linq.XElement rootElement);
        
        /// <summary>
        /// 计算XML复杂度
        /// </summary>
        /// <param name="rootElement">根元素</param>
        /// <returns>复杂度分数</returns>
        Task<ComplexityScore> CalculateComplexityAsync(System.Xml.Linq.XElement rootElement);
        
        /// <summary>
        /// 验证XML结构
        /// </summary>
        /// <param name="xmlDocument">XML文档</param>
        /// <param name="expectedType">期望的类型</param>
        /// <returns>验证结果</returns>
        Task<StructureValidationResult> ValidateStructureAsync(
            System.Xml.Linq.XDocument xmlDocument, 
            string expectedType);
    }
}
```

### 5. 增强的验证服务接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 增强的验证服务接口
    /// </summary>
    public interface IEnhancedValidationService
    {
        #region 文件验证
        
        /// <summary>
        /// 验证XML文件
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateXmlFileAsync(string xmlPath);
        
        /// <summary>
        /// 验证Excel文件
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateExcelFileAsync(string excelPath);
        
        /// <summary>
        /// 验证转换输入
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="direction">转换方向</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateConversionInputAsync(
            string sourcePath, 
            string targetPath, 
            ConversionDirection direction);
        
        #endregion
        
        #region 类型化验证
        
        /// <summary>
        /// 验证类型化XML文件
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateTypedXmlAsync<T>(string xmlPath) 
            where T : class, new();
        
        /// <summary>
        /// 验证Excel文件是否符合指定类型
        /// </summary>
        /// <typeparam name="T">XML模型类型</typeparam>
        /// <param name="excelPath">Excel文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateExcelAgainstTypeAsync<T>(string excelPath) 
            where T : class, new();
        
        #endregion
        
        #region 数据验证
        
        /// <summary>
        /// 验证数据完整性
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="direction">转换方向</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateDataIntegrityAsync(
            string sourcePath, 
            string targetPath, 
            ConversionDirection direction);
        
        /// <summary>
        /// 验证模式合规性
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="xmlType">XML类型</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateSchemaComplianceAsync(string xmlPath, string xmlType);
        
        #endregion
        
        #region 业务规则验证
        
        /// <summary>
        /// 验证业务规则
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateBusinessRulesAsync<T>(T data) 
            where T : class, new();
        
        /// <summary>
        /// 验证交叉引用
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateCrossReferencesAsync(string xmlPath);
        
        #endregion
        
        #region 自定义验证
        
        /// <summary>
        /// 注册自定义验证器
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="validator">验证器</param>
        void RegisterValidator<T>(IValidator<T> validator) 
            where T : class, new();
        
        /// <summary>
        /// 注册验证规则
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="rule">验证规则</param>
        void RegisterValidationRule<T>(IValidationRule<T> rule) 
            where T : class, new();
        
        #endregion
    }
    
    /// <summary>
    /// 验证器接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public interface IValidator<T>
        where T : class, new()
    {
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateAsync(T data);
        
        /// <summary>
        /// 验证器名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 验证器描述
        /// </summary>
        string Description { get; }
    }
    
    /// <summary>
    /// 验证规则接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public interface IValidationRule<T>
        where T : class, new()
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>验证结果</returns>
        Task<RuleValidationResult> ValidateAsync(T data);
        
        /// <summary>
        /// 规则名称
        /// </summary>
        string RuleName { get; }
        
        /// <summary>
        /// 规则描述
        /// </summary>
        string RuleDescription { get; }
        
        /// <summary>
        /// 严重程度
        /// </summary>
        ValidationSeverity Severity { get; }
    }
}
```

### 6. 性能优化服务接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 性能优化服务接口
    /// </summary>
    public interface IPerformanceOptimizer
    {
        /// <summary>
        /// 优化转换过程
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> OptimizeConversionAsync(
            string sourcePath, 
            string targetPath, 
            ConversionOptions options);
        
        /// <summary>
        /// 获取内存使用信息
        /// </summary>
        /// <returns>内存信息</returns>
        Task<MemoryInfo> GetMemoryUsageAsync();
        
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns>异步任务</returns>
        Task ClearCacheAsync();
        
        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计</returns>
        Task<CacheStats> GetCacheStatsAsync();
        
        /// <summary>
        /// 配置优化选项
        /// </summary>
        /// <param name="options">优化选项</param>
        void ConfigureOptimization(OptimizationOptions options);
        
        /// <summary>
        /// 获取性能统计信息
        /// </summary>
        /// <returns>性能统计</returns>
        Task<PerformanceStats> GetPerformanceStatsAsync();
    }
    
    /// <summary>
    /// 大文件处理器接口
    /// </summary>
    public interface ILargeFileProcessor
    {
        /// <summary>
        /// 处理大文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="options">转换选项</param>
        /// <param name="processingOptions">大文件处理选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ProcessLargeFileAsync(
            string sourcePath, 
            string targetPath, 
            ConversionOptions options, 
            LargeFileProcessingOptions processingOptions);
        
        /// <summary>
        /// 分析文件分块信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件分块信息</returns>
        Task<FileChunkInfo> AnalyzeFileForChunkingAsync(string filePath);
        
        /// <summary>
        /// 处理文件块
        /// </summary>
        /// <param name="chunk">文件块</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ProcessChunkAsync(
            FileChunk chunk, 
            ConversionOptions options);
        
        /// <summary>
        /// 合并文件块
        /// </summary>
        /// <param name="chunkPaths">文件块路径列表</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> MergeChunksAsync(
            IEnumerable<string> chunkPaths, 
            string targetPath);
    }
    
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IObjectPool<T>
        where T : class
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns>对象实例</returns>
        PooledObject<T> Get();
        
        /// <summary>
        /// 返回对象到池中
        /// </summary>
        /// <param name="pooledObject">池化对象</param>
        void Return(PooledObject<T> pooledObject);
        
        /// <summary>
        /// 清空对象池
        /// </summary>
        void Clear();
        
        /// <summary>
        /// 获取池统计信息
        /// </summary>
        /// <returns>池统计信息</returns>
        PoolStats GetStats();
    }
}
```

### 7. 错误处理服务接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 错误处理服务接口
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="context">错误上下文</param>
        /// <returns>错误结果</returns>
        Task<ErrorResult> HandleErrorAsync(Exception exception, ErrorContext context);
        
        /// <summary>
        /// 处理转换错误
        /// </summary>
        /// <param name="exception">转换异常</param>
        /// <returns>错误结果</returns>
        Task<ErrorResult> HandleConversionErrorAsync(ConversionException exception);
        
        /// <summary>
        /// 处理验证错误
        /// </summary>
        /// <param name="exception">验证异常</param>
        /// <returns>错误结果</returns>
        Task<ErrorResult> HandleValidationErrorAsync(ValidationException exception);
        
        /// <summary>
        /// 尝试错误恢复
        /// </summary>
        /// <param name="errorResult">错误结果</param>
        /// <returns>恢复结果</returns>
        Task<RecoveryResult> AttemptRecoveryAsync(ErrorResult errorResult);
        
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="errorResult">错误结果</param>
        void LogError(ErrorResult errorResult);
        
        /// <summary>
        /// 错误发生事件
        /// </summary>
        event EventHandler<ErrorEventArgs>? ErrorOccurred;
    }
    
    /// <summary>
    /// 恢复策略接口
    /// </summary>
    public interface IRecoveryStrategy
    {
        /// <summary>
        /// 判断是否可以恢复
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>是否可以恢复</returns>
        bool CanRecover(Exception exception);
        
        /// <summary>
        /// 执行恢复
        /// </summary>
        /// <param name="errorResult">错误结果</param>
        /// <returns>恢复结果</returns>
        Task<RecoveryResult> RecoverAsync(ErrorResult errorResult);
        
        /// <summary>
        /// 策略描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 策略优先级
        /// </summary>
        RecoveryPriority Priority { get; }
    }
    
    /// <summary>
    /// 恢复策略提供者接口
    /// </summary>
    public interface IRecoveryStrategyProvider
    {
        /// <summary>
        /// 获取恢复策略
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>恢复策略</returns>
        IRecoveryStrategy? GetRecoveryStrategy(Exception exception);
        
        /// <summary>
        /// 注册恢复策略
        /// </summary>
        /// <param name="strategy">恢复策略</param>
        void RegisterStrategy(IRecoveryStrategy strategy);
        
        /// <summary>
        /// 获取所有策略
        /// </summary>
        /// <returns>恢复策略列表</returns>
        IEnumerable<IRecoveryStrategy> GetAllStrategies();
    }
}
```

### 8. 进度管理服务接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 转换进度服务接口
    /// </summary>
    public interface IConversionProgressService
    {
        /// <summary>
        /// 开始进度跟踪
        /// </summary>
        /// <param name="operation">操作名称</param>
        /// <param name="totalSteps">总步数</param>
        /// <returns>进度令牌</returns>
        ProgressToken StartProgress(string operation, int? totalSteps = null);
        
        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="token">进度令牌</param>
        /// <param name="currentStep">当前步数</param>
        /// <param name="message">进度消息</param>
        void UpdateProgress(ProgressToken token, int currentStep, string? message = null);
        
        /// <summary>
        /// 完成进度
        /// </summary>
        /// <param name="token">进度令牌</param>
        void CompleteProgress(ProgressToken token);
        
        /// <summary>
        /// 取消进度
        /// </summary>
        /// <param name="token">进度令牌</param>
        void CancelProgress(ProgressToken token);
        
        /// <summary>
        /// 获取活动进度
        /// </summary>
        /// <returns>活动进度信息列表</returns>
        IEnumerable<ActiveProgressInfo> GetActiveProgress();
        
        /// <summary>
        /// 进度更新事件
        /// </summary>
        event EventHandler<ProgressEventArgs>? ProgressUpdated;
    }
    
    /// <summary>
    /// 通知服务接口
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="type">通知类型</param>
        /// <param name="duration">显示持续时间</param>
        void SendNotification(string message, NotificationType type, TimeSpan? duration = null);
        
        /// <summary>
        /// 发送成功通知
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="duration">显示持续时间</param>
        void SendSuccess(string message, TimeSpan? duration = null);
        
        /// <summary>
        /// 发送错误通知
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="duration">显示持续时间</param>
        void SendError(string message, TimeSpan? duration = null);
        
        /// <summary>
        /// 发送警告通知
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="duration">显示持续时间</param>
        void SendWarning(string message, TimeSpan? duration = null);
        
        /// <summary>
        /// 发送信息通知
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="duration">显示持续时间</param>
        void SendInfo(string message, TimeSpan? duration = null);
        
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
        
        /// <summary>
        /// 清除所有通知
        /// </summary>
        void ClearNotifications();
    }
}
```

### 9. 插件系统接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// XML-Excel转换器插件接口
    /// </summary>
    public interface IXmlExcelConverterPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 插件版本
        /// </summary>
        string Version { get; }
        
        /// <summary>
        /// 插件描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 支持的XML类型
        /// </summary>
        IEnumerable<string> SupportedTypes { get; }
        
        /// <summary>
        /// 支持的操作
        /// </summary>
        IEnumerable<SupportedOperation> SupportedOperations { get; }
        
        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ConvertAsync(
            string sourcePath, 
            string targetPath, 
            ConversionOptions options);
        
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="mode">验证模式</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateAsync(string filePath, ValidationMode mode);
        
        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <param name="options">模板选项</param>
        /// <returns>创建结果</returns>
        Task<CreationResult> CreateTemplateAsync(
            string outputPath, 
            TemplateOptions options);
        
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <returns>插件信息</returns>
        Task<PluginInfo> GetPluginInfoAsync();
        
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <returns>初始化结果</returns>
        Task<InitializationResult> InitializeAsync(IServiceProvider serviceProvider);
        
        /// <summary>
        /// 停止插件
        /// </summary>
        /// <returns>停止结果</returns>
        Task<ShutdownResult> ShutdownAsync();
    }
    
    /// <summary>
    /// 插件管理器接口
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <returns>加载结果</returns>
        Task<PluginLoadResult> LoadPluginAsync(string pluginPath);
        
        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="pluginId">插件ID</param>
        /// <returns>卸载结果</returns>
        Task<PluginUnloadResult> UnloadPluginAsync(string pluginId);
        
        /// <summary>
        /// 获取已加载的插件
        /// </summary>
        /// <returns>插件信息列表</returns>
        IEnumerable<IPluginInfo> GetLoadedPlugins();
        
        /// <summary>
        /// 获取插件
        /// </summary>
        /// <param name="pluginId">插件ID</param>
        /// <returns>插件实例</returns>
        IXmlExcelConverterPlugin? GetPlugin(string pluginId);
        
        /// <summary>
        /// 检查插件是否可用
        /// </summary>
        /// <param name="pluginId">插件ID</param>
        /// <returns>是否可用</returns>
        Task<bool> IsPluginAvailableAsync(string pluginId);
        
        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="pluginId">插件ID</param>
        /// <returns>启用结果</returns>
        Task<PluginEnableResult> EnablePluginAsync(string pluginId);
        
        /// <summary>
        /// 禁用插件
        /// </summary>
        /// <param name="pluginId">插件ID</param>
        /// <returns>禁用结果</returns>
        Task<PluginDisableResult> DisablePluginAsync(string pluginId);
        
        /// <summary>
        /// 插件加载事件
        /// </summary>
        event EventHandler<PluginEventArgs>? PluginLoaded;
        
        /// <summary>
        /// 插件卸载事件
        /// </summary>
        event EventHandler<PluginEventArgs>? PluginUnloaded;
    }
    
    /// <summary>
    /// 插件加载器接口
    /// </summary>
    public interface IPluginLoader
    {
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <returns>插件实例</returns>
        Task<IXmlExcelConverterPlugin?> LoadPluginAsync(string pluginPath);
        
        /// <summary>
        /// 验证插件
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <returns>验证结果</returns>
        Task<PluginValidationResult> ValidatePluginAsync(string pluginPath);
        
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <returns>插件信息</returns>
        Task<PluginInfo?> GetPluginInfoAsync(string pluginPath);
    }
}
```

### 10. 监控和诊断接口

```csharp
namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 转换指标收集器接口
    /// </summary>
    public interface IConversionMetricsCollector
    {
        /// <summary>
        /// 记录转换开始
        /// </summary>
        /// <param name="conversionId">转换ID</param>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        void RecordConversionStart(string conversionId, string sourceType, string targetType);
        
        /// <summary>
        /// 记录转换结束
        /// </summary>
        /// <param name="conversionId">转换ID</param>
        /// <param name="success">是否成功</param>
        /// <param name="duration">持续时间</param>
        void RecordConversionEnd(string conversionId, bool success, TimeSpan duration);
        
        /// <summary>
        /// 记录转换错误
        /// </summary>
        /// <param name="conversionId">转换ID</param>
        /// <param name="errorType">错误类型</param>
        void RecordConversionError(string conversionId, string errorType);
        
        /// <summary>
        /// 记录性能指标
        /// </summary>
        /// <param name="metricName">指标名称</param>
        /// <param name="value">指标值</param>
        void RecordPerformanceMetric(string metricName, double value);
        
        /// <summary>
        /// 获取指标报告
        /// </summary>
        /// <returns>指标报告</returns>
        Task<ConversionMetricsReport> GetMetricsReportAsync();
        
        /// <summary>
        /// 获取最近的转换记录
        /// </summary>
        /// <param name="count">记录数量</param>
        /// <returns>转换指标列表</returns>
        Task<IEnumerable<ConversionMetrics>> GetRecentConversionsAsync(int count = 100);
    }
    
    /// <summary>
    /// 健康检查服务接口
    /// </summary>
    public interface IConversionHealthChecker
    {
        /// <summary>
        /// 检查健康状态
        /// </summary>
        /// <returns>健康检查结果</returns>
        Task<HealthCheckResult> CheckHealthAsync();
        
        /// <summary>
        /// 检查服务健康状态
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>健康检查结果</returns>
        Task<HealthCheckResult> CheckServiceHealthAsync<T>() where T : class;
        
        /// <summary>
        /// 检查所有服务健康状态
        /// </summary>
        /// <returns>健康检查结果列表</returns>
        Task<IEnumerable<HealthCheckResult>> CheckAllServicesHealthAsync();
        
        /// <summary>
        /// 健康状态变化事件
        /// </summary>
        event EventHandler<HealthEventArgs>? HealthStatusChanged;
    }
    
    /// <summary>
    /// 诊断服务接口
    /// </summary>
    public interface IDiagnosticService
    {
        /// <summary>
        /// 运行诊断
        /// </summary>
        /// <param name="diagnosticType">诊断类型</param>
        /// <returns>诊断结果</returns>
        Task<DiagnosticResult> RunDiagnosticAsync(string diagnosticType);
        
        /// <summary>
        /// 运行所有诊断
        /// </summary>
        /// <returns>诊断结果列表</returns>
        Task<IEnumerable<DiagnosticResult>> RunAllDiagnosticsAsync();
        
        /// <summary>
        /// 获取诊断报告
        /// </summary>
        /// <returns>诊断报告</returns>
        Task<DiagnosticReport> GetDiagnosticReportAsync();
        
        /// <summary>
        /// 收集诊断数据
        /// </summary>
        /// <param name="dataPoints">数据点列表</param>
        /// <returns>收集结果</returns>
        Task<DataCollectionResult> CollectDiagnosticDataAsync(IEnumerable<string> dataPoints);
    }
}
```

## 数据模型定义

### 1. 增强的转换结果模型

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 增强的转换结果
    /// </summary>
    public class EnhancedConversionResult : ConversionResult
    {
        /// <summary>
        /// 转换ID
        /// </summary>
        public string ConversionId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 转换类型
        /// </summary>
        public string ConversionType { get; set; } = string.Empty;
        
        /// <summary>
        /// 源文件信息
        /// </summary>
        public FileInfo SourceInfo { get; set; } = new();
        
        /// <summary>
        /// 目标文件信息
        /// </summary>
        public FileInfo TargetInfo { get; set; } = new();
        
        /// <summary>
        /// 转换器信息
        /// </summary>
        public ConverterInfo ConverterInfo { get; set; } = new();
        
        /// <summary>
        /// 性能指标
        /// </summary>
        public PerformanceMetrics PerformanceMetrics { get; set; } = new();
        
        /// <summary>
        /// 验证结果
        /// </summary>
        public ValidationResult? ValidationResult { get; set; }
        
        /// <summary>
        /// 预览结果
        /// </summary>
        public ConversionPreviewResult? PreviewResult { get; set; }
        
        /// <summary>
        /// 错误恢复信息
        /// </summary>
        public RecoveryResult? RecoveryResult { get; set; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
    
    /// <summary>
    /// 批量转换结果
    /// </summary>
    public class BatchConversionResult
    {
        /// <summary>
        /// 批量转换ID
        /// </summary>
        public string BatchId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        
        /// <summary>
        /// 持续时间
        /// </summary>
        public TimeSpan? Duration { get; set; }
        
        /// <summary>
        /// 总任务数
        /// </summary>
        public int TotalTasks { get; set; }
        
        /// <summary>
        /// 成功任务数
        /// </summary>
        public int SuccessfulTasks { get; set; }
        
        /// <summary>
        /// 失败任务数
        /// </summary>
        public int FailedTasks { get; set; }
        
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 任务结果列表
        /// </summary>
        public List<ConversionResult> TaskResults { get; set; } = new();
        
        /// <summary>
        /// 批量转换选项
        /// </summary>
        public BatchConversionOptions? Options { get; set; }
        
        /// <summary>
        /// 错误汇总
        /// </summary>
        public List<string> ErrorSummary { get; set; } = new();
        
        /// <summary>
        /// 警告汇总
        /// </summary>
        public List<string> WarningSummary { get; set; } = new();
    }
    
    /// <summary>
    /// 转换任务
    /// </summary>
    public class ConversionTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 源文件路径
        /// </summary>
        public string SourcePath { get; set; } = string.Empty;
        
        /// <summary>
        /// 目标文件路径
        /// </summary>
        public string TargetPath { get; set; } = string.Empty;
        
        /// <summary>
        /// 转换方向
        /// </summary>
        public ConversionDirection Direction { get; set; }
        
        /// <summary>
        /// XML类型
        /// </summary>
        public string? XmlType { get; set; }
        
        /// <summary>
        /// 转换选项
        /// </summary>
        public ConversionOptions? Options { get; set; }
        
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        
        /// <summary>
        /// 任务优先级
        /// </summary>
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartedAt { get; set; }
        
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompletedAt { get; set; }
        
        /// <summary>
        /// 任务结果
        /// </summary>
        public ConversionResult? Result { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
```

### 2. 性能和监控模型

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 性能指标
    /// </summary>
    public class PerformanceMetrics
    {
        /// <summary>
        /// 内存使用量（字节）
        /// </summary>
        public long MemoryUsed { get; set; }
        
        /// <summary>
        /// CPU使用时间（毫秒）
        /// </summary>
        public long CpuTimeMs { get; set; }
        
        /// <summary>
        /// 文件读取时间（毫秒）
        /// </summary>
        public long FileReadTimeMs { get; set; }
        
        /// <summary>
        /// 文件写入时间（毫秒）
        /// </summary>
        public long FileWriteTimeMs { get; set; }
        
        /// <summary>
        /// 处理时间（毫秒）
        /// </summary>
        public long ProcessingTimeMs { get; set; }
        
        /// <summary>
        /// 验证时间（毫秒）
        /// </summary>
        public long ValidationTimeMs { get; set; }
        
        /// <summary>
        /// 峰值内存使用量（字节）
        /// </summary>
        public long PeakMemoryUsed { get; set; }
        
        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double CacheHitRate { get; set; }
        
        /// <summary>
        /// 处理速度（记录/秒）
        /// </summary>
        public double ProcessingSpeed { get; set; }
        
        /// <summary>
        /// 自定义指标
        /// </summary>
        public Dictionary<string, double> CustomMetrics { get; set; } = new();
    }
    
    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryInfo
    {
        /// <summary>
        /// 总内存（字节）
        /// </summary>
        public long TotalMemory { get; set; }
        
        /// <summary>
        /// 已用内存（字节）
        /// </summary>
        public long UsedMemory { get; set; }
        
        /// <summary>
        /// 可用内存（字节）
        /// </summary>
        public long AvailableMemory { get; set; }
        
        /// <summary>
        /// 内存使用百分比
        /// </summary>
        public double UsedPercentage { get; set; }
        
        /// <summary>
        /// GC收集次数
        /// </summary>
        public int GcCollectionCount { get; set; }
        
        /// <summary>
        /// 最后GC时间
        /// </summary>
        public DateTime? LastGcTime { get; set; }
    }
    
    /// <summary>
    /// 缓存统计
    /// </summary>
    public class CacheStats
    {
        /// <summary>
        /// 缓存项数量
        /// </summary>
        public int CacheCount { get; set; }
        
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public long HitCount { get; set; }
        
        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public long MissCount { get; set; }
        
        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double HitRate { get; set; }
        
        /// <summary>
        /// 缓存大小（字节）
        /// </summary>
        public long CacheSizeBytes { get; set; }
        
        /// <summary>
        /// 最后清理时间
        /// </summary>
        public DateTime? LastCleanupTime { get; set; }
    }
    
    /// <summary>
    /// 健康检查结果
    /// </summary>
    public class HealthCheckResult
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;
        
        /// <summary>
        /// 健康状态
        /// </summary>
        public HealthStatus Status { get; set; }
        
        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// 响应时间（毫秒）
        /// </summary>
        public double ResponseTimeMs { get; set; }
        
        /// <summary>
        /// 详细信息
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
}
```

### 3. 插件和扩展模型

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class PluginInfo : IPluginInfo
    {
        /// <summary>
        /// 插件ID
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 插件版本
        /// </summary>
        public Version Version { get; set; } = new Version(1, 0, 0);
        
        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; set; } = string.Empty;
        
        /// <summary>
        /// 支持的XML类型
        /// </summary>
        public List<string> SupportedTypes { get; set; } = new();
        
        /// <summary>
        /// 支持的操作
        /// </summary>
        public List<SupportedOperation> SupportedOperations { get; set; } = new();
        
        /// <summary>
        /// 插件功能
        /// </summary>
        public List<PluginCapability> Capabilities { get; set; } = new();
        
        /// <summary>
        /// 插件状态
        /// </summary>
        public PluginStatus Status { get; set; } = PluginStatus.Loaded;
        
        /// <summary>
        /// 加载时间
        /// </summary>
        public DateTime? LoadedAt { get; set; }
        
        /// <summary>
        /// 插件路径
        /// </summary>
        public string? PluginPath { get; set; }
        
        /// <summary>
        /// 依赖项
        /// </summary>
        public List<PluginDependency> Dependencies { get; set; } = new();
    }
    
    /// <summary>
    /// 插件加载结果
    /// </summary>
    public class PluginLoadResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// 插件实例
        /// </summary>
        public IXmlExcelConverterPlugin? Plugin { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> Errors { get; set; } = new();
        
        /// <summary>
        /// 警告信息
        /// </summary>
        public List<string> Warnings { get; set; } = new();
    }
    
    /// <summary>
    /// 转换器信息
    /// </summary>
    public class ConverterInfo
    {
        /// <summary>
        /// 转换器ID
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// 转换器名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 转换器类型
        /// </summary>
        public string ConverterType { get; set; } = string.Empty;
        
        /// <summary>
        /// 支持的XML类型
        /// </summary>
        public List<string> SupportedTypes { get; set; } = new();
        
        /// <summary>
        /// 转换器版本
        /// </summary>
        public Version Version { get; set; } = new Version(1, 0, 0);
        
        /// <summary>
        /// 转换器描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 支持的操作
        /// </summary>
        public List<SupportedOperation> SupportedOperations { get; set; } = new();
        
        /// <summary>
        /// 性能特征
        /// </summary>
        public PerformanceCharacteristics Performance { get; set; } = new();
        
        /// <summary>
        /// 限制条件
        /// </summary>
        public List<string> Limitations { get; set; } = new();
        
        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
```

## 枚举和常量定义

### 1. 转换相关枚举

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 转换方向
    /// </summary>
    public enum ConversionDirection
    {
        /// <summary>
        /// XML到Excel
        /// </summary>
        XmlToExcel,
        
        /// <summary>
        /// Excel到XML
        /// </summary>
        ExcelToXml
    }
    
    /// <summary>
    /// 支持的操作
    /// </summary>
    public enum SupportedOperation
    {
        /// <summary>
        /// 读取
        /// </summary>
        Read,
        
        /// <summary>
        /// 写入
        /// </summary>
        Write,
        
        /// <summary>
        /// 验证
        /// </summary>
        Validate,
        
        /// <summary>
        /// 转换为Excel
        /// </summary>
        ConvertToExcel,
        
        /// <summary>
        /// 从Excel转换
        /// </summary>
        ConvertFromExcel,
        
        /// <summary>
        /// 模板生成
        /// </summary>
        TemplateGeneration,
        
        /// <summary>
        /// 预览
        /// </summary>
        Preview
    }
    
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Pending,
        
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
        
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled
    }
    
    /// <summary>
    /// 任务优先级
    /// </summary>
    public enum TaskPriority
    {
        /// <summary>
        /// 低
        /// </summary>
        Low,
        
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        
        /// <summary>
        /// 高
        /// </summary>
        High,
        
        /// <summary>
        /// 紧急
        /// </summary>
        Urgent
    }
}
```

### 2. 健康和状态枚举

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 健康状态
    /// </summary>
    public enum HealthStatus
    {
        /// <summary>
        /// 健康
        /// </summary>
        Healthy,
        
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        
        /// <summary>
        /// 不健康
        /// </summary>
        Unhealthy,
        
        /// <summary>
        /// 降级
        /// </summary>
        Degraded
    }
    
    /// <summary>
    /// 进度状态
    /// </summary>
    public enum ProgressStatus
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled,
        
        /// <summary>
        /// 失败
        /// </summary>
        Failed
    }
    
    /// <summary>
    /// 插件状态
    /// </summary>
    public enum PluginStatus
    {
        /// <summary>
        /// 未加载
        /// </summary>
        NotLoaded,
        
        /// <summary>
        /// 已加载
        /// </summary>
        Loaded,
        
        /// <summary>
        /// 已启用
        /// </summary>
        Enabled,
        
        /// <summary>
        /// 已禁用
        /// </summary>
        Disabled,
        
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
}
```

### 3. 性能和优化枚举

```csharp
namespace BannerlordModEditor.TUI.Models
{
    /// <summary>
    /// 验证级别
    /// </summary>
    public enum ValidationLevel
    {
        /// <summary>
        /// 无验证
        /// </summary>
        None,
        
        /// <summary>
        /// 基本验证
        /// </summary>
        Basic,
        
        /// <summary>
        /// 标准验证
        /// </summary>
        Standard,
        
        /// <summary>
        /// 严格验证
        /// </summary>
        Strict
    }
    
    /// <summary>
    /// 插件功能
    /// </summary>
    public enum PluginCapability
    {
        /// <summary>
        /// XML到Excel转换
        /// </summary>
        XmlToExcel,
        
        /// <summary>
        /// Excel到XML转换
        /// </summary>
        ExcelToXml,
        
        /// <summary>
        /// 验证
        /// </summary>
        Validation,
        
        /// <summary>
        /// 模板生成
        /// </summary>
        TemplateGeneration,
        
        /// <summary>
        /// 预览
        /// </summary>
        Preview,
        
        /// <summary>
        /// 批量处理
        /// </summary>
        BatchProcessing,
        
        /// <summary>
        /// 大文件处理
        /// </summary>
        LargeFileProcessing
    }
    
    /// <summary>
    /// 验证模式
    /// </summary>
    public enum ValidationMode
    {
        /// <summary>
        /// 快速验证
        /// </summary>
        Quick,
        
        /// <summary>
        /// 完整验证
        /// </summary>
        Full,
        
        /// <summary>
        /// 深度验证
        /// </summary>
        Deep
    }
}
```

## 扩展方法

### 1. 服务集合扩展

```csharp
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务集合扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加XML-Excel转换服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddXmlExcelConversion(this IServiceCollection services)
        {
            // 核心服务
            services.AddSingleton<IEnhancedFormatConversionService, EnhancedFormatConversionService>();
            services.AddSingleton<IEnhancedXmlTypeDetectionService, EnhancedXmlTypeDetectionService>();
            services.AddSingleton<IEnhancedValidationService, EnhancedValidationService>();
            
            // 转换器工厂
            services.AddSingleton<IXmlExcelConverterFactory, XmlExcelConverterFactory>();
            services.AddSingleton<IPluginManager, XmlExcelConverterPluginManager>();
            services.AddSingleton<IPluginLoader, PluginLoader>();
            
            // 性能优化
            services.AddSingleton<IPerformanceOptimizer, ConversionPerformanceOptimizer>();
            services.AddSingleton<ILargeFileProcessor, LargeFileProcessor>();
            
            // 错误处理
            services.AddSingleton<IErrorHandler, ConversionErrorHandler>();
            services.AddSingleton<IRecoveryStrategyProvider, RecoveryStrategyProvider>();
            
            // 进度管理
            services.AddSingleton<IConversionProgressService, ConversionProgressService>();
            services.AddSingleton<INotificationService, NotificationService>();
            
            // 监控和诊断
            services.AddSingleton<IConversionMetricsCollector, ConversionMetricsCollector>();
            services.AddSingleton<IConversionHealthChecker, ConversionHealthChecker>();
            services.AddSingleton<IDiagnosticService, DiagnosticService>();
            
            // 对象池
            services.AddSingleton<IObjectPool<XmlSerializer>, XmlSerializerPool>();
            services.AddSingleton<IObjectPool<XLWorkbook>, ExcelWorkbookPool>();
            
            // 缓存
            services.AddMemoryCache();
            
            // 配置
            services.Configure<XmlExcelConversionOptions>(Configuration.GetSection("XmlExcelConversion"));
            
            return services;
        }
        
        /// <summary>
        /// 添加转换器工厂配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureAction">配置动作</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddXmlExcelConverterFactory(
            this IServiceCollection services,
            Action<IXmlExcelConverterFactoryConfigurator> configureAction)
        {
            var configurator = new XmlExcelConverterFactoryConfigurator(services);
            configureAction(configurator);
            
            services.AddSingleton<IXmlExcelConverterFactory>(provider => configurator.Build());
            
            return services;
        }
        
        /// <summary>
        /// 添加插件支持
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="pluginDirectory">插件目录</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddXmlExcelConverterPlugins(
            this IServiceCollection services,
            string pluginDirectory = "plugins")
        {
            services.Configure<PluginOptions>(options =>
            {
                options.PluginDirectory = pluginDirectory;
            });
            
            services.AddSingleton<IPluginManager, XmlExcelConverterPluginManager>();
            services.AddSingleton<IPluginLoader, PluginLoader>();
            
            return services;
        }
    }
}
```

### 2. 配置构建器扩展

```csharp
namespace BannerlordModEditor.TUI.Configuration
{
    /// <summary>
    /// 转换选项构建器
    /// </summary>
    public static class ConversionOptionsBuilder
    {
        /// <summary>
        /// 创建默认转换选项
        /// </summary>
        /// <returns>转换选项</returns>
        public static ConversionOptions CreateDefault()
        {
            return new ConversionOptions
            {
                IncludeSchemaValidation = true,
                PreserveFormatting = true,
                CreateBackup = true,
                FlattenNestedElements = false,
                NestedElementSeparator = "_"
            };
        }
        
        /// <summary>
        /// 创建高性能转换选项
        /// </summary>
        /// <returns>转换选项</returns>
        public static ConversionOptions CreateHighPerformance()
        {
            return new ConversionOptions
            {
                IncludeSchemaValidation = false,
                PreserveFormatting = false,
                CreateBackup = false,
                FlattenNestedElements = true,
                NestedElementSeparator = "_"
            };
        }
        
        /// <summary>
        /// 创建严格验证转换选项
        /// </summary>
        /// <returns>转换选项</returns>
        public static ConversionOptions CreateStrictValidation()
        {
            return new ConversionOptions
            {
                IncludeSchemaValidation = true,
                PreserveFormatting = true,
                CreateBackup = true,
                FlattenNestedElements = false,
                NestedElementSeparator = "_"
            };
        }
    }
    
    /// <summary>
    /// 批量转换选项构建器
    /// </summary>
    public static class BatchConversionOptionsBuilder
    {
        /// <summary>
        /// 创建默认批量转换选项
        /// </summary>
        /// <returns>批量转换选项</returns>
        public static BatchConversionOptions CreateDefault()
        {
            return new BatchConversionOptions
            {
                MaxParallelism = Environment.ProcessorCount,
                ContinueOnError = false,
                EnableProgressReporting = true,
                EnableDetailedLogging = false
            };
        }
        
        /// <summary>
        /// 创建高性能批量转换选项
        /// </summary>
        /// <returns>批量转换选项</returns>
        public static BatchConversionOptions CreateHighPerformance()
        {
            return new BatchConversionOptions
            {
                MaxParallelism = Environment.ProcessorCount * 2,
                ContinueOnError = true,
                EnableProgressReporting = false,
                EnableDetailedLogging = false
            };
        }
    }
}
```

## 总结

本API规范为XML-Excel互转适配系统提供了完整的接口定义，包括：

1. **核心转换服务**: 类型化转换、批量转换、模板生成等
2. **转换器工厂**: 动态获取和管理转换器实例
3. **类型检测**: 智能识别XML类型和结构
4. **验证服务**: 多层次的验证机制
5. **性能优化**: 缓存、对象池、大文件处理
6. **错误处理**: 完善的错误恢复机制
7. **进度管理**: 实时进度反馈
8. **插件系统**: 可扩展的转换器架构
9. **监控诊断**: 健康检查和性能监控

所有接口都遵循一致的设计原则，支持异步操作、类型安全和可扩展性，为构建高性能、可靠的XML-Excel转换系统提供了坚实的基础。