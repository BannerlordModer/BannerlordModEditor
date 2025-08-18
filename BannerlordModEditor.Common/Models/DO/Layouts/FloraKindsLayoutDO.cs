using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Models.DO.Layouts
{
    /// <summary>
    /// 植被种类布局配置的领域对象
    /// 基于LayoutsBaseDO的通用结构
    /// </summary>
    [XmlRoot("base")]
    public class FloraKindsLayoutDO : LayoutsBaseDO
    {
        // 继承所有基础属性和方法
        // 这个类作为FloraKindsLayout的领域对象标识
        
        public FloraKindsLayoutDO()
        {
            // 设置默认值
            Type = "string";
        }
    }
}