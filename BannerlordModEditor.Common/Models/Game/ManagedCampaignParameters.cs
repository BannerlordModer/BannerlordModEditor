using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game;

// managed_campaign_parameters.xml - Campaign parameter definitions
[XmlRoot("base")]
public class ManagedCampaignParameters
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement("managed_campaign_parameters")]
    public CampaignParametersContainer Parameters { get; set; } = new CampaignParametersContainer();
}

public class CampaignParametersContainer
{
    [XmlElement("managed_campaign_parameter")]
    public List<CampaignParameter> ParameterList { get; set; } = new List<CampaignParameter>();
}

public class CampaignParameter
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;
} 