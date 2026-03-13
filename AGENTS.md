# PROJECT KNOWLEDGE BASE

**Generated:** 2026-03-13
**Commit:** 37bade5 fix: 删除过时的测试报告和项目模板文件
**Branch:** feature/gui-fix

## OVERVIEW
Bannerlord Mod Editor - A .NET 9 desktop application for editing Mount & Blade II: Bannerlord XML configuration files. Uses Avalonia UI (cross-platform), MVVM architecture, DO/DTO pattern for XML serialization.

## STRUCTURE
```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/      # Core: XML models, loaders, mappers
├── BannerlordModEditor.UI/           # Avalonia UI layer (MVVM)
├── BannerlordModEditor.Cli/          # CLI commands
├── BannerlordModEditor.TUI/          # Terminal UI
├── BannerlordModEditor.Common.Tests/ # Unit tests (xUnit)
├── BannerlordModEditor.UI.Tests/     # UI tests
├── BannerlordModEditor.Cli.Tests/    # CLI tests
└── BannerlordModEditor.Cli.IntegrationTests/
```

## WHERE TO LOOK
| Task | Location | Notes |
|------|----------|-------|
| XML Models | `BannerlordModEditor.Common/Models/` | Data models by domain (Audio, Game, Engine, etc.) |
| DO/DTO | `BannerlordModEditor.Common/Models/DO/` & `/DTO/` | Domain objects + mappers for XML roundtrip |
| Loaders | `BannerlordModEditor.Common/Loaders/` | XML serialization (GenericXmlLoader<T>) |
| UI ViewModels | `BannerlordModEditor.UI/ViewModels/` | MVVM ViewModels |
| Editors | `BannerlordModEditor.UI/ViewModels/Editors/` | Specific XML type editors |
| CLI Commands | `BannerlordModEditor.Cli/Commands/` | Convert, Recognize commands |
| Test Data | `TestData/` directories | XML files for testing |

## CODE MAP (Key Symbols)
| Symbol | Type | Location | Role |
|--------|------|----------|------|
| GenericXmlLoader<T> | Class | Common/Loaders | XML serialization |
| IFileDiscoveryService | Interface | Common/Services | File discovery |
| EditorManagerViewModel | Class | UI/ViewModels | Main editor logic |
| ServiceCollectionExtensions | Class | UI/Extensions | DI configuration |

## CONVENTIONS
- **DO/DTO Pattern**: Models/DO/ for domain objects, Models/DTO/ for serialization, Mappers/ for conversion
- **Namespace by domain**: BannerlordModEditor.Common.Models.{Audio,Game,Engine,Configuration...}
- **MVVM**: UI layer uses CommunityToolkit.Mvvm
- **Nullable**: Enabled (C# 9)
- **Testing**: xUnit, test data in TestData/ subdirectories per project
- **EditorConfig**: .editorconfig at root (already exists)

## ANTI-PATTERNS (THIS PROJECT)
- ❌ DO NOT use `as any`, `@ts-ignore`, `@ts-expect-error` in C#
- ❌ NEVER suppress type errors - use proper nullable handling
- ❌ DO NOT leave empty catch blocks
- ❌ DO NOT delete failing tests to "pass"
- ❌ NEVER commit without verification (build + tests pass)

## COMMANDS
```bash
# Build solution
dotnet build

# Run all tests
dotnet test

# Run specific project tests
dotnet test BannerlordModEditor.Common.Tests
dotnet test BannerlordModEditor.UI.Tests

# Run UI app
dotnet run --project BannerlordModEditor.UI
```

## NOTES
- **XML Roundtrip**: Must preserve exact structure - no data loss or addition during deserialize→serialize
- **Large XML Files**: Test data uses large XML files, may need chunked testing
- **Editor VMs**: Each XML type has dedicated EditorViewModel (GenericEditorViewModel for generic, specialized for complex types)
