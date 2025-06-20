using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class Scenes
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlArray("sites")]
        [XmlArrayItem("site")]
        public Site[]? Sites { get; set; }
    }

    public class Site
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        // The XML contains many other optional attributes on the <site> element.
        // For a minimal model, we can omit them, but a complete model would have them.
        // e.g., terrain, editor_size_x, editor_size_y, etc.
        // For now, we will stick to the required fields for the round-trip test.
    }
} 