using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class AttributesXmlTests
    {
        private const string TestDataPath = "TestData/attributes.xml";

        [Fact]
        public void AttributesXml_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<AttributesDataModel>(xml);
            var serialized = XmlTestUtils.Serialize(model, xml); // Pass original XML to preserve namespaces

            // 节点和属性数量断言
            var (nodeCountA, attrCountA) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (nodeCountB, attrCountB) = XmlTestUtils.CountNodesAndAttributes(serialized);
            Assert.Equal(nodeCountA, nodeCountB);
            Assert.Equal(attrCountA, attrCountB);

            // 回退参数，保留原始结构比较，后续可扩展为更智能比较
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}