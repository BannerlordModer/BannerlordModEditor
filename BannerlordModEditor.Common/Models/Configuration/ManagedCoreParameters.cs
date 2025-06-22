using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Configuration
{
    [XmlRoot("base")]
    public class ManagedCoreParametersRoot
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("managed_core_parameters")]
        public ManagedCoreParameters? ManagedCoreParameters { get; set; }
    }

    public class ManagedCoreParameters
    {
        [XmlElement("managed_core_parameter")]
        public ManagedCoreParameter[]? ManagedCoreParameter { get; set; }
    }

    public class ManagedCoreParameter
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }
    }
} 