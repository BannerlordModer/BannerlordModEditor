# BannerlordModEditor XML适配系统架构设计

## 概述

本文档详细描述了BannerlordModEditor项目中XML适配系统的完整架构设计，该系统采用DO/DTO（Domain Object/Data Transfer Object）架构模式，用于处理骑马与砍杀2（Bannerlord）游戏的各种XML配置文件。

## 系统目标

### 主要目标
1. **精确的XML序列化控制**：确保序列化后的XML与原始XML在结构和内容上完全一致
2. **类型安全的数据访问**：提供强类型的API来访问和操作XML数据
3. **关注点分离**：将业务逻辑与数据表示分离，提高代码的可维护性
4. **可扩展性**：支持新XML类型的快速适配和集成

### 技术目标
- 支持大型XML文件的高效处理
- 提供完整的空元素和可选属性处理
- 保持向后兼容性
- 支持复杂的嵌套XML结构

## 核心架构模式

### DO/DTO架构模式

#### 模式定义
- **DO (Domain Object)**：领域对象，包含业务逻辑和领域规则
- **DTO (Data Transfer Object)**：数据传输对象，专门用于序列化/反序列化
- **Mapper**：对象映射器，负责DO和DTO之间的转换

#### 架构优势
1. **关注点分离**：业务逻辑与数据表示完全分离
2. **精确控制**：可以对序列化行为进行细粒度控制
3. **可维护性**：易于修改和扩展
4. **测试友好**：便于单元测试和集成测试
5. **性能优化**：可以针对特定场景进行优化

#### 数据流向
```
XML文件 <-> GenericXmlLoader<T> <-> DTO <-> Mapper <-> DO <-> 业务逻辑
```

## 系统架构组件

### 1. 数据模型层 (Models)

#### DO层 (Domain Objects)
**位置**：`BannerlordModEditor.Common/Models/DO/`

**职责**：
- 包含业务逻辑和验证规则
- 提供类型安全的属性访问
- 实现ShouldSerialize方法进行序列化控制
- 包含运行时标记属性（如HasEmptyXXX）

**关键特性**：
```csharp
[XmlRoot("base")]
public class BannerIconsDO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }
    
    [XmlElement("BannerIconData")]
    public BannerIconDataDO? BannerIconData { get; set; }
    
    [XmlIgnore]
    public bool HasBannerIconData { get; set; } = false;
    
    public bool ShouldSerializeBannerIconData() => HasBannerIconData && BannerIconData != null;
}
```

#### DTO层 (Data Transfer Objects)
**位置**：`BannerlordModEditor.Common/Models/DTO/`

**职责**：
- 专门用于XML序列化/反序列化
- 提供便捷属性和类型转换方法
- 包含ShouldSerialize方法
- 不包含业务逻辑

**关键特性**：
```csharp
[XmlRoot("base")]
public class BannerIconsDTO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }
    
    [XmlElement("BannerIconData")]
    public BannerIconDataDTO? BannerIconData { get; set; }

    // 便捷属性
    public bool HasType => !string.IsNullOrEmpty(Type);
    
    // 类型安全的便捷属性
    public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
    
    // 设置方法
    public void SetIdInt(int? value) => Id = value?.ToString();
}
```

#### Data层 (向后兼容)
**位置**：`BannerlordModEditor.Common/Models/Data/`

**职责**：
- 保持与现有代码的兼容性
- 作为DO层实现前的临时模型

### 2. 映射器层 (Mappers)

**位置**：`BannerlordModEditor.Common/Mappers/`

**职责**：
- 实现DO和DTO之间的双向转换
- 处理null引用和空集合
- 确保数据完整性

**设计模式**：
```csharp
public static class BannerIconsMapper
{
    public static BannerIconsDTO ToDTO(BannerIconsDO source)
    {
        if (source == null) return null;
        
        return new BannerIconsDTO
        {
            Type = source.Type,
            BannerIconData = ToDTO(source.BannerIconData)
        };
    }
    
    public static BannerIconsDO ToDO(BannerIconsDTO source)
    {
        if (source == null) return null;
        
        return new BannerIconsDO
        {
            Type = source.Type,
            BannerIconData = ToDO(source.BannerIconData),
            HasBannerIconData = source.BannerIconData != null
        };
    }
}
```

### 3. 服务层 (Services)

**位置**：`BannerlordModEditor.Common/Services/`

**主要组件**：
- **FileDiscoveryService**：XML文件发现和适配状态检查
- **NamingConventionMapper**：命名约定转换
- **GenericXmlLoader<T>**：通用XML序列化/反序列化器

## XML适配策略

### 1. 序列化控制策略

#### ShouldSerialize方法
- 精确控制哪些属性应该被序列化
- 处理空值和可选属性
- 保持XML结构的完整性

#### 标记属性模式
- 使用`[XmlIgnore]`标记运行时属性
- 在反序列化时设置标记状态
- 在序列化时根据标记决定输出

```csharp
[XmlIgnore]
public bool HasEmptyBannerIconGroups { get; set; } = false;

public bool ShouldSerializeBannerIconGroups() => 
    HasEmptyBannerIconGroups || (BannerIconGroups != null && BannerIconGroups.Count > 0);
```

### 2. 空元素处理

#### 保留空元素
- 使用标记属性记录原始XML中的空元素
- 在序列化时重新生成空元素
- 确保与原始XML结构一致

#### 特殊处理逻辑
- 在XmlTestUtils.Deserialize<T>()中添加特殊处理
- 使用XDocument分析原始结构
- 设置相应的标记属性

### 3. 类型安全策略

