using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    public class ProjectSettings
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectPath { get; set; } = string.Empty;
        public string GameVersion { get; set; } = "Latest";
        public string LastOpenedFile { get; set; } = string.Empty;
        public DateTime LastOpened { get; set; }
        public Dictionary<string, string> CustomSettings { get; set; } = new();
        public List<string> RecentFiles { get; set; } = new();
        public List<string> OpenTabs { get; set; } = new();
        public WindowSettings Window { get; set; } = new();
    }

    public class WindowSettings
    {
        public double Width { get; set; } = 1200;
        public double Height { get; set; } = 800;
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsMaximized { get; set; }
    }

    public interface IProjectSettingsService
    {
        Task<ProjectSettings> LoadAsync(string projectPath);
        ProjectSettings Load(string projectPath);
        Task SaveAsync(ProjectSettings settings, string projectPath);
        void Save(ProjectSettings settings, string projectPath);
        ProjectSettings GetDefault(string projectPath);
        string GetSettingsFilePath(string projectPath);
    }

    public class ProjectSettingsService : IProjectSettingsService
    {
        private const string SettingsFileName = ".bannerlordmodeditor.json";
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<ProjectSettings> LoadAsync(string projectPath)
        {
            return await Task.Run(() => Load(projectPath));
        }

        public ProjectSettings Load(string projectPath)
        {
            var settingsPath = GetSettingsFilePath(projectPath);

            if (!File.Exists(settingsPath))
                return GetDefault(projectPath);

            try
            {
                var json = File.ReadAllText(settingsPath);
                var settings = JsonSerializer.Deserialize<ProjectSettings>(json, JsonOptions);
                return settings ?? GetDefault(projectPath);
            }
            catch
            {
                return GetDefault(projectPath);
            }
        }

        public async Task SaveAsync(ProjectSettings settings, string projectPath)
        {
            await Task.Run(() => Save(settings, projectPath));
        }

        public void Save(ProjectSettings settings, string projectPath)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var settingsPath = GetSettingsFilePath(projectPath);
            var directory = Path.GetDirectoryName(settingsPath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            settings.ProjectPath = projectPath;
            settings.LastOpened = DateTime.Now;

            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(settingsPath, json);
        }

        public ProjectSettings GetDefault(string projectPath)
        {
            return new ProjectSettings
            {
                ProjectPath = projectPath,
                ProjectName = Path.GetFileName(projectPath),
                GameVersion = "Latest",
                LastOpened = DateTime.Now,
                RecentFiles = new List<string>(),
                OpenTabs = new List<string>(),
                Window = new WindowSettings()
            };
        }

        public string GetSettingsFilePath(string projectPath)
        {
            return Path.Combine(projectPath, SettingsFileName);
        }
    }
}
