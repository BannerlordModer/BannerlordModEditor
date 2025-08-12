using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CombatParametersXmlTests
    {
        [Fact]
        public void CombatParameters_RoundTrip_StructuralEquality()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters.xml");
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"CombatParameters XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }

        [Theory]
        [InlineData("TestData/combat_parameters_part1.xml")]
        [InlineData("TestData/combat_parameters_part2.xml")]
        public void CombatParameters_PartFiles_RoundTrip_StructuralEquality(string xmlPath)
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var fullPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", xmlPath);
            var xml = File.ReadAllText(fullPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"CombatParameters Part XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }
    }
}