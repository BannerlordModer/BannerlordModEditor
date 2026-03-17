using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

// partyTemplates.xml - Party Templates for Bannerlord 1.2.9
// Root element: <PartyTemplates>
[XmlRoot("PartyTemplates")]
public class PartyTemplates
{
    [XmlElement("PartyTemplate")]
    public List<PartyTemplate> PartyTemplateList { get; set; } = new List<PartyTemplate>();
}

// PartyTemplate attributes:
// - id, culture, name, template
public class PartyTemplate
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("template")]
    public string? Template { get; set; }

    // Child elements
    [XmlElement("Roles")]
    public Roles? Roles { get; set; }

    // ShouldSerialize methods for optional attributes
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeTemplate() => !string.IsNullOrEmpty(Template);
    public bool ShouldSerializeRoles() => Roles != null && Roles.RoleList.Count > 0;
}

// Roles contains Role elements
public class Roles
{
    [XmlElement("Role")]
    public List<Role> RoleList { get; set; } = new List<Role>();
}

// Role attributes:
// - id, type, value
public class Role
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}
