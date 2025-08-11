using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsGeneralXmlTests
    {
        private const string TestDataPath = "TestData/particle_systems_general.xml";

        [Fact]
        public void ParticleSystemsGeneral_RoundTrip_StructuralEquality()
        {
            var xml = XmlTestUtils.ReadTestDataOrSkip(TestDataPath);
            if (xml == null) return; // Skip test if file doesn't exist

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            Assert.True(diff.IsStructurallyEqual, $"ParticleSystemsGeneral XML结构不一致。节点差异: {diff.NodeCountDifference}, 属性差异: {diff.AttributeCountDifference}, 属性值差异: {attributeValueDiffs}, 文本差异: {textDiffs}");
        }
    }
}