# 增强的XML适配系统使用指南

## 概述

基于对骑马与砍杀2游戏源代码的深入分析，我们实现了一个增强的XML适配系统，该系统借鉴了游戏中的最佳实践，提供了更强大和灵活的XML处理能力。

## 核心组件

### 1. BannerlordObjectManager
- **功能**: 对象管理器，类似游戏中的MBObjectManager
- **特点**: 
  - 类型安全的对象注册和管理
  - XML文档的批量加载
  - 对象缓存和生命周期管理
  - 支持异步操作

### 2. BannerlordXmlResource
- **功能**: XML资源管理器，类似游戏中的XmlResource
- **特点**:
  - 模块化XML文件管理
  - 支持多模块XML合并
  - XSD验证和XSLT转换
  - 游戏类型过滤

### 3. BannerlordXmlHelper
- **功能**: XML辅助工具类，类似游戏中的XmlHelper
- **特点**:
  - 提供基础数据类型的XML读取
  - 支持十六进制颜色值
  - 枚举类型支持
  - 强类型的数据转换

### 4. EnhancedXmlLoader
- **功能**: 增强的XML加载器
- **特点**:
  - 结合对象管理器的智能加载
  - 批量操作支持
  - XML验证功能
  - 传统序列化方式的回退

## 使用示例

### 基本使用

```csharp
// 1. 获取对象管理器实例
var objectManager = BannerlordObjectManager.Instance;

// 2. 注册XML对象类型
objectManager.RegisterType<ItemModifiersDO>("item_modifier", "item_modifiers", 1);
objectManager.RegisterType<CombatParametersDO>("combat_parameter", "combat_parameters", 2);

// 3. 加载XML文件
var objects = await objectManager.LoadXmlAsync("example/ModuleData/item_modifiers.xml");

// 4. 查询对象
var itemModifiers = objectManager.GetObjects<ItemModifiersDO>();
var firstModifier = itemModifiers.FirstOrDefault();

// 5. 根据ID查询
var foundObject = objectManager.GetObjectById("modifier_001");
```

### 使用EnhancedXmlLoader

```csharp
// 1. 创建加载器实例
var loader = new EnhancedXmlLoader<ItemModifiersDO>();

// 2. 从文件加载
var itemModifier = await loader.LoadFromFileAsync("example/ModuleData/item_modifiers.xml");

// 3. 批量加载
var allModifiers = await loader.LoadFromDirectoryAsync("example/ModuleData");

// 4. 保存对象
await loader.SaveToFileAsync(itemModifier, "output/item_modifiers.xml");

// 5. 批量保存
await loader.BatchSaveAsync(allModifiers, "output", obj => $"{obj.StringId}.xml");
```

### 使用XML资源管理器

```csharp
// 1. 初始化XML资源
BannerlordXmlResource.GetXmlListAndApply("NativeModule");

// 2. 获取合并的XML文档
var mergedXml = await BannerlordXmlResource.GetMergedXmlForManagedAsync(
    "item_modifiers", 
    skipValidation: false,
    gameType: "Campaign"
);

// 3. 使用合并后的XML
var document = mergedXml;
// 处理XML文档...
```

### 使用XML辅助工具

```csharp
// 从XML节点读取数据
var node = xmlDoc.SelectSingleNode("//item_modifier");

// 读取基础数据类型
var id = BannerlordXmlHelper.ReadString(node, "id");
var value = BannerlordXmlHelper.ReadInt(node, "value", 0);
var isActive = BannerlordXmlHelper.ReadBool(node, "is_active", false);
var color = BannerlordXmlHelper.ReadHexColor(node, "color", 0xFFFFFFFF);

// 读取枚举
var modifierType = BannerlordXmlHelper.ReadEnum<ModifierType>(node, "type", ModifierType.None);

// 读取子节点
var description = BannerlordXmlHelper.ReadChildNodeText(node, "description");
```

## 高级功能

### 1. 自定义领域对象

```csharp
public class CustomItemDO : DomainObjectBase
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public ItemRarity Rarity { get; set; }
    public List<string> Tags { get; set; } = new List<string>();

    public override void Deserialize(BannerlordObjectManager objectManager, XmlNode node)
    {
        base.Deserialize(objectManager, node);
        
        Name = BannerlordXmlHelper.ReadString(node, "name");
        Value = BannerlordXmlHelper.ReadInt(node, "value", 0);
        Rarity = BannerlordXmlHelper.ReadEnum(node, "rarity", ItemRarity.Common);
        
        // 读取标签
        var tagNodes = node.SelectNodes("tags/tag");
        Tags = tagNodes.Cast<XmlNode>()
            .Select(n => n.InnerText)
            .ToList();
    }
}
```

