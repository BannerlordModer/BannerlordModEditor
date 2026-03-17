using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.V1_3_15;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("EquipmentRosters")]
public class SandboxEquipmentSets
{
    [XmlElement("EquipmentRoster")]
    public List<SandboxEquipmentRoster> EquipmentRosterList { get; set; } = new List<SandboxEquipmentRoster>();

    public bool ShouldSerializeEquipmentRosterList() => EquipmentRosterList.Count > 0;
}

public class SandboxEquipmentRoster
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("culture")]
    public string Culture { get; set; } = string.Empty;

    [XmlElement("EquipmentSet")]
    public List<SandboxEquipmentSet> EquipmentSetList { get; set; } = new List<SandboxEquipmentSet>();

    public bool ShouldSerializeEquipmentSetList() => EquipmentSetList.Count > 0;

    [XmlElement("Flags")]
    public SandboxFlags? Flags { get; set; }

    public bool ShouldSerializeFlags() => Flags != null;
}

public class SandboxEquipmentSet
{
    [XmlAttribute("civilian")]
    public string Civilian { get; set; } = string.Empty;

    public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);

    [XmlElement("Equipment")]
    public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();

    public bool ShouldSerializeEquipmentList() => EquipmentList.Count > 0;
}

public class SandboxFlags
{
    [XmlAttribute("IsNobleTemplate")]
    public string IsNobleTemplate { get; set; } = string.Empty;

    [XmlAttribute("IsCombatantTemplate")]
    public string IsCombatantTemplate { get; set; } = string.Empty;

    public bool ShouldSerializeIsNobleTemplate() => !string.IsNullOrEmpty(IsNobleTemplate);

    public bool ShouldSerializeIsCombatantTemplate() => !string.IsNullOrEmpty(IsCombatantTemplate);
}
