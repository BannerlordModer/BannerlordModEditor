using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Game
{
    [XmlRoot("ArrayOfSkillData")]
    public class SkillsDO
    {
        [XmlElement("SkillData")]
        public List<SkillDataDO> SkillDataList { get; set; } = new List<SkillDataDO>();
    }

    public class SkillDataDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Modifiers")]
        public ModifiersDO? Modifiers { get; set; }

        [XmlElement("Documentation")]
        public string? Documentation { get; set; }

        [XmlIgnore]
        public bool HasModifiers { get; set; } = false;

        public bool ShouldSerializeModifiers() => HasModifiers && Modifiers != null;
    }

    public class ModifiersDO
    {
        [XmlElement("AttributeModifier")]
        public List<AttributeModifierDO> AttributeModifiers { get; set; } = new List<AttributeModifierDO>();
    }

    public class AttributeModifierDO
    {
        [XmlAttribute("AttribCode")]
        public string AttribCode { get; set; } = string.Empty;

        [XmlAttribute("Modification")]
        public string Modification { get; set; } = string.Empty;

        [XmlAttribute("Value")]
        public string Value { get; set; } = string.Empty;
    }
}