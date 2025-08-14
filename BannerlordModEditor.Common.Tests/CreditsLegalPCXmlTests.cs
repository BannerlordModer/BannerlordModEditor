using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsLegalPCXmlTests
    {
        private const string TestDataPath = "TestData/CreditsLegalPC.xml";

        [Fact]
        public void CreditsLegalPC_RoundTrip_StructuralEquality()
        {
            var fixedXml = File.ReadAllText(TestDataPath)
                .Replace("G. \"Anteru\" Chajdas", "G. &quot;Anteru&quot; Chajdas")
                .Replace("\"AS IS\"", "&quot;AS IS&quot;")
                .Replace("``AS IS''", "&quot;AS IS&quot;");

            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsLegalPC>(fixedXml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(model, fixedXml);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(fixedXml, xml2));
        }
    }
}