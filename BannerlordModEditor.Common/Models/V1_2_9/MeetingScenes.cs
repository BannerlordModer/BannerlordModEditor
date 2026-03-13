using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("MeetingScenes")]
public class MeetingScenes
{
    [XmlElement("Scene")]
    public List<MeetingScene> SceneList { get; set; } = new List<MeetingScene>();
}

public class MeetingScene
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
