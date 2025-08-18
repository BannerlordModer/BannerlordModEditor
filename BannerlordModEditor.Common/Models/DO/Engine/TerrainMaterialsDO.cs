using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Engine;

/// <summary>
/// 地形材质系统的领域对象
/// 用于terrain_materials.xml文件的完整处理
/// </summary>
[XmlRoot("terrain_materials")]
public class TerrainMaterialsDO
{
    [XmlElement("terrain_material")]
    public List<TerrainMaterialDO> TerrainMaterialList { get; set; } = new List<TerrainMaterialDO>();

    // 性能优化：大型文件处理标记
    [XmlIgnore]
    public bool IsLargeFile { get; set; } = false;
    
    [XmlIgnore]
    public int ProcessedChunks { get; set; } = 0;

    public bool ShouldSerializeTerrainMaterialList() => TerrainMaterialList != null && TerrainMaterialList.Count > 0;
}

/// <summary>
/// 单个地形材质的领域对象
/// 包含完整的地形材质属性和业务逻辑
/// </summary>
public class TerrainMaterialDO
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
    public TexturesContainerDO? Textures { get; set; }

    [XmlElement("layer_flags")]
    public LayerFlagsContainerDO? LayerFlags { get; set; }

    [XmlElement("meshes")]
    public TerrainMeshesContainerDO? Meshes { get; set; }

    // 运行时标记
    [XmlIgnore]
    public bool HasTextures { get; set; } = false;

    [XmlIgnore]
    public bool HasLayerFlags { get; set; } = false;

    [XmlIgnore]
    public bool HasMeshes { get; set; } = false;

    [XmlIgnore]
    public bool HasEmptyMeshes { get; set; } = false;

    // 业务逻辑方法
    public bool IsFloraMaterial() => IsFloraLayer == "true";
    
    public bool IsMeshBlendMaterial() => IsMeshBlendLayer == "true";
    
    public bool HasDetailMaps() => BigDetailMapMode != "0";

    // 序列化控制方法
    public bool ShouldSerializeDetailLevelAdjustment() => !string.IsNullOrEmpty(DetailLevelAdjustment);
    public bool ShouldSerializeGroundSlopeScale() => !string.IsNullOrEmpty(GroundSlopeScale);
    public bool ShouldSerializeAlbedoFactorColor() => !string.IsNullOrEmpty(AlbedoFactorColor);
    public bool ShouldSerializeAlbedoFactorMode() => !string.IsNullOrEmpty(AlbedoFactorMode);
    public bool ShouldSerializeSmoothBlendAmount() => !string.IsNullOrEmpty(SmoothBlendAmount);
    public bool ShouldSerializeTextures() => HasTextures && Textures != null && Textures.TextureList.Count > 0;
    public bool ShouldSerializeLayerFlags() => HasLayerFlags && LayerFlags != null && LayerFlags.FlagList.Count > 0;
    public bool ShouldSerializeMeshes() => HasMeshes && Meshes != null && (Meshes.MeshList.Count > 0 || HasEmptyMeshes);
}

/// <summary>
/// 纹理容器
/// </summary>
public class TexturesContainerDO
{
    [XmlElement("texture")]
    public List<TerrainTextureDO> TextureList { get; set; } = new List<TerrainTextureDO>();

    public bool ShouldSerializeTextureList() => TextureList != null && TextureList.Count > 0;
}

/// <summary>
/// 单个地形纹理
/// </summary>
public class TerrainTextureDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    // 纹理类型常量
    public const string DiffuseMapType = "diffusemap";
    public const string AreaMapType = "areamap";
    public const string NormalMapType = "normalmap";
    public const string SpecularMapType = "specularmap";
    public const string SplattingMapType = "splattingmap";

    // 业务逻辑方法
    public bool IsRequiredTexture() => Type switch
    {
        DiffuseMapType => true,
        AreaMapType => true,
        NormalMapType => true,
        SpecularMapType => true,
        SplattingMapType => true,
        _ => false
    };

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}

/// <summary>
/// 层标志容器
/// </summary>
public class LayerFlagsContainerDO
{
    [XmlElement("flag")]
    public List<LayerFlagDO> FlagList { get; set; } = new List<LayerFlagDO>();

    public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
}

/// <summary>
/// 单个层标志
/// </summary>
public class LayerFlagDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = "false";

    // 标志常量
    public const string UseParallaxFlag = "use_parallax";
    public const string UseDisplacementMappingFlag = "use_displacement_mapping";
    public const string UseRandomizedNormalMapFlag = "use_randomized_normalmap";
    public const string UseTransparencyOfDiffuseAlphaFlag = "use_transparency_of_diffuse_alpha";

    // 业务逻辑方法
    public bool IsEnabled() => Value.ToLower() == "true";

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

/// <summary>
/// 地形网格容器
/// </summary>
public class TerrainMeshesContainerDO
{
    [XmlElement("mesh")]
    public List<TerrainMeshDO> MeshList { get; set; } = new List<TerrainMeshDO>();

    public bool ShouldSerializeMeshList() => MeshList != null && MeshList.Count > 0;
}

/// <summary>
/// 单个地形网格
/// </summary>
public class TerrainMeshDO
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

    // 业务逻辑方法
    public bool IsFloraMesh() => Name.Contains("flora");
    
    public bool IsRockMesh() => Name.Contains("rock");
    
    public bool IsTreeMesh() => Name.Contains("tree");

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