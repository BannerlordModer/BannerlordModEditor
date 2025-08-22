using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class DecalSetsDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "decal_set";

        [XmlElement("decal_sets")]
        public DecalSetsContainerDO DecalSets { get; set; } = new DecalSetsContainerDO();

        [XmlIgnore]
        public bool HasDecalSets { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDecalSets() => HasDecalSets && DecalSets != null && DecalSets.Items.Count > 0;
    }

    public class DecalSetsContainerDO
    {
        [XmlElement("decal_set")]
        public List<DecalSetDO> Items { get; set; } = new List<DecalSetDO>();
    }

    public class DecalSetDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("total_decal_life_base")]
        public string TotalDecalLifeBase { get; set; } = string.Empty;

        [XmlAttribute("visible_decal_life_base")]
        public string VisibleDecalLifeBase { get; set; } = string.Empty;

        [XmlAttribute("maximum_decal_count_per_grid")]
        public string MaximumDecalCountPerGrid { get; set; } = string.Empty;

        [XmlAttribute("min_visibility_area")]
        public string MinVisibilityArea { get; set; } = string.Empty;

        [XmlAttribute("adaptive_time_limit")]
        public string AdaptiveTimeLimit { get; set; } = string.Empty;

        [XmlAttribute("fade_out_delete")]
        public string FadeOutDelete { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTotalDecalLifeBase() => !string.IsNullOrEmpty(TotalDecalLifeBase);
        public bool ShouldSerializeVisibleDecalLifeBase() => !string.IsNullOrEmpty(VisibleDecalLifeBase);
        public bool ShouldSerializeMaximumDecalCountPerGrid() => !string.IsNullOrEmpty(MaximumDecalCountPerGrid);
        public bool ShouldSerializeMinVisibilityArea() => !string.IsNullOrEmpty(MinVisibilityArea);
        public bool ShouldSerializeAdaptiveTimeLimit() => !string.IsNullOrEmpty(AdaptiveTimeLimit);
        public bool ShouldSerializeFadeOutDelete() => !string.IsNullOrEmpty(FadeOutDelete);
    }
}