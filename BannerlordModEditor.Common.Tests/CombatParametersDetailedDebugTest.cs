using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests
{
    public class CombatParametersDetailedDebugTest
    {
        [Fact]
        public void CombatParameters_Full_Detailed_Debug()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters.xml");
            string xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"Original XML length: {xml.Length}");
            
            // Parse with XDocument to see structure
            var doc = XDocument.Parse(xml);
            Console.WriteLine($"Root element: {doc.Root?.Name}");
            Console.WriteLine($"Definitions element exists: {doc.Root?.Element("definitions") != null}");
            Console.WriteLine($"CombatParameters element exists: {doc.Root?.Element("combat_parameters") != null}");
            
            if (doc.Root?.Element("definitions") != null)
            {
                Console.WriteLine($"Definitions count: {doc.Root.Element("definitions")?.Elements("def").Count()}");
            }
            
            if (doc.Root?.Element("combat_parameters") != null)
            {
                Console.WriteLine($"CombatParameters count: {doc.Root.Element("combat_parameters")?.Elements("combat_parameter").Count()}");
            }
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
            
            // 检查对象状态
            Console.WriteLine($"Definitions: {obj.Definitions}");
            Console.WriteLine($"Definitions.Defs count: {obj.Definitions.Defs.Count}");
            Console.WriteLine($"CombatParametersList count: {obj.CombatParametersList.Count}");
            Console.WriteLine($"HasDefinitions: {obj.HasDefinitions}");
            Console.WriteLine($"Type: {obj.Type}");
            
            // 再序列化
            string serialized = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"Serialized XML length: {serialized.Length}");
            
            // Parse serialized XML
            var serializedDoc = XDocument.Parse(serialized);
            Console.WriteLine($"Serialized Root element: {serializedDoc.Root?.Name}");
            Console.WriteLine($"Serialized Definitions element exists: {serializedDoc.Root?.Element("definitions") != null}");
            Console.WriteLine($"Serialized CombatParameters element exists: {serializedDoc.Root?.Element("combat_parameters") != null}");
            
            if (serializedDoc.Root?.Element("definitions") != null)
            {
                Console.WriteLine($"Serialized Definitions count: {serializedDoc.Root.Element("definitions")?.Elements("def").Count()}");
            }
            
            if (serializedDoc.Root?.Element("combat_parameters") != null)
            {
                Console.WriteLine($"Serialized CombatParameters count: {serializedDoc.Root.Element("combat_parameters")?.Elements("combat_parameter").Count()}");
            }
            
            // Save both for comparison
            var debugPath = Path.Combine("Debug", $"combat_parameters_debug_{DateTime.Now:yyyyMMdd_HHmmss}");
            Directory.CreateDirectory(debugPath);
            File.WriteAllText(Path.Combine(debugPath, "original.xml"), xml);
            File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), serialized);
            Console.WriteLine($"Debug files saved to: {debugPath}");
            
            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            
            Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
            Console.WriteLine($"MissingNodes: {string.Join(", ", diff.MissingNodes)}");
            Console.WriteLine($"ExtraNodes: {string.Join(", ", diff.ExtraNodes)}");
            
            // 输出节点和属性统计
            var (nodeCountA, attrCountA) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (nodeCountB, attrCountB) = XmlTestUtils.CountNodesAndAttributes(serialized);
            Console.WriteLine($"Original - Nodes: {nodeCountA}, Attributes: {attrCountA}");
            Console.WriteLine($"Serialized - Nodes: {nodeCountB}, Attributes: {attrCountB}");
        }
    }
}