using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("Credits")]
    public class CreditsDO
    {
        [XmlElement("Category")]
        public List<CreditsCategoryDO> Categories { get; set; } = new();

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFileDO> LoadFromFile { get; set; } = new();

        // 精确控制序列化行为
        public bool ShouldSerializeCategories() => Categories?.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile?.Count > 0;
    }

    public class CreditsCategoryDO
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Section")]
        public List<CreditsSectionDO> Sections { get; set; } = new();

        [XmlElement("Entry")]
        public List<CreditsEntryDO> Entries { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDO> EmptyLines { get; set; } = new();

        [XmlElement("LoadFromFile")]
        public List<CreditsLoadFromFileDO> LoadFromFile { get; set; } = new();

        [XmlElement("Image")]
        public List<CreditsImageDO> Images { get; set; } = new();

        // 严格按照XML定义的顺序控制序列化
        public bool ShouldSerializeSections() => Sections?.Count > 0;
        public bool ShouldSerializeEntries() => Entries?.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines?.Count > 0;
        public bool ShouldSerializeLoadFromFile() => LoadFromFile?.Count > 0;
        public bool ShouldSerializeImages() => Images?.Count > 0;
    }

    public class CreditsSectionDO
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("Entry")]
        public List<CreditsEntryDO> Entries { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDO> EmptyLines { get; set; } = new();

        public bool ShouldSerializeEntries() => Entries?.Count > 0;
        public bool ShouldSerializeEmptyLines() => EmptyLines?.Count > 0;
    }

    public class CreditsEntryDO
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlElement("EmptyLine")]
        public List<CreditsEmptyLineDO> EmptyLines { get; set; } = new();

        public bool ShouldSerializeEmptyLines() => EmptyLines?.Count > 0;
    }

    public class CreditsEmptyLineDO
    {
        // 空元素，用于格式化
    }

    public class CreditsLoadFromFileDO
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("PlatformSpecific")]
        public string PlatformSpecific { get; set; }

        [XmlAttribute("ConsoleSpecific")]
        public string ConsoleSpecific { get; set; }
    }

    public class CreditsImageDO
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }
    }
}