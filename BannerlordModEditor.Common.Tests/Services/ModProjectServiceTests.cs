using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Models.SubModule;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class ModProjectServiceTests
    {
        private readonly ModProjectService _service;

        public ModProjectServiceTests()
        {
            _service = new ModProjectService(new SubModuleLoader());
        }

        [Fact]
        public void ValidateModName_ReturnsTrueForValidName()
        {
            Assert.True(_service.ValidateModName("MyMod"));
            Assert.True(_service.ValidateModName("My_Cool_Mod"));
            Assert.True(_service.ValidateModName("Mod123"));
        }

        [Fact]
        public void ValidateModName_ReturnsFalseForInvalidName()
        {
            Assert.False(_service.ValidateModName(""));
            Assert.False(_service.ValidateModName("A"));
            Assert.False(_service.ValidateModName("Mod with spaces"));
            Assert.False(_service.ValidateModName("Mod@Special"));
        }

        [Fact]
        public void GenerateModId_ReturnsCorrectId()
        {
            Assert.Equal("MyMod", _service.GenerateModId("My Mod"));
            Assert.Equal("MyCoolMod", _service.GenerateModId("My-Cool-Mod"));
            Assert.Equal("TestMod123", _service.GenerateModId("Test Mod 123"));
        }

        [Fact]
        public async Task CreateModProjectAsync_CreatesProjectStructure()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var options = new ModProjectOptions
                {
                    ModName = "TestMod",
                    ModId = "TestMod",
                    OutputDirectory = tempDir,
                    UseButrTemplate = true
                };

                var result = await _service.CreateModProjectAsync(options);

                Assert.True(result.Success);
                Assert.True(Directory.Exists(result.ProjectPath));
                Assert.True(File.Exists(Path.Combine(result.ProjectPath, "SubModule.xml")));
                Assert.True(File.Exists(Path.Combine(result.ProjectPath, "TestMod.csproj")));
                Assert.True(Directory.Exists(Path.Combine(result.ProjectPath, "ModuleData")));
                Assert.True(File.Exists(Path.Combine(result.ProjectPath, ".gitignore")));
                Assert.True(File.Exists(Path.Combine(result.ProjectPath, "README.md")));
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void CreateModProject_ReturnsErrorForInvalidName()
        {
            var options = new ModProjectOptions
            {
                ModName = "A",
                OutputDirectory = "/tmp"
            };

            var result = _service.CreateModProject(options);

            Assert.False(result.Success);
            Assert.Contains("Invalid mod name", result.ErrorMessage);
        }

        [Fact]
        public void CreateModProject_ReturnsErrorForExistingDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var modDir = Path.Combine(tempDir, "ExistingMod");
            Directory.CreateDirectory(modDir);

            try
            {
                var options = new ModProjectOptions
                {
                    ModName = "ExistingMod",
                    OutputDirectory = tempDir
                };

                var result = _service.CreateModProject(options);

                Assert.False(result.Success);
                Assert.Contains("already exists", result.ErrorMessage);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void CreateModProject_CreatesValidSubModuleXml()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var options = new ModProjectOptions
                {
                    ModName = "XmlTestMod",
                    ModId = "XmlTestMod",
                    OutputDirectory = tempDir,
                    DependedModules = new() { "Native", "Sandbox" }
                };

                var result = _service.CreateModProject(options);

                Assert.True(result.Success);

                var loader = new SubModuleLoader();
                var subModule = loader.Load(Path.Combine(result.ProjectPath, "SubModule.xml"));

                Assert.NotNull(subModule);
                Assert.Equal("XmlTestMod", subModule.Name);
                Assert.Equal("XmlTestMod", subModule.Id);
                Assert.Equal(2, subModule.DependedModules.Count);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }
    }
}
