using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("LanguageData")]
public class LanguageData
{
    [XmlElement("Languages")]
    public Languages? Languages { get; set; }

    public bool ShouldSerializeLanguages() => Languages != null && Languages.LanguageList.Count > 0;
}

public class Languages
{
    [XmlElement("Language")]
    public List<Language> LanguageList { get; set; } = new List<Language>();
}

public class Language
{
    [XmlAttribute("code")]
    public string Code { get; set; } = string.Empty;

    [XmlElement("Id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("NativeName")]
    public string NativeName { get; set; } = string.Empty;

    public bool ShouldSerializeCode() => !string.IsNullOrEmpty(Code);
}
