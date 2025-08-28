using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 特殊网格 - 数据传输对象
    /// </summary>
    [XmlRoot("base")]
    public class SpecialMeshesDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("meshes")]
        public MeshesDTO? Meshes { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeType() => Type != null;
        public bool ShouldSerializeMeshes() => Meshes != null;

        // 便捷属性
        public bool HasType => Type != null;
        public bool HasMeshes => Meshes != null;
        public int MeshCount => Meshes?.MeshList?.Count ?? 0;
    }

    /// <summary>
    /// 网格集合 - 数据传输对象
    /// </summary>
    public class MeshesDTO
    {
        [XmlElement("mesh")]
        public List<SpecialMeshDTO> MeshList { get; set; } = new List<SpecialMeshDTO>();

        // ShouldSerialize方法
        public bool ShouldSerializeMeshList() => MeshList != null && MeshList.Count > 0;

        // 便捷属性
        public int MeshCount => MeshList?.Count ?? 0;
    }

    /// <summary>
    /// 单个特殊网格 - 数据传输对象
    /// </summary>
    public class SpecialMeshDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("types")]
        public SpecialMeshTypesDTO? Types { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeTypes() => Types != null;

        // 便捷属性
        public bool HasName => Name != null;
        public bool HasTypes => Types != null;
        public int TypeCount => Types?.TypeList?.Count ?? 0;
    }

    /// <summary>
    /// 特殊网格类型集合 - 数据传输对象
    /// </summary>
    public class SpecialMeshTypesDTO
    {
        [XmlElement("type")]
        public List<SpecialMeshTypeDTO> TypeList { get; set; } = new List<SpecialMeshTypeDTO>();

        // ShouldSerialize方法
        public bool ShouldSerializeTypeList() => TypeList != null && TypeList.Count > 0;

        // 便捷属性
        public int TypeCount => TypeList?.Count ?? 0;
    }

    /// <summary>
    /// 单个特殊网格类型 - 数据传输对象
    /// </summary>
    public class SpecialMeshTypeDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeName() => Name != null;

        // 便捷属性
        public bool HasName => Name != null;
    }
}