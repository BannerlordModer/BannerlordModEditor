# ParticleSystems XML序列化修复用户故事

## 概述: ParticleSystems XML序列化修复

## Epic: XML序列化一致性修复

### Story: PXS-001 - 修复decal_materials元素序列化问题
**As a** Mod开发者  
**I want** ParticleSystems XML中的decal_materials元素在序列化后保持正确的结构和位置  
**So that** 我可以正确编辑和保存粒子系统的贴花材质配置

**Acceptance Criteria** (EARS格式):
- **WHEN** 序列化包含decal_materials的ParticleSystems XML **THEN** decal_materials元素必须作为parameters的直接子元素正确序列化
- **IF** parameters元素包含decal_materials子元素 **THEN** 序列化后必须保持相同的相对位置
- **FOR** 所有包含decal_materials的effect元素 **VERIFY** 每个decal_materials包含完整的12个decal_material子元素

**Technical Notes**:
- decal_materials与parameter元素平级，不是parameter的子元素
- 需要确保XML序列化顺序的正确性
- 可能需要自定义序列化逻辑

**Story Points**: 8  
**Priority**: High

### Story: PXS-002 - 保持XML元素顺序一致性
**As a** XML处理系统  
**I want** 序列化后的XML元素顺序与原始文件完全一致  
**So that** 结构化对比测试能够通过，确保数据完整性

**Acceptance Criteria** (EARS格式):
- **WHEN** 反序列化然后重新序列化ParticleSystems XML **THEN** 所有元素必须保持原始顺序
- **IF** 原始XML中parameter元素在decal_materials之前 **THEN** 序列化后必须保持相同顺序
- **FOR** 所有嵌套的emitter、parameters、children元素 **VERIFY** 相对位置完全一致

**Technical Notes**:
- XML序列化器默认可能改变元素顺序
- 需要使用XmlElementAttribute的Order属性
- 考虑使用自定义序列化来保证顺序

**Story Points**: 5  
**Priority**: High

### Story: PXS-003 - 正确处理空元素和复杂嵌套结构
**As a** 数据完整性守护者  
**I want** 正确处理XML中的空元素和复杂嵌套关系  
**So that** 序列化后的XML结构与原始文件完全匹配

**Acceptance Criteria** (EARS格式):
- **WHEN** 处理包含空emitters或parameters的effect元素 **THEN** 空元素必须正确保留或按规则处理
- **IF** emitter元素包含空的children、flags或parameters子元素 **THEN** 必须正确序列化这些空元素
- **FOR** 所有多层嵌套的XML结构 **VERIFY** 嵌套关系完全保持一致

**Technical Notes**:
- 需要仔细处理ShouldSerialize方法的逻辑
- 空元素的处理需要符合XML规范
- 复杂嵌套结构需要递归处理

**Story Points**: 8  
**Priority**: High

### Story: PXS-004 - 确保大文件处理性能
**As a** 系统性能优化者  
**I want** 1.7MB的ParticleSystems XML文件能够在合理时间内完成序列化处理  
**So that** 用户体验不会因为文件大小而受到影响

**Acceptance Criteria** (EARS格式):
- **WHEN** 处理1.7MB的ParticleSystems XML文件 **THEN** 反序列化时间必须小于3秒
- **IF** 进行XML结构化对比 **THEN** 对比时间必须小于1秒
- **FOR** 多次连续处理 **VERIFY** 内存使用稳定，无内存泄漏

**Technical Notes**:
- 需要监控内存使用情况
- 考虑使用流式处理优化性能
- 避免不必要的字符串操作

**Story Points**: 3  
**Priority**: Medium

### Story: PXS-005 - 添加XmlTestUtils特殊处理逻辑
**As a** 测试框架使用者  
**I want** XmlTestUtils能够正确处理ParticleSystems的特殊序列化需求  
**So that** 测试能够准确验证XML结构一致性

**Acceptance Criteria** (EARS格式):
- **WHEN** 使用XmlTestUtils序列化ParticleSystems对象 **THEN** 必须应用特殊的处理逻辑
- **IF** 序列化过程中遇到decal_materials元素 **THEN** 必须确保其正确位置和格式
- **FOR** 所有ParticleSystems相关的XML测试 **VERIFY** 使用增强后的XmlTestUtils进行验证

**Technical Notes**:
- 需要在XmlTestUtils中添加ParticleSystems类型的特殊处理
- 可能需要保留原始XML的某些格式信息
- 确保不影响其他XML类型的处理

**Story Points**: 5  
**Priority**: High

### Story: PXS-006 - 创建全面的验证测试
**As a** 质量保证工程师  
**I want** 创建全面的测试用例来验证ParticleSystems XML序列化修复  
**So that** 确保修复的完整性和可靠性

**Acceptance Criteria** (EARS格式):
- **WHEN** 运行ParticleSystems测试套件 **THEN** 所有测试必须通过
- **IF** 修改了序列化逻辑 **THEN** 必须运行回归测试确保无副作用
- **FOR** 各种边界情况 **VERIFY** 都有对应的测试覆盖

**Technical Notes**:
- 需要创建正测试和负测试用例
- 测试应该覆盖各种XML结构变化
- 考虑添加性能测试

**Story Points**: 5  
**Priority**: Medium

### Story: PXS-007 - 确保向后兼容性
**As a** 系统架构师  
**I want** 确保修复不会破坏现有的功能和API  
**So that** 现有用户可以无缝升级到修复版本

**Acceptance Criteria** (EARS格式):
- **WHEN** 应用修复后的代码 **THEN** 所有现有的XML处理功能必须正常工作
- **IF** 使用现有的DO/DTO API **THEN** 行为必须保持一致
- **FOR** 所有其他XML类型的测试 **VERIFY** 仍然通过

**Technical Notes**:
- 需要运行完整的回归测试套件
- 确保API接口保持不变
- 监控潜在的副作用

**Story Points**: 3  
**Priority**: Medium

## 技术实现细节

### 关键技术挑战

1. **XML序列化顺序控制**
   - 使用XmlElementAttribute的Order属性
   - 可能需要实现IXmlSerializable接口
   - 考虑使用XmlSerializer的自定义处理

2. **复杂嵌套结构处理**
   - 递归处理多层嵌套
   - 正确处理空元素和null值
   - 保持XML命名空间和属性

3. **性能优化**
   - 避免不必要的XML解析
   - 使用StringBuilder进行字符串操作
   - 考虑内存使用优化

### 代码实现策略

```csharp
// 示例：可能需要修改的序列化逻辑
[XmlElement("parameter", Order = 1)]
public List<ParameterDO> ParameterList { get; set; } = new List<ParameterDO>();

[XmlElement("decal_materials", Order = 2)]
public DecalMaterialsDO? DecalMaterials { get; set; }
```

### 测试策略

1. **单元测试**: 测试各个组件的序列化逻辑
2. **集成测试**: 测试完整的XML处理流程
3. **性能测试**: 确保大文件处理性能
4. **回归测试**: 确保不影响现有功能

## 验收标准总结

### 必须满足的标准
- [ ] 所有ParticleSystems测试通过
- [ ] XML结构100%一致
- [ ] 性能指标达标
- [ ] 无回归问题
- [ ] 代码质量符合标准

### 期望达到的标准
- [ ] 测试覆盖率 > 90%
- [ ] 文档完整更新
- [ ] 性能优化到位
- [ ] 错误处理完善