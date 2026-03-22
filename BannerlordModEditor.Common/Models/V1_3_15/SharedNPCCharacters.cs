using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

public class NPCCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("default_group")]
    public string? DefaultGroup { get; set; }

    [XmlAttribute("level")]
    public string? Level { get; set; }

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("skill_template")]
    public string? SkillTemplate { get; set; }

    [XmlAttribute("is_hidden_encyclopedia")]
    public string? IsHiddenEncyclopedia { get; set; }

    [XmlAttribute("is_hero")]
    public string? IsHero { get; set; }

    [XmlAttribute("is_mercenary")]
    public string? IsMercenary { get; set; }

    [XmlElement("face")]
    public NPCFace? Face { get; set; }

    [XmlElement("skills")]
    public NpcCharacterSkills? Skills { get; set; }

    [XmlElement("Traits")]
    public NpcTraits? Traits { get; set; }

    [XmlElement("upgrade_targets")]
    public NpcUpgradeTargets? UpgradeTargets { get; set; }

    [XmlElement("Equipments")]
    public NpcEquipments? Equipments { get; set; }

    public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
    public bool ShouldSerializeLevel() => !string.IsNullOrEmpty(Level);
    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeSkillTemplate() => !string.IsNullOrEmpty(SkillTemplate);
    public bool ShouldSerializeIsHiddenEncyclopedia() => !string.IsNullOrEmpty(IsHiddenEncyclopedia);
    public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
    public bool ShouldSerializeIsMercenary() => !string.IsNullOrEmpty(IsMercenary);
    public bool ShouldSerializeFace() => Face != null && Face.HasContent;
    public bool ShouldSerializeSkills() => Skills != null && Skills.HasContent;
    public bool ShouldSerializeTraits() => Traits != null && Traits.HasContent;
    public bool ShouldSerializeUpgradeTargets() => UpgradeTargets != null && UpgradeTargets.HasContent;
    public bool ShouldSerializeEquipments() => Equipments != null && Equipments.HasContent;
}

public class NPCFace
{
    [XmlElement("face_key_template")]
    public FaceKeyTemplate? FaceKeyTemplate { get; set; }

    [XmlElement("BodyProperties")]
    public BodyProperties? BodyProperties { get; set; }

    [XmlElement("BodyPropertiesMax")]
    public BodyPropertiesMax? BodyPropertiesMax { get; set; }

    [XmlElement("hair_tags")]
    public HairTags? HairTags { get; set; }

    [XmlElement("beard_tags")]
    public BeardTags? BeardTags { get; set; }

    [XmlElement("tattoo_tags")]
    public TattooTags? TattooTags { get; set; }

    [XmlIgnore]
    public bool HasContent => (FaceKeyTemplate != null && FaceKeyTemplate.HasContent)
        || (BodyProperties != null && BodyProperties.HasContent)
        || (HairTags != null && HairTags.HasContent)
        || (BeardTags != null && BeardTags.HasContent)
        || (TattooTags != null && TattooTags.HasContent);
}

public class FaceKeyTemplate
{
    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlIgnore]
    public bool HasContent => !string.IsNullOrEmpty(Value);

    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class BodyPropertiesMax
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

public class NpcTraits
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class NpcUpgradeTargets
{
    [XmlElement("upgrade_target")]
    public List<UpgradeTarget> UpgradeTargetList { get; set; } = new List<UpgradeTarget>();

    [XmlIgnore]
    public bool HasContent => UpgradeTargetList.Count > 0;

    public bool ShouldSerializeUpgradeTargetList() => UpgradeTargetList.Count > 0;
}

public class NpcEquipments
{
    [XmlElement("EquipmentRoster")]
    public List<EquipmentRoster> EquipmentRosterList { get; set; } = new List<EquipmentRoster>();

    [XmlElement("EquipmentSet")]
    public List<NpcEquipmentSet> EquipmentSetList { get; set; } = new List<NpcEquipmentSet>();

    [XmlElement("equipment")]
    public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();

    [XmlIgnore]
    public bool HasContent => EquipmentRosterList.Count > 0 || EquipmentSetList.Count > 0 || EquipmentList.Count > 0;
}

public class EquipmentRoster
{
    [XmlAttribute("civilian")]
    public string? Civilian { get; set; }

    [XmlElement("equipment")]
    public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();

    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
}

public class Equipment
{
    [XmlAttribute("slot")]
    public string? Slot { get; set; }

    [XmlAttribute("id")]
    public string? Id { get; set; }

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}

public class NpcEquipmentSet
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("civilian")]
    public string? Civilian { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
}
