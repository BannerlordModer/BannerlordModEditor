using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class ProjectSettingsServiceTests
    {
        private readonly ProjectSettingsService _service;

        public ProjectSettingsServiceTests()
        {
            _service = new ProjectSettingsService();
        }

        [Fact]
        public void GetDefault_CreatesValidDefault()
        {
            var result = _service.GetDefault("/test/path");

            Assert.Equal("/test/path", result.ProjectPath);
            Assert.Equal("path", result.ProjectName);
            Assert.Equal("Latest", result.GameVersion);
            Assert.Empty(result.RecentFiles);
            Assert.Empty(result.OpenTabs);
            Assert.NotNull(result.Window);
        }

        [Fact]
        public void GetSettingsFilePath_ReturnsCorrectPath()
        {
            var result = _service.GetSettingsFilePath("/test/project");

            Assert.Equal("/test/project/.bannerlordmodeditor.json", result);
        }

        [Fact]
        public void Load_ReturnsDefaultForNonExistentFile()
        {
            var result = _service.Load("/nonexistent/path");

            Assert.NotNull(result);
            Assert.Equal("/nonexistent/path", result.ProjectPath);
        }

        [Fact]
        public async Task LoadAsync_ReturnsDefaultForNonExistentFile()
        {
            var result = await _service.LoadAsync("/nonexistent/path");

            Assert.NotNull(result);
            Assert.Equal("/nonexistent/path", result.ProjectPath);
        }

        [Fact]
        public void Save_CreatesSettingsFile()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var settings = new ProjectSettings
            {
                ProjectName = "Test Project",
                GameVersion = "v1.2.9"
            };

            try
            {
                _service.Save(settings, tempDir);

                var settingsPath = Path.Combine(tempDir, ".bannerlordmodeditor.json");
                Assert.True(File.Exists(settingsPath));

                var loaded = _service.Load(tempDir);
                Assert.Equal("Test Project", loaded.ProjectName);
                Assert.Equal("v1.2.9", loaded.GameVersion);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public async Task SaveAsync_CreatesSettingsFile()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var settings = _service.GetDefault(tempDir);
            settings.ProjectName = "Async Test";

            try
            {
                await _service.SaveAsync(settings, tempDir);

                var loaded = await _service.LoadAsync(tempDir);
                Assert.Equal("Async Test", loaded.ProjectName);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void Save_PreservesRecentFiles()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var settings = new ProjectSettings
            {
                ProjectName = "Test",
                RecentFiles = new System.Collections.Generic.List<string>
                {
                    "/path/to/file1.xml",
                    "/path/to/file2.xml"
                }
            };

            try
            {
                _service.Save(settings, tempDir);

                var loaded = _service.Load(tempDir);
                Assert.Equal(2, loaded.RecentFiles.Count);
                Assert.Contains("/path/to/file1.xml", loaded.RecentFiles);
                Assert.Contains("/path/to/file2.xml", loaded.RecentFiles);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }
    }
}
