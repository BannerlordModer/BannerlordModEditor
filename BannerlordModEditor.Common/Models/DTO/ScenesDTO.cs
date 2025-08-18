using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 场景配置的数据传输对象 - scenes.xml
    /// 基于ScenesBase Data模型，用于DO/DTO架构模式转换
    /// </summary>
    [XmlRoot("base")]
    public class ScenesDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("sites")]
        public SitesDTO Sites { get; set; } = new SitesDTO();
    }

    public class SitesDTO
    {
        [XmlElement("site")]
        public List<SiteDTO> SiteList { get; set; } = new List<SiteDTO>();
    }

    public class SiteDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
}