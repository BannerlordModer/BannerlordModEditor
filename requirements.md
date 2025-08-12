# 剩余单元测试失败修复需求文档

## 1. 项目背景

Bannerlord Mod Editor项目旨在为骑马与砍杀2游戏提供一个强大的XML配置文件编辑工具。项目使用C#和.NET 9开发，采用现代化的桌面应用架构。

## 2. 当前状态

根据最新测试运行结果，项目共有1059个测试，其中：
- 通过: 1027个
- 失败: 30个
- 跳过: 2个

失败率约为2.8%，需要进一步修复剩余的单元测试失败问题。

## 3. 失败测试分类

### 3.1 已应用DO/DTO模式但仍有问题的测试
1. **ActionTypesXmlTests** - 属性数量不匹配（A=9645, B=8468）
2. **BoneBodyTypesXmlTests** - 属性数量不匹配（A=23, B=18）
3. **ActionSetsXmlTests** - 严重的结构不匹配

### 3.2 尚未应用DO/DTO模式的测试
1. **MapIconsXmlTests** - 结构化相等性失败
2. **CombatParametersXmlTests** - 多个部分文件测试失败
3. **CreditsLegalPCXmlTests** - 结构化相等性失败
4. **ParticleSystemsHardcodedMisc1XmlTests** - 结构化相等性失败
5. **SimpleBannerIconsTest** - 结构化相等性失败
6. **CollisionInfosXmlTests** - 结构化相等性失败
7. **DataTests** - BannerIcons结构和颜色验证失败
8. **ParticleSystemsBasicXmlTests** - 结构化相等性失败
9. **FloraKindsXmlTests** - 结构化相等性失败
10. **CreditsExternalPartnersPlayStationXmlTests** - 结构化相等性失败
11. **ItemHolstersXmlTests** - 结构化相等性失败
12. **LooknfeelXmlTests** - 结构化相等性失败
13. **CreditsXmlTests** - 结构化相等性失败
14. **EnhancedActionTypesXmlTests** - 属性存在性跟踪和序列化行为测试失败
15. **ParticleSystemsHardcodedMisc2XmlTests** - 结构化相等性失败
16. **DebugMultiplayerScenes** - 序列化失败
17. **BeforeTransparentsGraphXmlTests** - 结构化相等性失败
18. **MpCraftingPiecesXmlTests** - 结构化相等性失败
19. **MpcosmeticsXmlTests** - 结构化相等性失败
20. **TempDebugTest** - 属性格式比较失败
21. **ParticleSystemsOutdoorXmlTests** - 结构化相等性失败

## 4. 根本原因分析

### 4.1 DO/DTO实现不完整
- 某些模型的DO层实现不完整，缺少部分XML属性
- ShouldSerialize方法逻辑不正确，导致属性序列化行为不一致
- 复杂嵌套结构支持不足

### 4.2 XML序列化排序问题
- XmlSerializer在序列化时可能改变元素顺序
- 命名空间声明处理不当
- 空值和默认值处理不一致

### 4.3 数据类型处理问题
- 布尔值序列化格式不一致（true/false vs True/False）
- 数值精度处理问题
- 字符串和null值区分不当

## 5. 技术约束和假设

### 5.1 技术约束
- 必须保持与原始XML结构的完全兼容性
- DO层所有属性必须使用字符串类型以避免类型转换问题
- DTO层提供类型安全的访问方法
- ShouldSerialize方法必须正确实现以控制序列化行为

### 5.2 假设
- 所有XML文件格式符合骑马与砍杀2的标准
- XML比较工具能够正确识别结构差异
- DO/DTO分层架构能够解决大部分序列化问题

## 6. 实施策略

### 6.1 优先级排序
1. **高优先级**: 已开始DO/DTO实现但仍有问题的测试（ActionTypes, BoneBodyTypes, ActionSets）
2. **中优先级**: 结构相对简单的XML模型
3. **低优先级**: 复杂嵌套结构或大型XML文件

### 6.2 实施步骤
1. 完善现有DO/DTO模型实现
2. 修复ShouldSerialize方法逻辑
3. 为剩余测试创建DO/DTO模型
4. 优化XML序列化/反序列化过程
5. 验证所有测试通过

## 7. 验证标准

### 7.1 成功指标
- 所有1059个测试必须通过
- XML序列化/反序列化必须保持结构完全一致
- DO/DTO分层架构必须正确实现
- 代码必须遵循现有架构模式和命名约定

### 7.2 质量要求
- 代码覆盖率不低于90%
- 无严重警告或错误
- 遵循C#编码规范
- 提供完整的单元测试