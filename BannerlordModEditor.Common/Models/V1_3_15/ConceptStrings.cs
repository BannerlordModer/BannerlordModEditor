using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("Concepts")]
public class ConceptStrings
{
    [XmlElement("Concept")]
    public List<ConceptStringItem> ConceptList { get; set; } = new List<ConceptStringItem>();

    public bool ShouldSerializeConceptList() => ConceptList.Count > 0;
}

public class ConceptStringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("title")]
    public string? Title { get; set; }

    [XmlAttribute("group")]
    public string? Group { get; set; }

    [XmlAttribute("link_id")]
    public string? LinkId { get; set; }

    [XmlAttribute("text")]
    public string? Text { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);
    public bool ShouldSerializeGroup() => !string.IsNullOrEmpty(Group);
    public bool ShouldSerializeLinkId() => !string.IsNullOrEmpty(LinkId);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
}
