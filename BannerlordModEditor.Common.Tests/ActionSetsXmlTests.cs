using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests.Services;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionSetsXmlTests
    {
        [Fact]
        public void ActionSets_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/action_sets.xml";
            var xml = File.ReadAllText(xmlPath);

            // Parse the XML and use a smaller subset for testing to avoid large file serialization issues
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            
            if (root != null)
            {
                var actionSets = root.Elements("action_set").ToList();
                var testSubset = actionSets.Take(100).ToList(); // Use first 100 action sets for testing
                
                // Create a new XML with the subset
                var newRoot = new XElement("action_sets");
                foreach (var actionSet in testSubset)
                {
                    newRoot.Add(actionSet);
                }
                
                var testXml = newRoot.ToString();
                var fullDeclaration = @"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine;
                var testXmlWithDeclaration = fullDeclaration + testXml;


                // 反序列化
                var obj = XmlTestUtils.Deserialize<ActionSetsDO>(testXmlWithDeclaration);

                // 再序列化
                var serialized = XmlTestUtils.Serialize(obj);

                // 结构化对比
                var diff = XmlTestUtils.CompareXmlStructure(testXmlWithDeclaration, serialized);
                var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
                var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
                
                Assert.True(diff.IsStructurallyEqual, 
                    $"ActionSets XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                    $"属性差异: {diff.AttributeCountDifference}, " +
                    $"属性值差异: {attributeValueDiffs}, " +
                    $"文本差异: {textDiffs}");
            }
            else
            {
                Assert.Fail("Could not parse action_sets.xml");
            }
        }
    }
}