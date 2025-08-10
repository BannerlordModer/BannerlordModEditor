using BannerlordModEditor.Common.Models;
using Xunit;
using System.IO;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Tests
{
    public class UpdatedMpItemsXmlTests : XmlModelTestBase<MpItems>
    {
        protected override string TestDataFileName => "mpitems.xml";
        protected override string ModelTypeName => "MpItems";

        // This class now inherits all round-trip tests from XmlModelTestBase
        // which provides comprehensive testing including skip handling, graceful missing file handling,
        // and enhanced comparison with boolean value tolerance

        [Fact]
        public void DebugTest_BattaniaCloak_DeserializeAndSerialize()
        {
            // Test the specific failing case
            var xmlPath = "BannerlordModEditor.Common.Tests/TestSubsets/MpItems/mp_battania_cloak_b.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // Deserialize
            var item = XmlTestUtils.Deserialize<Item>(xml);
            
            // Serialize back
            var serialized = XmlTestUtils.Serialize(item, xml);
            
            // Compare
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            
            // Save debug files
            var debugPath = Path.Combine("Debug", $"debug_test_{DateTime.Now:yyyyMMdd_HHmmss}");
            Directory.CreateDirectory(debugPath);
            File.WriteAllText(Path.Combine(debugPath, "original.xml"), xml);
            File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), serialized);
            File.WriteAllText(Path.Combine(debugPath, "diff_report.txt"), 
                $"Structurally equal: {diff.IsStructurallyEqual}\n" +
                $"Attribute count difference: {diff.AttributeCountDifference}\n" +
                $"Missing attributes: {string.Join(", ", diff.MissingAttributes)}\n" +
                $"Extra attributes: {string.Join(", ", diff.ExtraAttributes)}\n" +
                $"Item.Difficulty: {item.Difficulty}\n" +
                $"Item.DifficultySpecified: {item.DifficultySpecified}");
            
            Assert.True(diff.IsStructurallyEqual, 
                $"XML not structurally equal: {diff.AttributeCountDifference}, " +
                $"Missing: {string.Join(", ", diff.MissingAttributes)}, " +
                $"Extra: {string.Join(", ", diff.ExtraAttributes)}");
        }
    }
}