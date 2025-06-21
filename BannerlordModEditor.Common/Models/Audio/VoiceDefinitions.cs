using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Audio
{
    [XmlRoot("voice_definitions")]
    public class VoiceDefinitions
    {
        [XmlElement("voice_type_declarations")]
        public VoiceTypeDeclarations? VoiceTypeDeclarations { get; set; }

        [XmlElement("voice_definition")]
        public VoiceDefinition[]? VoiceDefinition { get; set; }
    }

    public class VoiceTypeDeclarations
    {
        [XmlElement("voice_type")]
        public VoiceType[]? VoiceType { get; set; }
    }

    public class VoiceType
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
    }

    public class VoiceDefinition
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("sound_and_collision_info_class")]
        public string? SoundAndCollisionInfoClass { get; set; }

        [XmlAttribute("only_for_npcs")]
        public string? OnlyForNpcs { get; set; }

        [XmlAttribute("min_pitch_multiplier")]
        public string? MinPitchMultiplier { get; set; }

        [XmlAttribute("max_pitch_multiplier")]
        public string? MaxPitchMultiplier { get; set; }

        [XmlElement("voice")]
        public Voice[]? Voice { get; set; }
    }

    public class Voice
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("path")]
        public string? Path { get; set; }

        [XmlAttribute("face_anim")]
        public string? FaceAnim { get; set; }
    }
} 