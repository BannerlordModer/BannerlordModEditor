# 物品相关XML文件分析需求文档

## 项目概述

本文档详细分析了骑马与砍杀2（Bannerlord）Mod编辑器中四个关键的物品相关XML文件的需求和功能要求。这些文件构成了游戏物品系统的核心配置，涵盖了物品修饰符、制作系统、制作模板和使用设置等方面。

## 执行摘要

本次分析涉及四个核心XML配置文件：
- **item_modifiers.xml** - 物品修饰符系统，定义物品的质量等级和属性修正
- **crafting_pieces.xml** - 制作零件系统，定义可制作的武器零件及其属性
- **crafting_templates.xml** - 制作模板系统，定义武器制作的结构和规则
- **item_usage_sets.xml** - 物品使用设置系统，定义物品的使用动作和行为

## 利益相关者

### 主要用户
- **Mod开发者** - 需要理解和修改物品系统的高级用户
- **内容创作者** - 需要配置物品属性和行为的创作者
- **游戏设计师** - 需要平衡游戏物品系统的设计师

### 系统管理员
- **Mod管理器** - 需要管理Mod兼容性和依赖性的工具开发者
- **服务器管理员** - 需要配置多人游戏物品系统的管理员

## 功能需求

### FR-001: 物品修饰符系统 (item_modifiers.xml)
**描述**: 实现完整的物品修饰符配置系统，支持不同类型物品的质量等级和属性修正

**优先级**: 高
**接受标准**:
- [ ] 支持所有21种物品修饰符组（sword, bow, crossbow, arrow, bolt, cheap_weapon, polearm, mace, axe, axe_throwing, knife_throwing, spear_dart_throwing, shield, plate, chain, leather, cloth, cloth_unarmoured, horse, companion）
- [ ] 每个修饰符组包含5个质量等级（legendary, masterwork/fine, balanced/quality, inferior/dented/bent, poor/cracked/rusty/splintered）
- [ ] 支持属性修正：damage, speed, missile_speed, hit_points, armor, horse_speed, maneuver, charge_damage, horse_hit_points, stack_count, price_factor
- [ ] 支持掉落概率和生产概率配置（loot_drop_score, production_drop_score）
- [ ] 支持本地化名称配置（使用{ITEMNAME}占位符）
- [ ] 支持质量等级标识（quality属性）

### FR-002: 制作零件系统 (crafting_pieces.xml)
**描述**: 实现武器制作零件配置系统，支持复杂的武器制作和自定义

**优先级**: 高
**接受标准**:
- [ ] 支持多种零件类型（Blade, Handle, Guard, Pommel等）
- [ ] 支持零件属性配置（tier, piece_type, culture, mesh, length, weight等）
- [ ] 支持伤害数据配置（Thrust, Swing伤害类型和系数）
- [ ] 支持物理材质配置（physics_material）
- [ ] 支持材料需求配置（Materials系统）
- [ ] 支持标志配置（Flags系统，如NoBlood, Civilian等）
- [ ] 支持隐藏零件配置（is_hidden属性）
- [ ] 支持制作成本配置（CraftingCost）

### FR-003: 制作模板系统 (crafting_templates.xml)
**描述**: 实现武器制作模板系统，定义完整的武器制作流程和规则

**优先级**: 高
**接受标准**:
- [ ] 支持武器类型定义（OneHandedWeapon, TwoHandedWeapon等）
- [ ] 支持零件组装顺序配置（build_order）
- [ ] 支持可用零件列表配置（UsablePieces）
- [ ] 支持武器属性范围配置（StatsData）
- [ ] 支持武器描述配置（WeaponDescriptions）
- [ ] 支持武器挂载配置（item_holsters）
- [ ] 支持挂载缩放配置（piece_type_to_scale_holster_with）

### FR-004: 物品使用设置系统 (item_usage_sets.xml)
**描述**: 实现物品使用行为配置系统，定义物品的使用动作和动画

**优先级**: 高
**接受标准**:
- [ ] 支持多种使用场景（闲置、移动、攻击等）
- [ ] 支持左右手姿态配置（is_left_stance）
- [ ] 支持动作配置（ready_action, release_action等）
- [ ] 支持攻击类型配置（strike_type: thrust, swing）
- [ ] 支持马背使用配置（is_mounted）
- [ ] 支持手势要求配置（require_free_left_hand）
- [ ] 支持音效配置（equip_sound, unequip_sound）
- [ ] 支持标志配置（flags系统）

## 非功能需求

### NFR-001: 性能要求
**描述**: 系统必须能够高效处理大型XML文件，支持快速加载和保存操作

**指标**:
- XML文件加载时间 < 3秒（对于10MB文件）
- 内存使用量 < 文件大小的3倍
- 支持异步操作，避免UI阻塞

### NFR-002: 数据完整性
**描述**: 确保XML数据的完整性和一致性，防止数据损坏和丢失

**标准**:
- 支持XML验证和格式检查
- 提供数据备份和恢复功能
- 实现事务性操作，确保数据一致性

### NFR-003: 可扩展性
**描述**: 系统架构必须支持未来功能的扩展和新物品类型的添加

**要求**:
- 模块化设计，支持插件式扩展
- 提供API接口供第三方开发者使用
- 支持自定义属性和配置项

### NFR-004: 用户体验
**描述**: 提供直观易用的用户界面，降低Mod开发的学习曲线

**标准**:
- 提供可视化编辑器
- 支持实时预览功能
- 提供详细的帮助文档和教程
- 支持批量操作和模板功能

## 系统依赖关系

