using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphDetailedAnalysisTest
    {
        [Fact]
        public void BeforeTransparentsGraph_Analyze_ElementOrder()
        {
            var xmlPath = "TestData/before_transparents_graph.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<BeforeTransparentsGraph>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Console.WriteLine("=== 分析原始XML和序列化XML的差异 ===");

            // 解析XML文档
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 获取所有postfx_node节点
            var nodes1 = doc1.Descendants("postfx_node").ToList();
            var nodes2 = doc2.Descendants("postfx_node").ToList();

            Console.WriteLine($"原始XML节点数量: {nodes1.Count}");
            Console.WriteLine($"序列化XML节点数量: {nodes2.Count}");

            for (int i = 0; i < nodes1.Count; i++)
            {
                var node1 = nodes1[i];
                var node2 = nodes2[i];
                
                var nodeId = node1.Attribute("id")?.Value;
                Console.WriteLine($"\n=== 节点 {i+1}: {nodeId} ===");

                // 获取子元素
                var children1 = node1.Elements().ToList();
                var children2 = node2.Elements().ToList();

                Console.WriteLine("原始XML子元素顺序:");
                for (int j = 0; j < children1.Count; j++)
                {
                    Console.WriteLine($"  {j+1}. {children1[j].Name.LocalName}");
                }

                Console.WriteLine("序列化XML子元素顺序:");
                for (int j = 0; j < children2.Count; j++)
                {
                    Console.WriteLine($"  {j+1}. {children2[j].Name.LocalName}");
                }

                // 检查顺序是否一致
                bool orderMatch = true;
                for (int j = 0; j < Math.Min(children1.Count, children2.Count); j++)
                {
                    if (children1[j].Name.LocalName != children2[j].Name.LocalName)
                    {
                        orderMatch = false;
                        Console.WriteLine($"*** 顺序不匹配: 位置 {j+1}, 原始: {children1[j].Name.LocalName}, 序列化: {children2[j].Name.LocalName}");
                    }
                }

                if (orderMatch)
                {
                    Console.WriteLine("✓ 子元素顺序一致");
                }
            }

            // 使用XmlTestUtils的结构化比较
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"\n=== XmlTestUtils比较结果 ===");
            Console.WriteLine($"结构相等: {report.IsStructurallyEqual}");
            Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
            Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
            
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"缺失节点: {string.Join(", ", report.MissingNodes)}");
                Console.WriteLine($"多余节点: {string.Join(", ", report.ExtraNodes)}");
                Console.WriteLine($"节点名差异: {string.Join(", ", report.NodeNameDifferences)}");
                Console.WriteLine($"缺失属性: {string.Join(", ", report.MissingAttributes)}");
                Console.WriteLine($"多余属性: {string.Join(", ", report.ExtraAttributes)}");
                Console.WriteLine($"属性值差异: {string.Join(", ", report.AttributeValueDifferences)}");
                Console.WriteLine($"文本差异: {string.Join(", ", report.TextDifferences)}");
            }
        }
    }
}