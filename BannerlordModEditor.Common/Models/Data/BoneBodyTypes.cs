using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// bone_body_types.xml - Body part definitions for bone collision detection
[XmlRoot("bone_body_types")]
public class BoneBodyTypes
{
    [XmlElement("bone_body_type")]
    public List<BoneBodyType> BoneBodyType { get; set; } = new();
}

public class BoneBodyType
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("priority")]
    public string? Priority { get; set; }

    [XmlAttribute("activate_sweep")]
    public string? ActivateSweep { get; set; }

    [XmlAttribute("use_smaller_radius_mult_while_holding_shield")]
    public string? UseSmallerRadiusMultWhileHoldingShield { get; set; }

    [XmlAttribute("do_not_scale_according_to_agent_scale")]
    public string? DoNotScaleAccordingToAgentScale { get; set; }
} 