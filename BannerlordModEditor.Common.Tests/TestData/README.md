# Test Data Files

This directory contains test data files used by the unit tests.

## Files:

- **mpitems.xml**: Sample multiplayer items data file used for testing MpItems serialization/deserialization
- **skills.xml**: Sample skills data file used for testing Skills XML handling
- **attributes.xml**: Attribute definitions for character and weapon properties
- **mpcultures.xml**: Multiplayer culture definitions (factions)
- **objects.xml**: Game object definitions including items and characters
- **native_skill_sets.xml**: Native skill set definitions (empty container)
- **mpbodypropertytemplates.xml**: Multiplayer body property templates (empty container)
- **native_equipment_sets.xml**: Native equipment set definitions (empty container)
- **bone_body_types.xml**: Bone body type definitions for hit detection
- **siegeengines.xml**: Siege engine definitions with detailed weapon stats
- **physics_materials.xml**: Physical material properties for collision detection
- **parties.xml**: Game party definitions with optional field elements
- **music.xml**: Music file configurations with hexadecimal flags
- **managed_campaign_parameters.xml**: Managed campaign parameter settings
- **gpu_particle_systems.xml**: GPU particle system configurations with precision floats
- **special_meshes.xml**: Special mesh definitions with type hierarchies
- **soundfiles.xml**: Sound bank file definitions with decompress settings
- **managed_core_parameters.xml**: Core game parameters (combat, physics, damage)
- **module_sounds.xml**: Module sound definitions with categories and sound variations
- **gog_achievement_data.xml**: Achievement definitions with requirements and thresholds

## XML Format Coverage:

The following XML formats from Bannerlord are currently supported:
1. **MpItems** - Complex multiplayer items with weapons, armor, horses
2. **Skills** - Character skill definitions
3. **Attributes** - Character and equipment attributes
4. **MpCultures** - Multiplayer culture/faction data
5. **Objects** - Game object definitions
6. **Simple XML Types** - Various container types (SkillSets, BodyProperties, EquipmentRosters, BoneBodyTypes)
7. **SiegeEngines** - Siege engine definitions with weapon statistics and attributes
8. **PhysicsMaterials** - Physics material properties with optional fields and precise numeric values
9. **Parties** - Game party definitions with optional nested field elements
10. **Music** - Music configurations with hexadecimal flag values
11. **ManagedCampaignParameters** - Campaign parameter configurations
12. **GpuParticleSystems** - GPU particle system configurations with high-precision floating point values
13. **SpecialMeshes** - Special mesh definitions with nested type structures
14. **SoundFiles** - Sound bank file definitions with decompress settings and asset/bank structure
15. **ManagedCoreParameters** - Core game parameters including combat, physics, and damage calculations
16. **ModuleSounds** - Module sound definitions with categories, variations, and pitch control
17. **AchievementData** - Achievement definitions with stat-based requirements and numeric thresholds

## Note:

These files are copies of sample data from the Bannerlord game files, used specifically for testing purposes. They are included in the repository to ensure that unit tests work consistently across different environments without requiring external dependencies.

The original example directory is excluded from git via .gitignore to avoid including large game data files in the repository. 