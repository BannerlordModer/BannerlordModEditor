using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionTypesDebugTest
    {
        [Fact]
        public void ActionTypesSimpleTest()
        {
            string testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""act_charge_start"" type=""act_charge"" usage_direction=""forward"" />
	<action name=""act_charge"" type=""act_charge"" usage_direction=""forward"" />
	<action name=""act_attack_with_offhand"" type=""act_melee_attack"" usage_direction=""forward"" />
</action_types>";

            // Deserialize
            var obj = XmlTestUtils.Deserialize<ActionTypesDO>(testXml);
            
            // Serialize back
            var serialized = XmlTestUtils.Serialize(obj);
            
            // Compare
            var diff = XmlTestUtils.CompareXmlStructure(testXml, serialized);
            
            Console.WriteLine("--- Diff Report ---");
            Console.WriteLine($"IsStructurallyEqual: {diff.IsStructurallyEqual}");
            Console.WriteLine($"NodeCountDifference: {diff.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {diff.AttributeCountDifference}");
            
            if (diff.AttributeValueDifferences.Count > 0)
            {
                Console.WriteLine($"AttributeValueDifferences count: {diff.AttributeValueDifferences.Count}");
                foreach (var diffItem in diff.AttributeValueDifferences.Take(5))
                    Console.WriteLine($"  {diffItem}");
            }
            
            Assert.True(diff.IsStructurallyEqual, "Simple ActionTypes should be structurally equal");
        }
    }
}