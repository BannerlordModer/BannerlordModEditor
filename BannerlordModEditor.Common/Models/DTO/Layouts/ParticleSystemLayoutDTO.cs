using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Layouts
{
    /// <summary>
    /// 粒子系统布局配置的数据传输对象
    /// 对应particle_system_layout.xml文件
    /// </summary>
    [XmlRoot("base")]
    public class ParticleSystemLayoutDTO : LayoutsBaseDTO
    {
        public ParticleSystemLayoutDTO()
        {
            Type = "string";
            if (Layouts != null && Layouts.LayoutList.Count > 0)
            {
                var layout = Layouts.LayoutList[0];
                layout.Class = "particle_system";
                layout.Version = "0.1";
                layout.XmlTag = "particle_systems.particle_system";
                layout.NameAttribute = "id";
                layout.UseInTreeview = "true";
            }
        }
    }
}