# BannerlordModEditor.Common

## OVERVIEW

Core library containing 200+ XML data models for Mount & Blade II: Bannerlord game files. Provides serialization, validation, and mapping infrastructure for editing game configuration XML files. All models support roundtrip serialization preserving exact structure.

## STRUCTURE

```
BannerlordModEditor.Common/
├── Loaders/
│   ├── GenericXmlLoader.cs      # Generic XML serialization
│   └── EnhancedXmlLoader.cs    # Enhanced loading with caching
├── Models/
│   ├── DO/                      # Domain Objects (business logic)
│   ├── DTO/                     # Data Transfer Objects (serialization)
│   ├── Audio/                   # Audio configuration
│   ├── Configuration/           # Game settings
│   ├── Engine/                  # Engine parameters
│   ├── Game/                    # Game mechanics
│   ├── Layouts/                 # UI layouts
│   ├── Map/                     # Map elements
│   └── Multiplayer/             # MP settings
├── Mappers/                     # DO ↔ DTO converters
└── Services/
    ├── IFileDiscoveryService.cs
    ├── FileDiscoveryService.cs
    └── LargeXmlFileProcessor.cs
```

## KEY CLASSES

| Class | Location | Role |
|-------|----------|------|
| GenericXmlLoader<T> | Loaders/ | Generic XML serialization/deserialization |
| EnhancedXmlLoader | Loaders/ | Cached XML loading with memory management |
| IFileDiscoveryService | Services/ | Interface for discovering game files |
| FileDiscoveryService | Services/ | Implementation of file discovery |
| XmlModelBase | Models/ | Base class for all XML models |
| IMapper | Mappers/ | Interface for DO/DTO conversion |
| SimpleXmlConversionFramework | Conversion/ | XML to simple type conversion |

## CONVENTIONS

- **DO/DTO Pattern**: Domain Objects in Models/DO/, serialization DTOs in Models/DTO/
- **Namespace by Domain**: Common.Models.{Audio,Configuration,Engine,Game,Layouts,Map,Multiplayer}
- **Roundtrip Required**: All XML serialization must preserve exact structure
- **Nullable**: C# 9 nullable reference types enabled
- **Loaders**: Use GenericXmlLoader<T> for new model types
- **Large Files**: Use LargeXmlFileProcessor for files exceeding 10MB
