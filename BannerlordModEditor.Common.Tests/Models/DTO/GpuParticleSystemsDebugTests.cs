using Xunit;
using System.IO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class GpuParticleSystemsDebugTests
{
    [Fact]
    public void DebugSerialization()
    {
        var testDataPath = Path.Combine("TestData", "gpu_particle_systems.xml");
        if (File.Exists(testDataPath))
        {
            var originalXml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<GpuParticleSystemsDTO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(dto, originalXml);
            
            // 输出原始XML
            System.Console.WriteLine("=== Original XML ===");
            System.Console.WriteLine(originalXml);
            
            // 输出序列化后的XML
            System.Console.WriteLine("=== Serialized XML ===");
            System.Console.WriteLine(serializedXml);
            
            // 检查是否结构相等
            var isEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            System.Console.WriteLine($"=== Are Equal: {isEqual} ===");
        }
    }
}