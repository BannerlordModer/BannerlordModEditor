using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// Credits.xml - Game credits and acknowledgments structure
[XmlRoot("Credits")]
public class Credits
{
    [XmlElement("Category")]
    public List<CreditsFileCategory> Category { get; set; } = new();

    [XmlElement("LoadFromFile")]
    public List<CreditsFileLoadFromFile> LoadFromFile { get; set; } = new();

    public bool ShouldSerializeCategory() => Category?.Count > 0;
    public bool ShouldSerializeLoadFromFile() => LoadFromFile?.Count > 0;
}

public class CreditsFileCategory
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("Section")]
    public List<CreditsFileSection> Section { get; set; } = new();

    [XmlElement("Entry")]
    public List<CreditsFileEntry> Entry { get; set; } = new();

    [XmlElement("EmptyLine")]
    public List<CreditsFileEmptyLine> EmptyLine { get; set; } = new();

    [XmlElement("Image")]
    public List<CreditsFileImage> Image { get; set; } = new();

    public bool ShouldSerializeSection() => Section?.Count > 0;
    public bool ShouldSerializeEntry() => Entry?.Count > 0;
    public bool ShouldSerializeEmptyLine() => EmptyLine?.Count > 0;
    public bool ShouldSerializeImage() => Image?.Count > 0;
}

public class CreditsFileSection
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("Entry")]
    public List<CreditsFileEntry> Entry { get; set; } = new();

    [XmlElement("EmptyLine")]
    public List<CreditsFileEmptyLine> EmptyLine { get; set; } = new();

    public bool ShouldSerializeEntry() => Entry?.Count > 0;
    public bool ShouldSerializeEmptyLine() => EmptyLine?.Count > 0;
}

public class CreditsFileEntry
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }

    [XmlElement("EmptyLine")]
    public List<CreditsFileEmptyLine> EmptyLine { get; set; } = new();

    public bool ShouldSerializeEmptyLine() => EmptyLine?.Count > 0;
}

public class CreditsFileEmptyLine
{
    // Empty element used for formatting
}

public class CreditsFileLoadFromFile
{
    [XmlAttribute("Name")]
    public string? Name { get; set; }

    [XmlAttribute("PlatformSpecific")]
    public string? PlatformSpecific { get; set; }
}

public class CreditsFileImage
{
    [XmlAttribute("Text")]
    public string? Text { get; set; }
} 