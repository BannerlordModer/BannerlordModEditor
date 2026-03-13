# PROJECT KNOWLEDGE BASE

**Generated:** 2026-03-13
**Commit:** 37bade5
**Branch:** feature/gui-fix

## OVERVIEW
Bannerlord Mod Editor - .NET 9 desktop app for editing Mount & Blade II XML configs. Avalonia UI (cross-platform), MVVM, DO/DTO for XML serialization.

## STRUCTURE
```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/      # Core: XML models, loaders, mappers
├── BannerlordModEditor.UI/           # Avalonia UI (MVVM)
├── BannerlordModEditor.Cli/          # CLI commands
├── BannerlordModEditor.TUI/          # Terminal UI
├── BannerlordModEditor.Common.Tests/ # Unit tests (xUnit)
└── BannerlordModEditor.UI.Tests/    # UI tests
```

## WHERE TO LOOK
| Task | Location | Notes |
|------|----------|-------|
| XML Models | `Common/Models/` | Data models by domain (Audio, Game, Engine) |
| DO/DTO | `Common/Models/DO/` & `/DTO/` | Domain objects + mappers |
| Loaders | `Common/Loaders/` | GenericXmlLoader<T> |
| UI ViewModels | `UI/ViewModels/` | MVVM ViewModels |
| Editors | `UI/ViewModels/Editors/` | XML type editors |
| CLI Commands | `Cli/Commands/` | Convert, Recognize |
| Test Data | `TestData/` dirs | XML test files |

## CODE MAP
| Symbol | Type | Location | Role |
|--------|------|----------|------|
| GenericXmlLoader<T> | Class | Common/Loaders | XML serialization |
| IFileDiscoveryService | Interface | Common/Services | File discovery |
| EditorManagerViewModel | Class | UI/ViewModels | Main editor logic |
| ServiceCollectionExtensions | Class | UI/Extensions | DI config |

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
