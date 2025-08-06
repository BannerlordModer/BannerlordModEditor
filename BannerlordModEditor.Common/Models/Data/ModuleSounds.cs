using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ModuleSounds
    {
       private string? _type;
       private bool _typeExists;

       [XmlAttribute("type")]
       public string? Type
       {
           get => _typeExists ? _type : null;
           set
           {
               _type = value;
               _typeExists = true;
           }
       }

       public bool ShouldSerializeType() => _typeExists;

       [XmlElement("module_sounds")]
       public ModuleSoundsContainer? ModuleSoundsContainer { get; set; }
    }

    public class ModuleSoundsContainer
    {
        [XmlElement("module_sound")]
        public List<ModuleSound> ModuleSoundList { get; set; } = new List<ModuleSound>();
    }

    public class ModuleSound
    {
       private string? _name;
       private bool _nameExists;
       private string? _is2D;
       private bool _is2DExists;
       private string? _soundCategory;
       private bool _soundCategoryExists;
       private string? _path;
       private bool _pathExists;
       private string? _minPitchMultiplier;
       private bool _minPitchMultiplierExists;
       private string? _maxPitchMultiplier;
       private bool _maxPitchMultiplierExists;

       [XmlAttribute("name")]
       public string? Name
       {
           get => _nameExists ? _name : null;
           set
           {
               _name = value;
               _nameExists = true;
           }
       }

       [XmlAttribute("is_2d")]
       public string? Is2D
       {
           get => _is2DExists ? _is2D : null;
           set
           {
               _is2D = value;
               _is2DExists = true;
           }
       }

       [XmlAttribute("sound_category")]
       public string? SoundCategory
       {
           get => _soundCategoryExists ? _soundCategory : null;
           set
           {
               _soundCategory = value;
               _soundCategoryExists = true;
           }
       }

       [XmlAttribute("path")]
       public string? Path
       {
           get => _pathExists ? _path : null;
           set
           {
               _path = value;
               _pathExists = true;
           }
       }

       [XmlAttribute("min_pitch_multiplier")]
       public string? MinPitchMultiplier
       {
           get => _minPitchMultiplierExists ? _minPitchMultiplier : null;
           set
           {
               _minPitchMultiplier = value;
               _minPitchMultiplierExists = true;
           }
       }

       [XmlAttribute("max_pitch_multiplier")]
       public string? MaxPitchMultiplier
       {
           get => _maxPitchMultiplierExists ? _maxPitchMultiplier : null;
           set
           {
               _maxPitchMultiplier = value;
               _maxPitchMultiplierExists = true;
           }
       }

       [XmlElement("variation")]
       public List<SoundVariation>? Variations { get; set; }

       public bool ShouldSerializeName() => _nameExists;
       public bool ShouldSerializeIs2D() => _is2DExists;
       public bool ShouldSerializeSoundCategory() => _soundCategoryExists;
       public bool ShouldSerializePath() => _pathExists;
       public bool ShouldSerializeMinPitchMultiplier() => _minPitchMultiplierExists;
       public bool ShouldSerializeMaxPitchMultiplier() => _maxPitchMultiplierExists;
    }

    public class SoundVariation
    {
        [XmlAttribute("path")]
        public string Path { get; set; } = string.Empty;

        [XmlAttribute("weight")]
        public string Weight { get; set; } = string.Empty;
    }
} 