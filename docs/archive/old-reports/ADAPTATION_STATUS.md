# XML文件适配状态对比

## 📊 总体状态概览
- **Example XML文件总数**: ~130+ 个
- **已适配POCO类数量**: ~65+ 个  
- **适配完成率**: ~50%

## ✅ 已适配完成的文件 (POCO类存在)

### 📁 Data 目录 (27个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `achievements_data.xml` | `Models/Data/AchievementData.cs` | ✅ |
| `Adjustables.xml` | `Models/Misc/Adjustables.cs` | ✅ |
| `attributes.xml` | `Models/Attributes.cs` | ✅ |
| `banner_icons.xml` | `Models/Data/BannerIcons.cs` | ✅ |
| `bone_body_types.xml` | `Models/Data/BoneBodyTypes.cs` | ✅ |
| `Credits.xml` | `Models/Data/Credits.cs` | ✅ |
| `decal_sets.xml` | `Models/Data/DecalSets.cs` | ✅ |
| `flora_groups.xml` | `Models/Data/FloraGroups.cs` | ✅ |
| `hard_coded_sounds.xml` | `Models/Data/HardCodedSounds.cs` | ✅ |
| `map_icons.xml` | `Models/Map/MapIcons.cs` | ✅ |
| `map_tree_types.xml` | `Models/Data/MapTreeTypes.cs` | ✅ |
| `monsters.xml` | `Models/Data/Monsters.cs` | ✅ |
| `mpbadges.xml` | `Models/Data/MPBadges.cs` | ✅ |
| `mpcharacters.xml` | `Models/Data/MPCharacters.cs` | ✅ |
| `mpcultures.xml` | `Models/MpCultures.cs` | ✅ |
| `mpitems.xml` | `Models/MpItems.cs` | ✅ |
| `music.xml` | `Models/Data/Music.cs` | ✅ |
| `music_parameters.xml` | `Models/Data/MusicParameters.cs` | ✅ |
| `objects.xml` | `Models/Objects.cs` | ✅ |
| `parties.xml` | `Models/Game/Parties.cs` | ✅ |
| `photo_mode_strings.xml` | `Models/Data/PhotoModeStrings.cs` | ✅ |
| `physics_materials.xml` | `Models/PhysicsMaterials.cs` | ✅ |
| `random_terrain_templates.xml` | `Models/Data/RandomTerrainTemplates.cs` | ✅ |
| `scenes.xml` | `Models/Data/Scenes.cs` | ✅ |
| `siegeengines.xml` | `Models/SiegeEngines.cs` | ✅ |
| `skeleton_scales.xml` | `Models/Data/SkeletonScales.cs` | ✅ |
| `skills.xml` | `Models/Data/Skills.cs` | ✅ |
| `skinned_decals.xml` | `Models/Data/SkinnedDecals.cs` | ✅ |
| `skins.xml` | `Models/Data/Skins.cs` | ✅ |
| `voices.xml` | `Models/Data/Voices.cs` | ✅ |
| `water_prefabs.xml` | `Models/Data/WaterPrefabs.cs` | ✅ |
| `weapon_descriptions.xml` | `Models/Data/WeaponDescriptions.cs` | ✅ |
| `worldmap_color_grades.xml` | `Models/Data/WorldmapColorGrades.cs` | ✅ |

### 📁 Engine 目录 (10个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `action_sets.xml` | `Models/Engine/ActionSets.cs` | ✅ |
| `cloth_bodies.xml` | `Models/Engine/ClothBodies.cs` | ✅ |
| `cloth_materials.xml` | `Models/Engine/ClothMaterials.cs` | ✅ |
| `collision_infos.xml` | `Models/Engine/CollisionInfos.cs` | ✅ |
| `combat_parameters.xml` | `Models/Engine/CombatParameters.cs` | ✅ |
| `gpu_particle_systems.xml` | `Models/Engine/GpuParticleSystems.cs` | ✅ |
| `managed_core_parameters.xml` | `Models/Engine/CoreParameters.cs` | ✅ |
| `before_transparents_graph.xml` | `Models/Engine/PostfxGraphs.cs` | ✅ |
| `special_meshes.xml` | `Models/Engine/SpecialMeshes.cs` | ✅ |
| `terrain_materials.xml` | `Models/Engine/TerrainMaterials.cs` | ✅ |

### 📁 Audio 目录 (2个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `module_sounds.xml` | `Models/Audio/ModuleSounds.cs` | ✅ |
| `soundfiles.xml` | `Models/Audio/SoundFiles.cs` | ✅ |

