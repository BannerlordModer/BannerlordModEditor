using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ModuleSounds
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("module_sounds")]
        public ModuleSoundsContainer ModuleSoundsContainer { get; set; } = new ModuleSoundsContainer();
    }

    public class ModuleSoundsContainer
    {
        [XmlElement("module_sound")]
        public List<ModuleSound> ModuleSoundList { get; set; } = new List<ModuleSound>();
    }

    public class ModuleSound
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("is_2d")]
        public string? Is2D { get; set; }

        [XmlAttribute("sound_category")]
        public string SoundCategory { get; set; } = string.Empty;

        [XmlAttribute("path")]
        public string? Path { get; set; }

        [XmlAttribute("min_pitch_multiplier")]
        public string? MinPitchMultiplier { get; set; }

        [XmlAttribute("max_pitch_multiplier")]
        public string? MaxPitchMultiplier { get; set; }

        [XmlElement("variation")]
        public List<SoundVariation> Variations { get; set; } = new List<SoundVariation>();

        // ShouldSerialize methods for optional properties
        public bool ShouldSerializeIs2D() => !string.IsNullOrEmpty(Is2D);
        public bool ShouldSerializePath() => !string.IsNullOrEmpty(Path);
        public bool ShouldSerializeMinPitchMultiplier() => !string.IsNullOrEmpty(MinPitchMultiplier);
        public bool ShouldSerializeMaxPitchMultiplier() => !string.IsNullOrEmpty(MaxPitchMultiplier);
    }

    public class SoundVariation
    {
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
} 