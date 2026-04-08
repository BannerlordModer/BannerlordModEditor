using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("NPCCharacters")]
public class Bandits
{
    [XmlElement("NPCCharacter")]
    public List<BanditCharacter> NPCCharacterList { get; set; } = new List<BanditCharacter>();
}

public class BanditCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("default_group")]
    public string? DefaultGroup { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("level")]
    public string? Level { get; set; }

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlElement("face")]
    public BanditFace? Face { get; set; }

    [XmlElement("skills")]
    public BanditSkills? Skills { get; set; }

    [XmlElement("Equipments")]
    public BanditEquipments? Equipments { get; set; }

    [XmlElement("upgrade_targets")]
    public BanditUpgradeTargets? UpgradeTargets { get; set; }

    public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeLevel() => !string.IsNullOrEmpty(Level);
    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeFace() => Face != null && Face.HasContent;
    public bool ShouldSerializeSkills() => Skills != null && Skills.HasContent;
    public bool ShouldSerializeEquipments() => Equipments != null && Equipments.HasContent;
    public bool ShouldSerializeUpgradeTargets() => UpgradeTargets != null && UpgradeTargets.HasContent;
}

public class BanditFace
{
    [XmlElement("BodyProperties")]
    public BanditBodyProperties? BodyProperties { get; set; }

    [XmlElement("BodyPropertiesMax")]
    public BanditBodyProperties? BodyPropertiesMax { get; set; }

    [XmlIgnore]
    public bool HasContent => BodyProperties != null || BodyPropertiesMax != null;
}

public class BanditBodyProperties
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
}

public class BanditSkills
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class BanditEquipments
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class BanditUpgradeTargets
{
    [XmlIgnore]
    public bool HasContent => false;
}
