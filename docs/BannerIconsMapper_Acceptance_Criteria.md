# BannerIconsMapper 类型转换修复验收标准

## 概述

本文档定义了BannerIconsMapper类型转换修复的具体验收标准，确保所有功能都按照预期工作。

## 通用验收标准

### AC-GEN-001: 空值处理
**描述**：所有映射器必须正确处理null输入  
**验收方法**：单元测试  
**预期结果**：
- 输入null时返回null
- 不抛出NullReferenceException
- 日志记录适当的调试信息

### AC-GEN-002: 异常处理
**描述**：所有映射器必须优雅处理异常情况  
**验收方法**：单元测试  
**预期结果**：
- 格式错误的数值输入不会导致崩溃
- 格式错误的布尔值输入不会导致崩溃
- 提供合理的默认值或保持原样

### AC-GEN-003: 性能基准
**描述**：修复后的映射器性能不能低于当前实现  
**验收方法**：性能测试  
**预期结果**：
- 单次映射操作响应时间 < 10ms
- 内存使用增长率 < 5%
- 无内存泄漏

## BannerIconGroupMapper 验收标准

### AC-BIG-001: Id属性类型转换
**描述**：验证Id属性在DO和DTO之间正确转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 正常数值
var source = new BannerIconGroupDO { Id = "123" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("123", result.Id);

// 测试用例2: 空值
var source = new BannerIconGroupDO { Id = null };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Null(result.Id);

// 测试用例3: 空字符串
var source = new BannerIconGroupDO { Id = "" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("", result.Id);
```

### AC-BIG-002: IsPattern属性类型转换
**描述**：验证IsPattern属性在DO和DTO之间正确转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: true值
var source = new BannerIconGroupDO { IsPattern = "true" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("true", result.IsPattern);

// 测试用例2: false值
var source = new BannerIconGroupDO { IsPattern = "false" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("false", result.IsPattern);

// 测试用例3: null值
var source = new BannerIconGroupDO { IsPattern = null };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Null(result.IsPattern);
```

### AC-BIG-003: 双向映射一致性
**描述**：验证DO到DTO再到DO的往返映射保持数据一致性  
**验收方法**：单元测试  
**测试数据**：
```csharp
var original = new BannerIconGroupDO 
{ 
    Id = "456", 
    Name = "Test Group", 
    IsPattern = "true" 
};

var dto = BannerIconGroupMapper.ToDTO(original);
var result = BannerIconGroupMapper.ToDO(dto);

Assert.Equal(original.Id, result.Id);
Assert.Equal(original.Name, result.Name);
Assert.Equal(original.IsPattern, result.IsPattern);
```

## BackgroundMapper 验收标准

### AC-BG-001: Id和IsBaseBackground类型转换
**描述**：验证Background属性的类型转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 完整属性
var source = new BackgroundDO 
{ 
    Id = "789", 
    MeshName = "test_mesh", 
    IsBaseBackground = "true" 
};
var result = BackgroundMapper.ToDTO(source);
Assert.Equal("789", result.Id);
Assert.Equal("test_mesh", result.MeshName);
Assert.Equal("true", result.IsBaseBackground);

// 测试用例2: 可选布尔属性缺失
var source = new BackgroundDO 
{ 
    Id = "790", 
    MeshName = "test_mesh_2", 
    IsBaseBackground = null 
};
var result = BackgroundMapper.ToDTO(source);
Assert.Equal("790", result.Id);
Assert.Equal("test_mesh_2", result.MeshName);
Assert.Null(result.IsBaseBackground);
```

## IconMapper 验收标准

### AC-ICON-001: 数值属性类型转换
**描述**：验证Icon的Id和TextureIndex数值类型转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 正常数值
var source = new IconDO 
{ 
    Id = "100", 
    MaterialName = "test_material", 
    TextureIndex = "5" 
};
var result = IconMapper.ToDTO(source);
Assert.Equal("100", result.Id);
Assert.Equal("test_material", result.MaterialName);
Assert.Equal("5", result.TextureIndex);

// 测试用例2: TextureIndex为0
var source = new IconDO 
{ 
    Id = "101", 
    MaterialName = "test_material_2", 
    TextureIndex = "0" 
};
var result = IconMapper.ToDTO(source);
Assert.Equal("101", result.Id);
Assert.Equal("test_material_2", result.MaterialName);
Assert.Equal("0", result.TextureIndex);
```

### AC-ICON-002: IsReserved布尔属性转换
**描述**：验证IsReserved布尔属性转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: IsReserved为true
var source = new IconDO 
{ 
    Id = "160", 
    MaterialName = "custom_banner_icons_15", 
    TextureIndex = "7", 
    IsReserved = "true" 
};
var result = IconMapper.ToDTO(source);
Assert.Equal("true", result.IsReserved);

// 测试用例2: IsReserved为false
var source = new IconDO 
{ 
    Id = "100", 
    MaterialName = "custom_banner_icons_01", 
    TextureIndex = "10", 
    IsReserved = "false" 
};
var result = IconMapper.ToDTO(source);
Assert.Equal("false", result.IsReserved);

// 测试用例3: IsReserved不存在
var source = new IconDO 
{ 
    Id = "100", 
    MaterialName = "custom_banner_icons_01", 
    TextureIndex = "10", 
    IsReserved = null 
};
var result = IconMapper.ToDTO(source);
Assert.Null(result.IsReserved);
```

## ColorEntryMapper 验收标准

### AC-COLOR-001: Id和布尔属性转换
**描述**：验证ColorEntry的Id和布尔属性转换  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 完整属性
var source = new ColorEntryDO 
{ 
    Id = "0", 
    Hex = "0xffB57A1E", 
    PlayerCanChooseForBackground = "true", 
    PlayerCanChooseForSigil = "true" 
};
var result = ColorEntryMapper.ToDTO(source);
Assert.Equal("0", result.Id);
Assert.Equal("0xffB57A1E", result.Hex);
Assert.Equal("true", result.PlayerCanChooseForBackground);
Assert.Equal("true", result.PlayerCanChooseForSigil);

// 测试用例2: 混合布尔值
var source = new ColorEntryDO 
{ 
    Id = "1", 
    Hex = "0xff4E1A13", 
    PlayerCanChooseForBackground = "false", 
    PlayerCanChooseForSigil = "true" 
};
var result = ColorEntryMapper.ToDTO(source);
Assert.Equal("1", result.Id);
Assert.Equal("0xff4E1A13", result.Hex);
Assert.Equal("false", result.PlayerCanChooseForBackground);
Assert.Equal("true", result.PlayerCanChooseForSigil);
```

## XML序列化一致性验收标准

### AC-XML-001: 完整往返测试
**描述**：验证完整的XML序列化/反序列化往返过程  
**验收方法**：集成测试  
**测试步骤**：
1. 读取原始banner_icons.xml文件
2. 使用XmlTestUtils.Deserialize<BannerIconsDO>反序列化
3. 使用XmlTestUtils.Serialize序列化回XML
4. 使用XmlTestUtils.AreStructurallyEqual比较结构一致性

**预期结果**：
- 结构相等性检查返回true
- 节点数量差异为0
- 属性数量差异为0
- 属性值差异为空
- 文本差异为空

### AC-XML-002: 特定属性验证
**描述**：验证特定属性在序列化过程中的正确性  
**验收方法**：集成测试  
**验证点**：
- BannerIconGroup的id属性保持数值字符串格式
- BannerIconGroup的is_pattern属性保持布尔字符串格式
- Background的is_base_background属性保持布尔字符串格式
- Icon的texture_index属性保持数值字符串格式
- Icon的is_reserved属性保持布尔字符串格式
- ColorEntry的玩家选择属性保持布尔字符串格式

## 错误处理验收标准

### AC-ERR-001: 格式错误数值处理
**描述**：验证格式错误数值的处理  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 非数值字符串
var source = new BannerIconGroupDO { Id = "not_a_number" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("not_a_number", result.Id); // 保持原样

// 测试用例2: 超大数值
var source = new BannerIconGroupDO { Id = "99999999999999999999" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("99999999999999999999", result.Id); // 保持原样
```

### AC-ERR-002: 格式错误布尔值处理
**描述**：验证格式错误布尔值的处理  
**验收方法**：单元测试  
**测试数据**：
```csharp
// 测试用例1: 非标准布尔值
var source = new BannerIconGroupDO { IsPattern = "yes" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("yes", result.IsPattern); // 保持原样

// 测试用例2: 数字布尔值
var source = new BannerIconGroupDO { IsPattern = "1" };
var result = BannerIconGroupMapper.ToDTO(source);
Assert.Equal("1", result.IsPattern); // 保持原样
```

## 性能验收标准

### AC-PERF-001: 映射性能
**描述**：验证映射器性能满足要求  
**验收方法**：性能测试  
**测试条件**：
- 测试数据：完整的banner_icons.xml（包含所有BannerIconGroup、Background、Icon、ColorEntry）
- 迭代次数：1000次
- 测试环境：标准开发环境

**预期结果**：
- 平均响应时间 < 10ms
- 95th percentile响应时间 < 20ms
- 内存使用稳定，无泄漏

### AC-PERF-002: 内存使用
**描述**：验证映射器内存使用情况  
**验收方法**：内存分析  
**预期结果**：
- 单次映射操作内存增长 < 1KB
- 垃圾回收频率合理
- 无内存泄漏

## 测试覆盖验收标准

### AC-TEST-001: 代码覆盖
**描述**：确保足够的代码覆盖  
**验收方法**：代码覆盖率分析  
**预期结果**：
- 映射器方法代码覆盖率达到100%
- 分支覆盖率达到90%以上
- 所有边界情况都有测试覆盖

### AC-TEST-002: 测试数据覆盖
**描述**：确保测试数据覆盖所有场景  
**验收方法**：测试数据审查  
**预期结果**：
- 覆盖所有属性类型（数值、布尔、字符串）
- 覆盖所有边界情况（null、空字符串、格式错误）
- 覆盖真实的XML数据结构

## 验收流程

### 阶段1: 单元测试验证
1. 运行所有映射器单元测试
2. 确保所有测试通过
3. 检查代码覆盖率

### 阶段2: 集成测试验证
1. 运行XML序列化往返测试
2. 验证结构一致性
3. 检查生成的调试文件

### 阶段3: 性能测试验证
1. 执行性能基准测试
2. 分析内存使用情况
3. 确认性能指标达标

### 阶段4: 回归测试验证
1. 运行完整测试套件
2. 确保没有回归问题
3. 验证现有功能正常

## 验收检查清单

### 必须检查项目
- [ ] 所有映射器单元测试通过
- [ ] XML序列化往返测试通过
- [ ] 性能测试达标
- [ ] 代码覆盖率达标
- [ ] 没有回归问题
- [ ] 错误处理正常工作
- [ ] 边界情况正确处理

### 文档检查项目
- [ ] 代码注释完整
- [ ] 测试文档更新
- [ ] 验收标准文档完整
- [ ] 用户故事文档更新

### 部署检查项目
- [ ] 构建成功
- [ ] 部署包生成正常
- [ ] 版本信息正确
- [ ] 依赖关系正确

## 验收失败处理

### 轻微失败
- 单个测试用例失败
- 性能略微不达标
- 文档不完整

**处理方式**：修复后重新验收

### 严重失败
- 核心功能测试失败
- XML序列化不一致
- 性能严重不达标

**处理方式**：停止部署，重新设计和实现

### 阻塞性失败
- 系统崩溃
- 数据丢失
- 严重回归问题

**处理方式**：立即回滚，紧急修复