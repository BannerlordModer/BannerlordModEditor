# Bannerlord XML 适配验收标准

## 概述

本文档为BannerlordModEditor项目的XML适配工作提供详细的验收标准。每个验收标准都基于EARS格式（Easy to Read, Actionable, Requirements-focused, Specific），确保测试和验证的准确性。

## 通用验收标准

### GEN-001: 基础XML处理能力
**WHEN** 加载任何XML文件 **THEN** 系统应正确解析XML结构而不抛出异常  
**IF** XML文件格式不正确 **THEN** 系统应提供清晰的错误信息和修复建议  
**FOR** 所有支持的XML文件类型 **VERIFY** 文件大小和复杂度不影响基本加载功能  

### GEN-002: 序列化一致性
**WHEN** 执行XML序列化和反序列化往返 **THEN** 生成的XML应与原始XML逻辑等效  
**IF** XML包含空元素或可选属性 **THEN** 序列化结果应保持原始结构  
**FOR** 数值数据和字符串内容 **VERIFY** 精度和格式完全保持  

### GEN-003: 错误处理和恢复
**WHEN** 遇到XML解析错误 **THEN** 系统应提供详细的错误位置和原因  
**IF** 文件损坏或格式不正确 **THEN** 系统应尝试恢复部分数据而非完全失败  
**FOR** 所有错误情况 **VERIFY** 系统保持稳定且不丢失用户数据  

### GEN-004: 性能基准
**WHEN** 加载小于100KB的XML文件 **THEN** 加载时间应小于1秒  
**WHEN** 加载100KB-1MB的XML文件 **THEN** 加载时间应小于3秒  
**WHEN** 加载大于1MB的XML文件 **THEN** 加载时间应小于10秒  
**FOR** 所有文件操作 **VERIFY** 内存使用量在合理范围内  

## Epic 1: 多人游戏系统验收标准

### MP-001.1: MPClassDivisions.xml 基础适配
**WHEN** 加载MPClassDivisions.xml文件 **THEN** 系统应正确解析所有MPClassDivision元素  
**IF** 文件包含多个职业分类 **THEN** 所有职业都应被正确加载和识别  
**FOR** 每个MPClassDivision元素 **VERIFY** 所有必需属性(id, hero, troop等)正确解析  

**测试数据**:
```xml
<MPClassDivisions>
  <MPClassDivision id="mp_light_infantry_vlandia" 
                   hero="mp_light_infantry_vlandia_hero" 
                   troop="mp_light_infantry_vlandia_troop"
                   multiplier="0.92"
                   cost="80">
    <!-- 其他属性和子元素 -->
  </MPClassDivision>
</MPClassDivisions>
```

**预期结果**:
- 正确解析所有MPClassDivision元素
- 数值属性保持原始精度
- 字符串属性完整保留

### MP-001.2: 复杂嵌套结构处理
**WHEN** 解析MPClassDivision的Perks结构 **THEN** 系统应正确处理多层嵌套  
**IF** Perk包含多种效果类型 **THEN** 所有效果都应被正确识别和分类  
**FOR** 复杂的嵌套结构 **VERIFY** 层级关系和数据关联完整保持  

**测试数据**:
```xml
<Perks>
  <Perk game_mode="all" name="Improved Armor">
    <OnSpawnEffect type="ArmorOnSpawn" value="9" />
    <RandomOnSpawnEffect type="RandomEquipmentOnSpawn">
      <Group>
        <Item slot="Body" item="mp_cloth_tunic" />
      </Group>
    </RandomOnSpawnEffect>
  </Perk>
</Perks>
```

**预期结果**:
- 正确解析Perk及其子元素
- 支持多种效果类型(OnSpawnEffect, RandomOnSpawnEffect)
- 保持Group和Item的层级关系

### MP-001.3: 条件逻辑和游戏模式
**WHEN** 处理Perk的游戏模式属性 **THEN** 系统应正确识别不同的游戏模式  
**IF** Perk包含条件逻辑 **THEN** 系统应正确解析和应用条件限制  
**FOR** 所有条件属性 **VERIFY** 枚举值和字符串值正确处理  

