using BannerlordModEditor.Common.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class FileDiscoveryServiceIntegrationTests
    {
        [Fact]
        public async Task FileDiscoveryService_ShouldFindKnownUnadaptedFiles()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlDirectory = Path.Combine(solutionRoot, "example", "ModuleData");
            var modelsDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common", "Models");
            var service = new FileDiscoveryService(xmlDirectory, modelsDirectory);

            // Act
            var result = await service.FindUnadaptedFilesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            // 验证一些我们知道未适配的文件
            var fileNames = result.Select(f => f.FileName).ToList();
            
            Assert.Contains("looknfeel.xml", fileNames);
            Assert.Contains("before_transparents_graph.xml", fileNames);
            Assert.Contains("particle_systems2.xml", fileNames);

            // 验证文件属性设置正确
            foreach (var file in result)
            {
                Assert.True(file.FileSize > 0);
                Assert.False(string.IsNullOrEmpty(file.ExpectedModelName));
                Assert.True(File.Exists(file.FullPath));
            }

            // 验证命名转换正确
            var looknfeelFile = result.FirstOrDefault(f => f.FileName == "looknfeel.xml");
            Assert.NotNull(looknfeelFile);
            Assert.Equal("LookAndFeel", looknfeelFile.ExpectedModelName);

            var particleFile = result.FirstOrDefault(f => f.FileName == "particle_systems2.xml");
            Assert.NotNull(particleFile);
            Assert.Equal("ParticleSystems2", particleFile.ExpectedModelName);
        }

        [Fact]
        public async Task FileDiscoveryService_ShouldNotIncludeAdaptedFiles()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlDirectory = Path.Combine(solutionRoot, "example", "ModuleData");
            var modelsDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common", "Models");
            var service = new FileDiscoveryService(xmlDirectory, modelsDirectory);

            // Act
            var result = await service.FindUnadaptedFilesAsync();

            // Assert
            var fileNames = result.Select(f => f.FileName).ToList();
            
            // 这些文件应该已经被适配了，不应该出现在结果中
            Assert.DoesNotContain("attributes.xml", fileNames);
            Assert.DoesNotContain("skills.xml", fileNames);
            Assert.DoesNotContain("decal_textures_all.xml", fileNames);
        }
    }
}