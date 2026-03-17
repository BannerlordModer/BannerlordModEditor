using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("EquipmentRosters")]
public class EducationEquipmentTemplates
{
    [XmlElement("EquipmentRoster")]
    public List<EducationEquipmentTemplateRoster> EquipmentRosterList { get; set; } = new List<EducationEquipmentTemplateRoster>();
}

public class EducationEquipmentTemplateRoster
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("EquipmentSet")]
    public EducationEquipmentSetContainer? EquipmentSet { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}

public class EducationEquipmentSetContainer
{
    [XmlElement("Equipment")]
    public List<EducationEquipmentTemplate> EquipmentList { get; set; } = new List<EducationEquipmentTemplate>();
}

public class EducationEquipmentTemplate
{
    [XmlAttribute("slot")]
    public string? Slot { get; set; }

    [XmlAttribute("id")]
    public string? Id { get; set; }

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
