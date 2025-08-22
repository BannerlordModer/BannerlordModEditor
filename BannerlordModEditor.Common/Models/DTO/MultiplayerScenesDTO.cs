using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 精确映射 MultiplayerScenes.xml 的根对象。
    /// </summary>
    [XmlRoot("MultiplayerScenes")]
    public class MultiplayerScenesDTO
    {
        [XmlElement("Scene")]
        public List<SceneDTO> Scenes { get; set; } = new List<SceneDTO>();
    }

    /// <summary>
    /// 表示一个场景节点。
    /// </summary>
    public class SceneDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("GameType")]
        public List<GameTypeDTO> GameTypes { get; set; } = new List<GameTypeDTO>();
    }

    /// <summary>
    /// 表示一个游戏模式类型节点。
    /// </summary>
    public class GameTypeDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
}