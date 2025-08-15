# 混合XML架构使用指南

## 概述

新的"弱类型合并 + 强类型编辑"混合架构已经成功实现并验证。该架构解决了Looknfeel XML序列化中节点数量从539变成537的问题，并提供了更好的XML处理能力。

## 核心优势

### 1. 解决的问题
- ✅ **节点数量保持**：不再出现539→537的节点丢失问题
- ✅ **属性数量保持**：保持1220个属性不变
- ✅ **结构完整性**：保持原始XML的完整结构
- ✅ **精确控制**：只修改实际发生变更的部分

### 2. 性能优势
- 🚀 **最小变更**：只处理实际修改的部分
- 🚀 **内存优化**：避免整棵树重新序列化
- 🚀 **并行处理**：支持大型XML文件的并行处理

### 3. 架构优势
- 🏗️ **关注点分离**：读取合并与编辑处理分离
- 🏗️ **模块化设计**：清晰的组件边界和接口
- 🏗️ **可扩展性**：易于添加新的XML类型支持

## 快速开始

### 1. 基本使用

```csharp
using BannerlordModEditor.Common.Services.HybridXml;
using BannerlordModEditor.Common.Services.HybridXml.Dto;
using BannerlordModEditor.Common.Services.HybridXml.Mappers;

// 创建服务实例
var documentMerger = new XmlDocumentMerger();
var mapper = new LooknfeelMapper();
var editorManager = new XmlEditorManager<LooknfeelEditDto>(documentMerger, mapper);

// 加载XML进行编辑
var editDto = await editorManager.LoadForEditAsync("path/to/looknfeel.xml", "/base");

// 修改内容
editDto.Type = "modified_type";
if (editDto.Widgets?.WidgetList?.Count > 0)
{
    editDto.Widgets.WidgetList[0].Name = "modified_widget_name";
}

// 保存修改
var originalDto = await editorManager.LoadForEditAsync("path/to/looknfeel.xml", "/base");
await editorManager.SaveChangesAsync("path/to/looknfeel.xml", editDto, originalDto);
```

### 2. 直接使用服务

```csharp
// 使用XmlDocument合并器
var merger = new XmlDocumentMerger();
var document = merger.LoadAndMerge("base.xml", new[] { "override1.xml", "override2.xml" });

// 提取编辑元素
var element = merger.ExtractElementForEditing(document, "/base/widgets/widget[0]");

// 使用映射器
var mapper = new LooknfeelMapper();
var widgetDto = mapper.FromXElement(element);

// 修改并生成补丁
widgetDto.Name = "new_name";
var patch = mapper.GeneratePatch(originalWidgetDto, widgetDto);

// 应用补丁
patch.ApplyTo(document);

// 保存
merger.SaveToOriginalLocation(document, "output.xml");
```

## 迁移指南

### 从旧架构迁移

#### 1. 替换GenericXmlLoader使用

**旧代码：**
```csharp
var loader = new GenericXmlLoader<LooknfeelDO>();
var data = loader.Load("looknfeel.xml");
// 修改data
loader.Save(data, "looknfeel.xml");
```

**新代码：**
```csharp
var editorManager = new XmlEditorManager<LooknfeelEditDto>(
    new XmlDocumentMerger(), 
    new LooknfeelMapper()
);
var dto = await editorManager.LoadForEditAsync("looknfeel.xml", "/base");
// 修改dto
var originalDto = await editorManager.LoadForEditAsync("looknfeel.xml", "/base");
await editorManager.SaveChangesAsync("looknfeel.xml", dto, originalDto);
```

#### 2. 替换XmlTestUtils使用

**旧代码：**
```csharp
var original = File.ReadAllText("looknfeel.xml");
var data = XmlTestUtils.Deserialize<LooknfeelDO>(original);
var serialized = XmlTestUtils.Serialize(data, original);
XmlTestUtils.AssertXmlRoundTrip(original, serialized);
```

**新代码：**
```csharp
var merger = new XmlDocumentMerger();
var mapper = new LooknfeelMapper();
var document = merger.LoadAndMerge("looknfeel.xml", Enumerable.Empty<string>());
var element = merger.ExtractElementForEditing(document, "/base");
var dto = mapper.FromXElement(element);
var convertedElement = mapper.ToXElement(dto);
// 验证节点数量保持一致
```

### 创建新的XML类型支持

#### 1. 创建DTO类

