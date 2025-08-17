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

        [Fact]
        public void Debug_Credits_XmlDifference()
        {
            var testDataPath = "TestData/Credits.xml";
            var xml = File.ReadAllText(testDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model, xml);

            // 解析两个XML文档
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 手动统计节点数量
            int nodeCount1 = doc1.Descendants().Count();
            int nodeCount2 = doc2.Descendants().Count();
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());

            Console.WriteLine($"=== 手动统计 ===");
            Console.WriteLine($"原始XML - 节点数: {nodeCount1}, 属性数: {attrCount1}");
            Console.WriteLine($"序列化XML - 节点数: {nodeCount2}, 属性数: {attrCount2}");
            Console.WriteLine($"节点差异: {nodeCount1 - nodeCount2}");
            Console.WriteLine($"属性差异: {attrCount1 - attrCount2}");

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            
            Console.WriteLine($"\n=== 差异报告 ===");
            Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
            
            // 直接比较XML字符串
            Console.WriteLine($"\n=== 直接字符串比较 ===");
            Console.WriteLine($"XML长度相同: {xml.Length == xml2.Length}");
            Console.WriteLine($"原始XML长度: {xml.Length}");
            Console.WriteLine($"序列化XML长度: {xml2.Length}");

            // 如果长度不同，找到第一个不同的位置
            if (xml.Length != xml2.Length)
            {
                int minLength = Math.Min(xml.Length, xml2.Length);
                for (int i = 0; i < minLength; i++)
                {
                    if (xml[i] != xml2[i])
                    {
                        Console.WriteLine($"第一个不同位置: {i}");
                        Console.WriteLine($"原始XML上下文: {xml.Substring(Math.Max(0, i-20), 40)}");
                        Console.WriteLine($"序列化XML上下文: {xml2.Substring(Math.Max(0, i-20), 40)}");
                        break;
                    }
                }
            }

            // 检查是否有空格或换行符差异
            var normalizedXml1 = xml.Replace("\r\n", "\n").Replace("  ", " ").Trim();
            var normalizedXml2 = xml2.Replace("\r\n", "\n").Replace("  ", " ").Trim();
            Console.WriteLine($"\n=== 标准化后比较 ===");
            Console.WriteLine($"标准化后相同: {normalizedXml1 == normalizedXml2}");
            Console.WriteLine($"标准化后长度相同: {normalizedXml1.Length == normalizedXml2.Length}");
        }
    }
}