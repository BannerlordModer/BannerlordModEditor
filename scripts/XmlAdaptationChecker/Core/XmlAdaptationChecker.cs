using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Services;
using Microsoft.Extensions.Logging;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;

namespace BannerlordModEditor.XmlAdaptationChecker.Core
{
    /// <summary>
    /// XML适配状态检查引擎
    /// </summary>
    public class XmlAdaptationChecker
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly AdaptationCheckerConfiguration _configuration;
        private readonly ILogger<XmlAdaptationChecker> _logger;
        private readonly IConfigurationValidator _configurationValidator;

        public XmlAdaptationChecker(
            IFileDiscoveryService fileDiscoveryService,
            AdaptationCheckerConfiguration configuration,
            ILogger<XmlAdaptationChecker> logger,
            IConfigurationValidator configurationValidator)
        {
            _fileDiscoveryService = fileDiscoveryService ?? throw new ArgumentNullException(nameof(fileDiscoveryService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationValidator = configurationValidator ?? throw new ArgumentNullException(nameof(configurationValidator));
        }

        /// <summary>
        /// 执行适配状态检查
        /// </summary>
        /// <returns>检查结果</returns>
        public async Task<AdaptationCheckResult> CheckAdaptationStatusAsync()
        {
            _logger.LogInformation("开始XML适配状态检查");

            try
            {
                // 验证配置
                var validationResult = _configurationValidator.Validate(_configuration);
                if (!validationResult.IsValid)
                {
                    _logger.LogError("配置验证失败: {Errors}", string.Join(", ", validationResult.Errors));
                    return new AdaptationCheckResult
                    {
                        Errors = validationResult.Errors
                    };
                }

                // 创建输出目录
                if (!string.IsNullOrWhiteSpace(_configuration.OutputDirectory))
                {
                    Directory.CreateDirectory(_configuration.OutputDirectory);
                }

                // 执行分析
                var result = await AnalyzeAdaptationStatusAsync();

                _logger.LogInformation("XML适配状态检查完成");
                _logger.LogInformation("总文件数: {TotalFiles}, 已适配: {AdaptedFiles}, 未适配: {UnadaptedFiles}, 适配率: {AdaptationRate:F1}%",
                    result.TotalFiles, result.AdaptedFiles, result.UnadaptedFiles, result.AdaptationRate);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML适配状态检查过程中发生错误");
                return new AdaptationCheckResult
                {
                    Errors = new List<string> { $"检查过程中发生错误: {ex.Message}" }
                };
            }
        }

        /// <summary>
        /// 分析适配状态
        /// </summary>
        /// <returns>分析结果</returns>
        private async Task<AdaptationCheckResult> AnalyzeAdaptationStatusAsync()
        {
            var result = new AdaptationCheckResult();

            try
            {
                // 获取所有XML文件
                var xmlFiles = GetXmlFiles();
                result.TotalFiles = xmlFiles.Count;

                // 分析每个文件
                var analysisTasks = xmlFiles.Select(file => AnalyzeFileAsync(file));

                List<FileAnalysisResult> analysisResults;
                if (_configuration.EnableParallelProcessing)
                {
                    analysisResults = (await Task.WhenAll(analysisTasks)).ToList();
                } else
                {
                    analysisResults = analysisTasks.Select(t => t.Result).ToList();
                }

                // 处理分析结果
                foreach (var analysisResult in analysisResults)
                {
                    if (analysisResult.IsAdapted)
                    {
                        result.AdaptedFiles++;
                        result.AdaptedFileInfos.Add(analysisResult.AdaptedInfo!);
                    } else
                    {
                        result.UnadaptedFiles++;
                        result.UnadaptedFileInfos.Add(analysisResult.UnadaptedInfo!);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"分析过程中发生错误: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 获取所有XML文件
        /// </summary>
        /// <returns>XML文件列表</returns>
        private List<string> GetXmlFiles()
        {
            var files = new List<string>();

            try
            {
                var searchOption = SearchOption.TopDirectoryOnly;
                files.AddRange(Directory.GetFiles(_configuration.XmlDirectory, "*.xml", searchOption));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"无法扫描XML目录: {_configuration.XmlDirectory}", ex);
            }

            // 应用排除模式
            if (_configuration.ExcludePatterns.Any())
            {
                files = files.Where(file =>
                    !_configuration.ExcludePatterns.Any(pattern =>
                        file.Contains(pattern.Replace("*", "")))).ToList();
            }

            return files;
        }

        /// <summary>
        /// 分析单个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>分析结果</returns>
        private async Task<FileAnalysisResult> AnalyzeFileAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);

            // 检查是否已适配
            var isAdapted = _fileDiscoveryService.IsFileAdapted(fileName);

            if (isAdapted)
            {
                var modelName = _fileDiscoveryService.ConvertToModelName(fileName);
                var adaptedInfo = new AdaptedFileInfo
                {
                    FileName = fileName,
                    FullPath = filePath,
                    FileSize = fileInfo.Length,
                    ModelName = modelName
                };

                return new FileAnalysisResult
                {
                    IsAdapted = true,
                    AdaptedInfo = adaptedInfo
                };
            } else
            {
                var unadaptedInfo = new UnadaptedFileInfo
                {
                    FileName = fileName,
                    FullPath = filePath,
                    FileSize = fileInfo.Length,
                    ExpectedModelName = _fileDiscoveryService.ConvertToModelName(fileName),
                    Complexity = await AnalyzeComplexityAsync(filePath)
                };

                return new FileAnalysisResult
                {
                    IsAdapted = false,
                    UnadaptedInfo = unadaptedInfo
                };
            }
        }

        /// <summary>
        /// 分析文件复杂度
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>复杂度等级</returns>
        private async Task<AdaptationComplexity> AnalyzeComplexityAsync(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);

                // 基于文件大小的初步判断
                if (fileInfo.Length > _configuration.FileSizeThreshold)
                {
                    return AdaptationComplexity.Large;
                }

                // 简单的复杂度分析
                using var stream = File.OpenRead(filePath);
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                // 计算复杂度指标
                var lineCount = content.Split('\n').Length;
                var elementCount = content.Count(c => c == '<') / 2; // 简单估算

                // 基于指标判断复杂度
                if (elementCount > 500 || lineCount > 1000)
                {
                    return AdaptationComplexity.Complex;
                } else if (elementCount > 100 || lineCount > 200)
                {
                    return AdaptationComplexity.Medium;
                } else
                {
                    return AdaptationComplexity.Simple;
                }
            }
            catch
            {
                return AdaptationComplexity.Medium; // 默认中等复杂度
            }
        }

        /// <summary>
        /// 获取未适配文件摘要
        /// </summary>
        /// <returns>未适配文件摘要</returns>
        public async Task<string> GetUnadaptedFilesSummaryAsync()
        {
            var result = await CheckAdaptationStatusAsync();

            if (result.Errors.Any())
            {
                return $"错误: {string.Join(", ", result.Errors)}";
            }

            var summary = new System.Text.StringBuilder();
            summary.AppendLine($"XML适配状态摘要 ({result.CheckTime:yyyy-MM-dd HH:mm:ss})");
            summary.AppendLine("=".PadRight(50, '='));
            summary.AppendLine($"总文件数: {result.TotalFiles}");
            summary.AppendLine($"已适配: {result.AdaptedFiles}");
            summary.AppendLine($"未适配: {result.UnadaptedFiles}");
            summary.AppendLine($"适配率: {result.AdaptationRate:F1}%");

            if (result.UnadaptedFiles > 0)
            {
                summary.AppendLine();
                summary.AppendLine("未适配文件按复杂度:");
                foreach (var complexity in new[] { AdaptationComplexity.Large, AdaptationComplexity.Complex, AdaptationComplexity.Medium, AdaptationComplexity.Simple })
                {
                    var count = result.UnadaptedFileInfos.Count(f => f.Complexity == complexity);
                    if (count > 0)
                    {
                        summary.AppendLine($"  {complexity}: {count} 个文件");
                    }
                }
            }

            return summary.ToString();
        }

        /// <summary>
        /// 文件分析结果内部类
        /// </summary>
        private class FileAnalysisResult
        {
            public bool IsAdapted { get; set; }
            public AdaptedFileInfo? AdaptedInfo { get; set; }
            public UnadaptedFileInfo? UnadaptedInfo { get; set; }
        }

        /// <summary>
        /// 已适配文件信息
        /// </summary>
        public class AdaptedFileInfo
        {
            public string FileName { get; set; } = string.Empty;
            public string FullPath { get; set; } = string.Empty;
            public long FileSize { get; set; }
            public string ModelName { get; set; } = string.Empty;
        }

        /// <summary>
        /// 未适配文件信息
        /// </summary>
        public class UnadaptedFileInfo
        {
            public string FileName { get; set; } = string.Empty;
            public string FullPath { get; set; } = string.Empty;
            public long FileSize { get; set; }
            public string ExpectedModelName { get; set; } = string.Empty;
            public AdaptationComplexity Complexity { get; set; }
        }

        /// <summary>
        /// 适配检查结果
        /// </summary>
        public class AdaptationCheckResult
        {
            public DateTime CheckTime { get; set; } = DateTime.UtcNow;
            public int TotalFiles { get; set; }
            public int AdaptedFiles { get; set; }
            public int UnadaptedFiles { get; set; }
            public double AdaptationRate => TotalFiles > 0 ? (double)AdaptedFiles / TotalFiles * 100 : 0;
            public List<UnadaptedFileInfo> UnadaptedFileInfos { get; set; } = new();
            public List<AdaptedFileInfo> AdaptedFileInfos { get; set; } = new();
            public List<string> Errors { get; set; } = new();
        }
    }
}
