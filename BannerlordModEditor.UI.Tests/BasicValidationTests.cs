using Xunit;
using System.IO;
using System.Reflection;

namespace BannerlordModEditor.UI.Tests
{
    /// <summary>
    /// 基本功能验证测试
    /// 
    /// 这是一个简化的测试套件，用于验证最基本的测试功能是否正常工作。
    /// 主要验证：
    /// - 测试数据文件是否存在
    /// - 基本的文件操作
    /// - 项目结构
    /// </summary>
    public class BasicValidationTests
    {
        [Fact]
        public void TestDataDirectory_Should_Exist()
        {
            // Arrange
            var testDataDir = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            
            // Act & Assert
            Assert.True(Directory.Exists(testDataDir), $"测试数据目录不存在: {testDataDir}");
        }

        [Fact]
        public void TestDataFiles_Should_Exist()
        {
            // Arrange
            var testDataDir = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            var expectedFiles = new[] { "attributes.xml", "skills.xml", "bone_body_types.xml" };
            
            // Act & Assert
            foreach (var file in expectedFiles)
            {
                var filePath = Path.Combine(testDataDir, file);
                Assert.True(File.Exists(filePath), $"测试数据文件不存在: {filePath}");
            }
        }

        [Fact]
        public void ProjectStructure_Should_Be_Valid()
        {
            // Arrange - 使用程序集位置来获取项目根目录
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var projectDir = Path.GetDirectoryName(Path.GetDirectoryName(assemblyLocation));
            
            // Act & Assert
            Assert.True(Directory.Exists(Path.Combine(projectDir, "Helpers")), "Helpers目录不存在");
            Assert.True(Directory.Exists(Path.Combine(projectDir, "ViewModels")), "ViewModels目录不存在");
            Assert.True(Directory.Exists(Path.Combine(projectDir, "Integration")), "Integration目录不存在");
        }

        [Fact]
        public void XmlFiles_Should_Be_Readable()
        {
            // Arrange
            var testDataDir = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            var testFile = Path.Combine(testDataDir, "attributes.xml");
            
            // Act
            var content = File.ReadAllText(testFile);
            
            // Assert
            Assert.False(string.IsNullOrEmpty(content), "XML文件内容为空");
            Assert.Contains("<?xml", content, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void TestRunner_Should_Work()
        {
            // Arrange & Act & Assert
            Assert.True(true, "测试运行器正常工作");
        }
    }
}