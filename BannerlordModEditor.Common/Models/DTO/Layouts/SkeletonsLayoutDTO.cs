using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Layouts
{
    /// <summary>
    /// 骨骼布局配置的数据传输对象
    /// 对应skeletons_layout.xml文件
    /// </summary>
    [XmlRoot("base")]
    public class SkeletonsLayoutDTO : LayoutsBaseDTO
    {
        // 继承自LayoutsBaseDTO的所有属性和方法
        // 这个类主要提供类型安全
        
        public SkeletonsLayoutDTO()
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