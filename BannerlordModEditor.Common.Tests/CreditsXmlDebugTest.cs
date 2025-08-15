using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsXmlDebugTest
    {
        [Fact]
        public void DebugCreditsXmlStructure()
        {
            var testDataPath = "TestData/Credits.xml";
            var xml = XmlTestUtils.ReadTestDataOrSkip(testDataPath);
            if (xml == null) return;

            // 解析原始XML
            var doc = XDocument.Parse(xml);
            
            // 获取前几个Category元素
            var categories = doc.Root.Elements("Category").Take(3).ToList();
            
            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                var textAttr = category.Attribute("Text");
                Console.WriteLine($"Category[{i}]: {textAttr?.Value}");
                
                var sections = category.Elements("Section").ToList();
                for (int j = 0; j < Math.Min(sections.Count, 5); j++)
                {
                    var section = sections[j];
                    var sectionText = section.Attribute("Text");
                    Console.WriteLine($"  Section[{j}]: {sectionText?.Value}");
                }
            }
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsDO>(xml);
            
            // 检查反序列化后的结构
            Console.WriteLine($"\nDeserialized model has {model.Categories.Count} categories");
            
            for (int i = 0; i < Math.Min(model.Categories.Count, 3); i++)
            {
                var category = model.Categories[i];
                Console.WriteLine($"Model Category[{i}]: {category.Text}");
                
                for (int j = 0; j < Math.Min(category.Sections.Count, 5); j++)
                {
                    var section = category.Sections[j];
                    Console.WriteLine($"  Model Section[{j}]: {section.Text}");
                }
            }
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model, xml);
            
            // 解析序列化后的XML
            var doc2 = XDocument.Parse(xml2);
            var categories2 = doc2.Root.Elements("Category").Take(3).ToList();
            
            Console.WriteLine($"\nRe-serialized XML has {categories2.Count} categories");
            
            for (int i = 0; i < categories2.Count; i++)
            {
                var category = categories2[i];
                var textAttr = category.Attribute("Text");
                Console.WriteLine($"Re-serialized Category[{i}]: {textAttr?.Value}");
                
                var sections = category.Elements("Section").ToList();
                for (int j = 0; j < Math.Min(sections.Count, 5); j++)
                {
                    var section = sections[j];
                    var sectionText = section.Attribute("Text");
                    Console.WriteLine($"  Re-serialized Section[{j}]: {sectionText?.Value}");
                }
            }
        }
    }
}