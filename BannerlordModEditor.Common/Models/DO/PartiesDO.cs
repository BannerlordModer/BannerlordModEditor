using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 队伍配置的领域对象 - parties.xml
    /// 基于PartiesBase Data模型，添加DO/DTO架构模式支持
    /// </summary>
    [XmlRoot("base")]
    public class PartiesDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("parties")]
        public Parties Parties { get; set; } = new Parties();

        [XmlIgnore]
        public bool HasParties { get; set; } = false;

        public bool ShouldSerializeParties() => HasParties;
    }

    public class Parties
    {
        [XmlElement("party")]
        public List<Party> PartyList { get; set; } = new List<Party>();
    }

    public class Party
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
        public List<Field> FieldList { get; set; } = new List<Field>();
    }

    public class Field
    {
        [XmlAttribute("field_name")]
        public string FieldName { get; set; } = string.Empty;

        [XmlAttribute("field_value")]
        public string FieldValue { get; set; } = string.Empty;
    }
}