using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("strings")]
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
