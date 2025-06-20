using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ScenesXmlTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(System.AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        private string TestDataPath => Path.Combine(FindSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData");

        [Fact]
        public void Scenes_RoundTripTest()
        {
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var originalXml = File.ReadAllText(filePath);

            var deserialized = XmlTestUtils.Deserialize<Scenes>(originalXml);
            Assert.NotNull(deserialized);

            var serializedXml = XmlTestUtils.Serialize(deserialized);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }
    }
} 