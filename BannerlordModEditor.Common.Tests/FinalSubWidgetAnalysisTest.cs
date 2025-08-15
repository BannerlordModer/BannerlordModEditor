using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class FinalSubWidgetAnalysisTest
    {
        [Fact]
        public void Find_Missing_SubWidget()
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
            
            // 按父元素分组分析
            var originalByParent = originalSubWidgets.GroupBy(w => w.Parent?.Name.LocalName).ToDictionary(g => g.Key, g => g.ToList());
            var serializedByParent = serializedSubWidgets.GroupBy(w => w.Parent?.Name.LocalName).ToDictionary(g => g.Key, g => g.ToList());
            
            foreach (var parentName in originalByParent.Keys)
            {
                var origCount = originalByParent[parentName].Count;
                var serCount = serializedByParent.ContainsKey(parentName) ? serializedByParent[parentName].Count : 0;
                
                Console.WriteLine($"\nParent: {parentName}");
                Console.WriteLine($"  Original: {origCount}, Serialized: {serCount}");
                
                if (origCount != serCount)
                {
                    Console.WriteLine($"  *** MISMATCH DETECTED ***");
                    
                    // 分析具体的sub_widget
                    for (int i = 0; i < origCount; i++)
                    {
                        var orig = originalByParent[parentName][i];
                        var ser = serializedByParent.ContainsKey(parentName) && i < serializedByParent[parentName].Count 
                            ? serializedByParent[parentName][i] 
                            : null;
                        
                        var origName = orig.Attribute("name")?.Value ?? "no-name";
                        var serName = ser?.Attribute("name")?.Value ?? "missing";
                        
                        Console.WriteLine($"    [{i}] Original: {origName}, Serialized: {serName}");
                        
                        if (ser == null)
                        {
                            Console.WriteLine($"    *** MISSING SUB_WIDGET: {origName} ***");
                            Console.WriteLine($"    Attributes: {string.Join(", ", orig.Attributes().Select(a => $"{a.Name.LocalName}={a.Value}"))}");
                        }
                    }
                }
            }
            
            // 分析属性总数差异
            int totalOriginalAttrs = originalSubWidgets.Sum(w => w.Attributes().Count());
            int totalSerializedAttrs = serializedSubWidgets.Sum(w => w.Attributes().Count());
            
            Console.WriteLine($"\nTotal original sub_widget attributes: {totalOriginalAttrs}");
            Console.WriteLine($"Total serialized sub_widget attributes: {totalSerializedAttrs}");
            Console.WriteLine($"Difference: {totalOriginalAttrs - totalSerializedAttrs}");
            
            // 分析整体属性差异
            int overallOriginalAttrs = doc1.Descendants().Sum(e => e.Attributes().Count());
            int overallSerializedAttrs = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"\nOverall original attributes: {overallOriginalAttrs}");
            Console.WriteLine($"Overall serialized attributes: {overallSerializedAttrs}");
            Console.WriteLine($"Overall difference: {overallOriginalAttrs - overallSerializedAttrs}");
            
            // 验证我们的修复效果
            Assert.Equal(overallOriginalAttrs, overallSerializedAttrs);
        }
    }
}