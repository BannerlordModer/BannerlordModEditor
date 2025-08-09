# Technology Stack Decisions

## Core Technology Choices

### .NET Platform
| Technology | Choice | Rationale |
|------------|--------|-----------|
| Runtime | .NET 9.0 | Latest LTS version with performance improvements |
| Language | C# 9.0 | Modern features with nullable reference types |
| XML Processing | System.Xml.Serialization | Built-in, mature, attribute-driven control |
| UI Framework | Avalonia 11.3 | Cross-platform desktop UI with MVVM support |
| Testing Framework | xUnit 2.5 | Industry standard with strong assertion capabilities |

### Development Patterns

#### XML Serialization Strategy
```csharp
// String-based properties for exact format preservation
[XmlAttribute("static_friction")]
public string? StaticFriction { get; set; }

// Conditional serialization for optional attributes
public bool ShouldSerializeStaticFriction() => 
    !string.IsNullOrWhiteSpace(StaticFriction);
```

#### Model Organization
- **Functional namespaces**: Engine, Configuration, Data, Game, Audio
- **Interface-driven design**: IXmlModel<T> for consistent API
- **Factory patterns**: XmlModelFactory for centralized model creation

### Quality Assurance Tools

#### Validation Framework
```csharp
public class XmlValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Differences { get; } = new();
    public List<string> MissingAttributes { get; } = new();
    public List<string> ExtraAttributes { get; } = new();
}
```

#### Testing Strategy
- **Structural equality**: Round-trip XML validation
- **Edge case coverage**: Null, empty, default value scenarios
- **Performance benchmarks**: Large XML file processing
- **Regression prevention**: Dedicated test suites per model

### Dependency Management

#### Core Dependencies
- **CommunityToolkit.Mvvm**: MVVM foundation for UI layer
- **Velopack**: Application packaging and updates
- **Avalonia.Themes.Fluent**: Modern UI theme support
- **coverlet.collector**: Code coverage analysis

#### Build and Deployment
- **dotnet CLI**: Build automation and packaging
- **GitHub Actions**: CI/CD pipeline integration
- **NuGet Package Management**: Dependency resolution

## Decision Rationale

### Why String-Based Properties?
1. **Exact Format Preservation**: Bannerlord's XML parser expects specific string representations
2. **Boolean Value Control**: XML uses "true"/"false" vs C# default "True"/"False"
3. **Numeric Precision**: Floating-point values must match original decimal representation
4. **Absent vs Empty**: Distinguishes between missing attributes and empty values

### Why ShouldSerialize Pattern?
1. **Attribute Presence Control**: Ensures only explicitly set attributes are serialized
2. **Backward Compatibility**: Maintains exact XML structure of original files
3. **Parser Compatibility**: Prevents XML parser failures due to unexpected attributes
4. **Minimal Implementation**: Standard .NET pattern with attribute-driven approach

### Why Incremental Approach?
1. **Risk Mitigation**: Systematic fixes reduce regression probability
2. **Test Validation**: Each model validated before proceeding
3. **Pattern Establishment**: Early models establish reusable patterns
4. **Team Coordination**: Clear implementation roadmap for parallel development

## Future Considerations

### Potential Enhancements
- **Performance Optimization**: Async XML processing for large files
- **Schema Validation**: Optional strict validation against game schemas
- **Migration Tools**: Convert between XML versions and formats
- **Localization Support**: Multi-language XML processing capabilities

### Scalability Planning
- **Model Caching**: Frequently used models in memory
- **Parallel Processing**: Concurrent XML file handling
- **Streaming Support**: Process XML without full file loading
- **Error Recovery**: Graceful handling of malformed XML