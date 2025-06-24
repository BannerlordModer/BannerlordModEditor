using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("SkillSets")]
    public class NativeSkillSets
    {
        [XmlElement("SkillSet")]
        public List<NativeSkillSet> SkillSets { get; set; } = new List<NativeSkillSet>();
    }

    public class NativeSkillSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("Skill")]
        public List<NativeSkill> Skills { get; set; } = new List<NativeSkill>();

        // ShouldSerialize methods to control serialization
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkills() => Skills.Count > 0;
    }

    public class NativeSkill
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public int Value { get; set; }

        // ShouldSerialize methods to control serialization
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeValue() => Value > 0;
    }
} 