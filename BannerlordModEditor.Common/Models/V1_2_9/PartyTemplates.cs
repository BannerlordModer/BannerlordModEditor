using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

// partyTemplates.xml - Party Templates for Bannerlord 1.2.9
// Root element: <partyTemplates>
[XmlRoot("partyTemplates")]
public class PartyTemplates
{
    [XmlElement("MBPartyTemplate")]
    public List<MBPartyTemplate> MBPartyTemplateList { get; set; } = new List<MBPartyTemplate>();
}

public class MBPartyTemplate
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("stacks")]
    public Stacks? Stacks { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}

public class Stacks
{
    [XmlElement("PartyTemplateStack")]
    public List<PartyTemplateStack> PartyTemplateStackList { get; set; } = new List<PartyTemplateStack>();
}

public class PartyTemplateStack
{
    [XmlAttribute("min_value")]
    public string? MinValue { get; set; }

    [XmlAttribute("max_value")]
    public string? MaxValue { get; set; }

    [XmlAttribute("troop")]
    public string? Troop { get; set; }

    public bool ShouldSerializeMinValue() => !string.IsNullOrEmpty(MinValue);
    public bool ShouldSerializeMaxValue() => !string.IsNullOrEmpty(MaxValue);
    public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
}
