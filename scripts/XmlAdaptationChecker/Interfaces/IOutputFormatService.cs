using BannerlordModEditor.XmlAdaptationChecker.Core;

namespace BannerlordModEditor.XmlAdaptationChecker.Interfaces
{
    /// <summary>
    /// 输出格式服务接口
    /// </summary>
    public interface IOutputFormatService
    {
        /// <summary>
        /// 输出适配检查结果
        /// </summary>
        /// <param name="result">检查结果</param>
        /// <param name="format">输出格式</param>
        /// <param name="outputPath">输出路径（可选）</param>
        /// <returns>输出内容</returns>
        Task<string> OutputResultAsync(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result, OutputFormat format, string? outputPath = null);
    }
}
