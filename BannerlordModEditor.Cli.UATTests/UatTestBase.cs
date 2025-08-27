using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.UATTests
{
    /// <summary>
    /// UAT测试基类 - 模拟真实用户使用场景
    /// </summary>
    public class UatTestBase
    {
        protected readonly string _cliProjectPath;
        protected readonly string _testDataPath;
        protected readonly string _tempPath;
        protected readonly string _uatWorkspace;

        public UatTestBase()
        {
            var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            _cliProjectPath = Path.Combine(solutionRoot, "BannerlordModEditor.Cli");
            _testDataPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData");
            _tempPath = Path.GetTempPath();
            _uatWorkspace = Path.Combine(_tempPath, $"uat_workspace_{Guid.NewGuid()}");
            
            // 创建UAT工作空间
            Directory.CreateDirectory(_uatWorkspace);
        }

        /// <summary>
        /// 执行CLI命令并返回结果
        /// </summary>
        protected async Task<UatExecutionResult> ExecuteCliCommandAsync(string arguments, string? workingDirectory = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{_cliProjectPath}\" -- {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory ?? _uatWorkspace
            };

            using var process = new Process { StartInfo = processStartInfo };
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
            process.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            var timeoutMs = 60000; // UAT测试使用更长的超时时间
            var exited = process.WaitForExit(timeoutMs);

            if (!exited)
            {
                process.Kill();
                throw new TimeoutException($"UAT命令执行超时: {arguments}");
            }

            return new UatExecutionResult
            {
                ExitCode = process.ExitCode,
                StandardOutput = outputBuilder.ToString(),
                StandardError = errorBuilder.ToString(),
                Arguments = arguments,
                WorkingDirectory = workingDirectory ?? _uatWorkspace
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
        /// 获取UAT工作空间中的文件路径
        /// </summary>
        protected string GetUatFilePath(string fileName)
        {
            return Path.Combine(_uatWorkspace, fileName);
        }

        /// <summary>
        /// 复制测试数据到UAT工作空间
        /// </summary>
        protected async Task CopyTestDataToWorkspaceAsync(string fileName)
        {
            var sourcePath = GetTestDataPath(fileName);
            var destPath = GetUatFilePath(fileName);
            
            if (File.Exists(sourcePath))
            {
                await File.CopyAsync(sourcePath, destPath);
            }
            else
            {
                throw new FileNotFoundException($"测试数据文件不存在: {sourcePath}");
            }
        }

        /// <summary>
        /// 创建用户场景的测试数据
        /// </summary>
        protected async Task CreateUserScenarioDataAsync()
        {
            // 复制一些常用的XML文件到工作空间
            await CopyTestDataToWorkspaceAsync("action_types.xml");
            await CopyTestDataToWorkspaceAsync("combat_parameters.xml");
            await CopyTestDataToWorkspaceAsync("map_icons.xml");
            
            // 创建用户说明文件
            var readmeContent = @"
# Bannerlord Mod Editor CLI - 用户测试场景

这个目录包含用于UAT测试的文件和脚本。

## 文件说明
- action_types.xml: 动作类型定义文件
- combat_parameters.xml: 战斗参数文件
- map_icons.xml: 地图图标文件

## 测试场景
1. 基本转换：XML 转 Excel
2. 格式识别：识别XML文件类型
3. 批量处理：处理多个文件
4. 错误处理：测试各种错误情况

## 使用方法
1. 运行 `dotnet run -- list-models` 查看支持的模型类型
2. 运行 `dotnet run -- recognize -i file.xml` 识别文件格式
3. 运行 `dotnet run -- convert -i file.xml -o output.xlsx` 转换文件
";
            await File.WriteAllTextAsync(GetUatFilePath("README.md"), readmeContent);
        }

        /// <summary>
        /// 清理UAT工作空间
        /// </summary>
        public void Cleanup()
        {
            try
            {
                if (Directory.Exists(_uatWorkspace))
                {
                    Directory.Delete(_uatWorkspace, true);
                }
            }
            catch
            {
                // 忽略清理错误
            }
        }

        /// <summary>
        /// 验证文件存在且内容符合预期
        /// </summary>
        protected async Task VerifyOutputFileAsync(string filePath, string expectedContentFragment)
        {
            File.Exists(filePath).Should().BeTrue($"输出文件应该存在: {filePath}");
            var content = await File.ReadAllTextAsync(filePath);
            content.Should().Contain(expectedContentFragment, $"文件应该包含预期内容: {expectedContentFragment}");
        }

        /// <summary>
        /// 验证Excel文件格式
        /// </summary>
        protected void VerifyExcelFile(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"Excel文件应该存在: {filePath}");
            Path.GetExtension(filePath).ToLower().Should().Be(".xlsx", $"文件应该是.xlsx格式: {filePath}");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(1024, $"Excel文件应该大于1KB: {filePath}");
        }

        /// <summary>
        /// 验证XML文件格式
        /// </summary>
        protected async Task VerifyXmlFileAsync(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"XML文件应该存在: {filePath}");
            var content = await File.ReadAllTextAsync(filePath);
            content.Should().StartWith("<?xml", $"文件应该是有效的XML格式: {filePath}");
            
            // 尝试解析XML
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(content);
        }

        /// <summary>
        /// 模拟用户输入和验证
        /// </summary>
        protected async Task SimulateUserWorkflowAsync(params string[] commands)
        {
            foreach (var command in commands)
            {
                var result = await ExecuteCliCommandAsync(command);
                result.Success.Should().BeTrue($"用户命令应该成功: {command}");
            }
        }

        /// <summary>
        /// 创建测试报告
        /// </summary>
        protected async Task CreateTestReportAsync(string testName, Dictionary<string, object> results)
        {
            var reportPath = GetUatFilePath($"{testName}_report.md");
            var reportContent = new StringBuilder();
            
            reportContent.AppendLine($"# {testName} - UAT测试报告");
            reportContent.AppendLine();
            reportContent.AppendLine($"测试时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            reportContent.AppendLine($"测试环境: {Environment.OSVersion}");
            reportContent.AppendLine();
            reportContent.AppendLine("## 测试结果");
            reportContent.AppendLine();
            
            foreach (var kvp in results)
            {
                reportContent.AppendLine($"- {kvp.Key}: {kvp.Value}");
            }
            
            reportContent.AppendLine();
            reportContent.AppendLine("## 测试文件");
            reportContent.AppendLine();
            
            var files = Directory.GetFiles(_uatWorkspace);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                reportContent.AppendLine($"- {fileInfo.Name}: {fileInfo.Length / 1024}KB");
            }
            
            await File.WriteAllTextAsync(reportPath, reportContent.ToString());
        }
    }

    /// <summary>
    /// UAT执行结果
    /// </summary>
    public class UatExecutionResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; } = string.Empty;
        public string StandardError { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;
        public string WorkingDirectory { get; set; } = string.Empty;

        public bool Success => ExitCode == 0;
        public string AllOutput => $"{StandardOutput}{StandardError}";

        public void ShouldSucceed(string because = "")
        {
            Success.Should().BeTrue(because);
            StandardError.Should().BeEmpty(because);
        }

        public void ShouldFailWithError(string expectedError, string because = "")
        {
            Success.Should().BeFalse(because);
            AllOutput.Should().Contain(expectedError, because);
        }

        public void ShouldContain(string expectedText, string because = "")
        {
            AllOutput.Should().Contain(expectedText, because);
        }

        public void ShouldNotContain(string unexpectedText, string because = "")
        {
            AllOutput.Should().NotContain(unexpectedText, because);
        }
    }
}