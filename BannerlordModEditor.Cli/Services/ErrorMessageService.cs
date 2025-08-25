using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Cli.Exceptions;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// 错误消息服务，提供用户友好的错误提示
    /// </summary>
    public class ErrorMessageService
    {
        private readonly Dictionary<Type, string> _errorMessages = new()
        {
            [typeof(FileNotFoundException)] = "文件不存在，请检查文件路径是否正确。",
            [typeof(DirectoryNotFoundException)] = "目录不存在，请检查目录路径是否正确。",
            [typeof(UnauthorizedAccessException)] = "没有访问权限，请检查文件权限。",
            [typeof(InvalidOperationException)] = "操作无效，请检查输入参数。",
            [typeof(ArgumentException)] = "参数无效，请检查输入的参数。",
            [typeof(XmlFormatException)] = "XML 格式错误，请检查 XML 文件格式。",
            [typeof(ExcelOperationException)] = "Excel 操作失败，请检查 Excel 文件格式。",
            [typeof(ConversionException)] = "转换失败，请检查输入数据格式。",
            [typeof(NotImplementedException)] = "该功能尚未实现。",
            [typeof(NotSupportedException)] = "不支持的操作或格式。",
            [typeof(CliCommandException)] = "命令执行失败。"
        };

        /// <summary>
        /// 获取用户友好的错误消息
        /// </summary>
        public string GetUserFriendlyMessage(Exception ex)
        {
            var baseMessage = GetBaseErrorMessage(ex);
            var suggestion = GetSuggestion(ex);
            
            return $"{baseMessage}{suggestion}";
        }

        /// <summary>
        /// 获取基础错误消息
        /// </summary>
        private string GetBaseErrorMessage(Exception ex)
        {
            if (_errorMessages.TryGetValue(ex.GetType(), out var message))
            {
                return message;
            }

            return ex.Message;
        }

        /// <summary>
        /// 获取解决建议
        /// </summary>
        private string GetSuggestion(Exception ex)
        {
            return ex switch
            {
                FileNotFoundException => " 使用 'ls' 命令检查文件是否存在。",
                DirectoryNotFoundException => " 使用 'ls' 命令检查目录是否存在。",
                UnauthorizedAccessException => " 使用 'chmod' 命令修改文件权限。",
                ArgumentException => " 使用 '--help' 命令查看正确的参数格式。",
                XmlFormatException => " 检查 XML 文件格式是否符合标准。",
                ExcelOperationException => " 确保 Excel 文件格式正确且未损坏。",
                ConversionException => " 检查输入数据格式是否与目标模型匹配。",
                InvalidOperationException => " 检查操作步骤是否正确。",
                NotImplementedException => " 该功能正在开发中，请关注后续版本。",
                NotSupportedException => " 检查是否使用了不支持的文件格式或操作。",
                _ => " 请查看详细错误信息以获取更多帮助。"
            };
        }

        /// <summary>
        /// 获取详细的错误信息（用于调试）
        /// </summary>
        public string GetDetailedError(Exception ex)
        {
            var details = new System.Text.StringBuilder();
            
            details.AppendLine($"错误类型: {ex.GetType().Name}");
            details.AppendLine($"错误消息: {ex.Message}");
            details.AppendLine($"用户友好提示: {GetUserFriendlyMessage(ex)}");
            
            if (ex.InnerException != null)
            {
                details.AppendLine($"内部错误: {ex.InnerException.Message}");
            }
            
            details.AppendLine($"堆栈跟踪: {ex.StackTrace}");
            
            return details.ToString();
        }

        /// <summary>
        /// 获取文件格式错误的具体信息
        /// </summary>
        public string GetFileFormatError(string filePath, string expectedFormat)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            
            return extension switch
            {
                ".xlsx" => $"Excel 文件格式可能有问题。请确保文件是有效的 .xlsx 格式，并且未损坏。",
                ".xls" => "不支持 .xls 格式，请使用 .xlsx 格式的 Excel 文件。",
                ".xml" => $"XML 文件格式可能有问题。请检查 XML 语法是否正确，并且符合 {expectedFormat} 格式。",
                _ => $"不支持的文件格式: {extension}。支持的格式有: .xlsx, .xml"
            };
        }

        /// <summary>
        /// 获取模型类型相关的错误信息
        /// </summary>
        public string GetModelError(string modelType, IEnumerable<string> availableModels)
        {
            var message = $"不支持的模型类型: {modelType}。";
            
            // 查找相似的模型类型
            var similarModels = availableModels
                .Where(m => m.Contains(modelType, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            if (similarModels.Any())
            {
                message += " 您是否指的是: " + string.Join(", ", similarModels.Take(3));
            }
            
            message += $" 使用 'list-models' 命令查看所有支持的模型类型。";
            
            return message;
        }

        /// <summary>
        /// 获取转换建议
        /// </summary>
        public string GetConversionSuggestion(string inputFormat, string outputFormat, string? modelType)
        {
            var suggestion = new System.Text.StringBuilder();
            
            if (inputFormat == ".xlsx" && outputFormat == ".xml")
            {
                suggestion.AppendLine("Excel 转 XML 转换建议:");
                suggestion.AppendLine("1. 确保 Excel 文件的第一行是表头");
                suggestion.AppendLine("2. 表头名称应与目标模型的属性名称匹配");
                suggestion.AppendLine("3. 使用 '--validate' 参数先验证格式");
                if (!string.IsNullOrEmpty(modelType))
                {
                    suggestion.AppendLine($"4. 确保使用正确的模型类型: {modelType}");
                }
            }
            else if (inputFormat == ".xml" && outputFormat == ".xlsx")
            {
                suggestion.AppendLine("XML 转 Excel 转换建议:");
                suggestion.AppendLine("1. 使用 'recognize' 命令先识别 XML 格式");
                suggestion.AppendLine("2. 确保 XML 文件格式正确");
                suggestion.AppendLine("3. 检查 XML 文件是否有正确的根元素");
            }
            
            return suggestion.ToString();
        }

        /// <summary>
        /// 获取性能建议
        /// </summary>
        public string GetPerformanceSuggestion(int fileSizeKB, int recordCount)
        {
            var suggestion = new System.Text.StringBuilder();
            
            if (fileSizeKB > 10 * 1024) // 10MB
            {
                suggestion.AppendLine("文件较大，建议:");
                suggestion.AppendLine("1. 分批处理数据");
                suggestion.AppendLine("2. 关闭其他应用程序以释放内存");
                suggestion.AppendLine("3. 确保有足够的磁盘空间");
            }
            
            if (recordCount > 10000)
            {
                suggestion.AppendLine("数据量较大，建议:");
                suggestion.AppendLine("1. 分批处理数据");
                suggestion.AppendLine("2. 考虑使用更高效的存储格式");
                suggestion.AppendLine("3. 优化数据结构");
            }
            
            return suggestion.ToString();
        }
    }
}