# BannerlordModEditor XML适配任务规格说明

## 项目概述

BannerlordModEditor是一个骑马与砍杀2（Bannerlord）的Mod编辑器工具，使用C#和.NET 9开发。项目采用DO/DTO架构模式来处理XML序列化问题，确保XML配置文件的精确读写。

## 当前问题分析

### 1. BannerIcons - 最高优先级
**现状**: 使用旧的Data模型（BannerIconsRoot），测试失败
**问题**: 缺乏DO/DTO架构，无法精确控制XML序列化行为
**影响**: 无法保证XML结构的完整性和一致性

### 2. ItemModifiers - 次高优先级  
**现状**: 已有DO/DTO架构但测试失败
**问题**: 属性数量不匹配（原始840 vs 序列化842）
**影响**: 序列化后的XML与原始XML结构不一致

### 3. ParticleSystems - 普通优先级
**现状**: 已有DO/DTO架构但有编译警告
**问题**: 存在null引用问题和编译警告
**影响**: 代码质量下降，可能存在运行时错误

## 技术架构要求

### DO/DTO架构模式
项目采用标准的DO/DTO架构模式：

- **DO (Domain Object)**: 领域对象，包含业务逻辑和数据结构
- **DTO (Data Transfer Object)**: 数据传输对象，专门用于序列化/反序列化
- **Mapper**: 对象映射器，负责DO和DTO之间的转换

### 文件组织结构
```
BannerlordModEditor.Common/
├── Models/
│   ├── DO/              # 领域对象
│   │   ├── BannerIconsDO.cs
│   │   ├── ItemModifiersDO.cs
│   │   └── ParticleSystemsDO.cs
│   ├── DTO/             # 数据传输对象
│   │   ├── BannerIconsDTO.cs
│   │   ├── ItemModifiersDTO.cs
│   │   └── ParticleSystemsDTO.cs
│   └── Data/            # 原始数据模型（兼容性）
│       ├── BannerIconsModel.cs
│       ├── ItemModifiers.cs
│       └── ParticleSystems.cs
├── Mappers/
│   ├── BannerIconsMapper.cs
│   ├── ItemModifiersMapper.cs
│   └── ParticleSystemsMapper.cs
└── Tests/
    └── ... (更新为使用DO层)
```

## 功能需求

### FR-001: BannerIcons DO/DTO架构实现
**描述**: 为BannerIcons实现完整的DO/DTO架构
**优先级**: 高
**验收标准**:
- [ ] 创建BannerIconsDO.cs领域对象模型
- [ ] 创建BannerIconsDTO.cs数据传输对象模型
- [ ] 创建BannerIconsMapper.cs对象映射器
- [ ] 更新测试文件使用DO层而非Data层
- [ ] 所有BannerIcons相关测试通过

### FR-002: ItemModifiers属性数量修复
**描述**: 修复ItemModifiers序列化后属性数量不匹配问题
**优先级**: 高
**验收标准**:
- [ ] 分析ItemModifiersDO模型找出属性数量差异原因
- [ ] 修复ShouldSerialize方法的逻辑
- [ ] 确保序列化前后属性数量完全一致（840个）
- [ ] 验证所有ItemModifiers相关测试通过

### FR-003: ParticleSystems编译警告修复
**描述**: 修复ParticleSystems DO/DTO架构的编译警告
**优先级**: 中
**验收标准**:
- [ ] 消除所有null引用警告
- [ ] 确保所有属性正确初始化
- [ ] 验证ParticleSystems相关测试通过
- [ ] 代码编译无警告

### FR-004: XML结构一致性保证
**描述**: 确保所有XML类型的序列化结构一致性
**优先级**: 高
**验收标准**:
- [ ] 序列化前后节点数量完全一致
- [ ] 序列化前后属性数量完全一致
- [ ] 保持XML声明和命名空间信息
- [ ] 保持注释和格式化信息

### FR-005: 特殊元素处理机制
**描述**: 实现空元素和可选元素的精确控制
**优先级**: 中
**验收标准**:
- [ ] 实现Has*标记属性来跟踪空元素状态
- [ ] 实现ShouldSerialize方法精确控制序列化行为
- [ ] 在XmlTestUtils中添加特殊处理逻辑
- [ ] 支持嵌套空元素的处理

## 非功能性需求

### NFR-001: 性能要求
**描述**: XML处理性能要求
**指标**:
- 大型XML文件（>10MB）处理时间 < 5秒
- 内存使用峰值 < 100MB
- 支持异步处理

### NFR-002: 可维护性要求
**描述**: 代码质量和可维护性
**标准**:
- 遵循现有代码风格和命名约定
- 代码覆盖率 > 80%
- 所有公共API都有XML文档注释
- 消除所有编译警告

