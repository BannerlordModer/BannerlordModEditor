# AGENTS.md - BannerlordModEditor.Common

## OVERVIEW

Core library containing 200+ C# model classes for Bannerlord XML configuration files. Provides XML serialization, file discovery, and DO/DTO mapping for 50+ XML types.

## STRUCTURE

```
Common/
├── Models/
│   ├── DO/              # Domain Objects (60+ files)
│   ├── DTO/             # Data Transfer Objects (60+ files)
│   ├── Audio/           # SoundFiles, Music, Voices
│   ├── Engine/          # ActionTypes, PhysicsMaterials, PostfxGraphs
│   ├── Game/            # ItemModifiers, CraftingPieces, Parties
│   ├── Layouts/         # Layout-specific models
│   ├── Map/             # MapIcons, TerrainMaterials
│   └── Multiplayer/     # MpItems, MpCultures, TauntUsageSets
├── Loaders/
│   ├── GenericXmlLoader.cs     # Core XML serialization
│   └── EnhancedXmlLoader.cs    # Enhanced loading features
├── Mappers/                     # 70+ mapper classes
│   ├── IMapper.cs              # Generic mapper interface
│   └── *_Mapper.cs             # DO <-> DTO converters
└── Services/
    ├── IFileDiscoveryService.cs
    ├── FileDiscoveryService.cs
    ├── LargeXmlFileProcessor.cs
    └── XmlMemoryManager.cs
```

## KEY CLASSES

| Class | Location | Role |
|-------|----------|------|
| GenericXmlLoader<T> | Loaders/ | XML serialization (sync/async), preserves namespaces |
| EnhancedXmlLoader | Loaders/ | Enhanced XML loading with validation |
| IFileDiscoveryService | Services/ | Interface for discovering Bannerlord XML files |
| FileDiscoveryService | Services/ | Scans directories, identifies unadapted XML files |
| IMapper<TS,TT> | Mappers/ | Generic interface for DO/DTO conversion |
| MapperBase | Mappers/ | Base class with common mapping logic |
| XmlModelBase | Models/ | Base class for all XML models |

## CONVENTIONS

- **DO/DTO Pattern**: Domain Objects in `Models/DO/`, DTOs in `Models/DTO/`, Mappers in `Mappers/{Name}Mapper.cs`
- **Namespace by domain**: `BannerlordModEditor.Common.Models.{Audio,Game,Engine,Configuration}`
- **XML Roundtrip**: Must preserve exact structure (no data loss/gain during deserialize->serialize)
- **Nullable**: Enabled C# 9 nullable reference types
- **Mapper Pattern**: Static methods `ToDO()` and `ToDTO()` for bidirectional conversion
- **Test Data**: XML test files located in `BannerlordModEditor.Common.Tests/TestData/`
