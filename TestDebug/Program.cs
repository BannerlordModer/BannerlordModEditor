using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Tests;
using BannerlordModEditor.Common.Models.DO;

namespace TestDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlPath = "../BannerlordModEditor.Common.Tests/TestData/item_modifiers.xml";
            var xml = File.ReadAllText(xmlPath);
            var originalDoc = XDocument.Parse(xml);
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<ItemModifiersDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 测试RemoveNamespaceDeclarations方法
            var testDoc = XDocument.Parse(xml2);
            var cleanedDoc = XmlTestUtils.RemoveNamespaceDeclarationsForTesting(testDoc);
            Console.WriteLine("\n=== 清理后的XML根元素 ===");
            Console.WriteLine(cleanedDoc.Root?.ToString());
            
            // 检查注释内容
            Console.WriteLine("\n=== 注释内容检查 ===");
            var commentsForCheck = originalDoc.DescendantNodes().OfType<XComment>().ToList();
            for (int i = 0; i < Math.Min(3, commentsForCheck.Count); i++)
            {
                Console.WriteLine($"注释 {i+1}: '{commentsForCheck[i].Value}'");
            }
            
            // 分析差异
            var (origNodeCount, origAttrCount) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (serNodeCount, serAttrCount) = XmlTestUtils.CountNodesAndAttributes(xml2);
            
            Console.WriteLine($"=== 调试信息 ===");
            Console.WriteLine($"原始节点数: {origNodeCount}, 序列化节点数: {serNodeCount}");
            Console.WriteLine($"原始属性数: {origAttrCount}, 序列化属性数: {serAttrCount}");
            Console.WriteLine($"属性差异: {serAttrCount - origAttrCount}");
            
            // 详细分析属性差异
            var serializedDoc = XDocument.Parse(xml2);
            
            var originalAttrs = originalDoc.Descendants().SelectMany(d => d.Attributes()).Count();
            var serializedAttrs = serializedDoc.Descendants().SelectMany(d => d.Attributes()).Count();
            
            Console.WriteLine($"详细属性计数 - 原始: {originalAttrs}, 序列化: {serializedAttrs}");
            
            // 检查注释
            var originalComments = originalDoc.DescendantNodes().OfType<XComment>().Count();
            var serializedComments = serializedDoc.DescendantNodes().OfType<XComment>().Count();
            
            Console.WriteLine($"注释数量 - 原始: {originalComments}, 序列化: {serializedComments}");
            
            // 输出原始和序列化后的XML根元素，查看命名空间问题
            Console.WriteLine("\n=== 原始XML根元素 ===");
            Console.WriteLine(originalDoc.Root?.ToString());
            Console.WriteLine("\n=== 序列化后的XML根元素 ===");
            Console.WriteLine(serializedDoc.Root?.ToString());
            
            // 检查最终结果
            Console.WriteLine("\n=== 最终XML结果 ===");
            var finalDoc = XDocument.Parse(xml2);
            Console.WriteLine(finalDoc.Root?.ToString());
            
            // 分析多出的属性
            Console.WriteLine("\n=== 属性分析 ===");
            var originalElements = originalDoc.Descendants().ToList();
            var serializedElements = serializedDoc.Descendants().ToList();
            
            for (int i = 0; i < Math.Min(originalElements.Count, serializedElements.Count); i++)
            {
                var origElem = originalElements[i];
                var serElem = serializedElements[i];
                
                if (origElem.Name.LocalName != serElem.Name.LocalName)
                {
                    Console.WriteLine($"元素名称不匹配: {origElem.Name.LocalName} vs {serElem.Name.LocalName}");
                    continue;
                }
                
                var origAttrs = origElem.Attributes().ToList();
                var serAttrs = serElem.Attributes().ToList();
                
                if (origAttrs.Count != serAttrs.Count)
                {
                    Console.WriteLine($"元素 '{origElem.Name.LocalName}' 属性数量不匹配: {origAttrs.Count} vs {serAttrs.Count}");
                    
                    var origAttrNames = origAttrs.Select(a => a.Name.LocalName).ToHashSet();
                    var serAttrNames = serAttrs.Select(a => a.Name.LocalName).ToHashSet();
                    
                    var extraAttrs = serAttrNames.Except(origAttrNames).ToList();
                    var missingAttrs = origAttrNames.Except(serAttrNames).ToList();
                    
                    if (extraAttrs.Count > 0)
                    {
                        Console.WriteLine($"  多出的属性: {string.Join(", ", extraAttrs)}");
                    }
                    if (missingAttrs.Count > 0)
                    {
                        Console.WriteLine($"  缺失的属性: {string.Join(", ", missingAttrs)}");
                    }
                }
            }
        }
    }
}