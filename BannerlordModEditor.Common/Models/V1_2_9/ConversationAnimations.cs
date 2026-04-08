using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("ConversationAnimations")]
public class ConversationAnimations
{
    [XmlElement("IdleAnim")]
    public List<IdleAnim> IdleAnimList { get; set; } = new List<IdleAnim>();

    public bool ShouldSerializeIdleAnimList() => IdleAnimList.Count > 0;
}

public class IdleAnim
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("Reactions")]
    public Reactions? Reactions { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}

public class Reactions
{
    [XmlElement("Reaction")]
    public List<Reaction> ReactionList { get; set; } = new List<Reaction>();
}

public class Reaction
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
