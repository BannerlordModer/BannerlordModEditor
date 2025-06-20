using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class BannerIconsXmlTests
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
        public void BannerIcons_RoundTripTest()
        {
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var originalXml = File.ReadAllText(filePath);

            var deserialized = XmlTestUtils.Deserialize<BannerIcons>(originalXml);
            Assert.NotNull(deserialized);

            var serializedXml = XmlTestUtils.Serialize(deserialized);
            
            var originalDoc = System.Xml.Linq.XDocument.Parse(originalXml);
            originalDoc.Root?.Attribute("type")?.Remove();
            var cleanedOriginalXml = originalDoc.ToString();
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(cleanedOriginalXml, serializedXml));
        }
    }
} 