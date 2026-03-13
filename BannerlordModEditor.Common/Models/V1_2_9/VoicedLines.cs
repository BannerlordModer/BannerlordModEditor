using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("Base")]
public class VoicedLines
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("VoiceOvers")]
    public VoiceOvers? VoiceOvers { get; set; }

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeVoiceOvers() => VoiceOvers != null && VoiceOvers.VoiceOverList.Count > 0;
}

public class VoiceOvers
{
    [XmlElement("VoiceOver")]
    public List<VoiceOver> VoiceOverList { get; set; } = new List<VoiceOver>();
}

public class VoiceOver
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("Voice")]
    public Voice? Voice { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeVoice() => Voice != null && !string.IsNullOrEmpty(Voice.Path);
}

public class Voice
{
    [XmlAttribute("path")]
    public string Path { get; set; } = string.Empty;

    public bool ShouldSerializePath() => !string.IsNullOrEmpty(Path);
}