**测试数据**:
```xml
<Perk game_mode="skirmish" name="Skirmish Only">
  <OnSpawnEffect type="GoldCostOnSpawn" value="10" />
</Perk>
<Perk game_mode="all" name="All Modes">
  <OnSpawnEffect type="ArmorOnSpawn" value="5" />
</Perk>
```

**预期结果**:
- 正确处理"all"、"skirmish"等游戏模式
- 条件逻辑正确应用
- 效果参数准确传递

## Epic 2: 编辑器UI系统验收标准

### UI-001.1: Layouts文件基础解析
**WHEN** 加载skeletons_layout.xml文件 **THEN** 系统应正确解析layout元数据结构  
**IF** 文件包含复杂的insertion_definitions **THEN** 所有定义都应被正确识别  
**FOR** 布局定义元素 **VERIFY** class、version、xml_tag等属性正确解析  

**测试数据**:
```xml
<base type="string">
  <layouts>
    <layout class="skeleton" version="0.1" xml_tag="skeletons.skeleton">
      <columns>
        <column id="0" width="400" />
      </columns>
      <insertion_definitions>
        <insertion_definition label="Add Hinge Joint">
          <default_node>
            <hinge_joint name="default_hinge" />
          </default_node>
        </insertion_definition>
      </insertion_definitions>
    </layout>
  </layouts>
</base>
```

**预期结果**:
- 正确解析base、layouts、layout等层级结构
- 准确提取layout的元数据属性
- 完整处理insertion_definitions和default_node

### UI-001.2: 复杂关节类型支持
**WHEN** 解析default_node中的关节类型 **THEN** 系统应支持所有关节类型(hinge_joint, d6_joint等)  
**IF** 关节包含复杂配置参数 **THEN** 所有参数都应被正确解析  
**FOR** 每种关节类型 **VERIFY** 特有属性和通用属性都正确处理  

**测试数据**:
```xml
<default_node>
  <hinge_joint name="default_hinge"
               bone1="_AUTO_GET_BONE_0_"
               bone2="_AUTO_GET_BONE_1_"
               pos="2.000, 0.000, 0.000"
               min_angle="0"
               max_angle="90" />
  <d6_joint name="default_d6"
            axis_lock_x="locked"
            axis_lock_y="locked"
            axis_lock_z="locked"
            twist_lock="limited"
            twist_lower_limit="0.000"
            twist_upper_limit="45.000" />
</default_node>
```

**预期结果**:
- 正确识别hinge_joint和d6_joint类型
- 准确解析位置、角度、锁定状态等参数
- 保持数值精度和枚举值

### UI-001.3: Layouts文件完整性验证
**WHEN** 验证Layouts文件 **THEN** 系统应检查所有必要字段和引用关系  
**IF** 发现缺失或无效的配置 **THEN** 系统应提供具体的错误信息  
**FOR** 所有Layouts文件 **VERIFY** 结构完整性和数据一致性  

**验证检查点**:
- [ ] layout元素包含必需的class、version属性
- [ ] insertion_definitions包含有效的label和xml_path
- [ ] default_node包含有效的关节配置
- [ ] 所有数值属性在合理范围内
- [ ] 字符串引用不为空或无效

## Epic 3: 地形系统验收标准

### TERR-001.1: TerrainMaterials基础适配
**WHEN** 加载terrain_materials.xml文件 **THEN** 系统应正确解析所有terrain_material元素  
**IF** 材质包含多层纹理配置 **THEN** 所有纹理都应被正确识别和分类  
**FOR** 每个terrain_material元素 **VERIFY** 材质属性和纹理配置完整保持  

**测试数据**:
```xml
<terrain_materials>
  <terrain_material name="default"
                   is_enabled="true"
                   is_flora_layer="false"
                   physics_material="soil">
    <textures>
      <texture type="diffusemap" name="editor_grid_8" />
      <texture type="areamap" name="none" />
      <texture type="normalmap" name="none" />
    </textures>
  </terrain_material>
</terrain_materials>
```

**预期结果**:
- 正确解析terrain_material及其属性
- 准确提取textures集合中的各个texture
- 保持纹理类型和名称的对应关系

