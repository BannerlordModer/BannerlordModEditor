using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

// partyTemplates.xml - Party Templates for Bannerlord 1.3.15
// Root element: <partyTemplates>
[XmlRoot("partyTemplates")]
public class PartyTemplates
{
    [XmlElement("MBPartyTemplate")]
    public List<MBPartyTemplate> PartyTemplateList { get; set; } = new List<MBPartyTemplate>();
}

// MBPartyTemplate attributes:
// - id, difficulty, preferred_role, max_number_of_parties, min_number_of_parties
public class MBPartyTemplate
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("difficulty")]
    public string? Difficulty { get; set; }

    [XmlAttribute("preferred_role")]
    public string? PreferredRole { get; set; }

    [XmlAttribute("max_number_of_parties")]
    public string? MaxNumberOfParties { get; set; }

    [XmlAttribute("min_number_of_parties")]
    public string? MinNumberOfParties { get; set; }

    // Child elements
    [XmlElement("stacks")]
    public Stacks? Stacks { get; set; }

    // ShouldSerialize methods for optional attributes
    public bool ShouldSerializeDifficulty() => !string.IsNullOrEmpty(Difficulty);
    public bool ShouldSerializePreferredRole() => !string.IsNullOrEmpty(PreferredRole);
    public bool ShouldSerializeMaxNumberOfParties() => !string.IsNullOrEmpty(MaxNumberOfParties);
    public bool ShouldSerializeMinNumberOfParties() => !string.IsNullOrEmpty(MinNumberOfParties);
    public bool ShouldSerializeStacks() => Stacks != null && Stacks.StackList.Count > 0;
}

// Stacks contains PartyTemplateStack elements
public class Stacks
{
    [XmlElement("PartyTemplateStack")]
    public List<PartyTemplateStack> StackList { get; set; } = new List<PartyTemplateStack>();
}

// PartyTemplateStack attributes:
// - min_value, max_value, troop
public class PartyTemplateStack
{
    // Optional attributes
    [XmlAttribute("min_value")]
    public string? MinValue { get; set; }

    [XmlAttribute("max_value")]
    public string? MaxValue { get; set; }

    [XmlAttribute("troop")]
    public string? Troop { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeMinValue() => !string.IsNullOrEmpty(MinValue);
    public bool ShouldSerializeMaxValue() => !string.IsNullOrEmpty(MaxValue);
    public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
}
