using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("base")]
public class SandboxView
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("tags")]
    public LanguageTags? Tags { get; set; }

    [XmlElement("strings")]
    public StringList? Strings { get; set; }

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeTags() => Tags != null && Tags.TagList.Count > 0;
    public bool ShouldSerializeStrings() => Strings != null && Strings.StringItems.Count > 0;
}
