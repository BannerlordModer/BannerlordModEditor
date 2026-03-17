using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Language;

[XmlRoot("LanguageData")]
public class LanguageDataDO
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
    public List<LanguageFileDO> LanguageFiles { get; set; } = new List<LanguageFileDO>();

    [XmlElement("VoiceFile")]
    public List<VoiceFileDO> VoiceFiles { get; set; } = new List<VoiceFileDO>();

    public bool ShouldSerializeLanguageFiles() => LanguageFiles != null && LanguageFiles.Count > 0;
    public bool ShouldSerializeVoiceFiles() => VoiceFiles != null && VoiceFiles.Count > 0;
}

public class LanguageFileDO
{
    [XmlAttribute("xml_path")]
    public string XmlPath { get; set; } = string.Empty;
}

public class VoiceFileDO
{
    [XmlAttribute("xml_path")]
    public string XmlPath { get; set; } = string.Empty;
}