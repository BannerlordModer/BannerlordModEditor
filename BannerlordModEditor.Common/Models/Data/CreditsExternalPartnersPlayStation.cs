using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Credits")]
    public class CreditsExternalPartnersPlayStation
    {
        [XmlElement("Category")]
        public List<CreditsExternalPartnersPlayStationCategory> Category { get; set; } = new();

        public bool ShouldSerializeCategory() => Category?.Count > 0;
    }

    public class CreditsExternalPartnersPlayStationCategory
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("Section")]
        public List<CreditsExternalPartnersPlayStationSection> Section { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<EmptyLine> EmptyLine { get; set; } = new();

        public bool ShouldSerializeSection() => Section?.Count > 0;
        public bool ShouldSerializeEmptyLine() => EmptyLine?.Count > 0;
    }

    public class CreditsExternalPartnersPlayStationSection
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }

        [XmlElement("Entry")]
        public List<CreditsExternalPartnersPlayStationEntry> Entry { get; set; } = new();

        [XmlElement("EmptyLine")]
        public List<EmptyLine> EmptyLine { get; set; } = new();

        public bool ShouldSerializeEntry() => Entry?.Count > 0;
        public bool ShouldSerializeEmptyLine() => EmptyLine?.Count > 0;
    }

    public class CreditsExternalPartnersPlayStationEntry
    {
        [XmlAttribute("Text")]
        public string? Text { get; set; }
    }

    public class EmptyLine { }
}
