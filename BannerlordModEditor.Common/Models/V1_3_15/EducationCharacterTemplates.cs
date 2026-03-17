using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("NPCCharacters")]
public class EducationCharacterTemplates
{
    [XmlElement("NPCCharacter")]
    public List<EducationCharacter> NPCCharacterList { get; set; } = new List<EducationCharacter>();
}

public class EducationCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlAttribute("default_group")]
    public string? DefaultGroup { get; set; }

    [XmlAttribute("is_hero")]
    public string? IsHero { get; set; }

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("skill_template")]
    public string? SkillTemplate { get; set; }

    [XmlAttribute("is_template")]
    public string? IsTemplate { get; set; }

    [XmlElement("face")]
    public EducationFace? Face { get; set; }

    [XmlElement("skills")]
    public EducationSkills? Skills { get; set; }

    [XmlElement("Equipments")]
    public EducationEquipments? Equipments { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
    public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeSkillTemplate() => !string.IsNullOrEmpty(SkillTemplate);
    public bool ShouldSerializeIsTemplate() => !string.IsNullOrEmpty(IsTemplate);
    public bool ShouldSerializeFace() => Face != null && Face.HasContent;
    public bool ShouldSerializeSkills() => Skills != null && Skills.HasContent;
    public bool ShouldSerializeEquipments() => Equipments != null && Equipments.HasContent;
}

public class EducationFace
{
    [XmlElement("face_key_template")]
    public NPCFaceKeyTemplate? FaceKeyTemplate { get; set; }

    [XmlIgnore]
    public bool HasContent => FaceKeyTemplate != null && FaceKeyTemplate.HasContent;
}

public class NPCFaceKeyTemplate
{
    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlIgnore]
    public bool HasContent => !string.IsNullOrEmpty(Value);

    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class EducationSkills
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class EducationEquipments
{
    [XmlElement("EquipmentRoster")]
    public List<EducationEquipmentRoster> EquipmentRosterList { get; set; } = new List<EducationEquipmentRoster>();

    [XmlIgnore]
    public bool HasContent => EquipmentRosterList.Count > 0;
}

public class EducationEquipmentRoster
{
    [XmlAttribute("civilian")]
    public string? Civilian { get; set; }

    [XmlElement("equipment")]
    public List<EducationEquipment> EquipmentList { get; set; } = new List<EducationEquipment>();

    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
}

public class EducationEquipment
{
    [XmlAttribute("slot")]
    public string? Slot { get; set; }

    [XmlAttribute("id")]
    public string? Id { get; set; }

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
