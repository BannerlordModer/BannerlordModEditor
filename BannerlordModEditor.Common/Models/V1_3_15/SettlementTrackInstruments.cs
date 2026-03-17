using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("MusicInstruments")]
public class SettlementTrackInstruments
{
    [XmlElement("MusicInstrument")]
    public List<MusicInstrument> Instruments { get; set; } = new List<MusicInstrument>();

    public bool ShouldSerializeInstruments() => Instruments.Count > 0;
}

public class MusicInstrument
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("sittingAction")]
    public string? SittingAction { get; set; }

    [XmlAttribute("standingAction")]
    public string? StandingAction { get; set; }

    [XmlAttribute("tag")]
    public string? Tag { get; set; }

    [XmlElement("Entities")]
    public Entities? Entities { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeSittingAction() => !string.IsNullOrEmpty(SittingAction);
    public bool ShouldSerializeStandingAction() => !string.IsNullOrEmpty(StandingAction);
    public bool ShouldSerializeTag() => !string.IsNullOrEmpty(Tag);
    public bool ShouldSerializeEntities() => Entities != null && Entities.EntityList.Count > 0;
}

public class Entities
{
    [XmlElement("Entity")]
    public List<Entity> EntityList { get; set; } = new List<Entity>();

    public bool ShouldSerializeEntityList() => EntityList.Count > 0;
}

public class Entity
{
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("bone")]
    public string Bone { get; set; } = string.Empty;

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeBone() => !string.IsNullOrEmpty(Bone);
}
