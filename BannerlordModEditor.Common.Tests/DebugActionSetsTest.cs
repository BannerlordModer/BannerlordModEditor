using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugActionSetsTest
    {
        [Fact]
        public void Debug_ActionSets_Deserialization_Serialization()
        {
            var xmlPath = "./BannerlordModEditor.Common.Tests/TestData/action_sets.xml";
            if (!File.Exists(xmlPath))
            {
                xmlPath = "TestData/action_sets.xml"; // Try alternative path
                if (!File.Exists(xmlPath))
                {
                    // Skip test if data not found
                    return;
                }
            }
            
            var xml = File.ReadAllText(xmlPath);
            
            // Count nodes and attributes in original
            var originalDoc = XDocument.Parse(xml);
            var originalCount = XmlTestUtils.CountNodesAndAttributes(xml);
            Console.WriteLine($"Original - Nodes: {originalCount.nodeCount}, Attributes: {originalCount.attrCount}");
            
            // Deserialize
            var obj = XmlTestUtils.Deserialize<ActionSetsDO>(xml);
            
            // Serialize back
            var serialized = XmlTestUtils.Serialize(obj);
            
            // Count nodes and attributes in serialized
            var serializedCount = XmlTestUtils.CountNodesAndAttributes(serialized);
            Console.WriteLine($"Serialized - Nodes: {serializedCount.nodeCount}, Attributes: {serializedCount.attrCount}");
            
            // Compare structures
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            
            Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
            
            if (diff.MissingNodes.Count > 0)
                Console.WriteLine($"MissingNodes: {string.Join(", ", diff.MissingNodes)}");
            if (diff.ExtraNodes.Count > 0)
                Console.WriteLine($"ExtraNodes: {string.Join(", ", diff.ExtraNodes)}");
            if (diff.AttributeValueDifferences.Count > 0)
                Console.WriteLine($"AttributeValueDifferences: {string.Join(", ", diff.AttributeValueDifferences)}");
                
            // Save files for debugging
            Directory.CreateDirectory("Debug");
            File.WriteAllText("Debug/original_action_sets.xml", xml);
            File.WriteAllText("Debug/serialized_action_sets.xml", serialized);
        }
    }
}