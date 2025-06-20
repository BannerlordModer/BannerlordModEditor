using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// random_terrain_templates.xml - Random terrain generation templates
[XmlRoot("templates")]
public class RandomTerrainTemplates
{
    [XmlElement("template")]
    public List<TerrainTemplate> Template { get; set; } = new();
}

public class TerrainTemplate
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("mountain_layer_enabled")]
    public string? MountainLayerEnabled { get; set; }

    [XmlAttribute("mountain_layer_octaves")]
    public string? MountainLayerOctaves { get; set; }

    [XmlAttribute("mountain_layer_lacunarity")]
    public string? MountainLayerLacunarity { get; set; }

    [XmlAttribute("main_layer_octaves")]
    public string? MainLayerOctaves { get; set; }

    [XmlAttribute("main_layer_scale")]
    public string? MainLayerScale { get; set; }

    [XmlAttribute("main_layer_bias")]
    public string? MainLayerBias { get; set; }

    [XmlAttribute("main_layer_frequency")]
    public string? MainLayerFrequency { get; set; }

    [XmlAttribute("main_layer_lacunarity")]
    public string? MainLayerLacunarity { get; set; }

    [XmlAttribute("main_layer_persistance")]
    public string? MainLayerPersistance { get; set; }

    [XmlAttribute("smoothness")]
    public string? Smoothness { get; set; }

    [XmlAttribute("turbulance_enabled")]
    public string? TurbulanceEnabled { get; set; }

    [XmlAttribute("turbulance_frequency")]
    public string? TurbulanceFrequency { get; set; }

    [XmlAttribute("turbulance_power")]
    public string? TurbulancePower { get; set; }

    [XmlAttribute("turbulance_roughness")]
    public string? TurbulanceRoughness { get; set; }

    [XmlAttribute("height_level")]
    public string? HeightLevel { get; set; }

    [XmlAttribute("biome")]
    public string? Biome { get; set; }

    [XmlAttribute("flora_density")]
    public string? FloraDensity { get; set; }

    [XmlAttribute("min_height")]
    public string? MinHeight { get; set; }

    [XmlAttribute("max_height")]
    public string? MaxHeight { get; set; }

    [XmlAttribute("scale")]
    public string? Scale { get; set; }
} 