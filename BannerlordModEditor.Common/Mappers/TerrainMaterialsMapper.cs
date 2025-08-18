using System.Linq;
using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DTO.Engine;

namespace BannerlordModEditor.Common.Mappers;

/// <summary>
/// TerrainMaterials的DO/DTO映射器
/// 处理地形材质系统的领域对象和数据传输对象之间的双向转换
/// </summary>
public static class TerrainMaterialsMapper
{
    /// <summary>
    /// 将DO转换为DTO
    /// </summary>
    public static TerrainMaterialsDTO ToDTO(TerrainMaterialsDO source)
    {
        if (source == null) return null;

        return new TerrainMaterialsDTO
        {
            TerrainMaterialList = source.TerrainMaterialList?
                .Select(ToDTO)
                .ToList() ?? new List<TerrainMaterialDTO>()
        };
    }

    /// <summary>
    /// 将DTO转换为DO
    /// </summary>
    public static TerrainMaterialsDO ToDO(TerrainMaterialsDTO source)
    {
        if (source == null) return null;

        return new TerrainMaterialsDO
        {
            TerrainMaterialList = source.TerrainMaterialList?
                .Select(ToDo)
                .ToList() ?? new List<TerrainMaterialDO>(),
            IsLargeFile = false, // 需要在反序列化后设置
            ProcessedChunks = 0
        };
    }

    /// <summary>
    /// 将单个TerrainMaterialDO转换为DTO
    /// </summary>
    public static TerrainMaterialDTO ToDTO(TerrainMaterialDO source)
    {
        if (source == null) return null;

        return new TerrainMaterialDTO
        {
            IsEnabled = source.IsEnabled,
            Name = source.Name,
            IsFloraLayer = source.IsFloraLayer,
            IsMeshBlendLayer = source.IsMeshBlendLayer,
            PitchRollYaw = source.PitchRollYaw,
            Scale = source.Scale,
            Shear = source.Shear,
            PositionOffset = source.PositionOffset,
            PhysicsMaterial = source.PhysicsMaterial,
            DetailLevelAdjustment = source.DetailLevelAdjustment,
            ElevationAmount = source.ElevationAmount,
            ParallaxAmount = source.ParallaxAmount,
            GroundSlopeScale = source.GroundSlopeScale,
            BigDetailMapMode = source.BigDetailMapMode,
            BigDetailMapWeight = source.BigDetailMapWeight,
            BigDetailMapScaleX = source.BigDetailMapScaleX,
            BigDetailMapScaleY = source.BigDetailMapScaleY,
            BigDetailMapBiasX = source.BigDetailMapBiasX,
            BigDetailMapBiasY = source.BigDetailMapBiasY,
            AlbedoFactorColor = source.AlbedoFactorColor,
            AlbedoFactorMode = source.AlbedoFactorMode,
            SmoothBlendAmount = source.SmoothBlendAmount,
            Textures = ToDTO(source.Textures),
            LayerFlags = ToDTO(source.LayerFlags),
            Meshes = ToDTO(source.Meshes)
        };
    }

    /// <summary>
    /// 将单个TerrainMaterialDTO转换为DO
    /// </summary>
    public static TerrainMaterialDO ToDo(TerrainMaterialDTO source)
    {
        if (source == null) return null;

        return new TerrainMaterialDO
        {
            IsEnabled = source.IsEnabled,
            Name = source.Name,
            IsFloraLayer = source.IsFloraLayer,
            IsMeshBlendLayer = source.IsMeshBlendLayer,
            PitchRollYaw = source.PitchRollYaw,
            Scale = source.Scale,
            Shear = source.Shear,
            PositionOffset = source.PositionOffset,
            PhysicsMaterial = source.PhysicsMaterial,
            DetailLevelAdjustment = source.DetailLevelAdjustment,
            ElevationAmount = source.ElevationAmount,
            ParallaxAmount = source.ParallaxAmount,
            GroundSlopeScale = source.GroundSlopeScale,
            BigDetailMapMode = source.BigDetailMapMode,
            BigDetailMapWeight = source.BigDetailMapWeight,
            BigDetailMapScaleX = source.BigDetailMapScaleX,
            BigDetailMapScaleY = source.BigDetailMapScaleY,
            BigDetailMapBiasX = source.BigDetailMapBiasX,
            BigDetailMapBiasY = source.BigDetailMapBiasY,
            AlbedoFactorColor = source.AlbedoFactorColor,
            AlbedoFactorMode = source.AlbedoFactorMode,
            SmoothBlendAmount = source.SmoothBlendAmount,
            Textures = ToDo(source.Textures),
            LayerFlags = ToDo(source.LayerFlags),
            Meshes = ToDo(source.Meshes),
            HasTextures = source.Textures != null && source.Textures.TextureList.Count > 0,
            HasLayerFlags = source.LayerFlags != null && source.LayerFlags.FlagList.Count > 0,
            HasMeshes = source.Meshes != null && source.Meshes.MeshList.Count > 0,
            HasEmptyMeshes = source.Meshes != null && source.Meshes.MeshList.Count == 0
        };
    }

