using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("taunt_usage_sets")]
    public class TauntUsageSetsDO
    {
        [XmlElement("taunt_usage_set")]
        public List<TauntUsageSetDO> TauntUsageSetList { get; set; } = new List<TauntUsageSetDO>();
        
        public bool ShouldSerializeTauntUsageSetList() => TauntUsageSetList != null && TauntUsageSetList.Count > 0;
    }

    public class TauntUsageSetDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;
        
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);

        [XmlElement("taunt_usage")]
        public List<TauntUsageDO> TauntUsages { get; set; } = new List<TauntUsageDO>();
        
        public bool ShouldSerializeTauntUsages() => TauntUsages != null && TauntUsages.Count > 0;
    }

    public class TauntUsageDO
    {
        private string? _isLeftStance;
        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance
        {
            get => _isLeftStance;
            set => _isLeftStance = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeIsLeftStance() => IsLeftStance != null;

        private string? _requiresBow;
        [XmlAttribute("requires_bow")]
        public string? RequiresBow
        {
            get => _requiresBow;
            set => _requiresBow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresBow() => RequiresBow != null;

        private string? _requiresOnFoot;
        [XmlAttribute("requires_on_foot")]
        public string? RequiresOnFoot
        {
            get => _requiresOnFoot;
            set => _requiresOnFoot = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresOnFoot() => RequiresOnFoot != null;

        private string? _requiresShield;
        [XmlAttribute("requires_shield")]
        public string? RequiresShield
        {
            get => _requiresShield;
            set => _requiresShield = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeRequiresShield() => RequiresShield != null;

        private string? _unsuitableForShield;
        [XmlAttribute("unsuitable_for_shield")]
        public string? UnsuitableForShield
        {
            get => _unsuitableForShield;
            set => _unsuitableForShield = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForShield() => UnsuitableForShield != null;

        private string? _unsuitableForBow;
        [XmlAttribute("unsuitable_for_bow")]
        public string? UnsuitableForBow
        {
            get => _unsuitableForBow;
            set => _unsuitableForBow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForBow() => UnsuitableForBow != null;

        private string? _unsuitableForCrossbow;
        [XmlAttribute("unsuitable_for_crossbow")]
        public string? UnsuitableForCrossbow
        {
            get => _unsuitableForCrossbow;
            set => _unsuitableForCrossbow = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForCrossbow() => UnsuitableForCrossbow != null;

        private string? _unsuitableForTwoHanded;
        [XmlAttribute("unsuitable_for_two_handed")]
        public string? UnsuitableForTwoHanded
        {
            get => _unsuitableForTwoHanded;
            set => _unsuitableForTwoHanded = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForTwoHanded() => UnsuitableForTwoHanded != null;

        private string? _unsuitableForEmpty;
        [XmlAttribute("unsuitable_for_empty")]
        public string? UnsuitableForEmpty
        {
            get => _unsuitableForEmpty;
            set => _unsuitableForEmpty = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForEmpty() => UnsuitableForEmpty != null;

        private string? _unsuitableForOneHanded;
        [XmlAttribute("unsuitable_for_one_handed")]
        public string? UnsuitableForOneHanded
        {
            get => _unsuitableForOneHanded;
            set => _unsuitableForOneHanded = value != null ? value.ToLower() : null;
        }
        public bool ShouldSerializeUnsuitableForOneHanded() => UnsuitableForOneHanded != null;

        [XmlAttribute("action")]
        public string Action { get; set; } = string.Empty;
        public bool ShouldSerializeAction() => !string.IsNullOrEmpty(Action);
    }
}