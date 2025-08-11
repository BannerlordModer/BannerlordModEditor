using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("bone_body_types")]
    public class BoneBodyTypesDO
    {
        [XmlElement("bone_body_type")]
        public List<BoneBodyTypeDO> Items { get; set; } = new List<BoneBodyTypeDO>();

        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class BoneBodyTypeDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("priority")]
        public string? Priority { get; set; }

        [XmlElement("activate_sweep")]
        public string? ActivateSweep { get; set; }

        [XmlElement("use_smaller_radius_mult_while_holding_shield")]
        public string? UseSmallerRadiusMultWhileHoldingShield { get; set; }

        [XmlElement("do_not_scale_according_to_agent_scale")]
        public string? DoNotScaleAccordingToAgentScale { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePriority() => !string.IsNullOrEmpty(Priority);
        public bool ShouldSerializeActivateSweep() => !string.IsNullOrEmpty(ActivateSweep);
        public bool ShouldSerializeUseSmallerRadiusMultWhileHoldingShield() => !string.IsNullOrEmpty(UseSmallerRadiusMultWhileHoldingShield);
        public bool ShouldSerializeDoNotScaleAccordingToAgentScale() => !string.IsNullOrEmpty(DoNotScaleAccordingToAgentScale);
    }
}