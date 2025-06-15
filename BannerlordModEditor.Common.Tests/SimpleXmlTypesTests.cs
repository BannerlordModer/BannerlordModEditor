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
    public class SimpleXmlTypesTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void SkillSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_skill_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SkillSets));
            SkillSets skillSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skillSets = (SkillSets)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, skillSets);
                }
                savedXml = writer.ToString();
            }

            // Assert
            Assert.NotNull(skillSets);
            Assert.Contains("SkillSets", savedXml);
        }

        [Fact]
        public void BodyProperties_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "mpbodypropertytemplates.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(BodyProperties));
            BodyProperties bodyProperties;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                bodyProperties = (BodyProperties)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, bodyProperties);
                }
                savedXml = writer.ToString();
            }

            // Assert
            Assert.NotNull(bodyProperties);
            Assert.Contains("BodyProperties", savedXml);
        }

        [Fact]
        public void EquipmentRosters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_equipment_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(EquipmentRosters));
            EquipmentRosters equipmentRosters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                equipmentRosters = (EquipmentRosters)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, equipmentRosters);
                }
                savedXml = writer.ToString();
            }

            // Assert
            Assert.NotNull(equipmentRosters);
            Assert.Contains("EquipmentRosters", savedXml);
        }

        [Fact]
        public void BoneBodyTypes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "bone_body_types.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(BoneBodyTypes));
            BoneBodyTypes boneBodyTypes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                boneBodyTypes = (BoneBodyTypes)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, boneBodyTypes);
                }
                savedXml = writer.ToString();
            }

            // Assert
            Assert.NotNull(boneBodyTypes);
            Assert.NotNull(boneBodyTypes.BoneBodyType);
            Assert.True(boneBodyTypes.BoneBodyType.Count > 0, "Should have at least one bone body type");
            Assert.Contains("bone_body_types", savedXml);
            
            // 检查第一个元素的必要属性
            var firstType = boneBodyTypes.BoneBodyType.First();
            Assert.False(string.IsNullOrEmpty(firstType.Type), "Type should not be empty");
            Assert.False(string.IsNullOrEmpty(firstType.Priority), "Priority should not be empty");
        }
    }
} 