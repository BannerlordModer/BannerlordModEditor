using System.IO;
using System.Text.Json;
using BannerlordModEditor.XmlAdaptationChecker.Core;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;

namespace BannerlordModEditor.XmlAdaptationChecker.Services
{
    /// <summary>
    /// 输出格式服务实现
    /// </summary>
    public class OutputFormatService : IOutputFormatService
    {
        /// <summary>
        /// 输出适配检查结果
        /// </summary>
        /// <param name="result">检查结果</param>
        /// <param name="format">输出格式</param>
        /// <param name="outputPath">输出路径（可选）</param>
        /// <returns>输出内容</returns>
        public async Task<string> OutputResultAsync(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result, OutputFormat format, string? outputPath = null)
        {
            var content = format switch
            {
                OutputFormat.Markdown => GenerateMarkdownOutput(result),
                OutputFormat.Csv => GenerateCsvOutput(result),
                OutputFormat.Json => GenerateJsonOutput(result),
                OutputFormat.Console => GenerateConsoleOutput(result),
                _ => throw new NotSupportedException($"不支持的输出格式: {format}")
            };

            if (!string.IsNullOrEmpty(outputPath))
            {
                await File.WriteAllTextAsync(outputPath, content);
            }

            return content;
        }

        /// <summary>
        /// 生成Markdown格式输出
        /// </summary>
        private string GenerateMarkdownOutput(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("# XML适配状态报告");
            sb.AppendLine();
            sb.AppendLine($"**检查时间**: {result.CheckTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"**总文件数**: {result.TotalFiles}");
            sb.AppendLine($"**已适配**: {result.AdaptedFiles}");
            sb.AppendLine($"**未适配**: {result.UnadaptedFiles}");
            sb.AppendLine($"**适配率**: {result.AdaptationRate:F1}%");
            sb.AppendLine();

            // 统计表格
            sb.AppendLine("## 统计信息");
            sb.AppendLine();
            sb.AppendLine("| 指标 | 数值 |");
            sb.AppendLine("|------|------|");
            sb.AppendLine($"| 总文件数 | {result.TotalFiles} |");
            sb.AppendLine($"| 已适配 | {result.AdaptedFiles} |");
            sb.AppendLine($"| 未适配 | {result.UnadaptedFiles} |");
            sb.AppendLine($"| 适配率 | {result.AdaptationRate:F1}% |");
            sb.AppendLine();

            // 未适配文件详情
            if (result.UnadaptedFiles > 0)
            {
                sb.AppendLine("## 未适配文件");
                sb.AppendLine();
                sb.AppendLine("### 按复杂度分类");
                sb.AppendLine();

                foreach (var complexity in new[] { AdaptationComplexity.Large, AdaptationComplexity.Complex, AdaptationComplexity.Medium, AdaptationComplexity.Simple })
                {
                    var files = result.UnadaptedFileInfos.Where(f => f.Complexity == complexity).ToList();
                    if (files.Any())
                    {
                        sb.AppendLine($"#### {complexity} 复杂度 ({files.Count} 个文件)");
                        sb.AppendLine();
                        sb.AppendLine("| 文件名 | 文件大小 | 预期模型名 |");
                        sb.AppendLine("|--------|----------|------------|");

                        foreach (var file in files)
                        {
                            sb.AppendLine($"| {file.FileName} | {FormatFileSize(file.FileSize)} | {file.ExpectedModelName} |");
                        }
                        sb.AppendLine();
                    }
                }
            }

            // 错误信息
            if (result.Errors.Any())
            {
                sb.AppendLine("## 错误信息");
                sb.AppendLine();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine($"- {error}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成CSV格式输出
        /// </summary>
        private string GenerateCsvOutput(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result)
        {
            var sb = new System.Text.StringBuilder();

            // 头部信息
            sb.AppendLine("XML适配状态报告");
            sb.AppendLine($"检查时间,{result.CheckTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"总文件数,{result.TotalFiles}");
            sb.AppendLine($"已适配,{result.AdaptedFiles}");
            sb.AppendLine($"未适配,{result.UnadaptedFiles}");
            sb.AppendLine($"适配率,{result.AdaptationRate:F1}%");
            sb.AppendLine();

            // 未适配文件详情
            if (result.UnadaptedFiles > 0)
            {
                sb.AppendLine("未适配文件详情");
                sb.AppendLine("文件名,文件大小,复杂度,预期模型名,完整路径");

                foreach (var file in result.UnadaptedFileInfos)
                {
                    sb.AppendLine($"{file.FileName},{file.FileSize},{file.Complexity},{file.ExpectedModelName},{file.FullPath}");
                }
            }

            // 错误信息
            if (result.Errors.Any())
            {
                sb.AppendLine();
                sb.AppendLine("错误信息");
                sb.AppendLine("错误消息");
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成JSON格式输出
        /// </summary>
        private string GenerateJsonOutput(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result)
        {
            var jsonResult = new
            {
                CheckTime = result.CheckTime,
                TotalFiles = result.TotalFiles,
                AdaptedFiles = result.AdaptedFiles,
                UnadaptedFiles = result.UnadaptedFiles,
                AdaptationRate = result.AdaptationRate,
                UnadaptedFileDetails = result.UnadaptedFileInfos.Select(f => new
                {
                    f.FileName,
                    f.FileSize,
                    FileSizeFormatted = FormatFileSize(f.FileSize),
                    f.Complexity,
                    f.ExpectedModelName,
                    f.FullPath
                }),
                AdaptedFileDetails = result.AdaptedFileInfos.Select(f => new
                {
                    f.FileName,
                    f.FileSize,
                    FileSizeFormatted = FormatFileSize(f.FileSize),
                    f.ModelName,
                    f.FullPath
                }),
                Errors = result.Errors
            };

            return JsonSerializer.Serialize(jsonResult, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        /// <summary>
        /// 生成控制台格式输出
        /// </summary>
        private string GenerateConsoleOutput(BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker.AdaptationCheckResult result)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("XML适配状态检查结果");
            sb.AppendLine("=".PadRight(50, '='));
            sb.AppendLine($"检查时间: {result.CheckTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"总文件数: {result.TotalFiles}");
            sb.AppendLine($"已适配: {result.AdaptedFiles}");
            sb.AppendLine($"未适配: {result.UnadaptedFiles}");
            sb.AppendLine($"适配率: {result.AdaptationRate:F1}%");
            sb.AppendLine();

            if (result.UnadaptedFiles > 0)
            {
                sb.AppendLine("未适配文件:");
                sb.AppendLine("-".PadRight(50, '-'));

                foreach (var file in result.UnadaptedFileInfos.Take(10))
                {
                    sb.AppendLine($"  {file.FileName} ({FormatFileSize(file.FileSize)}) - {file.Complexity}");
                }

                if (result.UnadaptedFiles > 10)
                {
                    sb.AppendLine($"  ... 还有 {result.UnadaptedFiles - 10} 个文件");
                }
            }

            if (result.Errors.Any())
            {
                sb.AppendLine();
                sb.AppendLine("错误信息:");
                sb.AppendLine("-".PadRight(50, '-'));
                foreach (var error in result.Errors)
                {
                    sb.AppendLine($"  {error}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:F1} {sizes[order]}";
        }
    }
}
