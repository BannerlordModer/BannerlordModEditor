using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Audio
{
    /// <summary>
    /// Root element for module_sounds.xml - Contains module sound definitions
    /// </summary>
    [XmlRoot("base")]
    public class ModuleSoundsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "module_sound";

        [XmlElement("module_sounds")]
        public ModuleSoundsContainer ModuleSounds { get; set; } = new ModuleSoundsContainer();
    }

    /// <summary>
    /// Container for module sounds collection
    /// </summary>
    public class ModuleSoundsContainer
    {
        [XmlElement("module_sound")]
        public List<ModuleSound> ModuleSound { get; set; } = new List<ModuleSound>();
    }

    /// <summary>
    /// Individual module sound definition with category and variations
    /// </summary>
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
        public List<SoundVariation>? Variation { get; set; }
    }

    /// <summary>
    /// Sound variation with path and weight
    /// </summary>
    public class SoundVariation
    {
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
} 