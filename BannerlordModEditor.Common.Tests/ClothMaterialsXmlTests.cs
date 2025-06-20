using BannerlordModEditor.Common.Models.Engine;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ClothMaterialsXmlTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void ClothMaterials_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_materials.xml");
            var serializer = new XmlSerializer(typeof(ClothMaterialsBase));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as ClothMaterialsBase;

            Assert.NotNull(result);
            Assert.NotNull(result.Materials);
            Assert.True(result.Materials.Any());
        }
    }
} 