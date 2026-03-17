using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("BanditFactions")]
public class BanditFactions
{
    [XmlElement("BanditFaction")]
    public List<BanditFaction> BanditFactionList { get; set; } = new List<BanditFaction>();
}

public class BanditFaction
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlElement("members")]
    public Members? Members { get; set; }

    public bool ShouldSerializeMembers() => Members != null && Members.MemberList.Count > 0;
}

public class Members
{
    [XmlElement("member")]
    public List<Member> MemberList { get; set; } = new List<Member>();
}

public class Member
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;
}
