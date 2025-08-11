using System;
using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugBoneBodyTypesTest
    {
        [Fact]
        public void Debug_BoneBodyTypes_Deserialization_Serialization()
        {
            var xmlPath = "TestData/bone_body_types.xml";
            if (!File.Exists(xmlPath))
            {
                return;
            }
            
            var xml = File.ReadAllText(xmlPath);
            
            // Count nodes and attributes in original
            var originalCount = XmlTestUtils.CountNodesAndAttributes(xml);
            Console.WriteLine($"Original - Nodes: {originalCount.nodeCount}, Attributes: {originalCount.attrCount}");
            
            // Deserialize
            var obj = XmlTestUtils.Deserialize<BoneBodyTypesDO>(xml);
            
            // Count bone_body_type elements
            Console.WriteLine($"Deserialized Items Count: {obj.Items?.Count ?? 0}");
            
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
                
            // Save files for debugging if different
            if (!diff.IsStructurallyEqual)
            {
                Directory.CreateDirectory("Debug");
                File.WriteAllText("Debug/original_bone_body_types.xml", xml);
                File.WriteAllText("Debug/serialized_bone_body_types.xml", serialized);
            }
        }
    }
}