using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("partyTemplates")]
public class PartyTemplates
{
    [XmlElement("MBPartyTemplate")]
    public List<MBPartyTemplate> MBPartyTemplateList { get; set; } = new List<MBPartyTemplate>();

    public bool ShouldSerializeMBPartyTemplateList() => MBPartyTemplateList.Count > 0;
}

public class MBPartyTemplate
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("stacks")]
    public Stacks? Stacks { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeStacks() => Stacks != null && Stacks.PartyTemplateStackList.Count > 0;
}

public class Stacks
{
    [XmlElement("PartyTemplateStack")]
    public List<PartyTemplateStack> PartyTemplateStackList { get; set; } = new List<PartyTemplateStack>();

    public bool ShouldSerializePartyTemplateStackList() => PartyTemplateStackList.Count > 0;
}

public class PartyTemplateStack
{
    [XmlAttribute("min_value")]
    public string MinValue { get; set; } = string.Empty;

    [XmlAttribute("max_value")]
    public string MaxValue { get; set; } = string.Empty;

    [XmlAttribute("troop")]
    public string Troop { get; set; } = string.Empty;

    public bool ShouldSerializeMinValue() => !string.IsNullOrEmpty(MinValue);
    public bool ShouldSerializeMaxValue() => !string.IsNullOrEmpty(MaxValue);
    public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
}
