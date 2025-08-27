using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Language;

[XmlRoot("base")]
public class LanguageBaseDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("tags")]
    public TagsDTO Tags { get; set; } = new TagsDTO();

    [XmlArray("strings")]
    [XmlArrayItem("string")]
    public List<StringItemDTO> Strings { get; set; } = new List<StringItemDTO>();

    [XmlArray("functions")]
    [XmlArrayItem("function")]
    public List<FunctionItemDTO> Functions { get; set; } = new List<FunctionItemDTO>();
}

public class TagsDTO
{
    [XmlElement("tag")]
    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

    // 空元素标记
    [XmlIgnore]
    public bool HasEmptyTags { get; set; } = false;

    public bool ShouldSerializeTags() => (Tags != null && Tags.Count > 0) || HasEmptyTags;
}

public class TagDTO
{
    [XmlAttribute("language")]
    public string Language { get; set; } = string.Empty;
}

public class StringItemDTO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;
}

public class FunctionItemDTO
{
    [XmlAttribute("functionName")]
    public string FunctionName { get; set; } = string.Empty;

    [XmlAttribute("functionBody")]
    public string FunctionBody { get; set; } = string.Empty;
}