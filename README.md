# BannerlordModEditor

骑马与砍杀2（Bannerlord）的Mod编辑实用工具

## 项目概述

这是一个专业的骑马与砍杀2（Bannerlord）Mod编辑器工具，使用C#和.NET 9开发。项目采用现代化的分层架构设计，专门用于处理和编辑骑砍2的XML配置文件。

## 最新更新

### 🎯 项目状态：生产就绪
- **架构重构完成**：成功实现完整的XML适配系统
- **XML适配**: 支持50+种骑砍2XML配置文件类型
- **性能优化**: 大型XML文件分片处理，内存使用优化
- **文档完善**: 完整的项目文档和使用指南

### 核心架构特性

项目采用标准的分层架构设计：

- **Common层**: 核心业务逻辑层，包含数据模型、服务、映射器等
- **UI层**: 用户界面层，基于Avalonia UI的现代化桌面应用
- **Tests层**: 完整的测试体系，包含单元测试和集成测试

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
│   ├── Models/                          # 数据模型（按功能域组织）
│   │   ├── DO/                         # 领域对象 (Domain Objects)
│   │   ├── DTO/                        # 数据传输对象 (Data Transfer Objects)
│   │   ├── Data/                       # 原始数据模型（向后兼容）
│   │   ├── Audio/                      # 音频系统模型
│   │   ├── Engine/                     # 游戏引擎模型
│   │   ├── Game/                       # 游戏机制模型
│   │   ├── Multiplayer/                # 多人游戏模型
│   │   ├── Configuration/              # 游戏配置模型
│   │   └── ...                         # 其他功能域模型
│   ├── Services/                       # 核心业务服务
│   │   ├── FileDiscoveryService.cs     # XML文件发现服务
│   │   ├── NamingConventionMapper.cs   # 命名约定映射
│   │   ├── QualityMonitoringService.cs # 质量监控服务
│   │   ├── XmlMemoryManager.cs         # XML内存管理
│   │   └── HybridXml/                  # 混合XML处理服务
│   ├── Mappers/                        # 数据映射器 (40+个)
│   │   ├── ActionTypesMapper.cs
│   │   ├── CombatParametersMapper.cs
│   │   └── ...                         # 其他XML类型映射器
│   └── Loaders/                        # 数据加载器
│       ├── GenericXmlLoader.cs         # 通用XML加载器
│       └── EnhancedXmlLoader.cs        # 增强XML加载器
├── BannerlordModEditor.UI/               # UI表现层 (Avalonia)
│   ├── ViewModels/                     # MVVM视图模型
│   │   ├── EditorManagerViewModel.cs
│   │   ├── MainWindowViewModel.cs
│   │   └── EditorCategoryViewModel.cs
│   ├── Views/                          # Avalonia XAML视图
│   │   └── MainWindow.axaml
│   └── Assets/                         # UI资源
├── BannerlordModEditor.Common.Tests/     # Common层测试
│   ├── TestData/                       # 真实XML测试数据 (80+个文件)
│   ├── TestSubsets/                    # 大型XML文件分片
│   │   ├── ActionTypes/               # 分片测试示例
│   │   ├── FloraKinds/                # 35个分片文件
│   │   └── MpItems/                   # 多人物品分片
│   ├── Comprehensive/                  # 综合测试套件
│   └── Integration/                    # 集成测试
├── BannerlordModEditor.UI.Tests/         # UI层测试
├── docs/                                # 完整项目文档
│   ├── README.md                       # 文档中心导航
│   ├── CLAUDE.md                       # 文档管理规范
│   ├── PROJECT_FINAL_SUMMARY.md        # 项目最终总结
│   ├── development/                    # 开发文档
│   │   ├── project/                   # 项目计划文档
│   │   └── technical/                 # 技术规格文档
│   ├── guides/                        # 使用指南
│   ├── reference/                     # 技术参考
│   └── archive/                       # 归档文档
├── example/                            # 参考示例和真实XML文件
│   └── ModuleData/                    # 真实的骑砍2XML文件
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

## 🛠️ 技术栈

### 核心技术

