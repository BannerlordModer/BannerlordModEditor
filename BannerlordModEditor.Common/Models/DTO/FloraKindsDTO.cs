using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("flora_kinds")]
    public class FloraKindsDTO
    {
        [XmlElement("flora_kind")]
        public List<FloraKindDTO> FloraKindsList { get; set; } = new List<FloraKindDTO>();
        
        public bool ShouldSerializeFloraKindsList() => FloraKindsList != null && FloraKindsList.Count > 0;
    }

    public class FloraKindDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("view_distance")]
        public string? ViewDistance { get; set; }

        [XmlElement("flags")]
        public FloraFlagsDTO? Flags { get; set; }

        [XmlElement("seasonal_kind")]
        public List<SeasonalKindDTO> SeasonalKinds { get; set; } = new List<SeasonalKindDTO>();
        
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeViewDistance() => !string.IsNullOrEmpty(ViewDistance);
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagsList.Count > 0;
        public bool ShouldSerializeSeasonalKinds() => SeasonalKinds != null && SeasonalKinds.Count > 0;
    }

    /// <summary>
    /// 植物标志集合
    /// </summary>
    public class FloraFlagsDTO
    {
        [XmlElement("flag")]
        public List<FloraFlagDTO> FlagsList { get; set; } = new List<FloraFlagDTO>();
    }

    /// <summary>
    /// 单个植物标志
    /// </summary>
    public class FloraFlagDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    public class SeasonalKindDTO
    {
        [XmlAttribute("season")]
        public string? Season { get; set; }

        [XmlElement("flora_variations")]
        public FloraVariationsDTO FloraVariations { get; set; } = new FloraVariationsDTO();
        
        public bool ShouldSerializeSeason() => !string.IsNullOrEmpty(Season);
        public bool ShouldSerializeFloraVariations() => FloraVariations != null && FloraVariations.FloraVariationList.Count > 0;
    }

    public class FloraVariationsDTO
    {
        [XmlElement("flora_variation")]
        public List<FloraVariationDTO> FloraVariationList { get; set; } = new List<FloraVariationDTO>();
        
        public bool ShouldSerializeFloraVariationList() => FloraVariationList != null && FloraVariationList.Count > 0;
    }

    public class FloraVariationDTO
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
        public List<MeshDTO> Meshes { get; set; } = new List<MeshDTO>();
        
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeBodyName() => !string.IsNullOrEmpty(BodyName);
        public bool ShouldSerializeDensityMultiplier() => !string.IsNullOrEmpty(DensityMultiplier);
        public bool ShouldSerializeBbRadius() => !string.IsNullOrEmpty(BbRadius);
        public bool ShouldSerializeMeshes() => Meshes != null && Meshes.Count > 0;
    }

    public class MeshDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("material")]
        public string? Material { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeMaterial() => !string.IsNullOrEmpty(Material);
    }
}