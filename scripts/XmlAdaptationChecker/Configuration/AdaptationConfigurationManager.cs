using System.IO;
using Microsoft.Extensions.Configuration;

namespace BannerlordModEditor.XmlAdaptationChecker.Configuration
{
    /// <summary>
    /// 适配检查器配置管理器
    /// </summary>
    public class AdaptationConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public AdaptationConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// 从配置文件加载配置
        /// </summary>
        /// <returns>适配检查器配置</returns>
        public AdaptationCheckerConfiguration LoadConfiguration()
        {
            var config = new AdaptationCheckerConfiguration();

            // 基础配置
            config.XmlDirectory = _configuration["XmlDirectory"] ??
                Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "BannerlordModEditor.Common.Tests", "TestData");

            config.OutputDirectory = _configuration["OutputDirectory"] ??
                Path.Combine(Directory.GetCurrentDirectory(), "output");

            // 模型目录配置
            var modelDirs = _configuration.GetSection("ModelDirectories").Get<List<string>>();
            if (modelDirs != null && modelDirs.Count > 0)
            {
                config.ModelDirectories = modelDirs;
            } else
            {
                // 默认模型目录
                config.ModelDirectories = new List<string>
                {
                    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "BannerlordModEditor.Common", "Models"),
                    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "BannerlordModEditor.Common", "Models", "DO"),
                    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "BannerlordModEditor.Common", "Models", "DTO")
                };
            }

            // 输出格式配置
            var outputFormats = _configuration.GetSection("OutputFormats").Get<List<string>>();
            if (outputFormats != null && outputFormats.Count > 0)
            {
                config.OutputFormats = outputFormats
                    .Select(f => Enum.Parse<OutputFormat>(f, true))
                    .ToList();
            } else
            {
                // 默认输出格式
                config.OutputFormats = new List<OutputFormat>
                {
                    OutputFormat.Console,
                    OutputFormat.Markdown,
                    OutputFormat.Csv
                };
            }

            // 其他配置
            config.VerboseLogging = _configuration.GetValue<bool>("VerboseLogging", false);
            config.EnableParallelProcessing = _configuration.GetValue<bool>("EnableParallelProcessing", true);
            config.MaxParallelism = _configuration.GetValue<int>("MaxParallelism", Environment.ProcessorCount);
            config.FileSizeThreshold = _configuration.GetValue<long>("FileSizeThreshold", 1024 * 1024);
            config.AnalyzeComplexity = _configuration.GetValue<bool>("AnalyzeComplexity", true);
            config.GenerateStatistics = _configuration.GetValue<bool>("GenerateStatistics", true);

            // 排除模式配置
            var excludePatterns = _configuration.GetSection("ExcludePatterns").Get<List<string>>();
            if (excludePatterns != null)
            {
                config.ExcludePatterns = excludePatterns;
            }

            return config;
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        public static void CreateDefaultConfig(string configPath)
        {
            var defaultConfig = new
            {
                XmlDirectory = "../TestData",
                OutputDirectory = "./output",
                ModelDirectories = new[]
                {
                    "../BannerlordModEditor.Common/Models",
                    "../BannerlordModEditor.Common/Models/DO",
                    "../BannerlordModEditor.Common/Models/DTO"
                },
                OutputFormats = new[] { "Console", "Markdown", "Csv", "Json" },
                VerboseLogging = false,
                EnableParallelProcessing = true,
                MaxParallelism = Environment.ProcessorCount,
                FileSizeThreshold = 1048576,
                AnalyzeComplexity = true,
                GenerateStatistics = true,
                ExcludePatterns = new[] { "*.backup", "*.tmp" }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(defaultConfig, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });

            File.WriteAllText(configPath, json);
        }

        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果</returns>
        public static (bool IsValid, List<string> Errors) ValidateConfiguration(AdaptationCheckerConfiguration config)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(config.XmlDirectory))
            {
                errors.Add("XML目录路径不能为空");
            } else if (!Directory.Exists(config.XmlDirectory))
            {
                errors.Add($"XML目录不存在: {config.XmlDirectory}");
            }

            if (config.ModelDirectories == null || config.ModelDirectories.Count == 0)
            {
                errors.Add("至少需要指定一个模型目录");
            } else
            {
                foreach (var dir in config.ModelDirectories)
                {
                    if (!Directory.Exists(dir))
                    {
                        errors.Add($"模型目录不存在: {dir}");
                    }
                }
            }

            if (config.OutputFormats == null || config.OutputFormats.Count == 0)
            {
                errors.Add("至少需要指定一种输出格式");
            }

            if (config.MaxParallelism <= 0)
            {
                errors.Add("最大并行度必须大于0");
            }

            if (config.FileSizeThreshold < 0)
            {
                errors.Add("文件大小阈值不能为负数");
            }

            return (errors.Count == 0, errors);
        }
    }
}
