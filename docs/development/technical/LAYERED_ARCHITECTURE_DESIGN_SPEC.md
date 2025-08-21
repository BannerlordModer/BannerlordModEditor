# Bannerlord Mod Editor 分层架构设计规范

## 1. 概述

本文档详细描述了Bannerlord Mod Editor项目的分层架构设计规范，该架构基于DO/DTO模式，旨在解决XML序列化中的复杂类型转换问题。

## 2. 架构原则

### 2.1 分层职责分离
- **DO层**（Data Object）: 负责XML序列化，与XML原生格式完全一致
- **DTO层**（Data Transfer Object）: 负责业务逻辑，提供类型安全的便捷属性
- **Mapper层**: 实现DO/DTO之间的双向映射转换

### 2.2 序列化一致性原则
- DO层和DTO层的ShouldSerialize方法必须保持逻辑一致性
- Mapper层专注于数据完整复制，不处理序列化逻辑
- XML序列化行为由各层自身的ShouldSerialize方法控制

## 3. DO层设计规范

### 3.1 属性设计
```csharp
// 所有属性使用字符串类型，与XML原生格式一致
public class ExampleDO 
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
    
    [XmlAttribute("value")]
    public string? Value { get; set; }
    
    [XmlElement("child")]
    public ChildDO? Child { get; set; }
}
```

### 3.2 ShouldSerialize方法设计
```csharp
// 字符串属性：检查是否为null或空
public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

// 对象属性：检查是否为null
public bool ShouldSerializeChild() => Child != null;

// 数值属性：检查是否为null或空
public bool ShouldSerializeCount() => !string.IsNullOrEmpty(Count);
```

### 3.3 集合属性设计
```csharp
[XmlElement("item")]
public List<ItemDO> Items { get; set; } = new List<ItemDO>();

// 集合属性：检查集合是否为null且有元素
public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
```

## 4. DTO层设计规范

### 4.1 属性设计
```csharp
// 与DO层属性一一对应，保持相同名称和类型（字符串）
public class ExampleDTO 
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public ChildDTO? Child { get; set; }
    public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
}
```

### 4.2 ShouldSerialize方法设计
```csharp
// 与DO层保持完全一致的逻辑
public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
public bool ShouldSerializeChild() => Child != null;
public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
```

### 4.3 便捷属性设计
```csharp
// 提供类型安全的数值便捷属性
public int? ValueInt => int.TryParse(Value, out int val) ? val : (int?)null;
public double? ValueDouble => double.TryParse(Value, out double val) ? val : (double?)null;
public bool? ValueBool => bool.TryParse(Value, out bool val) ? val : (bool?)null;

// 提供类型安全的设置方法
public void SetValueInt(int? value) => Value = value?.ToString();
public void SetValueDouble(double? value) => Value = value?.ToString();
public void SetValueBool(bool? value) => Value = value?.ToString().ToLower();
```

## 5. Mapper层设计规范

### 5.1 映射方法签名
```csharp
// DO → DTO 映射
public static ExampleDTO ToDTO(ExampleDO source)
{
    if (source == null) return null;
    
    return new ExampleDTO
    {
        Name = source.Name,
        Value = source.Value,
        Child = ToDTO(source.Child),
        Items = source.Items?.Select(ToDTO).ToList() ?? new List<ItemDTO>()
    };
}

// DTO → DO 映射
public static ExampleDO ToDO(ExampleDTO source)
{
    if (source == null) return null;
    
    return new ExampleDO
    {
        Name = source.Name,
        Value = source.Value,
        Child = ToDO(source.Child),
        Items = source.Items?.Select(ToDO).ToList() ?? new List<ItemDO>()
    };
}
```

### 5.2 空值安全处理
```csharp
// 每个映射方法都必须处理null值
public static ChildDTO ToDTO(ChildDO source)
{
    if (source == null) return null;  // 关键：处理null输入
    
    return new ChildDTO
    {
        // 映射属性...
    };
}
```

## 6. 序列化一致性保证

### 6.1 ShouldSerialize方法一致性
DO层和DTO层的ShouldSerialize方法必须遵循相同的逻辑规则：
- 字符串属性：`!string.IsNullOrEmpty(Property)`
- 对象属性：`Property != null`
- 集合属性：`Property != null && Property.Count > 0`

### 6.2 测试验证
```csharp
[Fact]
public void ShouldSerialize_Consistency_Between_DO_And_DTO()
{
    // Arrange
    var doObj = new ExampleDO { Name = "test", Value = null };
    var dtoObj = new ExampleDTO { Name = "test", Value = null };

    // Act
    var doShouldSerialize = doObj.ShouldSerializeName();
    var dtoShouldSerialize = dtoObj.ShouldSerializeName();

    // Assert
    Assert.Equal(doShouldSerialize, dtoShouldSerialize);
}
```

## 7. 最佳实践

### 7.1 命名规范
- DO层类名以`DO`后缀结尾（如`ParticleSystemsDO`）
- DTO层类名以`DTO`后缀结尾（如`ParticleSystemsDTO`）
- Mapper类名以`Mapper`结尾（如`ParticleSystemsMapper`）

### 7.2 文件组织
```
Models/
├── DO/                 # DO层模型
│   ├── ParticleSystemsDO.cs
│   └── MpItemsDO.cs
├── DTO/                # DTO层模型
│   ├── ParticleSystemsDTO.cs
│   └── MpItemsDTO.cs
Mappers/                # Mapper层
├── ParticleSystemsMapper.cs
└── MpItemsMapper.cs
```

### 7.3 错误处理
```csharp
// Mapper中处理可能的转换错误
public static int? IntFromString(string value)
{
    if (string.IsNullOrEmpty(value)) return null;
    return int.TryParse(value, out int result) ? result : (int?)null;
}
```

## 8. 实施验证

### 8.1 一致性测试
每个分层架构实现都应包含以下测试：
1. **ShouldSerialize一致性测试**：验证DO/DTO层ShouldSerialize方法逻辑一致性
2. **数据完整性测试**：验证Mapper完整复制数据
3. **序列化行为测试**：验证XML序列化正确应用ShouldSerialize控制
4. **完整往返测试**：验证DO→DTO→DO完整转换流程

### 8.2 性能考虑
- 对于大型集合，考虑使用延迟加载
- 对于频繁转换的对象，考虑添加对象池
- 避免在Mapper中进行复杂计算

## 9. 维护指南

### 9.1 添加新属性
1. 在DO层和DTO层同时添加对应的字符串属性
2. 在两个层中添加相应的ShouldSerialize方法
3. 更新Mapper层的映射方法
4. 添加对应的便捷属性到DTO层（如需要）
5. 更新相关测试

### 9.2 修改现有属性
1. 确保DO层和DTO层的ShouldSerialize逻辑保持一致
2. 更新Mapper层的相关映射逻辑
3. 更新测试验证修改

### 9.3 扩展到新XML模型
遵循相同的DO/DTO/Mapper模式：
1. 分析XML结构和属性类型
2. 创建对应的DO层模型
3. 创建对应的DTO层模型
4. 实现Mapper转换逻辑
5. 添加完整测试验证

---

**文档版本**: 1.0
**最后更新**: 2025-08-10
**适用范围**: 所有基于XML的模型适配