using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class PhysicsMaterialsXmlTests
    {
        [Fact]
        public void PhysicsMaterials_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            PhysicsMaterials physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterials)serializer.Deserialize(reader)!;
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
            Assert.NotNull(physicsMaterials.PhysicsMaterialList);
            Assert.NotNull(physicsMaterials.SoundAndCollisionInfoClassDefinitions);
            Assert.True(physicsMaterials.PhysicsMaterialList.Count > 0, "Should have at least one physics material");
            Assert.True(physicsMaterials.SoundAndCollisionInfoClassDefinitions.Count > 0, "Should have sound definitions");

            // 验证序列化后能正确反序列化
            var deserializedResult = serializer.Deserialize(new StringReader(savedXml)) as PhysicsMaterials;
            Assert.NotNull(deserializedResult);
            Assert.Equal(physicsMaterials.PhysicsMaterialList.Count, deserializedResult.PhysicsMaterialList.Count);
            Assert.Equal(physicsMaterials.SoundAndCollisionInfoClassDefinitions.Count, deserializedResult.SoundAndCollisionInfoClassDefinitions.Count);
        }

        [Fact]
        public void PhysicsMaterial_ShouldHandleAllAttributes()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            PhysicsMaterials physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterials)serializer.Deserialize(reader)!;
            }

            // Act
            var defaultMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "default");
            var woodMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "wood");
            var woodNonstickMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "wood_nonstick");
            var potMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "pot");

            // Assert - Basic material with minimal attributes
            Assert.Equal("default", defaultMaterial.Id);
            Assert.Equal(0.800f, defaultMaterial.StaticFriction);
            Assert.Equal(0.400f, defaultMaterial.DynamicFriction);
            Assert.Equal(0.100f, defaultMaterial.Restitution);
            Assert.Equal(0.000f, defaultMaterial.Softness);
            Assert.Equal(0.050f, defaultMaterial.LinearDamping);
            Assert.Equal(0.025f, defaultMaterial.AngularDamping);
            Assert.Equal("255, 112, 125, 136", defaultMaterial.DisplayColor);
            Assert.False(defaultMaterial.RainSplashesEnabled);
            Assert.False(defaultMaterial.Flammable);
            Assert.False(defaultMaterial.DontStickMissiles);
            Assert.Null(defaultMaterial.AttacksCanPassThrough);
            Assert.Null(defaultMaterial.OverrideMaterialNameForImpactSounds);

            // Assert - Wood material with boolean flags
            Assert.Equal("wood", woodMaterial.Id);
            Assert.True(woodMaterial.RainSplashesEnabled);
            Assert.True(woodMaterial.Flammable);
            Assert.False(woodMaterial.DontStickMissiles);

            // Assert - Wood nonstick with override material name
            Assert.Equal("wood_nonstick", woodNonstickMaterial.Id);
            Assert.Equal("wood", woodNonstickMaterial.OverrideMaterialNameForImpactSounds);
            Assert.True(woodNonstickMaterial.DontStickMissiles);
            Assert.True(woodNonstickMaterial.RainSplashesEnabled);
            Assert.True(woodNonstickMaterial.Flammable);

            // Assert - Pot material with attacks_can_pass_through = false
            Assert.Equal("pot", potMaterial.Id);
            Assert.True(potMaterial.DontStickMissiles);
            Assert.Equal("false", potMaterial.AttacksCanPassThrough);
        }

        [Fact]
        public void PhysicsMaterial_ShouldHandleOptionalBooleanAttributes()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            PhysicsMaterials physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterials)serializer.Deserialize(reader)!;
            }

            // Act
            var potMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "pot");
            var potBreakableMaterial = physicsMaterials.PhysicsMaterialList.First(x => x.Id == "pot_breakable");

            // Assert - pot has attacks_can_pass_through="false"
            Assert.NotNull(potMaterial.AttacksCanPassThrough);
            Assert.Equal("false", potMaterial.AttacksCanPassThrough);

            // Assert - pot_breakable has attacks_can_pass_through="true"
            Assert.NotNull(potBreakableMaterial.AttacksCanPassThrough);
            Assert.Equal("true", potBreakableMaterial.AttacksCanPassThrough);
        }

        [Fact]
        public void SoundAndCollisionInfoClassDefinitions_ShouldDeserializeCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "physics_materials.xml");
            
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            PhysicsMaterials physicsMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                physicsMaterials = (PhysicsMaterials)serializer.Deserialize(reader)!;
            }

            // Act
            var definitions = physicsMaterials.SoundAndCollisionInfoClassDefinitions;

            // Assert
            Assert.Equal(10, definitions.Count);
            Assert.Contains(definitions, d => d.Name == "human");
            Assert.Contains(definitions, d => d.Name == "horse");
            Assert.Contains(definitions, d => d.Name == "camel");
            Assert.Contains(definitions, d => d.Name == "ovine");
            Assert.Contains(definitions, d => d.Name == "boar");
            Assert.Contains(definitions, d => d.Name == "bovine");
            Assert.Contains(definitions, d => d.Name == "donkey");
            Assert.Contains(definitions, d => d.Name == "goose");
            Assert.Contains(definitions, d => d.Name == "chicken");
            Assert.Contains(definitions, d => d.Name == "dog");
        }

        [Fact]
        public void PhysicsMaterials_ShouldHandleNullVsEmptyString()
        {
            // Arrange
            var physicsMaterials = new PhysicsMaterials();
            var material = new PhysicsMaterial
            {
                Id = "test",
                StaticFriction = 1.0f,
                DynamicFriction = 0.5f,
                Restitution = 0.1f,
                LinearDamping = 0.05f,
                AngularDamping = 0.025f,
                OverrideMaterialNameForImpactSounds = null, // Explicitly null
                DisplayColor = null // Explicitly null
            };
            physicsMaterials.PhysicsMaterialList.Add(material);

            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            string serializedXml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, physicsMaterials);
                serializedXml = writer.ToString();
            }
            
            var deserialized = (PhysicsMaterials)serializer.Deserialize(new StringReader(serializedXml))!;

            // Assert
            var deserializedMaterial = deserialized.PhysicsMaterialList.First();
            Assert.Null(deserializedMaterial.OverrideMaterialNameForImpactSounds);
            Assert.Null(deserializedMaterial.DisplayColor);
            Assert.Null(deserializedMaterial.AttacksCanPassThrough);
        }

        [Fact]
        public void PhysicsMaterials_ShouldPreserveBooleanDefaults()
        {
            // Arrange
            var physicsMaterials = new PhysicsMaterials();
            var material = new PhysicsMaterial
            {
                Id = "test",
                StaticFriction = 1.0f,
                DynamicFriction = 0.5f,
                Restitution = 0.1f,
                LinearDamping = 0.05f,
                AngularDamping = 0.025f,
                // Default values should be preserved
                DontStickMissiles = false,
                RainSplashesEnabled = false,
                Flammable = false,
                Softness = 0.0f
            };
            physicsMaterials.PhysicsMaterialList.Add(material);

            // Act
            var serializer = new XmlSerializer(typeof(PhysicsMaterials));
            string serializedXml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, physicsMaterials);
                serializedXml = writer.ToString();
            }
            
            var deserialized = (PhysicsMaterials)serializer.Deserialize(new StringReader(serializedXml))!;

            // Assert
            var deserializedMaterial = deserialized.PhysicsMaterialList.First();
            Assert.False(deserializedMaterial.DontStickMissiles);
            Assert.False(deserializedMaterial.RainSplashesEnabled);
            Assert.False(deserializedMaterial.Flammable);
            Assert.Equal(0.0f, deserializedMaterial.Softness);
        }
    }
} 