using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 语言XML的基础DO模型
    /// </summary>
    [XmlRoot("base")]
    public class LanguageBaseDO
    {
        public LanguageBaseDO()
        {
            Tags = new LanguageTagsDO();
            Strings = new List<LanguageStringDO>();
            Functions = new List<LanguageFunctionDO>();
            Type = "string";
        }

        [XmlAttribute("type")]
        public string Type { get; set; } = "string";

        [XmlElement("tags")]
        public LanguageTagsDO Tags { get; set; }

        [XmlArray("strings")]
        [XmlArrayItem("string")]
        public List<LanguageStringDO> Strings { get; set; }

        [XmlArray("functions")]
        [XmlArrayItem("function")]
        public List<LanguageFunctionDO> Functions { get; set; }

        // 序列化控制方法
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeTags() => Tags != null && Tags.Tags.Count > 0;
        public bool ShouldSerializeStrings() => Strings != null && Strings.Count > 0;
        public bool ShouldSerializeFunctions() => Functions != null && Functions.Count > 0;

        // 运行时属性
        [XmlIgnore]
        public bool HasTags => Tags != null && Tags.Tags.Count > 0;
        [XmlIgnore]
        public bool HasStrings => Strings != null && Strings.Count > 0;
        [XmlIgnore]
        public bool HasFunctions => Functions != null && Functions.Count > 0;
    }

    /// <summary>
    /// 语言标签容器
    /// </summary>
    public class LanguageTagsDO
    {
        public LanguageTagsDO()
        {
            Tags = new List<LanguageTagDO>();
        }

        [XmlElement("tag")]
        public List<LanguageTagDO> Tags { get; set; }

        [XmlIgnore]
        public bool HasTags => Tags != null && Tags.Count > 0;
    }

    /// <summary>
    /// 语言标签
    /// </summary>
    public class LanguageTagDO
    {
        [XmlAttribute("language")]
        public string Language { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasLanguage => !string.IsNullOrEmpty(Language);
    }

    /// <summary>
    /// 语言字符串
    /// </summary>
    public class LanguageStringDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string Text { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasId => !string.IsNullOrEmpty(Id);
        [XmlIgnore]
        public bool HasText => !string.IsNullOrEmpty(Text);
    }

    /// <summary>
    /// 语言函数
    /// </summary>
    public class LanguageFunctionDO
    {
        [XmlAttribute("functionName")]
        public string FunctionName { get; set; } = string.Empty;

        [XmlAttribute("functionBody")]
        public string FunctionBody { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasFunctionName => !string.IsNullOrEmpty(FunctionName);
        [XmlIgnore]
        public bool HasFunctionBody => !string.IsNullOrEmpty(FunctionBody);
    }
}