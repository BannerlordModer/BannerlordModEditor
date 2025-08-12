# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个骑马与砍杀2（Bannerlord）的Mod编辑器工具，使用C#和.NET 9开发。项目采用现代化的桌面应用架构，主要功能是处理和编辑骑砍2的XML配置文件。

## 构建和开发命令

### 常用命令
```bash
# 构建整个解决方案
dotnet build

# 运行所有测试
dotnet test

# 运行特定项目的测试
dotnet test BannerlordModEditor.Common.Tests
dotnet test BannerlordModEditor.UI.Tests

# 运行UI应用程序
dotnet run --project BannerlordModEditor.UI

# 清理解决方案
dotnet clean

# 还原依赖
dotnet restore
```

### 测试命令
```bash
# 运行Common层测试
dotnet test BannerlordModEditor.Common.Tests --verbosity normal

# 运行UI层测试  
dotnet test BannerlordModEditor.UI.Tests --verbosity normal

# 运行特定测试方法
dotnet test --filter "TestName"
```

## 核心架构

### 解决方案结构
```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/           # 核心业务逻辑层
├── BannerlordModEditor.UI/               # UI表现层 (Avalonia)
├── BannerlordModEditor.Common.Tests/     # Common层测试
└── BannerlordModEditor.UI.Tests/         # UI层测试
```

### 主要架构组件

#### 1. Common层 (BannerlordModEditor.Common)
**核心职责**: XML数据模型、文件处理、业务逻辑

