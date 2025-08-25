# 基于现有TUI项目的通用XML转换框架实现

## 1. 项目结构调整

为了实现通用XML转换框架，我们需要对现有项目结构进行调整。以下是基于现有TUI项目的重构方案：

```
BannerlordModEditor.Common/
├── Framework/                          # 新增：框架核心代码
│   ├── Converters/                     # 转换器实现
│   │   ├── Base/
│   │   │   ├── ConverterBase.cs
│   │   │   ├── FileConverterBase.cs
│   │   │   └── BatchConverterBase.cs
│   │   ├── Xml/
│   │   │   ├── XmlToExcelConverter.cs
│   │   │   └── ExcelToXmlConverter.cs
│   │   └── Interfaces/
│   │       └── IConverter.cs
│   ├── Strategies/                     # 转换策略
│   │   ├── Base/
│   │   │   └── ConversionStrategyBase.cs
│   │   ├── Xml/
│   │   │   ├── XmlToExcelStrategy.cs
│   │   │   └── ExcelToXmlStrategy.cs
│   │   └── Interfaces/
│   │       └── IConversionStrategy.cs
│   ├── Pipelines/                      # 转换管道
│   │   ├── Steps/
│   │   │   ├── Validation/
│   │   │   │   ├── FileValidationStep.cs
│   │   │   │   └── XmlValidationStep.cs
│   │   │   ├── Processing/
│   │   │   │   ├── BackupStep.cs
│   │   │   │   ├── FormatDetectionStep.cs
│   │   │   │   └── DataTransformationStep.cs
│   │   │   └── Output/
│   │   │       └── FileSaveStep.cs
│   │   ├── ConversionPipeline.cs
│   │   └── Interfaces/
│   │       └── IPipeline.cs
│   ├── Context/                        # 转换上下文
│   │   ├── ConversionContext.cs
│   │   └── ConversionResult.cs
│   ├── Registries/                     # 注册器
│   │   ├── ConverterRegistry.cs
│   │   └── StrategyRegistry.cs
│   ├── Factories/                      # 工厂类
│   │   ├── ConverterFactory.cs
│   │   ├── StrategyFactory.cs
│   │   └── PipelineFactory.cs
│   ├── Managers/                       # 管理器
│   │   ├── ConversionManager.cs
│   │   └── PluginManager.cs
│   ├── Caching/                        # 缓存
│   │   ├── IConversionCache.cs
│   │   └── MemoryCache.cs
│   ├── Configuration/                   # 配置
│   │   ├── ConversionConfiguration.cs
│   │   └── ConfigurationManager.cs
│   └── Extensions/                     # 扩展方法
│       └── ServiceCollectionExtensions.cs
├── Services/                           # 现有服务，需要重构
│   ├── FormatConversionService.cs       # 重构为使用新框架
│   ├── TypedXmlConversionService.cs    # 重构为使用新框架
│   └── IFormatConversionService.cs     # 更新接口
└── Models/                             # 现有模型
    └── ...

BannerlordModEditor.TUI/
└── Services/                           # TUI特定服务
    ├── TuiConversionService.cs          # TUI专用的转换服务
    └── ITuiConversionService.cs
```

## 2. 核心框架实现

### 2.1 重构FormatConversionService

