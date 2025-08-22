using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.UATTests
{
    /// <summary>
    /// BDD风格的UAT测试基类
    /// </summary>
    public class BddUatTestBase : IDisposable
    {
        protected readonly string _cliProjectPath;
        protected readonly string _testDataPath;
        protected readonly string _tempPath;
        protected readonly string _uatWorkspace;
        protected readonly StringBuilder _testLog;

        public BddUatTestBase()
        {
            var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            _cliProjectPath = Path.Combine(solutionRoot, "BannerlordModEditor.Cli");
            _testDataPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData");
            _tempPath = Path.GetTempPath();
            _uatWorkspace = Path.Combine(_tempPath, $"uat_workspace_{Guid.NewGuid()}");
            _testLog = new StringBuilder();
            
            // 创建UAT工作空间
            Directory.CreateDirectory(_uatWorkspace);
            
            Log("=== BDD UAT Test Started ===");
            Log($"Test Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Log($"Workspace: {_uatWorkspace}");
        }

        /// <summary>
        /// BDD Given 步骤 - 设置测试前提条件
        /// </summary>
        protected async Task GivenAsync(string description, Func<Task> action)
        {
            Log($"GIVEN: {description}");
            await action();
        }

        /// <summary>
        /// BDD When 步骤 - 执行测试操作
        /// </summary>
        protected async Task WhenAsync(string description, Func<Task> action)
        {
            Log($"WHEN: {description}");
            await action();
        }

        /// <summary>
        /// BDD Then 步骤 - 验证测试结果
        /// </summary>
        protected async Task ThenAsync(string description, Func<Task> assertion)
        {
            Log($"THEN: {description}");
            await assertion();
        }

        /// <summary>
        /// BDD And 步骤 - 添加更多的Given/When/Then步骤
        /// </summary>
        protected async Task AndAsync(string description, Func<Task> action)
        {
            Log($"AND: {description}");
            await action();
        }

        /// <summary>
        /// 执行CLI命令
        /// </summary>
        protected async Task<BddExecutionResult> ExecuteCliAsync(string arguments, string? workingDirectory = null)
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
                throw new TimeoutException($"BDD UAT命令执行超时: {arguments}");
            }

            var result = new BddExecutionResult
            {
                ExitCode = process.ExitCode,
                StandardOutput = outputBuilder.ToString(),
                StandardError = errorBuilder.ToString(),
                Arguments = arguments,
                WorkingDirectory = workingDirectory ?? _uatWorkspace
            };

            Log($"Command executed: {arguments}");
            Log($"Exit code: {result.ExitCode}");
            Log($"Success: {result.Success}");

            return result;
        }

        /// <summary>
        /// 创建用户故事场景
        /// </summary>
        protected UserStory CreateUserStory(string title, string actor, string goal)
        {
            return new UserStory(title, actor, goal);
        }

        /// <summary>
        /// 记录测试日志
        /// </summary>
        protected void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _testLog.AppendLine($"[{timestamp}] {message}");
        }

        /// <summary>
        /// 获取测试数据文件路径
        /// </summary>
        protected string GetTestDataPath(string fileName)
        {
            return Path.Combine(_testDataPath, fileName);
        }

        /// <summary>
        /// 获取UAT工作空间文件路径
        /// </summary>
        protected string GetUatFilePath(string fileName)
        {
            return Path.Combine(_uatWorkspace, fileName);
        }

        /// <summary>
        /// 复制测试数据到工作空间
        /// </summary>
        protected async Task CopyTestDataToWorkspaceAsync(string fileName)
        {
            var sourcePath = GetTestDataPath(fileName);
            var destPath = GetUatFilePath(fileName);
            
            if (File.Exists(sourcePath))
            {
                await File.CopyAsync(sourcePath, destPath);
                Log($"Copied test data: {fileName}");
            }
            else
            {
                throw new FileNotFoundException($"测试数据文件不存在: {sourcePath}");
            }
        }

        /// <summary>
        /// 验证文件存在
        /// </summary>
        protected void VerifyFileExists(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"文件应该存在: {filePath}");
            Log($"✓ 文件存在: {Path.GetFileName(filePath)}");
        }

        /// <summary>
        /// 验证Excel文件格式
        /// </summary>
        protected void VerifyExcelFile(string filePath)
        {
            VerifyFileExists(filePath);
            Path.GetExtension(filePath).ToLower().Should().Be(".xlsx", $"文件应该是.xlsx格式: {filePath}");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(1024, $"Excel文件应该大于1KB: {filePath}");
            Log($"✓ Excel文件格式正确: {Path.GetFileName(filePath)} ({fileInfo.Length / 1024}KB)");
        }

        /// <summary>
        /// 验证XML文件格式
        /// </summary>
        protected async Task VerifyXmlFileAsync(string filePath)
        {
            VerifyFileExists(filePath);
            var content = await File.ReadAllTextAsync(filePath);
            content.Should().StartWith("<?xml", $"文件应该是有效的XML格式: {filePath}");
            
            // 尝试解析XML
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(content);
            Log($"✓ XML文件格式正确: {Path.GetFileName(filePath)}");
        }

        /// <summary>
        /// 生成BDD测试报告
        /// </summary>
        protected async Task GenerateBddReportAsync(string featureName, string description, Dictionary<string, object> results)
        {
            var reportPath = GetUatFilePath($"{featureName}_BDD_Report.md");
            var reportContent = new StringBuilder();
            
            reportContent.AppendLine($"# {featureName}");
            reportContent.AppendLine();
            reportContent.AppendLine($"**描述**: {description}");
            reportContent.AppendLine();
            reportContent.AppendLine($"**测试时间**: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            reportContent.AppendLine();
            reportContent.AppendLine("## 测试场景");
            reportContent.AppendLine();
            
            foreach (var kvp in results)
            {
                reportContent.AppendLine($"- **{kvp.Key}**: {kvp.Value}");
            }
            
            reportContent.AppendLine();
            reportContent.AppendLine("## 测试日志");
            reportContent.AppendLine();
            reportContent.AppendLine("```");
            reportContent.AppendLine(_testLog.ToString());
            reportContent.AppendLine("```");
            
            await File.WriteAllTextAsync(reportPath, reportContent.ToString());
            Log($"✓ BDD报告已生成: {reportPath}");
        }

        public void Dispose()
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
            
            Log("=== BDD UAT Test Completed ===");
        }
    }

    /// <summary>
    /// BDD执行结果
    /// </summary>
    public class BddExecutionResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; } = string.Empty;
        public string StandardError { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;
        public string WorkingDirectory { get; set; } = string.Empty;

        public bool Success => ExitCode == 0;
        public string AllOutput => $"{StandardOutput}{StandardError}";

        public void ShouldSucceed()
        {
            Success.Should().BeTrue($"命令应该成功执行: {Arguments}");
            StandardError.Should().BeEmpty($"不应该有错误输出: {Arguments}");
        }

        public void ShouldFailWithError(string expectedError)
        {
            Success.Should().BeFalse($"命令应该失败: {Arguments}");
            AllOutput.Should().Contain(expectedError, $"应该包含错误信息: {expectedError}");
        }

        public void ShouldContain(string expectedText)
        {
            AllOutput.Should().Contain(expectedText, $"输出应该包含: {expectedText}");
        }

        public void ShouldNotContain(string unexpectedText)
        {
            AllOutput.Should().NotContain(unexpectedText, $"输出不应该包含: {unexpectedText}");
        }
    }

    /// <summary>
    /// 用户故事类
    /// </summary>
    public class UserStory
    {
        public string Title { get; }
        public string Actor { get; }
        public string Goal { get; }
        public List<Scenario> Scenarios { get; } = new List<Scenario>();

        public UserStory(string title, string actor, string goal)
        {
            Title = title;
            Actor = actor;
            Goal = goal;
        }

        public Scenario AddScenario(string name)
        {
            var scenario = new Scenario(name);
            Scenarios.Add(scenario);
            return scenario;
        }
    }

    /// <summary>
    /// 测试场景类
    /// </summary>
    public class Scenario
    {
        public string Name { get; }
        public List<string> Steps { get; } = new List<string>();

        public Scenario(string name)
        {
            Name = name;
        }

        public void AddStep(string step)
        {
            Steps.Add(step);
        }
    }
}