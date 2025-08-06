using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("taunt_usage_sets")]
    public class TauntUsageSets
    {
        [XmlElement("taunt_usage_set")]
        public List<TauntUsageSet> TauntUsageSetList { get; set; } = new List<TauntUsageSet>();
    }

    public class TauntUsageSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("taunt_usage")]
        public List<TauntUsage> TauntUsages { get; set; } = new List<TauntUsage>();
    }

    public class TauntUsage
    {
        [XmlAttribute("is_left_stance")]
        public bool IsLeftStance { get; set; }
        public bool ShouldSerializeIsLeftStance() => _isLeftStanceSpecified;
        [XmlIgnore]
        public bool _isLeftStanceSpecified;

        [XmlAttribute("requires_bow")]
        public bool RequiresBow { get; set; }
        public bool ShouldSerializeRequiresBow() => _requiresBowSpecified;
        [XmlIgnore]
        public bool _requiresBowSpecified;

        [XmlAttribute("requires_on_foot")]
        public bool RequiresOnFoot { get; set; }
        public bool ShouldSerializeRequiresOnFoot() => _requiresOnFootSpecified;
        [XmlIgnore]
        public bool _requiresOnFootSpecified;

        [XmlAttribute("requires_shield")]
        public bool RequiresShield { get; set; }
        public bool ShouldSerializeRequiresShield() => _requiresShieldSpecified;
        [XmlIgnore]
        public bool _requiresShieldSpecified;

        [XmlAttribute("unsuitable_for_shield")]
        public bool UnsuitableForShield { get; set; }
        public bool ShouldSerializeUnsuitableForShield() => _unsuitableForShieldSpecified;
        [XmlIgnore]
        public bool _unsuitableForShieldSpecified;

        [XmlAttribute("unsuitable_for_bow")]
        public bool UnsuitableForBow { get; set; }
        public bool ShouldSerializeUnsuitableForBow() => _unsuitableForBowSpecified;
        [XmlIgnore]
        public bool _unsuitableForBowSpecified;

        [XmlAttribute("unsuitable_for_crossbow")]
        public bool UnsuitableForCrossbow { get; set; }
        public bool ShouldSerializeUnsuitableForCrossbow() => _unsuitableForCrossbowSpecified;
        [XmlIgnore]
        public bool _unsuitableForCrossbowSpecified;

        [XmlAttribute("unsuitable_for_two_handed")]
        public bool UnsuitableForTwoHanded { get; set; }
        public bool ShouldSerializeUnsuitableForTwoHanded() => _unsuitableForTwoHandedSpecified;
        [XmlIgnore]
        public bool _unsuitableForTwoHandedSpecified;

        [XmlAttribute("unsuitable_for_empty")]
        public bool UnsuitableForEmpty { get; set; }
        public bool ShouldSerializeUnsuitableForEmpty() => _unsuitableForEmptySpecified;
        [XmlIgnore]
        public bool _unsuitableForEmptySpecified;

        [XmlAttribute("action")]
        public string Action { get; set; }
        public bool ShouldSerializeAction() => !string.IsNullOrEmpty(Action);
    }
}