using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Models.DO.Layouts
{
    /// <summary>
    /// 动画布局配置的领域对象
    /// 基于LayoutsBaseDO的通用结构
    /// </summary>
    [XmlRoot("base")]
    public class AnimationsLayoutDO : LayoutsBaseDO
    {
        // 继承所有基础属性和方法
        // 这个类作为AnimationsLayout的领域对象标识
        
        public AnimationsLayoutDO()
        {
            // 设置默认值
            Type = "string";
        }
    }
}