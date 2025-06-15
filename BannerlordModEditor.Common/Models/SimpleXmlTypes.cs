using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    // native_skill_sets.xml
    [XmlRoot("SkillSets")]
    public class SkillSets
    {
        // Empty container - may be populated in future
    }

    // mpbodypropertytemplates.xml
    [XmlRoot("BodyProperties")]
    public class BodyProperties
    {
        // Empty container - may be populated in future
    }

    // native_equipment_sets.xml
    [XmlRoot("EquipmentRosters")]
    public class EquipmentRosters
    {
        // Empty container - may be populated in future
    }

    // bone_body_types.xml
    [XmlRoot("bone_body_types")]
    public class BoneBodyTypes
    {
        [XmlElement("bone_body_type")]
        public List<BoneBodyType> BoneBodyType { get; set; } = new List<BoneBodyType>();
    }

    public class BoneBodyType
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("priority")]
        public string Priority { get; set; } = string.Empty;

        [XmlAttribute("activate_sweep")]
        public string? ActivateSweep { get; set; }

        [XmlAttribute("use_smaller_radius_mult_while_holding_shield")]
        public string? UseSmallerRadiusMultWhileHoldingShield { get; set; }

        [XmlAttribute("do_not_scale_according_to_agent_scale")]
        public string? DoNotScaleAccordingToAgentScale { get; set; }
    }
} 