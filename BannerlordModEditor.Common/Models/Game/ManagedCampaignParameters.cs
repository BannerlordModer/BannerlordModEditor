using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game;

// managed_campaign_parameters.xml - Campaign parameter definitions
[XmlRoot("base")]
public class ManagedCampaignParametersBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "campaign_parameters";

    [XmlElement("managed_campaign_parameters")]
    public ManagedCampaignParametersContainer ManagedCampaignParameters { get; set; } = new ManagedCampaignParametersContainer();
}

public class ManagedCampaignParametersContainer
{
    [XmlElement("managed_campaign_parameter")]
    public List<ManagedCampaignParameter> ManagedCampaignParameter { get; set; } = new List<ManagedCampaignParameter>();
}

public class ManagedCampaignParameter
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
} 