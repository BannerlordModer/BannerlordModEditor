# BannerlordModEditor.Common

**Generated:** 2026-03-17

## OVERVIEW

Core library containing 200+ XML data models for Mount & Blade II: Bannerlord game files. Provides serialization, validation, and mapping infrastructure for editing game configuration XML files. All models support roundtrip serialization preserving exact structure.

## CRITICAL: Multi-Version Adaptation
- **Version-Specific Models**: Use `V1_2_9` and `V1_3_15` namespaces for version-specific models
- **XML Structure Differences**: v1.2.9 and v1.3.15 have different XML schemas - handle fields that exist in one but not the other
- **Test Data Organization**: Real game XML files in `TestData/V1_2_9/` and `TestData/V1_3_15/`

## CRITICAL: XML Deserialization Reliability
- **Avoid System.Xml Direct Deserialization**: Bannerlord's XML format has reliability issues
- **Must Use ShouldSerialize*() Pattern**: Explicitly control optional XML element emission
- **Never Rely on Implicit Behavior**: Always be explicit about optional elements - don't trust serializer defaults
- **100% Roundtrip**: Serialize → Deserialize must preserve all data exactly

## STRUCTURE

```
BannerlordModEditor.Common/
├── Loaders/
│   ├── GenericXmlLoader.cs      # Generic XML serialization
│   └── EnhancedXmlLoader.cs    # Enhanced loading with caching
├── Models/
│   ├── DO/                      # Domain Objects (business logic)
│   ├── DTO/                     # Data Transfer Objects (serialization)
│   ├── V1_2_9/                  # Legacy version models
│   ├── V1_3_15/                 # Current version models
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
- **Namespace by Domain**: Common.Models.{Audio,Configuration,Engine,Game,Layouts,Map,Multiplayer,V1_2_9,V1_3_15}
- **Roundtrip Required**: All XML serialization must preserve exact structure
- **Nullable**: C# 9 nullable reference types enabled
- **Loaders**: Use GenericXmlLoader<T> for new model types
- **Large Files**: Use LargeXmlFileProcessor for files exceeding 10MB
- **ShouldSerialize*()**: MUST implement for all optional XML elements
- **Version-Specific Fields**: Handle fields that exist in one game version but not another
