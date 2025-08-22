using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("Credits")]
    public class CreditsDTO
    {
        [XmlElement("Category")]
        public List<CreditsCategoryDTO> Categories { get; set; } = new();

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFileDTO> LoadFromFile { get; set; } = new();

        public bool ShouldSerializeCategories() => Categories != null && Categories.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile != null && LoadFromFile.Count > 0;

        // 业务逻辑属性
        public bool HasCategories => Categories?.Count > 0;
        public bool HasExternalFiles => LoadFromFile?.Count > 0;
    }

    public class CreditsCategoryDTO
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("Section")]
        public List<CreditsSectionDTO> Sections { get; set; } = new();

        [XmlElement("Entry")]
        public List<CreditsEntryDTO> Entries { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFileDTO> LoadFromFile { get; set; } = new();

        [XmlElement("Image")]
        public List<CreditsImageDTO> Images { get; set; } = new();

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
        public bool ShouldSerializeSections() => Sections != null && Sections.Count > 0;
        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile != null && LoadFromFile.Count > 0;
        public bool ShouldSerializeImages() => Images != null && Images.Count > 0;

        // 业务逻辑方法
        public bool HasContent => Sections?.Count > 0 || Entries?.Count > 0;
        public bool HasFormatting => EmptyLines?.Count > 0;
        public bool HasImages => Images?.Count > 0;
    }

    public class CreditsSectionDTO
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("Entry")]
        public List<CreditsEntryDTO> Entries { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();

        [XmlElement("Image")]
        public List<CreditsImageDTO> Images { get; set; } = new();

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;
        public bool ShouldSerializeImages() => Images != null && Images.Count > 0;

        public bool HasContent => Entries?.Count > 0;
        public bool HasFormatting => EmptyLines?.Count > 0;
    }

    public class CreditsEntryDTO
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;

        public bool HasFormatting => EmptyLines?.Count > 0;
    }

    public class CreditsEmptyLineDTO
    {
        // 空元素，用于格式化
    }

    public class CreditsLoadFromFileDTO
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("PlatformSpecific")]
        public string? PlatformSpecific { get; set; }

        [XmlAttribute("ConsoleSpecific")]
        public string? ConsoleSpecific { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializePlatformSpecific() => !string.IsNullOrEmpty(PlatformSpecific);
        public bool ShouldSerializeConsoleSpecific() => !string.IsNullOrEmpty(ConsoleSpecific);

        // 业务逻辑方法
        public bool IsPlatformSpecific => !string.IsNullOrEmpty(PlatformSpecific);
        public bool IsConsoleSpecific => !string.IsNullOrEmpty(ConsoleSpecific);
    }

    public class CreditsImageDTO
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
}