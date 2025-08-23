namespace BannerlordModEditor.XmlAdaptationChecker
{
    /// <summary>
    /// XML适配复杂度等级
    /// </summary>
    public enum AdaptationComplexity
    {
        /// <summary>
        /// 简单 - 小型文件，结构简单
        /// </summary>
        Simple,

        /// <summary>
        /// 中等 - 中型文件，结构适中
        /// </summary>
        Medium,

        /// <summary>
        /// 复杂 - 大型文件，结构复杂
        /// </summary>
        Complex,

        /// <summary>
        /// 超大 - 超大文件，需要特殊处理
        /// </summary>
        Large
    }
}
