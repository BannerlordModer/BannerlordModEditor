using System;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// 验证SiegeEngines和WaterPrefabs修复的测试
    /// </summary>
    public class QualityFixValidationTests
    {
        [Fact]
        public async Task SiegeEngines_RoundTrip_ShouldBeConsistent()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var roundtripDo = SiegeEnginesMapper.ToDO(dto);
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(roundtripDo);
            
            // 验证HasEmptySiegeEngines属性正确映射
            Assert.Equal(originalDo.HasEmptySiegeEngines, dto.HasEmptySiegeEngines);
            Assert.Equal(dto.HasEmptySiegeEngines, roundtripDo.HasEmptySiegeEngines);
            
            // 验证往返测试通过
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
        }

        [Fact]
        public async Task WaterPrefabs_RoundTrip_ShouldHandleBooleanCaseSensitivity()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            var dto = WaterPrefabsMapper.ToDTO(originalDo);
            var roundtripDo = WaterPrefabsMapper.ToDO(dto);
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(roundtripDo);
            
            // 验证HasEmptyWaterPrefabs属性正确映射
            Assert.Equal(originalDo.HasEmptyWaterPrefabs, dto.HasEmptyWaterPrefabs);
            Assert.Equal(dto.HasEmptyWaterPrefabs, roundtripDo.HasEmptyWaterPrefabs);
            
            // 验证往返测试通过
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
            
            // 验证布尔值处理
            Assert.NotEmpty(originalDo.WaterPrefabs);
            foreach (var prefab in originalDo.WaterPrefabs)
            {
                Assert.NotNull(prefab.IsGlobal);
                Assert.True(XmlTestUtils.CommonBooleanTrueValues.Contains(prefab.IsGlobal, StringComparer.OrdinalIgnoreCase) ||
                           XmlTestUtils.CommonBooleanFalseValues.Contains(prefab.IsGlobal, StringComparer.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task XmlTestUtils_ShouldNormalizeBooleanValues()
        {
            // Arrange
            string testXml = @"<Root>
    <Item IsGlobal=""True"" is_test=""False"" />
    <Item IsGlobal=""false"" is_test=""TRUE"" />
    <Item IsGlobal=""1"" is_test=""0"" />
</Root>";

            // Act
            var normalizedXml = XmlTestUtils.NormalizeXml(testXml);

            // Assert
            Assert.Contains("IsGlobal=\"true\"", normalizedXml);
            Assert.Contains("IsGlobal=\"false\"", normalizedXml);
            Assert.Contains("is_test=\"false\"", normalizedXml);
            Assert.Contains("is_test=\"true\"", normalizedXml);
            
            // 确保不再包含混合大小写的布尔值
            Assert.DoesNotContain("IsGlobal=\"True\"", normalizedXml);
            Assert.DoesNotContain("IsGlobal=\"False\"", normalizedXml);
        }
    }
}