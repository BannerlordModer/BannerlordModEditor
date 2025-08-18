using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("flora_kinds")]
    public class FloraKinds
    {
        [XmlElement("flora_kind")]
        public List<FloraKind> FloraKindList { get; set; } = new List<FloraKind>();
    }

    public class FloraKindsContainer
    {
        [XmlElement("flora_kind")]
        public List<FloraKind> FloraKindList { get; set; } = new List<FloraKind>();
        public bool ShouldSerializeFloraKindList() => FloraKindList != null && FloraKindList.Count > 0;
    }

    public class FloraKind
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("view_distance")]
        public string ViewDistanceString
        {
            get => ViewDistance.HasValue ? ViewDistance.Value.ToString() : null;
            set => ViewDistance = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? ViewDistance { get; set; }

        [XmlElement("flags")]
        public FloraFlags Flags { get; set; }

        [XmlElement("seasonal_kind")]
        public List<SeasonalKind> SeasonalKinds { get; set; } = new List<SeasonalKind>();

        public bool ShouldSerializeViewDistance() => ViewDistance.HasValue;
        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeSeasonalKinds() => SeasonalKinds != null && SeasonalKinds.Count > 0;
    }

    public class FloraFlags
    {
        [XmlElement("flag")]
        public List<FloraFlag> FlagList { get; set; } = new List<FloraFlag>();
        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class FloraFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrWhiteSpace(Value);
    }

    public class SeasonalKind
    {
        [XmlAttribute("season")]
        public string Season { get; set; }

        [XmlElement("flora_variations")]
        public FloraVariations FloraVariations { get; set; }

        public bool ShouldSerializeSeason() => !string.IsNullOrWhiteSpace(Season);
        public bool ShouldSerializeFloraVariations() => FloraVariations != null;
    }

    public class FloraVariations
    {
        [XmlElement("flora_variation")]
        public List<FloraVariation> FloraVariationList { get; set; } = new List<FloraVariation>();
        public bool ShouldSerializeFloraVariationList() => FloraVariationList != null && FloraVariationList.Count > 0;
    }

    public class FloraVariation
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("density_multiplier")]
        public string DensityMultiplierString
        {
            get => DensityMultiplier.HasValue ? DensityMultiplier.Value.ToString() : null;
            set => DensityMultiplier = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? DensityMultiplier { get; set; }

        [XmlAttribute("bb_radius")]
        public string BbRadiusString
        {
            get => BbRadius.HasValue ? BbRadius.Value.ToString() : null;
            set => BbRadius = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? BbRadius { get; set; }
        
        // 简化实现：添加缺失的body_name属性
        // 原本实现：FloraVariation类没有body_name属性
        // 简化实现：添加body_name属性以匹配XML结构
        [XmlAttribute("body_name")]
        public string BodyName { get; set; }

        [XmlElement("mesh")]
        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeDensityMultiplier() => DensityMultiplier.HasValue;
        public bool ShouldSerializeBbRadius() => BbRadius.HasValue;
        public bool ShouldSerializeBodyName() => !string.IsNullOrWhiteSpace(BodyName);
        public bool ShouldSerializeMeshes() => Meshes != null && Meshes.Count > 0;
    }

    public class Mesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("material")]
        public string Material { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeMaterial() => !string.IsNullOrWhiteSpace(Material);
    }
}