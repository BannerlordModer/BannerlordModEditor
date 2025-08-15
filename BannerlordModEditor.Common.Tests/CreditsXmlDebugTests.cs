using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsXmlDebugTests
    {
        [Fact]
        public void DebugCreditsXmlIssue()
        {
            var testDataPath = "TestData/Credits.xml";
            var xml = XmlTestUtils.ReadTestDataOrSkip(testDataPath);
            if (xml == null) return;

            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsXmlModel>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model, xml);

            // 比较原始XML和重新序列化的XML
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 检查节点数量
            var elements1 = doc1.Descendants().Count();
            var elements2 = doc2.Descendants().Count();
            
            File.WriteAllText("/tmp/original.xml", xml);
            File.WriteAllText("/tmp/serialized.xml", xml2);
            
            Assert.Equal(elements1, elements2);
        }
    }
}