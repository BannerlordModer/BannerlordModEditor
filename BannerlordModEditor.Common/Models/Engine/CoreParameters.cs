using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    /// <summary>
    /// Root element for managed_core_parameters.xml - Contains core game engine parameters
    /// </summary>
    [XmlRoot("base")]
    public class ManagedCoreParametersBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "combat_parameters";

        [XmlElement("managed_core_parameters")]
        public ManagedCoreParametersContainer ManagedCoreParameters { get; set; } = new ManagedCoreParametersContainer();
    }

    /// <summary>
    /// Container for managed core parameters
    /// </summary>
    public class ManagedCoreParametersContainer
    {
        [XmlElement("managed_core_parameter")]
        public List<ManagedCoreParameter> ManagedCoreParameter { get; set; } = new List<ManagedCoreParameter>();
    }

    /// <summary>
    /// Individual core game parameter (combat, physics, damage, etc.)
    /// </summary>
    public class ManagedCoreParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
} 