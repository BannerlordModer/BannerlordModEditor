using BannerlordModEditor.Common.Models.Multiplayer;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class TauntUsageSetsXmlTests
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
        public void TauntUsageSets_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "taunt_usage_sets.xml");
            var serializer = new XmlSerializer(typeof(TauntUsageSets));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as TauntUsageSets;

            Assert.NotNull(result);
            Assert.NotNull(result.TauntUsageSet);
            Assert.True(result.TauntUsageSet.Any());
        }
    }
} 