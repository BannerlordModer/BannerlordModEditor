using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("Cheats")]
public class SandboxCheats
{
    [XmlElement("Cheat")]
    public List<SandboxCheat> CheatList { get; set; } = new List<SandboxCheat>();

    public bool ShouldSerializeCheatList() => CheatList.Count > 0;
}

public class SandboxCheat
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
