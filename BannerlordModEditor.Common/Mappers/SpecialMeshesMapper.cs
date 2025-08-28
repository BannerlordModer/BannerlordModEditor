using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 特殊网格映射器
    /// </summary>
    public static class SpecialMeshesMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SpecialMeshesDTO ToDTO(SpecialMeshesDO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshesDTO
            {
                Type = source.Type,
                Meshes = MeshesMapper.ToDTO(source.Meshes)
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SpecialMeshesDO ToDO(SpecialMeshesDTO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshesDO
            {
                Type = source.Type,
                Meshes = MeshesMapper.ToDO(source.Meshes)
            };
        }
    }

    /// <summary>
    /// 网格集合映射器
    /// </summary>
    public static class MeshesMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static MeshesDTO ToDTO(MeshesDO source)
        {
            if (source == null) return null;
            
            return new MeshesDTO
            {
                MeshList = source.MeshList?
                    .Select(SpecialMeshMapper.ToDTO)
                    .ToList() ?? new List<SpecialMeshDTO>()
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static MeshesDO ToDO(MeshesDTO source)
        {
            if (source == null) return null;
            
            return new MeshesDO
            {
                MeshList = source.MeshList?
                    .Select(SpecialMeshMapper.ToDO)
                    .ToList() ?? new List<SpecialMeshDO>()
            };
        }
    }

    /// <summary>
    /// 单个特殊网格映射器
    /// </summary>
    public static class SpecialMeshMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SpecialMeshDTO ToDTO(SpecialMeshDO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshDTO
            {
                Name = source.Name,
                Types = SpecialMeshTypesMapper.ToDTO(source.Types)
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SpecialMeshDO ToDO(SpecialMeshDTO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshDO
            {
                Name = source.Name,
                Types = SpecialMeshTypesMapper.ToDO(source.Types)
            };
        }
    }

    /// <summary>
    /// 特殊网格类型集合映射器
    /// </summary>
    public static class SpecialMeshTypesMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SpecialMeshTypesDTO ToDTO(SpecialMeshTypesDO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshTypesDTO
            {
                TypeList = source.TypeList?
                    .Select(SpecialMeshTypeMapper.ToDTO)
                    .ToList() ?? new List<SpecialMeshTypeDTO>()
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SpecialMeshTypesDO ToDO(SpecialMeshTypesDTO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshTypesDO
            {
                TypeList = source.TypeList?
                    .Select(SpecialMeshTypeMapper.ToDO)
                    .ToList() ?? new List<SpecialMeshTypeDO>()
            };
        }
    }

    /// <summary>
    /// 单个特殊网格类型映射器
    /// </summary>
    public static class SpecialMeshTypeMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SpecialMeshTypeDTO ToDTO(SpecialMeshTypeDO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshTypeDTO
            {
                Name = source.Name
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SpecialMeshTypeDO ToDO(SpecialMeshTypeDTO source)
        {
            if (source == null) return null;
            
            return new SpecialMeshTypeDO
            {
                Name = source.Name
            };
        }
    }
}