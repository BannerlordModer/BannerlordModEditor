using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ManagedCampaignParameters
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("managed_campaign_parameters")]
        public ManagedCampaignParametersContainer ParametersContainer { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeParametersContainer() => ParametersContainer != null;
    }

    public class ManagedCampaignParametersContainer
    {
        [XmlElement("managed_campaign_parameter")]
        public List<ManagedCampaignParameter> Parameters { get; set; }

        public bool ShouldSerializeParameters() => Parameters != null && Parameters.Count > 0;
    }

    public class ManagedCampaignParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}