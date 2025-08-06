using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class EnhancedXmlLoaderLogicalEquivalenceTests
    {
        [Fact]
        public void EnhancedXmlLoader_WithActionTypes_ShouldLoadAndSave()
        {
            // Arrange - 创建一个原始XML
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""act_smithing_machine_anvil_part_1"" />
	<action name=""act_jump"" type=""actt_jump_start"" />
	<action name=""act_jump_left_stance"" type=""actt_jump_start"" usage_direction=""left"" />
	<action name=""act_jump_loop"" type="""" />
</action_types>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, originalXml, Encoding.UTF8);

            // Act - 加载、保存
            var loader = new EnhancedXmlLoader<EnhancedActionTypesList>();
            var model = loader.Load(tempFile);
            
            Assert.NotNull(model);

            var savedFile = Path.GetTempFileName();
            loader.Save(model, savedFile);
            
            // 读取保存的XML
            var savedXmlContent = File.ReadAllText(savedFile, Encoding.UTF8);

            // Assert - 验证基本功能
            Assert.Equal(4, model.Actions.Count);
            
            var firstAction = model.Actions[0];
            Assert.Equal("act_smithing_machine_anvil_part_1", firstAction.Name);

            var secondAction = model.Actions[1];
            Assert.Equal("act_jump", secondAction.Name);
            Assert.Equal("actt_jump_start", secondAction.Type);

            var thirdAction = model.Actions[2];
            Assert.Equal("act_jump_left_stance", thirdAction.Name);
            Assert.Equal("actt_jump_start", thirdAction.Type);
            Assert.Equal("left", thirdAction.UsageDirection);

            var fourthAction = model.Actions[3];
            Assert.Equal("act_jump_loop", fourthAction.Name);
            Assert.Equal("", fourthAction.Type);

            // 清理
            File.Delete(tempFile);
            File.Delete(savedFile);
        }

        [Fact]
        public void EnhancedXmlLoader_WithFloraKinds_ShouldLoadAndSave()
        {
            // Arrange - 创建一个原始的FloraKinds XML
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<flora_kinds>
	<flora_kind name=""flora_grass_a_visualtest"" view_distance=""150.000"">
		<flags>
			<flag name=""align_with_ground"" value=""true"" />
			<flag name=""is_grass"" value="""" />
		</flags>
		<seasonal_kind season=""summer"">
			<flora_variations>
				<flora_variation name=""flora_grass_a_big_visualtest"" />
			</flora_variations>
		</seasonal_kind>
	</flora_kind>
	<flora_kind name=""flora_grass_b_simple"">
	</flora_kind>
</flora_kinds>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, originalXml, Encoding.UTF8);

            // Act - 加载、保存
            var loader = new EnhancedXmlLoader<EnhancedFloraKinds>();
            var model = loader.Load(tempFile);
            
            Assert.NotNull(model);

            var savedFile = Path.GetTempFileName();
            loader.Save(model, savedFile);
            
            // 读取保存的XML
            var savedXmlContent = File.ReadAllText(savedFile, Encoding.UTF8);

            // Assert - 验证基本功能
            Assert.NotNull(model.FloraKind);
            Assert.Equal(2, model.FloraKind.Count);
            
            var firstFlora = model.FloraKind[0];
            Assert.Equal("flora_grass_a_visualtest", firstFlora.Name);
            Assert.Equal("150.000", firstFlora.ViewDistance);

            // 验证flags
            Assert.NotNull(firstFlora.Flags);
            Assert.Equal(2, firstFlora.Flags.Flag.Count);
            
            var firstFlag = firstFlora.Flags.Flag[0];
            Assert.Equal("align_with_ground", firstFlag.Name);
            Assert.Equal("true", firstFlag.Value);

            var secondFlag = firstFlora.Flags.Flag[1];
            Assert.Equal("is_grass", secondFlag.Name);
            Assert.Equal("", secondFlag.Value); // 空字符串

            // 验证seasonal_kind
            Assert.Single(firstFlora.SeasonalKind);
            var seasonalKind = firstFlora.SeasonalKind[0];
            Assert.Equal("summer", seasonalKind.Season);

            // 验证flora_variations
            Assert.NotNull(seasonalKind.FloraVariations);
            Assert.Single(seasonalKind.FloraVariations.FloraVariation);
            
            var variation = seasonalKind.FloraVariations.FloraVariation[0];
            Assert.Equal("flora_grass_a_big_visualtest", variation.Name);

            var secondFlora = model.FloraKind[1];
            Assert.Equal("flora_grass_b_simple", secondFlora.Name);

            // 清理
            File.Delete(tempFile);
            File.Delete(savedFile);
        }

        [Fact]
        public void EnhancedXmlLoader_AreXmlElementsLogicallyEqual_ShouldWorkWithEnhancedXmlLoader()
        {
            // Arrange
            var loader = new EnhancedXmlLoader<EnhancedActionTypesList>();
            
            // 测试EnhancedXmlLoader中的AreXmlDocumentsLogicallyEquivalent方法
            var xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""test1"" type=""type1"" />
	<action name=""test2"" usage_direction=""dir2"" />
</action_types>";

            var xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""test1"" type=""type1"" />
	<action name=""test2"" usage_direction=""dir2"" />
</action_types>";

            var xml3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""test1"" type=""type1"" />
	<action name=""test2"" usage_direction=""different"" />
</action_types>";

            // Act & Assert
            var doc1 = XDocument.Parse(xml1);
            var doc2 = XDocument.Parse(xml2);
            var doc3 = XDocument.Parse(xml3);

            // 相同的文档应该等价
            Assert.True(EnhancedXmlLoader<EnhancedActionTypesList>.AreXmlDocumentsLogicallyEquivalent(doc1, doc2));

            // 不同的文档不应该等价
            Assert.False(EnhancedXmlLoader<EnhancedActionTypesList>.AreXmlDocumentsLogicallyEquivalent(doc1, doc3));
        }

        /// <summary>
        /// 复用MpItemsSubsetTests.cs中的逻辑等价性验证方法
        /// </summary>
        private static bool AreXmlElementsLogicallyEqual(XElement? original, XElement? generated)
        {
            if (original == null && generated == null) return true;
            if (original == null || generated == null) return false;

            // 比较元素名称
            if (original.Name != generated.Name) return false;

            // 比较属性（忽略顺序）
            var originalAttrs = original.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
            var generatedAttrs = generated.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);

            if (originalAttrs.Count != generatedAttrs.Count) return false;

            foreach (var attr in originalAttrs)
            {
                if (!generatedAttrs.TryGetValue(attr.Key, out var generatedValue))
                    return false;
                
                // 对于数值类型，进行宽松比较（例如 1.0 == 1）
                if (IsNumericValue(attr.Value, generatedValue))
                {
                    if (!AreNumericValuesEqual(attr.Value, generatedValue))
                        return false;
                }
                else if (attr.Value != generatedValue)
                {
                    return false;
                }
            }

            // 比较子元素（保持顺序，因为这对于列表类很重要）
            var originalChildren = original.Elements().ToList();
            var generatedChildren = generated.Elements().ToList();

            if (originalChildren.Count != generatedChildren.Count) return false;

            for (int i = 0; i < originalChildren.Count; i++)
            {
                if (!AreXmlElementsLogicallyEqual(originalChildren[i], generatedChildren[i]))
                    return false;
            }

            // 比较文本内容
            return original.Value == generated.Value;
        }

        private static bool IsNumericValue(string value1, string value2)
        {
            return (double.TryParse(value1, out _) && double.TryParse(value2, out _)) ||
                   (int.TryParse(value1, out _) && int.TryParse(value2, out _));
        }

        private static bool AreNumericValuesEqual(string value1, string value2)
        {
            if (double.TryParse(value1, out var d1) && double.TryParse(value2, out var d2))
            {
                return Math.Abs(d1 - d2) < 0.0001; // 允许小的浮点误差
            }
            return value1 == value2;
        }
    }
}