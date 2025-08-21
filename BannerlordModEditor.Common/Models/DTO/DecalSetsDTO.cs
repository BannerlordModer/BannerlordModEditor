using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

[XmlRoot("base")]
public class DecalSetsDTO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("decal_sets")]
    public DecalSetsContainerDTO? DecalSets { get; set; } = new DecalSetsContainerDTO();

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeDecalSets() => DecalSets != null && DecalSets.Items.Count > 0;
}

public class DecalSetsContainerDTO
{
    [XmlElement("decal_set")]
    public List<DecalSetDTO> Items { get; set; } = new List<DecalSetDTO>();
}

public class DecalSetDTO
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

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeTotalDecalLifeBase() => !string.IsNullOrEmpty(TotalDecalLifeBase);
    public bool ShouldSerializeVisibleDecalLifeBase() => !string.IsNullOrEmpty(VisibleDecalLifeBase);
    public bool ShouldSerializeMaximumDecalCountPerGrid() => !string.IsNullOrEmpty(MaximumDecalCountPerGrid);
    public bool ShouldSerializeMinVisibilityArea() => !string.IsNullOrEmpty(MinVisibilityArea);
    public bool ShouldSerializeAdaptiveTimeLimit() => !string.IsNullOrEmpty(AdaptiveTimeLimit);
    public bool ShouldSerializeFadeOutDelete() => !string.IsNullOrEmpty(FadeOutDelete);
}