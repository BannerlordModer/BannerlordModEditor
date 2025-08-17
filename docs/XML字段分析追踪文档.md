# XML字段分析追踪文档

## 概述
本文档追踪骑马与砍杀2中所有XML文件与其对应代码的映射关系，包括字段含义、数值范围和依赖关系。

## XML文件分类

### 1. 核心游戏机制
- action_types.xml - 动作类型定义
- action_sets.xml - 动作集合
- combat_parameters.xml - 战斗参数
- attributes.xml - 角色属性
- skills.xml - 技能系统
- item_usage_sets.xml - 物品使用集合

### 2. 物品和装备
- item_modifiers.xml - 物品修饰符
- item_modifiers_groups.xml - 物品修饰符组
- crafting_pieces.xml - 制作部件
- crafting_templates.xml - 制作模板
- weapon_descriptions.xml - 武器描述

### 3. 角色和生物
- characters.xml - 角色定义
- monsters.xml - 怪物定义
- monster_usage_sets.xml - 怪物使用集合
- bone_body_types.xml - 骨骼身体类型
- skins.xml - 皮肤定义

### 4. UI和视觉效果
- looknfeel.xml - 界面外观
- banner_icons.xml - 旗帜图标
- map_icons.xml - 地图图标
- gauges.xml - 计量器（待确认）
- before_transparents_graph.xml - 透明前渲染图

### 5. 音频系统
- soundfiles.xml - 音效文件
- module_sounds.xml - 模块音效
- voice_definitions.xml - 语音定义
- voices.xml - 语音
- music.xml - 音乐
- music_parameters.xml - 音乐参数

### 6. 粒子和特效
- particle_systems_*.xml - 粒子系统系列
- gpu_particle_systems.xml - GPU粒子系统
- skinned_decals.xml - 蒙皮贴花
- decal_textures_*.xml - 贴花纹理系列

### 7. 物理和材质
- physics_materials.xml - 物理材质
- cloth_materials.xml - 布料材质
- terrain_materials.xml - 地形材质
- collision_infos.xml - 碰撞信息

### 8. 多人和游戏参数
- mpcultures.xml - 多人文化
- mpitems.xml - 多人物品
- mpcharacters.xml - 多人角色
- managed_core_parameters.xml - 核心参数
- native_parameters.xml - 原生参数

## 分析状态

| XML文件 | 状态 | 主要代码位置 | 完成度 |
|---------|------|-------------|--------|
| action_types.xml | 待分析 | | 0% |
| combat_parameters.xml | 待分析 | | 0% |
| attributes.xml | 待分析 | | 0% |
| item_modifiers.xml | 待分析 | | 0% |
| looknfeel.xml | 待分析 | | 0% |
| ... | ... | ... | 0% |

## 字段分析模板

### XML文件名：`filename.xml`
**代码位置**：`路径到代码文件`
**功能描述**：简要描述该XML的用途

#### 字段详细分析

| 字段名 | 类型 | 必需 | 数值范围 | 默认值 | 描述 | 依赖关系 |
|--------|------|------|----------|--------|------|----------|
| field1 | string | 是 | - | - | 字段描述 | 依赖其他字段/系统 |
| field2 | float | 否 | 0.0-1.0 | 0.5 | 数值字段 | 影响xxx系统 |

#### 重要发现
- 发现1：字段间的依赖关系
- 发现2：特殊的数值约束
- 发现3：与其他系统的交互

## 代码搜索模式

### 常用搜索关键词
1. XML文件名（不含扩展名）
2. "Deserialize" + XML名
3. "Serialize" + XML名
4. "Load" + XML名
5. "Save" + XML名

### 代码文件模式
1. `MBObjectManager` - 对象管理器
2. `XmlSerializer` - XML序列化
3. `Module` - 模块系统
4. `GameManager` - 游戏管理器

## 注意事项
1. 某些XML可能有多个读取位置
2. 部分字段可能有隐藏的约束条件
3. 需要关注字段的版本兼容性