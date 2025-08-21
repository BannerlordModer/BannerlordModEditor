using Xunit;
using System.IO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class DecalSetsSimpleComparisonTests
{
    [Fact]
    public void SimpleComparison()
    {
        var testDataPath = Path.Combine("TestData", "decal_sets.xml");
        if (File.Exists(testDataPath))
        {
            var originalXml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<DecalSetsDTO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(dto, originalXml);
            
            // 标准化两个XML然后直接比较字符串
            var normalizedOriginal = XmlTestUtils.NormalizeXml(originalXml);
            var normalizedSerialized = XmlTestUtils.NormalizeXml(serializedXml);
            
            System.Console.WriteLine("=== Normalized Original ===");
            System.Console.WriteLine(normalizedOriginal);
            
            System.Console.WriteLine("=== Normalized Serialized ===");
            System.Console.WriteLine(normalizedSerialized);
            
            var areEqual = normalizedOriginal == normalizedSerialized;
            System.Console.WriteLine($"=== Direct String Comparison: {areEqual} ===");
            
            // 逐行比较
            var originalLines = normalizedOriginal.Split('\n');
            var serializedLines = normalizedSerialized.Split('\n');
            
            System.Console.WriteLine($"=== Line Count: {originalLines.Length} vs {serializedLines.Length} ===");
            
            for (int i = 0; i < Math.Min(originalLines.Length, serializedLines.Length); i++)
            {
                if (originalLines[i] != serializedLines[i])
                {
                    System.Console.WriteLine($"=== Difference at line {i + 1} ===");
                    System.Console.WriteLine($"Original: '{originalLines[i]}'");
                    System.Console.WriteLine($"Serialized: '{serializedLines[i]}'");
                }
            }
        }
    }
}