# BannerlordModEditor.TUI

## OVERVIEW

Terminal UI application using Terminal.Gui for editing Mount & Blade II XML configs. Cross-platform TUI with ncurses-style interface.

## STRUCTURE

```
BannerlordModEditor.TUI/
├── ViewModels/
│   ├── MainViewModel.cs
│   └── ViewModelBase.cs
├── Views/
│   └── MainWindow.cs
├── Services/
│   ├── FormatConversionService.cs
│   ├── TypedXmlConversionService.cs
│   ├── XmlAdaptationStatusService.cs
│   └── IFormatConversionService.cs
├── Models/
│   └── XmlTypeInfo.cs
├── XmlAdaptationChecker.cs
└── Program.cs                  # Entry point
```

## KEY CLASSES

| Class | Location | Role |
|-------|----------|------|
| MainViewModel | ViewModels/ | Main TUI logic |
| MainWindow | Views/ | Terminal window definition |
| FormatConversionService | Services/ | XML format conversion |
| XmlAdaptationChecker | . | XML adaptation analysis |
| XmlTypeInfo | Models/ | XML type metadata |

## CONVENTIONS

- Uses Terminal.Gui for cross-platform TUI
- MVVM with ViewModelBase
- Services for XML processing
- Cross-platform testing via tmux

## BUILD
```bash
dotnet run --project BannerlordModEditor.TUI --
```
