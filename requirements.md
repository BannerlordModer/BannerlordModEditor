# 剩余单元测试失败修复需求文档（第二阶段）

## 1. 项目背景

Bannerlord Mod Editor项目第二阶段修复工作旨在解决剩余的25个单元测试失败问题，完成DO/DTO分层架构的全面实施，确保所有测试通过并推送至GitHub。

## 2. 当前状态分析

### 2.1 测试现状
- **总测试数**: 1063个
- **通过测试**: 1036个
- **失败测试**: 25个
- **跳过测试**: 2个
- **通过率**: 97.5%
- **目标通过率**: 99%+

### 2.2 失败测试分类

#### A类：缺少测试数据文件（2个）
1. **CollisionInfosXmlTests** - 缺少collision_infos.xml测试文件
2. **DataTests** - BannerIcons相关测试，可能缺少数据文件

#### B类：未应用DO/DTO模式的测试（23个）
1. **BeforeTransparentsGraphXmlTests** - 结构化相等性失败
2. **LooknfeelXmlTests** - 结构化相等性失败
3. **ParticleSystemsBasicXmlTests** - 结构化相等性失败
4. **ParticleSystemsHardcodedMisc1XmlTests** - 结构化相等性失败
5. **ParticleSystemsHardcodedMisc2XmlTests** - 结构化相等性失败
6. **ParticleSystemsOutdoorXmlTests** - 结构化相等性失败
7. **SimpleBannerIconsTest** - 结构化相等性失败
8. **TempDebugTest** - 属性格式比较失败
9. **ActionSetsXmlTests** - 序列化排序问题（严重）
10. **ActionTypesXmlTests** - 子集XMLs测试失败
11. **ItemHolstersXmlTests** - 结构化相等性失败
12. **MpcosmeticsXmlTests** - 结构化相等性失败
13. **CombatParametersXmlTests** - 主测试和部分文件测试
14. **CreditsXmlTests** - 结构化相等性失败
15. **CreditsLegalPCXmlTests** - 结构化相等性失败
16. **CreditsExternalPartnersPlayStationXmlTests** - 结构化相等性失败
17. **FloraKindsXmlTests** - 结构化相等性失败
18. **MpCraftingPiecesXmlTests** - 结构化相等性失败
19. **DebugMultiplayerScenes** - 序列化失败
20. **__tests__.XmlTestUtils_CompareXmlStructure_Tests** - 测试框架相关
21. **DataTests** - BannerIcons结构和颜色验证

## 3. 根本原因分析

### 3.1 技术问题
- **DO/DTO架构未完全覆盖**：大部分失败测试仍未应用DO/DTO模式
- **XML序列化排序问题**：ActionSets存在严重的元素顺序问题
- **测试数据文件缺失**：部分测试缺少必要的XML文件
- **复杂嵌套结构处理不当**：ParticleSystems等复杂模型处理不够完善

### 3.2 架构问题
- **模型实现不完整**：多个XML模型尚未创建DO/DTO实现
- **映射器缺失**：对应的DO/DTO映射器未实现
- **测试未更新**：测试代码仍在使用旧的模型结构

### 3.3 流程问题
- **依赖关系复杂**：某些测试依赖于特定的文件和数据
- **测试环境配置**：部分测试在特定环境下可能失败

## 4. 实施策略

### 4.1 优先级排序

#### 最高优先级（P0）- 立即修复
1. **ActionSetsXmlTests** - 最严重的序列化排序问题
2. **CollisionInfosXmlTests** - 需要缺失的测试文件
3. **DataTests** - BannerIcons相关测试

#### 高优先级（P1）- 本周完成
1. **ParticleSystems相关测试** - 5个测试，复杂嵌套结构
2. **Credits相关测试** - 4个测试，信用模型
3. **CombatParametersXmlTests** - 3个测试，战斗参数

#### 中优先级（P2）- 后续完成
1. **ItemHolstersXmlTests** - 物品挂载测试
2. **MpcosmeticsXmlTests** - 多人装饰品测试
3. **MpCraftingPiecesXmlTests** - 多人制作件测试

#### 低优先级（P3）- 最后处理
1. **DebugMultiplayerScenes** - 调试测试
2. **TempDebugTest** - 临时调试测试
3. **__tests__.XmlTestUtils_CompareXmlStructure_Tests** - 测试框架

### 4.2 实施步骤

#### 步骤1: 修复最高优先级问题（1-2天）
1. **解决ActionSets序列化排序问题**
   - 研究XML序列化机制
   - 实现自定义XML序列化器
   - 测试和验证解决方案

