using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    public class CreditsItemBase
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }

    public class CreditsCategoryLegal : CreditsItemBase
    {
        [XmlElement("Section", typeof(CreditsSectionLegal))]
        [XmlElement("Entry", typeof(CreditsEntryLegal))]
        [XmlElement("Image", typeof(CreditsImageLegal))]
        [XmlElement("EmptyLine", typeof(CreditsEmptyLineLegal))]
        public List<object> Items { get; set; } = new();

        public bool ShouldSerializeItems() => Items.Count > 0;
    }

    public class CreditsSectionLegal : CreditsItemBase
    {
        [XmlElement("Entry")]
        public List<CreditsEntryLegal> Entries { get; set; } = new();
        
        public bool ShouldSerializeEntries() => Entries.Count > 0;
    }

    public class CreditsEntryLegal : CreditsItemBase { }
    public class CreditsImageLegal : CreditsItemBase { }
    public class CreditsEmptyLineLegal { }


    [XmlRoot("Credits")]
    public class CreditsLegalConsole
    {
        [XmlElement("Category")]
        public CreditsCategoryLegal? Category { get; set; }
    }
} 