using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// 地图图标粒子系统配置 - particle_systems_map_icon.xml
    /// 定义地图图标相关的粒子特效系统
    /// </summary>
    [XmlRoot("particle_effects")]
    public class ParticleSystemsMapIcon
    {
        /// <summary>
        /// 粒子特效列表
        /// </summary>
        [XmlElement("effect")]
        public List<ParticleEffect> Effects { get; set; } = new List<ParticleEffect>();
    }

    /// <summary>
    /// 单个粒子特效定义
    /// </summary>
    public class ParticleEffect
    {
        /// <summary>
        /// 特效名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 特效的全局唯一标识符
        /// </summary>
        [XmlAttribute("guid")]
        public string? Guid { get; set; }

        /// <summary>
        /// 发射器列表
        /// </summary>
        [XmlArray("emitters")]
        [XmlArrayItem("emitter")]
        public List<ParticleEmitter> Emitters { get; set; } = new List<ParticleEmitter>();

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
    }

    /// <summary>
    /// 粒子发射器定义
    /// </summary>
    public class ParticleEmitter
    {
        /// <summary>
        /// 发射器名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 发射器索引
        /// </summary>
        [XmlAttribute("_index_")]
        public string? Index { get; set; }

        /// <summary>
        /// 标志配置
        /// </summary>
        [XmlArray("flags")]
        [XmlArrayItem("flag")]
        public List<ParticleFlag> Flags { get; set; } = new List<ParticleFlag>();

        /// <summary>
        /// 参数配置
        /// </summary>
        [XmlArray("parameters")]
        [XmlArrayItem("parameter")]
        public List<ParticleParameter> Parameters { get; set; } = new List<ParticleParameter>();

        /// <summary>
        /// 曲线配置
        /// </summary>
        [XmlArray("curves")]
        [XmlArrayItem("curve")]
        public List<ParticleCurve>? Curves { get; set; }

        /// <summary>
        /// 材质配置
        /// </summary>
        [XmlElement("material")]
        public ParticleMaterial? Material { get; set; }

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeCurves() => Curves != null && Curves.Count > 0;
        public bool ShouldSerializeMaterial() => Material != null;
    }

    /// <summary>
    /// 粒子标志定义
    /// </summary>
    public class ParticleFlag
    {
        /// <summary>
        /// 标志名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 标志值
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    /// <summary>
    /// 粒子参数定义
    /// </summary>
    public class ParticleParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 参数值
        /// </summary>
        [XmlAttribute("value")]
        public string? Value { get; set; }

        /// <summary>
        /// 基础值
        /// </summary>
        [XmlAttribute("base")]
        public string? Base { get; set; }

        /// <summary>
        /// 偏移值
        /// </summary>
        [XmlAttribute("bias")]
        public string? Bias { get; set; }

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
    }

    /// <summary>
    /// 粒子曲线定义
    /// </summary>
    public class ParticleCurve
    {
        /// <summary>
        /// 曲线名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 曲线点列表
        /// </summary>
        [XmlArray("points")]
        [XmlArrayItem("point")]
        public List<CurvePoint>? Points { get; set; }

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializePoints() => Points != null && Points.Count > 0;
    }

    /// <summary>
    /// 曲线点定义
    /// </summary>
    public class CurvePoint
    {
        /// <summary>
        /// X 坐标
        /// </summary>
        [XmlAttribute("x")]
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Y 坐标
        /// </summary>
        [XmlAttribute("y")]
        public string Y { get; set; } = string.Empty;

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeX() => !string.IsNullOrEmpty(X);
        public bool ShouldSerializeY() => !string.IsNullOrEmpty(Y);
    }

    /// <summary>
    /// 粒子材质定义
    /// </summary>
    public class ParticleMaterial
    {
        /// <summary>
        /// 材质名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 材质贴图
        /// </summary>
        [XmlElement("texture")]
        public string? Texture { get; set; }

        // ShouldSerialize 方法控制可选属性的序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTexture() => !string.IsNullOrEmpty(Texture);
    }
} 