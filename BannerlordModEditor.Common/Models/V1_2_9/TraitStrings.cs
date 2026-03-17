using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("base")]
public class TraitStrings
{
    [XmlElement("string")]
    public List<StringItem> Strings { get; set; } = new List<StringItem>();
}

public class StringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    [XmlElement("tags")]
    public Tags? Tags { get; set; }

    public bool ShouldSerializeTags() => Tags != null && Tags.TagList.Count > 0;
}

public class Tags
{
    [XmlElement("tag")]
    public List<Tag> TagList { get; set; } = new List<Tag>();
}

public class Tag
{
    [XmlAttribute("tag_name")]
    public string TagName { get; set; } = string.Empty;

    [XmlAttribute("weight")]
    public string Weight { get; set; } = string.Empty;

    public bool ShouldSerializeTagName() => !string.IsNullOrEmpty(TagName);
    public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
}