### 📁 Game 目录 (5个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `crafting_pieces.xml` | `Models/Game/CraftingPieces.cs` | ✅ |
| `crafting_templates.xml` | `Models/Game/CraftingTemplates.cs` | ✅ |
| `item_modifiers.xml` | `Models/Game/ItemModifiers.cs` | ✅ |
| `managed_campaign_parameters.xml` | `Models/Game/ManagedCampaignParameters.cs` | ✅ |

### 📁 Multiplayer 目录 (2个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `MultiplayerScenes.xml` | `Models/Multiplayer/MultiplayerScenes.cs` | ✅ |
| `taunt_usage_sets.xml` | `Models/Multiplayer/TauntUsageSets.cs` | ✅ |

### 📁 杂项/其他 (4个已适配)
| XML文件 | POCO类位置 | 状态 |
|---------|-----------|------|
| `native_equipment_sets.xml` | `Models/SimpleXmlTypes.cs` | ✅ |
| `native_skill_sets.xml` | `Models/SimpleXmlTypes.cs` | ✅ |
| `mpbodypropertytemplates.xml` | `Models/SimpleXmlTypes.cs` | ✅ |
| `SkillData` (相关) | `Models/SkillData.cs` | ✅ |

## ❌ 尚未适配的文件 (需要创建POCO类)

### 🔥 高优先级 - 数据类文件 (15个)
| XML文件 | 建议POCO位置 | 复杂度 | 优先级 |
|---------|-------------|--------|--------|
| `item_holsters.xml` | `Models/Data/ItemHolsters.cs` | 中 | 🔥 |
| `item_modifiers_groups.xml` | `Models/Game/ItemModifiersGroups.cs` | 低 | 🔥 |
| `monster_usage_sets.xml` | `Models/Data/MonsterUsageSets.cs` | 中 | 🔥 |
| `movement_sets.xml` | `Models/Engine/MovementSets.cs` | 高 | 🔥 |
| `full_movement_sets.xml` | `Models/Engine/FullMovementSets.cs` | 高 | 🔥 |
| `flora_kinds.xml` | `Models/Data/FloraKinds.cs` | 超高 | 🔥 |
| `flora_layer_sets.xml` | `Models/Data/FloraLayerSets.cs` | 高 | 🔥 |
| `native_parameters.xml` | `Models/Configuration/NativeParameters.cs` | 中 | 🔥 |
| `looknfeel.xml` | `Models/Configuration/LookNFeel.cs` | 高 | 🔥 |
| `mp_crafting_pieces.xml` | `Models/Game/MpCraftingPieces.cs` | 高 | 🔥 |
| `mpcosmetics.xml` | `Models/Data/MpCosmetics.cs` | 高 | 🔥 |
| `mpclassdivisions.xml` | `Models/Data/MpClassDivisions.cs` | 高 | 🔥 |
| `item_usage_sets.xml` | `Models/Data/ItemUsageSets.cs` | 超高 | 🔥 |
| `action_types.xml` | `Models/Engine/ActionTypes.cs` | 超高 | 🔥 |
| `voice_definitions.xml` | `Models/Audio/VoiceDefinitions.cs` | 高 | 🔥 |

### 🎨 中优先级 - 渲染/图形类文件 (12个)
| XML文件 | 建议POCO位置 | 复杂度 | 优先级 |
|---------|-------------|--------|--------|
| `postfx_graphs.xml` | `Models/Engine/PostfxGraphs.cs` (扩展) | 高 | 🎨 |
| `thumbnail_postfx_graphs.xml` | `Models/Engine/ThumbnailPostfxGraphs.cs` | 高 | 🎨 |
| `prerender.xml` | `Models/Engine/Prerender.cs` | 中 | 🎨 |
| `decal_textures_all.xml` | `Models/Engine/DecalTexturesAll.cs` | 中 | 🎨 |
| `decal_textures_battle.xml` | `Models/Engine/DecalTexturesBattle.cs` | 中 | 🎨 |
| `decal_textures_multiplayer.xml` | `Models/Engine/DecalTexturesMultiplayer.cs` | 中 | 🎨 |
| `decal_textures_town.xml` | `Models/Engine/DecalTexturesTown.cs` | 中 | 🎨 |
| `decal_textures_worldmap.xml` | `Models/Engine/DecalTexturesWorldmap.cs` | 中 | 🎨 |
| `particle_systems_basic.xml` | `Models/Engine/ParticleSystemsBasic.cs` | 超高 | 🎨 |
| `particle_systems_general.xml` | `Models/Engine/ParticleSystemsGeneral.cs` | 超高 | 🎨 |
| `particle_systems_map_icon.xml` | `Models/Engine/ParticleSystemsMapIcon.cs` | 高 | 🎨 |
| `particle_systems_outdoor.xml` | `Models/Engine/ParticleSystemsOutdoor.cs` | 超高 | 🎨 |