**关键组件**:
- **Models/**: XML数据模型，按功能域组织命名空间
  - `Audio/`: 音频相关配置 (SoundFiles, ModuleSounds等)
  - `Engine/`: 引擎核心参数 (CoreParameters, Physics等)
  - `Configuration/`: 游戏配置 (Credits, ManagedParameters等)
  - `Data/`: 数据定义 (AchievementData, BannerIcons等)
  - `Game/`: 游戏机制 (Crafting, ItemModifiers等)
  - `Multiplayer/`: 多人游戏相关 (MpItems, MpCultures等)
  - 其他功能域模型...

- **Services/**: 核心服务
  - `FileDiscoveryService`: XML文件发现和适配状态检查
  - `NamingConventionMapper`: 命名约定转换
  - `IFileDiscoveryService`: 文件发现服务接口

- **Loaders/**: 数据加载器
  - `GenericXmlLoader<T>`: 通用XML序列化/反序列化器

#### 2. UI层 (BannerlordModEditor.UI)
**技术栈**: Avalonia UI + MVVM模式
**核心职责**: 用户界面和交互

**关键组件**:
- **ViewModels/**: MVVM视图模型
  - `EditorManagerViewModel`: 编辑器管理逻辑
  - `MainWindowViewModel`: 主窗口逻辑
  - `EditorCategoryViewModel`: 编辑分类逻辑

- **Views/**: Avalonia XAML视图
  - `MainWindow.axaml`: 主窗口界面

#### 3. 测试架构
**测试框架**: xUnit
**测试数据**: 大量XML测试文件在`TestData/`目录

**测试组织**:
- 每个XML类型都有对应的测试类 (如`ActionTypesXmlTests.cs`)
- 测试数据按功能分类存储
- 支持大型XML文件的分片测试

### XML适配系统

#### 核心机制
项目实现了一个完整的XML适配系统，将骑砍2的各种XML配置文件转换为C#强类型模型：

1. **文件发现**: `FileDiscoveryService`扫描XML目录，识别未适配的文件
2. **命名映射**: `NamingConventionMapper`处理XML文件名到C#类名的转换
3. **模型生成**: 基于XML结构生成对应的C#模型类
4. **序列化**: `GenericXmlLoader`处理XML的读写操作

#### 命名空间策略
模型按功能域组织，避免单一巨大命名空间：
- `BannerlordModEditor.Common.Models.Audio` - 音频系统
- `BannerlordModEditor.Common.Models.Engine` - 游戏引擎
- `BannerlordModEditor.Common.Models.Game` - 游戏机制
- `BannerlordModEditor.Common.Models.Multiplayer` - 多人游戏
- 等等...

### 关键设计模式

#### 1. 服务层模式
- `IFileDiscoveryService`接口定义契约
- `FileDiscoveryService`提供具体实现
- 依赖注入 ready 设计

#### 2. 泛型模式
- `GenericXmlLoader<T>`提供类型安全的XML处理
- 支持任意XML模型类型的序列化

#### 3. MVVM模式
- UI层采用标准的MVVM架构
- `CommunityToolkit.Mvvm`提供MVVM基础功能
- 数据绑定和命令处理

## 开发指南

### 添加新的XML适配
1. 在`Models/`相应功能域创建新的模型类
2. 继承适当的基类或实现接口
3. 在`BannerlordModEditor.Common.Tests/`添加对应的测试
4. 更新`FileDiscoveryService`的命名映射（如需要）

### 测试策略
- 所有XML适配都需要对应的单元测试
- 测试数据使用真实的骑砍2XML文件
- 大型XML文件支持分片测试以避免性能问题

### 代码规范
- 使用C# 9.0特性和模式匹配
- 启用Nullable引用类型
- 遵循现有的命名约定
- XML注释用于公共API文档

## 技术栈

### 核心技术
- **.NET 9.0**: 最新.NET平台
- **Avalonia UI 11.3**: 跨平台桌面UI框架
- **xUnit 2.5**: 单元测试框架
- **CommunityToolkit.Mvvm 8.2**: MVVM工具包

### 依赖包
- `Velopack`: 应用程序更新和打包
- `Avalonia.Themes.Fluent`: Fluent UI主题
- `coverlet.collector`: 代码覆盖率

## 特殊注意事项

### XML处理
- 项目处理大量骑砍2配置文件
- XML序列化保持原有格式和缩进
- 支持UTF-8编码和国际化字符

### 性能考虑
- 大型XML文件采用异步处理
- 文件发现服务支持并行处理
- 测试数据按需加载以减少内存占用

### 命名约定
- XML文件名使用下划线分隔 (如`action_types.xml`)
- C#类名使用PascalCase (如`ActionTypes.cs`)
- 命名空间按功能域分层组织

### XML适配注意事项
- 适配XML的时候要注意，XML文档中存在的字段就是要存在的，不存在的字段就不能存在，要严格区分字段或属性不存在以及字段或属性为空的情况，因为要面对一个随时可能因为XML多了一点东西就导致崩溃的解析器

## DO/DTO 模式转换经验总结

### 背景

在 BannerlordModEditor 项目中，为了解决 XML 序列化测试失败的问题，我采用 DO/DTO 架构模式重构了多个数据模型。这篇章节总结了转换过程中的经验、模式和最佳实践。

### 问题分析

#### 原始问题
- 大量 XML 序列化测试失败（如 CombatParameters、ItemHolsters、ActionTypes 等）
- XML 结构在反序列化和再序列化后出现结构差异
- 空元素的处理不当导致序列化结果与原始 XML 不匹配

#### 根本原因
1. **直接的数据模型与 XML 序列化耦合过紧**
2. **缺乏对空元素的精确控制**
3. **业务逻辑与数据表示混合**

### 解决方案：DO/DTO 架构模式

#### 模式定义
- **DO (Domain Object)**: 领域对象，表示业务逻辑和数据结构
- **DTO (Data Transfer Object)**: 数据传输对象，专门用于序列化/反序列化
- **Mapper**: 对象映射器，负责 DO 和 DTO 之间的转换

#### 架构优势
1. **关注点分离**: 业务逻辑与数据表示分离
2. **精确控制**: 可以对序列化行为进行细粒度控制
3. **可维护性**: 易于修改和扩展
4. **测试友好**: 便于单元测试和集成测试

### 实施模式

#### 1. 文件组织结构
```
BannerlordModEditor.Common/
├── Models/
│   ├── DO/              # 领域对象
│   │   ├── CombatParametersDO.cs
│   │   ├── ActionTypesDO.cs
│   │   └── ...
│   ├── DTO/             # 数据传输对象
│   │   ├── CombatParametersDTO.cs
│   │   ├── ActionTypesDTO.cs
│   │   └── ...
│   └── Data/            # 原始数据模型（兼容性）
│       ├── CombatParameters.cs
│       ├── ActionTypes.cs
│       └── ...
├── Mappers/
│   ├── CombatParametersMapper.cs
│   ├── ActionTypesMapper.cs
│   └── ...
└── Tests/
    └── ... (更新为使用 DO 层)
```

#### 2. DO 模型实现要点
```csharp
[XmlRoot("base")]
public class CombatParametersDO
{
    [XmlElement("definitions")]
    public DefinitionsDO Definitions { get; set; } = new DefinitionsDO();
    
    [XmlIgnore]
    public bool HasDefinitions { get; set; } = false;  // 标记是否应该序列化
    
    [XmlArray("combat_parameters")]
    [XmlArrayItem("combat_parameter")]
    public List<BaseCombatParameterDO> CombatParametersList { get; set; } = new List<BaseCombatParameterDO>();
    
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeDefinitions() => HasDefinitions && Definitions != null && Definitions.Defs.Count > 0;
    public bool ShouldSerializeCombatParametersList() => CombatParametersList != null && CombatParametersList.Count > 0;
}
```

#### 3. DTO 模型实现要点
```csharp
public class CombatParametersDTO
{
    // 与 DO 结构相同，但不包含业务逻辑
    // 专门用于序列化/反序列化
}
```

#### 4. Mapper 实现模式
```csharp
public static class CombatParametersMapper
{
    public static CombatParametersDTO ToDTO(CombatParametersDO source)
    {
        if (source == null) return null;
        
        return new CombatParametersDTO
        {
            // 属性映射
            Type = source.Type,
            Definitions = DefinitionsMapper.ToDTO(source.Definitions),
            CombatParametersList = source.CombatParametersList?
                .Select(CombatParametersMapper.ToDTO)
                .ToList() ?? new List<BaseCombatParameterDTO>()
        };
    }
    
    public static CombatParametersDO ToDO(CombatParametersDTO source)
    {
        if (source == null) return null;
        
        return new CombatParametersDO
        {
            // 反向映射
            Type = source.Type,
            Definitions = DefinitionsMapper.ToDO(source.Definitions),
            CombatParametersList = source.CombatParametersList?
                .Select(CombatParametersMapper.ToDO)
                .ToList() ?? new List<BaseCombatParameterDO>()
        };
    }
}
```

#### 5. 特殊处理逻辑
对于某些复杂的 XML 结构，需要在反序列化时添加特殊处理：

```csharp
// 在 XmlTestUtils.Deserialize<T>() 中添加
if (obj is CombatParametersDO combatParams)
{
    var doc = XDocument.Parse(xml);
    combatParams.HasDefinitions = doc.Root?.Element("definitions") != null;
    var combatParamsElement = doc.Root?.Element("combat_parameters");
    combatParams.HasEmptyCombatParameters = combatParamsElement != null && 
        (combatParamsElement.Elements().Count() == 0 || combatParamsElement.Elements("combat_parameter").Count() == 0);
}
```

### 关键经验总结

#### 1. 序列化控制
- **ShouldSerialize 方法**: 精确控制哪些属性应该被序列化
- **XmlIgnore 属性**: 将运行时标记属性排除在序列化之外
- **空元素处理**: 使用标记属性来保留原始 XML 结构中的空元素

#### 2. 数据验证
- **null 检查**: 在映射器方法中进行彻底的 null 检查
- **空集合处理**: 确保空集合被正确初始化，避免 NullReferenceException
- **类型安全**: 保持 DO 和 DTO 之间的类型一致性

#### 3. 向后兼容性
- **保留原始模型**: 不删除原有的 Data 层模型，确保现有代码不受影响
- **渐进式迁移**: 可以逐步将测试和业务逻辑迁移到 DO 层
- **测试更新**: 只需更新测试文件中的 using 语句和类型引用

#### 4. 测试策略
- **直接替换**: 测试文件中直接将 Data 类型替换为 DO 类型
- **结构验证**: 使用现有的 XmlTestUtils.AreStructurallyEqual 进行验证
- **调试支持**: 必要时创建临时的调试测试来诊断问题

### 转换检查清单

#### 准备阶段
- [ ] 分析失败的 XML 序列化测试
- [ ] 确认现有数据模型结构
- [ ] 设计 DO/DTO 层次结构

#### 实施阶段
- [ ] 创建 DO 模型文件
- [ ] 创建 DTO 模型文件
- [ ] 创建 Mapper 类
- [ ] 更新测试文件引用
- [ ] 添加特殊处理逻辑（如果需要）

#### 验证阶段
- [ ] 运行目标测试验证通过
- [ ] 确保没有回归问题
- [ ] 提交代码并创建 Git 记录

### 已成功转换的模型

1. **CombatParameters** - 包含 definitions 和 combat_parameters 的复杂结构
2. **ActionTypes** - 包含多个子类型的列表结构
3. **BoneBodyTypes** - 包含枚举值和复杂属性
4. **ActionSets** - 包含动画和动作的复杂关系
5. **CollisionInfos** - 包含碰撞检测的复杂配置
6. **MapIcons** - 包含地图图标的多层嵌套结构

### 常见问题及解决方案

#### 问题 1: 空元素被省略
**现象**: 原始 XML 中的空元素在序列化后被省略
**解决方案**: 添加标记属性和对应的 ShouldSerialize 方法

#### 问题 2: 节点数量不匹配
**现象**: 序列化前后的 XML 节点数量不一致
**解决方案**: 使用 XDocument 分析原始结构，添加相应的空元素处理逻辑

#### 问题 3: 属性顺序变化
**现象**: XML 属性在序列化后顺序改变
**解决方案**: 使用 XmlAttribute 而不是 XmlElement 来保持属性顺序

#### 问题 4: 命名空间丢失
**现象**: 序列化后 XML 命名空间信息丢失
**解决方案**: 在 XmlTestUtils.Serialize 方法中传递原始 XML 以保留命名空间

### 未来建议

#### 扩展性考虑
- 可以考虑使用代码生成工具来自动生成 DO/DTO 模型
- 建立统一的映射器基类来简化映射逻辑
- 添加更多的验证和错误处理逻辑

#### 性能优化
- 对于大型 XML 文件，可以考虑使用流式处理
- 缓存频繁使用的映射器实例
- 优化集合操作的内存使用

#### 维护性改进
- 添加更多的文档和注释
- 建立单元测试覆盖映射逻辑
- 定期审查和重构代码结构

### 结论

DO/DTO 模式在 BannerlordModEditor 项目中的成功应用证明了这种架构模式在处理复杂 XML 序列化问题时的有效性。通过关注点分离和精确控制，我们能够解决各种序列化测试失败的问题，同时保持代码的可维护性和可扩展性。

这种模式不仅适用于当前项目，也可以作为其他类似项目的参考模板。关键在于理解业务需求与数据表示之间的区别，并通过适当的架构设计来实现两者的分离。