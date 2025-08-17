using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Audio;

[XmlRoot("voice_definitions")]
public class VoiceDefinitionsDTO
{
    [XmlElement("voice_type_declarations")]
    public VoiceTypeDeclarationsDTO VoiceTypeDeclarations { get; set; } = new VoiceTypeDeclarationsDTO();

    [XmlElement("voice_definition")]
    public List<VoiceDefinitionDTO> VoiceDefinitions { get; set; } = new List<VoiceDefinitionDTO>();
}

public class VoiceTypeDeclarationsDTO
{
    [XmlElement("voice_type")]
    public List<VoiceTypeDTO> VoiceTypes { get; set; } = new List<VoiceTypeDTO>();
}

public class VoiceTypeDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
}

public class VoiceDefinitionDTO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("sound_and_collision_info_class")]
    public string SoundAndCollisionInfoClass { get; set; } = string.Empty;

    [XmlAttribute("only_for_npcs")]
    public string OnlyForNpcs { get; set; } = string.Empty;

    [XmlAttribute("min_pitch_multiplier")]
    public string MinPitchMultiplier { get; set; } = string.Empty;

    [XmlAttribute("max_pitch_multiplier")]
    public string MaxPitchMultiplier { get; set; } = string.Empty;

    [XmlElement("voice")]
    public List<VoiceDTO> Voices { get; set; } = new List<VoiceDTO>();
}

public class VoiceDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("path")]
    public string Path { get; set; } = string.Empty;

    [XmlAttribute("face_anim")]
    public string FaceAnim { get; set; } = string.Empty;
}