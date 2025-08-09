# Bannerlord Mod Editor - Technical Analysis and Implementation Guide

## 1. Critical Issue Analysis

### 1.1 Physics Materials XML Structure Analysis

From examining `physics_materials.xml` and the corresponding model `PhysicsMaterials.cs`, we've identified several critical issues:

#### Issue 1: Boolean Attribute Mismatch
**Problem**: The XML contains boolean values as strings, but the C# model uses boolean types
```xml
<!-- XML Structure -->
<physics_material id="pot" dont_stick_missiles="true" attacks_can_pass_through="false" />
<physics_material id="wood" rain_splashes_enabled="true" flammable="true" />
```

```csharp
// Current Model Problem
public class PhysicsMaterial
{
    [XmlAttribute("dont_stick_missiles")]
    [DefaultValue(false)]
    public bool DontStickMissiles { get; set; }  // Serializes as: dont_stick_missiles="true"
    
    [XmlAttribute("attacks_can_pass_through")]
    public string? AttacksCanPassThrough { get; set; }  // Correct: preserves string format
}
```

**Root Cause**: When `bool` properties are serialized, the output is correct, but the comparison logic may fail due to type conversion differences.

#### Issue 2: Missing ShouldSerialize Methods
**Problem**: Optional string attributes lack proper serialization control
```csharp
// Current Model
[XmlAttribute("override_material_name_for_impact_sounds")]
public string? OverrideMaterialNameForImpactSounds { get; set; }  // No ShouldSerialize method

// Should be:
[XmlAttribute("override_material_name_for_impact_sounds")]
public string? OverrideMaterialNameForImpactSounds { get; set; }

public bool ShouldSerializeOverrideMaterialNameForImpactSounds() 
    => !string.IsNullOrWhiteSpace(OverrideMaterialNameForImpactSounds);
```

#### Issue 3: Numeric Precision and Formatting
**Problem**: Float values may lose precision or format during round-trip
```xml
<!-- Original XML -->
<physics_material static_friction="0.800" dynamic_friction="0.400" />

<!-- After serialization, might become: -->
<physics_material static_friction="0.8" dynamic_friction="0.4" />
```

### 1.2 Monsters XML Structure Analysis

#### Issue 1: Complex Nested Structure with Optional Elements
```xml
<!-- XML Structure -->
<Monster id="human" weight="80">
    <Capsules>
        <body_capsule radius="0.4" pos1="0, 1.2, 1.5" pos2="0, -0.37, 1.5" />
    </Capsules>
    <Flags Mountable="true" CanRear="true" />
</Monster>
```

```csharp
// Current Model - Missing proper serialization control
public class Monster
{
    [XmlElement("Capsules")]
    public MonsterCapsules? Capsules { get; set; }  // No ShouldSerialize method
    
    [XmlElement("Flags")]
    public MonsterFlags? Flags { get; set; }  // No ShouldSerialize method
}
```

#### Issue 2: Boolean Attributes in Nested Elements
```xml
<!-- Nested boolean attributes -->
<Flags Mountable="true" CanRear="true" RunsAwayWhenHit="true" />
```

```csharp
// Current Model - Inconsistent boolean handling
public class MonsterFlags
{
    [XmlAttribute("Mountable")]
    public string? Mountable { get; set; }  // Correct: string preservation
    
    [XmlAttribute("CanAttack")]
    public string? CanAttack { get; set; }  // Correct: string preservation
}
```

## 2. Technical Implementation Strategy

### 2.1 Phase 1: Fix Boolean Attribute Handling

#### Strategy 1: Convert All Boolean Attributes to Strings
```csharp
// BEFORE (problematic)
[XmlAttribute("dont_stick_missiles")]
[DefaultValue(false)]
public bool DontStickMissiles { get; set; }

// AFTER (correct)
[XmlAttribute("dont_stick_missiles")]
public string? DontStickMissiles { get; set; }  // Preserves exact string format

public bool ShouldSerializeDontStickMissiles() 
    => !string.IsNullOrWhiteSpace(DontStickMissiles);
```

#### Strategy 2: Implement Proper ShouldSerialize Methods
```csharp
public class PhysicsMaterial
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;  // Required - no ShouldSerialize
    
    [XmlAttribute("override_material_name_for_impact_sounds")]
    public string? OverrideMaterialNameForImpactSounds { get; set; }
    
    [XmlAttribute("dont_stick_missiles")]
    public string? DontStickMissiles { get; set; }
    
    [XmlAttribute("attacks_can_pass_through")]
    public string? AttacksCanPassThrough { get; set; }
    
    [XmlAttribute("rain_splashes_enabled")]
    public string? RainSplashesEnabled { get; set; }
    
    [XmlAttribute("flammable")]
    public string? Flammable { get; set; }
    
    [XmlAttribute("display_color")]
    public string? DisplayColor { get; set; }
    
    // ShouldSerialize methods for optional attributes
    public bool ShouldSerializeOverrideMaterialNameForImpactSounds() 
        => !string.IsNullOrWhiteSpace(OverrideMaterialNameForImpactSounds);
    
    public bool ShouldSerializeDontStickMissiles() 
        => !string.IsNullOrWhiteSpace(DontStickMissiles);
    
    public bool ShouldSerializeAttacksCanPassThrough() 
        => !string.IsNullOrWhiteSpace(AttacksCanPassThrough);
    
    public bool ShouldSerializeRainSplashesEnabled() 
        => !string.IsNullOrWhiteSpace(RainSplashesEnabled);
    
    public bool ShouldSerializeFlammable() 
        => !string.IsNullOrWhiteSpace(Flammable);
    
    public bool ShouldSerializeDisplayColor() 
        => !string.IsNullOrWhiteSpace(DisplayColor);
}
```

