using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Configuration;

[XmlRoot("base")]
public class ProjectConfiguration
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("outputDirectory")]
    public string? OutputDirectory { get; set; }

    [XmlElement("XMLDirectory")]
    public string? XmlDirectory { get; set; }

    [XmlElement("ModuleAssemblyDirectory")]
    public string? ModuleAssemblyDirectory { get; set; }

    [XmlElement("file")]
    public List<ProjectFile> Files { get; set; } = new();
}

public class ProjectFile
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }
}