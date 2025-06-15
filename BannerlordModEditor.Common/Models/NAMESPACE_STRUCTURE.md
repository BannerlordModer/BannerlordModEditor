# Bannerlord XML Models Namespace Structure

## 🏗️ Current Namespace Organization

### ✅ Implemented Namespaces

#### `BannerlordModEditor.Common.Models` (Root Level - Legacy)
- **MpItems.cs** - 多人游戏物品（复杂，674行，需要重构）
- **SkillData.cs** - 技能数据
- **Attributes.cs** - 属性定义
- **MpCultures.cs** - 多人游戏文化
- **Objects.cs** - 游戏对象
- **SiegeEngines.cs** - 攻城器械
- **PhysicsMaterials.cs** - 物理材质
- **SimpleParametersAndSystems.cs** - 简单参数系统
- **SimpleXmlTypes.cs** - 简单XML类型
- **AdditionalSimpleTypes.cs** - 附加简单类型

#### `BannerlordModEditor.Common.Models.Audio` ✅
- **SoundFiles.cs** - 音频文件配置

#### `BannerlordModEditor.Common.Models.Engine` ✅
- **CoreParameters.cs** - 核心引擎参数

#### `BannerlordModEditor.Common.Models.Configuration` ✅ (Created but Empty)
- 预留给配置文件相关模型

## 🎯 Recommended Future Namespace Structure

### Core Game Systems
```
BannerlordModEditor.Common.Models.Engine/
├── CoreParameters.cs ✅
├── PhysicsParameters.cs (physics_materials.xml)
├── ParticleSystems.cs (各种particle_systems_*.xml)
└── CombatParameters.cs (combat_parameters.xml)
```

### Audio System
```
BannerlordModEditor.Common.Models.Audio/
├── SoundFiles.cs ✅
├── ModuleSounds.cs (module_sounds.xml - NEXT TARGET)
├── HardcodedSounds.cs (hard_coded_sounds.xml)
└── MusicConfiguration.cs (music.xml, music_parameters.xml)
```

### Game Configuration
```
BannerlordModEditor.Common.Models.Configuration/
├── Skills.cs (skills.xml)
├── Attributes.cs (attributes.xml)
├── Objects.cs (objects.xml)
├── Parties.cs (parties.xml)
├── MapIcons.cs (map_icons.xml)
└── Adjustables.cs (Adjustables.xml)
```

### Items & Equipment
```
BannerlordModEditor.Common.Models.Items/
├── MpItems.cs (mpitems.xml) - MOVE FROM ROOT
├── ItemModifiers.cs (item_modifiers.xml)
├── ItemHolsters.cs (item_holsters.xml)
├── CraftingPieces.cs (crafting_pieces.xml, mp_crafting_pieces.xml)
└── WeaponDescriptions.cs (weapon_descriptions.xml)
```

### Multiplayer
```
BannerlordModEditor.Common.Models.Multiplayer/
├── MpCultures.cs (mpcultures.xml) - MOVE FROM ROOT
├── MpCharacters.cs (mpcharacters.xml)
├── MpClassDivisions.cs (mpclassdivisions.xml)
├── MpCosmetics.cs (mpcosmetics.xml)
├── MpBadges.cs (mpbadges.xml)
├── TauntUsageSets.cs (Multiplayer/taunt_usage_sets.xml)
└── MultiplayerScenes.cs (Multiplayer/MultiplayerScenes.xml)
```

### Characters & Monsters
```
BannerlordModEditor.Common.Models.Characters/
├── Monsters.cs (monsters.xml)
├── MonsterUsageSets.cs (monster_usage_sets.xml)
├── BodyProperties.cs (mpbodypropertytemplates.xml) - MOVE FROM ROOT
├── BoneBodyTypes.cs (bone_body_types.xml) - MOVE FROM ROOT
└── SkeletonScales.cs (skeleton_scales.xml)
```

### World & Environment
```
BannerlordModEditor.Common.Models.World/
├── Flora.cs (flora_kinds.xml, flora_groups.xml, flora_layer_sets.xml)
├── TerrainMaterials.cs (terrain_materials.xml)
├── RandomTerrainTemplates.cs (random_terrain_templates.xml)
├── MapTreeTypes.cs (map_tree_types.xml)
├── WorldmapColorGrades.cs (worldmap_color_grades.xml)
└── WaterPrefabs.cs (water_prefabs.xml)
```

