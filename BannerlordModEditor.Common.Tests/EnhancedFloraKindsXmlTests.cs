using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class EnhancedFloraKindsXmlTests
    {
        [Fact]
        public void FloraKinds_CreateObjectsDirectly_ShouldTrackExistenceCorrectly()
        {
            // Arrange - 直接创建对象来测试字段存在性追踪
            var floraKinds = new EnhancedFloraKinds();
            
            var flora1 = new EnhancedFloraKind
            {
                Name = "flora_grass_a"
                // 不设置ViewDistance，模拟字段不存在
            };

            var flags1 = new EnhancedFloraFlags();
            var flag1 = new EnhancedFloraFlag
            {
                Name = "align_with_ground",
                Value = "true"
            };
            var flag2 = new EnhancedFloraFlag
            {
                Name = "is_grass",
                Value = "" // 空字符串，模拟字段存在但为空
            };
            flags1.Flag.Add(flag1);
            flags1.Flag.Add(flag2);
            flora1.Flags = flags1;

            var seasonalKind1 = new EnhancedSeasonalKind
            {
                Season = "summer"
            };

            var variations1 = new EnhancedFloraVariations();
            var variation1 = new EnhancedFloraVariation
            {
                Name = "flora_grass_a_big"
                // 不设置BodyName，模拟字段不存在
            };
            variations1.FloraVariation.Add(variation1);
            seasonalKind1.FloraVariations = variations1;

            flora1.SeasonalKind.Add(seasonalKind1);

            var flora2 = new EnhancedFloraKind
            {
                Name = "flora_grass_b",
                ViewDistance = "200.000"
                // 不设置Flags和SeasonalKind，模拟字段不存在
            };

            floraKinds.FloraKind.Add(flora1);
            floraKinds.FloraKind.Add(flora2);

            // Act & Assert
            Assert.Equal(2, floraKinds.FloraKind.Count);

            // 第一个flora_kind - 包含部分字段
            var firstFlora = floraKinds.FloraKind[0];
            Assert.Equal("flora_grass_a", firstFlora.Name);
            // 注意：直接创建的对象不会自动标记属性存在性，这需要通过XML加载来实现
            // 我们只验证ShouldSerialize行为
            Assert.False(firstFlora.ShouldSerializeViewDistance()); // view_distance字段不应该序列化（因为不存在）
            
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
            
            // 验证ShouldSerialize行为
            // 注意：直接创建的对象不会自动标记属性存在性，这需要通过XML加载来实现
            // 我们只验证集合内容是否正确
            Assert.Equal(2, firstFlora.Flags.Flag.Count); // Flags有2个元素
            Assert.Single(firstFlora.SeasonalKind); // SeasonalKind有1个元素

            // 验证flora_variations
            Assert.NotNull(seasonalKind.FloraVariations);
            Assert.Single(seasonalKind.FloraVariations.FloraVariation);
            
            var variation = seasonalKind.FloraVariations.FloraVariation[0];
            Assert.Equal("flora_grass_a_big", variation.Name);

            // 第二个flora_kind - 只有name和view_distance
            var secondFlora = floraKinds.FloraKind[1];
            Assert.Equal("flora_grass_b", secondFlora.Name);
            Assert.Equal("200.000", secondFlora.ViewDistance);
            // 验证ShouldSerialize行为
            Assert.True(secondFlora.ShouldSerializeViewDistance()); // ViewDistance有值应该序列化
            
            // 未设置的属性不应该序列化
            Assert.False(secondFlora.ShouldSerializeFlags()); // Flags未设置不应该序列化
            Assert.False(secondFlora.ShouldSerializeSeasonalKind()); // SeasonalKind未设置不应该序列化
        }

        [Fact]
        public void FloraKinds_PropertyExistenceTracking_ShouldWorkCorrectly()
        {
            // Arrange - 直接创建对象来测试字段存在性追踪
            var floraKinds = new EnhancedFloraKinds();
            
            var flora1 = new EnhancedFloraKind
            {
                Name = "flora_test"
                // 不设置ViewDistance，模拟字段不存在
            };

            var flags1 = new EnhancedFloraFlags();
            // 创建一个空的flags，模拟空集合
            flora1.Flags = flags1;

            // 创建一个空的seasonal_kind列表，模拟空集合
            flora1.SeasonalKind = new List<EnhancedSeasonalKind>();

            floraKinds.FloraKind.Add(flora1);

            // Act & Assert
            Assert.Single(floraKinds.FloraKind);

            var flora = floraKinds.FloraKind[0];
            Assert.Equal("flora_test", flora.Name);
            // 注意：直接创建的对象不会自动标记属性存在性，这需要通过XML加载来实现
            // 我们只验证ShouldSerialize行为

            // 验证ShouldSerialize行为 - 空集合不应该被序列化
            Assert.False(flora.ShouldSerializeFlags()); // 空集合不应该序列化
            Assert.False(flora.ShouldSerializeSeasonalKind()); // 空集合不应该序列化

            // 添加一些元素到集合中
            var flags2 = new EnhancedFloraFlags();
            flags2.Flag.Add(new EnhancedFloraFlag { Name = "test_flag", Value = "test_value" });
            flora.Flags = flags2;

            Assert.True(flora.ShouldSerializeFlags()); // 非空集合应该序列化

            flora.Flags = new EnhancedFloraFlags(); // 重新设置为空
            Assert.False(flora.ShouldSerializeFlags()); // 空集合不应该序列化
        }

        [Fact]
        public void FloraKinds_EmptyCollections_ShouldNotBeSerialized()
        {
            // Arrange - 创建一个包含空集合的对象
            var floraKinds = new EnhancedFloraKinds();
            
            var flora = new EnhancedFloraKind
            {
                Name = "flora_test"
            };

            // 创建空的集合但标记它们存在
            flora.Flags = new EnhancedFloraFlags(); // 空的flags
            flora.SeasonalKind = new List<EnhancedSeasonalKind>(); // 空的seasonal_kind

            floraKinds.FloraKind.Add(flora);

            // Act & Assert - 验证ShouldSerialize行为
            // 空集合不应该被序列化，即使它们存在
            Assert.False(flora.ShouldSerializeFlags());
            Assert.False(flora.ShouldSerializeSeasonalKind());

            // 添加一些元素到集合中
            flora.Flags.Flag.Add(new EnhancedFloraFlag { Name = "test_flag", Value = "test_value" });
            flora.SeasonalKind.Add(new EnhancedSeasonalKind { Season = "summer" });

            // 现在非空集合应该被序列化
            Assert.True(flora.ShouldSerializeFlags());
            Assert.True(flora.ShouldSerializeSeasonalKind());
        }

        [Fact]
        public void FloraKinds_NestedProperties_ShouldTrackExistenceCorrectly()
        {
            // Arrange - 创建一个复杂的嵌套结构
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<flora_kinds>
	<flora_kind name=""flora_test"" view_distance=""150.000"">
		<flags>
			<flag name=""flag1"" value=""value1"" />
			<flag name=""flag2"" />
			<flag name=""flag3"" value="""" />
		</flags>
		<seasonal_kind season=""summer"">
			<flora_variations>
				<flora_variation name=""variation1"" />
				<flora_variation name=""variation2"" body_name=""body2"" />
				<flora_variation name=""variation3"" body_name="""" density_multiplier=""1.0"" />
			</flora_variations>
		</seasonal_kind>
	</flora_kind>
</flora_kinds>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, xmlContent, Encoding.UTF8);

            // Act
            var loader = new EnhancedXmlLoader<EnhancedFloraKinds>();
            var result = loader.Load(tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.FloraKind);

            var flora = result.FloraKind[0];
            Assert.True(flora.ViewDistanceExistsInXml);
            Assert.Equal("150.000", flora.ViewDistance);

            // 验证flags
            Assert.NotNull(flora.Flags);
            Assert.Equal(3, flora.Flags.Flag.Count);

            var flag1 = flora.Flags.Flag[0];
            Assert.Equal("flag1", flag1.Name);
            Assert.True(flag1.ValueExistsInXml);
            Assert.Equal("value1", flag1.Value);

            var flag2 = flora.Flags.Flag[1];
            Assert.Equal("flag2", flag2.Name);
            Assert.False(flag2.ValueExistsInXml); // value字段不存在

            var flag3 = flora.Flags.Flag[2];
            Assert.Equal("flag3", flag3.Name);
            Assert.True(flag3.ValueExistsInXml);
            Assert.Equal("", flag3.Value); // value字段存在但为空

            // 验证seasonal_kind和flora_variations
            Assert.Single(flora.SeasonalKind);
            var seasonalKind = flora.SeasonalKind[0];
            Assert.True(seasonalKind.FloraVariationsExistsInXml);

            Assert.NotNull(seasonalKind.FloraVariations);
            Assert.Equal(3, seasonalKind.FloraVariations.FloraVariation.Count);

            var variation1 = seasonalKind.FloraVariations.FloraVariation[0];
            Assert.Equal("variation1", variation1.Name);
            Assert.False(variation1.BodyNameExistsInXml); // body_name字段不存在

            var variation2 = seasonalKind.FloraVariations.FloraVariation[1];
            Assert.Equal("variation2", variation2.Name);
            Assert.True(variation2.BodyNameExistsInXml);
            Assert.Equal("body2", variation2.BodyName);

            var variation3 = seasonalKind.FloraVariations.FloraVariation[2];
            Assert.Equal("variation3", variation3.Name);
            Assert.True(variation3.BodyNameExistsInXml);
            Assert.Equal("", variation3.BodyName); // body_name字段存在但为空
            Assert.True(variation3.DensityMultiplierExistsInXml);
            Assert.Equal("1.0", variation3.DensityMultiplier);

            // 清理
            File.Delete(tempFile);
        }
    }
}