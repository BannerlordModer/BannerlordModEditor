using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("strings")]
public class CommentOnActionStrings
{
    [XmlElement("string")]
    public List<CommentOnActionStringItem> Strings { get; set; } = new List<CommentOnActionStringItem>();
}

public class CommentOnActionStringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    [XmlElement("tags")]
    public CommentOnActionTags? Tags { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeTags() => Tags != null && Tags.TagList.Count > 0;
}

public class CommentOnActionTags
{
    [XmlElement("tag")]
    public List<CommentOnActionTag> TagList { get; set; } = new List<CommentOnActionTag>();
}

public class CommentOnActionTag
{
    [XmlAttribute("tag_name")]
    public string TagName { get; set; } = string.Empty;

    public bool ShouldSerializeTagName() => !string.IsNullOrEmpty(TagName);
}
