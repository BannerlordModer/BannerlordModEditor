using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("strings")]
public class ModuleStrings
{
    [XmlElement("string")]
    public List<ModuleString> StringList { get; set; } = new List<ModuleString>();
}

public class ModuleString
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("text")]
    public string Text { get; set; } = string.Empty;
}
