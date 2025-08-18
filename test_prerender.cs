using System;
using System.IO;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            var xml = File.ReadAllText(xmlPath);
            
            var obj = XmlTestUtils.Deserialize<PrerenderDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine("=== Serialized XML ===");
            Console.WriteLine(serializedXml);
            Console.WriteLine("====================");
            
            Console.WriteLine($"HasPostfxGraphs: {obj.HasPostfxGraphs}");
            Console.WriteLine($"PostfxGraphs count: {obj.PostfxGraphs.PostfxGraphList.Count}");
        }
    }
}