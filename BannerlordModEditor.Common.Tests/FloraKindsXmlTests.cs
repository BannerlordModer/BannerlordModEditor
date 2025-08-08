using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsXmlTests
    {
        [Theory]
        [InlineData("TestData/flora_kinds.xml")]
        public void FloraKinds_RoundTrip_StructuralEquality(string xmlPath)
        {
            var xml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKinds>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            // 回退参数，保留原始结构比较，后续可扩展为更智能比较
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}