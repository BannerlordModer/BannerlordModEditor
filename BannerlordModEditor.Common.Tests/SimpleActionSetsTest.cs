using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleActionSetsTest
    {
        [Fact]
        public void Test_Small_Section_ActionSets()
        {
            // Create a simple test XML with just a few action_sets
            string testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_sets>
	<action_set
		id=""as_human_warrior""
		skeleton=""human_skeleton""
		movement_system=""bipedal"">
		<action
			type=""act_troop_cavalry_sword""
			animation=""pose_cavalry_sword"" />
		<action
			type=""act_inventory_idle_start""
			animation=""inventory_idle_start"" />
	</action_set>
	<action_set
		id=""as_human_female_warrior""
		base_set=""as_human_warrior"">
		<action
			type=""act_run_idle_unarmed""
			animation=""female_stand_non_combat"" />
		<action
			type=""act_run_forward_unarmed""
			animation=""female_run_forward_unarmed"" />
	</action_set>
	<action_set
		id=""as_sheep""
		skeleton=""sheep_skeleton_01""
		movement_system=""quadrupedal"">
		<action
			type=""act_graze""
			animation=""sheep_graze"" />
	</action_set>
</action_sets>";

            // Deserialize
            var obj = XmlTestUtils.Deserialize<ActionSetsDO>(testXml);
            
            // Serialize back
            var serialized = XmlTestUtils.Serialize(obj, testXml);
            
            // Compare
            var diff = XmlTestUtils.CompareXmlStructure(testXml, serialized);
            
            // Output the serialized XML for debugging
            Console.WriteLine("--- Serialized XML ---");
            Console.WriteLine(serialized);
            
            Console.WriteLine("--- Diff Report ---");
            Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
            
            if (diff.AttributeValueDifferences.Count > 0)
                Console.WriteLine($"AttributeValueDifferences: {string.Join(", ", diff.AttributeValueDifferences)}");
                
            Assert.True(diff.IsStructurallyEqual);
        }

        [Fact]
        public void Test_First_50_ActionSets()
        {
            var xmlPath = "TestData/action_sets.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // Parse the XML and get the first 50 action_set elements
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            
            if (root != null)
            {
                var actionSets = root.Elements("action_set").ToList();
                var first50 = actionSets.Take(50).ToList();
                
                // Create a new XML with just the first 50 action sets
                var newRoot = new XElement("action_sets");
                foreach (var actionSet in first50)
                {
                    newRoot.Add(actionSet);
                }
                
                var testXml = newRoot.ToString();
                var fullDeclaration = @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine;
                var testXmlWithDeclaration = fullDeclaration + testXml;
                
                Console.WriteLine($"Testing with {first50.Count} action sets");
                
                // Deserialize
                var obj = XmlTestUtils.Deserialize<ActionSetsDO>(testXmlWithDeclaration);
                
                Console.WriteLine($"Deserialized {obj.ActionSets.Count} action sets");
                
                // Serialize back
                var serialized = XmlTestUtils.Serialize(obj, testXmlWithDeclaration);
                
                // Compare
                var diff = XmlTestUtils.CompareXmlStructure(testXmlWithDeclaration, serialized);
                
                Console.WriteLine("--- Diff Report ---");
                Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
                Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
                Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
                
                if (diff.MissingNodes.Count > 0)
                    Console.WriteLine($"MissingNodes: {string.Join(", ", diff.MissingNodes)}");
                if (diff.ExtraNodes.Count > 0)
                    Console.WriteLine($"ExtraNodes: {string.Join(", ", diff.ExtraNodes)}");
                if (diff.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine($"AttributeValueDifferences count: {diff.AttributeValueDifferences.Count}");
                    foreach (var diffItem in diff.AttributeValueDifferences.Take(5))
                        Console.WriteLine($"  {diffItem}");
                }
                    
                Assert.True(diff.IsStructurallyEqual, "First 50 action sets should be structurally equal");
            }
            else
            {
                Assert.Fail("Could not parse action_sets.xml");
            }
        }

        [Fact]
        public void Test_First_200_ActionSets()
        {
            var xmlPath = "TestData/action_sets.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // Parse the XML and get the first 200 action_set elements
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            
            if (root != null)
            {
                var actionSets = root.Elements("action_set").ToList();
                var first200 = actionSets.Take(200).ToList();
                
                // Create a new XML with just the first 200 action sets
                var newRoot = new XElement("action_sets");
                foreach (var actionSet in first200)
                {
                    newRoot.Add(actionSet);
                }
                
                var testXml = newRoot.ToString();
                var fullDeclaration = @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine;
                var testXmlWithDeclaration = fullDeclaration + testXml;
                
                Console.WriteLine($"Testing with {first200.Count} action sets");
                
                // Deserialize
                var obj = XmlTestUtils.Deserialize<ActionSetsDO>(testXmlWithDeclaration);
                
                Console.WriteLine($"Deserialized {obj.ActionSets.Count} action sets");
                
                // Serialize back
                var serialized = XmlTestUtils.Serialize(obj, testXmlWithDeclaration);
                
                // Compare
                var diff = XmlTestUtils.CompareXmlStructure(testXmlWithDeclaration, serialized);
                
                Console.WriteLine("--- Diff Report ---");
                Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
                Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
                Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
                
                if (diff.MissingNodes.Count > 0)
                    Console.WriteLine($"MissingNodes: {string.Join(", ", diff.MissingNodes)}");
                if (diff.ExtraNodes.Count > 0)
                    Console.WriteLine($"ExtraNodes: {string.Join(", ", diff.ExtraNodes)}");
                if (diff.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine($"AttributeValueDifferences count: {diff.AttributeValueDifferences.Count}");
                    foreach (var diffItem in diff.AttributeValueDifferences.Take(5))
                        Console.WriteLine($"  {diffItem}");
                }
                    
                Assert.True(diff.IsStructurallyEqual, "First 200 action sets should be structurally equal");
            }
            else
            {
                Assert.Fail("Could not parse action_sets.xml");
            }
        }

        [Fact]
        public void FindBreakingPoint()
        {
            var xmlPath = "TestData/action_sets.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // Parse the XML
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            
            if (root != null)
            {
                var actionSets = root.Elements("action_set").ToList();
                Console.WriteLine($"Total action sets in file: {actionSets.Count}");
                
                // Test with increasing numbers of action sets
                for (int count = 100; count <= actionSets.Count; count += 100)
                {
                    var subset = actionSets.Take(count).ToList();
                    
                    // Create a new XML with the subset
                    var newRoot = new XElement("action_sets");
                    foreach (var actionSet in subset)
                    {
                        newRoot.Add(actionSet);
                    }
                    
                    var testXml = newRoot.ToString();
                    var fullDeclaration = @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine;
                    var testXmlWithDeclaration = fullDeclaration + testXml;
                    
                    try
                    {
                        // Deserialize
                        var obj = XmlTestUtils.Deserialize<ActionSetsDO>(testXmlWithDeclaration);
                        
                        // Serialize back
                        var serialized = XmlTestUtils.Serialize(obj, testXmlWithDeclaration);
                        
                        // Compare
                        var diff = XmlTestUtils.CompareXmlStructure(testXmlWithDeclaration, serialized);
                        
                        if (!diff.IsStructurallyEqual)
                        {
                            Console.WriteLine($"Breaking point found at {count} action sets");
                            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
                            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
                            
                            if (diff.AttributeValueDifferences.Count > 0)
                            {
                                Console.WriteLine($"AttributeValueDifferences count: {diff.AttributeValueDifferences.Count}");
                                foreach (var diffItem in diff.AttributeValueDifferences.Take(10))
                                    Console.WriteLine($"  {diffItem}");
                            }
                            
                            // Save the problematic XML for debugging
                            var debugDir = Path.Combine("Debug");
                            Directory.CreateDirectory(debugDir);
                            File.WriteAllText(Path.Combine(debugDir, $"problematic_action_sets_{count}.xml"), testXmlWithDeclaration);
                            File.WriteAllText(Path.Combine(debugDir, $"serialized_action_sets_{count}.xml"), serialized);
                            
                            Assert.Fail($"Breaking point found at {count} action sets");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception at {count} action sets: {ex.Message}");
                        Assert.Fail($"Exception at {count} action sets: {ex.Message}");
                        return;
                    }
                }
                
                Console.WriteLine("No breaking point found up to full file size");
            }
            else
            {
                Assert.Fail("Could not parse action_sets.xml");
            }
        }
    }
}