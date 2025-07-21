using BannerlordModEditor.Common.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class FileDiscoveryServiceTests
    {
        private readonly string _testXmlDirectory;
        private readonly string _testModelsDirectory;
        private readonly FileDiscoveryService _service;

        public FileDiscoveryServiceTests()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            _testXmlDirectory = Path.Combine(solutionRoot, "example", "ModuleData");
            _testModelsDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common", "Models");
            _service = new FileDiscoveryService(_testXmlDirectory, _testModelsDirectory);
        }

        [Fact]
        public async Task FindUnadaptedFilesAsync_ShouldReturnUnadaptedFiles()
        {
            // Act
            var result = await _service.FindUnadaptedFilesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // 验证返回的文件确实是未适配的
            foreach (var file in result)
            {
                Assert.False(string.IsNullOrEmpty(file.FileName));
                Assert.False(string.IsNullOrEmpty(file.ExpectedModelName));
                Assert.True(File.Exists(file.FullPath));
            }
        }

        [Theory]
        [InlineData("some_file_name", "SomeFileName")]
        [InlineData("mp_items", "MpItems")]
        [InlineData("action_types", "ActionTypes")]
        [InlineData("simple_name", "SimpleName")]
        [InlineData("", "")]
        public void ConvertToModelName_ShouldConvertCorrectly(string input, string expected)
        {
            // Act
            var result = _service.ConvertToModelName(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertToModelName_WithXmlExtension_ShouldRemoveExtension()
        {
            // Act
            var result = _service.ConvertToModelName("test_file.xml");

            // Assert
            Assert.Equal("TestFile", result);
        }

        [Fact]
        public void ModelExists_WithExistingModel_ShouldReturnTrue()
        {
            // Arrange
            var searchDirs = new[] { _testModelsDirectory };

            // Act - 测试一个我们知道存在的模型
            var result = _service.ModelExists("Attributes", searchDirs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ModelExists_WithNonExistingModel_ShouldReturnFalse()
        {
            // Arrange
            var searchDirs = new[] { _testModelsDirectory };

            // Act
            var result = _service.ModelExists("NonExistentModel", searchDirs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task FindUnadaptedFilesAsync_ShouldOrderByComplexity()
        {
            // Act
            var result = await _service.FindUnadaptedFilesAsync();

            // Assert
            Assert.NotEmpty(result);
            
            // 验证排序：简单的文件应该在前面
            var complexities = result.Select(f => f.Complexity).ToList();
            var sortedComplexities = complexities.OrderBy(c => c).ToList();
            
            Assert.Equal(sortedComplexities, complexities);
        }

        [Fact]
        public async Task FindUnadaptedFilesAsync_ShouldSetCorrectFileProperties()
        {
            // Act
            var result = await _service.FindUnadaptedFilesAsync();

            // Assert
            Assert.NotEmpty(result);
            
            var firstFile = result.First();
            Assert.True(firstFile.FileSize > 0);
            Assert.True(File.Exists(firstFile.FullPath));
            Assert.Equal(Path.GetFileName(firstFile.FullPath), firstFile.FileName);
        }
    }
}