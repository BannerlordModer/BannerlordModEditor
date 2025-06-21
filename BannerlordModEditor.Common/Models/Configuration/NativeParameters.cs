using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Configuration;

// native_parameters.xml - Native game engine parameters for movement, combat, and physics
[XmlRoot("base")]
public class NativeParametersXml
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("native_parameters")]
    public NativeParameters? NativeParameters { get; set; }
}

public class NativeParameters
{
    [XmlElement("native_parameter")]
    public List<NativeParameter> NativeParameter { get; set; } = new();
}

public class NativeParameter
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
} 