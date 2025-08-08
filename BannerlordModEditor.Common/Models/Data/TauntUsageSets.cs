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
        private string? _isLeftStance;
        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance
        {
            get => _isLeftStance;
            set => _isLeftStance = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeIsLeftStance() => !string.IsNullOrEmpty(IsLeftStance);

        private string? _requiresBow;
        [XmlAttribute("requires_bow")]
        public string? RequiresBow
        {
            get => _requiresBow;
            set => _requiresBow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresBow() => !string.IsNullOrEmpty(RequiresBow);

        private string? _requiresOnFoot;
        [XmlAttribute("requires_on_foot")]
        public string? RequiresOnFoot
        {
            get => _requiresOnFoot;
            set => _requiresOnFoot = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresOnFoot() => !string.IsNullOrEmpty(RequiresOnFoot);

        private string? _requiresShield;
        [XmlAttribute("requires_shield")]
        public string? RequiresShield
        {
            get => _requiresShield;
            set => _requiresShield = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresShield() => !string.IsNullOrEmpty(RequiresShield);

        private string? _unsuitableForShield;
        [XmlAttribute("unsuitable_for_shield")]
        public string? UnsuitableForShield
        {
            get => _unsuitableForShield;
            set => _unsuitableForShield = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForShield() => !string.IsNullOrEmpty(UnsuitableForShield);

        private string? _unsuitableForBow;
        [XmlAttribute("unsuitable_for_bow")]
        public string? UnsuitableForBow
        {
            get => _unsuitableForBow;
            set => _unsuitableForBow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForBow() => !string.IsNullOrEmpty(UnsuitableForBow);

        private string? _unsuitableForCrossbow;
        [XmlAttribute("unsuitable_for_crossbow")]
        public string? UnsuitableForCrossbow
        {
            get => _unsuitableForCrossbow;
            set => _unsuitableForCrossbow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForCrossbow() => !string.IsNullOrEmpty(UnsuitableForCrossbow);

        private string? _unsuitableForTwoHanded;
        [XmlAttribute("unsuitable_for_two_handed")]
        public string? UnsuitableForTwoHanded
        {
            get => _unsuitableForTwoHanded;
            set => _unsuitableForTwoHanded = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForTwoHanded() => !string.IsNullOrEmpty(UnsuitableForTwoHanded);

        private string? _unsuitableForEmpty;
        [XmlAttribute("unsuitable_for_empty")]
        public string? UnsuitableForEmpty
        {
            get => _unsuitableForEmpty;
            set => _unsuitableForEmpty = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForEmpty() => !string.IsNullOrEmpty(UnsuitableForEmpty);

        [XmlAttribute("action")]
        public string Action { get; set; }
        public bool ShouldSerializeAction() => !string.IsNullOrEmpty(Action);
    }
}