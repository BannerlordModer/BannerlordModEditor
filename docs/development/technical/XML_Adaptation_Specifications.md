# Bannerlord Mod Editor - XML Model Adaptation Specifications

## 1. Current State Analysis

### Progress Summary
- Successfully reduced test failures from 273 to 46 failures through namespace serialization fixes
- Main remaining issues are model-specific adaptation problems
- Boolean normalization issues have been addressed
- Focus is now on field/attribute mapping precision

### Major Failure Categories

#### 1.1 Attribute Value Handling Issues
- **Boolean Value Normalization**: XML attributes with boolean values show case sensitivity issues (true/True, false/False)
- **Numeric Value Formatting**: Floating-point numbers may differ in precision or format between serialization rounds
- **Whitespace Handling**: Leading/trailing whitespace in attribute values affecting structural equality

#### 1.2 Missing or Extra Attributes
- **Optional Attribute Omission**: Attributes that should be omitted when empty are serialized with empty values
- **Required Attribute Absence**: Attributes that are required are not properly mapped or missing
- **Computed/Generated Attributes**: Attributes added by the game engine during runtime are being serialized inappropriately

#### 1.3 Nested Element Structure Mismatches
- **Element Ordering**: XML elements require specific ordering that isn't preserved in C# object serialization
- **Empty Element Serialization**: Empty elements that should be self-closing or omitted entirely
- **Collection Handling**: Repeated elements from collections not matching source XML structure

#### 1.4 Field Presence/Absence Rules
- **Strict Field Existence**: XML elements and attributes must exactly match - present attributes must be present, absent attributes must be absent
- **Null vs Empty vs Missing**: Different handling required for null values, empty strings, and completely missing fields
- **Conditional Field Serialization**: Fields that should only be serialized under certain conditions

## 2. Requirements Documentation

### 2.1 Bannerlord Game XML Format Requirements

#### 2.1.1 File Structure Constraints
- XML files must preserve exact element and attribute names as used by the Bannerlord game engine
- File encoding must be UTF-8 with appropriate XML declaration
- No additional namespaces or schema declarations should be added
- Self-closing tags should be properly formatted and consistent

#### 2.1.2 Element and Attribute Requirements
- **Attribute Names**: Must exactly match game XML files (case-sensitive)
- **Attribute Values**: Must preserve exact string representation from source files
- **Element Nesting**: Must preserve parent-child relationships exactly as in source files
- **Element Ordering**: Certain XML files require specific element ordering (game engine sensitive)

#### 2.1.3 Value Representation Requirements
- **Boolean Values**: Must preserve exact case and format as seen in original files
- **Numeric Values**: Must preserve exact precision and formatting (no rounding)
- **String Values**: Must preserve all whitespace and special character encoding
- **Empty Values**: Must distinguish between empty strings and missing attributes/elements

### 2.2 Critical Behavioral Requirements

#### 2.2.1 Strict File Format Preservation
```
MUST NOT modify field presence/absence in XML files
MUST preserve all original whitespace and formatting where possible
MUST maintain exact element and attribute ordering where specified
MUST preserve original boolean/numeric formatting and case
```

#### 2.2.2 Round-trip Serialization Consistency
```
Deserialize(Serialize(original_xml)) == original_xml (structurally equivalent)
No additional elements or attributes should be added
No existing elements or attributes should be removed
All values must be preserved exactly as originally parsed
```

#### 2.2.3 Field Handling Precision
```
Null values in C# objects must NOT create XML attributes/elements
Empty string values in C# objects must NOT create XML attributes/elements
Missing C# properties must NOT create XML attributes/elements
Only populated fields with non-null, non-empty values should be serialized
```

## 3. Technical Specifications

### 3.1 XML-C# Model Mapping Rules

#### 3.1.1 Attribute Mapping
```xml
<!-- XML Source -->
<action name="act_jump" type="actt_jump_start" usage_direction="ud_defend_up" />
```

```csharp
// C# Model
public class ActionType
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    
    [XmlAttribute("type")]
    public string? Type { get; set; }
    
    [XmlAttribute("usage_direction")]
    public string? UsageDirection { get; set; }
    
    // ShouldSerialize methods control attribute serialization
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeUsageDirection() => !string.IsNullOrEmpty(UsageDirection);
}
```

