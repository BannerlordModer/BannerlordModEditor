using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BannerlordModEditor.TUI.IntegrationTests.Common
{
    /// <summary>
    /// tmux集成测试基类，提供通用的tmux操作和测试工具方法
    /// </summary>
    public abstract class TmuxIntegrationTestBase : IDisposable
    {
        protected readonly ITestOutputHelper Output;
        protected readonly string TestTempDir;
        protected readonly string TestSessionName;
        protected readonly int TestTimeoutSeconds = 30;

        protected TmuxIntegrationTestBase(ITestOutputHelper output)
        {
            Output = output;
            TestSessionName = $"tui_test_{Guid.NewGuid():N}";
            TestTempDir = Path.Combine(Path.GetTempPath(), $"TuiTest_{Guid.NewGuid():N}");
            
            // 确保测试目录存在
            Directory.CreateDirectory(TestTempDir);
            
            Output.WriteLine($"测试临时目录: {TestTempDir}");
            Output.WriteLine($"tmux会话名称: {TestSessionName}");
        }

        /// <summary>
        /// 检查tmux是否可用
        /// </summary>
        protected bool IsTmuxAvailable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "tmux",
                        Arguments = "-V",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建新的tmux会话
        /// </summary>
        protected async Task CreateTmuxSessionAsync()
        {
            // 检查tmux可用性，如果不可用则返回false
            if (!IsTmuxAvailable())
            {
                Output.WriteLine("tmux不可用，跳过集成测试");
                return;
            }

            // 杀死可能存在的同名会话
            await ExecuteCommandAsync("tmux", $"kill-session -t {TestSessionName} 2>/dev/null || true");
            
            // 创建新会话
            var result = await ExecuteCommandAsync("tmux", $"new-session -d -s {TestSessionName}");
            result.ExitCode.ShouldBe(0, $"应该成功创建tmux会话: {result.Error}");
            
            Output.WriteLine($"创建tmux会话: {TestSessionName}");
        }

        /// <summary>
        /// 检查tmux会话是否活跃
        /// </summary>
        protected async Task<bool> IsTmuxSessionActiveAsync()
        {
            if (!IsTmuxAvailable())
            {
                return false;
            }

            var result = await ExecuteCommandAsync("tmux", $"list-sessions | grep {TestSessionName}");
            return result.ExitCode == 0 && !string.IsNullOrWhiteSpace(result.Output);
        }

        /// <summary>
        /// 在tmux会话中执行命令
        /// </summary>
        protected async Task<ProcessResult> ExecuteTmuxCommandAsync(string command, bool waitForCompletion = true)
        {
            var fullCommand = $"send-keys -t {TestSessionName} '{command}' Enter";
            var result = await ExecuteCommandAsync("tmux", fullCommand);
            
            result.ExitCode.ShouldBe(0, $"应该成功发送命令到tmux会话: {result.Error}");
            
            if (waitForCompletion)
            {
                // 等待命令执行完成
                await Task.Delay(1000);
            }
            
            Output.WriteLine($"在tmux中执行命令: {command}");
            return result;
        }

        /// <summary>
        /// 捕获tmux会话的输出
        /// </summary>
        protected async Task<string> CaptureTmuxOutputAsync(int lineCount = 10)
        {
            // 修复capture-pane命令的参数格式
            var result = await ExecuteCommandAsync("tmux", $"capture-pane -t {TestSessionName} -p");
            result.ExitCode.ShouldBe(0, $"应该成功捕获tmux输出: {result.Error}");
            
            // 只返回最后几行
            var lines = result.Output.Split('\n');
            return string.Join('\n', lines.Skip(Math.Max(0, lines.Length - lineCount)));
        }

        /// <summary>
        /// 等待tmux输出中出现特定文本
        /// </summary>
        protected async Task WaitForTmuxOutputAsync(string expectedText, int timeoutSeconds = 10)
        {
            var startTime = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            
            while (DateTime.UtcNow - startTime < timeout)
            {
                var output = await CaptureTmuxOutputAsync(20);
                if (output.Contains(expectedText))
                {
                    Output.WriteLine($"在tmux输出中找到预期文本: {expectedText}");
                    return;
                }
                
                await Task.Delay(500);
            }
            
            // 超时后获取最后的输出用于调试
            var finalOutput = await CaptureTmuxOutputAsync(20);
            throw new TimeoutException($"在{timeoutSeconds}秒内未找到预期文本 '{expectedText}'。最后输出:\n{finalOutput}");
        }

        /// <summary>
        /// 启动TUI应用程序
        /// </summary>
        protected async Task StartTuiApplicationAsync(string tuiAppPath)
        {
            if (!File.Exists(tuiAppPath))
            {
                throw new FileNotFoundException($"TUI应用程序不存在: {tuiAppPath}");
            }

            // 启动TUI应用（使用测试模式）
            var startCommand = $"dotnet {tuiAppPath} --test";
            await ExecuteTmuxCommandAsync(startCommand, waitForCompletion: false);
            
            // 等待应用启动
            await Task.Delay(2000);
            
            Output.WriteLine($"启动TUI应用程序: {tuiAppPath}");
        }

        /// <summary>
        /// 启动TUI应用程序并执行命令行转换
        /// </summary>
        protected async Task<bool> StartTuiCommandLineConversionAsync(string tuiAppPath, string inputFile, string outputFile)
        {
            if (!File.Exists(tuiAppPath))
            {
                throw new FileNotFoundException($"TUI应用程序不存在: {tuiAppPath}");
            }

            // 切换到TUI项目目录并执行命令
            var tuiProjectDir = Path.GetDirectoryName(tuiAppPath);
            var tuiFileName = Path.GetFileName(tuiAppPath);
            
            var result = await ExecuteCommandAsync("dotnet", $"run --project \"{tuiProjectDir}\" -- --convert \"{inputFile}\" \"{outputFile}\"", tuiProjectDir);
            
            Output.WriteLine($"执行TUI命令行转换: {inputFile} -> {outputFile}");
            Output.WriteLine($"转换结果: {result.ExitCode}");
            Output.WriteLine($"输出: {result.Output}");
            Output.WriteLine($"错误: {result.Error}");
            
            return result.ExitCode == 0;
        }

        /// <summary>
        /// 模拟键盘按键输入
        /// </summary>
        protected async Task SendKeySequenceAsync(string keys)
        {
            await ExecuteTmuxCommandAsync(keys);
            await Task.Delay(200); // 等待按键处理
        }

        /// <summary>
        /// 发送文件路径输入
        /// </summary>
        protected async Task SendFilePathAsync(string filePath)
        {
            await SendKeySequenceAsync(filePath);
            await SendKeySequenceAsync("Enter"); // 确认文件路径
        }

        /// <summary>
        /// 执行系统命令
        /// </summary>
        protected async Task<ProcessResult> ExecuteCommandAsync(string fileName, string arguments, string workingDirectory = null)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

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

            var exited = process.WaitForExit((int)TimeSpan.FromSeconds(TestTimeoutSeconds).TotalMilliseconds);
            if (!exited)
            {
                process.Kill();
                throw new TimeoutException($"命令执行超时: {fileName} {arguments}");
            }

            return new ProcessResult
            {
                ExitCode = process.ExitCode,
                Output = outputBuilder.ToString(),
                Error = errorBuilder.ToString()
            };
        }

        /// <summary>
        /// 创建测试用的Excel文件
        /// </summary>
        protected string CreateTestExcelFile(string fileName, string content = "")
        {
            var filePath = Path.Combine(TestTempDir, fileName);
            
            // 创建简单的CSV文件（TUI应该能处理）
            if (string.IsNullOrEmpty(content))
            {
                content = "Name,Value,Description\nTest1,100,测试数据1\nTest2,200,测试数据2";
            }
            
            File.WriteAllText(filePath, content);
            Output.WriteLine($"创建测试文件: {filePath}");
            return filePath;
        }

        /// <summary>
        /// 验证文件是否存在且内容正确
        /// </summary>
        protected void VerifyOutputFile(string filePath, bool shouldExist = true)
        {
            if (shouldExist)
            {
                File.Exists(filePath).Should().BeTrue($"输出文件应该存在: {filePath}");
                new FileInfo(filePath).Length.Should().BeGreaterThan(0, $"文件内容不应为空: {filePath}");
                Output.WriteLine($"验证输出文件存在且有效: {filePath}");
            }
            else
            {
                File.Exists(filePath).Should().BeFalse($"输出文件不应该存在: {filePath}");
                Output.WriteLine($"验证输出文件不存在（符合预期）: {filePath}");
            }
        }

        /// <summary>
        /// 获取TUI应用程序的路径
        /// </summary>
        protected string GetTuiAppPath()
        {
            var currentDir = Directory.GetCurrentDirectory();
            Output.WriteLine($"当前目录: {currentDir}");
            
            // 向上查找解决方案目录
            var solutionDir = currentDir;
            while (!File.Exists(Path.Combine(solutionDir, "BannerlordModEditor.sln")) && solutionDir != "/")
            {
                solutionDir = Directory.GetParent(solutionDir)?.FullName ?? "/";
            }
            
            Output.WriteLine($"解决方案目录: {solutionDir}");
            
            var tuiProjectDir = Path.Combine(solutionDir, "BannerlordModEditor.TUI");
            var buildDir = Path.Combine(tuiProjectDir, "bin", "Debug", "net9.0");
            
            Output.WriteLine($"TUI项目目录: {tuiProjectDir}");
            Output.WriteLine($"构建目录: {buildDir}");
            
            if (!Directory.Exists(buildDir))
            {
                throw new DirectoryNotFoundException($"TUI构建目录不存在: {buildDir}。请先构建项目。");
            }
            
            // 查找TUI应用程序
            var files = Directory.GetFiles(buildDir, "BannerlordModEditor.TUI.*");
            Output.WriteLine($"找到的文件: {string.Join(", ", files)}");
            
            var tuiApp = files.FirstOrDefault(f => !f.EndsWith(".dll") && !f.EndsWith(".pdb"));
            
            if (tuiApp == null)
            {
                // 如果没有找到exe文件，使用dll文件
                tuiApp = files.FirstOrDefault(f => f.EndsWith(".dll"));
            }

            if (tuiApp == null)
            {
                throw new FileNotFoundException($"无法找到TUI应用程序，请先构建项目: {buildDir}");
            }

            Output.WriteLine($"TUI应用程序路径: {tuiApp}");
            return tuiApp;
        }

        public void Dispose()
        {
            try
            {
                // 清理tmux会话
                if (IsTmuxAvailable())
                {
                    ExecuteCommandAsync("tmux", $"kill-session -t {TestSessionName} 2>/dev/null || true").Wait();
                }
                
                // 清理测试目录
                if (Directory.Exists(TestTempDir))
                {
                    Directory.Delete(TestTempDir, true);
                    Output.WriteLine($"清理测试目录: {TestTempDir}");
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine($"清理过程中发生错误: {ex.Message}");
            }
            
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 进程执行结果
    /// </summary>
    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}