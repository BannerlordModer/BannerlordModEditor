using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsDebugTest
    {
        [Fact]
        public void Debug_Credits_Element_Order()
        {
            var xmlPath = "TestData/Credits.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CreditsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 解析两个XML文档
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 比较Category的顺序
            var categories1 = doc1.Root.Elements("Category").ToList();
            var categories2 = doc2.Root.Elements("Category").ToList();

            for (int i = 0; i < Math.Min(categories1.Count, categories2.Count); i++)
            {
                var cat1 = categories1[i];
                var cat2 = categories2[i];

                var catText1 = cat1.Attribute("Text")?.Value;
                var catText2 = cat2.Attribute("Text")?.Value;

                if (catText1 != catText2)
                {
                    Assert.True(false, $"Category[{i}] Text不匹配: A={catText1}, B={catText2}");
                }

                // 比较Category内部的元素顺序
                var children1 = cat1.Elements().ToList();
                var children2 = cat2.Elements().ToList();

                for (int j = 0; j < Math.Min(children1.Count, children2.Count); j++)
                {
                    var child1 = children1[j];
                    var child2 = children2[j];

                    if (child1.Name != child2.Name)
                    {
                        Assert.True(false, $"Category[{i}] 的第{j}个元素类型不匹配: A={child1.Name}, B={child2.Name}");
                    }

                    if (child1.Name.LocalName == "Section")
                    {
                        var text1 = child1.Attribute("Text")?.Value;
                        var text2 = child2.Attribute("Text")?.Value;
                        if (text1 != text2)
                        {
                            Assert.True(false, $"Category[{i}] Section[{j}] Text不匹配: A={text1}, B={text2}");
                        }
                    }
                    else if (child1.Name.LocalName == "Entry")
                    {
                        var text1 = child1.Attribute("Text")?.Value;
                        var text2 = child2.Attribute("Text")?.Value;
                        if (text1 != text2)
                        {
                            Assert.True(false, $"Category[{i}] Entry[{j}] Text不匹配: A={text1}, B={text2}");
                        }
                    }
                }
            }
        }
    }
}