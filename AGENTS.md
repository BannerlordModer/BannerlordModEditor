# PROJECT KNOWLEDGE BASE

**Generated:** 2026-03-17
**Commit:** 1b3bac0
**Branch:** compat/1.2.9-legacy-support

## OVERVIEW
Bannerlord Mod Editor - .NET 9 desktop app for editing Mount & Blade II (骑砍2) XML configs. Supports multi-version game adaptation (v1.2.9, v1.3.15+). Avalonia UI (cross-platform), MVVM, DO/DTO for XML serialization.

## CRITICAL: Multi-Version Adaptation
- **Game Versions**: This is a Mod editor for Mount & Blade II - MUST support different game versions (v1.2.9 legacy, v1.3.15 current)
- **XML Structure Changes**: Different game versions have different XML structures - models must account for version-specific fields
- **Version Detection**: Use `V1_2_9` and `V1_3_15` namespaces/folders to distinguish version-specific models
- **Test Data**: Real game XML files in `example/` and `TestData/` organized by version

## CRITICAL: XML Deserialization Reliability
- **Unreliable Deserialization**: Bannerlord's XML format has reliability issues - avoid relying on System.Xml deserialization directly
- **ShouldSerialize Pattern**: Use `ShouldSerialize*()` methods to explicitly control optional XML element emission
- **Roundtrip Required**: All XML must preserve exact structure - no data loss/gain during serialize/deserialize
- **Avoid Implicit Behavior**: Never rely on XML serializer's default behavior for optional elements - always be explicit

## STRUCTURE
```
BannerlordModEditor.sln (11 projects)
├── BannerlordModEditor.Common/      # Core: XML models, loaders, mappers, services
├── BannerlordModEditor.UI/           # Avalonia UI (MVVM)
├── BannerlordModEditor.Cli/         # CLI commands (CliFx)
├── BannerlordModEditor.TUI/         # Terminal UI (Terminal.Gui)
├── BannerlordModEditor.Common.Tests/ # Unit tests (xUnit)
├── BannerlordModEditor.UI.Tests/    # UI tests (Avalonia.Headless)
├── BannerlordModEditor.Cli.Tests/   # CLI tests
├── BannerlordModEditor.TUI.Tests/   # TUI tests
└── docs/                            # Documentation
```

## WHERE TO LOOK
| Task | Location | Notes |
|------|----------|-------|
| XML Models | `Common/Models/` | Data models by domain (Audio, Game, Engine) |
| Version-Specific | `Common/Models/V1_2_9/` & `/V1_3_15/` | Legacy + current game versions |
| DO/DTO | `Common/Models/DO/` & `/DTO/` | Domain objects + mappers |
| Loaders | `Common/Loaders/` | GenericXmlLoader<T> |
| UI ViewModels | `UI/ViewModels/` | MVVM ViewModels |
| Editors | `UI/ViewModels/Editors/` | XML type editors |
| CLI Commands | `Cli/Commands/` | Convert, Recognize |
| Test Data | `TestData/V1_2_9/` & `/V1_3_15/` | XML test files by version |

## CODE MAP
| Symbol | Type | Location | Role |
|--------|------|----------|------|
| GenericXmlLoader<T> | Class | Common/Loaders | XML serialization |
| IFileDiscoveryService | Interface | Common/Services | File discovery |
| EditorManagerViewModel | Class | UI/ViewModels | Main editor logic |
| ServiceCollectionExtensions | Class | UI/App.axaml.cs | DI config |

## CONVENTIONS
- **DO/DTO**: Models/DO/ (domain), Models/DTO/ (serialization), Mappers/ (conversion)
- **Namespace by domain**: Common.Models.{Audio,Game,Engine,Configuration}
- **MVVM**: CommunityToolkit.Mvvm with [ObservableProperty], [RelayCommand]
- **Nullable**: Enabled C# 9
- **Testing**: xUnit, test data in TestData/ subdirs

## ANTI-PATTERNS (THIS PROJECT)
- ❌ NEVER use `as any`, suppress type errors
- ❌ NEVER leave empty catch blocks
- ❌ NEVER delete failing tests to "pass"
- ❌ NEVER commit without build + tests passing
- ❌ NEVER rely on implicit XML serialization for optional elements - use ShouldSerialize*()
- ❌ NEVER ignore version-specific XML field differences

## COMMANDS
```bash
dotnet build
dotnet test
dotnet run --project BannerlordModEditor.UI
```

## NOTES
- **XML Roundtrip**: Must preserve exact structure - no data loss/gain
- **Large XML**: Test data includes large files, may need chunked testing
- **Editor VMs**: Each XML type has dedicated EditorViewModel
- **Version-Specific Models**: V1_2_9 and V1_3_15 namespaces contain version-specific XML structures
- **Multi-Version Testing**: Always test with both legacy (v1.2.9) and current (v1.3.15) XML files
