using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;

class Program
{
    static void Main()
    {
        var xmlPath = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common.Tests/TestData/looknfeel.xml";
        var xml = File.ReadAllText(xmlPath);

        Console.WriteLine("测试命名空间处理...");
        
        // 反序列化
        var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);

        // 测试1：直接序列化，不传递原始XML
        Console.WriteLine("\n=== 测试1：不传递原始XML ===");
        var xml1 = XmlTestUtils.Serialize(obj);
        var doc1 = XDocument.Parse(xml1);
        Console.WriteLine("根元素属性:");
        foreach (var attr in doc1.Root.Attributes())
        {
            Console.WriteLine($"  {attr.Name.LocalName} = {attr.Value}");
        }
        var attrs1 = doc1.Descendants().SelectMany(e => e.Attributes()).Count();
        Console.WriteLine($"属性数量: {attrs1}");

        // 测试2：传递原始XML
        Console.WriteLine("\n=== 测试2：传递原始XML ===");
        var xml2 = XmlTestUtils.Serialize(obj, xml);
        var doc2 = XDocument.Parse(xml2);
        Console.WriteLine("根元素属性:");
        foreach (var attr in doc2.Root.Attributes())
        {
            Console.WriteLine($"  {attr.Name.LocalName} = {attr.Value}");
        }
        var attrs2 = doc2.Descendants().SelectMany(e => e.Attributes()).Count();
        Console.WriteLine($"属性数量: {attrs2}");
        
        Console.WriteLine($"\n原始属性数量: 1220");
        Console.WriteLine($"不传递原始XML: {attrs1} (差异: {attrs1 - 1220})");
        Console.WriteLine($"传递原始XML: {attrs2} (差异: {attrs2 - 1220})");
    }
}