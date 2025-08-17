using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Layouts
{
    /// <summary>
    /// 骨骼布局配置的领域对象
    /// 对应skeletons_layout.xml文件
    /// </summary>
    [XmlRoot("base")]
    public class SkeletonsLayoutDO : LayoutsBaseDO
    {
        // 继承自LayoutsBaseDO的所有属性和方法
        // 这个类主要提供类型安全和特定的业务逻辑
        
        public SkeletonsLayoutDO()
        {
            // 设置默认值
            Type = "string";
            if (Layouts != null && Layouts.LayoutList.Count > 0)
            {
                var layout = Layouts.LayoutList[0];
                layout.Class = "skeleton";
                layout.Version = "0.1";
                layout.XmlTag = "skeletons.skeleton";
                layout.NameAttribute = "name";
                layout.UseInTreeview = "true";
            }
        }
    }
}