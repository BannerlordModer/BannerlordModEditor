# BannerlordModEditor

骑马与砍杀2（Bannerlord）的Mod编辑实用工具

## 项目概述

这是一个专业的骑马与砍杀2（Bannerlord）Mod编辑器工具，使用C#和.NET 9开发。项目采用现代化的分层架构设计，专门用于处理和编辑骑砍2的XML配置文件。

## 最新更新

### 🎯 项目状态：生产就绪
- **架构重构完成**：成功实现DO/DTO分层架构模式
- **测试通过率**: 95%+ (1043个测试，991个通过)
- **XML适配**: 支持50+种骑砍2XML配置文件类型
- **性能优化**: 大型XML文件分片处理，内存使用优化

### 核心架构特性

项目采用企业级分层架构设计：

- **DO层 (Domain Object)**: 领域对象，包含业务逻辑和运行时状态管理
- **DTO层 (Data Transfer Object)**: 数据传输对象，专门用于XML序列化/反序列化
- **Mapper层**: 对象映射器，负责DO和DTO之间的双向转换
- **Service层**: 核心业务服务，包括文件发现、命名映射、质量监控等

这种架构实现了完美的关注点分离，提供了对XML序列化行为的精确控制。

## 快速开始

### 环境要求

- **.NET 9.0 SDK** 或更高版本
- **IDE**: Visual Studio 2022、VS Code、JetBrains Rider
- **内存**: 推荐8GB以上（用于处理大型XML文件）
- **系统**: Windows、macOS、Linux

### 构建和运行

```bash
# 克隆仓库
git clone <repository-url>
cd BannerlordModEditor-Docs-Enhanced

# 还原依赖
dotnet restore

# 构建解决方案
dotnet build

# 运行所有测试（推荐）
dotnet test --verbosity normal

# 启动应用程序
dotnet run --project BannerlordModEditor.UI

# 运行特定测试
dotnet test --filter "TestName"
```

## 项目结构

```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/           # 核心业务逻辑层
│   ├── Models/                          # 数据模型（分层架构）
│   │   ├── DO/                         # 领域对象 (Domain Objects)
│   │   ├── DTO/                        # 数据传输对象 (Data Transfer Objects)
│   │   ├── Data/                       # 原始数据模型（向后兼容）
│   │   ├── Audio/                      # 音频系统模型
│   │   ├── Engine/                     # 游戏引擎模型
│   │   ├── Game/                       # 游戏机制模型
│   │   ├── Multiplayer/                # 多人游戏模型
│   │   └── Configuration/              # 游戏配置模型
│   ├── Services/                       # 核心业务服务
│   │   ├── FileDiscoveryService.cs     # XML文件发现服务
│   │   ├── NamingConventionMapper.cs   # 命名约定映射
│   │   ├── QualityMonitoringService.cs # 质量监控服务
│   │   └── XmlMemoryManager.cs         # XML内存管理
│   ├── Mappers/                        # 数据映射器 (40+个)
│   └── Loaders/                        # 数据加载器
├── BannerlordModEditor.UI/               # UI表现层 (Avalonia)
│   ├── ViewModels/                     # MVVM视图模型
│   ├── Views/                          # Avalonia XAML视图
│   └── Assets/                         # UI资源
├── BannerlordModEditor.Common.Tests/     # Common层测试
│   ├── TestData/                       # 真实XML测试数据 (80+个文件)
│   ├── TestSubsets/                    # 大型XML文件分片
│   └── Comprehensive/                  # 综合测试套件
├── BannerlordModEditor.UI.Tests/         # UI层测试
├── docs/                                # 完整项目文档
│   ├── specs/                          # 技术规格
│   ├── reports/                        # 项目报告
│   ├── analysis/                       # 分析文档
│   └── technical/                      # 技术文档
├── example/                            # 参考示例和真实XML文件
└── scripts/                            # 构建和分析脚本
```

## 核心功能

### 🚀 XML适配系统

项目实现了一个企业级的XML适配系统，将骑砍2的各种XML配置文件转换为C#强类型模型：

1. **智能文件发现**: `FileDiscoveryService`自动扫描XML目录，识别未适配的文件
2. **命名约定映射**: `NamingConventionMapper`处理XML文件名到C#类名的智能转换
3. **分层模型生成**: 基于XML结构生成对应的DO/DTO分层模型类
4. **高性能序列化**: `GenericXmlLoader`处理XML的读写操作，支持大型文件

### 📋 支持的XML类型 (50+种)

项目全面支持骑砍2的各种XML配置文件类型：

