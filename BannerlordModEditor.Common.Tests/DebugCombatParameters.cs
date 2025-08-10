using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugCombatParameters : IDisposable
    {
        private readonly ITestOutputHelper _output;
        public DebugCombatParameters(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CombatParameters_DebugStructuralIssues()
        {
            var xmlPath = "TestData/combat_parameters.xml";
            var xml = File.ReadAllText(xmlPath);
            
            _output.WriteLine("=== DEBUG: CombatParameters XML Analysis ===");
            
            // Deserialize
            var obj = XmlTestUtils.Deserialize<CombatParameters>(xml);
            
            // Serialize back
            var xml2 = XmlTestUtils.Serialize(obj);
            
            // Write both versions
            var origFile = "/tmp/original_combat_param.xml";
            var newFile = "/tmp/serialized_combat_param.xml";
            File.WriteAllText(origFile, xml);
            File.WriteAllText(newFile, xml2);
            
            _output.WriteLine($"Original length: {xml.Length}");
            _output.WriteLine($"Serialized length: {xml2.Length}");
            
            // Compare structures
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            
            _output.WriteLine($"Structural equality: {report.IsStructurallyEqual}");
            
            if (!report.IsStructurallyEqual)
            {
                _output.WriteLine("=== DIFFERENCES FOUND ===");
                
                if (report.MissingNodes.Any())
                {
                    _output.WriteLine("Missing nodes:");
                    foreach (var missed in report.MissingNodes) _output.WriteLine($"  {missed}");
                }
                
                if (report.ExtraNodes.Any())
                {
                    _output.WriteLine("Extra nodes:");
                    foreach (var extra in report.ExtraNodes) _output.WriteLine($"  {extra}");
                }
                
                if (report.MissingAttributes.Any())
                {
                    _output.WriteLine("Missing attributes:");
                    foreach (var missed in report.MissingAttributes) _output.WriteLine($"  {missed}");
                }
                
                if (report.ExtraAttributes.Any())
                {
                    _output.WriteLine("Extra attributes:");
                    foreach (var extra in report.ExtraAttributes) _output.WriteLine($"  {extra}");
                }
                
                if (report.AttributeValueDifferences.Any())
                {
                    _output.WriteLine("Attribute value differences:");
                    foreach (var diff in report.AttributeValueDifferences.Take(10)) _output.WriteLine($"  {diff}");
                }
                
                if (report.NodeCountDifference != null)
                    _output.WriteLine($"Node count difference: {report.NodeCountDifference}");
                if (report.AttributeCountDifference != null)
                    _output.WriteLine($"Attribute count difference: {report.AttributeCountDifference}");
            }
            
            _output.WriteLine($"Original file: {origFile}");
            _output.WriteLine($"New file: {newFile}");
            
            // Print structure counts
            var (origNodes, origAttrs) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (serNodes, serAttrs) = XmlTestUtils.CountNodesAndAttributes(xml2);
            _output.WriteLine($"Original: {origNodes} nodes, {origAttrs} attributes");
            _output.WriteLine($"Serialized: {serNodes} nodes, {serAttrs} attributes");
            
            // Force this to succeed so we see the debug output
            Assert.True(true);
        }

        public void Dispose()
        {
        }
    }
}