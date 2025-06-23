using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Configuration;
using Xunit;
using System.Linq;

namespace BannerlordModEditor.Common.Tests
{
    public class ProjectConfigurationXmlTests
    {
        [Fact]
        public void ProjectConfiguration_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""solution"">
  <outputDirectory>../MBModule/MBModule/</outputDirectory>
  <XMLDirectory>../WOTS/Modules/Native/</XMLDirectory>
  <ModuleAssemblyDirectory>../WOTS/bin/</ModuleAssemblyDirectory>
  <file id=""soln_strings"" name=""ModuleData/strings.xml"" type=""string"" />
  <file id=""soln_postfx"" name=""ModuleData/postfx_graphs.xml"" type=""postfx"" />
</base>";

            var serializer = new XmlSerializer(typeof(ProjectConfiguration));

            // Act
            ProjectConfiguration? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ProjectConfiguration?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("solution", result.Type);
            Assert.Equal("../MBModule/MBModule/", result.OutputDirectory);
            Assert.Equal("../WOTS/Modules/Native/", result.XmlDirectory);
            Assert.Equal("../WOTS/bin/", result.ModuleAssemblyDirectory);
            Assert.NotNull(result.Files);
            Assert.Equal(2, result.Files.Count);

            // Test first file
            var stringsFile = result.Files[0];
            Assert.Equal("soln_strings", stringsFile.Id);
            Assert.Equal("ModuleData/strings.xml", stringsFile.Name);
            Assert.Equal("string", stringsFile.Type);

            // Test second file
            var postfxFile = result.Files[1];
            Assert.Equal("soln_postfx", postfxFile.Id);
            Assert.Equal("ModuleData/postfx_graphs.xml", postfxFile.Name);
            Assert.Equal("postfx", postfxFile.Type);
        }

        [Fact]
        public void ProjectConfiguration_FromActualFile_CanDeserializeCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "project.mbproj");
            
            // Act
            var serializer = new XmlSerializer(typeof(ProjectConfiguration));
            ProjectConfiguration result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ProjectConfiguration)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("solution", result.Type);
            Assert.Equal("../MBModule/MBModule/", result.OutputDirectory);
            Assert.Equal("../WOTS/Modules/Native/", result.XmlDirectory);
            Assert.Equal("../WOTS/bin/", result.ModuleAssemblyDirectory);
            Assert.NotNull(result.Files);
            
            // 验证文件数量大于40个（实际有47个文件）
            Assert.True(result.Files.Count >= 40);
            
            // 验证所有文件都有有效的ID、名称和类型
            foreach (var file in result.Files)
            {
                Assert.False(string.IsNullOrWhiteSpace(file.Id));
                Assert.False(string.IsNullOrWhiteSpace(file.Name));
                Assert.False(string.IsNullOrWhiteSpace(file.Type));
                Assert.True(file.Name!.StartsWith("ModuleData/"));
            }
            
            // 验证特定的重要文件存在 (注意：某些ID可能重复)
            var stringFiles = result.Files.Where(f => f.Id == "soln_strings").ToList();
            var postfxFiles = result.Files.Where(f => f.Id == "soln_postfx").ToList();
            
            // 验证字符串文件
            Assert.True(stringFiles.Count >= 1);
            var stringsFile = stringFiles.First();
            Assert.Equal("ModuleData/strings.xml", stringsFile.Name);
            Assert.Equal("string", stringsFile.Type);
            
            // 验证后处理效果文件
            Assert.True(postfxFiles.Count >= 1);
            var postfxFile = postfxFiles.First();
            Assert.Equal("ModuleData/postfx_graphs.xml", postfxFile.Name);
            Assert.Equal("postfx", postfxFile.Type);
            
            // 验证战斗参数文件
            var combatFiles = result.Files.Where(f => f.Id == "soln_combat_system").ToList();
            Assert.True(combatFiles.Count >= 2); // 应该有combat_parameters.xml和native_parameters.xml
            
            // 验证粒子系统文件
            var particleFiles = result.Files.Where(f => f.Id == "soln_particle_systems").ToList();
            Assert.True(particleFiles.Count >= 5); // 应该有多个粒子系统文件
        }
        
        [Fact]
        public void ProjectConfiguration_CanSerializeToXml()
        {
            // Arrange
            var config = new ProjectConfiguration
            {
                Type = "solution",
                OutputDirectory = "../Test/Output/",
                XmlDirectory = "../Test/XML/",
                ModuleAssemblyDirectory = "../Test/Assembly/",
                Files = new List<ProjectFile>
                {
                    new ProjectFile { Id = "test_file1", Name = "ModuleData/test1.xml", Type = "test_type1" },
                    new ProjectFile { Id = "test_file2", Name = "ModuleData/test2.xml", Type = "test_type2" }
                }
            };

            var serializer = new XmlSerializer(typeof(ProjectConfiguration));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, config);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains(@"type=""solution""", result);
            Assert.Contains("<outputDirectory>../Test/Output/</outputDirectory>", result);
            Assert.Contains("<XMLDirectory>../Test/XML/</XMLDirectory>", result);
            Assert.Contains("<ModuleAssemblyDirectory>../Test/Assembly/</ModuleAssemblyDirectory>", result);
            Assert.Contains(@"id=""test_file1""", result);
            Assert.Contains(@"name=""ModuleData/test1.xml""", result);
            Assert.Contains(@"type=""test_type1""", result);
        }
        
        [Fact]
        public void ProjectConfiguration_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "project.mbproj");
            
            var serializer = new XmlSerializer(typeof(ProjectConfiguration));
            
            // Act - 反序列化
            ProjectConfiguration original;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (ProjectConfiguration)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化后再反序列化
            ProjectConfiguration roundTrip;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                using (var reader = new StringReader(writer.ToString()))
                {
                    roundTrip = (ProjectConfiguration)serializer.Deserialize(reader)!;
                }
            }
            
            // Assert - 验证数据一致性
            Assert.Equal(original.Type, roundTrip.Type);
            Assert.Equal(original.OutputDirectory, roundTrip.OutputDirectory);
            Assert.Equal(original.XmlDirectory, roundTrip.XmlDirectory);
            Assert.Equal(original.ModuleAssemblyDirectory, roundTrip.ModuleAssemblyDirectory);
            Assert.Equal(original.Files.Count, roundTrip.Files.Count);
            
            for (int i = 0; i < original.Files.Count; i++)
            {
                Assert.Equal(original.Files[i].Id, roundTrip.Files[i].Id);
                Assert.Equal(original.Files[i].Name, roundTrip.Files[i].Name);
                Assert.Equal(original.Files[i].Type, roundTrip.Files[i].Type);
            }
        }
    }
}