using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class BoneBodyTypesXmlTests
    {
        private const string TestDataPath = "TestData/bone_body_types.xml";

        [Fact]
        public void BoneBodyTypes_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<BoneBodyTypesDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"BoneBodyTypes XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }
    }
}