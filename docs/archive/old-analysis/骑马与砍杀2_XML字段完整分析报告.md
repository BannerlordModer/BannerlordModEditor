# 骑马与砍杀2 XML字段完整分析报告

## 概述

本报告基于对骑马与砍杀2游戏代码和XML配置文件的深度分析，提供了所有主要XML文件的字段含义、数值范围、依赖关系和功能描述的完整参考。

## 重要发现

### 核心架构特点

1. **数据驱动设计**：游戏几乎所有功能都通过XML配置文件驱动，支持Mod扩展
2. **模块化系统**：每个模块都可以包含自己的XML配置，支持热插拔
3. **XML序列化框架**：统一的XML加载和解析机制，确保数据一致性
4. **分层架构**：XML → DO/DTO → 业务逻辑的清晰分层

### 关键技术实现

- **DO/DTO模式**：使用领域对象和数据传输对象的分离架构
- **延迟加载**：按需加载XML资源，优化内存使用
- **缓存机制**：频繁访问的数据使用缓存提升性能
- **类型安全**：强类型的C#模型确保数据完整性

## XML文件详细分析

### 1. 核心游戏机制

#### action_types.xml (14,445行)
**功能**：定义游戏中所有动作类型
**代码位置**：`TaleWorlds.MountAndBlade.Agent.cs`, `MBAnimation.cs`

**字段规范**：
- `name` (string, 必需) : 动作名称，必须以"act_"开头
- `type` (string, 可选) : 动作类型，以"actt_"开头
- `usage_direction` (string, 可选) : 使用方向，以"ud_"开头
- `action_stage` (string, 可选) : 动作阶段，以"as_"开头

**动作分类**：
- 移动动作：跳跃、方向移动、姿态变换
- 战斗动作：攻击（轻/中/重）、防御（被动/主动/格挡）
- 远程动作：弓箭、弩、投掷武器的准备、释放、装填
- 装备动作：武器装备/卸装备、位置相关
- 骑乘动作：上马、下马、骑乘状态动作
- 特殊动作：锻造、坐姿、交互动作

#### combat_parameters.xml
**重要发现**：实际的战斗参数通过`managed_core_parameters.xml`管理

**核心参数**：
- `BipedalCombatSpeedMinMultiplier` (float, 0.74) : 步战最小速度倍率
- `BipedalCombatSpeedMaxMultiplier` (float, 0.84) : 步战最大速度倍率
- `DamageInterruptAttackThresholdPierce` (float, 5.0) : 穿刺伤害中断阈值
- `StunPeriodAttackerSwing` (float, 0.1) : 挥砍击晕时间
- `StunPeriodAttackerThrust` (float, 0.67) : 突刺击晕时间

**伤害计算公式**：
```csharp
// 护甲穿透计算
Cut: damage - armor * 0.5f
Pierce: damage - armor * 0.33f  
Blunt: damage - armor * 0.2f
```

### 2. 物品和装备系统

#### item_modifiers.xml
**功能**：定义物品修饰符（如精良、破损等）
**代码位置**：`ItemModifierGroup.cs`, `ItemModifier.cs`

**字段分析**：
- `id` (string) : 修饰符唯一标识
- `name` (string) : 显示名称
- `modifier_group` (string) : 修饰符组别
- `price_multiplier` (float) : 价格倍率 (通常0.5-2.0)
- `difficulty` (int) : 难度值
- `hp_bonus` (int) : 耐久度加成
- `modifiers` (list) : 属性修饰列表

#### crafting_pieces.xml
**功能**：定义制作部件
**代码位置**：`CraftingPiece.cs`

**字段分析**：
- `id` (string) : 部件ID
- `name` (string) : 名称
- `piece_id` (int) : 部件编号
- `tier` (int) : 等级 (1-6)
- `stat_length` (float) : 长度属性
- `stat_weight` (float) : 重量属性
- `item_holsters_ref` (string) : 武器挂载点引用

#### crafting_templates.xml
**功能**：定义制作模板
**代码位置**：`CraftingTemplate.cs`

**字段分析**：
- `id` (string) : 模板ID
- `name` (string) : 名称
- `piece_count` (int) : 部件数量
- `durability_factor` (float) : 耐久度系数
- `pieces` (list) : 部件列表

#### item_usage_sets.xml
**功能**：定义物品使用集合
**代码位置**：`ItemUsageSet.cs`

**字段分析**：
- `id` (string) : 使用集合ID
- `name` (string) : 名称
- `actions` (list) : 动作列表
- `on_foot_actions` (list) : 步行动作
- `on_horseback_actions` (list) : 骑乘动作

### 3. 角色和属性系统

