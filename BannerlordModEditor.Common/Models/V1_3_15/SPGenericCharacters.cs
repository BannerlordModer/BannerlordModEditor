using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("NPCCharacters")]
public class SPGenericCharacters
{
    [XmlElement("NPCCharacter")]
    public List<NPCCharacter> NPCCharacterList { get; set; } = new List<NPCCharacter>();
}

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

    [XmlIgnore]
    public bool HasContent => FaceKeyTemplate != null && FaceKeyTemplate.HasContent;
}

public class FaceKeyTemplate
{
    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlIgnore]
    public bool HasContent => !string.IsNullOrEmpty(Value);

    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class NpcCharacterSkills
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

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class NpcTraits
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class NpcUpgradeTargets
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class NpcEquipments
{
    [XmlElement("EquipmentRoster")]
    public List<EquipmentRoster> EquipmentRosterList { get; set; } = new List<EquipmentRoster>();

    [XmlElement("EquipmentSet")]
    public List<NpcEquipmentSet> EquipmentSetList { get; set; } = new List<NpcEquipmentSet>();

    [XmlIgnore]
    public bool HasContent => EquipmentRosterList.Count > 0 || EquipmentSetList.Count > 0;
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

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
