using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("ConversationScenes")]
public class ConversationScenes
{
    [XmlElement("Scene")]
    public List<ConversationScene> SceneList { get; set; } = new List<ConversationScene>();

    public bool ShouldSerializeSceneList() => SceneList.Count > 0;
}

public class ConversationScene
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("terrain")]
    public string Terrain { get; set; } = string.Empty;

    [XmlAttribute("forest_density")]
    public string ForestDensity { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeTerrain() => !string.IsNullOrEmpty(Terrain);
    public bool ShouldSerializeForestDensity() => !string.IsNullOrEmpty(ForestDensity);
}