- **.NET 9.0**: 最新.NET平台，支持现代C#特性
- **Avalonia UI 11.3**: 跨平台桌面UI框架，支持Windows、macOS、Linux
- **xUnit 2.5**: 企业级单元测试框架
- **CommunityToolkit.Mvvm 8.2**: 微软官方MVVM工具包

### 关键依赖包

- **Velopack**: 现代化应用程序更新和打包解决方案
- **Avalonia.Themes.Fluent**: 微软Fluent UI设计主题
- **coverlet.collector**: .NET代码覆盖率工具
- **System.Xml**: 高性能XML处理库

## 🔒 质量保证

### XML适配质量标准

- **严格字段匹配**: XML文档中存在的字段就是要存在的，不存在的字段就不能存在
- **精确空元素处理**: 严格区分字段不存在和字段为空的情况
- **100%数据完整性**: 确保往返测试不会丢失或增加任何数据
- **UTF-8编码支持**: 完全支持国际化字符和特殊符号

### 性能优化策略

- **异步处理**: 大型XML文件采用异步处理，避免UI阻塞
- **并行处理**: 文件发现服务支持并行处理，提高处理速度
- **内存管理**: 智能内存管理，测试数据按需加载以减少内存占用
- **分片处理**: 大型文件自动分片，支持流式处理
- **缓存优化**: 智能缓存策略，避免重复处理相同文件

### 监控和诊断

- **质量监控服务**: 实时监控XML适配质量
- **性能分析**: 内置性能分析工具
- **错误追踪**: 完整的错误日志和追踪系统
- **内存监控**: XML内存管理器监控内存使用情况

## 🤝 贡献指南

### 开发流程
1. **Fork** 本仓库
2. **创建功能分支** (`git checkout -b feature/new-feature`)
3. **开发并测试** (`dotnet build && dotnet test`)
4. **提交更改** (`git commit -am 'Add new feature'`)
5. **推送到分支** (`git push origin feature/new-feature`)
6. **创建Pull Request**

### 代码要求
- 所有新功能必须包含相应的单元测试
- 代码风格必须与现有代码保持一致
- 遵循DO/DTO分层架构模式
- 提交前必须通过所有测试

### 问题报告
- 使用GitHub Issues报告bug
- 提供详细的复现步骤和期望结果
- 包含相关的XML文件示例（如果可能）

## 📄 许可证

本项目采用 **MIT许可证**。详情请参阅[LICENSE](LICENSE)文件。

## 📚 文档中心

### 技术文档
- [架构设计](docs/reference/architecture.md) - 系统架构和技术选型
- [技术规格](docs/development/technical/) - 完整的技术规格文档
- [API规格](docs/development/technical/api-spec.md) - API接口规格说明
- [测试指南](docs/guides/comprehensive-test-suite.md) - 测试策略和方法

### 项目文档
- [项目计划](docs/development/project/) - 项目需求、用户故事、验收标准
- [项目总结](docs/PROJECT_FINAL_SUMMARY.md) - 项目完整状态和成果总结
- [技术参考](docs/reference/) - 技术分析和参考文档
- [使用指南](docs/guides/) - 测试指南和最佳实践

### 开发资源
- [实现指南](docs/development/technical/xml-adaptation-implementation-guide.md) - XML适配实现指南
- [文档管理规范](docs/CLAUDE.md) - 项目文档管理规则和格式要求
- [故障排除](docs/guides/) - 常见问题解决方案
- [最佳实践](docs/guides/) - 开发最佳实践

## 📞 联系方式

如有问题或建议，请通过以下方式联系：

- **GitHub Issues**: 报告bug和功能请求
- **Pull Requests**: 代码贡献和改进
- **讨论区**: 技术讨论和经验分享
- **邮件**: [项目维护者邮箱](mailto:project@example.com)

---

## ⚠️ 免责声明

**注意**: 本项目仅用于骑马与砍杀2游戏的Mod开发工具，请遵守游戏的相关条款和条件。

- 本项目与TaleWorlds Entertainment无关
- 使用本项目产生的任何后果由用户自行承担
- 请确保在使用前备份重要的游戏数据

---

**最后更新**: 2025年8月21日  
**项目状态**: 生产就绪  
**维护团队**: BannerlordModEditor开发团队