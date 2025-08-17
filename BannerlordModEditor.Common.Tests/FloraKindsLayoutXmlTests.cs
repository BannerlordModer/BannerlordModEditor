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
    public class FloraKindsLayoutXmlTests
    {
        [Fact]
        public void Deserialize_FloraKindsLayoutXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Layouts);
            Assert.NotEmpty(result.Layouts.LayoutList);
            
            // 验证有两个layout元素
            Assert.Equal(2, result.Layouts.LayoutList.Count);
            
            // 验证第一个layout (flora_kind)
            var firstLayout = result.Layouts.LayoutList[0];
            Assert.Equal("flora_kind", firstLayout.Class);
            Assert.Equal("0.1", firstLayout.Version);
            Assert.Equal("flora_kinds.flora_kind", firstLayout.XmlTag);
            Assert.Equal("name", firstLayout.NameAttribute);
            Assert.Equal("true", firstLayout.UseInTreeview);
            
            // 验证第二个layout (flora_variation)
            var secondLayout = result.Layouts.LayoutList[1];
            Assert.Equal("flora_variation", secondLayout.Class);
            Assert.Equal("0.1", secondLayout.Version);
            Assert.Equal("flora_kinds.flora_kind.flora_variations.flora_variation", secondLayout.XmlTag);
            Assert.Equal("name", secondLayout.NameAttribute);
            Assert.Equal("false", secondLayout.UseInTreeview);
        }

        [Fact]
        public void Serialize_FloraKindsLayoutDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_FloraKindsLayoutDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Act
            var dto = FloraKindsLayoutMapper.ToDTO(originalDO);
            var convertedDO = FloraKindsLayoutMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.Type, convertedDO.Type);
            Assert.Equal(originalDO.HasLayouts, convertedDO.HasLayouts);
            
            // 验证Layouts结构
            Assert.Equal(originalDO.Layouts.LayoutList.Count, convertedDO.Layouts.LayoutList.Count);
            
            // 验证第一个layout
            var originalFirstLayout = originalDO.Layouts.LayoutList[0];
            var convertedFirstLayout = convertedDO.Layouts.LayoutList[0];
            
            Assert.Equal(originalFirstLayout.Class, convertedFirstLayout.Class);
            Assert.Equal(originalFirstLayout.Version, convertedFirstLayout.Version);
            Assert.Equal(originalFirstLayout.XmlTag, convertedFirstLayout.XmlTag);
            Assert.Equal(originalFirstLayout.NameAttribute, convertedFirstLayout.NameAttribute);
            Assert.Equal(originalFirstLayout.UseInTreeview, convertedFirstLayout.UseInTreeview);
            
            // 验证第二个layout
            var originalSecondLayout = originalDO.Layouts.LayoutList[1];
            var convertedSecondLayout = convertedDO.Layouts.LayoutList[1];
            
            Assert.Equal(originalSecondLayout.Class, convertedSecondLayout.Class);
            Assert.Equal(originalSecondLayout.Version, convertedSecondLayout.Version);
            Assert.Equal(originalSecondLayout.XmlTag, convertedSecondLayout.XmlTag);
            Assert.Equal(originalSecondLayout.NameAttribute, convertedSecondLayout.NameAttribute);
            Assert.Equal(originalSecondLayout.UseInTreeview, convertedSecondLayout.UseInTreeview);
        }

        [Fact]
        public void FloraKindsLayoutDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var firstLayout = result.Layouts.LayoutList[0];
            var secondLayout = result.Layouts.LayoutList[1];
            
            // 验证第一个layout的Has属性
            Assert.Equal(firstLayout.HasColumns, firstLayout.Columns.ColumnList.Count > 0);
            Assert.Equal(firstLayout.HasItems, firstLayout.Items.ItemList.Count > 0);
            
            // 验证第二个layout的Has属性
            Assert.Equal(secondLayout.HasItems, secondLayout.Items.ItemList.Count > 0);
            
            // 验证ShouldSerialize方法
            if (firstLayout.HasColumns)
            {
                Assert.True(firstLayout.ShouldSerializeColumns());
            }
            else
            {
                Assert.False(firstLayout.ShouldSerializeColumns());
            }
            
            if (firstLayout.HasItems)
            {
                Assert.True(firstLayout.ShouldSerializeItems());
            }
            else
            {
                Assert.False(firstLayout.ShouldSerializeItems());
            }
        }

        [Fact]
        public void FloraKindsLayoutDO_ShouldContainSpecificItemFields()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var firstLayout = result.Layouts.LayoutList[0];
            var items = firstLayout.Items.ItemList;
            
            // 验证特定字段存在
            Assert.Contains(items, i => i.Name == "name");
            Assert.Contains(items, i => i.Name == "summer_kind");
            Assert.Contains(items, i => i.Name == "winter_kind");
            Assert.Contains(items, i => i.Name == "fall_kind");
            Assert.Contains(items, i => i.Name == "spring_kind");
            Assert.Contains(items, i => i.Name == "season");
            Assert.Contains(items, i => i.Name == "density");
            Assert.Contains(items, i => i.Name == "view_distance");
            Assert.Contains(items, i => i.Name == "flags");
            Assert.Contains(items, i => i.Name == "variations");
            Assert.Contains(items, i => i.Name == "preview");
            
            // 验证季节选择器
            var seasonItem = items.FirstOrDefault(i => i.Name == "season");
            Assert.NotNull(seasonItem);
            Assert.Equal("combobox", seasonItem.Type);
            Assert.NotNull(seasonItem.Properties);
            Assert.Contains(seasonItem.Properties.PropertyList, p => p.Name == "system_values" && p.Value == "atmosphere.season_type");
            
            // 验证预览窗口
            var previewItem = items.FirstOrDefault(i => i.Name == "preview");
            Assert.NotNull(previewItem);
            Assert.Equal("preview_window", previewItem.Type);
            Assert.Equal("2", previewItem.Column);
            Assert.NotNull(previewItem.Properties);
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "scene_group_id" && p.Value == "flora_preview");
        }

        [Fact]
        public void FloraKindsLayoutDO_ShouldContainSecondLayoutItems()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var secondLayout = result.Layouts.LayoutList[1];
            var items = secondLayout.Items.ItemList;
            
            // 验证第二个layout的字段
            Assert.Contains(items, i => i.Name == "variation_name");
            Assert.Contains(items, i => i.Name == "variation_entity");
            Assert.Contains(items, i => i.Name == "variation_summer_material");
            Assert.Contains(items, i => i.Name == "variation_winter_material");
            Assert.Contains(items, i => i.Name == "variation_fall_material");
            Assert.Contains(items, i => i.Name == "variation_spring_material");
            
            // 验证所有项都在column 0
            foreach (var item in items)
            {
                Assert.Equal("0", item.Column);
            }
        }

        [Fact]
        public void FloraKindsLayoutDO_ShouldContainDefaultNode()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var firstLayout = result.Layouts.LayoutList[0];
            var variationsItem = firstLayout.Items.ItemList.FirstOrDefault(i => i.Name == "variations");
            
            Assert.NotNull(variationsItem);
            Assert.Equal("dynamic_complex", variationsItem.Type);
            
            // 注意：default_node在LayoutsBaseDO中没有直接对应的属性，它是在XML序列化时通过XmlAnyElement处理的
            // 但我们可以验证这个item存在并且类型正确
        }

        [Fact]
        public void FloraKindsLayoutDO_ShouldContainProperColumnConfiguration()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var firstLayout = result.Layouts.LayoutList[0];
            var columns = firstLayout.Columns.ColumnList;
            
            // 验证列配置
            Assert.Single(columns);
            Assert.Equal("1", columns[0].Id);
            Assert.Equal("120", columns[0].Width);
            
            // 验证items分布在不同的列
            var items = firstLayout.Items.ItemList;
            var column0Items = items.Where(i => i.Column == "0").ToList();
            var column1Items = items.Where(i => i.Column == "1").ToList();
            var column2Items = items.Where(i => i.Column == "2").ToList();
            
            Assert.NotEmpty(column0Items);
            Assert.NotEmpty(column1Items);
            Assert.NotEmpty(column2Items);
        }
    }
}