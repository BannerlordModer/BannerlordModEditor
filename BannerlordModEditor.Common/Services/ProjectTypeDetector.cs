using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BannerlordModEditor.Common.Services
{
    public enum ProjectType
    {
        Unknown,
        ButrTemplate,
        CustomTemplate,
        NativeMod
    }

    public class ProjectTypeInfo
    {
        public ProjectType Type { get; set; }
        public string ProjectPath { get; set; } = string.Empty;
        public bool HasSubmoduleXml { get; set; }
        public bool HasCsprojFile { get; set; }
        public bool HasButrMarker { get; set; }
        public string? ModName { get; set; }
        public string? ModuleDataPath { get; set; }
    }

    public interface IProjectTypeDetector
    {
        ProjectTypeInfo DetectProjectType(string projectPath);
        bool IsButrTemplateProject(string projectPath);
        string? GetModName(string projectPath);
    }

    public class ProjectTypeDetector : IProjectTypeDetector
    {
        private static readonly string[] ButrMarkers = new[]
        {
            "Bannerlord.ButterLib.dll",
            "Bannerlord.ButterLib",
            "BUTR",
            "BUTRLoadingScreen"
        };

        private static readonly string[] ButrDirectories = new[]
        {
            "BUTR",
            "ButterLib",
            "LoadingScreen"
        };

        public ProjectTypeInfo DetectProjectType(string projectPath)
        {
            var info = new ProjectTypeInfo
            {
                ProjectPath = projectPath,
                Type = ProjectType.Unknown
            };

            if (!Directory.Exists(projectPath))
                return info;

            info.HasSubmoduleXml = File.Exists(Path.Combine(projectPath, "SubModule.xml"));
            info.HasCsprojFile = Directory.GetFiles(projectPath, "*.csproj").Any();
            info.HasButrMarker = CheckForButrMarkers(projectPath);
            info.ModName = GetModName(projectPath);

            var moduleDataDir = Path.Combine(projectPath, "ModuleData");
            if (Directory.Exists(moduleDataDir))
            {
                info.ModuleDataPath = moduleDataDir;
            }

            if (info.HasButrMarker)
            {
                info.Type = ProjectType.ButrTemplate;
            }
            else if (info.HasSubmoduleXml && info.HasCsprojFile)
            {
                info.Type = ProjectType.CustomTemplate;
            }
            else if (info.HasSubmoduleXml)
            {
                info.Type = ProjectType.NativeMod;
            }

            return info;
        }

        public bool IsButrTemplateProject(string projectPath)
        {
            return DetectProjectType(projectPath).Type == ProjectType.ButrTemplate;
        }

        public string? GetModName(string projectPath)
        {
            if (!Directory.Exists(projectPath))
                return null;

            var csprojFiles = Directory.GetFiles(projectPath, "*.csproj");
            if (csprojFiles.Any())
            {
                var csprojPath = csprojFiles.First();
                var csprojName = Path.GetFileNameWithoutExtension(csprojPath);
                return csprojName;
            }

            var dirName = new DirectoryInfo(projectPath).Name;
            return dirName;
        }

        private bool CheckForButrMarkers(string projectPath)
        {
            var allFiles = GetAllFiles(projectPath);
            
            foreach (var file in allFiles)
            {
                var fileName = Path.GetFileName(file);
                if (ButrMarkers.Any(marker => fileName.Contains(marker, StringComparison.OrdinalIgnoreCase)))
                    return true;

                var content = SafeReadFile(file);
                if (content != null && content.Contains("BUTR", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            var directories = Directory.GetDirectories(projectPath, "*", SearchOption.AllDirectories);
            foreach (var dir in directories)
            {
                var dirName = new DirectoryInfo(dir).Name;
                if (ButrDirectories.Any(marker => dirName.Contains(marker, StringComparison.OrdinalIgnoreCase)))
                    return true;
            }

            return false;
        }

        private IEnumerable<string> GetAllFiles(string directory)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly));

                foreach (var subDir in Directory.GetDirectories(directory))
                {
                    var dirName = new DirectoryInfo(subDir).Name;
                    if (dirName != "bin" && dirName != "obj" && dirName != ".git")
                    {
                        files.AddRange(GetAllFiles(subDir));
                    }
                }
            }
            catch
            {
            }

            return files;
        }

        private string? SafeReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