### 2.2 Phase 2: Fix Complex Element Structure Handling

#### Strategy 1: Implement Proper ShouldSerialize for Complex Elements
```csharp
public class Monster
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;  // Required
    
    [XmlAttribute("weight")]
    public string? Weight { get; set; }
    
    [XmlElement("Capsules")]
    public MonsterCapsules? Capsules { get; set; }
    
    [XmlElement("Flags")]
    public MonsterFlags? Flags { get; set; }
    
    public bool ShouldSerializeWeight() 
        => !string.IsNullOrWhiteSpace(Weight);
    
    public bool ShouldSerializeCapsules() 
        => Capsules != null;
    
    public bool ShouldSerializeFlags() 
        => Flags != null;
}
```

#### Strategy 2: Handle Nested Boolean Attributes Correctly
```csharp
public class MonsterFlags
{
    [XmlAttribute("CanAttack")]
    public string? CanAttack { get; set; }
    
    [XmlAttribute("CanDefend")]
    public string? CanDefend { get; set; }
    
    [XmlAttribute("CanKick")]
    public string? CanKick { get; set; }
    
    [XmlAttribute("CanBeCharged")]
    public string? CanBeCharged { get; set; }
    
    [XmlAttribute("CanCharge")]
    public string? CanCharge { get; set; }
    
    [XmlAttribute("CanClimbLadders")]
    public string? CanClimbLadders { get; set; }
    
    [XmlAttribute("CanSprint")]
    public string? CanSprint { get; set; }
    
    [XmlAttribute("CanCrouch")]
    public string? CanCrouch { get; set; }
    
    [XmlAttribute("CanRetreat")]
    public string? CanRetreat { get; set; }
    
    [XmlAttribute("CanRear")]
    public string? CanRear { get; set; }
    
    [XmlAttribute("CanWander")]
    public string? CanWander { get; set; }
    
    [XmlAttribute("CanBeInGroup")]
    public string? CanBeInGroup { get; set; }
    
    [XmlAttribute("MoveAsHerd")]
    public string? MoveAsHerd { get; set; }
    
    [XmlAttribute("MoveForwardOnly")]
    public string? MoveForwardOnly { get; set; }
    
    [XmlAttribute("IsHumanoid")]
    public string? IsHumanoid { get; set; }
    
    [XmlAttribute("Mountable")]
    public string? Mountable { get; set; }
    
    [XmlAttribute("CanRide")]
    public string? CanRide { get; set; }
    
    [XmlAttribute("CanWieldWeapon")]
    public string? CanWieldWeapon { get; set; }
    
    [XmlAttribute("RunsAwayWhenHit")]
    public string? RunsAwayWhenHit { get; set; }
    
    [XmlAttribute("CanGetScared")]
    public string? CanGetScared { get; set; }
    
    // ShouldSerialize methods for all optional attributes
    public bool ShouldSerializeCanAttack() 
        => !string.IsNullOrWhiteSpace(CanAttack);
    
    public bool ShouldSerializeCanDefend() 
        => !string.IsNullOrWhiteSpace(CanDefend);
    
    // ... similar methods for all other attributes
}
```

### 2.3 Phase 3: Numeric Precision and Formatting

#### Strategy 1: Use String Properties for Numeric Values
```csharp
// BEFORE (problematic - may lose precision/formatting)
[XmlAttribute("static_friction")]
public float StaticFriction { get; set; }

// AFTER (correct - preserves exact format)
[XmlAttribute("static_friction")]
public string? StaticFriction { get; set; }

public bool ShouldSerializeStaticFriction() 
    => !string.IsNullOrWhiteSpace(StaticFriction);
```

#### Strategy 2: Implement Validation for Numeric Strings
```csharp
public class PhysicsMaterial
{
    [XmlAttribute("static_friction")]
    public string? StaticFriction { get; set; }
    
    public bool ShouldSerializeStaticFriction() 
        => !string.IsNullOrWhiteSpace(StaticFriction);
    
    public float? GetStaticFrictionValue()
    {
        if (float.TryParse(StaticFriction, out var value))
            return value;
        return null;
    }
    
    public void SetStaticFrictionValue(float? value)
    {
        StaticFriction = value?.ToString("0.000");  // Preserve formatting
    }
}
```

## 3. Implementation Checklist