### Animations & Actions
```
BannerlordModEditor.Common.Models.Animation/
├── ActionSets.cs (action_sets.xml)
├── ActionTypes.cs (action_types.xml)
├── MovementSets.cs (movement_sets.xml, full_movement_sets.xml)
├── PrebakedAnimations.cs (prebaked_animations.xml)
└── ItemUsageSets.cs (item_usage_sets.xml)
```

### UI & Visuals
```
BannerlordModEditor.Common.Models.Visual/
├── Skins.cs (skins.xml)
├── SpecialMeshes.cs (special_meshes.xml) - MOVE FROM ROOT
├── ClothBodies.cs (cloth_bodies.xml)
├── ClothMaterials.cs (cloth_materials.xml)
├── SkinnedDecals.cs (skinned_decals.xml)
├── DecalTextures.cs (decal_textures_*.xml)
└── DecalSets.cs (decal_sets.xml)
```

### System & Technical
```
BannerlordModEditor.Common.Models.System/
├── Scenes.cs (scenes.xml)
├── CollisionInfos.cs (collision_infos.xml)
├── PostfxGraphs.cs (postfx_graphs.xml, thumbnail_postfx_graphs.xml)
├── Prerender.cs (prerender.xml)
├── LookNFeel.cs (looknfeel.xml)
└── Voices.cs (voices.xml, voice_definitions.xml)
```

### Localization
```
BannerlordModEditor.Common.Models.Localization/
├── GlobalStrings.cs (global_strings.xml)
├── ModuleStrings.cs (module_strings.xml)
├── NativeStrings.cs (native_strings.xml)
├── MultiplayerStrings.cs (multiplayer_strings.xml)
└── PhotoModeStrings.cs (photo_mode_strings.xml)
```

### Parameters & Settings
```
BannerlordModEditor.Common.Models.Parameters/
├── ManagedCampaignParameters.cs (managed_campaign_parameters.xml) - MOVE FROM ROOT
├── ManagedCoreParameters.cs → MOVED TO Engine/CoreParameters.cs ✅
├── NativeParameters.cs (native_parameters.xml)
└── MusicParameters.cs (music_parameters.xml)
```

### Achievement & Data
```
BannerlordModEditor.Common.Models.Data/
├── AchievementData.cs (AchievementData/gog_achievement_data.xml)
├── Credits.cs (Credits*.xml)
└── BannerIcons.cs (banner_icons.xml)
```

## 🔄 Migration Plan

### Phase 1: Immediate (Next Session)
1. Create `BannerlordModEditor.Common.Models.Audio.ModuleSounds` class
2. Create `BannerlordModEditor.Common.Models.Data.AchievementData` class

### Phase 2: Reorganization (Future Sessions)
1. Move large files to appropriate namespaces:
   - `MpItems.cs` → `Items/`
   - `MpCultures.cs` → `Multiplayer/`
   - `SpecialMeshes.cs` → `Visual/`
2. Create missing namespace directories
3. Update all test files to use new namespaces

### Phase 3: Advanced (Long-term)
1. Split large model files into multiple smaller files
2. Add comprehensive XML documentation comments
3. Implement common base classes for shared functionality

## 🏆 Benefits of This Structure

### Code Organization
- **Logical Grouping:** Related models grouped by game system
- **Easy Navigation:** Clear namespace hierarchy
- **Maintainability:** Each file focused on single responsibility

### Development Experience
- **IntelliSense:** Better auto-completion by namespace
- **Import Clarity:** Explicit using statements show dependencies
- **Testing:** Easier to test specific model groups

### Scalability
- **New Features:** Easy to add new XML formats to appropriate namespace
- **Team Development:** Multiple developers can work on different namespaces
- **Documentation:** Clear structure for API documentation

## 📋 Naming Conventions

### File Names
- Singular nouns for single entities (e.g., `MpItem.cs`)
- Plural nouns for collections (e.g., `MpItems.cs`)
- Descriptive names matching XML root elements

### Class Names
- PascalCase following C# conventions
- Clear, descriptive names
- Avoid abbreviations except for well-known terms (Mp, Xml, etc.)

### Namespace Names
- Meaningful business domains
- Avoid technical implementation details
- Keep hierarchy shallow (max 3 levels)

## 🎯 Implementation Priority

1. **High Priority:** Audio, Data, Multiplayer (simple structures)
2. **Medium Priority:** Items, Configuration, Characters (moderate complexity)
3. **Low Priority:** World, Animation, Visual (large, complex files)

This structure provides a solid foundation for the continued development of the Bannerlord XML adaptation project. 