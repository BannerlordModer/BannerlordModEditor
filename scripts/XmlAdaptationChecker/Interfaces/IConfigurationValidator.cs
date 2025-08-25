namespace BannerlordModEditor.XmlAdaptationChecker.Interfaces
{
    /// <summary>
    /// 配置验证结果
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息列表
        /// </summary>
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 配置验证器接口
    /// </summary>
    public interface IConfigurationValidator
    {
        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="configuration">要验证的配置</param>
        /// <returns>验证结果</returns>
        ValidationResult Validate(AdaptationCheckerConfiguration configuration);
    }
}
