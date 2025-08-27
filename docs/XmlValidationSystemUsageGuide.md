# BannerlordModEditor XML校验系统使用指南

## 概述

本XML校验系统基于对Mount & Blade源码的深入分析，提供了一个完整的XML数据校验解决方案。该系统能够检测XML文件中的各种问题，包括依赖关系、隐式校验规则、Schema验证、引用完整性等。

## 核心特性

### 1. 依赖关系分析
- **自动依赖检测**: 分析XML文件之间的依赖关系
- **加载顺序计算**: 确定正确的XML文件加载顺序
- **循环依赖检测**: 识别并报告循环依赖问题

### 2. 隐式校验逻辑检测
- **源码规则提取**: 从Mount & Blade源码中提取隐式校验规则
- **业务逻辑验证**: 验证数据是否符合游戏逻辑
- **自动建议生成**: 提供修复建议

### 3. Schema验证
- **XSD验证**: 支持标准的XSD Schema验证
- **动态Schema生成**: 自动生成XML Schema
- **类型安全**: 确保数据类型正确性

### 4. 引用完整性检查
- **对象引用验证**: 验证对象引用的有效性
- **跨文件引用**: 检查跨文件的对象引用
- **索引构建**: 自动构建对象索引

## 快速开始

### 1. 初始化校验系统

```csharp
using BannerlordModEditor.Common.Validation.Core;
using BannerlordModEditor.Common.Validation.DependencyAnalysis;
using BannerlordModEditor.Common.Validation.ImplicitValidation;
using BannerlordModEditor.Common.Validation.Schema;

// 创建校验设置
var settings = new ValidationSettings
{
    ValidateSchema = true,
    ValidateDependencies = true,
    ValidateImplicitRules = true,
    ValidateReferences = true,
    EnableParallelValidation = true,
    GenerateDetailedReport = true
};

// 创建依赖分析器
var dependencyAnalyzer = new MbObjectDependencyAnalyzer(
    new XmlTypeResolver(),
    new ReferenceExtractor());

// 创建隐式校验检测器
var implicitValidationDetector = new MbImplicitValidationDetector(
    new ReflectionHelper(),
    new XmlDeserializer());

// 创建Schema验证器
var schemaValidator = new XsdSchemaValidator(
    new MbSchemaGenerator(
        new TypeAnalyzer(),
        new XmlReflectionHelper()),
    new XmlTypeResolver());

// 创建引用完整性检查器
var referenceIntegrityChecker = new ReferenceIntegrityChecker(
    new ObjectIndexBuilder(),
    new ReferenceParser());

// 创建主校验系统
var validationSystem = new BannerlordXmlValidationSystem(
    dependencyAnalyzer,
    implicitValidationDetector,
    schemaValidator,
    referenceIntegrityChecker,
    new DataTypeValidator(),
    new ValueRangeValidator(),
    new ValidationReportGenerator(),
    settings);
```

### 2. 验证单个XML文件

```csharp
// 验证单个文件
var result = await validationSystem.ValidateSingleAsync("ModuleData/crafting_pieces.xml");

// 检查结果
if (result.OverallStatus == OverallValidationStatus.Passed)
{
    Console.WriteLine("文件验证通过！");
}
else
{
    Console.WriteLine("文件验证失败：");
    foreach (var fileResult in result.FileResults.Values)
    {
        if (!fileResult.IsValid)
        {
            Console.WriteLine($"- {fileResult.Errors.Count} 个错误");
            foreach (var error in fileResult.Errors)
            {
                Console.WriteLine($"  - {error.Message} (行 {error.LineNumber})");
            }
        }
    }
}
```

### 3. 批量验证XML文件

```csharp
// 获取所有XML文件
var xmlFiles = Directory.GetFiles("ModuleData", "*.xml", SearchOption.AllDirectories);

// 批量验证
var result = await validationSystem.ValidateAllAsync(xmlFiles);

// 生成报告
var report = await validationSystem.GenerateReportAsync(result);

// 输出结果
Console.WriteLine($"验证完成：{result.FileResults.Count} 个文件");
Console.WriteLine($"通过：{result.FileResults.Values.Count(r => r.IsValid)} 个");
Console.WriteLine($"失败：{result.FileResults.Values.Count(r => !r.IsValid)} 个");
Console.WriteLine($"总计错误：{result.FileResults.Values.Sum(r => r.Errors.Count)} 个");
Console.WriteLine($"总计警告：{result.FileResults.Values.Sum(r => r.Warnings.Count)} 个");
```

### 4. 依赖关系分析

