using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("SkillSets")]
public class SandboxSkillSets
{
    [XmlElement("SkillSet")]
    public List<SandboxSkillSet> SkillSetList { get; set; } = new List<SandboxSkillSet>();
}

public class SandboxSkillSet
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("skill")]
    public List<SandboxSkill> SkillList { get; set; } = new List<SandboxSkill>();

    public bool ShouldSerializeSkillList() => SkillList.Count > 0;
}

public class SandboxSkill
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public int Value { get; set; }
}
