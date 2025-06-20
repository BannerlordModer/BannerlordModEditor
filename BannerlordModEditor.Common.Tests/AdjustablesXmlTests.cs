using BannerlordModEditor.Common.Models.Misc;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class AdjustablesXmlTests
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
        public void Adjustables_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "Adjustables.xml");
            var serializer = new XmlSerializer(typeof(Adjustables));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as Adjustables;

            Assert.NotNull(result);
            Assert.NotNull(result.AdjustableList);
            Assert.True(result.AdjustableList.Any());
            Assert.Equal(33, result.AdjustableList.Count);
        }
    }
} 