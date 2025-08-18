using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDetailedDiffTest
    {
        [Fact]
        public void Find_Exact_Difference_Point()
        {
            Console.WriteLine("=== 查找精确差异点 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            var originalXml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(obj);
            
            // 比较两个XML
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            // 获取第一个layout的items
            var originalItems = originalDoc.Descendants("layout").First().Element("items").Elements("item").ToList();
            var serializedItems = serializedDoc.Descendants("layout").First().Element("items").Elements("item").ToList();
            
            Console.WriteLine($"Original items count: {originalItems.Count}");
            Console.WriteLine($"Serialized items count: {serializedItems.Count}");
            
            // 逐个比较items
            for (int i = 0; i < Math.Min(originalItems.Count, serializedItems.Count); i++)
            {
                var originalItem = originalItems[i];
                var serializedItem = serializedItems[i];
                
                var originalName = originalItem.Attribute("name")?.Value;
                var serializedName = serializedItem.Attribute("name")?.Value;
                
                if (originalName != serializedName)
                {
                    Console.WriteLine($"*** DIFFERENCE FOUND at index {i} ***");
                    Console.WriteLine($"Original: {originalName}");
                    Console.WriteLine($"Serialized: {serializedName}");
                    return;
                }
                
                // 比较属性
                var originalAttrs = originalItem.Attributes().OrderBy(a => a.Name.LocalName).ToList();
                var serializedAttrs = serializedItem.Attributes().OrderBy(a => a.Name.LocalName).ToList();
                
                if (originalAttrs.Count != serializedAttrs.Count)
                {
                    Console.WriteLine($"*** ATTRIBUTE COUNT DIFFERENCE at index {i} ({originalName}) ***");
                    Console.WriteLine($"Original attributes: {originalAttrs.Count}");
                    Console.WriteLine($"Serialized attributes: {serializedAttrs.Count}");
                    return;
                }
                
                for (int j = 0; j < originalAttrs.Count; j++)
                {
                    if (originalAttrs[j].Name.LocalName != serializedAttrs[j].Name.LocalName ||
                        originalAttrs[j].Value != serializedAttrs[j].Value)
                    {
                        Console.WriteLine($"*** ATTRIBUTE DIFFERENCE at index {i} ({originalName}) ***");
                        Console.WriteLine($"Original: {originalAttrs[j].Name}=\"{originalAttrs[j].Value}\"");
                        Console.WriteLine($"Serialized: {serializedAttrs[j].Name}=\"{serializedAttrs[j].Value}\"");
                        return;
                    }
                }
                
                // 比较子元素
                var originalChildren = originalItem.Elements().ToList();
                var serializedChildren = serializedItem.Elements().ToList();
                
                if (originalChildren.Count != serializedChildren.Count)
                {
                    Console.WriteLine($"*** CHILD COUNT DIFFERENCE at index {i} ({originalName}) ***");
                    Console.WriteLine($"Original children: {originalChildren.Count}");
                    Console.WriteLine($"Serialized children: {serializedChildren.Count}");
                    return;
                }
                
                for (int j = 0; j < originalChildren.Count; j++)
                {
                    var originalChild = originalChildren[j];
                    var serializedChild = serializedChildren[j];
                    
                    if (originalChild.Name.LocalName != serializedChild.Name.LocalName)
                    {
                        Console.WriteLine($"*** CHILD NAME DIFFERENCE at index {i} ({originalName}) ***");
                        Console.WriteLine($"Original: {originalChild.Name}");
                        Console.WriteLine($"Serialized: {serializedChild.Name}");
                        return;
                    }
                    
                    // 比较子元素的属性
                    var originalChildAttrs = originalChild.Attributes().OrderBy(a => a.Name.LocalName).ToList();
                    var serializedChildAttrs = serializedChild.Attributes().OrderBy(a => a.Name.LocalName).ToList();
                    
                    if (originalChildAttrs.Count != serializedChildAttrs.Count)
                    {
                        Console.WriteLine($"*** CHILD ATTRIBUTE COUNT DIFFERENCE at index {i} ({originalName}.{originalChild.Name}) ***");
                        Console.WriteLine($"Original: {originalChildAttrs.Count}");
                        Console.WriteLine($"Serialized: {serializedChildAttrs.Count}");
                        return;
                    }
                    
                    for (int k = 0; k < originalChildAttrs.Count; k++)
                    {
                        if (originalChildAttrs[k].Name.LocalName != serializedChildAttrs[k].Name.LocalName ||
                            originalChildAttrs[k].Value != serializedChildAttrs[k].Value)
                        {
                            Console.WriteLine($"*** CHILD ATTRIBUTE DIFFERENCE at index {i} ({originalName}.{originalChild.Name}) ***");
                            Console.WriteLine($"Original: {originalChildAttrs[k].Name}=\"{originalChildAttrs[k].Value}\"");
                            Console.WriteLine($"Serialized: {serializedChildAttrs[k].Name}=\"{serializedChildAttrs[k].Value}\"");
                            return;
                        }
                    }
                }
            }
            
            // 如果到这里还没有返回，说明没有差异
            Console.WriteLine("No differences found in item structure!");
            
            // 检查整体结构
            if (originalItems.Count != serializedItems.Count)
            {
                Console.WriteLine($"*** TOTAL ITEM COUNT DIFFERENCE ***");
                Console.WriteLine($"Original: {originalItems.Count}");
                Console.WriteLine($"Serialized: {serializedItems.Count}");
                return;
            }
            
            // 使用XmlTestUtils进行比较
            bool areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            Console.WriteLine($"XmlTestUtils.AreStructurallyEqual result: {areEqual}");
            
            if (!areEqual)
            {
                var report = XmlTestUtils.CompareXmlStructure(originalXml, serializedXml);
                Console.WriteLine($"Node count difference: {report.NodeCountDifference}");
                Console.WriteLine($"Attribute count difference: {report.AttributeCountDifference}");
                Console.WriteLine($"Missing nodes: {report.MissingNodes.Count}");
                Console.WriteLine($"Extra nodes: {report.ExtraNodes.Count}");
                Console.WriteLine($"Missing attributes: {report.MissingAttributes.Count}");
                Console.WriteLine($"Extra attributes: {report.ExtraAttributes.Count}");
                Console.WriteLine($"Attribute value differences: {report.AttributeValueDifferences.Count}");
                
                if (report.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine("Attribute value differences:");
                    foreach (var diff in report.AttributeValueDifferences)
                    {
                        Console.WriteLine($"  {diff}");
                    }
                }
            }
            
            Assert.True(areEqual, "XML should be structurally equal");
        }
    }
}