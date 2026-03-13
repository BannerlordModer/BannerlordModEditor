using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("ConversationAnimations")]
public class ConversationAnimations
{
    [XmlElement("Animation")]
    public List<ConversationAnimation> AnimationList { get; set; } = new List<ConversationAnimation>();

    public bool ShouldSerializeAnimationList() => AnimationList.Count > 0;
}

public class ConversationAnimation
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
