using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class DecalSetsRoot
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("decal_sets")]
        public DecalSets DecalSets { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDecalSets() => DecalSets != null;
    }

    public class DecalSets
    {
        [XmlElement("decal_set")]
        public List<DecalSet> Items { get; set; }

        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class DecalSet
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("total_decal_life_base")]
        public string TotalDecalLifeBase { get; set; }

        [XmlAttribute("visible_decal_life_base")]
        public string VisibleDecalLifeBase { get; set; }

        [XmlAttribute("maximum_decal_count_per_grid")]
        public int MaximumDecalCountPerGrid { get; set; }

        [XmlAttribute("min_visibility_area")]
        public string MinVisibilityArea { get; set; }

        [XmlAttribute("adaptive_time_limit")]
        public bool AdaptiveTimeLimit { get; set; }

        [XmlAttribute("fade_out_delete")]
        public bool FadeOutDelete { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTotalDecalLifeBase() => !string.IsNullOrEmpty(TotalDecalLifeBase);
        public bool ShouldSerializeVisibleDecalLifeBase() => !string.IsNullOrEmpty(VisibleDecalLifeBase);
        public bool ShouldSerializeMaximumDecalCountPerGrid() => true;
        public bool ShouldSerializeMinVisibilityArea() => !string.IsNullOrEmpty(MinVisibilityArea);
        public bool ShouldSerializeAdaptiveTimeLimit() => true;
        public bool ShouldSerializeFadeOutDelete() => true;
    }
}