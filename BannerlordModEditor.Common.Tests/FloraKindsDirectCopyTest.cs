using System;
using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDirectCopyTest
    {
        [Fact]
        public void Direct_Copy_Of_Original_Test()
        {
            // 直接复制原始测试的逻辑
            Console.WriteLine("=== 直接复制原始测试逻辑 ===");
            
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            bool areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            Console.WriteLine($"AreStructurallyEqual result: {areEqual}");
            
            if (!areEqual)
            {
                var report = XmlTestUtils.CompareXmlStructure(originalXml, serializedXml);
                Console.WriteLine($"Node count difference: {report.NodeCountDifference}");
                Console.WriteLine($"Attribute count difference: {report.AttributeCountDifference}");
                Console.WriteLine($"Missing nodes: {report.MissingNodes.Count}");
                Console.WriteLine($"Extra nodes: {report.ExtraNodes.Count}");
                Console.WriteLine($"Missing attributes: {report.MissingAttributes.Count}");
                Console.WriteLine($"Extra attributes: {report.ExtraAttributes.Count}");
                Console.WriteLine($"Attribute value differences: {report.AttributeValueDifferences.Count}");
                
                if (report.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine("Attribute value differences:");
                    foreach (var diff in report.AttributeValueDifferences.Take(5))
                    {
                        Console.WriteLine($"  {diff}");
                    }
                }
                
                // 保存文件进行手动检查
                File.WriteAllText("debug_original.xml", originalXml);
                File.WriteAllText("debug_serialized.xml", serializedXml);
            }
            
            Assert.True(areEqual, "XML should be structurally equal");
        }
    }
}