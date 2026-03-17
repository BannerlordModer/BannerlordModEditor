using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("LanguageData")]
public class LanguageData
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("VoiceFile")]
    public List<VoiceFile> VoiceFiles { get; set; } = new List<VoiceFile>();

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeVoiceFiles() => VoiceFiles.Count > 0;
}

public class VoiceFile
{
    [XmlAttribute("xml_path")]
    public string XmlPath { get; set; } = string.Empty;

    public bool ShouldSerializeXmlPath() => !string.IsNullOrEmpty(XmlPath);
}
