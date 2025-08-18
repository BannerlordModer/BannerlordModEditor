using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Game
{
    [XmlRoot("ArrayOfSkillData")]
    public class SkillsDTO
    {
        [XmlElement("SkillData")]
        public List<SkillDataDTO> SkillDataList { get; set; } = new List<SkillDataDTO>();
    }

    public class SkillDataDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Modifiers")]
        public ModifiersDTO? Modifiers { get; set; }

        [XmlElement("Documentation")]
        public string? Documentation { get; set; }
    }

    public class ModifiersDTO
    {
        [XmlElement("AttributeModifier")]
        public List<AttributeModifierDTO> AttributeModifiers { get; set; } = new List<AttributeModifierDTO>();
    }

    public class AttributeModifierDTO
    {
        [XmlAttribute("AttribCode")]
        public string AttribCode { get; set; } = string.Empty;

        [XmlAttribute("Modification")]
        public string Modification { get; set; } = string.Empty;

        [XmlAttribute("Value")]
        public string Value { get; set; } = string.Empty;
    }
}