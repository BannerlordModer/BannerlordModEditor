# BannerlordModEditor.UI

## OVERVIEW

Avalonia UI cross-platform desktop application for editing Mount & Blade II XML configs. Uses MVVM pattern with CommunityToolkit.Mvvm for reactive UI updates.

## STRUCTURE

```
BannerlordModEditor.UI/
├── ViewModels/          # MVVM ViewModels
│   ├── Editors/         # XML type editors (Item, CraftingPiece, CombatParameter, etc.)
│   ├── MainWindowViewModel.cs
│   ├── EditorManagerViewModel.cs
│   └── EditorContentConverter.cs
├── Views/               # Avalonia views (.axaml)
│   └── Editors/         # Editor views matching ViewModels
├── Factories/           # Factory pattern for editor creation
├── Services/            # Validation, logging, error handling
├── Controls/            # Reusable UI controls
└── App.axaml.cs         # Application entry + DI setup
```

## KEY_CLASSES

| Class | Location | Role |
|-------|----------|------|
| EditorManagerViewModel | ViewModels/ | Main editor orchestration |
| GenericEditorViewModel | ViewModels/Editors/ | Generic XML editor for any type |
| ItemEditorViewModel | ViewModels/Editors/ | Specialized editor for items |
| CraftingPieceEditorViewModel | ViewModels/Editors/ | Specialized for crafting pieces |
| CombatParameterEditorViewModel | ViewModels/Editors/ | Combat stat editing |
| UnifiedEditorFactory | Factories/ | Creates appropriate editor by type |
| ServiceCollectionExtensions | App.axaml.cs | DI configuration |

## CONVENTIONS

- ViewModels use `[ObservableProperty]` and `[RelayCommand]` from CommunityToolkit.Mvvm
- Editors implement `IBaseEditorViewModel` interface
- Factory uses `EditorTypeAttribute` to map XML types to editors
- Views follow `*EditorView.axaml` naming, code-behind `*EditorView.axaml.cs`
- All editors inherit from `BaseEditorViewModel`
