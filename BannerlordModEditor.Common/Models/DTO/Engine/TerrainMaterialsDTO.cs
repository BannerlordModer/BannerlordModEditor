using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Engine;

/// <summary>
/// 地形材质系统的数据传输对象
/// 专用于XML序列化和反序列化
/// </summary>
[XmlRoot("terrain_materials")]
public class TerrainMaterialsDTO
{
    [XmlElement("terrain_material")]
    public List<TerrainMaterialDTO> TerrainMaterialList { get; set; } = new List<TerrainMaterialDTO>();

    public bool ShouldSerializeTerrainMaterialList() => TerrainMaterialList != null && TerrainMaterialList.Count > 0;
}

/// <summary>
/// 单个地形材质的数据传输对象
/// 纯数据结构，不包含业务逻辑
/// </summary>
public class TerrainMaterialDTO
{
    [XmlAttribute("is_enabled")]
    public string IsEnabled { get; set; } = "true";

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("is_flora_layer")]
    public string IsFloraLayer { get; set; } = "false";

    [XmlAttribute("is_mesh_blend_layer")]
    public string IsMeshBlendLayer { get; set; } = "false";

    [XmlAttribute("pitch_roll_yaw")]
    public string PitchRollYaw { get; set; } = "0.000, 0.000, 0.000";

    [XmlAttribute("scale")]
    public string Scale { get; set; } = "5.000, 5.000";

    [XmlAttribute("shear")]
    public string Shear { get; set; } = "0.000, 0.000";

    [XmlAttribute("position_offset")]
    public string PositionOffset { get; set; } = "0.000, 0.000";

    [XmlAttribute("physics_material")]
    public string PhysicsMaterial { get; set; } = string.Empty;

    // 可选属性 - 只在某些材质中出现
    [XmlAttribute("detail_level_adjustment")]
    public string? DetailLevelAdjustment { get; set; }

    [XmlAttribute("elevation_amount")]
    public string ElevationAmount { get; set; } = "0.000";

    [XmlAttribute("parallax_amount")]
    public string ParallaxAmount { get; set; } = "0.000";

    [XmlAttribute("ground_slope_scale")]
    public string? GroundSlopeScale { get; set; }

    [XmlAttribute("bigdetailmap_mode")]
    public string BigDetailMapMode { get; set; } = "0";

    [XmlAttribute("bigdetailmap_weight")]
    public string BigDetailMapWeight { get; set; } = "0.000";

    [XmlAttribute("bigdetailmap_scale_x")]
    public string BigDetailMapScaleX { get; set; } = "0.080";

    [XmlAttribute("bigdetailmap_scale_y")]
    public string BigDetailMapScaleY { get; set; } = "0.080";

    [XmlAttribute("bigdetailmap_bias_x")]
    public string BigDetailMapBiasX { get; set; } = "0.080";

    [XmlAttribute("bigdetailmap_bias_y")]
    public string BigDetailMapBiasY { get; set; } = "0.080";

    // 可选属性 - 仅在特定材质中出现
    [XmlAttribute("albedo_factor_color")]
    public string? AlbedoFactorColor { get; set; }

    [XmlAttribute("albedo_factor_mode")]
    public string? AlbedoFactorMode { get; set; }

    [XmlAttribute("smooth_blend_amount")]
    public string? SmoothBlendAmount { get; set; }

    [XmlElement("textures")]
    public TexturesContainerDTO? Textures { get; set; }

    [XmlElement("layer_flags")]
    public LayerFlagsContainerDTO? LayerFlags { get; set; }

    [XmlElement("meshes")]
    public TerrainMeshesContainerDTO? Meshes { get; set; }

    // 序列化控制方法
    public bool ShouldSerializeDetailLevelAdjustment() => !string.IsNullOrEmpty(DetailLevelAdjustment);
    public bool ShouldSerializeGroundSlopeScale() => !string.IsNullOrEmpty(GroundSlopeScale);
    public bool ShouldSerializeAlbedoFactorColor() => !string.IsNullOrEmpty(AlbedoFactorColor);
    public bool ShouldSerializeAlbedoFactorMode() => !string.IsNullOrEmpty(AlbedoFactorMode);
    public bool ShouldSerializeSmoothBlendAmount() => !string.IsNullOrEmpty(SmoothBlendAmount);
    public bool ShouldSerializeTextures() => Textures != null && Textures.TextureList.Count > 0;
    public bool ShouldSerializeLayerFlags() => LayerFlags != null && LayerFlags.FlagList.Count > 0;
    public bool ShouldSerializeMeshes() => Meshes != null && Meshes.MeshList.Count > 0;
}

/// <summary>
/// 纹理容器数据传输对象
/// </summary>
public class TexturesContainerDTO
{
    [XmlElement("texture")]
    public List<TerrainTextureDTO> TextureList { get; set; } = new List<TerrainTextureDTO>();

    public bool ShouldSerializeTextureList() => TextureList != null && TextureList.Count > 0;
}

/// <summary>
/// 单个地形纹理数据传输对象
/// </summary>
public class TerrainTextureDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}

/// <summary>
/// 层标志容器数据传输对象
/// </summary>
public class LayerFlagsContainerDTO
{
    [XmlElement("flag")]
    public List<LayerFlagDTO> FlagList { get; set; } = new List<LayerFlagDTO>();

    public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
}

/// <summary>
/// 单个层标志数据传输对象
/// </summary>
public class LayerFlagDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = "false";

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

/// <summary>
/// 地形网格容器数据传输对象
/// </summary>
public class TerrainMeshesContainerDTO
{
    [XmlElement("mesh")]
    public List<TerrainMeshDTO> MeshList { get; set; } = new List<TerrainMeshDTO>();

    public bool ShouldSerializeMeshList() => MeshList != null && MeshList.Count > 0;
}

/// <summary>
/// 单个地形网格数据传输对象
/// </summary>
public class TerrainMeshDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("density")]
    public string Density { get; set; } = "0";

    [XmlAttribute("seed_index")]
    public string SeedIndex { get; set; } = "0";

    [XmlAttribute("colony_radius")]
    public string ColonyRadius { get; set; } = "0.000";

    [XmlAttribute("colony_threshold")]
    public string ColonyThreshold { get; set; } = "0.000";

    [XmlAttribute("size_min")]
    public string SizeMin { get; set; } = "1.000, 1.000, 1.000";

    [XmlAttribute("size_max")]
    public string SizeMax { get; set; } = "1.000, 1.000, 1.000";

    [XmlAttribute("albedo_multiplier")]
    public string AlbedoMultiplier { get; set; } = "1.000, 1.000, 1.000";

    [XmlAttribute("weight_offset")]
    public string WeightOffset { get; set; } = "0.500";

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeDensity() => !string.IsNullOrEmpty(Density);
    public bool ShouldSerializeSeedIndex() => !string.IsNullOrEmpty(SeedIndex);
    public bool ShouldSerializeColonyRadius() => !string.IsNullOrEmpty(ColonyRadius);
    public bool ShouldSerializeColonyThreshold() => !string.IsNullOrEmpty(ColonyThreshold);
    public bool ShouldSerializeSizeMin() => !string.IsNullOrEmpty(SizeMin);
    public bool ShouldSerializeSizeMax() => !string.IsNullOrEmpty(SizeMax);
    public bool ShouldSerializeAlbedoMultiplier() => !string.IsNullOrEmpty(AlbedoMultiplier);
    public bool ShouldSerializeWeightOffset() => !string.IsNullOrEmpty(WeightOffset);
}