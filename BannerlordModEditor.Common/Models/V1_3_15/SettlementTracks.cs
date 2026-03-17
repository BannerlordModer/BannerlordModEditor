using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("MusicTracks")]
public class SettlementTracks
{
    [XmlElement("MusicTrack")]
    public List<MusicTrack> Tracks { get; set; } = new List<MusicTrack>();

    public bool ShouldSerializeTracks() => Tracks.Count > 0;
}

public class MusicTrack
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("event_id")]
    public string? EventId { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("location")]
    public string? Location { get; set; }

    [XmlAttribute("tempo")]
    public string? Tempo { get; set; }

    [XmlElement("Instruments")]
    public Instruments? Instruments { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeEventId() => !string.IsNullOrEmpty(EventId);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeLocation() => !string.IsNullOrEmpty(Location);
    public bool ShouldSerializeTempo() => !string.IsNullOrEmpty(Tempo);
    public bool ShouldSerializeInstruments() => Instruments != null && Instruments.InstrumentList.Count > 0;
}

public class Instruments
{
    [XmlElement("Instrument")]
    public List<Instrument> InstrumentList { get; set; } = new List<Instrument>();

    public bool ShouldSerializeInstrumentList() => InstrumentList.Count > 0;
}

public class Instrument
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
}