```csharp
public class YourXmlEditDto
{
    public string? Type { get; set; }
    // 其他属性...
    
    public ValidationResult Validate()
    {
        var result = new ValidationResult();
        // 验证逻辑
        return result;
    }
}
```

#### 2. 创建映射器

```csharp
public class YourXmlMapper : IXElementToDtoMapper<YourXmlEditDto>
{
    public YourXmlEditDto FromXElement(XElement element)
    {
        // 转换逻辑
        return new YourXmlEditDto();
    }

    public XElement ToXElement(YourXmlEditDto dto)
    {
        // 转换逻辑
        return new XElement("root");
    }

    public XmlPatch GeneratePatch(YourXmlEditDto original, YourXmlEditDto modified)
    {
        // 补丁生成逻辑
        return new XmlPatch();
    }

    public ValidationResult Validate(YourXmlEditDto dto)
    {
        return dto.Validate();
    }
}
```

#### 3. 使用新类型

```csharp
var editorManager = new XmlEditorManager<YourXmlEditDto>(
    new XmlDocumentMerger(), 
    new YourXmlMapper()
);
```

## 高级用法

### 1. 自定义补丁生成

```csharp
// 生成细粒度补丁
var patch = new XmlPatch();
patch.AddOperation(new XmlNodeOperation
{
    Type = OperationType.UpdateAttribute,
    XPath = "/base/widgets/widget[0]",
    Attributes = new Dictionary<string, string>
    {
        { "name", "new_name" },
        { "type", "new_type" }
    }
});
```

### 2. 批量处理

```csharp
// 批量处理多个XML文件
var files = Directory.GetFiles("path/to/xmls", "*.xml");
foreach (var file in files)
{
    var dto = await editorManager.LoadForEditAsync(file, "/base");
    // 批量修改
    await editorManager.SaveChangesAsync(file, dto, dto);
}
```

### 3. 验证和错误处理

```csharp
try
{
    var dto = await editorManager.LoadForEditAsync("looknfeel.xml", "/base");
    var validationResult = editorManager.Validate(dto);
    
    if (!validationResult.IsValid)
    {
        Console.WriteLine($"验证失败: {string.Join(", ", validationResult.Errors)}");
        return;
    }
    
    // 处理修改...
}
catch (Exception ex)
{
    Console.WriteLine($"处理失败: {ex.Message}");
}
```

## 性能优化建议

### 1. 大型XML文件处理
- 使用`ExtractElementForEditing`只提取需要的部分
- 避免频繁的完整文档加载和保存
- 考虑使用流式处理对于超大文件

### 2. 内存管理
- 及时释放XmlDocument资源
- 使用`using`语句管理资源生命周期
- 避免在内存中同时保持多个大型文档

### 3. 并发处理
- 新架构支持并行处理多个XML文件
- 可以使用`Task.WhenAll`进行批量操作
- 注意线程安全和资源竞争

## 故障排除

### 常见问题

#### 1. XPath查询失败
- 确保XPath表达式正确
- 检查XML文档结构
- 使用调试工具验证XPath

#### 2. 补丁应用失败
- 检查目标XPath是否存在
- 验证补丁操作的类型和参数
- 确保文档结构没有意外变化

#### 3. 验证失败
- 检查DTO的必填字段
- 验证数据类型和格式
- 查看详细的验证错误信息

### 调试工具

```csharp
// 获取文档统计信息
var stats = ((XmlDocumentMerger)documentMerger).GetStatistics(document);
Console.WriteLine(stats.GetSummary());

// 生成调试补丁
var debugPatch = mapper.GeneratePatch(original, modified);
foreach (var operation in debugPatch.Operations)
{
    Console.WriteLine($"{operation.Type}: {operation.XPath}");
}
```

## 未来发展

### 计划中的功能
1. **自动代码生成**：基于XML Schema自动生成DTO和映射器
2. **可视化编辑器**：提供GUI界面进行XML编辑
3. **性能监控**：添加性能指标和监控功能
4. **更多XML类型支持**：逐步支持所有Bannerlord XML类型

### 扩展点
- 自定义补丁操作类型
- 插件式验证器
- 自定义序列化格式
- 第三方工具集成

## 总结

新的混合XML架构成功解决了原有的序列化问题，提供了更好的性能、可维护性和扩展性。通过采用"弱类型合并 + 强类型编辑"的设计模式，我们既保持了与现有系统的兼容性，又提供了现代化的XML处理能力。

建议新项目直接使用新架构，现有项目可以逐步迁移以获得更好的性能和可靠性。