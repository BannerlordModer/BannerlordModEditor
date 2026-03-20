using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    public class SdkInfo
    {
        public bool IsInstalled { get; set; }
        public string? Version { get; set; }
        public string? Path { get; set; }
        public List<string> InstalledVersions { get; set; } = new();
        public string? RecommendedVersion { get; set; }
        public string? DownloadUrl { get; set; }
    }

    public interface ISdkCheckerService
    {
        Task<SdkInfo> CheckSdkAsync();
        SdkInfo CheckSdk();
        bool IsSdkInstalled();
        string? GetSdkVersion();
        List<string> GetInstalledVersions();
        string GetDownloadUrl();
    }

    public class SdkCheckerService : ISdkCheckerService
    {
        private const string RecommendedSdkVersion = "9.0.100";
        private const string DownloadUrlBase = "https://dotnet.microsoft.com/download/dotnet/9.0";

        public async Task<SdkInfo> CheckSdkAsync()
        {
            return await Task.Run(() => CheckSdk());
        }

        public SdkInfo CheckSdk()
        {
            var info = new SdkInfo
            {
                IsInstalled = IsSdkInstalled(),
                Version = GetSdkVersion(),
                Path = GetSdkPath(),
                InstalledVersions = GetInstalledVersions(),
                RecommendedVersion = RecommendedSdkVersion,
                DownloadUrl = GetDownloadUrl()
            };

            return info;
        }

        public bool IsSdkInstalled()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output);
            }
            catch
            {
                return false;
            }
        }

        public string? GetSdkVersion()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                return process.ExitCode == 0 ? output : null;
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetInstalledVersions()
        {
            var versions = new List<string>();

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--list-sdks",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    {
                        var parts = line.Trim().Split(' ');
                        if (parts.Length > 0)
                        {
                            versions.Add(parts[0]);
                        }
                    }
                }
            }
            catch
            {
            }

            return versions;
        }

        public string GetDownloadUrl()
        {
            return DownloadUrlBase;
        }

        private string? GetSdkPath()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--list-sdks",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    {
                        var bracketIndex = line.IndexOf('[');
                        if (bracketIndex > 0)
                        {
                            var path = line[(bracketIndex + 1)..].TrimEnd(']');
                            if (Directory.Exists(path))
                            {
                                return path;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