### TERR-001.2: 复杂材质属性处理
**WHEN** 处理terrain_material的复杂属性 **THEN** 系统应正确解析数值向量和枚举值  
**IF** 材质包含高级渲染参数 **THEN** 所有参数都应被正确解析  
**FOR** 所有材质属性 **VERIFY** 数值精度和格式完全保持  

**测试数据**:
```xml
<terrain_material name="advanced_terrain"
                 pitch_roll_yaw="0.000, 0.000, 0.000"
                 scale="5.000, 5.000"
                 shear="0.000, 0.000"
                 position_offset="0.000, 0.000"
                 bigdetailmap_mode="0"
                 bigdetailmap_weight="0.000"
                 bigdetailmap_scale_x="0.080"
                 bigdetailmap_scale_y="0.080">
  <!-- 纹理配置 -->
</terrain_material>
```

**预期结果**:
- 正确解析三维向量(pitch_roll_yaw, scale等)
- 准确处理浮点数值，保持原始精度
- 正确处理枚举类型(bigdetailmap_mode)

## Epic 4: 游戏机制验收标准

### GAME-001.1: MovementSets基础适配
**WHEN** 加载movement_sets.xml文件 **THEN** 系统应正确解析所有movement_set元素  
**IF** 动作集包含大量动作引用 **THEN** 所有动作引用都应被正确识别  
**FOR** 每个movement_set元素 **VERIFY** 动作配置和引用关系完整保持  

**测试数据**:
```xml
<movement_sets>
  <movement_set id="walk_unarmed"
                idle="act_walk_idle_unarmed"
                forward="act_walk_forward_unarmed"
                backward="act_walk_backward_unarmed"
                right="act_walk_right_unarmed"
                left="act_walk_left_unarmed"
                rotate="act_turn_unarmed" />
</movement_sets>
```

**预期结果**:
- 正确解析movement_set及其所有动作属性
- 准确提取动作引用字符串
- 保持动作间的逻辑关系

### GAME-002.1: SkeletonScales基础适配
**WHEN** 加载skeleton_scales.xml文件 **THEN** 系统应正确解析所有骨骼缩放配置  
**IF** 骨骼包含多轴缩放参数 **THEN** 所有缩放参数都应被正确解析  
**FOR** 每个骨骼缩放配置 **VERIFY** 缩放比例和轴向关系正确保持  

**测试数据**:
```xml
<skeleton_scales>
  <skeleton_scale name="human"
                   scale="1.0, 1.0, 1.0"
                   bone_override="spine_2:1.2,1.0,1.0" />
</skeleton_scales>
```

**预期结果**:
- 正确解析skeleton_scale元素
- 准确提取基础缩放和骨骼覆盖
- 保持数值精度和格式

## Epic 5: 性能优化验收标准

### PERF-001.1: 大型文件加载性能
**WHEN** 加载1MB以上的XML文件 **THEN** 系统应在10秒内完成加载  
**IF** 文件大小超过5MB **THEN** 系统应显示进度指示器  
**FOR** 内存使用情况 **VERIFY** 内存增长在文件大小的2-3倍范围内  

**性能基准**:
- 文件大小: 1MB → 加载时间: < 3秒
- 文件大小: 5MB → 加载时间: < 8秒
- 文件大小: 10MB → 加载时间: < 15秒
- 内存使用: < 文件大小 × 3

### PERF-001.2: 流式处理验证
**WHEN** 使用流式处理大型文件 **THEN** 系统应保持响应性和稳定性  
**IF** 处理过程中发生错误 **THEN** 系统应恢复并报告已处理的数据  
**FOR** 流式处理结果 **VERIFY** 与完整加载的结果完全一致  

**验证方法**:
1. 使用XmlReader流式处理大型文件
2. 监控内存使用情况
3. 验证数据完整性
4. 测试错误恢复能力

## Epic 6: 质量保证验收标准

### QA-001.1: 数据完整性验证
**WHEN** 执行数据完整性检查 **THEN** 系统应验证所有必要字段和约束  
**IF** 发现数据不一致 **THEN** 系统应提供详细的修复建议  
**FOR** 所有XML文件类型 **VERIFY** 验证规则覆盖所有业务需求  

