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
    public class AnimationsLayoutXmlTests
    {
        [Fact]
        public void Deserialize_AnimationsLayoutXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Layouts);
            Assert.NotEmpty(result.Layouts.LayoutList);
            
            // 验证有一个layout元素
            Assert.Single(result.Layouts.LayoutList);
            
            // 验证layout (animation)
            var layout = result.Layouts.LayoutList[0];
            Assert.Equal("animation", layout.Class);
            Assert.Equal("0.1", layout.Version);
            Assert.Equal("animations.animation", layout.XmlTag);
            Assert.Equal("id", layout.NameAttribute);
            Assert.Equal("true", layout.UseInTreeview);
            
            // 验证columns
            Assert.NotNull(layout.Columns);
            Assert.Single(layout.Columns.ColumnList);
            Assert.Equal("0", layout.Columns.ColumnList[0].Id);
            Assert.Equal("500", layout.Columns.ColumnList[0].Width);
            
            // 验证items
            Assert.NotNull(layout.Items);
            Assert.NotEmpty(layout.Items.ItemList);
        }

        [Fact]
        public void Serialize_AnimationsLayoutDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<AnimationsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_AnimationsLayoutDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Act
            var dto = AnimationsLayoutMapper.ToDTO(originalDO);
            var convertedDO = AnimationsLayoutMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.Type, convertedDO.Type);
            Assert.Equal(originalDO.HasLayouts, convertedDO.HasLayouts);
            
            // 验证Layouts结构
            Assert.Equal(originalDO.Layouts.LayoutList.Count, convertedDO.Layouts.LayoutList.Count);
            
            // 验证layout
            var originalLayout = originalDO.Layouts.LayoutList[0];
            var convertedLayout = convertedDO.Layouts.LayoutList[0];
            
            Assert.Equal(originalLayout.Class, convertedLayout.Class);
            Assert.Equal(originalLayout.Version, convertedLayout.Version);
            Assert.Equal(originalLayout.XmlTag, convertedLayout.XmlTag);
            Assert.Equal(originalLayout.NameAttribute, convertedLayout.NameAttribute);
            Assert.Equal(originalLayout.UseInTreeview, convertedLayout.UseInTreeview);
        }

        [Fact]
        public void AnimationsLayoutDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            
            // 验证Has属性
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
        public void AnimationsLayoutDO_ShouldContainSpecificItemFields()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证特定字段存在
            Assert.Contains(items, i => i.Name == "preview");
            Assert.Contains(items, i => i.Name == "id");
            Assert.Contains(items, i => i.Name == "param1");
            Assert.Contains(items, i => i.Name == "duration");
            Assert.Contains(items, i => i.Name == "anim_data_name");
            Assert.Contains(items, i => i.Name == "source1");
            Assert.Contains(items, i => i.Name == "blend_in_period");
            Assert.Contains(items, i => i.Name == "flags");
            
            // 验证预览窗口
            var previewItem = items.FirstOrDefault(i => i.Name == "preview");
            Assert.NotNull(previewItem);
            Assert.Equal("preview_window", previewItem.Type);
            Assert.Equal("0", previewItem.Column);
            Assert.NotNull(previewItem.Properties);
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "scene_group_id" && p.Value == "skeleton_preview");
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "width" && p.Value == "400");
            Assert.Contains(previewItem.Properties.PropertyList, p => p.Name == "height" && p.Value == "400");
            
            // 验证flags项
            var flagsItem = items.FirstOrDefault(i => i.Name == "flags");
            Assert.NotNull(flagsItem);
            Assert.Equal("flag_set", flagsItem.Type);
            Assert.Equal("1", flagsItem.Column);
            Assert.NotNull(flagsItem.Properties);
            Assert.Contains(flagsItem.Properties.PropertyList, p => p.Name == "system_value" && p.Value == "animation");
        }

        [Fact]
        public void AnimationsLayoutDO_ShouldContainProperColumnConfiguration()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var columns = layout.Columns.ColumnList;
            
            // 验证列配置
            Assert.Single(columns);
            Assert.Equal("0", columns[0].Id);
            Assert.Equal("500", columns[0].Width);
            
            // 验证items分布在不同的列
            var items = layout.Items.ItemList;
            var column0Items = items.Where(i => i.Column == "0").ToList();
            var column1Items = items.Where(i => i.Column == "1").ToList();
            
            Assert.NotEmpty(column0Items);
            Assert.NotEmpty(column1Items);
            
            // 验证大多数项在column 0，只有flags在column 1
            Assert.Equal(8, column0Items.Count);
            Assert.Single(column1Items);
            Assert.Equal("flags", column1Items[0].Name);
        }

        [Fact]
        public void AnimationsLayoutDO_ShouldContainCorrectXmlPaths()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证xml_path属性
            var idItem = items.FirstOrDefault(i => i.Name == "id");
            Assert.NotNull(idItem);
            Assert.Equal("id", idItem.XmlPath);
            
            var param1Item = items.FirstOrDefault(i => i.Name == "param1");
            Assert.NotNull(param1Item);
            Assert.Equal("param1", param1Item.XmlPath);
            
            var durationItem = items.FirstOrDefault(i => i.Name == "duration");
            Assert.NotNull(durationItem);
            Assert.Equal("duration", durationItem.XmlPath);
            
            var animDataNameItem = items.FirstOrDefault(i => i.Name == "anim_data_name");
            Assert.NotNull(animDataNameItem);
            Assert.Equal("anim_data_name", animDataNameItem.XmlPath);
            
            // 验证预览窗口的xml_path为空
            var previewItem = items.FirstOrDefault(i => i.Name == "preview");
            Assert.NotNull(previewItem);
            Assert.Equal("", previewItem.XmlPath);
        }

        [Fact]
        public void AnimationsLayoutDO_ShouldHandleDuplicateSource1Names()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证有两个名为"source1"的项，但label不同
            var source1Items = items.Where(i => i.Name == "source1").ToList();
            Assert.Equal(2, source1Items.Count);
            
            // 验证第一个source1对应Start Frame
            var startFrameItem = source1Items.FirstOrDefault(i => i.Label == "Start Frame");
            Assert.NotNull(startFrameItem);
            Assert.Equal("source1", startFrameItem.XmlPath);
            
            // 验证第二个source1对应End Frame，但xml_path为source2
            var endFrameItem = source1Items.FirstOrDefault(i => i.Label == "End Frame");
            Assert.NotNull(endFrameItem);
            Assert.Equal("source2", endFrameItem.XmlPath);
        }

        [Fact]
        public void AnimationsLayoutDO_ShouldHaveCorrectLabels()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "animations_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<AnimationsLayoutDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var layout = result.Layouts.LayoutList[0];
            var items = layout.Items.ItemList;
            
            // 验证特定标签
            Assert.Contains(items, i => i.Name == "preview" && i.Label == "Preview Window");
            Assert.Contains(items, i => i.Name == "id" && i.Label == "ID");
            Assert.Contains(items, i => i.Name == "param1" && i.Label == "Param");
            Assert.Contains(items, i => i.Name == "duration" && i.Label == "Duration");
            Assert.Contains(items, i => i.Name == "anim_data_name" && i.Label == "Animation Data Name");
            Assert.Contains(items, i => i.Name == "blend_in_period" && i.Label == "Blend In Period");
            Assert.Contains(items, i => i.Name == "flags" && i.Label == "Flags");
        }
    }
}