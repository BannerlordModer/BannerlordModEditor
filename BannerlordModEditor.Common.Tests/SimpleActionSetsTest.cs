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
    }
}