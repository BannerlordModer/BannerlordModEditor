using BannerlordModEditor.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MpItemsSubsetTests
    {
        public static IEnumerable<object[]> GetItemFiles()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var subsetDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestSubsets", "MpItems");
            if (!Directory.Exists(subsetDirectory))
            {
                return Enumerable.Empty<object[]>();
            }
            return Directory.GetFiles(subsetDirectory, "*.xml").Select(file => new object[] { file });
        }

        [Theory]
        [MemberData(nameof(GetItemFiles))]
        public void SingleItem_LoadAndSave_ShouldBeLogicallyIdentical(string itemFilePath)
        {
            // Arrange
            var xml = File.ReadAllText(itemFilePath);
            
            // Act
            var item = XmlTestUtils.Deserialize<Item>(xml);
            var xml2 = XmlTestUtils.Serialize(item);

            // Assert - 使用结构比较作为判断标准
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            
            if (!diff.IsStructurallyEqual)
            {
                var errorMessage = $"File '{Path.GetFileName(itemFilePath)}' is not structurally equivalent.\n";
                errorMessage += $"Node count difference: {diff.NodeCountDifference}\n";
                errorMessage += $"Attribute count difference: {diff.AttributeCountDifference}\n";
                errorMessage += $"Missing nodes: {string.Join(", ", diff.MissingNodes)}\n";
                errorMessage += $"Extra nodes: {string.Join(", ", diff.ExtraNodes)}\n";
                errorMessage += $"Node name differences: {string.Join(", ", diff.NodeNameDifferences)}\n";
                errorMessage += $"Missing attributes: {string.Join(", ", diff.MissingAttributes)}\n";
                errorMessage += $"Extra attributes: {string.Join(", ", diff.ExtraAttributes)}\n";
                errorMessage += $"Attribute value differences: {string.Join(", ", diff.AttributeValueDifferences)}\n";
                errorMessage += $"Text differences: {string.Join(", ", diff.TextDifferences)}\n";
                
                // Output original and serialized XML to files for debugging
                var debugPath = Path.Combine("Debug", $"mpitems_{Path.GetFileNameWithoutExtension(itemFilePath)}_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(debugPath);
                File.WriteAllText(Path.Combine(debugPath, "original.xml"), xml);
                File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), xml2);
                errorMessage += $"Debug files saved to: {debugPath}";
                
                Assert.False(true, errorMessage);
            }
        }
    }
}