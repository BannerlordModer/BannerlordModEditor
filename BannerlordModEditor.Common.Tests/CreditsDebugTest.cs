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

            // 比较Category的顺序，重点关注Category[1]
            var categories1 = doc1.Root.Elements("Category").ToList();
            var categories2 = doc2.Root.Elements("Category").ToList();

            if (categories1.Count > 1 && categories2.Count > 1)
            {
                var cat1 = categories1[1]; // Lead Designers
                var cat2 = categories2[1];

                var catText1 = cat1.Attribute("Text")?.Value;
                var catText2 = cat2.Attribute("Text")?.Value;

                Console.WriteLine($"Category[1]: {catText1}");

                // 比较Category内部的元素顺序
                var children1 = cat1.Elements().ToList();
                var children2 = cat2.Elements().ToList();

                Console.WriteLine($"  原始XML子元素数量: {children1.Count}");
                Console.WriteLine($"  序列化XML子元素数量: {children2.Count}");

                for (int j = 0; j < Math.Min(children1.Count, children2.Count); j++)
                {
                    var child1 = children1[j];
                    var child2 = children2[j];

                    Console.WriteLine($"    [{j}]: 原始={child1.Name}, 序列化={child2.Name}");

                    if (child1.Name != child2.Name)
                    {
                        Assert.True(false, $"Category[1] 的第{j}个元素类型不匹配: A={child1.Name}, B={child2.Name}");
                    }
                }
            }
        }

        private void AnalyzeXmlStructure(string xml, string tag)
        {
            var doc = XDocument.Parse(xml);
            var categories = doc.Root.Elements("Category").ToList();
            
            Console.WriteLine($"{tag} XML包含 {categories.Count} 个Category:");
            
            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                var text = category.Attribute("Text")?.Value;
                var children = category.Elements().ToList();
                
                Console.WriteLine($"  Category[{i}]: {text} ({children.Count} 个子元素)");
                
                for (int j = 0; j < children.Count; j++)
                {
                    var child = children[j];
                    Console.WriteLine($"    [{j}]: {child.Name.LocalName}");
                    
                    if (child.Name.LocalName == "Section" || child.Name.LocalName == "Entry")
                    {
                        var childText = child.Attribute("Text")?.Value;
                        Console.WriteLine($"        Text: {childText}");
                    }
                }
            }
        }

        private void AnalyzeObjectStructure(CreditsDO obj)
        {
            Console.WriteLine($"CreditsDO包含 {obj.Categories.Count} 个Category:");
            
            for (int i = 0; i < obj.Categories.Count; i++)
            {
                var category = obj.Categories[i];
                Console.WriteLine($"  Category[{i}]: {category.Text}");
                Console.WriteLine($"    Sections: {category.Sections.Count}");
                Console.WriteLine($"    Entries: {category.Entries.Count}");
                Console.WriteLine($"    EmptyLines: {category.EmptyLines.Count}");
                Console.WriteLine($"    LoadFromFile: {category.LoadFromFile.Count}");
                Console.WriteLine($"    Images: {category.Images.Count}");
                
                for (int j = 0; j < category.Sections.Count; j++)
                {
                    var section = category.Sections[j];
                    Console.WriteLine($"    Section[{j}]: {section.Text} (Entries: {section.Entries.Count}, EmptyLines: {section.EmptyLines.Count})");
                }
                
                for (int j = 0; j < category.Entries.Count; j++)
                {
                    var entry = category.Entries[j];
                    Console.WriteLine($"    Entry[{j}]: {entry.Text} (EmptyLines: {entry.EmptyLines.Count})");
                }
            }
        }
    }
}