#### 字符串到类型转换
- 在DTO层提供类型安全的便捷属性
- 实现安全的类型转换方法
- 提供类型安全的设置方法

```csharp
// 类型安全的便捷属性
public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;

// 设置方法
public void SetIdInt(int? value) => Id = value?.ToString();
```

## 具体XML类型适配设计

### 1. BannerIcons适配

#### XML结构特点
- 根元素：`<base type="string">`
- 主要元素：`<BannerIconData>`
- 嵌套结构：`<BannerIconGroup>` → `<Background>`/`<Icon>`
- 颜色数据：`<BannerColors>` → `<Color>`

#### DO/DTO设计要点
- 处理多层嵌套结构
- 支持可选的BannerColors元素
- 提供类型转换的便捷属性

#### 关键组件
```csharp
// DO层
public class BannerIconGroupDO
{
    [XmlAttribute("id")]
    public string? Id { get; set; }
    
    [XmlAttribute("is_pattern")]
    public string? IsPattern { get; set; }
    
    [XmlElement("Background")]
    public List<BackgroundDO> Backgrounds { get; set; } = new List<BackgroundDO>();
    
    [XmlElement("Icon")]
    public List<IconDO> Icons { get; set; } = new List<IconDO>();
    
    // 类型安全的便捷属性
    public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
    public bool? IsPatternBool => bool.TryParse(IsPattern, out bool pattern) ? pattern : (bool?)null;
}
```

### 2. ItemModifiers适配

#### XML结构特点
- 根元素：`<ItemModifiers>`
- 扁平结构：`<ItemModifier>`列表
- 多属性：每个ItemModifier包含多个属性
- 数据类型混合：包含int、float、string等多种类型

#### DO/DTO设计要点
- 处理属性数量不匹配问题
- 提供类型转换的双向绑定
- 支持可选属性的精确控制

#### 关键组件
```csharp
// DO层 - 双向绑定属性
public class ItemModifierDO
{
    [XmlAttribute("damage")]
    public string? DamageString
    {
        get => Damage.HasValue ? Damage.Value.ToString() : null;
        set => Damage = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
    }
    [XmlIgnore]
    public int? Damage { get; set; }
}
```

### 3. ParticleSystems适配

#### XML结构特点
- 根元素：`<particle_effects>`
- 深度嵌套：Effect → Emitters → Emitter → (Children, Flags, Parameters)
- 递归结构：Children可以包含更多的Emitter
- 复杂参数：包含曲线、颜色、alpha等复杂数据类型

#### DO/DTO设计要点
- 处理递归嵌套结构
- 支持复杂的参数类型
- 确保Children元素的完整映射

#### 关键组件
```csharp
// DO层 - 递归结构
public class EmitterDO
{
    [XmlElement("children")]
    public ChildrenDO? Children { get; set; }
    
    [XmlElement("flags")]
    public ParticleFlagsDO? Flags { get; set; }
    
    [XmlElement("parameters")]
    public ParametersDO? Parameters { get; set; }
}
```

## 性能优化策略

### 1. 内存管理
- 使用null引用类型减少内存占用
- 延迟加载大型XML文件
- 及时释放不再使用的资源

### 2. 处理效率
- 使用并行处理提高性能
- 优化集合操作
- 缓存频繁使用的映射器实例

### 3. 大型文件支持
- 支持分片处理大型XML文件
- 使用流式处理减少内存占用
- 提供进度反馈机制

## 错误处理机制

### 1. 数据验证
- 在DO层实现业务规则验证
- 在DTO层实现数据格式验证
- 提供详细的错误信息

### 2. 异常处理
- 统一的异常处理策略
- 优雅的错误恢复机制
- 详细的日志记录

### 3. 调试支持
- 提供丰富的调试信息
- 支持XML结构的可视化比较
- 集成测试工具

## 测试策略

### 1. 单元测试
- 每个DO/DTO类都有对应的单元测试
- 测试所有ShouldSerialize方法
- 验证类型转换的正确性

### 2. 集成测试
- XML序列化/反序列化的往返测试
- 节点和属性数量的一致性验证
- 结构等价性验证

### 3. 性能测试
- 大型XML文件的处理性能测试
- 内存使用情况测试
- 并发处理能力测试

## 扩展性设计

### 1. 新XML类型适配
- 标准化的DO/DTO模板
- 自动化的映射器生成
- 统一的测试模式

### 2. 功能扩展
- 支持XML验证和转换
- 提供批量处理功能
- 支持XML格式转换

### 3. 插件架构
- 支持第三方XML适配器
- 可扩展的验证规则
- 自定义序列化策略

## 部署和维护

### 1. 版本管理
- 语义化版本控制
- 向后兼容性保证
- 迁移路径规划

### 2. 文档和培训
- 完整的API文档
- 开发者指南
- 最佳实践文档

### 3. 监控和反馈
- 性能监控指标
- 错误日志分析
- 用户反馈收集

## 总结

BannerlordModEditor的XML适配系统采用DO/DTO架构模式，成功解决了骑马与砍杀2游戏XML配置文件处理的复杂性问题。该系统具有以下核心优势：

1. **精确控制**：通过ShouldSerialize方法和标记属性，实现对XML序列化的精确控制
2. **类型安全**：提供强类型的API和便捷的类型转换方法
3. **可维护性**：清晰的关注点分离和模块化设计
4. **可扩展性**：标准化的架构模式支持快速适配新的XML类型
5. **性能优化**：针对大型XML文件和复杂嵌套结构的优化策略

该架构不仅解决了当前的技术问题，还为未来的功能扩展和性能优化奠定了坚实的基础。通过持续的改进和优化，该系统将能够更好地服务于骑马与砍杀2模组开发社区。