    /// <summary>
    /// 将TexturesContainerDO转换为DTO
    /// </summary>
    public static TexturesContainerDTO ToDTO(TexturesContainerDO source)
    {
        if (source == null) return null;

        return new TexturesContainerDTO
        {
            TextureList = source.TextureList?
                .Select(ToDTO)
                .ToList() ?? new List<TerrainTextureDTO>()
        };
    }

    /// <summary>
    /// 将TexturesContainerDTO转换为DO
    /// </summary>
    public static TexturesContainerDO ToDo(TexturesContainerDTO source)
    {
        if (source == null) return null;

        return new TexturesContainerDO
        {
            TextureList = source.TextureList?
                .Select(ToDo)
                .ToList() ?? new List<TerrainTextureDO>()
        };
    }

    /// <summary>
    /// 将单个TerrainTextureDO转换为DTO
    /// </summary>
    public static TerrainTextureDTO ToDTO(TerrainTextureDO source)
    {
        if (source == null) return null;

        return new TerrainTextureDTO
        {
            Type = source.Type,
            Name = source.Name
        };
    }

    /// <summary>
    /// 将单个TerrainTextureDTO转换为DO
    /// </summary>
    public static TerrainTextureDO ToDo(TerrainTextureDTO source)
    {
        if (source == null) return null;

        return new TerrainTextureDO
        {
            Type = source.Type,
            Name = source.Name
        };
    }

    /// <summary>
    /// 将LayerFlagsContainerDO转换为DTO
    /// </summary>
    public static LayerFlagsContainerDTO ToDTO(LayerFlagsContainerDO source)
    {
        if (source == null) return null;

        return new LayerFlagsContainerDTO
        {
            FlagList = source.FlagList?
                .Select(ToDTO)
                .ToList() ?? new List<LayerFlagDTO>()
        };
    }

    /// <summary>
    /// 将LayerFlagsContainerDTO转换为DO
    /// </summary>
    public static LayerFlagsContainerDO ToDo(LayerFlagsContainerDTO source)
    {
        if (source == null) return null;

        return new LayerFlagsContainerDO
        {
            FlagList = source.FlagList?
                .Select(ToDo)
                .ToList() ?? new List<LayerFlagDO>()
        };
    }

    /// <summary>
    /// 将单个LayerFlagDO转换为DTO
    /// </summary>
    public static LayerFlagDTO ToDTO(LayerFlagDO source)
    {
        if (source == null) return null;

        return new LayerFlagDTO
        {
            Name = source.Name,
            Value = source.Value
        };
    }

    /// <summary>
    /// 将单个LayerFlagDTO转换为DO
    /// </summary>
    public static LayerFlagDO ToDo(LayerFlagDTO source)
    {
        if (source == null) return null;

        return new LayerFlagDO
        {
            Name = source.Name,
            Value = source.Value
        };
    }

    /// <summary>
    /// 将TerrainMeshesContainerDO转换为DTO
    /// </summary>
    public static TerrainMeshesContainerDTO ToDTO(TerrainMeshesContainerDO source)
    {
        if (source == null) return null;

        return new TerrainMeshesContainerDTO
        {
            MeshList = source.MeshList?
                .Select(ToDTO)
                .ToList() ?? new List<TerrainMeshDTO>()
        };
    }

    /// <summary>
    /// 将TerrainMeshesContainerDTO转换为DO
    /// </summary>
    public static TerrainMeshesContainerDO ToDo(TerrainMeshesContainerDTO source)
    {
        if (source == null) return null;

        return new TerrainMeshesContainerDO
        {
            MeshList = source.MeshList?
                .Select(ToDo)
                .ToList() ?? new List<TerrainMeshDO>()
        };
    }

    /// <summary>
    /// 将单个TerrainMeshDO转换为DTO
    /// </summary>
    public static TerrainMeshDTO ToDTO(TerrainMeshDO source)
    {
        if (source == null) return null;

        return new TerrainMeshDTO
        {
            Name = source.Name,
            Density = source.Density,
            SeedIndex = source.SeedIndex,
            ColonyRadius = source.ColonyRadius,
            ColonyThreshold = source.ColonyThreshold,
            SizeMin = source.SizeMin,
            SizeMax = source.SizeMax,
            AlbedoMultiplier = source.AlbedoMultiplier,
            WeightOffset = source.WeightOffset
        };
    }

    /// <summary>
    /// 将单个TerrainMeshDTO转换为DO
    /// </summary>
    public static TerrainMeshDO ToDo(TerrainMeshDTO source)
    {
        if (source == null) return null;

        return new TerrainMeshDO
        {
            Name = source.Name,
            Density = source.Density,
            SeedIndex = source.SeedIndex,
            ColonyRadius = source.ColonyRadius,
            ColonyThreshold = source.ColonyThreshold,
            SizeMin = source.SizeMin,
            SizeMax = source.SizeMax,
            AlbedoMultiplier = source.AlbedoMultiplier,
            WeightOffset = source.WeightOffset
        };
    }
}