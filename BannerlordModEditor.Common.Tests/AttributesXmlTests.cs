using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class AttributesXmlTests
    {
        private const string TestDataPath = "TestData/attributes.xml";

        [Fact]
        public void AttributesXml_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<AttributesDO>(xml);
            var serialized = XmlTestUtils.Serialize(model, xml); // Pass original XML to preserve namespaces

            // 节点和属性数量断言（使用非命名空间属性计数来忽略xmlns差异）
            var (nodeCountA, attrCountA) = XmlTestUtils.CountNodesAndNonNamespaceAttributes(xml);
            var (nodeCountB, attrCountB) = XmlTestUtils.CountNodesAndNonNamespaceAttributes(serialized);
            Assert.Equal(nodeCountA, nodeCountB);
            Assert.Equal(attrCountA, attrCountB);

            // 回退参数，保留原始结构比较，后续可扩展为更智能比较
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}