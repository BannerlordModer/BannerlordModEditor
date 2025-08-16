using System.IO;
using System.Xml.Linq;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsDecalMaterialValidationTest
    {
        [Fact]
        public void Validate_DecalMaterial_Fix()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 分析原始XML
            var doc1 = XDocument.Parse(xml);
            var originalDecalMaterials = doc1.Descendants("decal_material").Count();
            var originalDecalMaterialsContainers = doc1.Descendants("decal_materials").Count();
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的XML
            var doc2 = XDocument.Parse(xml2);
            var serializedDecalMaterials = doc2.Descendants("decal_material").Count();
            var serializedDecalMaterialsContainers = doc2.Descendants("decal_materials").Count();
            
            // 输出结果
            var result = $"=== DecalMaterial修复验证结果 ===\n\n";
            result += $"原始XML:\n";
            result += $"  decal_material元素数: {originalDecalMaterials}\n";
            result += $"  decal_materials元素数: {originalDecalMaterialsContainers}\n\n";
            
            result += $"序列化后XML:\n";
            result += $"  decal_material元素数: {serializedDecalMaterials}\n";
            result += $"  decal_materials元素数: {serializedDecalMaterialsContainers}\n\n";
            
            result += $"修复状态:\n";
            result += $"  decal_material: {(originalDecalMaterials == serializedDecalMaterials ? "✓ 已修复" : "✗ 仍有问题")}\n";
            result += $"  decal_materials: {(originalDecalMaterialsContainers == serializedDecalMaterialsContainers ? "✓ 已修复" : "✗ 仍有问题")}\n\n";
            
            if (originalDecalMaterials == serializedDecalMaterials && 
                originalDecalMaterialsContainers == serializedDecalMaterialsContainers)
            {
                result += "🎉 DecalMaterial问题已成功解决！\n";
            }
            else
            {
                result += "❌ DecalMaterial问题仍未解决，需要进一步调查。\n";
            }
            
            // 保存结果
            File.WriteAllText("TestData/decal_material_validation_result.txt", result);
            
            // 验证修复
            Assert.Equal(originalDecalMaterials, serializedDecalMaterials);
            Assert.Equal(originalDecalMaterialsContainers, serializedDecalMaterialsContainers);
        }
    }
}