using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("Heroes")]
public class Heroes
{
    [XmlElement("Hero")]
    public List<Hero> HeroList { get; set; } = new List<Hero>();

    public bool ShouldSerializeHeroList() => HeroList.Count > 0;
}

public class Hero
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("alive")]
    public string? Alive { get; set; }

    [XmlAttribute("faction")]
    public string? Faction { get; set; }

    [XmlAttribute("text")]
    public string? Text { get; set; }

    [XmlAttribute("father")]
    public string? Father { get; set; }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeAlive() => !string.IsNullOrEmpty(Alive);
    public bool ShouldSerializeFaction() => !string.IsNullOrEmpty(Faction);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeFather() => !string.IsNullOrEmpty(Father);
}
