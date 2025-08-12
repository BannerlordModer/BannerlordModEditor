# 剩余单元测试失败修复技术栈

## 概述
本文档概述了用于修复Bannerlord Mod Editor剩余单元测试失败的技术栈决策，重点关注DO/DTO分层架构的进一步完善和扩展应用。

## Core Technologies

### .NET 9.0
| Aspect | Choice | Rationale |
|--------|--------|-----------|
| Runtime | .NET 9.0 | Latest LTS version with performance improvements, nullable reference types, and modern C# features |
| Language Features | C# 9.0+ | Pattern matching, nullable reference types, and records for better code safety and expressiveness |

### XML Processing
| Aspect | Choice | Rationale |
|--------|--------|-----------|
| Serialization | System.Xml.Serialization | Native .NET XML serialization with attribute-driven control, well-documented and performant |
| Extensions | Custom XmlSerializer extensions | Handle namespace preservation and boolean value normalization |
| Validation | Custom validation logic | Ensure exact XML preservation and structural consistency |

### UI Framework
| Aspect | Choice | Rationale |
|--------|--------|-----------|
| Framework | Avalonia UI 11.3 | Cross-platform desktop UI framework with good .NET integration |
| MVVM | CommunityToolkit.Mvvm 8.2 | Provides robust MVVM implementation with source generators |
| Theme | Fluent theme | Modern UI consistent with contemporary design guidelines |

### Testing Framework
| Aspect | Choice | Rationale |
|--------|--------|-----------|
| Framework | xUnit 2.5 | Industry-standard testing framework with good .NET integration |
| Assertions | Custom XML validation helpers | Handle structural equality and format preservation verification |

## Architecture Layers

### DO (Data Object) Layer
| Technology | Choice | Rationale |
|------------|--------|-----------|
| Properties | String-based | Preserve exact XML representations including formatting and case |
| Serialization | System.Xml.Serialization attributes | Standard .NET approach with attribute control |
| Conditional Serialization | ShouldSerialize pattern | Prevent serialization of absent attributes |

### DTO (Data Transfer Object) Layer
| Technology | Choice | Rationale |
|------------|--------|-----------|
| Properties | Strongly-typed | Provide type safety for business logic |
| Validation | Data Annotations | Standard .NET validation approach |
| Boolean Handling | BooleanProperty wrapper | Preserve original string values while providing normalized access |

### Mapping Layer
| Technology | Choice | Rationale |
|------------|--------|-----------|
| Implementation | Custom reflection-based mapping | Flexible and extensible conversion between DO/DTO |
| Performance | Object pooling | Reduce allocation overhead for frequent conversions |

### XML Processing Layer
| Technology | Choice | Rationale |
|------------|--------|-----------|
| Namespace Preservation | XDocument parsing | Extract and preserve namespace declarations from original XML |
| Formatting | XmlWriterSettings | Maintain consistent formatting with original files |
| Encoding | UTF-8 without BOM | Match Bannerlord's XML file encoding |

## Development Tools

### Build System
| Tool | Choice | Rationale |
|------|--------|-----------|
| Build Tool | dotnet CLI | Standard .NET build tooling with good IDE integration |
| Package Management | NuGet | Standard .NET package management |
| CI/CD | GitHub Actions | Integrated with GitHub repository hosting |

### IDE Support
| Tool | Choice | Rationale |
|------|--------|-----------|
| Primary IDEs | Visual Studio 2022, Rider | Full .NET support and debugging capabilities |
| Code Analysis | Built-in analyzers | Ensure code quality and consistency |

## Quality Attributes

### Performance Considerations
1. **Memory Efficiency**: String interning for frequently used values
2. **Object Pooling**: Reuse of mapping objects and XML processors
3. **Async Operations**: Non-blocking file I/O operations
4. **Lazy Loading**: Deferred loading of large XML structures

### Security Measures
1. **XML Security**: DTD processing disabled to prevent XXE attacks
2. **File Validation**: Path validation to prevent directory traversal
3. **Input Sanitization**: Validation of XML content before processing

### Maintainability
1. **Separation of Concerns**: Clear DO/DTO layer separation
2. **Standard Patterns**: Use of established .NET patterns and practices
3. **Comprehensive Testing**: Round-trip validation for all XML models
4. **Documentation**: Inline XML documentation and architectural decision records

## Decision Factors

### Primary Considerations
1. **XML Fidelity**: Exact preservation of original XML format is critical for game compatibility
2. **Boolean Handling**: Case-insensitive parsing with original value preservation
3. **Type Safety**: Strong typing for business logic while maintaining flexibility
4. **Performance**: Efficient processing of large XML files
5. **Maintainability**: Clear architecture that's easy to extend and modify

### Trade-offs
1. **Complexity vs. Fidelity**: Additional mapping layer complexity for exact XML preservation
2. **Performance vs. Safety**: String-based properties for safety with potential performance impact
3. **Standardization vs. Customization**: Custom solutions for specific XML challenges where standard approaches fall short

## Future Considerations

### Potential Enhancements
1. **Performance**: Investigate System.Text.Json for alternative serialization approaches
2. **Scalability**: Consider streaming XML processing for extremely large files
3. **Extensibility**: Plugin architecture for custom XML model adapters
4. **Monitoring**: Enhanced telemetry for XML processing performance and errors

### Migration Path
1. **Incremental Adoption**: Gradually migrate existing models to DO/DTO pattern
2. **Backward Compatibility**: Ensure existing functionality continues to work during transition
3. **Testing Coverage**: Maintain comprehensive test coverage throughout migration