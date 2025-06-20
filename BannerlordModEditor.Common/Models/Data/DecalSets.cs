using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// decal_sets.xml - Decal rendering configuration sets
[XmlRoot("base")]
public class DecalSets
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("decal_sets")]
    public DecalSetsContainer? DecalSetsContainer { get; set; }
}

public class DecalSetsContainer
{
    [XmlElement("decal_set")]
    public List<DecalSet> DecalSet { get; set; } = new();
}

public class DecalSet
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("total_decal_life_base")]
    public string? TotalDecalLifeBase { get; set; }

    [XmlAttribute("visible_decal_life_base")]
    public string? VisibleDecalLifeBase { get; set; }

    [XmlAttribute("maximum_decal_count_per_grid")]
    public string? MaximumDecalCountPerGrid { get; set; }

    [XmlAttribute("min_visibility_area")]
    public string? MinVisibilityArea { get; set; }

    [XmlAttribute("adaptive_time_limit")]
    public string? AdaptiveTimeLimit { get; set; }

    [XmlAttribute("fade_out_delete")]
    public string? FadeOutDelete { get; set; }
} 