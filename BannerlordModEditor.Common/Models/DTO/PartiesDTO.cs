using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 队伍配置的数据传输对象 - parties.xml
    /// 基于PartiesBase Data模型，用于DO/DTO架构模式转换
    /// </summary>
    [XmlRoot("base")]
    public class PartiesDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("parties")]
        public PartiesList Parties { get; set; } = new PartiesList();

        [XmlIgnore]
        public bool HasParties { get; set; } = false;

        public bool ShouldSerializeParties() => HasParties;
    }

    public class PartiesList
    {
        [XmlElement("party")]
        public List<PartyDTO> PartyList { get; set; } = new List<PartyDTO>();
    }

    public class PartyDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("flags")]
        public string Flags { get; set; } = string.Empty;

        [XmlAttribute("party_template")]
        public string PartyTemplate { get; set; } = string.Empty;

        [XmlAttribute("position")]
        public string Position { get; set; } = string.Empty;

        [XmlAttribute("average_bearing_rot")]
        public string AverageBearingRot { get; set; } = string.Empty;

        [XmlElement("field")]
        public List<FieldDTO> FieldList { get; set; } = new List<FieldDTO>();
    }

    public class FieldDTO
    {
        [XmlAttribute("field_name")]
        public string FieldName { get; set; } = string.Empty;

        [XmlAttribute("field_value")]
        public string FieldValue { get; set; } = string.Empty;
    }
}