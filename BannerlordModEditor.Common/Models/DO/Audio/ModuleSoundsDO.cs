using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Audio
{
    [XmlRoot("base")]
    public class ModuleSoundsDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "module_sound";

        [XmlElement("module_sounds")]
        public ModuleSoundsContainerDO ModuleSounds { get; set; } = new ModuleSoundsContainerDO();

        [XmlIgnore]
        public bool HasModuleSounds { get; set; } = false;

        public bool ShouldSerializeModuleSounds() => HasModuleSounds && ModuleSounds != null && ModuleSounds.Sounds.Count > 0;
    }

    public class ModuleSoundsContainerDO
    {
        [XmlElement("module_sound")]
        public List<ModuleSoundDO> Sounds { get; set; } = new List<ModuleSoundDO>();
    }

    public class ModuleSoundDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("is_2d")]
        public bool Is2D { get; set; } = false;

        [XmlAttribute("sound_category")]
        public string SoundCategory { get; set; } = string.Empty;

        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("min_pitch_multiplier")]
        public string MinPitchMultiplier { get; set; } = string.Empty;

        [XmlAttribute("max_pitch_multiplier")]
        public string MaxPitchMultiplier { get; set; } = string.Empty;

        [XmlArray("variations")]
        [XmlArrayItem("variation")]
        public List<SoundVariationDO> Variations { get; set; } = new List<SoundVariationDO>();

        [XmlIgnore]
        public bool HasVariations { get; set; } = false;

        public bool ShouldSerializeVariations() => HasVariations && Variations != null && Variations.Count > 0;
    }

    public class SoundVariationDO
    {
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
}