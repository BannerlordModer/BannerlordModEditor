using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("base")]
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
