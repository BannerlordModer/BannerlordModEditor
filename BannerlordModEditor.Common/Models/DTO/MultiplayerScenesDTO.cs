using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Multiplayer;

[XmlRoot("MultiplayerScenes", Namespace = "")]
public class MultiplayerScenesDTO
{
    [XmlElement("Scene")]
    public List<SceneDTO> Scenes { get; set; } = new List<SceneDTO>();

    public bool ShouldSerializeScenes() => Scenes != null && Scenes.Count > 0;
}

public class SceneDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("GameType")]
    public List<GameTypeDTO> GameTypes { get; set; } = new List<GameTypeDTO>();

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGameTypes() => GameTypes != null && GameTypes.Count > 0;
}

public class GameTypeDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}