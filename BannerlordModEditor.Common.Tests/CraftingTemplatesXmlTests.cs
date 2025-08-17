using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CraftingTemplatesXmlTests
    {
        [Theory]
        [InlineData("TestData/crafting_templates.xml")]
        [InlineData("TestData/crafting_templates_part1.xml")]
        [InlineData("TestData/crafting_templates_part2.xml")]
        public void CraftingTemplates_RoundTrip_StructuralEquality(string filePath)
        {
            var xml = File.ReadAllText(filePath);
            var obj = XmlTestUtils.Deserialize<CraftingTemplatesDO>(xml);
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2), $"结构不一致: {filePath}");
        }
    }
}