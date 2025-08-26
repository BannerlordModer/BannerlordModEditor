using System.Text.RegularExpressions;

namespace BannerlordModEditor.Cli.Helpers
{
    /// <summary>
    /// CLI输出助手类，用于标准化和解析CLI命令的输出格式
    /// </summary>
    public static class CliOutputHelper
    {
        /// <summary>
        /// 标准化错误消息格式
        /// </summary>
        /// <param name="errorMessage">原始错误消息</param>
        /// <returns>标准化的错误消息</returns>
        public static string StandardizeErrorMessage(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                return "未知错误";
            }

            // 移除多余的空白字符
            var standardized = errorMessage.Trim();
            
            // 统一错误消息格式
            standardized = Regex.Replace(standardized, @"错误[：:]\s*", "错误：");
            
            // 确保错误消息以句号结尾
            if (!standardized.EndsWith("."))
            {
                standardized += ".";
            }
            
            return standardized;
        }

        /// <summary>
        /// 解析命令执行结果，判断是否成功
        /// </summary>
        /// <param name="output">命令输出</param>
        /// <param name="error">错误输出</param>
        /// <returns>是否成功</returns>
        public static bool IsCommandSuccessful(string output, string error)
        {
            // 如果有错误输出，检查是否为预期的错误消息
            if (!string.IsNullOrEmpty(error))
            {
                // 某些错误消息是预期的，不应该视为失败
                var expectedErrors = new[]
                {
                    "错误：转换失败",
                    "错误：输入文件不存在",
                    "错误：不支持的文件格式",
                    "错误：无法识别 XML 格式"
                };

                return !expectedErrors.Any(expected => error.Contains(expected));
            }

            // 检查输出中是否包含成功标识
            var successIndicators = new[]
            {
                "✓",
                "转换成功",
                "识别成功",
                "验证通过"
            };

            return successIndicators.Any(indicator => output.Contains(indicator));
        }

        /// <summary>
        /// 从输出中提取模型类型
        /// </summary>
        /// <param name="output">命令输出</param>
        /// <returns>提取的模型类型，如果没有找到则返回null</returns>
        public static string? ExtractModelType(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return null;
            }

            // 匹配"识别成功: ModelType"格式
            var match = Regex.Match(output, @"识别成功:\s*([^\s]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // 匹配"识别的模型类型: ModelType"格式
            match = Regex.Match(output, @"识别的模型类型:\s*([^\s]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// 从输出中提取文件路径
        /// </summary>
        /// <param name="output">命令输出</param>
        /// <returns>提取的文件路径列表</returns>
        public static List<string> ExtractFilePaths(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return new List<string>();
            }

            var filePaths = new List<string>();
            
            // 匹配文件路径模式
            var matches = Regex.Matches(output, @"[a-zA-Z]:\\[^""\s]+|/[^""\s]+");
            foreach (Match match in matches)
            {
                var path = match.Value;
                if (File.Exists(path) || Directory.Exists(path))
                {
                    filePaths.Add(path);
                }
            }

            return filePaths;
        }

        /// <summary>
        /// 格式化帮助信息
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="description">命令描述</param>
        /// <param name="options">选项列表</param>
        /// <returns>格式化的帮助信息</returns>
        public static string FormatHelpInfo(string commandName, string description, Dictionary<string, string> options)
        {
            var help = new System.Text.StringBuilder();
            help.AppendLine($"命令: {commandName}");
            help.AppendLine($"描述: {description}");
            help.AppendLine("选项:");
            
            foreach (var option in options)
            {
                help.AppendLine($"  {option.Key,-20} {option.Value}");
            }

            return help.ToString();
        }

        /// <summary>
        /// 验证输出格式是否符合预期
        /// </summary>
        /// <param name="output">命令输出</param>
        /// <param name="expectedPatterns">预期的模式列表</param>
        /// <returns>验证结果</returns>
        public static bool ValidateOutputFormat(string output, IEnumerable<string> expectedPatterns)
        {
            if (string.IsNullOrEmpty(output))
            {
                return !expectedPatterns.Any();
            }

            foreach (var pattern in expectedPatterns)
            {
                if (!Regex.IsMatch(output, pattern))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 清理调试信息，只保留用户需要的信息
        /// </summary>
        /// <param name="output">原始输出</param>
        /// <returns>清理后的输出</returns>
        public static string CleanDebugInfo(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return output;
            }

            // 移除调试信息行
            var lines = output.Split('\n');
            var cleanedLines = lines.Where(line => !line.Trim().StartsWith("调试:"));
            
            return string.Join('\n', cleanedLines);
        }
    }
}