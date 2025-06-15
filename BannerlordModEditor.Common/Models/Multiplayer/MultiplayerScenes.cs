using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Multiplayer
{
    /// <summary>
    /// Root element for MultiplayerScenes.xml - Contains multiplayer scene definitions
    /// </summary>
    [XmlRoot("MultiplayerScenes")]
    public class MultiplayerScenes
    {
        [XmlElement("Scene")]
        public List<MultiplayerScene> Scene { get; set; } = new List<MultiplayerScene>();
    }

    /// <summary>
    /// Individual multiplayer scene with supported game types
    /// </summary>
    public class MultiplayerScene
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("GameType")]
        public List<GameType> GameType { get; set; } = new List<GameType>();
    }

    /// <summary>
    /// Game type supported by a multiplayer scene
    /// </summary>
    public class GameType
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 