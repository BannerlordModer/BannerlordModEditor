using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Credits")]
    public class CreditsXmlModel
    {
        [XmlElement("Category")]
        public List<CreditsCategory> Categories { get; set; }

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFile> LoadFromFile { get; set; }

        public bool ShouldSerializeCategories() => Categories != null && Categories.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile != null && LoadFromFile.Count > 0;
    }

    public class CreditsCategory
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Section")]
        public List<CreditsSection> Sections { get; set; }

        [XmlElement("Entry")]
        public List<CreditsEntry> Entries { get; set; }

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLine> EmptyLines { get; set; }

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFile> LoadFromFile { get; set; }

        public bool ShouldSerializeSections() => Sections != null && Sections.Count > 0;
        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile != null && LoadFromFile.Count > 0;
    }

    public class CreditsSection
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Entry")]
        public List<CreditsEntry> Entries { get; set; }

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLine> EmptyLines { get; set; }

        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;
    }

    public class CreditsEntry
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }
    }

    public class CreditsEmptyLine
    {
        // 空节点，无属性
    }

    public class CreditsLoadFromFile
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("PlatformSpecific")]
        public string PlatformSpecific { get; set; }

        [XmlAttribute("ConsoleSpecific")]
        public string ConsoleSpecific { get; set; }
    }
}