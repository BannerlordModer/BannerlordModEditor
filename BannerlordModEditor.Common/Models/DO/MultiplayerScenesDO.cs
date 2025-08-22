using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Multiplayer;

[XmlRoot("MultiplayerScenes", Namespace = "")]
public class MultiplayerScenesDO
{
    [XmlElement("Scene")]
    public List<SceneDO> Scenes { get; set; } = new List<SceneDO>();

    public bool ShouldSerializeScenes() => Scenes != null && Scenes.Count > 0;
}

public class SceneDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("GameType")]
    public List<GameTypeDO> GameTypes { get; set; } = new List<GameTypeDO>();

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGameTypes() => GameTypes != null && GameTypes.Count > 0;
}

public class GameTypeDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}