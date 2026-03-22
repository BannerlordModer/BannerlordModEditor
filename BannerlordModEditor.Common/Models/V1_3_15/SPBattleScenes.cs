using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("SPBattleScenes")]
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
