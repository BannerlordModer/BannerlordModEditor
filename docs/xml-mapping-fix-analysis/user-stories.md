# XML映射适配修复用户故事

## 项目概述

本文档定义了BannerlordModEditor-CLI项目中XML映射适配问题的详细用户故事和验收标准，为开发团队提供清晰的实施指导。

## Epic 1: XmlTestUtils 核心修复

### Story 1.1: 修复布尔值标准化逻辑

**As a** XML处理引擎  
**I want** 布尔值标准化逻辑能够正确处理所有布尔值格式  
**So that** XML比较结果的一致性和准确性得到保证

**验收标准 (EARS格式):**
- **WHEN** 遇到布尔值属性 **THEN** 所有常见布尔值格式都应标准化为"true"/"false"
- **IF** 布尔值属性包含"True"/"TRUE"/"1" **THEN** 应标准化为"true"
- **IF** 布尔值属性包含"False"/"FALSE"/"0" **THEN** 应标准化为"false"
- **FOR** 所有XML文件中的布尔值属性 **VERIFY** 标准化后的一致性

**技术说明:**
- 需要扩展XmlTestUtils.CommonBooleanTrueValues和CommonBooleanFalseValues
- 改进布尔值检测逻辑，支持更多格式
- 确保标准化过程不会影响非布尔值属性

**依赖项:**
- XmlTestUtils.NormalizeXml方法
- XML比较选项配置

**Story Points:** 5
**Priority:** High

### Story 1.2: 优化属性排序算法

**As a** XML序列化系统  
**I want** 属性排序算法能够产生一致的输出顺序  
**So that** XML比较结果不受属性顺序影响

**验收标准 (EARS格式):**
- **WHEN** 序列化XML元素 **THEN** 属性应按预定义顺序排列
- **IF** 元素包含命名空间属性 **THEN** 命名空间属性应排在最前面
- **IF** 元素包含普通属性 **THEN** 应按字母顺序排序
- **FOR** 所有XML元素 **VERIFY** 属性顺序的一致性

**技术说明:**
- 修改XmlTestUtils.NormalizeXml中的属性排序逻辑
- 实现命名空间属性优先的排序策略
- 确保排序算法的性能和稳定性

**依赖项:**
- XmlTestUtils.NormalizeXml方法
- XML序列化器配置

**Story Points:** 3
**Priority:** High

### Story 1.3: 改进空元素处理逻辑

**As a** XML处理引擎  
**I want** 空元素处理逻辑能够正确保留原始XML结构  
**So that** 往返测试不会因为空元素处理而失败

**验收标准 (EARS格式):**
- **WHEN** 处理空元素 **THEN** 应保留原始的空元素格式
- **IF** 原始XML使用自闭合标签 **THEN** 序列化后也应使用自闭合标签
- **IF** 原始XML使用开始/结束标签 **THEN** 序列化后也应使用开始/结束标签
- **FOR** 所有空元素 **VERIFY** 格式的一致性

**技术说明:**
- 分析原始XML的空元素格式
- 在序列化时保留原始格式
- 添加空元素格式检测逻辑

**依赖项:**
- XmlTestUtils.Serialize方法
- XmlTestUtils.NormalizeXml方法

**Story Points:** 4
**Priority:** Medium

## Epic 2: 特殊XML类型修复

### Story 2.1: 修复SiegeEngines往返测试

**As a** 攻城器械配置处理器  
**I want** SiegeEngines往返测试能够通过  
**So that** 攻城器械配置可以被正确编辑和保存

**验收标准 (EARS格式):**
- **WHEN** 反序列化siegeengines.xml **THEN** 根元素应为"SiegeEngineTypes"
- **IF** 序列化SiegeEnginesDO对象 **THEN** 应保持原始根元素名称
- **WHEN** 进行往返测试 **THEN** 序列化结果应与原始XML结构一致
- **FOR** 所有攻城器械属性 **VERIFY** 属性值的正确性

**技术说明:**
- 检查SiegeEnginesDO的XmlRoot配置
- 确保根元素名称正确映射
- 验证所有属性的ShouldSerialize方法

**依赖项:**
- SiegeEnginesDO模型
- SiegeEnginesMapper映射器
- XmlTestUtils测试工具

**Story Points:** 8
**Priority:** High

### Story 2.2: 修复SpecialMeshes往返测试

**As a** 特殊网格配置处理器  
**I want** SpecialMeshes往返测试能够通过  
**So that** 特殊网格配置可以被正确编辑和保存

**验收标准 (EARS格式):**
- **WHEN** 反序列化special_meshes.xml **THEN** 嵌套结构应正确解析
- **IF** 处理meshes和types元素 **THEN** 应保持原始的嵌套关系
- **WHEN** 进行往返测试 **THEN** 序列化结果应保持原始结构
- **FOR** 所有网格和类型 **VERIFY** 数据的完整性

