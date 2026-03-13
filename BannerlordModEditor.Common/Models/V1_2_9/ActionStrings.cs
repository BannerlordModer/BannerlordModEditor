using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("strings")]
public class ActionStrings
{
    [XmlElement("string")]
    public List<ActionString> StringList { get; set; } = new List<ActionString>();
}

public class ActionString
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;
}