```csharp
// 分析依赖关系
var loadingOrder = await dependencyAnalyzer.DetermineLoadingOrderAsync(xmlFiles);

// 输出加载顺序
Console.WriteLine("建议的XML加载顺序：");
foreach (var phase in loadingOrder.Phases)
{
    Console.WriteLine($"阶段 {phase.Phase}: {phase.Description}");
    foreach (var file in phase.XmlFiles)
    {
        Console.WriteLine($"  - {Path.GetFileName(file)}");
    }
}

// 检查循环依赖
var circularDependencies = dependencyAnalyzer.FindCircularDependencies(xmlFiles);
if (circularDependencies.Any())
{
    Console.WriteLine("发现循环依赖：");
    foreach (var cycle in circularDependencies)
    {
        Console.WriteLine($"  - {string.Join(" -> ", cycle.Path)}");
    }
}
```

### 5. 隐式校验规则检测

```csharp
// 从源码提取隐式规则
var craftingPieceType = typeof(CraftingPiece);
var rules = await implicitValidationDetector.ExtractRulesFromSourceAsync(craftingPieceType);

// 输出规则
Console.WriteLine($"检测到 {rules.Count} 个隐式校验规则：");
foreach (var rule in rules)
{
    Console.WriteLine($"- {rule.Name} ({rule.Severity}): {rule.Description}");
}

// 验证特定对象
var xmlDocument = new XmlDocument();
xmlDocument.Load("ModuleData/crafting_pieces.xml");
var validationResult = await implicitValidationDetector.DetectImplicitRulesAsync(xmlDocument, "ModuleData/crafting_pieces.xml");

// 输出违规信息
Console.WriteLine($"发现 {validationResult.Violations.Count} 个违规：");
foreach (var violation in validationResult.Violations)
{
    Console.WriteLine($"- {violation.Rule.Name}: {violation.Message}");
}
```

## 高级用法

### 1. 自定义验证器

```csharp
// 创建自定义验证器
public class CustomCraftingValidator : IXmlValidator
{
    public bool CanValidate(XmlDataType dataType)
    {
        return dataType == XmlDataType.CraftingPieces;
    }

    public int Priority => 100;

    public async Task<ValidationResult> ValidateAsync(XmlValidationContext context)
    {
        var result = new ValidationResult();
        
        // 自定义验证逻辑
        var craftingPieces = context.XmlDocument.SelectNodes("//CraftingPiece");
        foreach (XmlElement pieceElement in craftingPieces)
        {
            var length = pieceElement.GetAttribute("length");
            if (double.TryParse(length, out var lengthValue))
            {
                if (lengthValue > 200)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = "CraftingPiece length should not exceed 200",
                        Severity = ValidationSeverity.Warning,
                        XPath = pieceElement.GetXPath()
                    });
                }
            }
        }
        
        result.IsValid = !result.Errors.Any();
        return result;
    }
}

// 注册自定义验证器
validationSystem.RegisterValidator(new CustomCraftingValidator());
```

### 2. 自定义Schema

```csharp
// 创建自定义Schema
var customSchema = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CraftingPieces"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""CraftingPiece"" maxOccurs=""unbounded"">
          <xs:complexType>
            <xs:attribute name=""id"" type=""xs:string"" use=""required""/>
            <xs:attribute name=""name"" type=""xs:string"" use=""required""/>
            <xs:attribute name=""length"" type=""xs:decimal"" use=""required""/>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

// 保存Schema
File.WriteAllText("custom_crafting_schema.xsd", customSchema);

// 注册Schema
schemaValidator.RegisterSchema(XmlDataType.CraftingPieces, "custom_crafting_schema.xsd");
```

### 3. 生成详细报告

```csharp
// 生成详细报告
var report = await validationSystem.GenerateReportAsync(result);

// 输出报告摘要
Console.WriteLine($"报告生成时间：{report.GeneratedAt}");
Console.WriteLine($"整体状态：{report.OverallStatus}");
Console.WriteLine($"总文件数：{report.Summary.TotalFiles}");
Console.WriteLine($"有效文件数：{report.Summary.ValidFiles}");
Console.WriteLine($"失败文件数：{report.Summary.FailedFiles}");
Console.WriteLine($"总错误数：{report.Summary.TotalErrors}");
Console.WriteLine($"总警告数：{report.Summary.TotalWarnings}");

// 输出建议
Console.WriteLine("修复建议：");
foreach (var recommendation in report.Recommendations)
{
    Console.WriteLine($"- [{recommendation.Priority}] {recommendation.Title}");
    Console.WriteLine($"  {recommendation.Description}");
    Console.WriteLine($"  影响文件：{recommendation.AffectedFiles.Count} 个");
}
```

