using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("worldmap_color_grades")]
    public class WorldmapColorGrades
    {
        [XmlElement("color_grade_grid")]
        public ColorGradeGrid? ColorGradeGrid { get; set; }

        [XmlElement("color_grade_default")]
        public ColorGradeDefault? ColorGradeDefault { get; set; }

        [XmlElement("color_grade_night")]
        public ColorGradeNight? ColorGradeNight { get; set; }

        [XmlElement("color_grade")]
        public List<ColorGrade> ColorGrades { get; set; } = new List<ColorGrade>();
    }

    public class ColorGradeGrid
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ColorGradeDefault
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ColorGradeNight
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ColorGrade
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
} 