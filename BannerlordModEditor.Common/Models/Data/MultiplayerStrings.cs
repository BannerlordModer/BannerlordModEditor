using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// 多人游戏字符串配置 - multiplayer_strings.xml
    /// 定义多人游戏模式中使用的本地化字符串
    /// </summary>
    [XmlRoot("strings")]
    public class MultiplayerStrings
    {
        /// <summary>
        /// 字符串列表
        /// </summary>
        [XmlElement("string")]
        public List<MultiplayerString> Strings { get; set; } = new List<MultiplayerString>();
    }

    /// <summary>
    /// 单个本地化字符串定义
    /// </summary>
    public class MultiplayerString
    {
        /// <summary>
        /// 字符串唯一标识符
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 本地化文本内容
        /// </summary>
        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
} 