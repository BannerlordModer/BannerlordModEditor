using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Audio;

[XmlRoot("voice_definitions")]
public class VoiceDefinitionsDO
{
    [XmlElement("voice_type_declarations")]
    public VoiceTypeDeclarationsDO VoiceTypeDeclarations { get; set; } = new VoiceTypeDeclarationsDO();

    [XmlElement("voice_definition")]
    public List<VoiceDefinitionDO> VoiceDefinitions { get; set; } = new List<VoiceDefinitionDO>();

    [XmlIgnore]
    public bool HasVoiceTypeDeclarations { get; set; } = false;

    public bool ShouldSerializeVoiceTypeDeclarations() => HasVoiceTypeDeclarations && VoiceTypeDeclarations != null && VoiceTypeDeclarations.VoiceTypes.Count > 0;
    public bool ShouldSerializeVoiceDefinitions() => VoiceDefinitions != null && VoiceDefinitions.Count > 0;
}

public class VoiceTypeDeclarationsDO
{
    [XmlElement("voice_type")]
    public List<VoiceTypeDO> VoiceTypes { get; set; } = new List<VoiceTypeDO>();

    public bool ShouldSerializeVoiceTypes() => VoiceTypes != null && VoiceTypes.Count > 0;
}

public class VoiceTypeDO
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
}

public class VoiceDefinitionDO
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
    public List<VoiceDO> Voices { get; set; } = new List<VoiceDO>();

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeSoundAndCollisionInfoClass() => !string.IsNullOrEmpty(SoundAndCollisionInfoClass);
    public bool ShouldSerializeOnlyForNpcs() => !string.IsNullOrEmpty(OnlyForNpcs);
    public bool ShouldSerializeMinPitchMultiplier() => !string.IsNullOrEmpty(MinPitchMultiplier);
    public bool ShouldSerializeMaxPitchMultiplier() => !string.IsNullOrEmpty(MaxPitchMultiplier);
    public bool ShouldSerializeVoices() => Voices != null && Voices.Count > 0;
}

public class VoiceDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("path")]
    public string Path { get; set; } = string.Empty;

    [XmlAttribute("face_anim")]
    public string FaceAnim { get; set; } = string.Empty;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializePath() => !string.IsNullOrEmpty(Path);
    public bool ShouldSerializeFaceAnim() => !string.IsNullOrEmpty(FaceAnim);
}