using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests
{
    public class CombatParametersDebugTest
    {
        [Fact]
        public void CombatParameters_Part1_Debug()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters_part1.xml");
            string xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"Original XML length: {xml.Length}");
            Console.WriteLine($"Original XML preview:\n{xml.Substring(0, Math.Min(200, xml.Length))}");
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
            
            // 检查对象状态
            Console.WriteLine($"Definitions: {obj.Definitions}");
            Console.WriteLine($"Definitions.Defs count: {obj.Definitions.Defs.Count}");
            Console.WriteLine($"CombatParametersList count: {obj.CombatParametersList.Count}");
            Console.WriteLine($"HasDefinitions: {obj.HasDefinitions}");
            
            // 再序列化
            string serialized = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"Serialized XML length: {serialized.Length}");
            Console.WriteLine($"Serialized XML preview:\n{serialized.Substring(0, Math.Min(200, serialized.Length))}");
            
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

        [Fact]
        public void CombatParameters_Full_Debug()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters.xml");
            string xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"Original XML length: {xml.Length}");
            Console.WriteLine($"Original XML preview:\n{xml.Substring(0, Math.Min(200, xml.Length))}");
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
            
            // 检查对象状态
            Console.WriteLine($"Definitions: {obj.Definitions}");
            Console.WriteLine($"Definitions.Defs count: {obj.Definitions.Defs.Count}");
            Console.WriteLine($"CombatParametersList count: {obj.CombatParametersList.Count}");
            Console.WriteLine($"HasDefinitions: {obj.HasDefinitions}");
            
            // 再序列化
            string serialized = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"Serialized XML length: {serialized.Length}");
            Console.WriteLine($"Serialized XML preview:\n{serialized.Substring(0, Math.Min(200, serialized.Length))}");
            
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

        [Fact]
        public void CombatParameters_Part2_Debug()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters_part2.xml");
            string xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"Original XML length: {xml.Length}");
            Console.WriteLine($"Original XML preview:\n{xml.Substring(0, Math.Min(200, xml.Length))}");
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
            
            // 检查对象状态
            Console.WriteLine($"Definitions: {obj.Definitions}");
            Console.WriteLine($"Definitions.Defs count: {obj.Definitions.Defs.Count}");
            Console.WriteLine($"CombatParametersList count: {obj.CombatParametersList.Count}");
            Console.WriteLine($"HasDefinitions: {obj.HasDefinitions}");
            
            // 再序列化
            string serialized = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"Serialized XML length: {serialized.Length}");
            Console.WriteLine($"Serialized XML preview:\n{serialized.Substring(0, Math.Min(200, serialized.Length))}");
            
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