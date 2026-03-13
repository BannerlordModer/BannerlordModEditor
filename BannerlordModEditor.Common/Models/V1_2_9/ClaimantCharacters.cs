using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("NPCCharacters")]
public class ClaimantCharacters
{
    [XmlElement("NPCCharacter")]
    public List<ClaimantCharacter> CharacterList { get; set; } = new List<ClaimantCharacter>();
}

public class ClaimantCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlAttribute("voice")]
    public string? Voice { get; set; }

    [XmlAttribute("default_group")]
    public string? DefaultGroup { get; set; }

    [XmlAttribute("is_hero")]
    public string? IsHero { get; set; }

    [XmlAttribute("is_female")]
    public string? IsFemale { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlAttribute("face_mesh_cache")]
    public string? FaceMeshCache { get; set; }

    [XmlElement("face")]
    public Face? Face { get; set; }

    [XmlElement("Traits")]
    public Traits? Traits { get; set; }

    [XmlElement("Equipments")]
    public Equipments? Equipments { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeVoice() => !string.IsNullOrEmpty(Voice);
    public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
    public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
    public bool ShouldSerializeIsFemale() => !string.IsNullOrEmpty(IsFemale);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeFaceMeshCache() => !string.IsNullOrEmpty(FaceMeshCache);
    public bool ShouldSerializeFace() => Face != null && Face.HasContent;
    public bool ShouldSerializeTraits() => Traits != null && Traits.HasContent;
    public bool ShouldSerializeEquipments() => Equipments != null && Equipments.HasContent;
}

public class Face
{
    [XmlElement("BodyProperties")]
    public BodyProperties? BodyProperties { get; set; }

    public bool HasContent => BodyProperties != null && BodyProperties.HasContent;
}

public class BodyProperties
{
    [XmlAttribute("version")]
    public string? Version { get; set; }

    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlAttribute("weight")]
    public string? Weight { get; set; }

    [XmlAttribute("build")]
    public string? Build { get; set; }

    [XmlAttribute("key")]
    public string? Key { get; set; }

    public bool HasContent => !string.IsNullOrEmpty(Key);
    public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
    public bool ShouldSerializeBuild() => !string.IsNullOrEmpty(Build);
    public bool ShouldSerializeKey() => !string.IsNullOrEmpty(Key);
}

public class Traits
{
    [XmlElement("Trait")]
    public List<Trait> TraitList { get; set; } = new List<Trait>();

    public bool HasContent => TraitList.Count > 0;
}

public class Trait
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class Equipments
{
    [XmlElement("EquipmentSet")]
    public List<ClaimantEquipmentSet> EquipmentSetList { get; set; } = new List<ClaimantEquipmentSet>();

    public bool HasContent => EquipmentSetList.Count > 0;
}

public class ClaimantEquipmentSet
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("civilian")]
    public string? Civilian { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
}