2. **创建缺失的测试文件**
   - 创建collision_infos.xml
   - 修复DataTests的依赖问题

#### 步骤2: 高优先级模型实现（3-4天）
1. **ParticleSystems DO/DTO实现**
   - 分析ParticleSystems XML结构
   - 创建DO/DTO模型
   - 实现映射器和测试更新

2. **Credits DO/DTO实现**
   - 实现Credits相关模型
   - 更新4个相关测试

3. **CombatParameters完善**
   - 修复CombatParameters部分文件测试
   - 完善DO/DTO实现

#### 步骤3: 中优先级模型实现（2-3天）
1. **ItemHolsters DO/DTO实现**
2. **Mpcosmetics DO/DTO实现**
3. **MpCraftingPieces DO/DTO实现**

#### 步骤4: 低优先级问题处理（1-2天）
1. **修复Debug相关测试**
2. **完善测试框架**
3. **全面测试验证**

#### 步骤5: 最终验证和推送（1天）
1. **运行完整测试套件**
2. **修复剩余问题**
3. **文档更新**
4. **GitHub推送**

## 5. 技术约束和假设

### 5.1 技术约束
- **架构一致性**：必须保持DO/DTO分层架构模式
- **性能要求**：不能显著影响XML处理性能
- **兼容性要求**：必须保持与游戏原生XML的完全兼容
- **时间约束**：总修复时间不超过10个工作日

### 5.2 技术假设
- **XML结构一致性**：所有XML文件格式符合Bannerlord标准
- **架构有效性**：DO/DTO分层架构能够解决大部分问题
- **数据完整性**：所有测试数据文件都是可获取或可创建的

## 6. 风险评估

### 6.1 技术风险
- **风险1**: ActionSets排序问题可能需要深度修改XML序列化机制
- **风险2**: 某些复杂嵌套结构可能难以精确匹配
- **风险3**: 测试数据文件创建可能遇到格式问题

### 6.2 时间风险
- **风险1**: 复杂问题解决可能超过预期时间
- **风险2**: 测试验证和调试时间可能较长

### 6.3 质量风险
- **风险1**: 快速修复可能引入新的问题
- **风险2**: 某些边缘情况可能未被覆盖

## 7. 应对策略

### 7.1 技术风险应对
- **策略1**: 为ActionSets准备多种解决方案
- **策略2**: 建立问题隔离和回退机制
- **策略3**: 增强测试覆盖和验证

### 7.2 时间风险应对
- **策略1**: 优先处理最高价值问题
- **策略2**: 并行处理多个简单问题
- **策略3**: 设置时间缓冲和里程碑

### 7.3 质量风险应对
- **策略1**: 增量开发，频繁验证
- **策略2**: 代码审查和静态分析
- **策略3**: 全面的回归测试

## 8. 成功指标

### 8.1 核心指标
- **测试通过率**: ≥99%（1052/1063以上）
- **失败测试数**: ≤11个
- **代码质量评分**: ≥95%

### 8.2 技术指标
- **DO/DTO覆盖率**: ≥90%的XML模型
- **架构符合度**: ≥95%
- **性能影响**: ≤5%的性能下降

### 8.3 时间指标
- **修复周期**: ≤10个工作日
- **里程碑达成**: 按计划完成各阶段目标
- **交付时间**: 按时完成GitHub推送

## 9. 验收标准

### 9.1 功能验收
- [ ] 所有高优先级测试通过
- [ ] DO/DTO架构完整覆盖
- [ ] XML序列化问题解决
- [ ] 测试数据文件完备

### 9.2 质量验收
- [ ] 代码质量评分≥95%
- [ ] 无严重或致命错误
- [ ] 文档完整更新
- [ ] 回归测试全部通过

### 9.3 流程验收
- [ ] 按时完成所有修复
- [ ] 成功推送到GitHub
- [ ] 项目文档完整
- [ ] 知识转移完成

## 10. 后续计划

### 10.1 短期计划（1-2周）
- 完成剩余测试修复
- 性能优化和用户体验改进
- 文档完善和知识共享

### 10.2 中期计划（1-2月）
- 扩展DO/DTO架构到更多模型
- 增强测试框架和工具
- 用户体验优化

### 10.3 长期计划（3-6月）
- 插件化架构实施
- 高级功能开发
- 社区贡献和开源

---

**项目目标**: 修复剩余25个单元测试失败，达到99%+测试通过率
**预计时间**: 10个工作日
**质量目标**: 95%+代码质量评分
**最终交付**: 完整的代码修复和GitHub推送