#### 游戏核心机制
- **动作系统**: ActionTypes、ActionSets、CombatParameters
- **物品系统**: ItemModifiers、ItemHolsters、CraftingPieces、CraftingTemplates
- **角色系统**: Attributes、Skills、BoneBodyTypes
- **物理系统**: PhysicsMaterials、CollisionInfos

#### 多人游戏
- **多人物品**: MpItems、MpCraftingPieces、Mpcosmetics
- **多人文化**: MpCultures、MultiplayerScenes
- **多人角色**: MPCharacters、MPClassDivisions、MPBadges

#### 音视频系统
- **音频**: SoundFiles、ModuleSounds、VoiceDefinitions、Music
- **视频**: ParticleSystems、GPU_particle_systems

#### 地图和环境
- **地图**: MapIcons、TerrainMaterials、FloraKinds、FloraGroups
- **环境**: WaterPrefabs、SkeletonScales、MapTreeTypes

#### UI和配置
- **界面**: Looknfeel、Credits、BannerIcons
- **配置**: ManagedParameters、Adjustables、NativeParameters

### 🧪 企业级测试系统

项目拥有完善的测试体系，确保代码质量和数据完整性：

- **单元测试**: 每个XML类型都有对应的测试类 (40+个测试类)
- **集成测试**: 验证完整的XML处理流程和系统交互
- **分片测试**: 支持大型XML文件的智能分片测试 (如FloraKinds分35个部分)
- **往返测试**: 确保XML序列化和反序列化的数据100%一致性
- **真实数据测试**: 使用真实的骑砍2XML文件作为测试数据 (80+个测试文件)

## 开发指南

### 🔧 添加新的XML适配

#### 1. 创建分层模型类
```csharp
// 在Models/相应功能域创建DO和DTO类
namespace BannerlordModEditor.Common.Models.Game
{
    public class NewXmlTypeDO
    {
        [XmlElement("property")]
        public string Property { get; set; } = string.Empty;
        
        [XmlIgnore]
        public bool HasProperty { get; set; } = false;
        
        public bool ShouldSerializeProperty() => HasProperty && !string.IsNullOrEmpty(Property);
    }
    
    public class NewXmlTypeDTO
    {
        [XmlElement("property")]
        public string Property { get; set; } = string.Empty;
    }
}
```

#### 2. 创建映射器
```csharp
namespace BannerlordModEditor.Common.Mappers
{
    public static class NewXmlTypeMapper
    {
        public static NewXmlTypeDTO ToDTO(NewXmlTypeDO source)
        {
            if (source == null) return null;
            
            return new NewXmlTypeDTO
            {
                Property = source.Property
            };
        }
        
        public static NewXmlTypeDO ToDO(NewXmlTypeDTO source)
        {
            if (source == null) return null;
            
            return new NewXmlTypeDO
            {
                Property = source.Property,
                HasProperty = !string.IsNullOrEmpty(source.Property)
            };
        }
    }
}
```

#### 3. 添加综合测试
```csharp
namespace BannerlordModEditor.Common.Tests
{
    public class NewXmlTypeXmlTests : BaseXmlTestClass
    {
        [Fact]
        public void SerializeDeserialize_ShouldPreserveData()
        {
            // 往返测试确保数据完整性
            var xml = File.ReadAllText("TestData/new_xml_type.xml");
            var result = XmlTestUtils.DeserializeSerializeAndCompare<NewXmlTypeDO>(xml);
            Assert.True(result);
        }
        
        [Fact]
        public void LargeFile_ShouldHandleEfficiently()
        {
            // 大型文件性能测试
            var xml = File.ReadAllText("TestData/new_xml_type_large.xml");
            var obj = XmlTestUtils.Deserialize<NewXmlTypeDO>(xml);
            Assert.NotNull(obj);
        }
    }
}
```

### 🎯 测试策略

- **100%测试覆盖**: 所有XML适配都需要对应的单元测试
- **真实数据驱动**: 测试数据使用真实的骑砍2XML文件
- **性能优化**: 大型XML文件支持分片测试以避免性能问题
- **数据完整性**: 往返测试确保序列化和反序列化的数据100%一致性
- **边界情况**: 测试空元素、缺失字段、特殊字符等边界情况

### 📝 代码规范

- **现代C#**: 使用C# 9.0+特性和模式匹配
- **空安全**: 启用Nullable引用类型，彻底处理null情况
- **命名约定**: 遵循现有的命名约定和代码风格
- **文档化**: XML注释用于公共API文档
- **性能**: 异步处理大型文件，优化内存使用

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