using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Audio
{
    [XmlRoot("base")]
    public class ModuleSoundsDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "module_sound";

        [XmlElement("module_sounds")]
        public ModuleSoundsContainerDTO ModuleSounds { get; set; } = new ModuleSoundsContainerDTO();
    }

    public class ModuleSoundsContainerDTO
    {
        [XmlElement("module_sound")]
        public List<ModuleSoundDTO> Sounds { get; set; } = new List<ModuleSoundDTO>();
    }

    public class ModuleSoundDTO
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
        public List<SoundVariationDTO> Variations { get; set; } = new List<SoundVariationDTO>();
    }

    public class SoundVariationDTO
    {
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
}