#### 3.1.2 Element Mapping
```xml
<!-- XML Source -->
<Monster id="human" weight="80">
    <Capsules>
        <body_capsule radius="0.4" pos1="0, 1.2, 1.5" pos2="0, -0.37, 1.5" />
    </Capsules>
    <Flags CanAttack="true" CanDefend="true" />
</Monster>
```

```csharp
// C# Model
public class Monster
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
    
    [XmlAttribute("weight")]
    public string? Weight { get; set; }
    
    [XmlElement("Capsules")]
    public MonsterCapsules? Capsules { get; set; }
    
    [XmlElement("Flags")]
    public MonsterFlags? Flags { get; set; }
    
    public bool ShouldSerializeWeight() => !string.IsNullOrWhiteSpace(Weight);
    public bool ShouldSerializeCapsules() => Capsules != null;
    public bool ShouldSerializeFlags() => Flags != null;
}
```

### 3.2 Field Handling Specifications

#### 3.2.1 Null Value Handling
- **C# Property Value**: `null`
- **XML Serialization Result**: Attribute/Element is NOT created
- **ShouldSerialize Method**: Must return `false`

#### 3.2.2 Empty String Handling
- **C# Property Value**: `""` (empty string)
- **XML Serialization Result**: Attribute/Element is NOT created
- **ShouldSerialize Method**: Must return `false`

#### 3.2.3 Whitespace-only String Handling
- **C# Property Value**: `"   "` (whitespace-only)
- **XML Serialization Result**: Attribute/Element is NOT created
- **ShouldSerialize Method**: Must return `false` (using `string.IsNullOrWhiteSpace()`)

#### 3.2.4 Valid Value Handling
- **C# Property Value**: `"actual_value"`
- **XML Serialization Result**: Attribute/Element IS created with exact value
- **ShouldSerialize Method**: Must return `true`

### 3.3 Special Value Type Handling

#### 3.3.1 Boolean Values
```
XML Input:  "true"  → C# Property: "true"  → XML Output: "true"
XML Input:  "True"  → C# Property: "True"  → XML Output: "True"
XML Input:  "false" → C# Property: "false" → XML Output: "false"
XML Input:  "False" → C# Property: "False" → XML Output: "False"
```

#### 3.3.2 Numeric Values
```
XML Input:  "1.0"    → C# Property: "1.0"    → XML Output: "1.0"
XML Input:  "1.0000" → C# Property: "1.0000" → XML Output: "1.0000"
XML Input:  "80"     → C# Property: "80"     → XML Output: "80"
```

#### 3.3.3 Coordinate/Tuple Values
```
XML Input:  "0.13, 0.1, 0.0" → C# Property: "0.13, 0.1, 0.0" → XML Output: "0.13, 0.1, 0.0"
XML Input:  "0, 1.2, 1.5"    → C# Property: "0, 1.2, 1.5"    → XML Output: "0, 1.2, 1.5"
```

## 4. Acceptance Criteria

### 4.1 Round-trip Testing Requirements

#### 4.1.1 Structural Equality
```csharp
// Test Pattern
string originalXml = File.ReadAllText(testFile);
var model = XmlTestUtils.Deserialize<ModelType>(originalXml);
string serializedXml = XmlTestUtils.Serialize(model);
Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
```

#### 4.1.2 Node and Attribute Count Matching
```csharp
var (nodeCountA, attrCountA) = XmlTestUtils.CountNodesAndAttributes(originalXml);
var (nodeCountB, attrCountB) = XmlTestUtils.CountNodesAndAttributes(serializedXml);
Assert.Equal(nodeCountA, nodeCountB);
Assert.Equal(attrCountA, attrCountB);
```

#### 4.1.3 Value Preservation
- All attribute values must be identical (byte-for-byte matching)
- All element content must be preserved
- Whitespace within values must be maintained exactly
- Case sensitivity must be preserved for all values

### 4.2 Model Compliance Requirements

#### 4.2.1 Attribute Serialization Control
- Every optional attribute must have corresponding `ShouldSerialize{PropertyName}()` method
- `ShouldSerialize` methods must return `false` for null/empty/whitespace-only values
- Required attributes should NOT have `ShouldSerialize` methods

#### 4.2.2 Element Serialization Control
- Every optional complex element must have corresponding `ShouldSerialize{PropertyName}()` method
- Collections should be handled with appropriate null/empty checking logic
- Nested elements must preserve their structure through proper `XmlElement` attributes