**技术说明:**
- 分析SpecialMeshesDO的嵌套结构
- 确保XmlArray和XmlArrayItem配置正确
- 验证嵌套对象的序列化行为

**依赖项:**
- SpecialMeshesDO模型
- SpecialMeshesMapper映射器
- XmlTestUtils测试工具

**Story Points:** 6
**Priority:** High

### Story 2.3: 修复LanguageBase混合内容问题

**As a** 语言配置处理器  
**I want** LanguageBase往返测试能够正确处理混合内容  
**So that** 语言函数和字符串配置可以被正确处理

**验收标准 (EARS格式):**
- **WHEN** 反序列化包含函数的XML **THEN** 函数体应正确解析
- **IF** 函数体包含特殊字符 **THEN** 序列化时应正确转义
- **WHEN** 进行往返测试 **THEN** 函数体内容应保持不变
- **FOR** 所有语言配置元素 **VERIFY** 内容的完整性

**技术说明:**
- 分析LanguageBaseDO的函数体处理逻辑
- 确保特殊字符正确转义和反转义
- 验证混合内容的序列化行为

**依赖项:**
- LanguageBaseDO模型
- XmlTestUtils测试工具
- XML序列化配置

**Story Points:** 7
**Priority:** High

### Story 2.4: 修复MultiplayerScenes往返测试

**As a** 多人游戏场景配置处理器  
**I want** MultiplayerScenes往返测试能够通过  
**So that** 多人游戏场景配置可以被正确编辑和保存

**验收标准 (EARS格式):**
- **WHEN** 反序列化MultiplayerScenes.xml **THEN** 复杂嵌套结构应正确解析
- **IF** 处理游戏类型配置 **THEN** 应保持原始的嵌套关系
- **WHEN** 进行往返测试 **THEN** 序列化结果应保持原始结构
- **FOR** 所有场景和游戏类型 **VERIFY** 数据的完整性

**技术说明:**
- 分析MultiplayerScenesDO的复杂结构
- 确保游戏类型嵌套配置正确
- 验证复杂对象的序列化行为

**依赖项:**
- MultiplayerScenesDO模型
- MultiplayerScenesMapper映射器
- XmlTestUtils测试工具

**Story Points:** 5
**Priority:** Medium

### Story 2.5: 修复TauntUsageSets往返测试

**As a** 嘲笑使用配置处理器  
**I want** TauntUsageSets往返测试能够通过  
**So that** 嘲笑使用配置可以被正确编辑和保存

**验收标准 (EARS格式):**
- **WHEN** 反序列化taunt_usage_sets.xml **THEN** 注释应被正确处理
- **IF** 进行往返测试 **THEN** 注释处理选项应正确应用
- **WHEN** 比较XML结构 **THEN** 应忽略注释差异
- **FOR** 所有嘲笑使用配置 **VERIFY** 数据的完整性

**技术说明:**
- 配置XmlComparisonOptions.IgnoreComments选项
- 确保注释处理逻辑正确
- 验证往返测试的注释处理行为

**依赖项:**
- TauntUsageSetsDO模型
- TauntUsageSetsMapper映射器
- XmlTestUtils测试工具

**Story Points:** 4
**Priority:** Medium

## Epic 3: 质量保证和性能优化

### Story 3.1: 建立XML处理性能基准

**As a** 性能优化团队  
**I want** 建立XML处理的性能基准测试  
**So that** 可以量化优化效果并监控性能变化

**验收标准 (EARS格式):**
- **WHEN** 运行性能测试 **THEN** 应收集关键性能指标
- **IF** 处理大型XML文件 **THEN** 性能应在可接受范围内
- **WHEN** 进行性能比较 **THEN** 应有明确的基准数据
- **FOR** 所有XML操作 **VERIFY** 性能指标的一致性

**技术说明:**
- 创建性能测试套件
- 建立性能基准数据
- 实现性能监控机制

**依赖项:**
- 性能测试框架
- 基准测试数据
- 性能监控工具

**Story Points:** 6
**Priority:** Medium

### Story 3.2: 实现自动化质量监控

**As a** 质量保证团队  
**I want** 实现自动化的质量监控系统  
**So that** 可以持续监控代码质量和测试通过率

**验收标准 (EARS格式):**
- **WHEN** 代码变更时 **THEN** 应自动运行质量检查
- **IF** 质量指标下降 **THEN** 应触发警报
- **WHEN** 生成质量报告 **THEN** 应包含详细的分析数据
- **FOR** 所有质量指标 **VERIFY** 监控的准确性

**技术说明:**
- 集成代码质量检查工具
- 建立质量指标监控机制
- 实现自动化报告生成

**依赖项:**
- 质量检查工具
- 监控系统
- 报告生成工具

**Story Points:** 5
**Priority:** Medium

### Story 3.3: 完善测试覆盖

**As a** 测试团队  
**I want** 完善XML处理的测试覆盖  
**So that** 可以确保所有功能都得到充分测试

