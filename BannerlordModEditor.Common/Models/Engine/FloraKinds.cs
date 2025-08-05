using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("flora_kinds")]
    public class FloraKinds
    {
        [XmlElement("flora_kind")]
        public List<FloraKind> FloraKind { get; set; } = new();

        public bool ShouldSerializeFloraKind() => FloraKind?.Count > 0;
    }

    public class FloraKind
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("view_distance")]
        public string? ViewDistance { get; set; }

        [XmlElement("flags")]
        public FloraFlags? Flags { get; set; }

        [XmlElement("seasonal_kind")]
        public List<SeasonalKind> SeasonalKind { get; set; } = new();

        public bool ShouldSerializeSeasonalKind() => SeasonalKind?.Count > 0;
    }

    public class FloraFlags
    {
        [XmlElement("flag")]
        public List<FloraFlag> Flag { get; set; } = new();

        public bool ShouldSerializeFlag() => Flag?.Count > 0;
    }

    public class FloraFlag
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    public class SeasonalKind
    {
        [XmlAttribute("season")]
        public string? Season { get; set; }

        [XmlElement("flora_variations")]
        public FloraVariations? FloraVariations { get; set; }
    }

    public class FloraVariations
    {
        [XmlElement("flora_variation")]
        public List<FloraVariation> FloraVariation { get; set; } = new();

        public bool ShouldSerializeFloraVariation() => FloraVariation?.Count > 0;
    }

    public class FloraVariation
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
        public List<FloraMesh> Mesh { get; set; } = new();

        public bool ShouldSerializeMesh() => Mesh?.Count > 0;
    }

    public class FloraMesh
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("material")]
        public string? Material { get; set; }
    }
}