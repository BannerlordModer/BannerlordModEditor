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