## 配置选项

### 验证设置

```csharp
var settings = new ValidationSettings
{
    // 基础验证
    ValidateSchema = true,
    ValidateDependencies = true,
    ValidateImplicitRules = true,
    ValidateReferences = true,
    ValidateDataTypes = true,
    ValidateValueRanges = true,
    
    // 性能选项
    EnablePerformanceOptimizations = true,
    EnableParallelValidation = true,
    MaxValidationThreads = Environment.ProcessorCount,
    
    // 报告选项
    GenerateDetailedReport = true,
    
    // 自定义Schema路径
    CustomSchemaPaths = new List<string>
    {
        "Schemas/custom_crafting_schema.xsd",
        "Schemas/custom_items_schema.xsd"
    },
    
    // 自定义选项
    CustomOptions = new Dictionary<string, object>
    {
        { "StrictMode", true },
        { "IgnoreWarnings", false },
        { "MaxErrorsPerFile", 100 }
    }
};
```

## 错误处理

### 常见错误类型

1. **Schema验证错误**
   - XML结构不符合Schema定义
   - 数据类型不匹配
   - 必填属性缺失

2. **依赖关系错误**
   - 循环依赖
   - 缺失依赖文件
   - 加载顺序错误

3. **引用完整性错误**
   - 对象引用不存在
   - 引用格式错误
   - 类型不匹配

4. **隐式校验错误**
   - 业务逻辑违规
   - 数值范围错误
   - 状态不一致

### 错误处理示例

```csharp
try
{
    var result = await validationSystem.ValidateAllAsync(xmlFiles);
    
    if (result.OverallStatus == OverallValidationStatus.CriticalError)
    {
        // 处理严重错误
        Console.WriteLine("发现严重错误，请检查系统配置");
        return;
    }
    
    // 处理其他错误
    foreach (var fileResult in result.FileResults.Values)
    {
        if (!fileResult.IsValid)
        {
            HandleValidationErrors(fileResult);
        }
    }
}
catch (ValidationException ex)
{
    Console.WriteLine($"验证过程中发生错误：{ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"系统错误：{ex.Message}");
}
```

## 性能优化

### 1. 并行验证

```csharp
// 启用并行验证
settings.EnableParallelValidation = true;
settings.MaxValidationThreads = Environment.ProcessorCount;
```

### 2. 缓存机制

```csharp
// 系统会自动缓存以下内容：
// - Schema文件
// - 类型分析结果
// - 对象索引
// - 隐式校验规则
```

### 3. 增量验证

```csharp
// 只验证修改过的文件
var modifiedFiles = GetModifiedFiles();
var result = await validationSystem.ValidateAllAsync(modifiedFiles);
```

## 扩展开发

### 1. 实现自定义验证器

```csharp
public class CustomValidator : IXmlValidator
{
    public bool CanValidate(XmlDataType dataType)
    {
        // 返回是否支持验证指定类型
        return dataType == XmlDataType.CraftingPieces;
    }

    public int Priority => 100; // 优先级，数字越大优先级越高

    public async Task<ValidationResult> ValidateAsync(XmlValidationContext context)
    {
        var result = new ValidationResult();
        
        // 实现验证逻辑
        // ...
        
        return result;
    }
}
```

### 2. 实现自定义规则检测器

```csharp
public class CustomRuleDetector : IImplicitValidationDetector
{
    public async Task<List<ImplicitValidationRule>> ExtractRulesFromSourceAsync(Type objectType)
    {
        var rules = new List<ImplicitValidationRule>();
        
        // 实现规则提取逻辑
        // ...
        
        return rules;
    }
}
```

## 最佳实践

1. **分层验证**: 先进行Schema验证，再进行业务逻辑验证
2. **错误优先**: 优先处理严重错误和关键问题
3. **性能考虑**: 大量文件时启用并行验证
4. **定期验证**: 在开发过程中定期运行验证
5. **报告分析**: 仔细分析验证报告中的建议

## 故障排除

### 常见问题

1. **验证失败**
   - 检查XML文件格式是否正确
   - 确认Schema文件存在且有效
   - 验证依赖文件是否完整

2. **性能问题**
   - 减少同时验证的文件数量
   - 启用性能优化选项
   - 检查系统资源使用情况

3. **内存问题**
   - 分批处理大型XML文件
   - 定期清理缓存
   - 监控内存使用情况

通过本指南，您应该能够有效地使用BannerlordModEditor XML校验系统来验证和改进您的XML文件质量。