using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("Caravans")]
public class Caravans
{
    [XmlElement("Caravan")]
    public List<Caravan> CaravanList { get; set; } = new List<Caravan>();
}

public class Caravan
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("culture")]
    public string Culture { get; set; } = string.Empty;

    [XmlAttribute("member_size")]
    public string MemberSize { get; set; } = string.Empty;

    [XmlElement("mesh")]
    public string Mesh { get; set; } = string.Empty;

    public bool ShouldSerializeMesh() => !string.IsNullOrEmpty(Mesh);
}
