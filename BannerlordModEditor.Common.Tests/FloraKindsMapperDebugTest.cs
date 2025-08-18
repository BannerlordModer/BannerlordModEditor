using System;
using System.IO;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsMapperDebugTest
    {
        [Fact]
        public void Debug_Mapper_HasLayouts_Property()
        {
            Console.WriteLine("=== 调试Mapper的HasLayouts属性 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            var xml = File.ReadAllText(xmlPath);
            var originalDO = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);

            Console.WriteLine($"OriginalDO.HasLayouts: {originalDO.HasLayouts}");
            Console.WriteLine($"OriginalDO.Layouts.LayoutList.Count: {originalDO.Layouts.LayoutList.Count}");

            var dto = FloraKindsLayoutMapper.ToDTO(originalDO);
            var convertedDO = FloraKindsLayoutMapper.ToDO(dto);

            Console.WriteLine($"ConvertedDO.HasLayouts: {convertedDO.HasLayouts}");
            Console.WriteLine($"ConvertedDO.Layouts.LayoutList.Count: {convertedDO.Layouts.LayoutList.Count}");

            Console.WriteLine($"HasLayouts match: {originalDO.HasLayouts == convertedDO.HasLayouts}");
            
            // 检查LayoutsContainerDO的HasLayouts属性
            Console.WriteLine($"OriginalDO.Layouts.GetType(): {originalDO.Layouts.GetType()}");
            Console.WriteLine($"ConvertedDO.Layouts.GetType(): {convertedDO.Layouts.GetType()}");
            
            Assert.Equal(originalDO.HasLayouts, convertedDO.HasLayouts);
        }
    }
}