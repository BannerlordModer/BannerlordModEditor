using Xunit;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class DecalSetsDetailedDebugTests
{
    [Fact]
    public void DetailedDebugSerialization()
    {
        var testDataPath = Path.Combine("TestData", "decal_sets.xml");
        if (File.Exists(testDataPath))
        {
            var originalXml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<DecalSetsDTO>(originalXml);
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
            
            // 详细比较
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            System.Console.WriteLine("=== Detailed Comparison ===");
            
            // 比较根元素属性
            var originalRoot = originalDoc.Root;
            var serializedRoot = serializedDoc.Root;
            
            System.Console.WriteLine("Root element attributes comparison:");
            foreach (var attr in originalRoot.Attributes())
            {
                var serializedAttr = serializedRoot.Attribute(attr.Name);
                if (serializedAttr == null)
                {
                    System.Console.WriteLine($"  Missing attribute: {attr.Name}");
                }
                else if (attr.Value != serializedAttr.Value)
                {
                    System.Console.WriteLine($"  Attribute {attr.Name} differs: '{attr.Value}' vs '{serializedAttr.Value}'");
                }
                else
                {
                    System.Console.WriteLine($"  Attribute {attr.Name}: '{attr.Value}' (OK)");
                }
            }
            
            // 比较子元素
            var originalDecalSets = originalRoot.Element("decal_sets");
            var serializedDecalSets = serializedRoot.Element("decal_sets");
            
            if (originalDecalSets != null && serializedDecalSets != null)
            {
                var originalDecalSetElements = originalDecalSets.Elements("decal_set").ToList();
                var serializedDecalSetElements = serializedDecalSets.Elements("decal_set").ToList();
                
                System.Console.WriteLine($"Decal set count: {originalDecalSetElements.Count} vs {serializedDecalSetElements.Count}");
                
                for (int i = 0; i < Math.Min(originalDecalSetElements.Count, serializedDecalSetElements.Count); i++)
                {
                    var originalElement = originalDecalSetElements[i];
                    var serializedElement = serializedDecalSetElements[i];
                    
                    System.Console.WriteLine($"Decal set {i} attributes comparison:");
                    foreach (var attr in originalElement.Attributes())
                    {
                        var serializedAttr = serializedElement.Attribute(attr.Name);
                        if (serializedAttr == null)
                        {
                            System.Console.WriteLine($"  Missing attribute: {attr.Name}");
                        }
                        else if (attr.Value != serializedAttr.Value)
                        {
                            System.Console.WriteLine($"  Attribute {attr.Name} differs: '{attr.Value}' vs '{serializedAttr.Value}'");
                        }
                        else
                        {
                            System.Console.WriteLine($"  Attribute {attr.Name}: '{attr.Value}' (OK)");
                        }
                    }
                }
            }
        }
    }
}