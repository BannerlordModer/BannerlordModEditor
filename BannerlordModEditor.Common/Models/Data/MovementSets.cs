using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// 动作集合配置 - movement_sets.xml
    /// 定义角色在不同状态下的动作动画映射
    /// </summary>
    [XmlRoot("movement_sets")]
    public class MovementSets
    {
        /// <summary>
        /// 动作集合列表
        /// </summary>
        [XmlElement("movement_set")]
        public List<MovementSet> MovementSet { get; set; } = new List<MovementSet>();
    }

    /// <summary>
    /// 单个动作集合定义
    /// </summary>
    public class MovementSet
    {
        /// <summary>
        /// 动作集合唯一标识符
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 空闲状态动作
        /// </summary>
        [XmlAttribute("idle")]
        public string? Idle { get; set; }

        /// <summary>
        /// 向前移动动作
        /// </summary>
        [XmlAttribute("forward")]
        public string? Forward { get; set; }

        /// <summary>
        /// 向后移动动作
        /// </summary>
        [XmlAttribute("backward")]
        public string? Backward { get; set; }

        /// <summary>
        /// 向右移动动作
        /// </summary>
        [XmlAttribute("right")]
        public string? Right { get; set; }

        /// <summary>
        /// 右后移动动作
        /// </summary>
        [XmlAttribute("right_back")]
        public string? RightBack { get; set; }

        /// <summary>
        /// 向左移动动作
        /// </summary>
        [XmlAttribute("left")]
        public string? Left { get; set; }

        /// <summary>
        /// 左后移动动作
        /// </summary>
        [XmlAttribute("left_back")]
        public string? LeftBack { get; set; }

        /// <summary>
        /// 从左到右转身动作
        /// </summary>
        [XmlAttribute("left_to_right")]
        public string? LeftToRight { get; set; }

        /// <summary>
        /// 从右到左转身动作
        /// </summary>
        [XmlAttribute("right_to_left")]
        public string? RightToLeft { get; set; }

        /// <summary>
        /// 旋转动作
        /// </summary>
        [XmlAttribute("rotate")]
        public string? Rotate { get; set; }

        /// <summary>
        /// 向前移动附加动作
        /// </summary>
        [XmlAttribute("forward_adder")]
        public string? ForwardAdder { get; set; }

        /// <summary>
        /// 向后移动附加动作
        /// </summary>
        [XmlAttribute("backward_adder")]
        public string? BackwardAdder { get; set; }

        /// <summary>
        /// 向右移动附加动作
        /// </summary>
        [XmlAttribute("right_adder")]
        public string? RightAdder { get; set; }

        /// <summary>
        /// 右后移动附加动作
        /// </summary>
        [XmlAttribute("right_back_adder")]
        public string? RightBackAdder { get; set; }

        /// <summary>
        /// 向左移动附加动作
        /// </summary>
        [XmlAttribute("left_adder")]
        public string? LeftAdder { get; set; }

        /// <summary>
        /// 左后移动附加动作
        /// </summary>
        [XmlAttribute("left_back_adder")]
        public string? LeftBackAdder { get; set; }

        /// <summary>
        /// 从左到右转身附加动作
        /// </summary>
        [XmlAttribute("left_to_right_adder")]
        public string? LeftToRightAdder { get; set; }

        /// <summary>
        /// 从右到左转身附加动作
        /// </summary>
        [XmlAttribute("right_to_left_adder")]
        public string? RightToLeftAdder { get; set; }

        // ShouldSerialize方法控制可选属性的序列化
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeIdle() => !string.IsNullOrEmpty(Idle);
        public bool ShouldSerializeForward() => !string.IsNullOrEmpty(Forward);
        public bool ShouldSerializeBackward() => !string.IsNullOrEmpty(Backward);
        public bool ShouldSerializeRight() => !string.IsNullOrEmpty(Right);
        public bool ShouldSerializeRightBack() => !string.IsNullOrEmpty(RightBack);
        public bool ShouldSerializeLeft() => !string.IsNullOrEmpty(Left);
        public bool ShouldSerializeLeftBack() => !string.IsNullOrEmpty(LeftBack);
        public bool ShouldSerializeLeftToRight() => !string.IsNullOrEmpty(LeftToRight);
        public bool ShouldSerializeRightToLeft() => !string.IsNullOrEmpty(RightToLeft);
        public bool ShouldSerializeRotate() => !string.IsNullOrEmpty(Rotate);
        public bool ShouldSerializeForwardAdder() => !string.IsNullOrEmpty(ForwardAdder);
        public bool ShouldSerializeBackwardAdder() => !string.IsNullOrEmpty(BackwardAdder);
        public bool ShouldSerializeRightAdder() => !string.IsNullOrEmpty(RightAdder);
        public bool ShouldSerializeRightBackAdder() => !string.IsNullOrEmpty(RightBackAdder);
        public bool ShouldSerializeLeftAdder() => !string.IsNullOrEmpty(LeftAdder);
        public bool ShouldSerializeLeftBackAdder() => !string.IsNullOrEmpty(LeftBackAdder);
        public bool ShouldSerializeLeftToRightAdder() => !string.IsNullOrEmpty(LeftToRightAdder);
        public bool ShouldSerializeRightToLeftAdder() => !string.IsNullOrEmpty(RightToLeftAdder);
    }
} 