### 💬 低优先级 - 本地化/配置文件 (8个)
| XML文件 | 建议POCO位置 | 复杂度 | 优先级 |
|---------|-------------|--------|--------|
| `global_strings.xml` | `Models/Configuration/GlobalStrings.cs` | 低 | 💬 |
| `module_strings.xml` | `Models/Configuration/ModuleStrings.cs` | 低 | 💬 |
| `multiplayer_strings.xml` | `Models/Configuration/MultiplayerStrings.cs` | 低 | 💬 |
| `native_strings.xml` | `Models/Configuration/NativeStrings.cs` | 低 | 💬 |
| `CreditsExternalPartnersPC.xml` | `Models/Data/CreditsExternalPartnersPC.cs` | 极低 | 💬 |
| `CreditsExternalPartnersXBox.xml` | `Models/Data/CreditsExternalPartnersXBox.cs` | 极低 | 💬 |
| `CreditsExternalPartnersPlayStation.xml` | `Models/Data/CreditsExternalPartnersPlayStation.cs` | 极低 | 💬 |
| `CreditsLegalConsole.xml` | `Models/Data/CreditsLegalConsole.cs` | 极低 | 💬 |

### 🎭 特殊/巨型文件 (6个)
| XML文件 | 建议POCO位置 | 复杂度 | 优先级 |
|---------|-------------|--------|--------|
| `particle_systems_hardcoded_misc1.xml` | `Models/Engine/ParticleSystemsHardcodedMisc1.cs` | 巨型 | ⚠️ |
| `particle_systems_hardcoded_misc2.xml` | `Models/Engine/ParticleSystemsHardcodedMisc2.cs` | 巨型 | ⚠️ |
| `particle_systems_old.xml` | `Models/Engine/ParticleSystemsOld.cs` | 巨型 | ⚠️ |
| `particle_systems2.xml` | `Models/Engine/ParticleSystems2.cs` | 巨型 | ⚠️ |
| `prebaked_animations.xml` | `Models/Engine/PrebakedAnimations.cs` | 巨型 | ⚠️ |
| `project.mbproj` | (非XML,项目文件) | - | - |

## 📋 待适配子目录

### 📁 Languages/ 目录
- `std_*.xml` 文件 (25+个本地化文件)
- 建议位置: `Models/Languages/`

### 📁 Layouts/ 目录  
- 布局定义文件 (7个)
- 建议位置: `Models/Layouts/`

### 📁 AchievementData/ 目录
- 成就相关数据文件 (1个)
- 建议位置: `Models/Data/AchievementData/`

### 📁 CoreGameReferences/ 目录
- 核心游戏引用 (非XML文件)

## 🎯 下一步适配建议

### ✅ 最新完成 (本次会话)
1. ✅ `item_modifiers_groups.xml` → `Models/Game/ItemModifiersGroups.cs`
2. ✅ `native_parameters.xml` → `Models/Configuration/NativeParameters.cs`
3. ✅ `prerender.xml` → `Models/Engine/PostfxGraphs.cs` (扩展支持)
4. ✅ `item_holsters.xml` → `Models/Game/ItemHolsters.cs`
5. ✅ `monster_usage_sets.xml` → `Models/Data/MonsterUsageSets.cs`

### 立即可开始 (简单文件，1-2小时)
1. `prerender.xml` - 预渲染配置
2. `decal_textures_*.xml` 系列 - 贴花纹理配置
3. `monster_usage_sets.xml` - 怪物使用设置

### 近期目标 (中等复杂度，2-4小时)
1. `item_holsters.xml` - 武器收纳配置
2. `monster_usage_sets.xml` - 怪物使用设置
3. `voice_definitions.xml` - 语音定义

### 长期规划 (高复杂度，需要数天)
1. `movement_sets.xml` / `full_movement_sets.xml` - 动作系统
2. `flora_kinds.xml` / `flora_layer_sets.xml` - 植被系统  
3. `item_usage_sets.xml` / `action_types.xml` - 物品使用系统

## ⚡ 优化建议
- 优先适配业务逻辑相关的文件 (Game/, Data/ 目录)
- 推迟巨型粒子系统文件的适配
- 本地化文件可以最后处理
- 建立自动化测试保证质量 