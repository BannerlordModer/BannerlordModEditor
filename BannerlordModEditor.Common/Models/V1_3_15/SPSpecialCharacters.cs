using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

// spspecialcharacters.xml - Single Player Special Characters for Bannerlord 1.3.15
[XmlRoot("NPCCharacters")]
public class SpSpecialCharacters
{
    [XmlElement("NPCCharacter")]
    public List<SpCharacter> CharacterList { get; set; } = new List<SpCharacter>();
}

public class SpCharacter
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("occupation")]
    public string Occupation { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("level")]
    public string? Level { get; set; }

    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlAttribute("is_female")]
    public string? IsFemale { get; set; }

    [XmlAttribute("voice")]
    public string? Voice { get; set; }

    [XmlAttribute("is_hero")]
    public string? IsHero { get; set; }

    [XmlAttribute("is_major")]
    public string? IsMajor { get; set; }

    [XmlAttribute("is_minor")]
    public string? IsMinor { get; set; }

    [XmlAttribute("CustomName")]
    public string? CustomName { get; set; }

    [XmlAttribute("face_mesh")]
    public string? FaceMesh { get; set; }

    [XmlAttribute("body_mesh")]
    public string? BodyMesh { get; set; }

    [XmlAttribute("hair_mesh")]
    public string? HairMesh { get; set; }

    [XmlAttribute("beard_mesh")]
    public string? BeardMesh { get; set; }

    [XmlAttribute("DefaultTeam")]
    public string? DefaultTeam { get; set; }

    [XmlAttribute("formation_class")]
    public string? FormationClass { get; set; }

    [XmlAttribute("HeroClass")]
    public string? HeroClass { get; set; }

    [XmlAttribute("HeroMasteryLevel")]
    public string? HeroMasteryLevel { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("faction")]
    public string? Faction { get; set; }

    // ShouldSerialize methods for optional attributes - only serialize if value is not null/empty
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeLevel() => !string.IsNullOrEmpty(Level);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeIsFemale() => !string.IsNullOrEmpty(IsFemale);
    public bool ShouldSerializeVoice() => !string.IsNullOrEmpty(Voice);
    public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
    public bool ShouldSerializeIsMajor() => !string.IsNullOrEmpty(IsMajor);
    public bool ShouldSerializeIsMinor() => !string.IsNullOrEmpty(IsMinor);
    public bool ShouldSerializeCustomName() => !string.IsNullOrEmpty(CustomName);
    public bool ShouldSerializeFaceMesh() => !string.IsNullOrEmpty(FaceMesh);
    public bool ShouldSerializeBodyMesh() => !string.IsNullOrEmpty(BodyMesh);
    public bool ShouldSerializeHairMesh() => !string.IsNullOrEmpty(HairMesh);
    public bool ShouldSerializeBeardMesh() => !string.IsNullOrEmpty(BeardMesh);
    public bool ShouldSerializeDefaultTeam() => !string.IsNullOrEmpty(DefaultTeam);
    public bool ShouldSerializeFormationClass() => !string.IsNullOrEmpty(FormationClass);
    public bool ShouldSerializeHeroClass() => !string.IsNullOrEmpty(HeroClass);
    public bool ShouldSerializeHeroMasteryLevel() => !string.IsNullOrEmpty(HeroMasteryLevel);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeFaction() => !string.IsNullOrEmpty(Faction);
}
