using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("bone_body_types")]
    public class BoneBodyTypes
    {
        [XmlElement("bone_body_type")]
        public List<BoneBodyType> Items { get; set; } = new List<BoneBodyType>();
    }

    public class BoneBodyType
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("priority")]
        public int Priority { get; set; }

        [XmlAttribute("activate_sweep")]
        public bool? ActivateSweep { get; set; }

        [XmlAttribute("use_smaller_radius_mult_while_holding_shield")]
        public bool? UseSmallerRadiusMultWhileHoldingShield { get; set; }

        [XmlAttribute("do_not_scale_according_to_agent_scale")]
        public bool? DoNotScaleAccordingToAgentScale { get; set; }

        public bool ShouldSerializeActivateSweep() => ActivateSweep.HasValue;
        public bool ShouldSerializeUseSmallerRadiusMultWhileHoldingShield() => UseSmallerRadiusMultWhileHoldingShield.HasValue;
        public bool ShouldSerializeDoNotScaleAccordingToAgentScale() => DoNotScaleAccordingToAgentScale.HasValue;
    }
}