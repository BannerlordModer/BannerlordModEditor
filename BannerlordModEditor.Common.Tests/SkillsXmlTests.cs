using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Models;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkillsXmlTests
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
        public void Skills_RoundTripTest()
        {
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var originalXml = File.ReadAllText(filePath);

            var deserialized = XmlTestUtils.Deserialize<Skills>(originalXml);
            Assert.NotNull(deserialized);

            var serializedXml = XmlTestUtils.Serialize(deserialized);
            
            // The XML declaration and namespaces might differ slightly,
            // so we rely on structural comparison.
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }
    }
} 