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
    public class SkeletonsLayoutXmlTests
    {
        [Fact]
        public void Deserialize_SkeletonsLayoutXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Layouts);
            Assert.NotEmpty(result.Layouts.LayoutList);
            
            var layout = result.Layouts.LayoutList[0];
            Assert.Equal("skeleton", layout.Class);
            Assert.Equal("0.1", layout.Version);
            Assert.Equal("skeletons.skeleton", layout.XmlTag);
            Assert.Equal("name", layout.NameAttribute);
            Assert.Equal("true", layout.UseInTreeview);
        }

        [Fact]
        public void Serialize_SkeletonsLayoutDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_SkeletonsLayoutDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(xml);

            // Act
            var dto = SkeletonsLayoutMapper.ToDTO(originalDO);
            var convertedDO = SkeletonsLayoutMapper.ToDO(dto);

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
            Assert.Equal(originalLayout.HasInsertionDefinitions, convertedLayout.HasInsertionDefinitions);
            Assert.Equal(originalLayout.HasTreeviewContextMenu, convertedLayout.HasTreeviewContextMenu);
            Assert.Equal(originalLayout.HasItems, convertedLayout.HasItems);
        }

        [Fact]
        public void SkeletonsLayoutDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            
            // 验证Has属性正确设置
            Assert.Equal(layout.HasColumns, layout.Columns.ColumnList.Count > 0);
            Assert.Equal(layout.HasInsertionDefinitions, layout.InsertionDefinitions.InsertionDefinitionList.Count > 0);
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
            
            if (layout.HasInsertionDefinitions)
            {
                Assert.True(layout.ShouldSerializeInsertionDefinitions());
            }
            else
            {
                Assert.False(layout.ShouldSerializeInsertionDefinitions());
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
    }
}