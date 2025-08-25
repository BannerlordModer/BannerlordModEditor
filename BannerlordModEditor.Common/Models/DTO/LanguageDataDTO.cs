using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Language;

[XmlRoot("LanguageData")]
public class LanguageDataDTO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("subtitle_extension")]
    public string SubtitleExtension { get; set; } = string.Empty;

    [XmlAttribute("supported_iso")]
    public string SupportedIso { get; set; } = string.Empty;

    [XmlAttribute("text_processor")]
    public string TextProcessor { get; set; } = string.Empty;

    [XmlAttribute("under_development")]
    public bool UnderDevelopment { get; set; } = false;

    [XmlElement("LanguageFile")]
    public List<LanguageFileDTO> LanguageFiles { get; set; } = new List<LanguageFileDTO>();
}

public class LanguageFileDTO
{
    [XmlAttribute("xml_path")]
    public string XmlPath { get; set; } = string.Empty;
}