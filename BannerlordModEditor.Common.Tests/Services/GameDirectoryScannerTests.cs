using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class GameDirectoryScannerTests
    {
        private readonly GameDirectoryScanner _scanner;

        public GameDirectoryScannerTests()
        {
            _scanner = new GameDirectoryScanner();
        }

        [Fact]
        public async Task ScanForGameDirectoriesAsync_ReturnsEmptyListWhenNoGameInstalled()
        {
            var result = await _scanner.ScanForGameDirectoriesAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValidGameDirectory_ReturnsFalseForNullPath()
        {
            var result = _scanner.IsValidGameDirectory(null!);
            Assert.False(result);
        }

        [Fact]
        public void IsValidGameDirectory_ReturnsFalseForEmptyPath()
        {
            var result = _scanner.IsValidGameDirectory(string.Empty);
            Assert.False(result);
        }

        [Fact]
        public void IsValidGameDirectory_ReturnsFalseForNonExistentPath()
        {
            var result = _scanner.IsValidGameDirectory("/nonexistent/path");
            Assert.False(result);
        }

        [Fact]
        public void IsValidGameDirectory_ReturnsFalseForDirectoryWithoutModuleData()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var result = _scanner.IsValidGameDirectory(tempDir);
                Assert.False(result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void IsValidGameDirectory_ReturnsTrueForValidBannerlordDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var modulesDir = Path.Combine(tempDir, "Modules");
            var nativeDir = Path.Combine(modulesDir, "Native");

            Directory.CreateDirectory(nativeDir);

            try
            {
                var result = _scanner.IsValidGameDirectory(tempDir);
                Assert.True(result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GetGameVersion_ReturnsNullForNonExistentDirectory()
        {
            var result = _scanner.GetGameVersion("/nonexistent/path");
            Assert.Null(result);
        }

        [Fact]
        public void GetGameVersion_ReadsFromVersionFile()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var versionFile = Path.Combine(tempDir, "version.txt");
                File.WriteAllText(versionFile, "1.2.9.0");

                var result = _scanner.GetGameVersion(tempDir);
                Assert.Equal("1.2.9.0", result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GetGameVersion_ReadsFromConfigFile()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var configFile = Path.Combine(tempDir, "config.txt");
                File.WriteAllText(configFile, "version=1.3.15.0\nother_setting=value");

                var result = _scanner.GetGameVersion(tempDir);
                Assert.Equal("1.3.15.0", result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
