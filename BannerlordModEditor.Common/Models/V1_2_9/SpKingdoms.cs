using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

// spkingdoms.xml - Single Player Kingdoms for Bannerlord 1.2.9
[XmlRoot("Kingdoms")]
public class SpKingdoms
{
    [XmlElement("Kingdom")]
    public List<Kingdom> Kingdoms { get; set; } = new List<Kingdom>();
}

public class Kingdom
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("owner")]
    public string? Owner { get; set; }

    [XmlAttribute("banner_key")]
    public string? BannerKey { get; set; }

    [XmlAttribute("primary_banner_color")]
    public string? PrimaryBannerColor { get; set; }

    [XmlAttribute("secondary_banner_color")]
    public string? SecondaryBannerColor { get; set; }

    [XmlAttribute("label_color")]
    public string? LabelColor { get; set; }

    [XmlAttribute("color")]
    public string? Color { get; set; }

    [XmlAttribute("color2")]
    public string? Color2 { get; set; }

    [XmlAttribute("alternative_color")]
    public string? AlternativeColor { get; set; }

    [XmlAttribute("alternative_color2")]
    public string? AlternativeColor2 { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("settlement_banner_mesh")]
    public string? SettlementBannerMesh { get; set; }

    [XmlAttribute("flag_mesh")]
    public string? FlagMesh { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("short_name")]
    public string? ShortName { get; set; }

    [XmlAttribute("title")]
    public string? Title { get; set; }

    [XmlAttribute("ruler_title")]
    public string? RulerTitle { get; set; }

    [XmlAttribute("text")]
    public string? Text { get; set; }

    // Optional child elements
    [XmlElement("relationships")]
    public Relationships? Relationships { get; set; }

    [XmlElement("policies")]
    public Policies? Policies { get; set; }

    // ShouldSerialize methods for optional attributes - only serialize if value is not null/empty
    public bool ShouldSerializeOwner() => !string.IsNullOrEmpty(Owner);
    public bool ShouldSerializeBannerKey() => !string.IsNullOrEmpty(BannerKey);
    public bool ShouldSerializePrimaryBannerColor() => !string.IsNullOrEmpty(PrimaryBannerColor);
    public bool ShouldSerializeSecondaryBannerColor() => !string.IsNullOrEmpty(SecondaryBannerColor);
    public bool ShouldSerializeLabelColor() => !string.IsNullOrEmpty(LabelColor);
    public bool ShouldSerializeColor() => !string.IsNullOrEmpty(Color);
    public bool ShouldSerializeColor2() => !string.IsNullOrEmpty(Color2);
    public bool ShouldSerializeAlternativeColor() => !string.IsNullOrEmpty(AlternativeColor);
    public bool ShouldSerializeAlternativeColor2() => !string.IsNullOrEmpty(AlternativeColor2);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeSettlementBannerMesh() => !string.IsNullOrEmpty(SettlementBannerMesh);
    public bool ShouldSerializeFlagMesh() => !string.IsNullOrEmpty(FlagMesh);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeShortName() => !string.IsNullOrEmpty(ShortName);
    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);
    public bool ShouldSerializeRulerTitle() => !string.IsNullOrEmpty(RulerTitle);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeRelationships() => Relationships != null && Relationships.RelationshipList.Count > 0;
    public bool ShouldSerializePolicies() => Policies != null && Policies.PoliciesList.Count > 0;
}

public class Relationships
{
    [XmlElement("relationship")]
    public List<Relationship> RelationshipList { get; set; } = new List<Relationship>();
}

public class Relationship
{
    [XmlAttribute("kingdom")]
    public string Kingdom { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;

    [XmlAttribute("isAtWar")]
    public string? IsAtWar { get; set; }

    public bool ShouldSerializeIsAtWar() => !string.IsNullOrEmpty(IsAtWar);
}

public class Policies
{
    [XmlElement("policy")]
    public List<Policy> PoliciesList { get; set; } = new List<Policy>();
}

public class Policy
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
