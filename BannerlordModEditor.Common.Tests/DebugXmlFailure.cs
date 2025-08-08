using System;
using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugXmlFailure
    {
        [Fact(Skip = "Debug use only")]
        public void Debug_CombatParameters_EmptyTest()
        {
            var xmlPath = "TestData/combat_parameters.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParameters>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine("Original XML:");
            Console.WriteLine(xml.Substring(0, Math.Min(500, xml.Length)));
            Console.WriteLine("\nSerialized XML:");
            Console.WriteLine(xml2.Substring(0, Math.Min(500, xml2.Length)));
            
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine("First 5 missing attributes:");
                foreach (var miss in report.MissingAttributes.Take(5)) Console.WriteLine($"  {miss}");
                
                Console.WriteLine("First 5 extra attributes:");
                foreach (var extra in report.ExtraAttributes.Take(5)) Console.WriteLine($"  {extra}");
            }
        }
    }
}