**验收标准 (EARS格式):**
- **WHEN** 运行测试套件 **THEN** 关键路径应有测试覆盖
- **IF** 添加新功能 **THEN** 应包含相应的测试
- **WHEN** 进行代码变更 **THEN** 应运行所有相关测试
- **FOR** 所有XML处理功能 **VERIFY** 测试的完整性

**技术说明:**
- 分析现有测试覆盖
- 识别测试覆盖缺口
- 添加缺失的测试用例

**依赖项:**
- 测试框架
- 测试数据
- 代码覆盖率工具

**Story Points:** 8
**Priority:** Medium

## Epic 4: 文档和知识传递

### Story 4.1: 创建XML处理指南

**As a** 开发团队  
**I want** 创建详细的XML处理指南  
**So that** 新的开发者可以快速理解和维护XML处理逻辑

**验收标准 (EARS格式):**
- **WHEN** 查看处理指南 **THEN** 应包含所有关键概念说明
- **IF** 遇到XML处理问题 **THEN** 应能在指南中找到解决方案
- **WHEN** 遵循指南操作 **THEN** 应能正确处理XML文件
- **FOR** 所有XML处理功能 **VERIFY** 文档的准确性

**技术说明:**
- 创建详细的处理指南文档
- 包含示例代码和最佳实践
- 提供故障排除指南

**依赖项:**
- 文档编写工具
- 技术写作资源
- 代码示例

**Story Points:** 4
**Priority:** Low

### Story 4.2: 建立故障排除知识库

**As a** 支持团队  
**I want** 建立XML处理的故障排除知识库  
**So that** 可以快速解决常见问题和错误

**验收标准 (EARS格式):**
- **WHEN** 遇到XML处理错误 **THEN** 应能在知识库中找到解决方案
- **IF** 发现新的问题模式 **THEN** 应添加到知识库中
- **WHEN** 查看知识库 **THEN** 应包含详细的解决步骤
- **FOR** 所有常见错误 **VERIFY** 解决方案的有效性

**技术说明:**
- 收集常见问题和解决方案
- 建立知识库结构
- 实现知识库更新机制

**依赖项:**
- 知识库平台
- 问题跟踪系统
- 文档管理工具

**Story Points:** 3
**Priority:** Low

## 实施计划

### 阶段1: 核心修复 (第1-2周)
- [ ] Story 1.1: 修复布尔值标准化逻辑
- [ ] Story 1.2: 优化属性排序算法
- [ ] Story 1.3: 改进空元素处理逻辑

### 阶段2: 特殊XML类型修复 (第3-4周)
- [ ] Story 2.1: 修复SiegeEngines往返测试
- [ ] Story 2.2: 修复SpecialMeshes往返测试
- [ ] Story 2.3: 修复LanguageBase混合内容问题
- [ ] Story 2.4: 修复MultiplayerScenes往返测试
- [ ] Story 2.5: 修复TauntUsageSets往返测试

### 阶段3: 质量保证 (第5-6周)
- [ ] Story 3.1: 建立XML处理性能基准
- [ ] Story 3.2: 实现自动化质量监控
- [ ] Story 3.3: 完善测试覆盖

### 阶段4: 文档和知识传递 (第7-8周)
- [ ] Story 4.1: 创建XML处理指南
- [ ] Story 4.2: 建立故障排除知识库

## 验收标准总览

### 功能性验收标准
- [ ] 所有9个失败的往返测试修复完成
- [ ] XML处理功能正常工作
- [ ] 序列化和反序列化准确无误
- [ ] 错误处理机制完善

### 非功能性验收标准
- [ ] 性能达到预期指标
- [ ] 代码质量符合标准
- [ ] 测试覆盖充分
- [ ] 文档完整准确

### 质量标准
- [ ] 测试通过率 ≥ 95%
- [ ] 代码覆盖率 ≥ 85%
- [ ] 性能提升 ≥ 20%
- [ ] 代码复杂度降低 ≥ 10%

## 风险和依赖

### 主要风险
- **技术风险**: XML序列化的根本性问题可能导致需要重新设计
- **时间风险**: 修复所有问题可能需要比预期更长的时间
- **质量风险**: 在修复过程中可能引入新的问题

### 关键依赖
- **开发资源**: 需要熟练的C#开发人员
- **测试资源**: 需要充分的测试环境和数据
- **时间资源**: 需要足够的开发时间进行彻底修复

## 成功标准

### 量化成功指标
- 所有往返测试通过率达到100%
- 整体测试通过率 ≥ 95%
- 代码覆盖率 ≥ 85%
- 性能提升 ≥ 20%

### 定性成功指标
- XML处理功能稳定可靠
- 代码易于维护和扩展
- 文档完整且准确
- 团队对XML处理有深入理解

通过系统性地实施这些用户故事，项目将达到95%的质量标准，并为用户提供稳定可靠的XML编辑功能。