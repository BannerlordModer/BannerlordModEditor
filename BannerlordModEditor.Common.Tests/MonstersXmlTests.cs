using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MonstersXmlTests
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

        private string TestDataPath => Path.Combine(FindSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData");

        [Fact]
        public void Monsters_RoundTripTest()
        {
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var originalXml = File.ReadAllText(filePath);

            var deserialized = XmlTestUtils.Deserialize<Monsters>(originalXml);
            Assert.NotNull(deserialized);

            var serializedXml = XmlTestUtils.Serialize(deserialized);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }
    }
} 