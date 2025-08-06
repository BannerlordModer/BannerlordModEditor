using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.Engine
{
    /// <summary>
    /// 优化的FloraKinds模型，展示复杂嵌套结构的字段存在性追踪
    /// 原本实现：使用简单的可空类型和ShouldSerialize方法
    /// 简化实现：使用XmlModelBase基类并提供完整的字段存在性追踪
    /// </summary>
    [XmlRoot("flora_kinds")]
    public class EnhancedFloraKinds : XmlModelBase
    {
        private List<EnhancedFloraKind> _floraKind = new();

        [XmlElement("flora_kind")]
        public List<EnhancedFloraKind> FloraKind
        {
            get => _floraKind;
            set
            {
                _floraKind = value ?? new List<EnhancedFloraKind>();
                MarkPropertyExists(nameof(FloraKind));
            }
        }

        public bool ShouldSerializeFloraKind() => PropertyExists(nameof(FloraKind));
    }

    /// <summary>
    /// 优化的FloraKind模型，展示复杂对象的字段存在性追踪
    /// </summary>
    public class EnhancedFloraKind : XmlModelBase
    {
        private string? _name;
        private string? _viewDistance;
        private EnhancedFloraFlags? _flags;
        private List<EnhancedSeasonalKind> _seasonalKind = new();

        [XmlAttribute("name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkPropertyExists(nameof(Name));
            }
        }

        [XmlAttribute("view_distance")]
        public string? ViewDistance
        {
            get => _viewDistance;
            set
            {
                _viewDistance = value;
                MarkPropertyExists(nameof(ViewDistance));
            }
        }

        [XmlElement("flags")]
        public EnhancedFloraFlags? Flags
        {
            get => _flags;
            set
            {
                _flags = value;
                MarkPropertyExists(nameof(Flags));
            }
        }

        [XmlElement("seasonal_kind")]
        public List<EnhancedSeasonalKind> SeasonalKind
        {
            get => _seasonalKind;
            set
            {
                _seasonalKind = value ?? new List<EnhancedSeasonalKind>();
                MarkPropertyExists(nameof(SeasonalKind));
            }
        }

        public bool ShouldSerializeSeasonalKind() => PropertyExists(nameof(SeasonalKind)) && SeasonalKind?.Count > 0;

        /// <summary>
        /// 检查ViewDistance属性是否应该被序列化（存在且非空）
        /// </summary>
        public bool ShouldSerializeViewDistance() => PropertyExists(nameof(ViewDistance)) && !string.IsNullOrEmpty(ViewDistance);

        /// <summary>
        /// 检查Flags属性是否应该被序列化（存在且非空）
        /// </summary>
        public bool ShouldSerializeFlags() => PropertyExists(nameof(Flags)) && Flags?.Flag?.Count > 0;

        /// <summary>
        /// 检查Flags属性在XML中是否存在（即使为空）
        /// </summary>
        public bool FlagsExistsInXml => PropertyExists(nameof(Flags));

        /// <summary>
        /// 检查SeasonalKind属性在XML中是否存在（即使为空）
        /// </summary>
        public bool SeasonalKindExistsInXml => PropertyExists(nameof(SeasonalKind));

        /// <summary>
        /// 检查ViewDistance属性在XML中是否存在
        /// </summary>
        public bool ViewDistanceExistsInXml => PropertyExists(nameof(ViewDistance));
    }

    /// <summary>
    /// 优化的FloraFlags模型，展示集合类型的字段存在性追踪
    /// </summary>
    public class EnhancedFloraFlags : XmlModelBase
    {
        private List<EnhancedFloraFlag> _flag = new();

        [XmlElement("flag")]
        public List<EnhancedFloraFlag> Flag
        {
            get => _flag;
            set
            {
                _flag = value ?? new List<EnhancedFloraFlag>();
                MarkPropertyExists(nameof(Flag));
            }
        }

        public bool ShouldSerializeFlag() => PropertyExists(nameof(Flag));
    }

    /// <summary>
    /// 优化的FloraFlag模型，展示基础字段的字段存在性追踪
    /// </summary>
    public class EnhancedFloraFlag : XmlModelBase
    {
        private string? _name;
        private string? _value;

        [XmlAttribute("name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkPropertyExists(nameof(Name));
            }
        }

        [XmlAttribute("value")]
        public string? Value
        {
            get => _value;
            set
            {
                _value = value;
                MarkPropertyExists(nameof(Value));
            }
        }

        /// <summary>
        /// 检查Value属性是否存在于原始XML中
        /// 这对于区分空字符串和不存在的情况很重要
        /// </summary>
        public bool ValueExistsInXml => PropertyExists(nameof(Value));
    }

    /// <summary>
    /// 优化的SeasonalKind模型，展示嵌套结构的字段存在性追踪
    /// </summary>
    public class EnhancedSeasonalKind : XmlModelBase
    {
        private string? _season;
        private EnhancedFloraVariations? _floraVariations;

        [XmlAttribute("season")]
        public string? Season
        {
            get => _season;
            set
            {
                _season = value;
                MarkPropertyExists(nameof(Season));
            }
        }

        [XmlElement("flora_variations")]
        public EnhancedFloraVariations? FloraVariations
        {
            get => _floraVariations;
            set
            {
                _floraVariations = value;
                MarkPropertyExists(nameof(FloraVariations));
            }
        }

        /// <summary>
        /// 检查FloraVariations属性在XML中是否存在
        /// </summary>
        public bool FloraVariationsExistsInXml => PropertyExists(nameof(FloraVariations));
    }

    /// <summary>
    /// 优化的FloraVariations模型，展示复杂集合的字段存在性追踪
    /// </summary>
    public class EnhancedFloraVariations : XmlModelBase
    {
        private List<EnhancedFloraVariation> _floraVariation = new();

        [XmlElement("flora_variation")]
        public List<EnhancedFloraVariation> FloraVariation
        {
            get => _floraVariation;
            set
            {
                _floraVariation = value ?? new List<EnhancedFloraVariation>();
                MarkPropertyExists(nameof(FloraVariation));
            }
        }

        public bool ShouldSerializeFloraVariation() => PropertyExists(nameof(FloraVariation));
    }

    /// <summary>
    /// 优化的FloraVariation模型，展示复杂对象的字段存在性追踪
    /// </summary>
    public class EnhancedFloraVariation : XmlModelBase
    {
        private string? _name;
        private string? _bodyName;
        private string? _densityMultiplier;
        private string? _bbRadius;
        private List<EnhancedFloraMesh> _mesh = new();

        [XmlAttribute("name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkPropertyExists(nameof(Name));
            }
        }

        [XmlAttribute("body_name")]
        public string? BodyName
        {
            get => _bodyName;
            set
            {
                _bodyName = value;
                MarkPropertyExists(nameof(BodyName));
            }
        }

        [XmlAttribute("density_multiplier")]
        public string? DensityMultiplier
        {
            get => _densityMultiplier;
            set
            {
                _densityMultiplier = value;
                MarkPropertyExists(nameof(DensityMultiplier));
            }
        }

        [XmlAttribute("bb_radius")]
        public string? BbRadius
        {
            get => _bbRadius;
            set
            {
                _bbRadius = value;
                MarkPropertyExists(nameof(BbRadius));
            }
        }

        [XmlElement("mesh")]
        public List<EnhancedFloraMesh> Mesh
        {
            get => _mesh;
            set
            {
                _mesh = value ?? new List<EnhancedFloraMesh>();
                MarkPropertyExists(nameof(Mesh));
            }
        }

        public bool ShouldSerializeMesh() => PropertyExists(nameof(Mesh));

        /// <summary>
        /// 检查Mesh属性在XML中是否存在
        /// </summary>
        public bool MeshExistsInXml => PropertyExists(nameof(Mesh));

        /// <summary>
        /// 检查BodyName属性在XML中是否存在
        /// </summary>
        public bool BodyNameExistsInXml => PropertyExists(nameof(BodyName));

        /// <summary>
        /// 检查DensityMultiplier属性在XML中是否存在
        /// </summary>
        public bool DensityMultiplierExistsInXml => PropertyExists(nameof(DensityMultiplier));

        /// <summary>
        /// 检查BbRadius属性在XML中是否存在
        /// </summary>
        public bool BbRadiusExistsInXml => PropertyExists(nameof(BbRadius));
    }

    /// <summary>
    /// 优化的FloraMesh模型，展示简单对象的字段存在性追踪
    /// </summary>
    public class EnhancedFloraMesh : XmlModelBase
    {
        private string? _name;
        private string? _material;

        [XmlAttribute("name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkPropertyExists(nameof(Name));
            }
        }

        [XmlAttribute("material")]
        public string? Material
        {
            get => _material;
            set
            {
                _material = value;
                MarkPropertyExists(nameof(Material));
            }
        }

        /// <summary>
        /// 检查Material属性在XML中是否存在
        /// </summary>
        public bool MaterialExistsInXml => PropertyExists(nameof(Material));
    }
}