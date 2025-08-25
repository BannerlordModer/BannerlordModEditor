using BannerlordModEditor.XmlAdaptationChecker;

namespace BannerlordModEditor.XmlAdaptationChecker
{
    /// <summary>
    /// XML适配检查器主程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns>退出代码</returns>
        public static async Task<int> Main(string[] args)
        {
            return await AdaptationCheckerCLI.RunAsync(args);
        }
    }
}
