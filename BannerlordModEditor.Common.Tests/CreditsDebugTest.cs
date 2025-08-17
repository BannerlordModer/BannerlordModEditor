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

        [Fact]
        public void Debug_Credits_Element_Details()
        {
            var xmlPath = "TestData/Credits.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CreditsDO>(xml);

            // 分析对象结构
            Console.WriteLine("=== 对象结构分析 ===");
            for (int i = 0; i < obj.Categories.Count; i++)
            {
                var category = obj.Categories[i];
                Console.WriteLine($"Category[{i}]: {category.Text}");
                Console.WriteLine($"  Elements count: {category.Elements.Count}");
                
                for (int j = 0; j < category.Elements.Count; j++)
                {
                    var element = category.Elements[j];
                    Console.WriteLine($"    [{j}]: {element.GetType().Name}");
                    
                    if (element is CreditsEmptyLineDO emptyLine)
                    {
                        Console.WriteLine($"      EmptyLine - 没有属性");
                    }
                    else if (element is CreditsEntryDO entry)
                    {
                        Console.WriteLine($"      Entry - Text: '{entry.Text}'");
                    }
                    else if (element is CreditsSectionDO section)
                    {
                        Console.WriteLine($"      Section - Text: '{section.Text}', Entries: {section.Entries.Count}, EmptyLines: {section.EmptyLines.Count}");
                        
                        for (int k = 0; k < section.Entries.Count; k++)
                        {
                            var sectEntry = section.Entries[k];
                            Console.WriteLine($"        Entry[{k}]: '{sectEntry.Text}'");
                        }
                    }
                }
            }

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 分析序列化后的XML
            Console.WriteLine("\n=== 序列化后XML分析 ===");
            var doc2 = XDocument.Parse(xml2);
            var categories2 = doc2.Root.Elements("Category").ToList();
            
            for (int i = 0; i < categories2.Count; i++)
            {
                var category = categories2[i];
                var text = category.Attribute("Text")?.Value;
                Console.WriteLine($"Category[{i}]: {text}");
                
                var elements = category.Elements().ToList();
                for (int j = 0; j < elements.Count; j++)
                {
                    var element = elements[j];
                    Console.WriteLine($"    [{j}]: {element.Name.LocalName}");
                    
                    if (element.Name.LocalName == "EmptyLine")
                    {
                        var textAttr = element.Attribute("Text");
                        Console.WriteLine($"      EmptyLine - Text属性: {(textAttr != null ? $"有='{textAttr.Value}'" : "无")}");
                    }
                    else if (element.Name.LocalName == "Entry")
                    {
                        var textAttr = element.Attribute("Text");
                        Console.WriteLine($"      Entry - Text属性: {(textAttr != null ? $"有='{textAttr.Value}'" : "无")}");
                    }
                    else if (element.Name.LocalName == "Section")
                    {
                        var textAttr = element.Attribute("Text");
                        Console.WriteLine($"      Section - Text: '{textAttr?.Value}'");
                        
                        var entries = element.Elements("Entry").ToList();
                        var emptyLines = element.Elements("EmptyLine").ToList();
                        Console.WriteLine($"        Entries: {entries.Count}, EmptyLines: {emptyLines.Count}");
                    }
                }
            }
        }

        [Fact]
        public void Debug_RemoveNamespaceDeclarations()
        {
            // 测试RemoveNamespaceDeclarations方法是否能正确移除命名空间
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Credits xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <Category Text=""Test"">
        <Entry Text=""Test Entry"" />
        <EmptyLine />
    </Category>
</Credits>";

            Console.WriteLine("=== 原始XML ===");
            Console.WriteLine(testXml);

            var doc = XDocument.Parse(testXml);
            Console.WriteLine($"\n原始命名空间数量: {doc.Root.Attributes().Count(a => a.IsNamespaceDeclaration)}");

            var cleanedDoc = XmlTestUtils.RemoveNamespaceDeclarationsForTesting(doc);
            Console.WriteLine($"\n清理后命名空间数量: {cleanedDoc.Root.Attributes().Count(a => a.IsNamespaceDeclaration)}");

            Console.WriteLine("\n=== 清理后XML ===");
            Console.WriteLine(cleanedDoc.ToString());

            // 验证没有命名空间属性
            var namespaceAttrs = cleanedDoc.Root.Attributes().Where(a => a.IsNamespaceDeclaration).ToList();
            Assert.True(namespaceAttrs.Count == 0, $"仍有命名空间属性: {string.Join(", ", namespaceAttrs.Select(a => a.Name))}");
        }

        [Fact]
        public void Debug_Credits_XmlDifference()
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
            
            // 详细分析差异
            if (nodeCount1 != nodeCount2)
            {
                Console.WriteLine("\n=== 节点差异分析 ===");
                var allElements1 = doc1.Descendants().ToList();
                var allElements2 = doc2.Descendants().ToList();
                
                for (int i = 0; i < Math.Max(allElements1.Count, allElements2.Count); i++)
                {
                    if (i >= allElements1.Count)
                    {
                        Console.WriteLine($"[{i}]: 缺失 - {allElements2[i].Name}");
                    }
                    else if (i >= allElements2.Count)
                    {
                        Console.WriteLine($"[{i}]: 多余 - {allElements1[i].Name}");
                    }
                    else if (allElements1[i].Name != allElements2[i].Name)
                    {
                        Console.WriteLine($"[{i}]: 不同 - 原始={allElements1[i].Name}, 序列化={allElements2[i].Name}");
                    }
                }
            }
            
            if (attrCount1 != attrCount2)
            {
                Console.WriteLine("\n=== 属性差异分析 ===");
                var allElements1 = doc1.Descendants().ToList();
                var allElements2 = doc2.Descendants().ToList();
                
                for (int i = 0; i < Math.Min(allElements1.Count, allElements2.Count); i++)
                {
                    var elem1 = allElements1[i];
                    var elem2 = allElements2[i];
                    
                    var attrs1 = elem1.Attributes().ToList();
                    var attrs2 = elem2.Attributes().ToList();
                    
                    if (attrs1.Count != attrs2.Count)
                    {
                        Console.WriteLine($"元素 {elem1.Name} 属性数量不同: {attrs1.Count} vs {attrs2.Count}");
                        Console.WriteLine($"  原始属性: {string.Join(", ", attrs1.Select(a => a.Name))}");
                        Console.WriteLine($"  序列化属性: {string.Join(", ", attrs2.Select(a => a.Name))}");
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