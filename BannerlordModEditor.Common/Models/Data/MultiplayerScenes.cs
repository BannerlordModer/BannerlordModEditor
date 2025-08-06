using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// 精确映射 MultiplayerScenes.xml 的根对象。
    /// </summary>
    [XmlRoot("MultiplayerScenes")]
    public class MultiplayerScenes
    {
        [XmlElement("Scene")]
        public List<Scene> Scenes { get; set; } = new List<Scene>();

        public bool ShouldSerializeScenes() => Scenes != null && Scenes.Count > 0;
    }

    /// <summary>
    /// 表示一个场景节点。
    /// </summary>
    public class Scene
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("GameType")]
        public List<GameType> GameTypes { get; set; } = new List<GameType>();

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeGameTypes() => GameTypes != null && GameTypes.Count > 0;
    }

    /// <summary>
    /// 表示一个游戏模式类型节点。
    /// </summary>
    public class GameType
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}