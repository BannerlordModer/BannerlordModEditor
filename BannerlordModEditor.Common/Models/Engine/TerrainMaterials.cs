using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("terrain_materials")]
    public class TerrainMaterials
    {
        [XmlElement("terrain_material")]
        public List<TerrainMaterial> TerrainMaterialList { get; set; } = new List<TerrainMaterial>();
    }

    public class TerrainMaterial
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
        public TexturesContainer? Textures { get; set; }

        [XmlElement("layer_flags")]
        public LayerFlagsContainer? LayerFlags { get; set; }

        [XmlElement("meshes")]
        public TerrainMeshesContainer? Meshes { get; set; }

        // 为了确保null值不会序列化，我们需要实现ShouldSerialize方法
        public bool ShouldSerializeDetailLevelAdjustment() => DetailLevelAdjustment != null;
        public bool ShouldSerializeGroundSlopeScale() => GroundSlopeScale != null;
        public bool ShouldSerializeAlbedoFactorColor() => AlbedoFactorColor != null;
        public bool ShouldSerializeAlbedoFactorMode() => AlbedoFactorMode != null;
        public bool ShouldSerializeSmoothBlendAmount() => SmoothBlendAmount != null;
    }

    public class TexturesContainer
    {
        [XmlElement("texture")]
        public List<TerrainTexture> Texture { get; set; } = new List<TerrainTexture>();
    }

    public class TerrainTexture
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class LayerFlagsContainer
    {
        [XmlElement("flag")]
        public List<LayerFlag> Flag { get; set; } = new List<LayerFlag>();
    }

    public class LayerFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = "false";
    }

    public class TerrainMeshesContainer
    {
        [XmlElement("mesh")]
        public List<TerrainMesh> Mesh { get; set; } = new List<TerrainMesh>();
    }

    public class TerrainMesh
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
    }
} 