# XML适配状态检查工具

## 概述

XML适配状态检查工具是一个用于分析骑马与砍杀2（Bannerlord）XML文件适配状态的命令行工具。它可以扫描TestData目录中的XML文件，检查每个文件是否有对应的C#模型类，并提供详细的适配状态报告。

## 功能特性

- **全面扫描**: 自动扫描指定目录中的所有XML文件
- **适配状态检查**: 检查每个XML文件是否已有对应的C#模型类
- **复杂度分析**: 分析文件复杂度（Simple、Medium、Complex、Large）
- **统计报告**: 生成详细的适配统计信息
- **多种输出格式**: 支持控制台、Markdown、CSV、JSON等输出格式
- **并行处理**: 支持并行处理以提高性能
- **配置管理**: 灵活的配置系统

## 安装和构建

### 前置条件

- .NET 9.0 SDK
- BannerlordModEditor项目源代码

### 构建项目

```bash
cd scripts/XmlAdaptationChecker
dotnet build
```

## 使用方法

### 基本命令

#### 1. 初始化配置文件

```bash
dotnet run -- config init
```

这将创建一个默认的`appsettings.json`配置文件。

#### 2. 执行适配状态检查

```bash
dotnet run -- check
```

执行完整的XML适配状态检查并显示结果。

#### 3. 显示摘要信息

```bash
dotnet run -- summary
```

显示适配状态的简要摘要。

### 配置文件

配置文件`appsettings.json`包含以下设置：

```json
{
  "xmlDirectory": "../../BannerlordModEditor.Common.Tests/TestData",
  "outputDirectory": "./output",
  "modelDirectories": [
    "../../BannerlordModEditor.Common/Models",
    "../../BannerlordModEditor.Common/Models/DO",
    "../../BannerlordModEditor.Common/Models/DTO"
  ],
  "outputFormats": ["Console", "Markdown", "Csv", "Json"],
  "verboseLogging": false,
  "enableParallelProcessing": true,
  "maxParallelism": 16,
  "fileSizeThreshold": 1048576,
  "analyzeComplexity": true,
  "generateStatistics": true,
  "excludePatterns": ["*.backup", "*.tmp"]
}
```

### 配置参数说明

- **xmlDirectory**: XML文件目录路径
- **outputDirectory**: 输出文件目录
- **modelDirectories**: C#模型目录列表
- **outputFormats**: 输出格式列表
- **verboseLogging**: 是否启用详细日志
- **enableParallelProcessing**: 是否启用并行处理
- **maxParallelism**: 最大并行度
- **fileSizeThreshold**: 文件大小阈值（字节）
- **analyzeComplexity**: 是否分析文件复杂度
- **generateStatistics**: 是否生成统计信息
- **excludePatterns**: 要排除的文件模式

## 输出示例

### 控制台输出

```
开始XML适配状态检查...
┌──────────┬──────┐
│ 指标     │ 数值 │
├──────────┼──────┤
│ 总文件数 │ 102  │
│ 已适配   │ 0    │
│ 未适配   │ 102  │
│ 适配率   │ 0.0% │
└──────────┴──────┘

未适配文件:
┌──────────────────────────────────────┬─────────┬────────┐
│ 文件名                               │ 大小    │ 复杂度 │
├──────────────────────────────────────┼─────────┼────────┤
│ particle_systems_hardcoded_misc1.xml │ 1.7 MB  │ Large  │
│ flora_kinds.xml                      │ 1.5 MB  │ Large  │
│ terrain_materials.xml                │ 15.8 KB │ Medium │
└──────────────────────────────────────┴─────────┴────────┘
检查完成！
```

### 摘要输出

```
XML适配状态摘要 (2025-08-22 04:00:52)
==================================================
总文件数: 102
已适配: 0
未适配: 102
适配率: 0.0%

未适配文件按复杂度:
  Large: 3 个文件
  Complex: 33 个文件
  Medium: 21 个文件
  Simple: 45 个文件
```

## 复杂度分类

工具根据文件大小和结构复杂度将文件分为以下几类：

- **Simple**: 简单结构，少量属性
- **Medium**: 中等复杂度，嵌套结构
- **Complex**: 复杂结构，深度嵌套
- **Large**: 大型文件，需要分块处理

## 集成现有服务

工具集成了BannerlordModEditor项目中的以下服务：

- **FileDiscoveryService**: 文件发现和适配状态检查
- **NamingConventionMapper**: 命名约定转换
- **现有DO/DTO架构**: 遵循项目的架构模式

## 架构设计

### 核心组件

1. **XmlAdaptationChecker**: 核心检查引擎
2. **AdaptationConfigurationManager**: 配置管理
3. **AdaptationCheckerCLI**: 命令行界面
4. **FileDiscoveryService**: 文件发现服务（来自Common项目）

### 项目结构

```
scripts/XmlAdaptationChecker/
├── XmlAdaptationChecker.csproj          # 项目文件
├── Program.cs                           # 程序入口
├── AdaptationCheckerCLI.cs              # 命令行界面
├── AdaptationCheckerConfiguration.cs    # 配置类
├── Core/
│   └── XmlAdaptationChecker.cs          # 核心检查引擎
└── Configuration/
    └── AdaptationConfigurationManager.cs # 配置管理器
```

## 性能优化

- **并行处理**: 支持多线程并行处理XML文件
- **内存管理**: 优化大文件处理，避免内存溢出
- **异步IO**: 使用异步文件操作提高性能
- **智能缓存**: 合理的缓存策略

## 错误处理

工具提供完善的错误处理机制：

- 配置验证失败
- 文件访问权限问题
- XML解析错误
- 服务依赖问题

## 扩展性

工具设计具有良好的扩展性：

- 可以轻松添加新的输出格式
- 支持自定义复杂度分析算法
- 可扩展的配置系统
- 模块化的组件设计

## 许可证

本工具遵循BannerlordModEditor项目的许可证。

## 贡献

欢迎提交Issue和Pull Request来改进这个工具。