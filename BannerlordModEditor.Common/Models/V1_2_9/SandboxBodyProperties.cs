using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("BodyPropertiesMin")]
public class SandboxBodyProperties
{
    [XmlElement("BodyProperty")]
    public List<SandboxBodyProperty> BodyPropertyList { get; set; } = new List<SandboxBodyProperty>();

    public bool ShouldSerializeBodyPropertyList() => BodyPropertyList.Count > 0;
}

public class SandboxBodyProperty
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("age")]
    public string Age { get; set; } = string.Empty;

    [XmlAttribute("weight")]
    public string Weight { get; set; } = string.Empty;

    [XmlAttribute("build")]
    public string Build { get; set; } = string.Empty;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
    public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
    public bool ShouldSerializeBuild() => !string.IsNullOrEmpty(Build);
}
