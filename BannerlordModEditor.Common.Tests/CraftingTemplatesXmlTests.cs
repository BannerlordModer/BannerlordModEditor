using BannerlordModEditor.Common.Models.Game;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CraftingTemplatesXmlTests
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
        public void CraftingTemplates_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "crafting_templates.xml");
            var serializer = new XmlSerializer(typeof(CraftingTemplates));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as CraftingTemplates;

            Assert.NotNull(result);
            Assert.NotNull(result.CraftingTemplateList);
            Assert.True(result.CraftingTemplateList.Any());
        }
    }
} 