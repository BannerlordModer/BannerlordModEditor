# BannerlordModEditor.Cli

## OVERVIEW

Command-line interface application using CliFx for Mount & Blade II XML editing. Supports Excel/XML bidirectional conversion, format recognition, and model listing.

## STRUCTURE

```
BannerlordModEditor.Cli/
├── Commands/
│   ├── ConvertCommand.cs       # Excel ↔ XML conversion
│   ├── RecognizeCommand.cs     # XML format detection
│   └── ListModelsCommand.cs    # List supported models
├── Services/
│   ├── ExcelXmlConverterService.cs
│   ├── EnhancedExcelXmlConverterService.cs
│   ├── ModelTypeConverter.cs
│   └── ErrorMessageService.cs
├── Exceptions/
│   ├── CommandException.cs
│   └── CliCommandException.cs
├── Helpers/
│   └── CliOutputHelper.cs
└── Program.cs                  # Entry point
```

## KEY CLASSES

| Class | Location | Role |
|-------|----------|------|
| ConvertCommand | Commands/ | Main conversion logic (Excel↔XML) |
| RecognizeCommand | Commands/ | XML format auto-detection |
| ListModelsCommand | Commands/ | List 50+ supported models |
| ExcelXmlConverterService | Services/ | Excel↔XML conversion |
| ModelTypeConverter | Services/ | Type mapping |

## CONVENTIONS

- Commands inherit from `CliCommand` (CliFx)
- DI via `ICliFactory` injection
- Error handling via `CommandException`
- FluentAssertions for test assertions

## BUILD
```bash
dotnet run --project BannerlordModEditor.Cli -- [command]
```
