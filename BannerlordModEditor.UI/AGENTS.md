# BannerlordModEditor.UI

## OVERVIEW

Avalonia UI cross-platform desktop application for editing Mount & Blade II: Bannerlord XML configuration files. MVVM architecture using CommunityToolkit.Mvvm.

## STRUCTURE

```
BannerlordModEditor.UI/
├── ViewModels/           # MVVM ViewModels
│   └── Editors/          # 10+ XML type editors (Item, CraftingPiece, CombatParameter, etc.)
├── Views/                # Avalonia XAML views (Editors/ subfolder mirrors ViewModels)
├── Factories/            # Factory pattern (EditorManagerFactory, UnifiedEditorFactory)
├── Services/             # DI services (DataBinding, Validation, Log, ErrorHandler)
├── Controls/             # Reusable Avalonia controls
└── Extensions/           # ServiceCollectionExtensions for DI registration
```

## KEY CLASSES

| Class | Location | Role |
|-------|----------|------|
| EditorManagerViewModel | ViewModels/ | Main editor logic, file management |
| GenericEditorViewModel | ViewModels/Editors/ | Generic XML editor for any type |
| BaseEditorViewModel<TData,TItem> | ViewModels/Editors/ | Abstract base for specialized editors |
| EditorManagerFactory | Factories/ | Creates EditorManagerViewModel instances |
| UnifiedEditorFactory | Factories/ | Resolves editor types dynamically |
| ServiceCollectionExtensions | Extensions/ | DI configuration for all services |

## CONVENTIONS

- **ViewModels**: Use `[ObservableProperty]` and `[RelayCommand]` from CommunityToolkit.Mvvm
- **Editors**: Specialized editors extend `BaseEditorViewModel<TData,TItem>` with `[EditorType]` attribute
- **DI**: Services registered via `ServiceCollectionExtensions.AddEditorManagerServices()`
- **Views**: XAML files in Views/Editors/ mirror ViewModel names (e.g., ItemEditorViewModel -> ItemEditorView.axaml)
- **Null Services**: Fallback implementations (NullEditorFactory, NullDataBindingService) when DI unavailable

## EDITOR TYPES

Specialized editors in ViewModels/Editors/: ItemEditorViewModel, CraftingPieceEditorViewModel, CombatParameterEditorViewModel, SkillEditorViewModel, AttributeEditorViewModel, ItemModifierEditorViewModel, BoneBodyTypeEditorViewModel, SimpleEditorViewModel
