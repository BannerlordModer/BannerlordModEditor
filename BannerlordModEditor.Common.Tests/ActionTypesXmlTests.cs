using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionTypesXmlTests
    {
        private const string MainXmlPath = "TestData/action_types.xml";
        private const string SubsetDir = "BannerlordModEditor.Common.Tests/TestSubsets/ActionTypes/";

        [Fact]
        public void ActionTypes_MainXml_RoundTrip_StructuralEquality()
        {
            string xml = File.ReadAllText(MainXmlPath);
            var obj = XmlTestUtils.Deserialize<ActionTypesDO>(xml);
            string serialized = XmlTestUtils.Serialize(obj);
            
            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"ActionTypes XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }

        [Theory]
        [MemberData(nameof(GetSubsetFiles))]
        public void ActionTypes_SubsetXmls_RoundTrip_StructuralEquality(string subsetPath)
        {
            string xml = File.ReadAllText(subsetPath);
            var obj = XmlTestUtils.Deserialize<ActionTypesDO>(xml);
            string serialized = XmlTestUtils.Serialize(obj);
            
            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"ActionTypes Subset XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }

        public static System.Collections.Generic.IEnumerable<object[]> GetSubsetFiles()
        {
            if (!Directory.Exists(SubsetDir))
                yield break;

            var files = Directory.GetFiles(SubsetDir, "*.xml");
            foreach (var file in files.OrderBy(f => f))
            {
                yield return new object[] { file };
            }
        }
    }
}