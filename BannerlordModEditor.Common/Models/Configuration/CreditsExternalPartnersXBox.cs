using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Configuration
{
    [XmlRoot("Credits")]
    public class CreditsExternalPartnersXBox
    {
        [XmlElement("Category")]
        public CreditsCategory? Category { get; set; }

        public bool ShouldSerializeCategory() => Category != null;
    }

    public class CreditsCategory
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("Entry")]
        public CreditsEntry[]? Entry { get; set; }

        [XmlElement("EmptyLine")]
        public CreditsEmptyLine[]? EmptyLine { get; set; }

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
        public bool ShouldSerializeEntry() => Entry != null && Entry.Length > 0;
        public bool ShouldSerializeEmptyLine() => EmptyLine != null && EmptyLine.Length > 0;
    }

    public class CreditsEntry
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }

    public class CreditsEmptyLine
    {
        // 空行元素，无需属性
    }
}