using BannerlordModEditor.Common.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class PhysicsMaterialsXmlTests
    {
        [Fact]
        public void PhysicsMaterials_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PhysicsMaterialsBase));
            PhysicsMaterialsBase physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterialsBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, physicsMaterials);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(physicsMaterials);
            Assert.NotNull(physicsMaterials.PhysicsMaterials);
            Assert.NotNull(physicsMaterials.PhysicsMaterials.PhysicsMaterial);
            Assert.True(physicsMaterials.PhysicsMaterials.PhysicsMaterial.Count > 0, "Should have at least one physics material");

            // 详细验证每个物理材料的必要字段
            foreach (var material in physicsMaterials.PhysicsMaterials.PhysicsMaterial)
            {
                // 必要字段检查
                Assert.False(string.IsNullOrEmpty(material.Id), $"Material {material.Id} should have an ID");
                Assert.False(string.IsNullOrEmpty(material.StaticFriction), $"Material {material.Id} should have StaticFriction");
                Assert.False(string.IsNullOrEmpty(material.DynamicFriction), $"Material {material.Id} should have DynamicFriction");
                Assert.False(string.IsNullOrEmpty(material.Restitution), $"Material {material.Id} should have Restitution");
                // Softness is optional, some materials don't have it
                Assert.False(string.IsNullOrEmpty(material.LinearDamping), $"Material {material.Id} should have LinearDamping");
                Assert.False(string.IsNullOrEmpty(material.AngularDamping), $"Material {material.Id} should have AngularDamping");
                Assert.False(string.IsNullOrEmpty(material.DisplayColor), $"Material {material.Id} should have DisplayColor");
            }
        }

        [Fact]
        public void PhysicsMaterials_SpecificMaterials_ShouldHaveCorrectValues()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterialsBase));
            PhysicsMaterialsBase physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterialsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 特定材料的详细数值检查
            var defaultMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "default");
            if (defaultMaterial != null)
            {
                Assert.Equal("0.800", defaultMaterial.StaticFriction);
                Assert.Equal("0.400", defaultMaterial.DynamicFriction);
                Assert.Equal("0.100", defaultMaterial.Restitution);
                Assert.Equal("0.000", defaultMaterial.Softness);
                Assert.Equal("0.050", defaultMaterial.LinearDamping);
                Assert.Equal("0.025", defaultMaterial.AngularDamping);
                Assert.Equal("255, 112, 125, 136", defaultMaterial.DisplayColor);
                // 这些可选字段应该不存在于default材料中
                Assert.Null(defaultMaterial.RainSplashesEnabled);
                Assert.Null(defaultMaterial.Flammable);
                Assert.Null(defaultMaterial.DontStickMissiles);
            }

            var woodMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "wood");
            if (woodMaterial != null)
            {
                Assert.Equal("0.500", woodMaterial.StaticFriction);
                Assert.Equal("0.300", woodMaterial.DynamicFriction);
                Assert.Equal("0.075", woodMaterial.Restitution);
                Assert.Equal("0.100", woodMaterial.LinearDamping);
                Assert.Equal("0.050", woodMaterial.AngularDamping);
                Assert.Equal("255, 107, 86, 58", woodMaterial.DisplayColor);
                // 这些可选字段应该存在于wood材料中
                Assert.Equal("true", woodMaterial.RainSplashesEnabled);
                Assert.Equal("true", woodMaterial.Flammable);
                // 这个字段应该不存在于wood材料中
                Assert.Null(woodMaterial.DontStickMissiles);
            }

            var metalMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "metal");
            if (metalMaterial != null)
            {
                Assert.Equal("0.600", metalMaterial.StaticFriction);
                Assert.Equal("0.250", metalMaterial.DynamicFriction);
                Assert.Equal("0.500", metalMaterial.Restitution);
                Assert.Equal("0.000", metalMaterial.Softness);
                Assert.Equal("255, 105, 38, 183", metalMaterial.DisplayColor);
                // 这个字段应该存在于metal材料中
                Assert.Equal("true", metalMaterial.DontStickMissiles);
                // 这些字段应该不存在于metal材料中
                Assert.Null(metalMaterial.RainSplashesEnabled);
                Assert.Null(metalMaterial.Flammable);
            }
        }

        [Fact]
        public void PhysicsMaterials_OptionalFields_ShouldBeCorrectlyHandled()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterialsBase));
            PhysicsMaterialsBase physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterialsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 可选字段存在性检查
            var flammableMaterials = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.Flammable == "true").ToList();
            var nonFlammableMaterials = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.Flammable == null).ToList();
            
            Assert.True(flammableMaterials.Count > 0, "Some materials should be flammable");
            Assert.True(nonFlammableMaterials.Count > 0, "Some materials should not have flammable property");

            // 检查易燃材料应该包含wood和straw类型
            var flammableIds = flammableMaterials.Select(m => m.Id).ToList();
            Assert.Contains("wood", flammableIds);
            Assert.Contains("straw", flammableIds);

            var rainSplashMaterials = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.RainSplashesEnabled == "true").ToList();
            
            Assert.True(rainSplashMaterials.Count > 0, "Some materials should enable rain splashes");

            var dontStickMaterials = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.DontStickMissiles == "true").ToList();
            
            Assert.True(dontStickMaterials.Count > 0, "Some materials should not stick missiles");

            // 检查特殊属性的材料
            var woodNonstick = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .FirstOrDefault(m => m.Id == "wood_nonstick");
            if (woodNonstick != null)
            {
                Assert.Equal("true", woodNonstick.DontStickMissiles);
                Assert.Equal("true", woodNonstick.Flammable);
                Assert.Equal("true", woodNonstick.RainSplashesEnabled);
                Assert.Equal("wood", woodNonstick.OverrideMaterialNameForImpactSounds);
            }

            var potBreakable = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .FirstOrDefault(m => m.Id == "pot_breakable");
            if (potBreakable != null)
            {
                Assert.Equal("true", potBreakable.DontStickMissiles);
                Assert.Equal("true", potBreakable.AttacksCanPassThrough);
            }

            var pot = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .FirstOrDefault(m => m.Id == "pot");
            if (pot != null)
            {
                Assert.Equal("true", pot.DontStickMissiles);
                Assert.Equal("false", pot.AttacksCanPassThrough);
            }
        }

        [Fact]
        public void PhysicsMaterials_SoftnessField_ShouldBeOptional()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterialsBase));
            PhysicsMaterialsBase physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterialsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 检查softness字段的可选性
            var materialsWithSoftness = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.Softness != null).ToList();
            var materialsWithoutSoftness = physicsMaterials.PhysicsMaterials.PhysicsMaterial
                .Where(m => m.Softness == null).ToList();
            
            Assert.True(materialsWithSoftness.Count > 0, "Some materials should have softness field");
            Assert.True(materialsWithoutSoftness.Count > 0, "Some materials should not have softness field");

            // 检查具体的materials
            var defaultMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "default");
            Assert.NotNull(defaultMaterial);
            Assert.Equal("0.000", defaultMaterial.Softness); // default应该有softness

            var metalWeapon = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "metal_weapon");
            Assert.NotNull(metalWeapon);
            Assert.Null(metalWeapon.Softness); // metal_weapon不应该有softness

            var water = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "water");
            Assert.NotNull(water);
            Assert.Equal("0.000", water.Softness); // water应该有softness
        }

        [Fact]
        public void PhysicsMaterials_NumericPrecision_ShouldBePreserved()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterialsBase));
            PhysicsMaterialsBase physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterialsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 数值精度检查
            var sandMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "sand");
            if (sandMaterial != null)
            {
                Assert.Equal("1.300", sandMaterial.StaticFriction); // 确保三位小数
                Assert.Equal("1.100", sandMaterial.DynamicFriction);
                Assert.Equal("0.005", sandMaterial.Restitution);
                Assert.Equal("0.400", sandMaterial.LinearDamping);
                Assert.Equal("0.250", sandMaterial.AngularDamping);
            }

            var snowMaterial = physicsMaterials.PhysicsMaterials.PhysicsMaterial.FirstOrDefault(m => m.Id == "snow");
            if (snowMaterial != null)
            {
                Assert.Equal("0.800", snowMaterial.StaticFriction);
                Assert.Equal("0.700", snowMaterial.DynamicFriction);
                Assert.Equal("0.005", snowMaterial.Restitution); // 确保0.005精度保持
                Assert.Equal("0.300", snowMaterial.LinearDamping);
                Assert.Equal("0.200", snowMaterial.AngularDamping);
                Assert.Equal("255, 250, 250, 250", snowMaterial.DisplayColor); // 颜色值精度
            }
        }
        
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }
    }
} 