**验证检查点**:
- [ ] 必需字段存在性检查
- [ ] 数据类型验证
- [ ] 数值范围检查
- [ ] 字符串格式验证
- [ ] 引用完整性检查
- [ ] 枚举值有效性检查

### QA-001.2: 序列化往返测试
**WHEN** 执行序列化往返测试 **THEN** 生成的XML应与原始XML逻辑等效  
**IF** XML包含空元素或可选属性 **THEN** 往返结果应保持原始结构  
**FOR** 所有数据类型 **VERIFY** 精度和格式完全保持  

**测试方法**:
1. 加载原始XML文件
2. 反序列化为C#对象
3. 序列化回XML格式
4. 比较原始XML和生成XML的逻辑等价性
5. 验证数据完整性

### QA-002.1: 自动化测试覆盖
**WHEN** 运行自动化测试套件 **THEN** 所有测试应通过且覆盖率达到90%以上  
**IF** 发现测试失败 **THEN** 系统应提供详细的失败报告和调试信息  
**FOR** 代码质量指标 **VERIFY** 满足预定的质量标准  

**测试覆盖率要求**:
- 代码覆盖率: ≥ 90%
- 分支覆盖率: ≥ 85%
- 行覆盖率: ≥ 95%
- 功能测试覆盖率: 100%

## 特殊场景验收标准

### SPECIAL-001: 错误恢复能力
**WHEN** 遇到损坏的XML文件 **THEN** 系统应尝试恢复可用数据而非完全失败  
**IF** 文件包含格式错误 **THEN** 系统应准确定位错误位置并提供修复建议  
**FOR** 所有错误情况 **VERIFY** 系统保持稳定且不丢失有效数据  

### SPECIAL-002: 版本兼容性
**WHEN** 处理不同版本的XML文件 **THEN** 系统应支持向后兼容性  
**IF** 文件包含新字段或结构 **THEN** 系统应优雅处理未知字段  
**FOR** 版本兼容性 **VERIFY** 新旧版本文件都能正确处理  

### SPECIAL-003: 本地化支持
**WHEN** 处理包含本地化字符串的XML **THEN** 系统应正确处理UTF-8编码  
**IF** 字符串包含特殊字符 **THEN** 系统应保持字符完整性  
**FOR** 多语言支持 **VERIFY** 所有字符都能正确显示和处理  

## 验证流程

### 1. 单元测试验证
- 每个适配的XML类型对应一个测试类
- 测试覆盖所有主要功能场景
- 包含正向和负向测试用例

### 2. 集成测试验证
- 测试XML文件间的交互和引用
- 验证端到端的数据流程
- 测试用户界面的集成功能

### 3. 性能测试验证
- 验证大型文件的处理性能
- 测试内存使用和资源管理
- 验证并发处理能力

### 4. 用户验收测试
- 实际用户场景测试
- 用户体验评估
- 功能完整性验证

## 验收检查清单

### Phase 1: 基础适配验收
- [ ] MPClassDivisions.xml 完全适配
- [ ] Layouts目录文件完全适配
- [ ] TerrainMaterials.xml 完全适配
- [ ] MovementSets.xml 完全适配
- [ ] SkeletonScales.xml 完全适配

### Phase 2: 功能验收
- [ ] 用户界面功能完整
- [ ] 数据编辑功能正常
- [ ] 文件保存和加载功能正常
- [ ] 错误处理机制完善
- [ ] 性能指标达标

### Phase 3: 质量验收
- [ ] 所有测试用例通过
- [ ] 代码覆盖率达标
- [ ] 文档完整且准确
- [ ] 用户体验良好
- [ ] 性能优化完成

## 总结

本验收标准文档为BannerlordModEditor项目的XML适配工作提供了全面、具体、可执行的验收标准。通过严格按照这些标准进行测试和验证，可以确保项目的高质量交付。

验收标准基于EARS格式，确保了每个标准的可读性、可执行性、需求相关性和具体性。同时，标准涵盖了功能、性能、质量等多个维度，为项目的成功提供了全面的保障。