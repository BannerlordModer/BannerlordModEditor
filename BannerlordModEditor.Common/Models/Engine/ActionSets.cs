using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine;

// action_sets.xml - Character animation action set definitions
[XmlRoot("base")]
public class ActionSetsBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "action_set";

    [XmlElement("action_sets")]
    public ActionSetsContainer ActionSets { get; set; } = new ActionSetsContainer();
}

public class ActionSetsContainer
{
    [XmlElement("action_set")]
    public List<ActionSet> ActionSet { get; set; } = new List<ActionSet>();
}

public class ActionSet
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("skeleton")]
    public string? Skeleton { get; set; }

    [XmlAttribute("voice_definition")]
    public string? VoiceDefinition { get; set; }

    [XmlElement("actions")]
    public ActionsContainer? Actions { get; set; }

    [XmlElement("action_groups")]
    public ActionGroupsContainer? ActionGroups { get; set; }

    [XmlElement("flags")]
    public ActionSetFlags? Flags { get; set; }
}

public class ActionsContainer
{
    [XmlElement("action")]
    public List<Action> Action { get; set; } = new List<Action>();
}

public class Action
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("animation")]
    public string? Animation { get; set; }

    [XmlAttribute("priority")]
    public string? Priority { get; set; }

    [XmlAttribute("duration")]
    public string? Duration { get; set; }

    [XmlAttribute("blend_in_period")]
    public string? BlendInPeriod { get; set; }

    [XmlAttribute("blend_out_period")]
    public string? BlendOutPeriod { get; set; }

    [XmlAttribute("action_type")]
    public string? ActionType { get; set; }

    [XmlAttribute("usage_direction")]
    public string? UsageDirection { get; set; }

    [XmlAttribute("weapon_usage")]
    public string? WeaponUsage { get; set; }

    [XmlAttribute("movement_type")]
    public string? MovementType { get; set; }

    [XmlAttribute("continue_to_action")]
    public string? ContinueToAction { get; set; }

    [XmlAttribute("sound_code")]
    public string? SoundCode { get; set; }

    [XmlAttribute("flags")]
    public string? Flags { get; set; }

    [XmlElement("parameters")]
    public ActionParameters? Parameters { get; set; }

    [XmlElement("transitions")]
    public ActionTransitions? Transitions { get; set; }
}

public class ActionParameters
{
    [XmlElement("parameter")]
    public List<ActionParameter> Parameter { get; set; } = new List<ActionParameter>();
}

public class ActionParameter
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }
}

public class ActionTransitions
{
    [XmlElement("transition")]
    public List<ActionTransition> Transition { get; set; } = new List<ActionTransition>();
}

public class ActionTransition
{
    [XmlAttribute("to_action")]
    public string? ToAction { get; set; }

    [XmlAttribute("condition")]
    public string? Condition { get; set; }

    [XmlAttribute("probability")]
    public string? Probability { get; set; }

    [XmlAttribute("blend_duration")]
    public string? BlendDuration { get; set; }
}

public class ActionGroupsContainer
{
    [XmlElement("action_group")]
    public List<ActionGroup> ActionGroup { get; set; } = new List<ActionGroup>();
}

public class ActionGroup
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("priority")]
    public string? Priority { get; set; }

    [XmlElement("group_actions")]
    public GroupActions? GroupActions { get; set; }
}

public class GroupActions
{
    [XmlElement("group_action")]
    public List<GroupAction> GroupAction { get; set; } = new List<GroupAction>();
}

public class GroupAction
{
    [XmlAttribute("action_id")]
    public string? ActionId { get; set; }

    [XmlAttribute("weight")]
    public string? Weight { get; set; }

    [XmlAttribute("condition")]
    public string? Condition { get; set; }
}

public class ActionSetFlags
{
    [XmlElement("flag")]
    public List<ActionSetFlag> Flag { get; set; } = new List<ActionSetFlag>();
}

public class ActionSetFlag
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
} 