### 2. XML验证

```csharp
// 创建验证器
var loader = new EnhancedXmlLoader<ItemModifiersDO>();

// 验证XML
var validationResult = await loader.ValidateXmlAsync(xmlContent);

if (validationResult.IsValid)
{
    Console.WriteLine($"XML验证通过，包含 {validationResult.ObjectCount} 个对象");
}
else
{
    Console.WriteLine("XML验证失败:");
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"  - {error}");
    }
}
```

### 3. 批量处理

```csharp
// 批量处理大量XML文件
var processor = new BatchXmlProcessor();

await processor.ProcessDirectoryAsync("example/ModuleData", async (file, content) =>
{
    try
    {
        var loader = new EnhancedXmlLoader<ItemModifiersDO>();
        var obj = await loader.LoadFromXmlStringAsync(content, file);
        
        // 处理对象...
        return ProcessResult.Success;
    }
    catch (Exception ex)
    {
        return ProcessResult.Failed(ex.Message);
    }
});
```

## 性能优化

### 1. 对象缓存

```csharp
// 启用对象缓存
var objectManager = BannerlordObjectManager.Instance;

// 加载对象时会自动缓存
var objects = await objectManager.LoadXmlAsync("large_file.xml");

// 后续查询会使用缓存
var cachedObject = objectManager.GetObjectById("object_001");

// 清空缓存（当内存不足时）
objectManager.ClearCache();
```

### 2. 异步处理

```csharp
// 并行加载多个XML文件
var xmlFiles = Directory.GetFiles("example/ModuleData", "*.xml");
var tasks = xmlFiles.Select(file => objectManager.LoadXmlAsync(file));

var results = await Task.WhenAll(tasks);
var allObjects = results.SelectMany(r => r).ToList();
```

### 3. 内存管理

```csharp
// 使用大型XML文件处理器
var processor = new LargeXmlFileProcessor();

await processor.ProcessLargeXmlAsync("huge_file.xml", chunk =>
{
    // 处理数据块
    foreach (var obj in chunk)
    {
        // 处理对象...
    }
    
    return Task.CompletedTask;
});
```

## 错误处理和调试

### 1. 异常处理

```csharp
try
{
    var loader = new EnhancedXmlLoader<ItemModifiersDO>();
    var obj = await loader.LoadFromFileAsync("item_modifiers.xml");
    
    // 处理对象...
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"文件未找到: {ex.FileName}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"XML处理失败: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"未知错误: {ex.Message}");
}
```

### 2. 调试信息

```csharp
// 启用详细日志
var logger = new SimpleLogger();
logger.LogLevel = LogLevel.Debug;

// 使用带日志的加载器
var loader = new EnhancedXmlLoader<ItemModifiersDO>(logger);

// 加载时会输出详细日志
var obj = await loader.LoadFromFileAsync("item_modifiers.xml");
```

## 最佳实践

### 1. 类型注册

```csharp
// 在应用程序启动时注册所有类型
public static void ConfigureObjectManager()
{
    var manager = BannerlordObjectManager.Instance;
    
    // 按功能域分组注册
    manager.RegisterType<ItemModifiersDO>("item_modifier", "item_modifiers", 1);
    manager.RegisterType<CombatParametersDO>("combat_parameter", "combat_parameters", 2);
    manager.RegisterType<ActionTypesDO>("action_type", "action_types", 3);
    
    // 注册自定义类型
    manager.RegisterType<CustomItemDO>("custom_item", "custom_items", 100);
}
```

### 2. 资源管理

```csharp
// 使用using语句管理资源
using var loader = new EnhancedXmlLoader<ItemModifiersDO>();
var obj = await loader.LoadFromFileAsync("item_modifiers.xml");

// 处理对象...
```

### 3. 并发控制

```csharp
// 使用信号量控制并发
var semaphore = new SemaphoreSlim(maxConcurrentLoads: 4);

var tasks = xmlFiles.Select(async file =>
{
    await semaphore.WaitAsync();
    try
    {
        return await objectManager.LoadXmlAsync(file);
    }
    finally
    {
        semaphore.Release();
    }
});
```

## 总结

增强的XML适配系统提供了强大而灵活的XML处理能力，通过借鉴骑马与砍杀2游戏的最佳实践，我们实现了：

1. **类型安全的对象管理**
2. **高性能的批量处理**
3. **完善的错误处理**
4. **灵活的扩展机制**
5. **优秀的性能表现**

这个系统不仅能够处理当前的XML适配需求，还为未来的功能扩展提供了坚实的基础。