using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

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

    [XmlAttribute("action_id_1")]
    public string? ActionId1 { get; set; }

    [XmlAttribute("action_id_2")]
    public string? ActionId2 { get; set; }

    [XmlElement("Reactions")]
    public Reactions? Reactions { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeActionId1() => !string.IsNullOrEmpty(ActionId1);
    public bool ShouldSerializeActionId2() => !string.IsNullOrEmpty(ActionId2);
    public bool ShouldSerializeReactions() => Reactions != null && Reactions.ReactionList.Count > 0;
}

public class Reactions
{
    [XmlElement("Reaction")]
    public List<Reaction> ReactionList { get; set; } = new List<Reaction>();

    public bool ShouldSerializeReactionList() => ReactionList.Count > 0;
}

public class Reaction
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("action_id")]
    public string? ActionId { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeActionId() => !string.IsNullOrEmpty(ActionId);
}
