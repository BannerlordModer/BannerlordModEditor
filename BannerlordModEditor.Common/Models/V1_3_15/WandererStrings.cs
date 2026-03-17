using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("base")]
public class WandererStrings
{
    [XmlElement("string")]
    public List<StringItem> Strings { get; set; } = new List<StringItem>();
}

public class WandererStringItem
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
}
