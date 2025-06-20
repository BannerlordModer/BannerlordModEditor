using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkinnedDecalsXmlTests
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
        public void SkinnedDecals_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skinned_decals.xml");
            var serializer = new XmlSerializer(typeof(SkinnedDecalsBase));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as SkinnedDecalsBase;

            Assert.NotNull(result);
            Assert.NotNull(result.SkinnedDecals);
            Assert.NotNull(result.SkinnedDecals.SkinnedDecalList);
            Assert.True(result.SkinnedDecals.SkinnedDecalList.Any());
        }
    }
} 