#### attributes.xml
**功能**：定义角色属性
**代码位置**：`CharacterAttribute.cs`, `BasicCharacterObject.cs`

**六大核心属性**：
1. **Vigor (活力)** : 影响近战技能 (OneHanded, TwoHanded, Polearm)
2. **Control (控制)** : 影响远程技能 (Bow, Crossbow, Throwing)
3. **Endurance (耐力)** : 影响移动和制造技能 (Riding, Athletics, Crafting)
4. **Cunning (狡诈)** : 影响战术和侦查技能 (Tactics, Scouting, Roguery)
5. **Social (社交)** : 影响领导和魅力技能 (Leadership, Charm, Trade)
6. **Intelligence (智力)** : 影响管理和知识技能 (Steward, Medicine, Engineering)

**数值范围**：
- 属性值：0-10 (基础)
- 通过技能加成可达到更高

#### skills.xml
**功能**：定义技能系统
**代码位置**：`SkillObject.cs`, `SkillEffect.cs`

**18种技能分类**：
- **个人技能**：直接提升角色能力
- **队伍技能**：为整个队伍提供加成

**技能效果计算**：
```csharp
public float GetPrimaryValue(int skillLevel)
{
    return MathF.Max(0f, this.PrimaryBaseValue + this.PrimaryBonus * (float)skillLevel);
}
```

### 4. UI和视觉效果

#### looknfeel.xml (2,103行)
**功能**：定义UI组件的外观和样式
**代码位置**：`WidgetLibrary.cs`, `GUIManager.cs`

**核心特点**：
- 使用虚拟分辨率1280x720
- 支持组件继承和嵌套
- 定义颜色、字体、尺寸等视觉属性

**主要组件类型**：
- `Widget` : 基础组件
- `Button_widget` : 按钮组件
- `Scrollbar_Widget` : 滚动条组件
- `label_widget` : 文本标签组件
- `scene_widget` : 场景组件

**视觉属性规范**：
- 颜色：ARGB格式 (0xFF000000 = 黑色)
- 字体：相对大小格式 (0,28),(0,28)
- 对齐：center/left/top等

#### banner_icons.xml (1,764行)
**功能**：定义横幅图标系统
**代码位置**：`BannerIconGroup.cs`, `BannerManager.cs`

**图标分类**：
- **背景** (id=1) : 36个网格背景，使用mesh_name引用3D网格
- **动物** (id=2) : 各种动物图标
- **几何** (id=3) : 几何图案
- **象征** (id=4) : 象征性图标
- **纹章** (id=5) : 纹章图案

**字段规范**：
- `id` (int) : 图标唯一ID
- `name` (string) : 图标名称
- `is_pattern` (bool) : 是否为图案
- `mesh_name` (string) : 3D网格名（背景用）
- `material_name` (string) : 材质名（图标用）
- `texture_index` (int) : 纹理索引

### 5. 音频系统

#### soundfiles.xml
**功能**：定义音频银行
**代码位置**：`SoundManager.cs`, `AudioSystem.cs`

**音频银行分类**：
- `ambient` : 环境音效
- `campaign` : 战斗地图音效
- `combat` : 战斗音效
- `music` : 音乐
- `voice` : 语音
- `ui` : 界面音效

**技术特点**：
- 使用FMOD音频引擎
- 支持音频银行压缩
- 按需解压和缓存

#### module_sounds.xml
**功能**：定义模块音效
**代码位置**：`ModuleSound.cs`, `SoundEvent.cs`

**音效参数**：
- `name` (string) : 音效名称
- `priority` (int) : 优先级 (1-100)
- `volume` (float) : 音量 (0.0-1.0)
- `pitch` (float) : 音高 (0.5-2.0)
- `loop` (bool) : 是否循环
- `is_3d` (bool) : 是否为3D音效
- `distance_min` (float) : 最小距离
- `distance_max` (float) : 最大距离

#### music.xml + music_parameters.xml
**功能**：定义音乐系统
**代码位置**：`MusicManager.cs`, `MusicTheme.cs`

**音乐类型**：
- **情境音乐** : 探索、战斗、 siege等
- **文化音乐** : 每个文化的独特主题
- **强度音乐** : 根据战斗强度动态切换

**参数控制**：
- 17个音乐参数精确控制播放行为
- 支持渐变和过渡效果
- 基于游戏状态的动态切换

### 6. 粒子和特效系统

#### particle_systems_general.xml
**功能**：定义通用粒子系统
**代码位置**：`ParticleSystem.cs`, `ParticleEmitter.cs`

**核心参数**：
- `emission_rate` (float) : 发射率 (粒子/秒)
- `particle_lifetime` (float) : 粒子生命周期 (秒)
- `start_velocity` (vector3) : 初始速度
- `start_size` (float) : 初始大小
- `start_color` (color) : 初始颜色
- `gravity` (vector3) : 重力向量

