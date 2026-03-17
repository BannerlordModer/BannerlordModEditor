using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15
{

[XmlRoot("Factions")]
public class SpClans
{
    [XmlElement("Faction")]
    public List<Faction> Factions { get; set; } = new List<Faction>();
}

public class Faction
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("banner_key")]
    public string BannerKey { get; set; } = string.Empty;

    [XmlAttribute("label_color")]
    public string LabelColor { get; set; } = string.Empty;

    [XmlAttribute("color")]
    public string Color { get; set; } = string.Empty;

    [XmlAttribute("color2")]
    public string Color2 { get; set; } = string.Empty;

    [XmlAttribute("alternative_color")]
    public string AlternativeColor { get; set; } = string.Empty;

    [XmlAttribute("alternative_color2")]
    public string AlternativeColor2 { get; set; } = string.Empty;

    [XmlAttribute("culture")]
    public string Culture { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("tier")]
    public string Tier { get; set; } = string.Empty;

    [XmlAttribute("owner")]
    public string? Owner { get; set; }

    [XmlAttribute("is_minor_faction")]
    public string? IsMinorFaction { get; set; }

    [XmlAttribute("settlement_banner_mesh")]
    public string? SettlementBannerMesh { get; set; }

    [XmlAttribute("default_party_template")]
    public string? DefaultPartyTemplate { get; set; }

    [XmlAttribute("text")]
    public string? Text { get; set; }

    [XmlAttribute("short_name")]
    public string? ShortName { get; set; }

    [XmlAttribute("is_bandit")]
    public string? IsBandit { get; set; }

    [XmlAttribute("is_outlaw")]
    public string? IsOutlaw { get; set; }

    [XmlAttribute("is_clan_type_mercenary")]
    public string? IsClanTypeMercenary { get; set; }

    [XmlAttribute("is_mafia")]
    public string? IsMafia { get; set; }

    [XmlAttribute("is_nomad")]
    public string? IsNomad { get; set; }

    [XmlAttribute("is_sect")]
    public string? IsSect { get; set; }

    [XmlAttribute("is_noble")]
    public string? IsNoble { get; set; }

    [XmlAttribute("initial_posX")]
    public string? InitialPosX { get; set; }

    [XmlAttribute("initial_posY")]
    public string? InitialPosY { get; set; }

    [XmlAttribute("super_faction")]
    public string? SuperFaction { get; set; }

    [XmlElement("minor_faction_character_templates")]
    public MinorFactionCharacterTemplates? MinorFactionCharacterTemplates { get; set; }

    public bool ShouldSerializeOwner() => !string.IsNullOrEmpty(Owner);
    public bool ShouldSerializeIsMinorFaction() => !string.IsNullOrEmpty(IsMinorFaction);
    public bool ShouldSerializeSettlementBannerMesh() => !string.IsNullOrEmpty(SettlementBannerMesh);
    public bool ShouldSerializeDefaultPartyTemplate() => !string.IsNullOrEmpty(DefaultPartyTemplate);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeShortName() => !string.IsNullOrEmpty(ShortName);
    public bool ShouldSerializeIsBandit() => !string.IsNullOrEmpty(IsBandit);
    public bool ShouldSerializeIsOutlaw() => !string.IsNullOrEmpty(IsOutlaw);
    public bool ShouldSerializeIsClanTypeMercenary() => !string.IsNullOrEmpty(IsClanTypeMercenary);
    public bool ShouldSerializeIsMafia() => !string.IsNullOrEmpty(IsMafia);
    public bool ShouldSerializeIsNomad() => !string.IsNullOrEmpty(IsNomad);
    public bool ShouldSerializeIsSect() => !string.IsNullOrEmpty(IsSect);
    public bool ShouldSerializeIsNoble() => !string.IsNullOrEmpty(IsNoble);
    public bool ShouldSerializeInitialPosX() => !string.IsNullOrEmpty(InitialPosX);
    public bool ShouldSerializeInitialPosY() => !string.IsNullOrEmpty(InitialPosY);
    public bool ShouldSerializeSuperFaction() => !string.IsNullOrEmpty(SuperFaction);
    public bool ShouldSerializeMinorFactionCharacterTemplates() => MinorFactionCharacterTemplates != null && MinorFactionCharacterTemplates.Templates.Count > 0;
}

public class MinorFactionCharacterTemplates
{
    [XmlElement("template")]
    public List<Template> Templates { get; set; } = new List<Template>();
}

public class Template
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
}
