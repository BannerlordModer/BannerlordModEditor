using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("flora_kinds")]
    public class FloraKindsDO
    {
        [XmlElement("flora_kind")]
        public List<FloraKindDO> FloraKindsList { get; set; } = new List<FloraKindDO>();
        
        public bool ShouldSerializeFloraKindsList() => FloraKindsList != null && FloraKindsList.Count > 0;
    }

    public class FloraKindDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("view_distance")]
        public string? ViewDistance { get; set; }

        [XmlElement("flags")]
        public FloraFlagsDO? Flags { get; set; }
        
        [XmlIgnore]
        public bool HasFlags { get; set; } = false;

        [XmlElement("seasonal_kind")]
        public List<SeasonalKindDO> SeasonalKinds { get; set; } = new List<SeasonalKindDO>();
        
        [XmlIgnore]
        public bool HasName { get; set; } = false;
        
        public bool ShouldSerializeName() => HasName;
        public bool ShouldSerializeViewDistance() => !string.IsNullOrEmpty(ViewDistance);
        public bool ShouldSerializeFlags() => HasFlags && Flags != null && Flags.FlagsList.Count > 0;
        public bool ShouldSerializeSeasonalKinds() => SeasonalKinds != null && SeasonalKinds.Count > 0;
    }

    /// <summary>
    /// 植物标志集合
    /// </summary>
    public class FloraFlagsDO
    {
        [XmlElement("flag")]
        public List<FloraFlagDO> FlagsList { get; set; } = new List<FloraFlagDO>();
    }

    /// <summary>
    /// 单个植物标志
    /// </summary>
    public class FloraFlagDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    public class SeasonalKindDO
    {
        [XmlAttribute("season")]
        public string? Season { get; set; }

        [XmlElement("flora_variations")]
        public FloraVariationsDO FloraVariations { get; set; } = new FloraVariationsDO();
        
        [XmlIgnore]
        public bool HasFloraVariations { get; set; } = false;
        
        public bool ShouldSerializeSeason() => !string.IsNullOrEmpty(Season);
        public bool ShouldSerializeFloraVariations() => HasFloraVariations && FloraVariations != null && FloraVariations.FloraVariationList.Count > 0;
    }

    public class FloraVariationsDO
    {
        [XmlElement("flora_variation")]
        public List<FloraVariationDO> FloraVariationList { get; set; } = new List<FloraVariationDO>();
        
        public bool ShouldSerializeFloraVariationList() => FloraVariationList != null && FloraVariationList.Count > 0;
    }

    public class FloraVariationDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("body_name")]
        public string? BodyName { get; set; }

        [XmlAttribute("density_multiplier")]
        public string? DensityMultiplier { get; set; }

        [XmlAttribute("bb_radius")]
        public string? BbRadius { get; set; }

        [XmlElement("mesh")]
        public List<MeshDO> Meshes { get; set; } = new List<MeshDO>();
        
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeBodyName() => !string.IsNullOrEmpty(BodyName);
        public bool ShouldSerializeDensityMultiplier() => !string.IsNullOrEmpty(DensityMultiplier);
        public bool ShouldSerializeBbRadius() => !string.IsNullOrEmpty(BbRadius);
        public bool ShouldSerializeMeshes() => Meshes != null && Meshes.Count > 0;
    }

    public class MeshDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("material")]
        public string? Material { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeMaterial() => !string.IsNullOrEmpty(Material);
    }
}