using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("NPCCharacters")]
public class Caravans
{
    [XmlElement("NPCCharacter")]
    public List<CaravanCharacter> NPCCharacterList { get; set; } = new List<CaravanCharacter>();
}

public class CaravanCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("default_group")]
    public string? DefaultGroup { get; set; }

    [XmlAttribute("is_hero")]
    public string? IsHero { get; set; }

    [XmlAttribute("is_mercenary")]
    public string? IsMercenary { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("face")]
    public CaravanFace? Face { get; set; }

    [XmlElement("skills")]
    public CaravanSkills? Skills { get; set; }

    [XmlElement("Equipments")]
    public CaravanEquipments? Equipments { get; set; }

    public bool ShouldSerializeDefaultGroup() => !string.IsNullOrEmpty(DefaultGroup);
    public bool ShouldSerializeIsHero() => !string.IsNullOrEmpty(IsHero);
    public bool ShouldSerializeIsMercenary() => !string.IsNullOrEmpty(IsMercenary);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeFace() => Face != null && Face.HasContent;
    public bool ShouldSerializeSkills() => Skills != null;
    public bool ShouldSerializeEquipments() => Equipments != null && Equipments.HasContent;
}

public class CaravanFace
{
    [XmlElement("BodyProperties")]
    public CaravanBodyProperties? BodyProperties { get; set; }

    [XmlElement("BodyPropertiesMax")]
    public CaravanBodyProperties? BodyPropertiesMax { get; set; }

    [XmlIgnore]
    public bool HasContent => BodyProperties != null || BodyPropertiesMax != null;
}

public class CaravanBodyProperties
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

public class CaravanSkills
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class CaravanEquipments
{
    [XmlElement("EquipmentRoster")]
    public List<CaravanEquipmentRoster> EquipmentRosterList { get; set; } = new List<CaravanEquipmentRoster>();

    [XmlElement("EquipmentSet")]
    public List<CaravanEquipmentSet> EquipmentSetList { get; set; } = new List<CaravanEquipmentSet>();

    [XmlElement("equipment")]
    public List<CaravanEquipment> EquipmentList { get; set; } = new List<CaravanEquipment>();

    [XmlIgnore]
    public bool HasContent => EquipmentRosterList.Count > 0 || EquipmentSetList.Count > 0 || EquipmentList.Count > 0;
}

public class CaravanEquipmentRoster
{
    [XmlElement("equipment")]
    public List<CaravanEquipment> EquipmentList { get; set; } = new List<CaravanEquipment>();
}

public class CaravanEquipmentSet
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("civilian")]
    public string? Civilian { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
}

public class CaravanEquipment
{
    [XmlAttribute("slot")]
    public string? Slot { get; set; }

    [XmlAttribute("id")]
    public string? Id { get; set; }

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
