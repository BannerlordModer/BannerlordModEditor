using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("Projects")]
public class SPProjects
{
    [XmlElement("Project")]
    public List<SPProject> ProjectList { get; set; } = new List<SPProject>();
}

public class SPProject
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
