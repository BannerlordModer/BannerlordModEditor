using System;
using System.Runtime.InteropServices;

namespace BannerlordModEditor.UI.Tests.Environment
{
    /// <summary>
    /// 环境信息帮助类
    /// 提供跨平台的环境信息和系统信息访问功能
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// 获取环境变量
        /// </summary>
        public static string? GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }

        /// <summary>
        /// 获取处理器数量
        /// </summary>
        public static int ProcessorCount => System.Environment.ProcessorCount;

        /// <summary>
        /// 获取是否为64位进程
        /// </summary>
        public static bool Is64BitProcess => System.Environment.Is64BitProcess;

        /// <summary>
        /// 获取特殊文件夹路径
        /// </summary>
        public static string GetFolderPath(System.Environment.SpecialFolder folder)
        {
            return System.Environment.GetFolderPath(folder);
        }

        /// <summary>
        /// 获取机器名称
        /// </summary>
        public static string MachineName => System.Environment.MachineName;

        /// <summary>
        /// 获取用户名称
        /// </summary>
        public static string UserName => System.Environment.UserName;

        /// <summary>
        /// 获取操作系统版本
        /// </summary>
        public static string OSVersion => System.Environment.OSVersion.ToString();

        /// <summary>
        /// 获取当前目录
        /// </summary>
        public static string CurrentDirectory => System.Environment.CurrentDirectory;

        /// <summary>
        /// 获取系统目录
        /// </summary>
        public static string SystemDirectory => System.Environment.SystemDirectory;

        /// <summary>
        /// 获取命令行参数
        /// </summary>
        public static string[] GetCommandLineArgs()
        {
            return System.Environment.GetCommandLineArgs();
        }

        /// <summary>
        /// 获取工作集内存
        /// </summary>
        public static long WorkingSet => System.Environment.WorkingSet;

        /// <summary>
        /// 获取系统已运行时间
        /// </summary>
        public static TimeSpan TickCount => TimeSpan.FromMilliseconds(System.Environment.TickCount64);

        /// <summary>
        /// 获取是否在交互模式下运行
        /// </summary>
        public static bool UserInteractive => System.Environment.UserInteractive;

        /// <summary>
        /// 获取当前进程ID
        /// </summary>
        public static int ProcessId => System.Environment.ProcessId;

        /// <summary>
        /// 获取当前线程ID
        /// </summary>
        public static int CurrentManagedThreadId => System.Environment.CurrentManagedThreadId;

        /// <summary>
        /// 获取域名
        /// </summary>
        public static string UserDomainName => System.Environment.UserDomainName;

        /// <summary>
        /// 获取堆栈跟踪
        /// </summary>
        public static string StackTrace => System.Environment.StackTrace;

        /// <summary>
        /// 获取换行符
        /// </summary>
        public static string NewLine => System.Environment.NewLine;

        /// <summary>
        /// 获取版本信息
        /// </summary>
        public static Version Version => System.Environment.Version;

        /// <summary>
        /// 检查是否为Windows平台
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// 检查是否为Linux平台
        /// </summary>
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// 检查是否为macOS平台
        /// </summary>
        public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// 获取操作系统描述
        /// </summary>
        public static string OSDescription => RuntimeInformation.OSDescription;

        /// <summary>
        /// 获取进程架构
        /// </summary>
        public static string ProcessArchitecture => RuntimeInformation.ProcessArchitecture.ToString();

        /// <summary>
        /// 获取系统架构
        /// </summary>
        public static string OSArchitecture => RuntimeInformation.OSArchitecture.ToString();

        /// <summary>
        /// 获取框架描述
        /// </summary>
        public static string FrameworkDescription => RuntimeInformation.FrameworkDescription;

        /// <summary>
        /// 获取所有环境变量
        /// </summary>
        public static System.Collections.IDictionary GetEnvironmentVariables()
        {
            return System.Environment.GetEnvironmentVariables();
        }

        /// <summary>
        /// 设置环境变量
        /// </summary>
        public static void SetEnvironmentVariable(string variable, string? value)
        {
            System.Environment.SetEnvironmentVariable(variable, value);
        }

        /// <summary>
        /// 展开环境变量
        /// </summary>
        public static string ExpandEnvironmentVariables(string name)
        {
            return System.Environment.ExpandEnvironmentVariables(name);
        }

        /// <summary>
        /// 获取当前进程的主模块文件名
        /// </summary>
        public static string GetProcessFileName()
        {
            using var process = System.Diagnostics.Process.GetCurrentProcess();
            try
            {
                return process.MainModule?.FileName ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取当前进程的工作目录
        /// </summary>
        public static string GetCurrentProcessDirectory()
        {
            try
            {
                using var process = System.Diagnostics.Process.GetCurrentProcess();
                return process.StartInfo.WorkingDirectory;
            }
            catch
            {
                return System.Environment.CurrentDirectory;
            }
        }
    }
}