让我们基于现有的FormatConversionService，将其重构为使用新的框架架构：

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Framework.Converters;
using BannerlordModEditor.Common.Framework.Context;
using BannerlordModEditor.Common.Framework.Managers;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 格式转换服务 - 重构版本，使用新的框架架构
    /// </summary>
    public class FormatConversionService : IFormatConversionService
    {
        private readonly IConversionManager _conversionManager;
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly ILogger<FormatConversionService> _logger;

        public FormatConversionService(
            IConversionManager conversionManager,
            IXmlTypeDetectionService xmlTypeDetectionService,
            ILogger<FormatConversionService> logger)
        {
            _conversionManager = conversionManager;
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _logger = logger;
        }

        public async Task<ConversionResult> ExcelToXmlAsync(
            string excelFilePath, 
            string xmlFilePath, 
            ConversionOptions? options = null)
        {
            try
            {
                _logger.LogInformation("Converting Excel to XML: {Excel} -> {XML}", excelFilePath, xmlFilePath);

                // 检测XML类型（如果有）
                var xmlTypeInfo = options?.XmlType != null 
                    ? await GetXmlTypeInfoAsync(options.XmlType)
                    : null;

                // 设置转换选项
                var frameworkOptions = new ConversionOptions
                {
                    CreateBackup = options?.CreateBackup ?? true,
                    IncludeValidation = options?.IncludeSchemaValidation ?? true,
                    PreserveFormatting = options?.PreserveFormatting ?? true,
                    XmlType = xmlTypeInfo?.XmlType,
                    RootElementName = options?.RootElementName,
                    RowElementName = options?.RowElementName,
                    FlattenNestedElements = options?.FlattenNestedElements ?? false,
                    NestedElementSeparator = options?.NestedElementSeparator ?? "_",
                    UseCache = true
                };

                // 使用框架进行转换
                var frameworkResult = await _conversionManager.ConvertFileAsync(
                    excelFilePath,
                    xmlFilePath,
                    frameworkOptions);

                // 转换结果
                return new ConversionResult
                {
                    Success = frameworkResult.Success,
                    Message = frameworkResult.Message,
                    OutputPath = frameworkResult.OutputPath,
                    RecordsProcessed = frameworkResult.RecordsProcessed,
                    Duration = frameworkResult.Duration,
                    Warnings = frameworkResult.Warnings,
                    Errors = frameworkResult.Errors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel to XML conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = $"转换失败: {ex.Message}",
                    Errors = { ex.Message },
                    Duration = TimeSpan.Zero
                };
            }
        }

        public async Task<ConversionResult> XmlToExcelAsync(
            string xmlFilePath, 
            string excelFilePath, 
            ConversionOptions? options = null)
        {
            try
            {
                _logger.LogInformation("Converting XML to Excel: {XML} -> {Excel}", xmlFilePath, excelFilePath);

                // 检测XML类型
                var xmlTypeInfo = await _xmlTypeDetectionService.DetectXmlTypeAsync(xmlFilePath);
                
                // 设置转换选项
                var frameworkOptions = new ConversionOptions
                {
                    CreateBackup = options?.CreateBackup ?? true,
                    IncludeValidation = options?.IncludeSchemaValidation ?? true,
                    PreserveFormatting = options?.PreserveFormatting ?? true,
                    XmlType = xmlTypeInfo.XmlType,
                    WorksheetName = options?.WorksheetName,
                    FlattenNestedElements = options?.FlattenNestedElements ?? false,
                    NestedElementSeparator = options?.NestedElementSeparator ?? "_",
                    UseCache = true
                };

                // 使用框架进行转换
                var frameworkResult = await _conversionManager.ConvertFileAsync(
                    xmlFilePath,
                    excelFilePath,
                    frameworkOptions);

                // 转换结果
                return new ConversionResult
                {
                    Success = frameworkResult.Success,
                    Message = frameworkResult.Message,
                    OutputPath = frameworkResult.OutputPath,
                    RecordsProcessed = frameworkResult.RecordsProcessed,
                    Duration = frameworkResult.Duration,
                    Warnings = frameworkResult.Warnings,
                    Errors = frameworkResult.Errors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML to Excel conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = $"转换失败: {ex.Message}",
                    Errors = { ex.Message },
                    Duration = TimeSpan.Zero
                };
            }
        }

        public async Task<FileFormatInfo> DetectFileFormatAsync(string filePath)
        {
            try
            {
                var frameworkFormatInfo = await _conversionManager.DetectFormatAsync(filePath);
                
                return new FileFormatInfo
                {
                    FormatType = frameworkFormatInfo.FormatType switch
                    {
                        "xml" => FileFormatType.Xml,
                        "xlsx" => FileFormatType.Excel,
                        "xls" => FileFormatType.Excel,
                        _ => FileFormatType.Unknown
                    },
                    RootElement = frameworkFormatInfo.Properties.ContainsKey("RootElement") 
                        ? frameworkFormatInfo.Properties["RootElement"]?.ToString()
                        : null,
                    ColumnNames = frameworkFormatInfo.Properties.ContainsKey("ColumnNames")
                        ? (List<string>)frameworkFormatInfo.Properties["ColumnNames"]!
                        : new List<string>(),
                    RowCount = frameworkFormatInfo.Properties.ContainsKey("RowCount")
                        ? (int)frameworkFormatInfo.Properties["RowCount"]!
                        : 0,
                    IsSupported = frameworkFormatInfo.IsSupported,
                    FormatDescription = frameworkFormatInfo.Description,
                    Metadata = frameworkFormatInfo.Properties
                        .Where(kvp => kvp.Value is string)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()!)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Format detection failed");
                return new FileFormatInfo
                {
                    FormatType = FileFormatType.Unknown,
                    IsSupported = false,
                    FormatDescription = $"格式检测失败: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResult> ValidateConversionAsync(
            string sourceFilePath, 
            string targetFilePath, 
            ConversionDirection direction)
        {
            try
            {
                // 使用框架的验证服务
                var validator = _conversionManager as IConversionValidator;
                if (validator == null)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Errors = { new ValidationError { Message = "验证服务不可用" } }
                    };
                }

                var frameworkValidation = await validator.ValidateConversionAsync(
                    sourceFilePath,
                    targetFilePath,
                    new ConversionContext
                    {
                        SourcePath = sourceFilePath,
                        TargetPath = targetFilePath,
                        SourceFormat = Path.GetExtension(sourceFilePath).TrimStart('.'),
                        TargetFormat = Path.GetExtension(targetFilePath).TrimStart('.')
                    });

                return new ValidationResult
                {
                    IsValid = frameworkValidation.IsValid,
                    Message = frameworkValidation.Message,
                    Errors = frameworkValidation.Errors.Select(e => new ValidationError
                    {
                        Message = e.Message,
                        ErrorType = e.ErrorType switch
                        {
                            Framework.Models.ValidationErrorType.SchemaMismatch => ValidationErrorType.SchemaMismatch,
                            Framework.Models.ValidationErrorType.DataTypeMismatch => ValidationErrorType.DataTypeMismatch,
                            Framework.Models.ValidationErrorType.MissingRequiredField => ValidationErrorType.MissingRequiredField,
                            Framework.Models.ValidationErrorType.InvalidFormat => ValidationErrorType.InvalidFormat,
                            Framework.Models.ValidationErrorType.StructureMismatch => ValidationErrorType.StructureMismatch,
                            _ => ValidationErrorType.InvalidFormat
                        }
                    }).ToList(),
                    Warnings = frameworkValidation.Warnings.Select(w => new ValidationWarning
                    {
                        Message = w.Message,
                        WarningType = w.WarningType switch
                        {
                            Framework.Models.ValidationWarningType.DataTruncation => ValidationWarningType.DataTruncation,
                            Framework.Models.ValidationWarningType.FormatConversion => ValidationWarningType.FormatConversion,
                            Framework.Models.ValidationWarningType.EmptyField => ValidationWarningType.EmptyField,
                            Framework.Models.ValidationWarningType.OptionalFieldMissing => ValidationWarningType.OptionalFieldMissing,
                            _ => ValidationWarningType.FormatConversion
                        }
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed");
                return new ValidationResult
                {
                    IsValid = false,
                    Errors = { new ValidationError { Message = $"验证失败: {ex.Message}" } }
                };
            }
        }

        public async Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath)
        {
            return await _xmlTypeDetectionService.DetectXmlTypeAsync(xmlFilePath);
        }

        public async Task<List<XmlTypeInfo>> GetSupportedXmlTypesAsync()
        {
            return await _xmlTypeDetectionService.GetSupportedXmlTypesAsync();
        }

        public async Task<CreationResult> CreateTypedXmlTemplateAsync(string xmlType, string outputPath)
        {
            try
            {
                // 检查XML类型是否支持
                var isSupported = await _xmlTypeDetectionService.IsXmlTypeSupportedAsync(xmlType);
                if (!isSupported)
                {
                    return new CreationResult
                    {
                        Success = false,
                        Message = $"不支持的XML类型: {xmlType}",
                        Errors = { $"不支持的XML类型: {xmlType}" }
                    };
                }

                // 创建基本的XML模板
                var rootElement = xmlType.ToLower();
                var templateContent = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<{rootElement}>
    <!-- {xmlType} 模板 -->
    <!-- 在此添加配置内容 -->
</{rootElement}>";

                await File.WriteAllTextAsync(outputPath, templateContent);

                return new CreationResult
                {
                    Success = true,
                    Message = $"成功创建 {xmlType} XML模板",
                    OutputPath = outputPath
                };
            }
            catch (Exception ex)
            {
                return new CreationResult
                {
                    Success = false,
                    Message = $"创建XML模板失败: {ex.Message}",
                    Errors = { $"创建XML模板失败: {ex.Message}" }
                };
            }
        }

        private async Task<XmlTypeInfo?> GetXmlTypeInfoAsync(string xmlType)
        {
            var supportedTypes = await _xmlTypeDetectionService.GetSupportedXmlTypesAsync();
            return supportedTypes.FirstOrDefault(t => t.XmlType.Equals(xmlType, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// 框架转换结果到TUI转换结果的映射
    /// </summary>
    internal static class ConversionResultMapper
    {
        public static ConversionResult ToTuiResult(
            this Framework.Context.ConversionResult frameworkResult)
        {
            return new ConversionResult
            {
                Success = frameworkResult.Success,
                Message = frameworkResult.Message,
                OutputPath = frameworkResult.OutputPath,
                RecordsProcessed = frameworkResult.RecordsProcessed,
                Duration = frameworkResult.Duration,
                Warnings = frameworkResult.Warnings.Select(m => m.Message).ToList(),
                Errors = frameworkResult.Errors.Select(m => m.Message).ToList()
            };
        }
    }
}
```

### 2.2 基于现有代码的转换器实现

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Framework.Context;
using BannerlordModEditor.Common.Framework.Converters.Base;
using BannerlordModEditor.Common.Models.DO;
using ClosedXML.Excel;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Framework.Converters.Xml
{
    /// <summary>
    /// XML到Excel转换器 - 基于现有FormatConversionService的逻辑
    /// </summary>
    public class XmlToExcelConverter : FileConverterBase
    {
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly ILogger<XmlToExcelConverter> _logger;

        public XmlToExcelConverter(
            IXmlTypeDetectionService xmlTypeDetectionService,
            ILogger<XmlToExcelConverter> logger)
        {
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _logger = logger;
            
            Name = "XmlToExcel";
            Description = "Convert XML files to Excel format";
            Version = new Version(2, 0, 0);
            SupportedSourceFormats = new[] { "xml" };
            SupportedTargetFormats = new[] { "xlsx", "xls" };
        }

        protected override async Task<Framework.Context.ConversionResult> ConvertFileInternalAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            try
            {
                _logger.LogInformation("Converting XML to Excel: {Source} -> {Target}", sourcePath, targetPath);

                // 检测XML类型
                var xmlTypeInfo = await _xmlTypeDetectionService.DetectXmlTypeAsync(sourcePath);

                // 基于XML类型选择转换方法
                if (xmlTypeInfo.IsAdapted && !string.IsNullOrEmpty(xmlTypeInfo.ModelType))
                {
                    // 类型化XML转换
                    return await ConvertTypedXmlToExcelAsync(sourcePath, targetPath, xmlTypeInfo, options);
                }
                else
                {
                    // 通用XML转换
                    return await ConvertGenericXmlToExcelAsync(sourcePath, targetPath, options);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML to Excel conversion failed");
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = $"转换失败: {ex.Message}",
                    Errors = { ex.Message }
                };
            }
        }

        private async Task<Framework.Context.ConversionResult> ConvertGenericXmlToExcelAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            // 基于现有FormatConversionService的通用XML转换逻辑
            var xmlDoc = XDocument.Load(sourcePath);
            var rootElement = xmlDoc.Root;

            if (rootElement == null)
            {
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = "XML文件没有根元素",
                    Errors = { "XML文件没有根元素" }
                };
            }

            // 收集所有可能的列名
            var columnNames = new HashSet<string>();
            var rowData = new List<Dictionary<string, string>>();

            // 分析XML结构并提取数据
            var rowElements = rootElement.Elements();
            foreach (var rowElement in rowElements)
            {
                var rowDict = new Dictionary<string, string>();
                ExtractElementData(rowElement, rowDict, "", options);
                
                foreach (var key in rowDict.Keys)
                {
                    columnNames.Add(key);
                }
                
                rowData.Add(rowDict);
            }

            // 创建Excel工作簿
            using var workbook = new XLWorkbook();
            var worksheetName = options.WorksheetName ?? "Sheet1";
            var worksheet = workbook.Worksheets.Add(worksheetName);

            // 添加表头
            var sortedColumns = columnNames.OrderBy(c => c).ToList();
            for (int i = 0; i < sortedColumns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = sortedColumns[i];
            }

            // 添加数据行
            for (int rowIndex = 0; rowIndex < rowData.Count; rowIndex++)
            {
                var rowDict = rowData[rowIndex];
                for (int colIndex = 0; colIndex < sortedColumns.Count; colIndex++)
                {
                    var columnName = sortedColumns[colIndex];
                    if (rowDict.TryGetValue(columnName, out var value))
                    {
                        worksheet.Cell(rowIndex + 2, colIndex + 1).Value = value;
                    }
                }
            }

            // 自动调整列宽
            worksheet.Columns().AdjustToContents();

            // 保存Excel文件
            workbook.SaveAs(targetPath);

            return new Framework.Context.ConversionResult
            {
                Success = true,
                Message = $"成功将 {rowData.Count} 条记录从XML转换为Excel",
                RecordsProcessed = rowData.Count
            };
        }

        private async Task<Framework.Context.ConversionResult> ConvertTypedXmlToExcelAsync(
            string sourcePath,
            string targetPath,
            XmlTypeInfo xmlTypeInfo,
            ConversionOptions options)
        {
            try
            {
                // 获取模型类型
                var modelType = Type.GetType(xmlTypeInfo.ModelType);
                if (modelType == null)
                {
                    return new Framework.Context.ConversionResult
                    {
                        Success = false,
                        Message = $"无法加载模型类型: {xmlTypeInfo.ModelType}",
                        Errors = { $"无法加载模型类型: {xmlTypeInfo.ModelType}" }
                    };
                }

                // 使用GenericXmlLoader加载XML
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(modelType);
                var loader = Activator.CreateInstance(loaderType) as dynamic;
                
                if (loader == null)
                {
                    return new Framework.Context.ConversionResult
                    {
                        Success = false,
                        Message = "无法创建XML加载器",
                        Errors = { "无法创建XML加载器" }
                    };
                }

                var xmlData = await loader.LoadAsync(sourcePath);
                
                // 转换为Excel
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(xmlTypeInfo.XmlType);

                // 这里需要根据具体的XML类型实现转换逻辑
                // 简化实现：使用反射将对象属性转换为Excel列
                await ConvertTypedDataToExcel(xmlData, worksheet);

                workbook.SaveAs(targetPath);

                return new Framework.Context.ConversionResult
                {
                    Success = true,
                    Message = $"成功将类型化XML转换为Excel",
                    RecordsProcessed = await CountRecordsAsync(xmlData)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Typed XML to Excel conversion failed");
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = $"类型化XML转换失败: {ex.Message}",
                    Errors = { ex.Message }
                };
            }
        }

        private void ExtractElementData(
            XElement element,
            Dictionary<string, string> data,
            string parentPath,
            ConversionOptions options)
        {
            if (element.HasElements)
            {
                // 处理嵌套元素
                foreach (var childElement in element.Elements())
                {
                    var childPath = string.IsNullOrEmpty(parentPath) 
                        ? childElement.Name.LocalName 
                        : $"{parentPath}{options.NestedElementSeparator}{childElement.Name.LocalName}";
                    
                    if (childElement.HasElements)
                    {
                        ExtractElementData(childElement, data, childPath, options);
                    }
                    else
                    {
                        var key = options.FlattenNestedElements ? childPath : childElement.Name.LocalName;
                        data[key] = childElement.Value;
                    }
                }
            }
            else
            {
                // 叶子节点
                var key = string.IsNullOrEmpty(parentPath) ? element.Name.LocalName : parentPath;
                data[key] = element.Value;
            }
        }

        private async Task ConvertTypedDataToExcel(dynamic xmlData, IXLWorksheet worksheet)
        {
            // 使用反射获取对象属性
            var dataType = xmlData.GetType();
            var properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 添加表头
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // 添加数据
            var row = 2;
            
            // 检查是否是集合
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(dataType))
            {
                // 处理集合
                foreach (var item in (System.Collections.IEnumerable)xmlData)
                {
                    var itemType = item.GetType();
                    var itemProperties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    
                    for (int col = 0; col < itemProperties.Length; col++)
                    {
                        var value = itemProperties[col].GetValue(item);
                        worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
                    }
                    
                    row++;
                }
            }
            else
            {
                // 处理单个对象
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(xmlData);
                    worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
                }
            }

            worksheet.Columns().AdjustToContents();
        }

        private async Task<int> CountRecordsAsync(dynamic xmlData)
        {
            var dataType = xmlData.GetType();
            
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(dataType))
            {
                var enumerable = (System.Collections.IEnumerable)xmlData;
                var count = 0;
                foreach (var _ in enumerable)
                {
                    count++;
                }
                return count;
            }
            
            return 1;
        }
    }
}
```

### 2.3 Excel到XML转换器实现

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Framework.Context;
using BannerlordModEditor.Common.Framework.Converters.Base;
using BannerlordModEditor.Common.Models.DO;
using ClosedXML.Excel;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Framework.Converters.Xml
{
    /// <summary>
    /// Excel到XML转换器 - 基于现有FormatConversionService的逻辑
    /// </summary>
    public class ExcelToXmlConverter : FileConverterBase
    {
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly ILogger<ExcelToXmlConverter> _logger;

        public ExcelToXmlConverter(
            IXmlTypeDetectionService xmlTypeDetectionService,
            ILogger<ExcelToXmlConverter> logger)
        {
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _logger = logger;
            
            Name = "ExcelToXml";
            Description = "Convert Excel files to XML format";
            Version = new Version(2, 0, 0);
            SupportedSourceFormats = new[] { "xlsx", "xls" };
            SupportedTargetFormats = new[] { "xml" };
        }

        protected override async Task<Framework.Context.ConversionResult> ConvertFileInternalAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            try
            {
                _logger.LogInformation("Converting Excel to XML: {Source} -> {Target}", sourcePath, targetPath);

                // 基于XML类型选择转换方法
                if (!string.IsNullOrEmpty(options.XmlType))
                {
                    var xmlTypeInfo = await GetXmlTypeInfoAsync(options.XmlType);
                    if (xmlTypeInfo?.IsAdapted == true)
                    {
                        return await ConvertExcelToTypedXmlAsync(sourcePath, targetPath, xmlTypeInfo, options);
                    }
                }

                // 通用XML转换
                return await ConvertExcelToGenericXmlAsync(sourcePath, targetPath, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel to XML conversion failed");
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = $"转换失败: {ex.Message}",
                    Errors = { ex.Message }
                };
            }
        }

        private async Task<Framework.Context.ConversionResult> ConvertExcelToGenericXmlAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            // 基于现有FormatConversionService的Excel到XML转换逻辑
            using var workbook = new XLWorkbook(sourcePath);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            
            if (worksheet == null)
            {
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = "Excel文件没有工作表",
                    Errors = { "Excel文件没有工作表" }
                };
            }

            // 获取列名
            var headers = new List<string>();
            var firstRowUsed = worksheet.FirstRowUsed();
            if (firstRowUsed != null)
            {
                foreach (var cell in firstRowUsed.Cells())
                {
                    var cellValue = cell.Value.ToString();
                    headers.Add(string.IsNullOrEmpty(cellValue?.Trim()) 
                        ? $"Column{cell.Address.ColumnNumber}" 
                        : cellValue.Trim());
                }
            }

            if (headers.Count == 0)
            {
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = "Excel文件中没有找到列标题",
                    Errors = { "Excel文件中没有找到列标题" }
                };
            }

            // 创建XML文档
            var xmlDoc = new XDocument();
            var rootName = options.RootElementName ?? "Root";
            var rootElement = new XElement(rootName);
            xmlDoc.Add(rootElement);

            var rowElementName = options.RowElementName ?? "Item";
            var recordsProcessed = 0;

            // 处理数据行
            var dataRows = worksheet.RowsUsed().Skip(1); // 跳过标题行
            
            foreach (var row in dataRows)
            {
                var rowElement = new XElement(rowElementName);
                
                for (int i = 0; i < headers.Count; i++)
                {
                    try
                    {
                        var cell = row.Cell(i + 1);
                        var cellValue = cell.Value.ToString() ?? "";
                        var headerName = headers[i];
                        
                        // 处理嵌套元素
                        if (options.FlattenNestedElements && headerName.Contains(options.NestedElementSeparator))
                        {
                            CreateNestedElements(rowElement, headerName, cellValue, options.NestedElementSeparator);
                        }
                        else
                        {
                            var element = new XElement(SanitizeXmlName(headerName), cellValue);
                            rowElement.Add(element);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "处理单元格失败");
                        // 添加空元素以保持结构
                        var headerName = headers[i];
                        var element = new XElement(SanitizeXmlName(headerName), "");
                        rowElement.Add(element);
                    }
                }
                
                rootElement.Add(rowElement);
                recordsProcessed++;
            }

            // 保存XML文件
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = System.Text.Encoding.UTF8,
                CheckCharacters = true,
                CloseOutput = true
            };

            using var writer = XmlWriter.Create(targetPath, settings);
            xmlDoc.Save(writer);

            return new Framework.Context.ConversionResult
            {
                Success = true,
                Message = $"成功将 {recordsProcessed} 条记录从Excel转换为XML",
                RecordsProcessed = recordsProcessed
            };
        }

        private async Task<Framework.Context.ConversionResult> ConvertExcelToTypedXmlAsync(
            string sourcePath,
            string targetPath,
            XmlTypeInfo xmlTypeInfo,
            ConversionOptions options)
        {
            try
            {
                // 获取模型类型
                var modelType = Type.GetType(xmlTypeInfo.ModelType);
                if (modelType == null)
                {
                    return new Framework.Context.ConversionResult
                    {
                        Success = false,
                        Message = $"无法加载模型类型: {xmlTypeInfo.ModelType}",
                        Errors = { $"无法加载模型类型: {xmlTypeInfo.ModelType}" }
                    };
                }

                // 读取Excel数据
                using var workbook = new XLWorkbook(sourcePath);
                var worksheet = workbook.Worksheets.First();
                
                // 转换Excel为类型化对象
                var xmlData = await ConvertExcelToTypedData(worksheet, modelType, options);
                
                // 保存为XML
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(modelType);
                var loader = Activator.CreateInstance(loaderType) as dynamic;
                
                if (loader == null)
                {
                    return new Framework.Context.ConversionResult
                    {
                        Success = false,
                        Message = "无法创建XML加载器",
                        Errors = { "无法创建XML加载器" }
                    };
                }

                await loader.SaveAsync(xmlData, targetPath);

                return new Framework.Context.ConversionResult
                {
                    Success = true,
                    Message = $"成功将Excel转换为类型化XML",
                    RecordsProcessed = await CountRecordsAsync(xmlData)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel to typed XML conversion failed");
                return new Framework.Context.ConversionResult
                {
                    Success = false,
                    Message = $"Excel到类型化XML转换失败: {ex.Message}",
                    Errors = { ex.Message }
                };
            }
        }

        private async Task<dynamic> ConvertExcelToTypedData(
            IXLWorksheet worksheet,
            Type modelType,
            ConversionOptions options)
        {
            // 这里需要根据具体的XML类型实现转换逻辑
            // 简化实现：创建一个对象并设置属性
            
            // 检查模型类型是否是集合
            var collectionInterface = modelType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && 
                    i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            
            if (collectionInterface != null)
            {
                // 处理集合类型
                var elementType = collectionInterface.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(elementType);
                var list = Activator.CreateInstance(listType);
                
                var addMethod = listType.GetMethod("Add");
                var headers = GetHeaders(worksheet);
                
                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var item = Activator.CreateInstance(elementType);
                    
                    for (int i = 0; i < headers.Count; i++)
                    {
                        var property = elementType.GetProperty(headers[i]);
                        if (property != null)
                        {
                            var value = row.Cell(i + 1).Value.ToString();
                            var convertedValue = ConvertValue(value, property.PropertyType);
                            property.SetValue(item, convertedValue);
                        }
                    }
                    
                    addMethod?.Invoke(list, new[] { item });
                }
                
                return list;
            }
            else
            {
                // 处理单个对象
                var obj = Activator.CreateInstance(modelType);
                var headers = GetHeaders(worksheet);
                
                var firstDataRow = worksheet.RowsUsed().Skip(1).First();
                for (int i = 0; i < headers.Count; i++)
                {
                    var property = modelType.GetProperty(headers[i]);
                    if (property != null)
                    {
                        var value = firstDataRow.Cell(i + 1).Value.ToString();
                        var convertedValue = ConvertValue(value, property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                }
                
                return obj;
            }
        }

        private List<string> GetHeaders(IXLWorksheet worksheet)
        {
            var headers = new List<string>();
            var firstRow = worksheet.FirstRowUsed();
            
            if (firstRow != null)
            {
                foreach (var cell in firstRow.Cells())
                {
                    headers.Add(cell.Value.ToString() ?? $"Column{cell.Address.ColumnNumber}");
                }
            }
            
            return headers;
        }

        private object? ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
                return null;
                
            if (targetType == typeof(string))
                return value;
            if (targetType == typeof(int))
                return int.TryParse(value, out var intValue) ? intValue : 0;
            if (targetType == typeof(bool))
                return bool.TryParse(value, out var boolValue) ? boolValue : false;
            if (targetType == typeof(float))
                return float.TryParse(value, out var floatValue) ? floatValue : 0f;
            if (targetType == typeof(double))
                return double.TryParse(value, out var doubleValue) ? doubleValue : 0d;
            if (targetType == typeof(decimal))
                return decimal.TryParse(value, out var decimalValue) ? decimalValue : 0m;
                
            return value;
        }

        private void CreateNestedElements(
            XElement parent,
            string nestedPath,
            string value,
            string separator)
        {
            var parts = nestedPath.Split(separator);
            XElement currentElement = parent;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var elementName = SanitizeXmlName(part);

                if (i == parts.Length - 1)
                {
                    // 最后一个部分，设置值
                    currentElement.Add(new XElement(elementName, value));
                }
                else
                {
                    // 中间部分，查找或创建元素
                    var existingElement = currentElement.Element(elementName);
                    if (existingElement == null)
                    {
                        existingElement = new XElement(elementName);
                        currentElement.Add(existingElement);
                    }
                    currentElement = existingElement;
                }
            }
        }

        private string SanitizeXmlName(string name)
        {
            // 移除无效的XML字符
            var invalidChars = new[] { ' ', '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            var sanitized = name;
            
            foreach (var invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }

            // 确保以字母或下划线开头
            if (sanitized.Length > 0 && !char.IsLetter(sanitized[0]) && sanitized[0] != '_')
            {
                sanitized = "_" + sanitized;
            }

            return string.IsNullOrEmpty(sanitized) ? "element" : sanitized;
        }

        private async Task<XmlTypeInfo?> GetXmlTypeInfoAsync(string xmlType)
        {
            var supportedTypes = await _xmlTypeDetectionService.GetSupportedXmlTypesAsync();
            return supportedTypes.FirstOrDefault(t => 
                t.XmlType.Equals(xmlType, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<int> CountRecordsAsync(dynamic xmlData)
        {
            var dataType = xmlData.GetType();
            
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(dataType))
            {
                var enumerable = (System.Collections.IEnumerable)xmlData;
                var count = 0;
                foreach (var _ in enumerable)
                {
                    count++;
                }
                return count;
            }
            
            return 1;
        }
    }
}
```

### 2.4 依赖注入配置

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Framework.Converters;
using BannerlordModEditor.Common.Framework.Converters.Base;
using BannerlordModEditor.Common.Framework.Converters.Xml;
using BannerlordModEditor.Common.Framework.Factories;
using BannerlordModEditor.Common.Framework.Managers;
using BannerlordModEditor.Common.Framework.Pipelines.Steps;
using BannerlordModEditor.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.Common.Framework.Extensions
{
    /// <summary>
    /// 服务集合扩展 - 为TUI项目配置框架
    /// </summary>
    public static class TuiServiceCollectionExtensions
    {
        /// <summary>
        /// 为TUI项目添加转换框架服务
        /// </summary>
        public static IServiceCollection AddTuiConversionFramework(this IServiceCollection services)
        {
            // 框架核心服务
            services.AddSingleton<IConversionManager, ConversionManager>();
            services.AddSingleton<IConverterRegistry, ConverterRegistry>();
            services.AddSingleton<IStrategyFactory, StrategyFactory>();
            services.AddSingleton<IPipelineFactory, PipelineFactory>();
            services.AddSingleton<IConfigurationManager, ConfigurationManager>();
            
            // 缓存
            services.AddSingleton<IConversionCache, MemoryConversionCache>();
            
            // 转换器
            services.AddSingleton<IConverter, XmlToExcelConverter>();
            services.AddSingleton<IConverter, ExcelToXmlConverter>();
            
            // 管道步骤
            services.AddSingleton<IPipelineStep, FileValidationStep>();
            services.AddSingleton<IPipelineStep, BackupStep>();
            services.AddSingleton<IPipelineStep, FormatDetectionStep>();
            services.AddSingleton<IPipelineStep, DataTransformationStep>();
            services.AddSingleton<IPipelineStep, FileSaveStep>();
            services.AddSingleton<IPipelineStep, ValidationStep>();
            
            // 保持现有的服务接口，但内部使用新的框架
            services.AddSingleton<IFormatConversionService, FormatConversionService>();
            services.AddSingleton<ITypedXmlConversionService, TypedXmlConversionService>();
            services.AddSingleton<IXmlTypeDetectionService, XmlTypeDetectionService>();
            
            // 添加现有服务
            services.AddSingleton<IFileDiscoveryService, FileDiscoveryService>();
            
            return services;
        }

        /// <summary>
        /// 添加性能优化服务
        /// </summary>
        public static IServiceCollection AddTuiPerformanceOptimizations(this IServiceCollection services)
        {
            // 并行处理
            services.AddSingleton<IParallelConversionProcessor, ParallelConversionProcessor>();
            
            // 批量转换器
            services.AddSingleton<IBatchConverter, XmlBatchConverter>();
            services.AddSingleton<IBatchConverter, ExcelBatchConverter>();
            
            return services;
        }

        /// <summary>
        /// 添加插件支持
        /// </summary>
        public static IServiceCollection AddTuiPluginSupport(this IServiceCollection services)
        {
            services.AddSingleton<IPluginManager, PluginManager>();
            services.AddSingleton<IPluginLoader, PluginLoader>();
            
            return services;
        }
    }
}
```

## 3. TUI应用程序集成

### 3.1 更新Program.cs

```csharp
using BannerlordModEditor.Common.Framework.Extensions;
using BannerlordModEditor.TUI;
using BannerlordModEditor.TUI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // 配置日志
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        // 添加框架服务
        services.AddTuiConversionFramework();
        services.AddTuiPerformanceOptimizations();
        services.AddTuiPluginSupport();

        // 添加TUI服务
        services.AddSingleton<ITuiService, TuiService>();
        services.AddSingleton<IUserInterfaceService, UserInterfaceService>();
    })
    .Build();

// 初始化框架
var conversionManager = host.Services.GetRequiredService<IConversionManager>();
await conversionManager.InitializeAsync();

// 运行TUI应用程序
var tuiService = host.Services.GetRequiredService<ITuiService>();
await tuiService.RunAsync();
```

### 3.2 TUI服务实现

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Models.DO;
using Spectre.Console;

namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// TUI服务实现
    /// </summary>
    public class TuiService : ITuiService
    {
        private readonly IConversionManager _conversionManager;
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly IUserInterfaceService _uiService;
        private readonly ILogger<TuiService> _logger;

        public TuiService(
            IConversionManager conversionManager,
            IXmlTypeDetectionService xmlTypeDetectionService,
            IUserInterfaceService uiService,
            ILogger<TuiService> logger)
        {
            _conversionManager = conversionManager;
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _uiService = uiService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                _uiService.ShowHeader();
                
                var choice = await _uiService.ShowMainMenuAsync();
                
                switch (choice)
                {
                    case MainMenuOption.ConvertFile:
                        await ConvertFileAsync();
                        break;
                        
                    case MainMenuOption.BatchConvert:
                        await BatchConvertAsync();
                        break;
                        
                    case MainMenuOption.DetectFormat:
                        await DetectFormatAsync();
                        break;
                        
                    case MainMenuOption.ListSupportedTypes:
                        await ListSupportedTypesAsync();
                        break;
                        
                    case MainMenuOption.Exit:
                        _uiService.ShowGoodbyeMessage();
                        return;
                }
                
                _uiService.WaitForContinue();
            }
        }

        private async Task ConvertFileAsync()
        {
            try
            {
                // 选择源文件
                var sourcePath = await _uiService.SelectSourceFileAsync();
                if (string.IsNullOrEmpty(sourcePath))
                    return;

                // 选择目标文件
                var targetPath = await _uiService.SelectTargetFileAsync(sourcePath);
                if (string.IsNullOrEmpty(targetPath))
                    return;

                // 选择转换选项
                var options = await _uiService.ConfigureConversionOptionsAsync();

                // 显示进度
                await AnsiConsole.Progress()
                    .StartAsync(async progress =>
                    {
                        progress.StartTask()
                            .MaxValue(100)
                            .Description("正在转换文件...");

                        // 执行转换
                        var result = await _conversionManager.ConvertFileAsync(
                            sourcePath,
                            targetPath,
                            options);

                        progress.Refresh();

                        // 显示结果
                        _uiService.ShowConversionResult(result);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File conversion failed");
                _uiService.ShowError($"转换失败: {ex.Message}");
            }
        }

        private async Task BatchConvertAsync()
        {
            try
            {
                // 选择源目录
                var sourceDir = await _uiService.SelectSourceDirectoryAsync();
                if (string.IsNullOrEmpty(sourceDir))
                    return;

                // 选择输出目录
                var outputDir = await _uiService.SelectOutputDirectoryAsync();
                if (string.IsNullOrEmpty(outputDir))
                    return;

                // 选择文件类型
                var fileType = await _uiService.SelectFileTypeAsync();
                var sourceFiles = Directory.GetFiles(sourceDir, $"*.{fileType}");

                if (sourceFiles.Length == 0)
                {
                    _uiService.ShowWarning("没有找到匹配的文件");
                    return;
                }

                // 配置选项
                var options = await _uiService.ConfigureConversionOptionsAsync();
                options.TargetFormat = fileType == "xml" ? "xlsx" : "xml";

                // 显示进度
                await AnsiConsole.Progress()
                    .StartAsync(async progress =>
                    {
                        var task = progress.AddTask("批量转换")
                            .MaxValue(sourceFiles.Length);

                        var batchResult = await _conversionManager.ConvertBatchAsync(
                            sourceFiles,
                            outputDir,
                            options);

                        task.Value = sourceFiles.Length;
                        progress.Refresh();

                        // 显示结果
                        _uiService.ShowBatchConversionResult(batchResult);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Batch conversion failed");
                _uiService.ShowError($"批量转换失败: {ex.Message}");
            }
        }

        private async Task DetectFormatAsync()
        {
            try
            {
                var filePath = await _uiService.SelectFileToDetectAsync();
                if (string.IsNullOrEmpty(filePath))
                    return;

                var formatInfo = await _conversionManager.DetectFormatAsync(filePath);
                
                _uiService.ShowFormatDetectionResult(formatInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Format detection failed");
                _uiService.ShowError($"格式检测失败: {ex.Message}");
            }
        }

        private async Task ListSupportedTypesAsync()
        {
            try
            {
                var supportedTypes = await _xmlTypeDetectionService.GetSupportedXmlTypesAsync();
                
                _uiService.ShowSupportedXmlTypes(supportedTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list supported types");
                _uiService.ShowError($"获取支持的类型失败: {ex.Message}");
            }
        }
    }
}
```

## 4. 迁移策略

### 4.1 渐进式迁移

1. **第一阶段**: 保持现有接口不变，内部使用新框架
   - 重构FormatConversionService，内部使用ConversionManager
   - 保持IFormatConversionService接口不变
   - 确保现有TUI功能正常工作

2. **第二阶段**: 逐步引入新特性
   - 添加批量转换功能
   - 添加插件支持
   - 添加性能优化

3. **第三阶段**: 完全迁移到新架构
   - 更新TUI以使用新的框架特性
   - 废弃旧的实现方式
   - 清理冗余代码

### 4.2 兼容性保证

- 保持所有现有公共接口不变
- 确保所有现有测试通过
- 逐步替换内部实现
- 提供详细的迁移文档

## 5. 性能优化建议

1. **使用缓存**: 对于重复转换相同文件，使用缓存机制
2. **并行处理**: 对于批量转换，使用并行处理
3. **流式处理**: 对于大文件，考虑使用流式处理
4. **内存管理**: 及时释放大型对象，避免内存泄漏
5. **异步操作**: 确保所有IO操作都是异步的

## 6. 总结

通过这个重构方案，我们可以：

1. **保持兼容性**: 不破坏现有功能
2. **提高可维护性**: 使用更清晰的架构
3. **增强扩展性**: 支持插件和新的转换格式
4. **改善性能**: 通过缓存和并行处理
5. **简化开发**: 提供统一的API和工具

这个方案充分利用了现有的FormatConversionService中的逻辑，同时引入了更强大的框架架构，为未来的发展奠定了基础。