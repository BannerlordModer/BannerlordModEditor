using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO;

[XmlRoot("base")]
public class LanguageBaseDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("tags")]
    public LanguageTagsDO Tags { get; set; } = new LanguageTagsDO();

    [XmlArray("strings")]
    [XmlArrayItem("string")]
    public List<LanguageStringDO> Strings { get; set; } = new List<LanguageStringDO>();

    [XmlArray("functions")]
    [XmlArrayItem("function")]
    public List<LanguageFunctionDO> Functions { get; set; } = new List<LanguageFunctionDO>();

  
    // 强制内容属性，确保标签不会自闭合
        [XmlIgnore]
        public string ForceContent { get; set; } = string.Empty;
        
        [XmlText]
        public string XmlContent
        {
            get => ForceContent;
            set => ForceContent = value;
        }

        // 序列化控制方法
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeTags() => (Tags != null && Tags.Tags.Count > 0) || HasEmptyTags;
        public bool ShouldSerializeStrings() => Strings != null && Strings.Count > 0;
        public bool ShouldSerializeFunctions() => Functions != null && Functions.Count > 0;
        public bool ShouldSerializeXmlContent() => false; // 永远不序列化内容，但确保标签不闭合

        // 运行时属性
        [XmlIgnore]
        public bool HasTags => Tags != null && Tags.Tags.Count > 0;
        [XmlIgnore]
        public bool HasStrings => Strings != null && Strings.Count > 0;
        [XmlIgnore]
        public bool HasFunctions => Functions != null && Functions.Count > 0;
  
        // 空元素标记
        [XmlIgnore]
        public bool HasEmptyTags { get; set; } = false;
    }

public class LanguageTagsDO
{
    [XmlElement("tag")]
    public List<LanguageTagDO> Tags { get; set; } = new List<LanguageTagDO>();

    public bool ShouldSerializeTags() => Tags != null && Tags.Count > 0;
}

public class LanguageTagDO
{
    [XmlAttribute("language")]
    public string Language { get; set; } = string.Empty;
}

public class LanguageStringDO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;
}

public class LanguageFunctionDO
{
    [XmlAttribute("functionName")]
    public string FunctionName { get; set; } = string.Empty;

    [XmlAttribute("functionBody")]
    public string FunctionBody { get; set; } = string.Empty;
}