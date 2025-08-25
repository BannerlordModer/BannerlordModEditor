using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Language;

[XmlRoot("base")]
public class LanguageBaseDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("tags")]
    public TagsDO Tags { get; set; } = new TagsDO();

    [XmlArray("strings")]
    [XmlArrayItem("string")]
    public List<StringItemDO> Strings { get; set; } = new List<StringItemDO>();

    [XmlArray("functions")]
    [XmlArrayItem("function")]
    public List<FunctionItemDO> Functions { get; set; } = new List<FunctionItemDO>();

    [XmlIgnore]
    public bool HasTags { get; set; } = false;

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

public class TagsDO
{
    [XmlElement("tag")]
    public List<TagDO> Tags { get; set; } = new List<TagDO>();

    public bool ShouldSerializeTags() => Tags != null && Tags.Count > 0;
}

public class TagDO
{
    [XmlAttribute("language")]
    public string Language { get; set; } = string.Empty;
}

public class StringItemDO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;
}

public class FunctionItemDO
{
    [XmlAttribute("functionName")]
    public string FunctionName { get; set; } = string.Empty;

    [XmlAttribute("functionBody")]
    public string FunctionBody { get; set; } = string.Empty;
}