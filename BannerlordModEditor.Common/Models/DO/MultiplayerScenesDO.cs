using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 精确映射 MultiplayerScenes.xml 的根对象。
    /// </summary>
    [XmlRoot("MultiplayerScenes")]
    public class MultiplayerScenesDO
    {
        [XmlElement("Scene")]
        public List<SceneDO> Scenes { get; set; } = new List<SceneDO>();

        public bool ShouldSerializeScenes() => Scenes != null && Scenes.Count > 0;
    }

    /// <summary>
    /// 表示一个场景节点。
    /// </summary>
    public class SceneDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("GameType")]
        public List<GameTypeDO> GameTypes { get; set; } = new List<GameTypeDO>();

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeGameTypes() => GameTypes != null && GameTypes.Count > 0;
    }

    /// <summary>
    /// 表示一个游戏模式类型节点。
    /// </summary>
    public class GameTypeDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}