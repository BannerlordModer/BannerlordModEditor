using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models;

[XmlRoot("ArrayOfSkillData")]
public class SkillsWrapper
{
    [XmlElement("SkillData")]
    public List<SkillData> Skills { get; set; } = new();
}

public class SkillData
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("Name")]
    public string? Name { get; set; }

    [XmlArray("Modifiers")]
    [XmlArrayItem("AttributeModifier")]
    public List<AttributeModifier> Modifiers { get; set; } = new();

    [XmlElement("Documentation")]
    public string? Documentation { get; set; }
}

public class AttributeModifier
{
    [XmlAttribute("AttribCode")]
    public string? AttribCode { get; set; }

    [XmlAttribute("Modification")]
    public string? Modification { get; set; }

    [XmlAttribute("Value")]
    public string? Value { get; set; }
} 