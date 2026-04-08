using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class ConceptStringsRoundtripTests
    {
        [Fact]
        public async Task ConceptStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "concept_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConceptStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task ConceptStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "concept_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConceptStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task ConceptStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "concept_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var conceptStrings = XmlTestUtils.Deserialize<ConceptStrings>(xmlContent);

            Assert.NotNull(conceptStrings);
            Assert.NotNull(conceptStrings.Strings);
            Assert.True(conceptStrings.Strings.Count > 0);
        }
    }
}
