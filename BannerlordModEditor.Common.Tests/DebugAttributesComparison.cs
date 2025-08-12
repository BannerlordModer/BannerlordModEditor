using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugAttributesComparison
    {
        private const string TestDataPath = "TestData/attributes.xml";

        [Fact(Skip = "Debug only")]
        public void Debug_Attributes_Format_Comparison()
        {
            // 读取原始 XML
            var xml = File.ReadAllText(TestDataPath);
            
            Console.WriteLine($"=== 原始XML ===");
            Console.WriteLine(xml);
            
            // 反序列化为模型对象
            var obj = XmlTestUtils.Deserialize<AttributesDataModel>(xml);
            Console.WriteLine($"\n=== 反序列化成功，属性数量: {obj.Attributes.Count} ===");

            // 再序列化为字符串
            var xml2 = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"\n=== 序列化后的XML ===");
            Console.WriteLine(xml2);
            
            // 统计节点和属性
            var (nodeCount1, attrCount1) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (nodeCount2, attrCount2) = XmlTestUtils.CountNodesAndAttributes(xml2);
            
            Console.WriteLine($"\n=== 节点和属性统计 ===");
            Console.WriteLine($"原始XML - 节点: {nodeCount1}, 属性: {attrCount1}");
            Console.WriteLine($"序列化XML - 节点: {nodeCount2}, 属性: {attrCount2}");
            
            // 使用更详细的方式比较XML
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            Console.WriteLine($"\n=== 根元素比较 ===");
            Console.WriteLine($"原始根元素名称: {doc1.Root.Name}");
            Console.WriteLine($"序列化根元素名称: {doc2.Root.Name}");
            
            Console.WriteLine($"\n=== 原始根元素属性 ===");
            foreach (var attr in doc1.Root.Attributes())
            {
                Console.WriteLine($"  {attr.Name}: {attr.Value}");
            }
            
            Console.WriteLine($"\n=== 序列化根元素属性 ===");
            foreach (var attr in doc2.Root.Attributes())
            {
                Console.WriteLine($"  {attr.Name}: {attr.Value}");
            }
        }
    }
}