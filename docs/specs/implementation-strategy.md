# Implementation Strategy for XML Model Classes

## Core Implementation Principles

### 1. String-Based Property Pattern
All XML attributes must be implemented as string properties to preserve exact format:

```csharp
// CORRECT - Preserves exact XML format
[XmlAttribute("static_friction")]
public string? StaticFriction { get; set; }

// INCORRECT - May change format during serialization
[XmlAttribute("static_friction")]
public float StaticFriction { get; set; }
```

### 2. Conditional Serialization
Every optional attribute requires a corresponding ShouldSerialize method:

```csharp
public bool ShouldSerializeStaticFriction() => 
    !string.IsNullOrWhiteSpace(StaticFriction);
```

### 3. Required vs Optional Attributes
- **Required**: Non-nullable string properties with empty string defaults
- **Optional**: Nullable string properties with null defaults

## Model Implementation Process

### Phase 1: PhysicsMaterials (High Priority - 15 expected fixes)

#### Current Issues:
1. Boolean properties serialized as "True"/"False" instead of "true"/"false"
2. Missing ShouldSerialize methods for optional attributes
3. Default values being serialized when they shouldn't be

#### Implementation Steps:
1. Convert all boolean properties to string type
2. Convert all numeric properties to string type
3. Add ShouldSerialize methods for all optional attributes
4. Validate with existing test suite

#### Example Transformation:
```csharp
// BEFORE (Problematic)
[XmlAttribute("dont_stick_missiles")]
[DefaultValue(false)]
public bool DontStickMissiles { get; set; }

// AFTER (Correct)
[XmlAttribute("dont_stick_missiles")]
public string? DontStickMissiles { get; set; }

public bool ShouldSerializeDontStickMissiles() => 
    !string.IsNullOrWhiteSpace(DontStickMissiles);
```

### Phase 2: Monsters Model (High Priority - 20 expected fixes)

#### Current Issues:
1. Complex nested structures with inconsistent serialization
2. Boolean flag attributes not following string pattern
3. Missing ShouldSerialize methods throughout hierarchy

#### Implementation Steps:
1. Apply string-based property pattern to Monster class
2. Fix nested classes (MonsterFlags, MonsterCapsules, etc.)
3. Add ShouldSerialize methods for all optional properties
4. Ensure proper XML element/attribute mapping

#### Nested Structure Fixes:
```csharp
public class MonsterCapsules
{
    [XmlElement("body_capsule")]
    public MonsterCapsule? BodyCapsule { get; set; }
    
    [XmlElement("crouched_body_capsule")]
    public MonsterCapsule? CrouchedBodyCapsule { get; set; }
    
    public bool ShouldSerializeBodyCapsule() => BodyCapsule != null;
    public bool ShouldSerializeCrouchedBodyCapsule() => CrouchedBodyCapsule != null;
}
```

### Phase 3: Remaining Models (Medium Priority - 11 expected fixes)

#### Implementation Checklist:
1. **ProjectConfiguration.cs** - Review all string attributes
2. **AttributesDataModel.cs** - Ensure all optional attributes have ShouldSerialize
3. **ActionTypesModel.cs** - Apply consistent string pattern
4. **ModuleXmlModel.cs** - Validate attribute handling
5. **MultiplayerScenes.cs** - Check complex nested structures
6. **GameTypes.cs** - Apply standard patterns

## Quality Assurance Process

### Pre-Implementation Checklist
- [ ] Identify all XML attributes in model
- [ ] Classify attributes as required or optional
- [ ] Note current property types and serialization behavior
- [ ] Document expected XML format from test data

### Implementation Checklist
- [ ] Convert all properties to string type
- [ ] Add ShouldSerialize methods for optional attributes
- [ ] Maintain required property non-null constraints
- [ ] Preserve XML attribute/element mapping
- [ ] Apply XML namespace and root element declarations

### Post-Implementation Validation
- [ ] Run targeted model tests
- [ ] Verify round-trip XML structural equality
- [ ] Check XML format preservation
- [ ] Validate performance with large files

## Model-Specific Guidelines

### PhysicsMaterials Specifics
- All numeric values stored as strings with 3 decimal places
- Boolean attributes: "true" or "false" exactly
- Optional color attributes with RGBA format preservation
- Sound definitions with simple name attributes

### Monsters Specifics
- Complex nested flag structures
- Capsule definitions with position vectors
- Inheritance relationships between monster types
- Gender-specific action sets

### MultiplayerScenes Specifics
- Scene configuration with multiple nested elements
- Game mode definitions with parameters
- Mission-specific settings
- Team and spawn point configurations

## Error Handling Strategy

### Validation Approach
```csharp
public class XmlValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; } = new();
    public List<string> Warnings { get; } = new();
    
    public void AddError(string error) => Errors.Add(error);
    public void AddWarning(string warning) => Warnings.Add(warning);
}
```

### Exception Management
- Graceful handling of malformed XML
- Detailed error messages for debugging
- Recovery options for partial model loading
- Logging for production diagnostics

## Testing Strategy

### Unit Test Validation
```csharp
[Fact]
public void PhysicsMaterial_RoundTripPreservesExactStructure()
{
    // Arrange
    var originalXml = LoadTestFile("physics_materials.xml");
    
    // Act
    var model = Deserialize<PhysicsMaterials>(originalXml);
    var serializedXml = Serialize(model);
    
    // Assert
    AssertXmlStructurallyEqual(originalXml, serializedXml);
}
```

### Integration Testing
- Validate against real Bannerlord XML files
- Test edge cases from existing test suite
- Performance testing with large XML files
- Regression testing for previously fixed models

## Timeline and Milestones

### Week 1: Foundation Models
- **Days 1-2**: PhysicsMaterials complete implementation
- **Days 3-4**: Monsters model implementation
- **Days 5**: Integration testing and validation

### Week 2: Core Coverage
- **Days 6-7**: MultiplayerScenes and GameTypes
- **Days 8-9**: Remaining high-impact models
- **Days 10**: Comprehensive testing

### Week 3: Final Validation
- **Days 11-12**: Low-impact model completion
- **Days 13-14**: Full test suite validation
- **Days 15**: Performance optimization and documentation