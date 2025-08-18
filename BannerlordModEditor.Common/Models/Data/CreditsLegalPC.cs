using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Credits")]
    public class CreditsLegalPC
    {
        [XmlElement("Category")]
        public List<Category> Categories { get; set; }

        public bool ShouldSerializeCategories() => Categories != null && Categories.Count > 0;
    }

    public class Category
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Section")]
        public List<Section> Sections { get; set; }

        [XmlElement("Entry")]
        public List<Entry> Entries { get; set; }

        [XmlElement("Image")]
        public List<Image> Images { get; set; }

        [XmlElement("EmptyLine")]
        public List<CreditsLegalPCEmptyLine> EmptyLines { get; set; }

        public bool ShouldSerializeSections() => Sections != null && Sections.Count > 0;
        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
        public bool ShouldSerializeImages() => Images != null && Images.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines != null && EmptyLines.Count > 0;
    }

    public class Section
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Entry")]
        public List<Entry> Entries { get; set; }

        public bool ShouldSerializeEntries() => Entries != null && Entries.Count > 0;
    }

    public class Entry
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        public bool ShouldSerializeText() => Text != null;
    }

    public class Image
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        public bool ShouldSerializeText() => Text != null;
    }

    public class CreditsLegalPCEmptyLine
    {
        // 空节点，无属性
    }
}