using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// Credits.xml - Game credits and acknowledgments structure
[XmlRoot("Credits")]
public class Credits
{
    [XmlElement("Category")]
    public List<CreditsCategory> Category { get; set; } = new();

    [XmlElement("LoadFromFile")]
    public List<LoadFromFile> LoadFromFile { get; set; } = new();
}

public class CreditsCategory
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("Section")]
    public List<CreditsSection> Section { get; set; } = new();

    [XmlElement("Entry")]
    public List<CreditsEntry> Entry { get; set; } = new();

    [XmlElement("EmptyLine")]
    public List<EmptyLine> EmptyLine { get; set; } = new();

    [XmlElement("Image")]
    public List<CreditsImage> Image { get; set; } = new();
}

public class CreditsSection
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("Entry")]
    public List<CreditsEntry> Entry { get; set; } = new();

    [XmlElement("EmptyLine")]
    public List<EmptyLine> EmptyLine { get; set; } = new();
}

public class CreditsEntry
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("EmptyLine")]
    public List<EmptyLine> EmptyLine { get; set; } = new();
}

public class EmptyLine
{
    // Empty element used for formatting
}

public class LoadFromFile
{
    [XmlAttribute("Name")]
    public string? Name { get; set; }

    [XmlAttribute("PlatformSpecific")]
    public string? PlatformSpecific { get; set; }
}

public class CreditsImage
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }
} 