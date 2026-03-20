using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class ProjectTypeDetectorTests
    {
        private readonly ProjectTypeDetector _detector;

        public ProjectTypeDetectorTests()
        {
            _detector = new ProjectTypeDetector();
        }

        [Fact]
        public void DetectProjectType_ReturnsUnknownForNonExistentPath()
        {
            var result = _detector.DetectProjectType("/nonexistent/path");
            Assert.Equal(ProjectType.Unknown, result.Type);
        }

        [Fact]
        public void DetectProjectType_ReturnsNativeModForSubmoduleOnly()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                File.WriteAllText(Path.Combine(tempDir, "SubModule.xml"), "<Module></Module>");

                var result = _detector.DetectProjectType(tempDir);
                Assert.Equal(ProjectType.NativeMod, result.Type);
                Assert.True(result.HasSubmoduleXml);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void DetectProjectType_ReturnsCustomTemplateForSubmoduleAndCsproj()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                File.WriteAllText(Path.Combine(tempDir, "SubModule.xml"), "<Module></Module>");
                File.WriteAllText(Path.Combine(tempDir, "TestMod.csproj"), "<Project></Project>");

                var result = _detector.DetectProjectType(tempDir);
                Assert.Equal(ProjectType.CustomTemplate, result.Type);
                Assert.True(result.HasSubmoduleXml);
                Assert.True(result.HasCsprojFile);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void DetectProjectType_ReturnsButrTemplateForButrMarker()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var butrDir = Path.Combine(tempDir, "BUTR");
            Directory.CreateDirectory(butrDir);

            try
            {
                File.WriteAllText(Path.Combine(tempDir, "SubModule.xml"), "<Module></Module>");
                File.WriteAllText(Path.Combine(tempDir, "TestMod.csproj"), "<Project></Project>");
                File.WriteAllText(Path.Combine(butrDir, "Bannerlord.ButterLib.dll"), "");

                var result = _detector.DetectProjectType(tempDir);
                Assert.Equal(ProjectType.ButrTemplate, result.Type);
                Assert.True(result.HasButrMarker);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void IsButrTemplateProject_ReturnsTrueForButrProject()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var butrDir = Path.Combine(tempDir, "BUTR");
            Directory.CreateDirectory(butrDir);

            try
            {
                File.WriteAllText(Path.Combine(butrDir, "marker.txt"), "BUTR marker");

                var result = _detector.IsButrTemplateProject(tempDir);
                Assert.True(result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GetModName_ReturnsCsprojName()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                File.WriteAllText(Path.Combine(tempDir, "MyCoolMod.csproj"), "<Project></Project>");

                var result = _detector.GetModName(tempDir);
                Assert.Equal("MyCoolMod", result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GetModName_ReturnsDirectoryNameWhenNoCsproj()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "TestModProject");
            Directory.CreateDirectory(tempDir);

            try
            {
                var result = _detector.GetModName(tempDir);
                Assert.Equal("TestModProject", result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
