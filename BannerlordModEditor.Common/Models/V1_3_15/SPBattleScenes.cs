using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("SPBattleScenes")]
public class SPBattleScenes
{
    [XmlElement("Scene")]
    public List<SPBattleScene> SceneList { get; set; } = new List<SPBattleScene>();
}

public class SPBattleScene
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("terrain")]
    public string? Terrain { get; set; }

    [XmlAttribute("forest_density")]
    public string? ForestDensity { get; set; }

    [XmlAttribute("map_indices")]
    public string? MapIndices { get; set; }

    [XmlElement("TerrainTypes")]
    public TerrainTypes? TerrainTypes { get; set; }

    public bool ShouldSerializeTerrain() => !string.IsNullOrEmpty(Terrain);
    public bool ShouldSerializeForestDensity() => !string.IsNullOrEmpty(ForestDensity);
    public bool ShouldSerializeMapIndices() => !string.IsNullOrEmpty(MapIndices);
    public bool ShouldSerializeTerrainTypes() => TerrainTypes != null && TerrainTypes.TerrainTypeList.Count > 0;
}

public class TerrainTypes
{
    [XmlElement("TerrainType")]
    public List<TerrainType> TerrainTypeList { get; set; } = new List<TerrainType>();

    public bool ShouldSerializeTerrainTypeList() => TerrainTypeList.Count > 0;
}

public class TerrainType
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}
