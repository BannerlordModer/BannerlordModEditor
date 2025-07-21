using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    /// <summary>
    /// BeforeTransparentsGraph 是 PostfxGraphsBase 的别名，用于 before_transparents_graph.xml
    /// </summary>
    [XmlRoot("base")]
    public class BeforeTransparentsGraph : PostfxGraphsBase
    {
        // 继承所有功能，无需额外实现
    }
}