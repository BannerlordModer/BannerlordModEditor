using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("base")]
public class LanguageStrings
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

public class LanguageTags
{
    [XmlElement("tag")]
    public List<LanguageTag> TagList { get; set; } = new List<LanguageTag>();
}

public class LanguageTag
{
    [XmlAttribute("language")]
    public string Language { get; set; } = string.Empty;

    public bool ShouldSerializeLanguage() => !string.IsNullOrEmpty(Language);
}

public class StringList
{
    [XmlElement("string")]
    public List<LanguageString> StringItems { get; set; } = new List<LanguageString>();
}

public class LanguageString
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
}
