using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("NPCCharacters")]
public class NpcCharacters
{
    [XmlElement("NPCCharacter")]
    public List<NpcCharacter> NpcCharacterList { get; set; } = new List<NpcCharacter>();
}

public class NpcCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("occupation")]
    public string Occupation { get; set; } = "Lord";

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

    [XmlElement("face_mesh")]
    public FaceMesh? FaceMeshElement { get; set; }

    [XmlElement("skills")]
    public Skills? Skills { get; set; }

    [XmlElement("equipmentSet")]
    public EquipmentSet? EquipmentSet { get; set; }

    [XmlElement("upgrade_targets")]
    public UpgradeTargets? UpgradeTargets { get; set; }

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
    public bool ShouldSerializeFaceMeshElement() => FaceMeshElement != null && FaceMeshElement.HasContent;
    public bool ShouldSerializeSkills() => Skills != null && Skills.HasContent;
    public bool ShouldSerializeEquipmentSet() => EquipmentSet != null && EquipmentSet.HasContent;
    public bool ShouldSerializeUpgradeTargets() => UpgradeTargets != null && UpgradeTargets.UpgradeTargetList.Count > 0;
}

public class UpgradeTargets
{
    [XmlElement("upgrade_target")]
    public List<UpgradeTarget> UpgradeTargetList { get; set; } = new List<UpgradeTarget>();
}

public class UpgradeTarget
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}

public class FaceMesh
{
    [XmlElement("BodyProperties")]
    public BodyProperties? BodyProperties { get; set; }

    [XmlElement("hair_tags")]
    public HairTags? HairTags { get; set; }

    [XmlElement("beard_tags")]
    public BeardTags? BeardTags { get; set; }

    [XmlElement("tattoo_tags")]
    public TattooTags? TattooTags { get; set; }

    [XmlIgnore]
    public bool HasContent => (BodyProperties != null && BodyProperties.HasContent)
        || (HairTags != null && HairTags.HasContent)
        || (BeardTags != null && BeardTags.HasContent)
        || (TattooTags != null && TattooTags.HasContent);
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

    [XmlIgnore]
    public bool HasContent => !string.IsNullOrEmpty(Version) || !string.IsNullOrEmpty(Age)
        || !string.IsNullOrEmpty(Weight) || !string.IsNullOrEmpty(Build) || !string.IsNullOrEmpty(Key);

    public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
    public bool ShouldSerializeBuild() => !string.IsNullOrEmpty(Build);
    public bool ShouldSerializeKey() => !string.IsNullOrEmpty(Key);
}

public class HairTags
{
    [XmlElement("hair_tag")]
    public List<HairTag> HairTagList { get; set; } = new List<HairTag>();

    [XmlIgnore]
    public bool HasContent => HairTagList.Count > 0;
}

public class HairTag
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
}

public class BeardTags
{
    [XmlElement("beard_tag")]
    public List<BeardTag> BeardTagList { get; set; } = new List<BeardTag>();

    [XmlIgnore]
    public bool HasContent => BeardTagList.Count > 0;
}

public class BeardTag
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
}

public class TattooTags
{
    [XmlElement("tattoo_tag")]
    public List<TattooTag> TattooTagList { get; set; } = new List<TattooTag>();

    [XmlIgnore]
    public bool HasContent => TattooTagList.Count > 0;
}

public class TattooTag
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
}

public class Skills
{
    [XmlElement("skill")]
    public List<Skill> SkillList { get; set; } = new List<Skill>();

    [XmlIgnore]
    public bool HasContent => SkillList.Count > 0;
}

public class Skill
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
}

public class EquipmentSet
{
    [XmlIgnore]
    public bool HasContent => false;
}
