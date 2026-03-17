using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("base")]
public class ConceptStrings
{
    [XmlElement("string")]
    public List<ConceptStringItem> Strings { get; set; } = new List<ConceptStringItem>();
}

public class ConceptStringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    [XmlAttribute("title")]
    public string? Title { get; set; }

    [XmlAttribute("group")]
    public string? Group { get; set; }

    [XmlAttribute("link_id")]
    public string? LinkId { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);
    public bool ShouldSerializeGroup() => !string.IsNullOrEmpty(Group);
    public bool ShouldSerializeLinkId() => !string.IsNullOrEmpty(LinkId);
}
