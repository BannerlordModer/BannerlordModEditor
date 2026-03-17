using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("NPCCharacters")]
public class ObsoleteCharacters
{
    [XmlElement("NPCCharacter")]
    public List<ObsoleteNPCCharacter> NPCCharacterList { get; set; } = new List<ObsoleteNPCCharacter>();
}

public class ObsoleteNPCCharacter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
