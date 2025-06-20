using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models;

[XmlRoot("ArrayOfSkillData")]
public class Skills
{
    [XmlElement("SkillData")]
    public SkillData[]? SkillsList { get; set; }
}

public class SkillData
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("Name")]
    public string? Name { get; set; }

    [XmlArray("Modifiers")]
    [XmlArrayItem("AttributeModifier")]
    public AttributeModifier[]? Modifiers { get; set; }

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
    public double Value { get; set; }
} 