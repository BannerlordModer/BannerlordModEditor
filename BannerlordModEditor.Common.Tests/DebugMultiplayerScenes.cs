using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugMultiplayerScenes
    {
        private const string TestDataPath = "TestData/MultiplayerScenes.xml";

        [Fact]
        public void Debug_MultiplayerScenes_Serialization()
        {
            // 读取原始 XML
            var xml = File.ReadAllText(TestDataPath);
            Console.WriteLine("Original XML:");
            Console.WriteLine(xml);
            Console.WriteLine();

            // 反序列化为模型对象
            var obj = XmlTestUtils.Deserialize<MultiplayerScenes>(xml);
            Console.WriteLine($"Deserialized object - Scene count: {obj.Scenes.Count}");
            Console.WriteLine();

            // 再序列化为字符串
            var xml2 = XmlTestUtils.Serialize(obj);
            Console.WriteLine("Serialized XML:");
            Console.WriteLine(xml2);
            Console.WriteLine();

            // 比较原始和序列化后的XML
            Console.WriteLine("Comparing XMLs...");
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            var areEqual = XNode.DeepEquals(doc1, doc2);
            Console.WriteLine($"Are XMLs equal: {areEqual}");
            
            if (!areEqual)
            {
                Console.WriteLine("XMLs are different!");
                // 查找不同之处
                var scenes1 = doc1.Root.Elements("Scene");
                var scenes2 = doc2.Root.Elements("Scene");
                
                Console.WriteLine($"Original scenes count: {scenes1.Count()}");
                Console.WriteLine($"Serialized scenes count: {scenes2.Count()}");
                
                // 检查每个场景
                var i = 0;
                foreach (var scene in scenes1)
                {
                    Console.WriteLine($"Scene {i}: name='{scene.Attribute("name")?.Value}' gameTypes={scene.Elements("GameType").Count()}");
                    i++;
                }
                
                i = 0;
                foreach (var scene in scenes2)
                {
                    Console.WriteLine($"Scene {i}: name='{scene.Attribute("name")?.Value}' gameTypes={scene.Elements("GameType").Count()}");
                    i++;
                }
            }

            // 结构化对比
            Assert.True(areEqual);
        }
    }
}