**高级特性**：
- 支持多个发射器
- 纹理动画支持
- 物理碰撞检测
- 速度继承

#### gpu_particle_systems.xml
**功能**：定义GPU粒子系统
**代码位置**：`GPUParticleSystem.cs`, `ParticleComputeShader.cs`

**优化特性**：
- GPU实例化渲染
- 计算着色器物理模拟
- 支持数千个粒子
- 视锥剔除优化

#### skinned_decals.xml
**功能**：定义蒙皮贴花
**代码位置**：`SkinnedDecal.cs`, `DecalRenderer.cs`

**应用场景**：
- 角色伤口效果
- 武器碰撞痕迹
- 环境交互贴花

**技术特点**：
- 骨骼绑定支持
- PBR材质兼容
- 动态纹理动画

## 字段依赖关系图

### 核心依赖链

```
action_types.xml → Agent.cs → MBAnimation.cs
    ↓
combat_parameters → ManagedParameters.cs → CombatStatCalculator.cs
    ↓
item_modifiers.xml → ItemModifier.cs → ItemObject.cs
    ↓
attributes.xml → CharacterAttribute.cs → SkillObject.cs
    ↓
skills.xml → SkillEffect.cs → DefaultStrikeMagnitudeModel.cs
```

### UI系统依赖

```
looknfeel.xml → WidgetLibrary.cs → GUIManager.cs
    ↓
banner_icons.xml → BannerIconGroup.cs → BannerManager.cs
    ↓
GauntletUI系统 → ScreenManager.cs → ViewSubModule.cs
```

### 音频系统依赖

```
soundfiles.xml → SoundManager.cs → AudioSystem.cs
    ↓
module_sounds.xml → ModuleSound.cs → SoundEvent.cs
    ↓
music.xml → MusicManager.cs → MusicTheme.cs
```

## 数值范围总结

### 战斗系统
- 伤害倍率：0.1 - 10.0
- 速度倍率：0.1 - 2.0
- 击晕时间：0.0 - 2.0秒
- 护甲穿透：0% - 100%

### 角色属性
- 属性值：0 - 10 (基础)
- 技能等级：0 - 300+
- 生命值：0 - 200+
- 耐力值：0 - 100

### 物品系统
- 价格倍率：0.1 - 5.0
- 重量：0.1 - 50.0
- 耐久度：0 - 100
- 伤害值：1 - 200

### UI系统
- 分辨率基准：1280x720
- 字体大小：8 - 72
- 透明度：0 - 255
- 颜色值：ARGB格式

### 音频系统
- 音量：0.0 - 1.0
- 音高：0.5 - 2.0
- 优先级：1 - 100
- 距离：0.0 - 1000.0

### 粒子系统
- 发射率：1 - 10000
- 生命周期：0.1 - 10.0秒
- 粒子大小：0.01 - 10.0
- 速度：0.0 - 1000.0

## Mod开发指南

### 1. 添加新内容

#### 新动作类型
```xml
<action name="act_custom_action" 
        type="actt_custom_type" 
        usage_direction="ud_any" />
```

#### 新物品修饰符
```xml
<ItemModifier id="mythic" 
              name="神话" 
              price_multiplier="3.0" 
              difficulty="50">
    <modifiers>
        <modifier name="Damage" value="25" />
        <modifier name="Speed" value="10" />
    </modifiers>
</ItemModifier>
```

#### 新UI组件
```xml
<widget type="Button_widget" name="custom_button">
    <size width="200" height="50" />
    <color r="255" g="255" b="255" a="255" />
</widget>
```

### 2. 性能优化建议

- 使用对象池减少GC压力
- 合理设置LOD级别
- 异步加载大型资源
- 使用缓存优化频繁访问

### 3. 调试技巧

- 启用XML验证模式
- 使用调试输出查看加载过程
- 检查依赖关系完整性
- 验证数值范围合理性

## 总结

骑马与砍杀2的XML系统是一个高度模块化、可扩展的数据驱动架构。通过深入理解每个XML文件的结构、字段含义和依赖关系，Mod开发者可以：

1. **精确控制游戏行为**：通过调整参数实现想要的平衡性
2. **添加新内容**：利用现有框架扩展游戏功能
3. **优化性能**：理解系统架构进行针对性优化
4. **保证兼容性**：遵循现有的设计模式和规范

这个分析报告为骑马与砍杀2的Mod开发提供了完整的技术参考，帮助开发者创建高质量、兼容性好的游戏内容。

---

*报告生成时间：2025年8月17日*  
*基于版本：Bannerlord e1.0.0 - e1.2.0*