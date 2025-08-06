using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("layer_flora_sets")]
    public class FloraLayerSets
    {
        [XmlElement("layer_flora_set")]
        public List<FloraLayerSet> LayerFloraSets { get; set; } = new List<FloraLayerSet>();

        public bool ShouldSerializeLayerFloraSets() => LayerFloraSets != null && LayerFloraSets.Count > 0;
    }

    public class FloraLayerSet
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("layer_flora")]
        public List<LayerFlora> LayerFloras { get; set; } = new List<LayerFlora>();

        public bool ShouldSerializeLayerFloras() => LayerFloras != null && LayerFloras.Count > 0;
    }

    public class LayerFlora
    {
        [XmlElement("mesh")]
        public Mesh Mesh { get; set; }

        public bool ShouldSerializeMesh() => Mesh != null;
    }

    public class Mesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("density")]
        public float Density { get; set; }

        [XmlAttribute("seed_index")]
        public int SeedIndex { get; set; }

        [XmlAttribute("colony_radius")]
        public float ColonyRadius { get; set; }

        [XmlAttribute("colony_threshold")]
        public float ColonyThreshold { get; set; }

        [XmlAttribute("size_min")]
        public string SizeMin { get; set; }

        [XmlAttribute("size_max")]
        public string SizeMax { get; set; }

        [XmlAttribute("albedo_multiplier")]
        public string AlbedoMultiplier { get; set; }

        [XmlAttribute("weight_offset")]
        public float WeightOffset { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIndex() => true;
        public bool ShouldSerializeDensity() => true;
        public bool ShouldSerializeSeedIndex() => true;
        public bool ShouldSerializeColonyRadius() => true;
        public bool ShouldSerializeColonyThreshold() => true;
        public bool ShouldSerializeSizeMin() => !string.IsNullOrEmpty(SizeMin);
        public bool ShouldSerializeSizeMax() => !string.IsNullOrEmpty(SizeMax);
        public bool ShouldSerializeAlbedoMultiplier() => !string.IsNullOrEmpty(AlbedoMultiplier);
        public bool ShouldSerializeWeightOffset() => true;
    }
}