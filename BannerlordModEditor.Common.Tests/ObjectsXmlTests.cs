using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class ObjectsXmlTests
    {
        [Fact]
        public void Deserialize_ObjectsXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "objects.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ObjectsDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Item);
            Assert.NotNull(result.Item.Object);
            Assert.NotEmpty(result.Item.Object);
            
            // 验证第一个object
            var firstObject = result.Item.Object[0];
            Assert.Equal("itm_practice_sword", firstObject.ItemKind);
            Assert.Equal("1", firstObject.Id);
            Assert.Equal("Sword", firstObject.Name);
            
            // 验证attributes
            Assert.NotNull(firstObject.Attributes);
            Assert.NotNull(firstObject.Attributes.Attribute);
            Assert.NotEmpty(firstObject.Attributes.Attribute);
            
            var firstAttribute = firstObject.Attributes.Attribute[0];
            Assert.Equal("WeaponSwingDamage", firstAttribute.Code);
            Assert.Equal("30", firstAttribute.Value);
        }

        [Fact]
        public void Serialize_ObjectsDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "objects.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<ObjectsDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert - 显示差异信息
            var areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            if (!areEqual)
            {
                Console.WriteLine("=== 原始XML ===");
                Console.WriteLine(originalXml);
                Console.WriteLine("=== 序列化XML ===");
                Console.WriteLine(serializedXml);
            }
            Assert.True(areEqual);
        }

        [Fact]
        public void Mapper_ObjectsDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "objects.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<ObjectsDO>(xml);

            // Act
            var dto = ObjectsMapper.ToDTO(originalDO);
            var convertedDO = ObjectsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.HasItem, convertedDO.HasItem);
            
            // 验证Item结构
            Assert.Equal(originalDO.Item.Object.Count, convertedDO.Item.Object.Count);
            
            // 验证第一个object
            var originalObject = originalDO.Item.Object[0];
            var convertedObject = convertedDO.Item.Object[0];
            
            Assert.Equal(originalObject.ItemKind, convertedObject.ItemKind);
            Assert.Equal(originalObject.Id, convertedObject.Id);
            Assert.Equal(originalObject.Name, convertedObject.Name);
            
            // 验证attributes
            Assert.Equal(originalObject.Attributes.Attribute.Count, convertedObject.Attributes.Attribute.Count);
            
            var originalAttribute = originalObject.Attributes.Attribute[0];
            var convertedAttribute = convertedObject.Attributes.Attribute[0];
            
            Assert.Equal(originalAttribute.Code, convertedAttribute.Code);
            Assert.Equal(originalAttribute.Value, convertedAttribute.Value);
        }

        [Fact]
        public void ObjectsDO_ShouldContainSpecificObjects()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "objects.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ObjectsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var objects = result.Item.Object;
            
            // 验证特定的object存在
            Assert.Contains(objects, o => o.Id == "1" && o.Name == "Sword");
            Assert.Contains(objects, o => o.Id == "2" && o.Name == "Spear");
            Assert.Contains(objects, o => o.Id == "3" && o.Name == "Shield");
            
            // 验证特定object的属性
            var sword = objects.FirstOrDefault(o => o.Id == "1");
            Assert.NotNull(sword);
            Assert.Equal("itm_practice_sword", sword.ItemKind);
            Assert.Equal("Sword", sword.Name);
            
            // 验证attributes
            Assert.NotNull(sword.Attributes);
            Assert.Contains(sword.Attributes.Attribute, a => a.Code == "WeaponSwingDamage" && a.Value == "30");
            Assert.Contains(sword.Attributes.Attribute, a => a.Code == "WeaponThrustDamage" && a.Value == "22");
            Assert.Contains(sword.Attributes.Attribute, a => a.Code == "WeaponLength" && a.Value == "20");
        }
    }
}