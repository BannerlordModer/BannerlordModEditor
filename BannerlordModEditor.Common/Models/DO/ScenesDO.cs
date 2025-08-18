using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 场景配置的领域对象 - scenes.xml
    /// 基于ScenesBase Data模型，添加DO/DTO架构模式支持
    /// </summary>
    [XmlRoot("base")]
    public class ScenesDO : ScenesBase
    {
        // 继承所有基础属性和方法
        // 这个类作为Scenes的领域对象标识
        
        public ScenesDO()
        {
            // 设置默认值
            Type = "scene";
        }
    }
}