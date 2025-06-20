using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game;

// parties.xml - Party definitions
[XmlRoot("base")]
public class PartiesBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "party";

    [XmlElement("parties")]
    public PartiesContainer Parties { get; set; } = new PartiesContainer();
}

public class PartiesContainer
{
    [XmlElement("party")]
    public List<Party> Party { get; set; } = new List<Party>();
}

public class Party
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("flags")]
    public string? Flags { get; set; }

    [XmlAttribute("party_template")]
    public string? PartyTemplate { get; set; }

    [XmlAttribute("position")]
    public string? Position { get; set; }

    [XmlAttribute("average_bearing_rot")]
    public string? AverageBearingRot { get; set; }

    [XmlElement("field")]
    public List<PartyField>? Field { get; set; }
}

public class PartyField
{
    [XmlAttribute("field_name")]
    public string? FieldName { get; set; }

    [XmlAttribute("field_value")]
    public string? FieldValue { get; set; }
} 