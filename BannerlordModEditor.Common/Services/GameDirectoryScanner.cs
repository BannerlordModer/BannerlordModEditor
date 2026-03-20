using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    public class GameDirectoryScanner : IGameDirectoryScanner
    {
        private const string BannerlordAppId = "261550";
        private const string BannerlordFolderName = "Mount & Blade II Bannerlord";
        private const string ModuleDataRelativePath = "Modules";

        public async Task<List<GameDirectoryInfo>> ScanForGameDirectoriesAsync()
        {
            return await Task.Run(() =>
            {
                var results = new List<GameDirectoryInfo>();
                
                var steamPaths = GetSteamLibraryPaths();
                foreach (var steamPath in steamPaths)
                {
                    var bannerlordPath = Path.Combine(steamPath, "steamapps", "common", BannerlordFolderName);
                    if (Directory.Exists(bannerlordPath))
                    {
                        var info = CreateGameDirectoryInfo(bannerlordPath, "Steam");
                        results.Add(info);
                    }
                }

                var gogPaths = GetGogPaths();
                foreach (var gogPath in gogPaths)
                {
                    if (Directory.Exists(gogPath))
                    {
                        var info = CreateGameDirectoryInfo(gogPath, "GOG");
                        results.Add(info);
                    }
                }

                var epicPaths = GetEpicPaths();
                foreach (var epicPath in epicPaths)
                {
                    if (Directory.Exists(epicPath))
                    {
                        var info = CreateGameDirectoryInfo(epicPath, "Epic");
                        results.Add(info);
                    }
                }

                return results.Where(r => r.IsValid).ToList();
            });
        }

        public async Task<string?> GetFirstGameDirectoryAsync()
        {
            var directories = await ScanForGameDirectoriesAsync();
            return directories.FirstOrDefault()?.Path;
        }

        public bool IsValidGameDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return false;

            var moduleDataPath = Path.Combine(path, ModuleDataRelativePath);
            if (!Directory.Exists(moduleDataPath))
                return false;

            var nativeModulePath = Path.Combine(moduleDataPath, "Native");
            return Directory.Exists(nativeModulePath);
        }

        public string? GetGameVersion(string gameDirectory)
        {
            if (string.IsNullOrEmpty(gameDirectory) || !Directory.Exists(gameDirectory))
                return null;

            var versionFile = Path.Combine(gameDirectory, "version.txt");
            if (File.Exists(versionFile))
            {
                return File.ReadAllText(versionFile).Trim();
            }

            var configFile = Path.Combine(gameDirectory, "config.txt");
            if (File.Exists(configFile))
            {
                var lines = File.ReadAllLines(configFile);
                var versionLine = lines.FirstOrDefault(l => l.StartsWith("version=", StringComparison.OrdinalIgnoreCase));
                if (versionLine != null)
                {
                    return versionLine["version=".Length..].Trim();
                }
            }

            return null;
        }

        private GameDirectoryInfo CreateGameDirectoryInfo(string path, string source)
        {
            var info = new GameDirectoryInfo
            {
                Path = path,
                Source = source,
                IsValid = IsValidGameDirectory(path),
                ModuleDataPath = Path.Combine(path, ModuleDataRelativePath),
                Version = GetGameVersion(path)
            };
            return info;
        }

        private List<string> GetSteamLibraryPaths()
        {
            var paths = new List<string>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                paths.Add(Path.Combine(programFiles, "Steam"));

                var defaultPaths = new[]
                {
                    @"C:\Program Files (x86)\Steam",
                    @"C:\Program Files\Steam",
                    @"D:\Steam",
                    @"E:\Steam",
                    @"F:\Steam"
                };
                paths.AddRange(defaultPaths);

                var steamConfigFile = Path.Combine(programFiles, "Steam", "steamapps", "libraryfolders.vdf");
                paths.AddRange(ParseSteamLibraryFolders(steamConfigFile));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                paths.Add(Path.Combine(home, ".steam", "steam"));
                paths.Add(Path.Combine(home, ".local", "share", "Steam"));

                var steamConfigFile = Path.Combine(home, ".steam", "steam", "steamapps", "libraryfolders.vdf");
                paths.AddRange(ParseSteamLibraryFolders(steamConfigFile));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                paths.Add(Path.Combine(home, "Library", "Application Support", "Steam"));

                var steamConfigFile = Path.Combine(home, "Library", "Application Support", "Steam", "steamapps", "libraryfolders.vdf");
                paths.AddRange(ParseSteamLibraryFolders(steamConfigFile));
            }

            return paths.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
        }

        private List<string> ParseSteamLibraryFolders(string configFile)
        {
            var paths = new List<string>();

            if (!File.Exists(configFile))
                return paths;

            try
            {
                var lines = File.ReadAllLines(configFile);
                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("\"path\"", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = trimmed.Split('"', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            var path = parts[3].Replace("\\\\", "\\");
                            if (Directory.Exists(path))
                            {
                                paths.Add(path);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return paths;
        }

        private List<string> GetGogPaths()
        {
            var paths = new List<string>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

                paths.Add(Path.Combine(programFiles, "GOG Galaxy", "Games", BannerlordFolderName));
                paths.Add(Path.Combine(programFilesX86, "GOG Galaxy", "Games", BannerlordFolderName));
            }

            return paths;
        }

        private List<string> GetEpicPaths()
        {
            var paths = new List<string>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                paths.Add(Path.Combine(programFiles, "Epic Games", BannerlordFolderName));

                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var epicManifestsDir = Path.Combine(localAppData, "EpicGamesLauncher", "Saved", "Config", "Windows");
                
                if (Directory.Exists(epicManifestsDir))
                {
                    var manifestFiles = Directory.GetFiles(epicManifestsDir, "*.item");
                    foreach (var manifest in manifestFiles)
                    {
                        try
                        {
                            var content = File.ReadAllText(manifest);
                            if (content.Contains("Mount", StringComparison.OrdinalIgnoreCase) && 
                                content.Contains("Blade", StringComparison.OrdinalIgnoreCase))
                            {
                                var installDirMatch = System.Text.RegularExpressions.Regex.Match(content, @"""InstallLocation""\s*:\s*""([^""]+)""");
                                if (installDirMatch.Success)
                                {
                                    var installPath = installDirMatch.Groups[1].Value.Replace("\\\\", "\\");
                                    if (Directory.Exists(installPath))
                                    {
                                        paths.Add(installPath);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return paths;
        }
    }
}
