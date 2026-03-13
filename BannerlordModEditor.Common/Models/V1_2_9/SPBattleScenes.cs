using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("Scenes")]
public class SPBattleScenes
{
    [XmlElement("Scene")]
    public List<SPBattleScene> SceneList { get; set; } = new List<SPBattleScene>();
}

public class SPBattleScene
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
