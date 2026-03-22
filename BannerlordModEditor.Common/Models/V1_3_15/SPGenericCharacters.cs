using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;


[XmlRoot("NPCCharacters")]
public class SPGenericCharacters
{
    [XmlElement("NPCCharacter")]
    public List<NPCCharacter> NPCCharacterList { get; set; } = new List<NPCCharacter>();
}
