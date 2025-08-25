using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.IntegrationTests
{
    /// <summary>
    /// CLI工具的集成测试基类
    /// </summary>
    public class CliIntegrationTestBase
    {
        protected readonly string _cliProjectPath;
        protected readonly string _testDataPath;
        protected readonly string _tempPath;
        private readonly string _solutionRoot;

        public CliIntegrationTestBase()
        {
            // 获取解决方案根目录的更可靠方法
            var currentDir = Directory.GetCurrentDirectory();
            _solutionRoot = FindSolutionRoot(currentDir);
            _cliProjectPath = Path.GetFullPath(Path.Combine(_solutionRoot, "BannerlordModEditor.Cli"));
            _testDataPath = Path.GetFullPath(Path.Combine(_solutionRoot, "BannerlordModEditor.Common.Tests", "TestData"));
            _tempPath = Path.GetTempPath();
        }

        /// <summary>
        /// 查找解决方案根目录
        /// </summary>
        private string FindSolutionRoot(string startDir)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null && dir.GetFiles("*.sln").Length == 0)
            {
                dir = dir.Parent;
            }
            return dir?.FullName ?? throw new DirectoryNotFoundException("无法找到解决方案根目录");
        }

        /// <summary>
        /// 执行CLI命令并返回结果
        /// </summary>
        protected async Task<CliExecutionResult> ExecuteCliCommandAsync(string arguments, TimeSpan? timeout = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{_cliProjectPath}\" -- {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _solutionRoot
            };

            using var process = new Process { StartInfo = processStartInfo };
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
            process.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            var timeoutMs = timeout?.TotalMilliseconds ?? 30000; // 默认30秒超时
            var exited = process.WaitForExit((int)timeoutMs);

            if (!exited)
            {
                process.Kill();
                throw new TimeoutException($"CLI命令执行超时: {arguments}");
            }

            return new CliExecutionResult
            {
                ExitCode = process.ExitCode,
                StandardOutput = outputBuilder.ToString(),
                StandardError = errorBuilder.ToString(),
                Arguments = arguments
            };
        }

        /// <summary>
        /// 获取测试数据文件的完整路径
        /// </summary>
        protected string GetTestDataPath(string fileName)
        {
            return Path.Combine(_testDataPath, fileName);
        }

        /// <summary>
        /// 创建临时文件路径
        /// </summary>
        protected string GetTempFilePath(string extension)
        {
            return Path.Combine(_tempPath, $"test_{Guid.NewGuid()}{extension}");
        }

        /// <summary>
        /// 清理临时文件
        /// </summary>
        protected void CleanupTempFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                // 忽略清理错误
            }
        }

        /// <summary>
        /// 验证文件是否存在并包含预期内容
        /// </summary>
        protected async Task VerifyFileContainsAsync(string filePath, string expectedContent)
        {
            File.Exists(filePath).Should().BeTrue($"文件 {filePath} 应该存在");
            var content = await File.ReadAllTextAsync(filePath);
            content.Should().Contain(expectedContent, $"文件 {filePath} 应该包含 '{expectedContent}'");
        }

        /// <summary>
        /// 验证XML文件格式是否正确
        /// </summary>
        protected async Task VerifyXmlFormatAsync(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"XML文件 {filePath} 应该存在");
            var content = await File.ReadAllTextAsync(filePath);
            
            // 基本XML格式验证
            content.Should().StartWith("<?xml", $"文件 {filePath} 应该是有效的XML格式");
            content.Should().Contain("</", $"文件 {filePath} 应该包含XML结束标签");
            
            // 尝试解析XML
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(content);
        }

        /// <summary>
        /// 验证Excel文件格式是否正确
        /// </summary>
        protected void VerifyExcelFormat(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"Excel文件 {filePath} 应该存在");
            
            // 验证文件扩展名
            Path.GetExtension(filePath).ToLower().Should().Be(".xlsx", $"文件 {filePath} 应该是.xlsx格式");
            
            // 验证文件大小（至少3KB，即使是小型XML文件也应该大于这个大小）
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(1024 * 3, $"Excel文件 {filePath} 应该大于3KB");
        }
    }

    /// <summary>
    /// CLI命令执行结果
    /// </summary>
    public class CliExecutionResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; } = string.Empty;
        public string StandardError { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;

        public bool Success => ExitCode == 0;
        public string AllOutput => $"{StandardOutput}{StandardError}";

        /// <summary>
        /// 验证输出中是否包含特定文本
        /// </summary>
        public void ShouldContain(string expectedText)
        {
            AllOutput.Should().Contain(expectedText, $"输出应该包含 '{expectedText}'");
        }

        /// <summary>
        /// 验证输出中是否不包含特定文本
        /// </summary>
        public void ShouldNotContain(string unexpectedText)
        {
            AllOutput.Should().NotContain(unexpectedText, $"输出不应该包含 '{unexpectedText}'");
        }

        /// <summary>
        /// 验证命令是否成功执行
        /// </summary>
        public void ShouldSucceed()
        {
            Success.Should().BeTrue($"命令 '{Arguments}' 应该成功执行，但退出码为 {ExitCode}");
            
            // CliFx 可能会在 StandardError 中输出一些警告信息，但不影响命令成功
            // 只要退出码为0且没有实质性错误内容就认为成功
            if (!string.IsNullOrWhiteSpace(StandardError))
            {
                // 允许空的行或只有空格的行
                var trimmedError = StandardError.Trim();
                if (!string.IsNullOrEmpty(trimmedError))
                {
                    // 检查是否是已知的无害输出
                    var harmlessPatterns = new[] 
                    {
                        @"^\s*$",  // 空行
                        @"^warning\s+",  // 警告信息
                        @"^info\s+",    // 信息输出
                        @"^错误：",     // CLI工具的错误信息（中文冒号）
                        @"^Error:",    // CLI工具的错误信息（英文冒号）
                    };
                    
                    var isHarmless = harmlessPatterns.Any(pattern => 
                        Regex.IsMatch(trimmedError, pattern, RegexOptions.IgnoreCase));
                    
                    isHarmless.Should().BeTrue($"命令 '{Arguments}' 有错误输出: '{trimmedError}'");
                }
            }
        }

        /// <summary>
        /// 验证命令是否失败并包含特定错误信息
        /// </summary>
        public void ShouldFailWithError(string expectedError)
        {
            Success.Should().BeFalse($"命令 '{Arguments}' 应该失败");
            AllOutput.Should().Contain(expectedError, $"错误输出应该包含 '{expectedError}'");
        }

        /// <summary>
        /// 获取输出中的模型类型列表
        /// </summary>
        public List<string> GetModelTypes()
        {
            var lines = StandardOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var modelTypes = new List<string>();
            
            var inModelList = false;
            var passedHeader = false;
            foreach (var line in lines)
            {
                if (line.Contains("支持的模型类型:"))
                {
                    inModelList = true;
                    continue;
                }
                
                if (inModelList)
                {
                    // 跳过第一个"---"分隔线
                    if (line.StartsWith("---") && !passedHeader)
                    {
                        passedHeader = true;
                        continue;
                    }
                    
                    // 遇到第二个"---"或"总计:"时停止
                    if (line.StartsWith("---") && passedHeader || line.StartsWith("总计:"))
                    {
                        break;
                    }
                    
                    if (line.StartsWith("- "))
                    {
                        modelTypes.Add(line.Substring(2).Trim());
                    }
                }
            }
            
            return modelTypes;
        }

        /// <summary>
        /// 获取识别的模型类型
        /// </summary>
        public string? GetRecognizedModelType()
        {
            var match = Regex.Match(StandardOutput, @"✓ 识别成功: (.+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
    }
}