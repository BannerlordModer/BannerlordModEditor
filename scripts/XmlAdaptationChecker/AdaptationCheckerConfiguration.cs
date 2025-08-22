namespace BannerlordModEditor.XmlAdaptationChecker
{
    /// <summary>
    /// XML适配状态检查的配置
    /// </summary>
    public class AdaptationCheckerConfiguration
    {
        /// <summary>
        /// 要扫描的XML目录路径
        /// </summary>
        public string XmlDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 要搜索的C#模型目录
        /// </summary>
        public List<string> ModelDirectories { get; set; } = new();

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 输出格式列表
        /// </summary>
        public List<OutputFormat> OutputFormats { get; set; } = new();

        /// <summary>
        /// 是否启用详细日志
        /// </summary>
        public bool VerboseLogging { get; set; } = false;

        /// <summary>
        /// 是否启用并行处理
        /// </summary>
        public bool EnableParallelProcessing { get; set; } = true;

        /// <summary>
        /// 最大并行度
        /// </summary>
        public int MaxParallelism { get; set; } = Environment.ProcessorCount;

        /// <summary>
        /// 文件大小阈值（字节）
        /// </summary>
        public long FileSizeThreshold { get; set; } = 1024 * 1024; // 1MB

        /// <summary>
        /// 是否分析文件复杂度
        /// </summary>
        public bool AnalyzeComplexity { get; set; } = true;

        /// <summary>
        /// 是否生成统计报告
        /// </summary>
        public bool GenerateStatistics { get; set; } = true;

        /// <summary>
        /// 要排除的文件模式
        /// </summary>
        public List<string> ExcludePatterns { get; set; } = new();
    }

    /// <summary>
    /// 输出格式枚举
    /// </summary>
    public enum OutputFormat
    {
        /// <summary>
        /// 控制台输出
        /// </summary>
        Console,

        /// <summary>
        /// Markdown格式
        /// </summary>
        Markdown,

        /// <summary>
        /// CSV格式
        /// </summary>
        Csv,

        /// <summary>
        /// JSON格式
        /// </summary>
        Json
    }
}
