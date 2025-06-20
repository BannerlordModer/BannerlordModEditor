using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("flora_groups")]
    public class FloraGroups
    {
        [XmlElement("flora_group")]
        public List<FloraGroup> FloraGroupList { get; set; } = new List<FloraGroup>();
    }

    public class FloraGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("flora_records")]
        public FloraRecords FloraRecords { get; set; } = new FloraRecords();
    }

    public class FloraRecords
    {
        [XmlElement("flora_record")]
        public List<FloraRecord> Records { get; set; } = new List<FloraRecord>();
    }

    public class FloraRecord
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("density")]
        public int Density { get; set; }
    }
} 