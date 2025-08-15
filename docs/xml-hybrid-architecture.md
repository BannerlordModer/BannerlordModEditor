# XML混合架构设计文档

## 架构概述

针对BannerlordModEditor项目中XML序列化测试失败的问题，设计一种新的"弱类型合并 + 强类型编辑"的混合架构。

## 核心问题分析

### 当前问题
1. **节点数量差异**：序列化后从539个节点变成537个
2. **属性数量差异**：从1220个属性变成1210个
3. **属性错位**：大量name属性在meshes和sub_widgets之间错位
4. **结构不一致**：元素顺序在序列化后发生变化

### 根本原因
- 直接使用XmlSerializer进行完整的XML序列化/反序列化
- 缺乏对XML结构的精确控制
- 空元素处理不当
- 属性顺序和节点顺序的不可控性

## 新架构设计

### 设计原则
1. **关注点分离**：读取合并与编辑处理分离
2. **结构保持**：保持原始XML的结构完整性
3. **最小变更**：只修改实际发生变更的部分
4. **向后兼容**：保持现有API的兼容性

### 架构组件

#### 1. XmlDocument合并服务 (弱类型)
```csharp
public interface IXmlDocumentMerger
{
    XmlDocument MergeModules(IEnumerable<string> modulePaths);
    XmlDocument LoadAndMerge(string basePath, IEnumerable<string> overridePaths);
    void SaveToOriginalLocation(XmlDocument document, string originalPath);
}
```

#### 2. 强类型DTO映射器
```csharp
public interface IXElementToDtoMapper<T>
{
    T FromXElement(XElement element);
    XElement ToXElement(T dto);
    XmlPatch GeneratePatch(T original, T modified);
}
```

#### 3. 差异补丁系统
```csharp
public class XmlPatch
{
    public List<XmlNodeOperation> Operations { get; } = new();
    public void ApplyTo(XmlDocument document);
    public void ApplyTo(XElement element);
}

public class XmlNodeOperation
{
    public OperationType Type { get; set; }
    public string XPath { get; set; }
    public string? Value { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}
```

#### 4. XML编辑管理器
```csharp
public class XmlEditorManager
{
    private readonly IXmlDocumentMerger _documentMerger;
    private readonly IXElementToDtoMapper<T> _mapper;
    
    public async Task<T> LoadForEditAsync(string xmlPath, string elementXPath);
    public async Task SaveChangesAsync(string xmlPath, T modifiedDto, T originalDto);
    public XmlDocument GetMergedDocument();
}
```

## 工作流程

### 1. 读取流程
```
原始XML文件 → XmlDocument合并器 → 合并后的XmlDocument → 解析为XDocument → 提取编辑节点 → 映射为DTO
```

### 2. 编辑流程
```
DTO → UI编辑 → 修改后的DTO → 生成差异补丁 → 应用补丁到XmlDocument → 保存到文件
```

### 3. 保存流程
```
修改后的DTO → 生成差异补丁 → 应用补丁到原始XmlDocument → 保持原始结构 → 保存到文件
```

## 技术实现

### XmlDocument合并服务实现
```csharp
public class XmlDocumentMerger : IXmlDocumentMerger
{
    public XmlDocument MergeModules(IEnumerable<string> modulePaths)
    {
        var baseDoc = new XmlDocument();
        // 实现模块合并逻辑
        return baseDoc;
    }
    
    public XmlDocument LoadAndMerge(string basePath, IEnumerable<string> overridePaths)
    {
        // 实现基础文件加载和覆盖合并
        var baseDoc = new XmlDocument();
        baseDoc.Load(basePath);
        
        foreach (var overridePath in overridePaths)
        {
            MergeOverride(baseDoc, overridePath);
        }
        
        return baseDoc;
    }
    
    private void MergeOverride(XmlDocument baseDoc, string overridePath)
    {
        // 实现覆盖合并逻辑
    }
}
```

### XElement到DTO映射器实现
```csharp
public class LooknfeelMapper : IXElementToDtoMapper<LooknfeelEditDto>
{
    public LooknfeelEditDto FromXElement(XElement element)
    {
        // 将XElement转换为强类型DTO
        return new LooknfeelEditDto
        {
            // 映射属性
        };
    }
    
    public XElement ToXElement(LooknfeelEditDto dto)
    {
        // 将DTO转换回XElement
        return new XElement("looknfeel");
    }
    
    public XmlPatch GeneratePatch(LooknfeelEditDto original, LooknfeelEditDto modified)
    {
        // 生成最小差异补丁
        var patch = new XmlPatch();
        // 比较差异并生成操作
        return patch;
    }
}
```

### 差异补丁应用
```csharp
public class XmlPatch
{
    public void ApplyTo(XmlDocument document)
    {
        foreach (var operation in Operations)
        {
            ApplyOperation(document, operation);
        }
    }
    
    private void ApplyOperation(XmlDocument document, XmlNodeOperation operation)
    {
        switch (operation.Type)
        {
            case OperationType.UpdateAttribute:
                UpdateAttribute(document, operation);
                break;
            case OperationType.UpdateText:
                UpdateText(document, operation);
                break;
            case OperationType.AddElement:
                AddElement(document, operation);
                break;
            case OperationType.RemoveElement:
                RemoveElement(document, operation);
                break;
        }
    }
}
```

## 优势

### 1. 结构保持
- 保持原始XML的节点结构和顺序
- 避免整棵树重新序列化导致的问题
- 精确控制空元素的处理

### 2. 性能优化
- 只处理实际发生变更的部分
- 减少内存使用和CPU开销
- 支持大型XML文件的分片处理

### 3. 可维护性
- 清晰的关注点分离
- 易于测试和调试
- 支持增量更新

### 4. 向后兼容
- 保持现有API的兼容性
- 可以逐步迁移现有功能
- 支持新旧架构并存

## 迁移策略

### 第一阶段：核心架构实现
1. 实现IXmlDocumentMerger接口
2. 实现IXElementToDtoMapper接口
3. 实现XmlPatch系统
4. 创建基本的编辑管理器

### 第二阶段：测试验证
1. 为新架构创建完整的测试套件
2. 验证Looknfeel等复杂XML的处理
3. 确保节点和属性数量的一致性
4. 验证差异补丁的正确性

### 第三阶段：逐步迁移
1. 优先迁移失败的测试用例
2. 逐步替换现有的XmlSerializer使用
3. 保持现有DO/DTO架构的兼容性
4. 更新相关文档和示例

## 风险评估

### 技术风险
- **复杂度增加**：新架构增加了系统复杂度
- **性能影响**：需要评估新架构的性能影响
- **兼容性问题**：需要确保与现有代码的兼容性

### 缓解措施
- 分阶段实施，降低风险
- 充分的测试覆盖
- 保持向后兼容性
- 详细的文档和示例

## 总结

这种"弱类型合并 + 强类型编辑"的混合架构能够有效解决当前XML序列化测试失败的问题，同时保持系统的可维护性和可扩展性。通过关注点分离和最小变更原则，可以确保XML结构的一致性和完整性。