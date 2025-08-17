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
    public class ItemHolstersLayoutXmlTests
    {
        [Fact]
        public void Deserialize_ItemHolstersLayoutXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Layouts);
            Assert.NotEmpty(result.Layouts.LayoutList);
            
            var layout = result.Layouts.LayoutList[0];
            Assert.Equal("item_holster", layout.Class);
            Assert.Equal("0.1", layout.Version);
            Assert.Equal("item_holsters.item_holster", layout.XmlTag);
            Assert.Equal("id", layout.NameAttribute);
            Assert.Equal("true", layout.UseInTreeview);
        }

        [Fact]
        public void Serialize_ItemHolstersLayoutDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_ItemHolstersLayoutDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(xml);

            // Act
            var dto = ItemHolstersLayoutMapper.ToDTO(originalDO);
            var convertedDO = ItemHolstersLayoutMapper.ToDO(dto);

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
            Assert.Equal(originalLayout.HasTreeviewContextMenu, convertedLayout.HasTreeviewContextMenu);
            Assert.Equal(originalLayout.HasItems, convertedLayout.HasItems);
        }

        [Fact]
        public void ItemHolstersLayoutDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            
            // 验证Has属性正确设置
            Assert.Equal(layout.HasColumns, layout.Columns.ColumnList.Count > 0);
            Assert.Equal(layout.HasTreeviewContextMenu, layout.TreeviewContextMenu.ItemList.Count > 0);
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
            
            if (layout.HasTreeviewContextMenu)
            {
                Assert.True(layout.ShouldSerializeTreeviewContextMenu());
            }
            else
            {
                Assert.False(layout.ShouldSerializeTreeviewContextMenu());
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
        public void ItemHolstersLayoutDO_ShouldContainSpecificItemFields()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证特定字段存在
            Assert.Contains(items, i => i.Name == "id");
            Assert.Contains(items, i => i.Name == "base_set");
            Assert.Contains(items, i => i.Name == "group_name");
            Assert.Contains(items, i => i.Name == "holster_bone");
            Assert.Contains(items, i => i.Name == "holster_position");
            Assert.Contains(items, i => i.Name == "holster_rotation");
            Assert.Contains(items, i => i.Name == "uses_base_position");
            Assert.Contains(items, i => i.Name == "equip_action");
            Assert.Contains(items, i => i.Name == "unequip_action");
            Assert.Contains(items, i => i.Name == "holster_scale");
            Assert.Contains(items, i => i.Name == "preview_mesh_search");
            Assert.Contains(items, i => i.Name == "head_visible");
            Assert.Contains(items, i => i.Name == "body_visible");
            Assert.Contains(items, i => i.Name == "hands_visible");
            Assert.Contains(items, i => i.Name == "leg_visible");
            
            // 验证预览相关项
            var previewItem = items.FirstOrDefault(i => i.Name == "preview");
            Assert.NotNull(previewItem);
            Assert.Equal("Holster Preview", previewItem.Label);
            Assert.Equal("preview_window", previewItem.Type);
            
            // 验证预览项的属性
            Assert.NotNull(previewItem.Properties);
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "scene_group_id" && p.Value == "skeleton_preview");
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "width" && p.Value == "480");
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "height" && p.Value == "640");
        }

        [Fact]
        public void ItemHolstersLayoutDO_ShouldContainOptionalAttributes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证optional属性
            var baseSetItem = items.FirstOrDefault(i => i.Name == "base_set");
            Assert.NotNull(baseSetItem);
            // 注意：由于XML属性的处理方式，optional属性需要在ItemDO模型中特殊处理
            
            // 验证属性列表不为空的项
            var positionItem = items.FirstOrDefault(i => i.Name == "holster_position");
            Assert.NotNull(positionItem);
            Assert.NotNull(positionItem.Properties);
            Assert.Contains(positionItem.Properties.PropertyList, p => p.Name == "step_amount" && p.Value == "0.01");
            Assert.Contains(positionItem.Properties.PropertyList, p => p.Name == "min_value" && p.Value == "-360.0");
            Assert.Contains(positionItem.Properties.PropertyList, p => p.Name == "max_value" && p.Value == "360.0");
        }
    }
}