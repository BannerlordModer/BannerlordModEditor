using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("ArrayOfSkillData")]
    public class Skills
    {
        [XmlElement("SkillData")]
        public List<SkillData> SkillDataList { get; set; } = new List<SkillData>();
    }

    public class SkillData
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Modifiers")]
        public Modifiers? Modifiers { get; set; }

        [XmlElement("Documentation")]
        public string? Documentation { get; set; }
    }

    public class Modifiers
    {
        [XmlElement("AttributeModifier")]
        public List<AttributeModifier> AttributeModifiers { get; set; } = new List<AttributeModifier>();
    }

    public class AttributeModifier
    {
        [XmlAttribute("AttribCode")]
        public string AttribCode { get; set; } = string.Empty;

        [XmlAttribute("Modification")]
        public string Modification { get; set; } = string.Empty;

        [XmlAttribute("Value")]
        public string Value { get; set; } = string.Empty;
    }
} 