using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class DetailedSubWidgetAnalysisTest
    {
        [Fact]
        public void Analyze_SubWidget_Attribute_Mismatch()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析差异
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            // 找到所有sub_widget元素
            var originalSubWidgets = doc1.Descendants().Where(e => e.Name.LocalName == "sub_widget").ToList();
            var serializedSubWidgets = doc2.Descendants().Where(e => e.Name.LocalName == "sub_widget").ToList();
            
            Console.WriteLine($"Original sub_widgets: {originalSubWidgets.Count}");
            Console.WriteLine($"Serialized sub_widgets: {serializedSubWidgets.Count}");
            
            // 分析前10个sub_widget的详细差异
            for (int i = 0; i < Math.Min(10, originalSubWidgets.Count); i++)
            {
                var orig = originalSubWidgets[i];
                var ser = serializedSubWidgets.Count > i ? serializedSubWidgets[i] : null;
                
                Console.WriteLine($"\n=== SubWidget {i} ===");
                Console.WriteLine($"Original attributes: {orig.Attributes().Count()}");
                
                if (ser != null)
                {
                    Console.WriteLine($"Serialized attributes: {ser.Attributes().Count()}");
                    
                    var origAttrs = orig.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    var serAttrs = ser.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    
                    var missing = origAttrs.Keys.Except(serAttrs.Keys).ToList();
                    var extra = serAttrs.Keys.Except(origAttrs.Keys).ToList();
                    
                    if (missing.Any())
                    {
                        Console.WriteLine($"Missing attributes: {string.Join(", ", missing)}");
                    }
                    
                    if (extra.Any())
                    {
                        Console.WriteLine($"Extra attributes: {string.Join(", ", extra)}");
                    }
                    
                    // 检查具体值
                    foreach (var attr in origAttrs.Keys)
                    {
                        if (serAttrs.ContainsKey(attr))
                        {
                            if (origAttrs[attr] != serAttrs[attr])
                            {
                                Console.WriteLine($"Value mismatch for {attr}: '{origAttrs[attr]}' vs '{serAttrs[attr]}'");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No corresponding serialized sub_widget found!");
                }
            }
        }

        [Fact]
        public void Analyze_Element_Order_Issues()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析差异
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            // 分析第100个元素附近的序列化问题
            var allOriginalElements = doc1.Descendants().ToList();
            var allSerializedElements = doc2.Descendants().ToList();
            
            Console.WriteLine($"Analyzing element 100 (from previous test output)...");
            
            if (allOriginalElements.Count > 100)
            {
                var elem100 = allOriginalElements[100];
                Console.WriteLine($"Element 100: {elem100.Name.LocalName}");
                Console.WriteLine($"Parent: {elem100.Parent?.Name.LocalName}");
                Console.WriteLine($"Attributes: {string.Join(", ", elem100.Attributes().Select(a => $"{a.Name.LocalName}={a.Value}"))}");
                
                // 查找对应的序列化元素
                var candidates = allSerializedElements.Where(e => e.Name.LocalName == elem100.Name.LocalName).ToList();
                Console.WriteLine($"Found {candidates.Count} serialized elements with same name");
                
                for (int i = 0; i < Math.Min(3, candidates.Count); i++)
                {
                    var candidate = candidates[i];
                    Console.WriteLine($"Candidate {i}: {string.Join(", ", candidate.Attributes().Select(a => $"{a.Name.LocalName}={a.Value}"))}");
                }
            }
        }
    }
}