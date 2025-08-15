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

        // 使用单个列表来保持元素的原始顺序
        [XmlElement("Section", Type = typeof(CreditsSectionDO))]
        [XmlElement("Entry", Type = typeof(CreditsEntryDO))]
        [XmlElement("EmptyLine", Type = typeof(CreditsEmptyLineDO))]
        [XmlElement("LoadFromFile", Type = typeof(CreditsLoadFromFileDO))]
        [XmlElement("Image", Type = typeof(CreditsImageDO))]
        public List<object> Elements { get; set; } = new();

        // 为了向后兼容，提供便捷属性
        [XmlIgnore]
        public List<CreditsSectionDO> Sections => Elements.OfType<CreditsSectionDO>().ToList();
        
        [XmlIgnore]
        public List<CreditsEntryDO> Entries => Elements.OfType<CreditsEntryDO>().ToList();
        
        [XmlIgnore]
        public List<CreditsEmptyLineDO> EmptyLines => Elements.OfType<CreditsEmptyLineDO>().ToList();
        
        [XmlIgnore]
        public List<CreditsLoadFromFileDO> LoadFromFile => Elements.OfType<CreditsLoadFromFileDO>().ToList();
        
        [XmlIgnore]
        public List<CreditsImageDO> Images => Elements.OfType<CreditsImageDO>().ToList();

        // 严格按照XML定义的顺序控制序列化
        public bool ShouldSerializeElements() => Elements?.Count > 0;
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