# BannerlordModEditor

骑马与砍杀2（Bannerlord）的Mod编辑实用工具

## 项目概述

这是一个骑马与砍杀2（Bannerlord）的Mod编辑器工具，使用C#和.NET 9开发。项目采用现代化的桌面应用架构，主要功能是处理和编辑骑砍2的XML配置文件。

## 最新更新

我们已成功实现了DO/DTO分层架构模式，以解决XML序列化测试失败的问题。所有核心XML模型都已重构完成，测试通过率达到95%以上。

### DO/DTO架构模式

项目采用了DO/DTO分层架构来解决XML序列化问题：

- **DO层 (Domain Object)**: 领域对象，包含业务逻辑和运行时标记
- **DTO层 (Data Transfer Object)**: 数据传输对象，专门用于XML序列化
- **Mapper层**: 对象映射器，负责DO和DTO之间的双向转换

这种架构实现了关注点分离，提供了对XML序列化行为的精确控制。

## 快速开始

### 环境要求

- .NET 9.0 SDK
- 支持.NET的IDE（Visual Studio、VS Code、JetBrains Rider等）

### 构建和运行

```bash
# 克隆仓库
git clone <repository-url>
cd BannerlordModEditor

# 还原依赖
dotnet restore

# 构建解决方案
dotnet build

# 运行所有测试
dotnet test

# 启动应用程序
dotnet run --project BannerlordModEditor.UI
```

## 项目结构

```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/           # 核心业务逻辑层
│   ├── Models/                          # 数据模型
│   │   ├── DO/                         # 领域对象
│   │   ├── DTO/                        # 数据传输对象
│   │   ├── Data/                       # 原始数据模型（兼容性）
│   │   ├── Audio/                      # 音频系统
│   │   ├── Engine/                     # 游戏引擎
│   │   ├── Game/                       # 游戏机制
│   │   ├── Multiplayer/                # 多人游戏
│   │   └── Configuration/              # 游戏配置
│   ├── Services/                       # 核心服务
│   ├── Mappers/                        # 数据映射器
│   └── Loaders/                        # 数据加载器
├── BannerlordModEditor.UI/               # UI表现层 (Avalonia)
│   ├── ViewModels/                     # MVVM视图模型
│   ├── Views/                          # Avalonia XAML视图
│   └── Assets/                         # UI资源
├── BannerlordModEditor.Common.Tests/     # Common层测试
│   ├── TestData/                       # XML测试数据
│   ├── TestSubsets/                    # 大型XML文件分片
│   └── Comprehensive/                  # 综合测试
├── BannerlordModEditor.UI.Tests/         # UI层测试
├── docs/                                # 项目文档
└── example/                            # 参考示例
```

## 核心功能

### XML适配系统

项目实现了一个完整的XML适配系统，将骑砍2的各种XML配置文件转换为C#强类型模型：

1. **文件发现**: `FileDiscoveryService`扫描XML目录，识别未适配的文件
2. **命名映射**: `NamingConventionMapper`处理XML文件名到C#类名的转换
3. **模型生成**: 基于XML结构生成对应的C#模型类
4. **序列化**: `GenericXmlLoader`处理XML的读写操作

### 支持的XML类型

项目支持骑砍2的多种XML配置文件类型：

- **游戏机制**: ActionTypes、CombatParameters、ItemModifiers、Crafting等
- **音频系统**: SoundFiles、ModuleSounds、VoiceDefinitions等
- **物理引擎**: PhysicsMaterials、CollisionInfos等
- **多人游戏**: MpItems、MpCultures、MultiplayerScenes等
- **地图系统**: MapIcons、TerrainMaterials、FloraKinds等
- **UI系统**: Looknfeel、Credits等

### 测试系统

项目拥有完善的测试体系：

- **单元测试**: 每个XML类型都有对应的测试类
- **集成测试**: 验证完整的XML处理流程
- **分片测试**: 支持大型XML文件的分片测试
- **往返测试**: 确保XML序列化和反序列化的数据一致性

## 开发指南

### 添加新的XML适配

1. **创建模型类**:
   ```csharp
   // 在Models/相应功能域创建DO和DTO类
   public class NewXmlTypeDO { ... }
   public class NewXmlTypeDTO { ... }
   ```

2. **创建映射器**:
   ```csharp
   public static class NewXmlTypeMapper
   {
       public static NewXmlTypeDTO ToDTO(NewXmlTypeDO source) { ... }
       public static NewXmlTypeDO ToDO(NewXmlTypeDTO source) { ... }
   }
   ```

3. **添加测试**:
   ```csharp
   [Fact]
   public void NewXmlTypeXmlTests()
   {
       // 添加XML序列化/反序列化测试
   }
   ```

### 测试策略

- **所有XML适配都需要对应的单元测试**
- **测试数据使用真实的骑砍2XML文件**
- **大型XML文件支持分片测试以避免性能问题**
- **往返测试确保数据完整性**

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

## 测试状态

### 当前测试结果

- **测试总数**: 1043个
- **通过数**: 991个
- **失败数**: 50个
- **跳过数**: 2个
- **通过率**: 95%

### 测试命令

```bash
# 运行所有测试
dotnet test

# 运行Common层测试
dotnet test BannerlordModEditor.Common.Tests --verbosity normal

# 运行UI层测试
dotnet test BannerlordModEditor.UI.Tests --verbosity normal

# 运行特定测试方法
dotnet test --filter "TestName"

# 生成代码覆盖率报告
dotnet test --collect:"XPlat Code Coverage"
```

## 质量保证

### XML适配注意事项

- **严格字段匹配**: XML文档中存在的字段就是要存在的，不存在的字段就不能存在
- **空元素处理**: 严格区分字段不存在和字段为空的情况
- **数据完整性**: 确保往返测试不会丢失或增加数据

### 性能优化

- **异步处理**: 大型XML文件采用异步处理
- **并行处理**: 文件发现服务支持并行处理
- **内存管理**: 测试数据按需加载以减少内存占用

## 贡献指南

1. Fork 本仓库
2. 创建功能分支 (`git checkout -b feature/new-feature`)
3. 提交更改 (`git commit -am 'Add new feature'`)
4. 推送到分支 (`git push origin feature/new-feature`)
5. 创建Pull Request

## 许可证

本项目采用MIT许可证。详情请参阅[LICENSE](LICENSE)文件。

## 文档

更多详细信息请参考以下文档：

- [架构设计](docs/architecture.md)
- [技术规范](docs/tech-stack.md)
- [用户故事](docs/user-stories.md)
- [API文档](docs/api-spec.md)
- [测试指南](docs/testing-guide.md)

## 联系方式

如有问题或建议，请通过以下方式联系：

- 创建Issue
- 发送Pull Request
- 参与讨论

---

**注意**: 本项目仅用于骑马与砍杀2游戏的Mod开发，请遵守游戏的相关条款和条件。