### 核心依赖
- **item_modifiers.xml** → **crafting_pieces.xml**（修饰符影响零件属性）
- **crafting_pieces.xml** → **crafting_templates.xml**（零件在模板中使用）
- **crafting_templates.xml** → **item_usage_sets.xml**（制作完成的物品需要使用设置）
- **item_usage_sets.xml** → **action_sets.xml**（使用设置引用动作定义）

### 外部依赖
- **attributes.xml** - 属性系统，影响物品属性计算
- **skills.xml** - 技能系统，影响制作和使用条件
- **cultures.xml** - 文化系统，影响物品文化属性
- **materials.xml** - 材料系统，影响制作材料需求

## 数据模型设计

### 物品修饰符数据模型
```csharp
public class ItemModifier
{
    public string ModifierGroup { get; set; }        // 修饰符组
    public string Id { get; set; }                   // 唯一标识
    public string Name { get; set; }                 // 显示名称
    public int? LootDropScore { get; set; }          // 掉落概率
    public int? ProductionDropScore { get; set; }    // 生产概率
    public int? Damage { get; set; }                // 伤害修正
    public int? Speed { get; set; }                 // 速度修正
    public int? MissileSpeed { get; set; }           // 投射速度修正
    public float? PriceFactor { get; set; }          // 价格系数
    public string Quality { get; set; }              // 质量等级
    // ... 其他属性
}
```

### 制作零件数据模型
```csharp
public class CraftingPiece
{
    public string Id { get; set; }                   // 零件ID
    public string Name { get; set; }                 // 零件名称
    public string PieceType { get; set; }            // 零件类型
    public string Tier { get; set; }                 // 等级
    public string Culture { get; set; }              // 文化
    public string Mesh { get; set; }                 // 网格模型
    public CraftingPieceData PieceData { get; set; } // 零件数据
    public Materials Materials { get; set; }         // 材料需求
    public Flags Flags { get; set; }                 // 标志
}
```

### 制作模板数据模型
```csharp
public class CraftingTemplate
{
    public string Id { get; set; }                   // 模板ID
    public string ItemType { get; set; }             // 物品类型
    public string ModifierGroup { get; set; }        // 修饰符组
    public PieceDatas PieceDatas { get; set; }       // 零件数据
    public UsablePieces UsablePieces { get; set; }   // 可用零件
    public List<StatsData> StatsDataList { get; set; } // 统计数据
}
```

### 物品使用设置数据模型
```csharp
public class ItemUsageSet
{
    public string Id { get; set; }                   // 使用设置ID
    public bool HasSingleStance { get; set; }        // 单一姿态
    public string BaseSet { get; set; }              // 基础设置
    public ItemUsageIdles Idles { get; set; }        // 闲置动作
    public ItemUsageMovementSets MovementSets { get; set; } // 移动设置
    public ItemUsageUsages Usages { get; set; }      // 使用动作
}
```

## 约束条件

### 技术约束
- 必须使用.NET 9.0和C# 9.0特性
- 必须支持UTF-8编码的XML文件
- 必须保持与现有项目架构的兼容性
- 必须支持大型XML文件的分片处理

### 业务约束
- 必须支持骑马与砍杀2的游戏机制
- 必须保持与原生XML文件的兼容性
- 必须支持Mod的加载和卸载
- 必须支持多人游戏环境

### 数据约束
- 必须保持XML结构的完整性
- 必须支持所有原生属性和配置
- 必须支持本地化字符串
- 必须支持数值范围验证

## 假设条件

- 用户具有基本的XML编辑知识
- 游戏版本保持稳定，API不会频繁变更
- Mod开发者遵循标准的命名约定
- 用户理解游戏机制和平衡性原则

## 风险评估

| 风险 | 影响 | 概率 | 缓解策略 |
|------|------|------|----------|
| XML格式变更 | 高 | 中 | 实现灵活的解析器，支持格式适配 |
| 性能问题 | 中 | 中 | 实现缓存机制和异步处理 |
| 数据损坏 | 高 | 低 | 实现数据验证和备份功能 |
| 兼容性问题 | 中 | 中 | 提供版本管理和兼容性检查 |

## 实施建议

### 阶段1: 基础架构
1. 设计统一的数据模型架构
2. 实现XML序列化/反序列化基础
3. 建立数据验证机制

### 阶段2: 核心功能
1. 实现物品修饰符系统
2. 实现制作零件系统
3. 实现制作模板系统

### 阶段3: 高级功能
1. 实现物品使用设置系统
2. 实现系统集成和依赖管理
3. 实现用户界面和交互功能

### 阶段4: 优化和测试
1. 性能优化和内存管理
2. 全面测试和质量保证
3. 文档编写和用户培训

## 成功指标

### 技术指标
- 所有XML文件解析成功率 > 99%
- 系统响应时间 < 2秒
- 内存使用效率 > 80%
- 代码覆盖率 > 90%

### 用户指标
- 用户满意度 > 85%
- 学习曲线时间 < 4小时
- 错误率 < 5%
- 功能使用率 > 70%

### 业务指标
- Mod开发效率提升 > 50%
- 支持的Mod数量 > 100
- 用户活跃度 > 60%
- 社区贡献度 > 30%

## 附录

### A. 术语表
- **ItemModifier**: 物品修饰符，影响物品属性和质量
- **CraftingPiece**: 制作零件，武器的组成部分
- **CraftingTemplate**: 制作模板，定义武器制作规则
- **ItemUsageSet**: 物品使用设置，定义物品使用行为
- **ModifierGroup**: 修饰符组，物品类型的分类

### B. 参考资料
- 骑马与砍杀2官方Mod开发文档
- Bannerlord Modding Community Wiki
- XML Schema Definition (XSD) 标准
- .NET XML Serialization 最佳实践

### C. 版本历史
- v1.0 - 初始需求分析
- v1.1 - 添加详细数据模型
- v1.2 - 完善风险评估和成功指标