#### 4.2.3 Type Safety and Validation
- All XML attributes should map to appropriate C# property types
- String properties should be used for values that require exact preservation
- Validation attributes should be applied where applicable
- Documentation should be provided for complex mappings

## 5. XML Adaptation Rules

### 5.1 Field Presence Rules

#### Rule 1: Required Fields
```
REQUIRED: Fields that must always be present in both XML and C# model
- XML: Attribute/element must exist with a non-empty value
- C# Model: Property must be non-nullable and assigned a default value
- ShouldSerialize: Not needed (always serialized)
```

#### Rule 2: Optional Fields
```
OPTIONAL: Fields that may or may not be present in XML
- XML: Attribute/element may be absent or present with value
- C# Model: Property should be nullable or have appropriate default
- ShouldSerialize: Required to control serialization behavior
```

#### Rule 3: Conditional Fields
```
CONDITIONAL: Fields that are present only under certain conditions
- XML: Attribute/element presence depends on other field values
- C# Model: Complex logic in ShouldSerialize method
- ShouldSerialize: Complex conditions checking related properties
```

### 5.2 Value Handling Rules

#### Rule 1: Exact Value Preservation
```
MUST preserve exact string representation from XML source
- NO conversion to different data types during round-trip
- NO formatting or normalization of values
- NO trimming or modification of whitespace
```

#### Rule 2: Boolean Value Handling
```
Preserve exact boolean string representation from source
- "true" stays "true", not converted to "True" or boolean true
- "false" stays "false", not converted to "False" or boolean false
- Case-sensitive matching to original file format
```

#### Rule 3: Numeric Value Handling
```
Preserve exact numeric string representation from source
- "1.0" stays "1.0", not converted to "1" or 1.0
- "1.0000" stays "1.0000", not rounded or simplified
- Scientific notation must be preserved exactly
```

### 5.3 Serialization Control Pattern

#### Pattern 1: Simple Optional String Attribute
```csharp
[XmlAttribute("description")]
public string? Description { get; set; }

public bool ShouldSerializeDescription() => !string.IsNullOrWhiteSpace(Description);
```

#### Pattern 2: Optional Complex Element
```csharp
[XmlElement("Documentation")]
public DocumentationElement? Documentation { get; set; }

public bool ShouldSerializeDocumentation() => Documentation != null;
```

#### Pattern 3: Optional Collection Element
```csharp
[XmlArray("Items")]
[XmlArrayItem("Item")]
public List<Item>? Items { get; set; }

public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
```

#### Pattern 4: Required Field (No ShouldSerialize)
```csharp
[XmlAttribute("id")]
public string Id { get; set; } = string.Empty;
// No ShouldSerialize method - always required
```

## 6. Implementation Guidelines

### 6.1 Model Design Principles

#### 6.1.1 Use String Properties for Exact Preservation
```csharp
// CORRECT - preserves exact format
[XmlAttribute("weight")]
public string? Weight { get; set; }

// INCORRECT - may cause format changes
[XmlAttribute("weight")]
public float Weight { get; set; }
```

#### 6.1.2 Implement Proper ShouldSerialize Methods
```csharp
// For optional string attributes
public bool ShouldSerializePropertyName() => !string.IsNullOrWhiteSpace(PropertyName);

// For optional complex objects
public bool ShouldSerializeObjectName() => ObjectName != null;

// For optional collections
public bool ShouldSerializeCollectionName() => CollectionName != null && CollectionName.Count > 0;
```

#### 6.1.3 Use Appropriate XML Attributes
```csharp
// For single elements
[XmlElement("Capsules")]

// For collections (arrays)
[XmlArray("Items")]
[XmlArrayItem("Item")]

// For attributes
[XmlAttribute("id")]

// For root elements
[XmlRoot("Monsters")]
```

### 6.2 Testing Requirements

#### 6.2.1 Comprehensive Test Coverage
- Every XML file must have corresponding round-trip test
- Large XML files should have subset testing capability
- Edge cases like empty elements, self-closing tags must be tested
- Files with different formatting styles must be supported

#### 6.2.2 Validation Testing
- Structural equality must be verified for all test cases
- Node and attribute counts must match before and after serialization
- Value preservation must be exact (byte-for-byte when appropriate)
- Error handling for malformed XML should be considered

This specification document provides the comprehensive framework for addressing the remaining 46 XML adaptation failures while maintaining strict compatibility with Bannerlord's XML format requirements.