### NFR-003: 向后兼容性
**描述**: 保持与现有代码的兼容性
**要求**:
- 不删除现有的Data层模型
- 渐进式迁移到DO/DTO架构
- 保持现有API接口不变

### NFR-004: 错误处理
**描述**: 健壮的错误处理机制
**要求**:
- 提供有意义的错误信息
- 支持部分失败恢复
- 记录详细的调试信息

## 技术约束

### 约束-001: 技术栈
- **框架**: .NET 9.0
- **语言**: C# 9.0
- **序列化**: System.Xml.Serialization
- **测试**: xUnit 2.5

### 约束-002: 架构模式
- 必须使用DO/DTO架构模式
- 必须遵循现有的Mapper模式
- 必须使用现有的XmlTestUtils基础设施

### 约束-003: XML处理
- 必须保持原始XML的格式和缩进
- 必须支持UTF-8编码
- 必须处理XML命名空间

## 数据模型要求

### BannerIcons数据模型
基于现有Data模型分析，需要处理的XML结构：
```xml
<base type="string">
    <BannerIconData>
        <BannerIconGroup id="1" name="..." is_pattern="true">
            <Background id="1" mesh_name="..." is_base_background="false"/>
            <Icon id="1" material_name="..." texture_index="0" is_reserved="false"/>
        </BannerIconGroup>
        <BannerColors>
            <Color id="1" hex="..." player_can_choose_for_background="true" player_can_choose_for_sigil="true"/>
        </BannerColors>
    </BannerIconData>
</base>
```

### ItemModifiers数据模型
需要修复的属性序列化问题：
- 16个数值属性的双向绑定（String ↔ Nullable类型）
- ShouldSerialize方法的精确控制
- 空属性的正确处理

### ParticleSystems数据模型
需要修复的null引用问题：
- 嵌套对象的null检查
- 可选元素的初始化
- 编译警告的消除

## 实现指导原则

### 原则-001: 精确控制
- 使用ShouldSerialize方法精确控制每个属性的序列化
- 使用Has*标记属性跟踪元素状态
- 在XmlTestUtils中添加特殊处理逻辑

### 原则-002: 一致性
- 遵循现有的命名约定
- 保持与成功案例（CombatParameters、ActionTypes）的一致性
- 统一的错误处理模式

### 原则-003: 可测试性
- 每个XML类型都有对应的单元测试
- 使用真实的XML测试数据
- 支持调试和问题诊断

### 原则-004: 性能优化
- 避免不必要的对象创建
- 使用高效的集合操作
- 支持大型XML文件的处理

## 风险评估

### 风险-001: 技术复杂性
**影响**: 高
**概率**: 中
**缓解措施**: 参考现有成功案例，分步骤实施

### 风险-002: 向后兼容性
**影响**: 中
**概率**: 低
**缓解措施**: 保持Data层兼容，渐进式迁移

### 风险-003: 性能问题
**影响**: 中
**概率**: 低
**缓解措施**: 性能测试，优化关键路径

## 成功标准

### 标准-001: 功能完整性
- 所有三个XML类型的测试通过
- XML序列化结构完全一致
- 没有编译警告

### 标准-002: 代码质量
- 代码覆盖率 > 80%
- 遵循编码规范
- 完整的文档注释

### 标准-003: 性能指标
- 大型XML文件处理时间 < 5秒
- 内存使用合理
- 无内存泄漏

## 时间估算

### 阶段-001: BannerIcons实现 (3-4天)
- DO/DTO模型设计和实现
- Mapper实现
- 测试更新和验证

### 阶段-002: ItemModifiers修复 (2-3天)
- 问题分析和诊断
- ShouldSerialize方法修复
- 测试验证

### 阶段-003: ParticleSystems修复 (1-2天)
- 编译警告修复
- null引用问题解决
- 测试验证

### 阶段-004: 整体验证 (1天)
- 全面测试
- 性能验证
- 文档更新

## 交付物

### 交付-001: 代码文件
- BannerIconsDO.cs
- BannerIconsDTO.cs
- BannerIconsMapper.cs
- 修复后的ItemModifiersDO.cs
- 修复后的ParticleSystemsDO.cs
- 更新的测试文件

### 交付-002: 文档
- 技术设计文档
- 测试报告
- 用户指南更新

### 交付-003: 验证报告
- 单元测试结果
- 性能测试结果
- 兼容性验证报告

## 附录

### 附录-A: 参考实现
- CombatParametersDO/DTO/Mapper
- ActionTypesDO/DTO/Mapper
- LooknfeelDO/DTO/Mapper

### 附录-B: 测试数据
- TestData/banner_icons.xml
- TestData/item_modifiers.xml
- TestData/particle_systems_*.xml

### 附录-C: 工具和脚本
- XmlTestUtils.cs
- 调试脚本
- 性能测试工具