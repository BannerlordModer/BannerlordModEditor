using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class LooknfeelDebugTest
    {
        [Fact]
        public void Debug_Looknfeel_Name_Attribute_Issues()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 解析两个XML文档
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 查找所有name属性不同的地方
            var nameDifferences = new List<string>();

            var elements1 = doc1.Descendants().ToList();
            var elements2 = doc2.Descendants().ToList();

            for (int i = 0; i < Math.Min(elements1.Count, elements2.Count); i++)
            {
                var elem1 = elements1[i];
                var elem2 = elements2[i];

                var nameAttr1 = elem1.Attribute("name");
                var nameAttr2 = elem2.Attribute("name");

                if (nameAttr1 != null && nameAttr2 != null && nameAttr1.Value != nameAttr2.Value)
                {
                    var path = GetElementPath(elem1);
                    nameDifferences.Add($"{path}: A={nameAttr1.Value}, B={nameAttr2.Value}");
                }
            }

            Assert.True(nameDifferences.Count == 0, $"发现{nameDifferences.Count}个name属性差异: {string.Join(", ", nameDifferences)}");
        }

        private string GetElementPath(XElement element)
        {
            var path = new System.Collections.Generic.List<string>();
            var current = element;

            while (current != null)
            {
                var index = current.ElementsBeforeSelf().Count();
                path.Insert(0, $"{current.Name.LocalName}[{index}]");
                current = current.Parent;
            }

            return string.Join("/", path);
        }
    }
}