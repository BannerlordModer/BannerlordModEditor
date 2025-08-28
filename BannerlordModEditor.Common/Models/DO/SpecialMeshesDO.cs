using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 特殊网格 - 领域对象
    /// </summary>
    [XmlRoot("base")]
    public class SpecialMeshesDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("meshes")]
        public MeshesDO? Meshes { get; set; }

        [XmlIgnore]
        public bool HasEmptyMeshes { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeMeshes() => Meshes != null && !HasEmptyMeshes;
    }

    /// <summary>
    /// 网格集合 - 领域对象
    /// </summary>
    public class MeshesDO
    {
        [XmlElement("mesh")]
        public List<SpecialMeshDO> MeshList { get; set; } = new List<SpecialMeshDO>();

        [XmlIgnore]
        public bool HasEmptyMeshList { get; set; } = false;

        public bool ShouldSerializeMeshList() => MeshList != null && MeshList.Count > 0 && !HasEmptyMeshList;
    }

    /// <summary>
    /// 单个特殊网格 - 领域对象
    /// </summary>
    public class SpecialMeshDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("types")]
        public SpecialMeshTypesDO? Types { get; set; }

        [XmlIgnore]
        public bool HasEmptyTypes { get; set; } = false;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTypes() => Types != null && !HasEmptyTypes;
    }

    /// <summary>
    /// 特殊网格类型集合 - 领域对象
    /// </summary>
    public class SpecialMeshTypesDO
    {
        [XmlElement("type")]
        public List<SpecialMeshTypeDO> TypeList { get; set; } = new List<SpecialMeshTypeDO>();

        [XmlIgnore]
        public bool HasEmptyTypeList { get; set; } = false;

        public bool ShouldSerializeTypeList() => TypeList != null && TypeList.Count > 0 && !HasEmptyTypeList;
    }

    /// <summary>
    /// 单个特殊网格类型 - 领域对象
    /// </summary>
    public class SpecialMeshTypeDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}