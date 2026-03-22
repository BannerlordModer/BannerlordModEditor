using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("strings")]
public class CommentStrings
{
    [XmlElement("string")]
    public List<CommentStringItem> Strings { get; set; } = new List<CommentStringItem>();
}

public class CommentStringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    [XmlElement("tags")]
    public Tags? Tags { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
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
