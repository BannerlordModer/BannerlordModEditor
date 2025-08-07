using System;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugXmlFormatComparison
    {
        private const string TestDataPath = "TestData/MultiplayerScenes.xml";

        [Fact]
        public void Debug_Xml_Format_Comparison()
        {
            // 读取原始 XML
            var xml = File.ReadAllText(TestDataPath);
            
            // 反序列化为模型对象
            var obj = XmlTestUtils.Deserialize<MultiplayerScenes>(xml);

            // 再序列化为字符串
            var xml2 = XmlTestUtils.Serialize(obj);

            // 使用更详细的方式比较XML
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            Console.WriteLine("=== 比较根元素 ===");
            Console.WriteLine($"Root1: {doc1.Root.Name}");
            Console.WriteLine($"Root2: {doc2.Root.Name}");
            
            // 比较所有节点
            var scenes1 = doc1.Root.Elements("Scene");
            var scenes2 = doc2.Root.Elements("Scene");
            
            Console.WriteLine($"\n=== 场景数量对比 ===");
            Console.WriteLine($"Original scenes: {scenes1.Count()}");
            Console.WriteLine($"Serialized scenes: {scenes2.Count()}");
            
            // 逐个比较场景
            var i = 0;
            foreach (var scene1 in scenes1)
            {
                if (i >= scenes2.Count())
                {
                    Console.WriteLine($"Scene {i}: Missing in serialized XML!");
                    break;
                }
                
                var scene2 = scenes2.ElementAt(i);
                var name1 = scene1.Attribute("name")?.Value;
                var name2 = scene2.Attribute("name")?.Value;
                
                Console.WriteLine($"\n--- Scene {i} ---");
                Console.WriteLine($"Original name: '{name1}'");
                Console.WriteLine($"Serialized name: '{name2}'");
                
                if (name1 != name2)
                {
                    Console.WriteLine($"NAME MISMATCH: '{name1}' vs '{name2}'");
                }
                
                // 比较GameType
                var gameTypes1 = scene1.Elements("GameType");
                var gameTypes2 = scene2.Elements("GameType");
                
                Console.WriteLine($"Original GameTypes: {gameTypes1.Count()}");
                Console.WriteLine($"Serialized GameTypes: {gameTypes2.Count()}");
                
                var j = 0;
                foreach (var gt1 in gameTypes1)
                {
                    if (j >= gameTypes2.Count())
                    {
                        Console.WriteLine($"  GameType {j}: Missing in serialized XML!");
                        break;
                    }
                    
                    var gt2 = gameTypes2.ElementAt(j);
                    var gtName1 = gt1.Attribute("name")?.Value;
                    var gtName2 = gt2.Attribute("name")?.Value;
                    
                    Console.WriteLine($"  GameType {j}: '{gtName1}' vs '{gtName2}'");
                    
                    if (gtName1 != gtName2)
                    {
                        Console.WriteLine($"  GAMETYPE MISMATCH: '{gtName1}' vs '{gtName2}'");
                    }
                    
                    j++;
                }
                
                i++;
            }
            
            // 比较XML字符串的具体差异
            Console.WriteLine("\n=== XML字符串差异分析 ===");
            var lines1 = xml.Split('\n');
            var lines2 = xml2.Split('\n');
            
            Console.WriteLine($"Original lines: {lines1.Length}");
            Console.WriteLine($"Serialized lines: {lines2.Length}");
            
            var maxLines = Math.Max(lines1.Length, lines2.Length);
            for (int line = 0; line < maxLines; line++)
            {
                var l1 = line < lines1.Length ? lines1[line] : "[MISSING]";
                var l2 = line < lines2.Length ? lines2[line] : "[MISSING]";
                
                if (l1.Trim() != l2.Trim())
                {
                    Console.WriteLine($"\nLine {line + 1} differs:");
                    Console.WriteLine($"  Original:  '{l1}'");
                    Console.WriteLine($"  Serialized: '{l2}'");
                    
                    // 只显示前几个差异，避免输出过多
                    if (line > 10)
                    {
                        Console.WriteLine("...");
                        break;
                    }
                }
            }
        }
    }
}