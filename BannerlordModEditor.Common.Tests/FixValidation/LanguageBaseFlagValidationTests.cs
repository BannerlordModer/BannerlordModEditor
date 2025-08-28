using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// LanguageBase标志验证测试
    /// </summary>
    public class LanguageBaseFlagValidationTests
    {
        [Fact]
        public void TestLanguageBaseEmptyTagsFlag()
        {
            // Arrange
            string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
  </tags>
</base>";

            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            
            // Check flags
            Console.WriteLine($"LanguageBaseDO.HasEmptyTags: {originalDo.HasEmptyTags}");
            Console.WriteLine($"LanguageBaseDO.Tags.HasEmptyTags: {originalDo.Tags.HasEmptyTags}");
            Console.WriteLine($"LanguageBaseDO.ShouldSerializeTags(): {originalDo.ShouldSerializeTags()}");
            Console.WriteLine($"LanguageBaseDO.Tags.ShouldSerializeTags(): {originalDo.Tags.ShouldSerializeTags()}");
            
            // Convert to DTO and back to DO
            var dto = LanguageBaseMapper.ToDTO(originalDo);
            var roundtripDo = LanguageBaseMapper.ToDO(dto);
            
            // Check flags after round trip
            Console.WriteLine($"After round trip:");
            Console.WriteLine($"LanguageBaseDO.HasEmptyTags: {roundtripDo.HasEmptyTags}");
            Console.WriteLine($"LanguageBaseDO.Tags.HasEmptyTags: {roundtripDo.Tags.HasEmptyTags}");
            Console.WriteLine($"LanguageBaseDO.ShouldSerializeTags(): {roundtripDo.ShouldSerializeTags()}");
            Console.WriteLine($"LanguageBaseDO.Tags.ShouldSerializeTags(): {roundtripDo.Tags.ShouldSerializeTags()}");
            
            // Check DTO flags
            Console.WriteLine($"DTO Tags.HasEmptyTags: {dto.Tags.HasEmptyTags}");
            Console.WriteLine($"DTO Tags.ShouldSerializeTags(): {dto.Tags.ShouldSerializeTags()}");
        }
    }
}