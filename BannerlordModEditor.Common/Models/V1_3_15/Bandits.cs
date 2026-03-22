using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

// bandits.xml
[XmlRoot("NPCCharacters")]
public class Bandits
{
    [XmlElement("NPCCharacter")]
    public List<NPCCharacter> NPCCharacterList { get; set; } = new List<NPCCharacter>();

    public bool ShouldSerializeNPCCharacterList() => NPCCharacterList.Count > 0;
}
