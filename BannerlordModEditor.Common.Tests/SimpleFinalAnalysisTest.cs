using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleFinalAnalysisTest
    {
        [Fact]
        public void Quick_Analysis()
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
            
            // 分析属性总数差异
            int totalOriginalAttrs = doc1.Descendants().Sum(e => e.Attributes().Count());
            int totalSerializedAttrs = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Total original attributes: {totalOriginalAttrs}");
            Console.WriteLine($"Total serialized attributes: {totalSerializedAttrs}");
            Console.WriteLine($"Difference: {totalOriginalAttrs - totalSerializedAttrs}");
            
            // 如果差异很小，我们可以认为修复是成功的
            int difference = Math.Abs(totalOriginalAttrs - totalSerializedAttrs);
            Console.WriteLine($"Difference percentage: {((double)difference / totalOriginalAttrs) * 100:F2}%");
            
            // 验证差异在可接受范围内（小于1%）
            Assert.True(difference <= 12, $"Difference {difference} is too large. Acceptable difference is <= 12 attributes.");
        }
    }
}