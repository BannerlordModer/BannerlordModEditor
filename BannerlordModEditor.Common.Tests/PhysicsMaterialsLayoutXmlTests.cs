using System;
using System.IO;
using System.Xml;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class PhysicsMaterialsLayoutXmlTests
    {
        [Fact]
        public void Deserialize_PhysicsMaterialsLayoutXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Layouts);
            Assert.NotEmpty(result.Layouts.LayoutList);
            
            var layout = result.Layouts.LayoutList[0];
            Assert.Equal("physics_material", layout.Class);
            Assert.Equal("0.1", layout.Version);
            Assert.Equal("physics_materials.physics_material", layout.XmlTag);
            Assert.Equal("id", layout.NameAttribute);
            Assert.Equal("true", layout.UseInTreeview);
        }

        [Fact]
        public void Serialize_PhysicsMaterialsLayoutDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_PhysicsMaterialsLayoutDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Act
            var dto = PhysicsMaterialsLayoutMapper.ToDTO(originalDO);
            var convertedDO = PhysicsMaterialsLayoutMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.Type, convertedDO.Type);
            Assert.Equal(originalDO.HasLayouts, convertedDO.HasLayouts);
            
            // 验证Layouts结构
            Assert.Equal(originalDO.Layouts.LayoutList.Count, convertedDO.Layouts.LayoutList.Count);
            
            var originalLayout = originalDO.Layouts.LayoutList[0];
            var convertedLayout = convertedDO.Layouts.LayoutList[0];
            
            Assert.Equal(originalLayout.Class, convertedLayout.Class);
            Assert.Equal(originalLayout.Version, convertedLayout.Version);
            Assert.Equal(originalLayout.XmlTag, convertedLayout.XmlTag);
            Assert.Equal(originalLayout.NameAttribute, convertedLayout.NameAttribute);
            Assert.Equal(originalLayout.UseInTreeview, convertedLayout.UseInTreeview);
            
            // 验证子元素状态
            Assert.Equal(originalLayout.HasColumns, convertedLayout.HasColumns);
            Assert.Equal(originalLayout.HasItems, convertedLayout.HasItems);
        }

        [Fact]
        public void PhysicsMaterialsLayoutDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            
            // 验证Has属性正确设置
            Assert.Equal(layout.HasColumns, layout.Columns.ColumnList.Count > 0);
            Assert.Equal(layout.HasItems, layout.Items.ItemList.Count > 0);
            
            // 验证ShouldSerialize方法
            if (layout.HasColumns)
            {
                Assert.True(layout.ShouldSerializeColumns());
            }
            else
            {
                Assert.False(layout.ShouldSerializeColumns());
            }
            
            if (layout.HasItems)
            {
                Assert.True(layout.ShouldSerializeItems());
            }
            else
            {
                Assert.False(layout.ShouldSerializeItems());
            }
        }

        [Fact]
        public void PhysicsMaterialsLayoutDO_ShouldContainSpecificItemFields()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证特定字段存在
            Assert.Contains(items, i => i.Name == "id");
            Assert.Contains(items, i => i.Name == "static_friction");
            Assert.Contains(items, i => i.Name == "dynamic_friction");
            Assert.Contains(items, i => i.Name == "restitution");
            Assert.Contains(items, i => i.Name == "softness");
            Assert.Contains(items, i => i.Name == "linear_damping");
            Assert.Contains(items, i => i.Name == "angular_damping");
            Assert.Contains(items, i => i.Name == "dont_stick_missiles");
            Assert.Contains(items, i => i.Name == "flammable");
            
            // 验证float类型的属性包含step_amount属性
            var staticFrictionItem = items.FirstOrDefault(i => i.Name == "static_friction");
            Assert.NotNull(staticFrictionItem);
            Assert.Equal("float", staticFrictionItem.Type);
            Assert.NotNull(staticFrictionItem.Properties);
            Assert.Contains(staticFrictionItem.Properties.PropertyList, p => p.Name == "step_amount" && p.Value == "0.05");
            
            // 验证checkbox类型的属性没有properties
            var dontStickMissilesItem = items.FirstOrDefault(i => i.Name == "dont_stick_missiles");
            Assert.NotNull(dontStickMissilesItem);
            Assert.Equal("checkbox", dontStickMissilesItem.Type);
            Assert.Equal(0, dontStickMissilesItem.Properties.PropertyList.Count);
        }

        [Fact]
        public void PhysicsMaterialsLayoutDO_ShouldContainProperColumnConfiguration()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var columns = layout.Columns.ColumnList;
            
            // 验证列配置
            Assert.Single(columns);
            Assert.Equal("0", columns[0].Id);
            Assert.Equal("200", columns[0].Width);
            
            // 验证所有item都在column 0
            var items = layout.Items.ItemList;
            foreach (var item in items)
            {
                Assert.Equal("0", item.Column);
            }
        }

        [Fact]
        public void PhysicsMaterialsLayoutDO_ShouldContainCorrectXmlPaths()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "physics_materials_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PhysicsMaterialsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证xml_path设置正确
            var idItem = items.FirstOrDefault(i => i.Name == "id");
            Assert.NotNull(idItem);
            Assert.Equal("id", idItem.XmlPath);
            
            var staticFrictionItem = items.FirstOrDefault(i => i.Name == "static_friction");
            Assert.NotNull(staticFrictionItem);
            Assert.Equal("static_friction", staticFrictionItem.XmlPath);
            
            var flammableItem = items.FirstOrDefault(i => i.Name == "flammable");
            Assert.NotNull(flammableItem);
            Assert.Equal("flammable", flammableItem.XmlPath);
        }
    }
}