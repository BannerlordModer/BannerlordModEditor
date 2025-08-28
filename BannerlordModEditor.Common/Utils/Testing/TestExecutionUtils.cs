using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Utils.Testing
{
    /// <summary>
    /// 测试执行工具类
    /// 提供测试执行、监控和结果收集功能
    /// </summary>
    public class TestExecutionUtils
    {
        private readonly QualityMonitoringService _qualityMonitoringService;
        private readonly string _dotnetPath;

        /// <summary>
        /// 初始化测试执行工具类
        /// </summary>
        public TestExecutionUtils(QualityMonitoringService qualityMonitoringService, string? dotnetPath = null)
        {
            _qualityMonitoringService = qualityMonitoringService ?? throw new ArgumentNullException(nameof(qualityMonitoringService));
            _dotnetPath = dotnetPath ?? "dotnet";
        }

        /// <summary>
        /// 执行测试项目
        /// </summary>
        public async Task<TestSessionDO> ExecuteTestProjectAsync(string projectPath, string? solutionPath = null, 
            string configuration = "Debug", string? framework = null, Dictionary<string, string>? additionalArgs = null)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var session = new TestSessionDO
                {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionName = $"Test Execution {DateTime.Now:yyyyMMdd_HHmmss}",
                    ProjectPath = projectPath,
                    SolutionPath = solutionPath ?? string.Empty,
                    BuildConfiguration = configuration,
                    TargetFramework = framework ?? "net9.0",
                    StartTime = DateTime.Now,
                    SessionStatus = TestSessionStatus.Running
                };

                // 构建测试命令
                var arguments = BuildTestArguments(projectPath, configuration, framework, additionalArgs);
                
                // 执行测试
                var executionResult = await ExecuteDotnetCommandAsync(arguments, Path.GetDirectoryName(projectPath));
                
                // 解析测试结果
                var testResults = ParseTestOutput(executionResult.Output, projectPath);
                
                // 更新会话信息
                session.EndTime = DateTime.Now;
                session.TestResults = testResults;
                session.CalculateStatistics();
                session.SessionStatus = executionResult.ExitCode == 0 ? TestSessionStatus.Completed : TestSessionStatus.Failed;

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteTestProject", stopwatch.ElapsedMilliseconds, 
                    executionResult.ExitCode == 0, executionResult.Error);

                return session;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteTestProject", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 执行解决方案中的所有测试
        /// </summary>
        public async Task<TestSessionDO> ExecuteSolutionTestsAsync(string solutionPath, string configuration = "Debug", 
            string? framework = null, Dictionary<string, string>? additionalArgs = null)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var session = new TestSessionDO
                {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionName = $"Solution Tests {DateTime.Now:yyyyMMdd_HHmmss}",
                    SolutionPath = solutionPath,
                    ProjectPath = string.Empty,
                    BuildConfiguration = configuration,
                    TargetFramework = framework ?? "net9.0",
                    StartTime = DateTime.Now,
                    SessionStatus = TestSessionStatus.Running
                };

                // 构建测试命令
                var arguments = BuildTestArguments(solutionPath, configuration, framework, additionalArgs);
                
                // 执行测试
                var executionResult = await ExecuteDotnetCommandAsync(arguments, Path.GetDirectoryName(solutionPath));
                
                // 解析测试结果
                var testResults = ParseTestOutput(executionResult.Output, solutionPath);
                
                // 更新会话信息
                session.EndTime = DateTime.Now;
                session.TestResults = testResults;
                session.CalculateStatistics();
                session.SessionStatus = executionResult.ExitCode == 0 ? TestSessionStatus.Completed : TestSessionStatus.Failed;

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteSolutionTests", stopwatch.ElapsedMilliseconds, 
                    executionResult.ExitCode == 0, executionResult.Error);

                return session;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteSolutionTests", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 执行特定测试方法
        /// </summary>
        public async Task<TestResultDO> ExecuteTestMethodAsync(string projectPath, string testMethodName, 
            string configuration = "Debug", string? framework = null)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var additionalArgs = new Dictionary<string, string>
                {
                    { "filter", $"FullyQualifiedName~{testMethodName}" }
                };

                var arguments = BuildTestArguments(projectPath, configuration, framework, additionalArgs);
                var executionResult = await ExecuteDotnetCommandAsync(arguments, Path.GetDirectoryName(projectPath));
                
                var testResults = ParseTestOutput(executionResult.Output, projectPath);
                var testResult = testResults.FirstOrDefault();

                if (testResult == null)
                {
                    testResult = new TestResultDO
                    {
                        Name = testMethodName,
                        Status = TestStatus.Failed,
                        ErrorMessage = "Test method not found in output",
                        DurationMs = (long)stopwatch.ElapsedMilliseconds
                    };
                }

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteTestMethod", stopwatch.ElapsedMilliseconds, 
                    testResult.Status == TestStatus.Passed, testResult.ErrorMessage);

                return testResult;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("ExecuteTestMethod", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 收集代码覆盖率信息
        /// </summary>
        public async Task<CoverageMetricsDO> CollectCoverageAsync(string projectPath, string configuration = "Debug")
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                // 这里简化实现，实际应用中需要使用coverlet或其他覆盖率工具
                var coverage = new CoverageMetricsDO
                {
                    ProjectName = Path.GetFileNameWithoutExtension(projectPath),
                    GeneratedAt = DateTime.Now,
                    LineCoverage = 0.0,
                    BranchCoverage = 0.0,
                    MethodCoverage = 0.0,
                    ClassCoverage = 0.0
                };

                // 模拟覆盖率收集
                await Task.Delay(100); // 模拟异步操作

                coverage.CalculateCoverageGrade();

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("CollectCoverage", stopwatch.ElapsedMilliseconds, true);

                return coverage;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("CollectCoverage", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 构建测试命令参数
        /// </summary>
        private string BuildTestArguments(string projectOrSolutionPath, string configuration, string? framework, 
            Dictionary<string, string>? additionalArgs)
        {
            var arguments = new List<string> { "test", projectOrSolutionPath };
            
            arguments.Add("--configuration");
            arguments.Add(configuration);
            
            arguments.Add("--verbosity");
            arguments.Add("normal");
            
            arguments.Add("--logger");
            arguments.Add("console;verbosity=normal");

            if (!string.IsNullOrEmpty(framework))
            {
                arguments.Add("--framework");
                arguments.Add(framework);
            }

            // 添加额外的参数
            if (additionalArgs != null)
            {
                foreach (var kvp in additionalArgs)
                {
                    if (kvp.Key.StartsWith("--"))
                    {
                        arguments.Add(kvp.Key);
                        if (!string.IsNullOrEmpty(kvp.Value))
                        {
                            arguments.Add(kvp.Value);
                        }
                    }
                    else
                    {
                        arguments.Add($"--{kvp.Key}");
                        if (!string.IsNullOrEmpty(kvp.Value))
                        {
                            arguments.Add(kvp.Value);
                        }
                    }
                }
            }

            return string.Join(" ", arguments);
        }

        /// <summary>
        /// 执行dotnet命令
        /// </summary>
        private async Task<CommandExecutionResult> ExecuteDotnetCommandAsync(string arguments, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = _dotnetPath,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            
            var outputBuilder = new System.Text.StringBuilder();
            var errorBuilder = new System.Text.StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            return new CommandExecutionResult
            {
                ExitCode = process.ExitCode,
                Output = outputBuilder.ToString(),
                Error = errorBuilder.ToString()
            };
        }

        /// <summary>
        /// 解析测试输出
        /// </summary>
        private List<TestResultDO> ParseTestOutput(string output, string projectPath)
        {
            var results = new List<TestResultDO>();
            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            TestResultDO? currentTest = null;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                // 解析测试开始
                if (trimmedLine.Contains("Starting test execution"))
                {
                    continue;
                }

                // 解析测试结果
                if (trimmedLine.Contains("Passed!") || trimmedLine.Contains("Failed!") || trimmedLine.Contains("Skipped!"))
                {
                    if (currentTest != null)
                    {
                        results.Add(currentTest);
                    }

                    currentTest = ParseTestResultLine(trimmedLine, projectPath);
                }
                // 解析错误信息
                else if (currentTest != null && (trimmedLine.Contains("Error Message:") || trimmedLine.Contains("Stack Trace:")))
                {
                    if (trimmedLine.Contains("Error Message:"))
                    {
                        currentTest.ErrorMessage = trimmedLine.Replace("Error Message:", "").Trim();
                    }
                    else if (trimmedLine.Contains("Stack Trace:"))
                    {
                        currentTest.ErrorStackTrace = trimmedLine.Replace("Stack Trace:", "").Trim();
                    }
                }
                // 解析执行时间
                else if (currentTest != null && trimmedLine.Contains("Elapsed time:"))
                {
                    var timeStr = trimmedLine.Replace("Elapsed time:", "").Replace("ms", "").Trim();
                    if (long.TryParse(timeStr, out var duration))
                    {
                        currentTest.DurationMs = duration;
                    }
                }
            }

            // 添加最后一个测试结果
            if (currentTest != null)
            {
                results.Add(currentTest);
            }

            return results;
        }

        /// <summary>
        /// 解析测试结果行
        /// </summary>
        private TestResultDO ParseTestResultLine(string line, string projectPath)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            var result = new TestResultDO
            {
                ProjectPath = projectPath,
                FilePath = string.Empty,
                LineNumber = 0
            };

            // 解析状态
            if (line.Contains("Passed!"))
            {
                result.Status = TestStatus.Passed;
            }
            else if (line.Contains("Failed!"))
            {
                result.Status = TestStatus.Failed;
            }
            else if (line.Contains("Skipped!"))
            {
                result.Status = TestStatus.Skipped;
            }

            // 解析测试名称（简化实现）
            var testStartIndex = line.IndexOf("Test:", StringComparison.OrdinalIgnoreCase);
            if (testStartIndex >= 0)
            {
                var testNamePart = line.Substring(testStartIndex + 5).Trim();
                result.Name = testNamePart;
                result.MethodFullName = testNamePart;
            }

            // 设置默认值
            if (string.IsNullOrEmpty(result.Name))
            {
                result.Name = "Unknown Test";
            }

            result.Type = "Unit";
            result.Category = "General";
            result.StartTime = DateTime.Now;
            result.EndTime = DateTime.Now;
            result.DurationMs = 0;

            return result;
        }

        /// <summary>
        /// 验证测试项目
        /// </summary>
        public async Task<bool> ValidateTestProjectAsync(string projectPath)
        {
            try
            {
                if (!File.Exists(projectPath))
                {
                    return false;
                }

                var projectContent = await File.ReadAllTextAsync(projectPath);
                return projectContent.Contains("<Project Sdk=\"Microsoft.NET.Sdk\"") && 
                       (projectContent.Contains("xunit") || projectContent.Contains("nunit") || projectContent.Contains("mstest"));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取测试项目信息
        /// </summary>
        public async Task<TestProjectInfo> GetTestProjectInfoAsync(string projectPath)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var info = new TestProjectInfo
                {
                    ProjectPath = projectPath,
                    IsValid = await ValidateTestProjectAsync(projectPath),
                    TestFramework = "Unknown",
                    TargetFrameworks = new List<string>()
                };

                if (info.IsValid)
                {
                    var projectContent = await File.ReadAllTextAsync(projectPath);
                    
                    // 检测测试框架
                    if (projectContent.Contains("xunit"))
                    {
                        info.TestFramework = "xUnit";
                    }
                    else if (projectContent.Contains("nunit"))
                    {
                        info.TestFramework = "NUnit";
                    }
                    else if (projectContent.Contains("mstest"))
                    {
                        info.TestFramework = "MSTest";
                    }

                    // 解析目标框架（简化实现）
                    var targetFrameworkMatches = System.Text.RegularExpressions.Regex.Matches(projectContent, @"<TargetFramework>([^<]+)</TargetFramework>");
                    foreach (System.Text.RegularExpressions.Match match in targetFrameworkMatches)
                    {
                        info.TargetFrameworks.Add(match.Groups[1].Value);
                    }
                }

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("GetTestProjectInfo", stopwatch.ElapsedMilliseconds, true);

                return info;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("GetTestProjectInfo", stopwatch.ElapsedMilliseconds, false, ex.Message);
                return new TestProjectInfo { ProjectPath = projectPath, IsValid = false };
            }
        }
    }

    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandExecutionResult
    {
        /// <summary>
        /// 退出代码
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// 输出内容
        /// </summary>
        public string Output { get; set; } = string.Empty;

        /// <summary>
        /// 错误内容
        /// </summary>
        public string Error { get; set; } = string.Empty;
    }

    /// <summary>
    /// 测试项目信息
    /// </summary>
    public class TestProjectInfo
    {
        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 测试框架
        /// </summary>
        public string TestFramework { get; set; } = "Unknown";

        /// <summary>
        /// 目标框架列表
        /// </summary>
        public List<string> TargetFrameworks { get; set; } = new List<string>();
    }
}