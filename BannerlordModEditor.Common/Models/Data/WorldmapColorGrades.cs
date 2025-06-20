using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("worldmap_color_grades")]
    public class WorldmapColorGrades
    {
        [XmlElement("color_grade_grid")]
        public ColorGradeSetting? Grid { get; set; }

        [XmlElement("color_grade_default")]
        public ColorGradeSetting? Default { get; set; }

        [XmlElement("color_grade_night")]
        public ColorGradeSetting? Night { get; set; }

        [XmlElement("color_grade")]
        public ColorGrade[]? Grades { get; set; }
    }

    public class ColorGradeSetting
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
    }

    public class ColorGrade
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }
    }
} 