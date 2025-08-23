using System.IO;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;

namespace BannerlordModEditor.XmlAdaptationChecker.Validators
{
    /// <summary>
    /// 配置验证器实现
    /// </summary>
    public class ConfigurationValidator : IConfigurationValidator
    {
        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="configuration">要验证的配置</param>
        /// <returns>验证结果</returns>
        public ValidationResult Validate(AdaptationCheckerConfiguration configuration)
        {
            var errors = new List<string>();

            // 验证XML目录
            ValidateXmlDirectory(configuration, errors);

            // 验证模型目录
            ValidateModelDirectories(configuration, errors);

            // 验证输出格式
            ValidateOutputFormats(configuration, errors);

            // 验证数值配置
            ValidateNumericConfiguration(configuration, errors);

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

        /// <summary>
        /// 验证XML目录配置
        /// </summary>
        private void ValidateXmlDirectory(AdaptationCheckerConfiguration configuration, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(configuration.XmlDirectory))
            {
                errors.Add("XML目录路径不能为空");
                return;
            }

            if (!Directory.Exists(configuration.XmlDirectory))
            {
                errors.Add($"XML目录不存在: {configuration.XmlDirectory}");
            }
        }

        /// <summary>
        /// 验证模型目录配置
        /// </summary>
        private void ValidateModelDirectories(AdaptationCheckerConfiguration configuration, List<string> errors)
        {
            if (configuration.ModelDirectories == null || configuration.ModelDirectories.Count == 0)
            {
                errors.Add("至少需要指定一个模型目录");
                return;
            }

            foreach (var directory in configuration.ModelDirectories)
            {
                if (!Directory.Exists(directory))
                {
                    errors.Add($"模型目录不存在: {directory}");
                }
            }
        }

        /// <summary>
        /// 验证输出格式配置
        /// </summary>
        private void ValidateOutputFormats(AdaptationCheckerConfiguration configuration, List<string> errors)
        {
            if (configuration.OutputFormats == null || configuration.OutputFormats.Count == 0)
            {
                errors.Add("至少需要指定一种输出格式");
            }
        }

        /// <summary>
        /// 验证数值配置
        /// </summary>
        private void ValidateNumericConfiguration(AdaptationCheckerConfiguration configuration, List<string> errors)
        {
            if (configuration.MaxParallelism <= 0)
            {
                errors.Add("最大并行度必须大于0");
            }

            if (configuration.FileSizeThreshold < 0)
            {
                errors.Add("文件大小阈值不能为负数");
            }
        }
    }
}
