using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// 精确映射 CreditsExternalPartnersPC.xml 的根节点结构。
    /// </summary>
    [XmlRoot("Credits")]
    public class CreditsExternalPartnersPC
    {
        // 空结构，无属性、无子元素
        public bool ShouldSerialize() => false;
    }
}