### 3.1 PhysicsMaterials.cs Fixes Required

#### Boolean Attributes to Convert to Strings
- [ ] `DontStickMissiles` - Convert `bool` to `string?`
- [ ] `RainSplashesEnabled` - Convert `bool` to `string?`
- [ ] `Flammable` - Convert `bool` to `string?`

#### ShouldSerialize Methods to Add
- [ ] `ShouldSerializeOverrideMaterialNameForImpactSounds()`
- [ ] `ShouldSerializeDontStickMissiles()`
- [ ] `ShouldSerializeAttacksCanPassThrough()`
- [ ] `ShouldSerializeRainSplashesEnabled()`
- [ ] `ShouldSerializeFlammable()`
- [ ] `ShouldSerializeDisplayColor()`

#### Numeric Attributes to Convert to Strings
- [ ] `StaticFriction` - Convert `float` to `string?`
- [ ] `DynamicFriction` - Convert `float` to `string?`
- [ ] `Restitution` - Convert `float` to `string?`
- [ ] `Softness` - Convert `float` to `string?`
- [ ] `LinearDamping` - Convert `float` to `string?`
- [ ] `AngularDamping` - Convert `float` to `string?`

### 3.2 Monsters.cs Fixes Required

#### ShouldSerialize Methods to Add
- [ ] `ShouldSerializeWeight()` for Weight attribute
- [ ] `ShouldSerializeCapsules()` for Capsules element
- [ ] `ShouldSerializeFlags()` for Flags element
- [ ] All MonsterFlags ShouldSerialize methods (20+ methods)

#### String Property Conversions
- [ ] Review all numeric attributes for string conversion
- [ ] Review all boolean attributes for consistent string handling

### 3.3 General Model Updates Required

#### Models Needing Review (based on 46 failures)
- [ ] `PhysicsMaterials.cs` - Boolean and numeric attribute handling
- [ ] `Monsters.cs` - Complex nested structure and boolean handling
- [ ] `AttributesDataModel.cs` - Optional attribute serialization
- [ ] `ActionTypesModel.cs` - Optional attribute serialization
- [ ] `ProjectConfiguration.cs` - Optional element handling
- [ ] All other models with failing tests

## 4. Testing and Validation Strategy

### 4.1 Unit Test Updates Required

#### PhysicsMaterials Test Updates
```csharp
[Fact]
public void PhysicsMaterials_RoundTrip_StructuralEquality()
{
    var xml = File.ReadAllText(TestDataPath);
    var model = XmlTestUtils.Deserialize<PhysicsMaterials>(xml);
    var serialized = XmlTestUtils.Serialize(model);
    
    // Test structural equality
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
    
    // Test specific attribute preservation
    var originalMaterial = model.PhysicsMaterialList.FirstOrDefault(m => m.Id == "pot");
    Assert.NotNull(originalMaterial);
    Assert.Equal("true", originalMaterial.DontStickMissiles);
    Assert.Equal("false", originalMaterial.AttacksCanPassThrough);
}
```

#### Monsters Test Updates
```csharp
[Fact]
public void Monsters_RoundTrip_StructuralEquality()
{
    var xml = File.ReadAllText(TestDataPath);
    var model = XmlTestUtils.Deserialize<Monsters>(xml);
    var serialized = XmlTestUtils.Serialize(model);
    
    // Test structural equality
    Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
    
    // Test complex nested structure preservation
    var humanMonster = model.MonsterList.FirstOrDefault(m => m.Id == "human");
    Assert.NotNull(humanMonster);
    Assert.NotNull(humanMonster.Capsules);
    Assert.NotNull(humanMonster.Flags);
    
    // Test boolean attribute preservation in nested elements
    Assert.Equal("true", humanMonster.Flags.CanAttack);
    Assert.Equal("true", humanMonster.Flags.CanDefend);
}
```

### 4.2 Integration Testing Strategy

#### Test All Failing Models
- [ ] Run all 46 failing tests to confirm fixes
- [ ] Verify round-trip serialization preserves exact structure
- [ ] Verify all optional attributes are handled correctly
- [ ] Verify numeric precision and formatting is preserved

#### Performance Testing
- [ ] Test large XML files for performance impact
- [ ] Verify memory usage is acceptable
- [ ] Ensure serialization speed meets requirements

## 5. Rollout Plan

### Phase 1: Core Model Fixes (High Priority)
1. Fix PhysicsMaterials.cs (highest failure rate)
2. Fix Monsters.cs (complex nested structure)
3. Fix AttributesDataModel.cs (simple structure, high visibility)

### Phase 2: Additional Model Fixes (Medium Priority)
1. Fix remaining models with similar patterns
2. Update unit tests to reflect new serialization behavior
3. Add integration tests for edge cases

### Phase 3: Validation and Optimization (Low Priority)
1. Performance testing and optimization
2. Documentation updates
3. Code review and refactoring

This technical analysis provides the detailed implementation strategy needed to address the remaining 46 XML adaptation failures while maintaining strict compliance with Bannerlord's XML format requirements.