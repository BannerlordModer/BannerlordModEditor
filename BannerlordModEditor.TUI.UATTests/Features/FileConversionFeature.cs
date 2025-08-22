using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.Common.Models;
using System.Collections.Generic;
using BannerlordModEditor.TUI.UATTests.Common;

namespace BannerlordModEditor.TUI.UATTests.Features
{
    /// <summary>
    /// BDD特性：文件格式转换功能
    /// 
    /// 作为一个Bannerlord Mod开发者
    /// 我希望能够将Excel文件转换为XML格式
    /// 以便在游戏中使用Mod配置
    /// </summary>
    public class FileConversionFeature : BddTestBase
    {
        public FileConversionFeature(ITestOutputHelper output) : base(output)
        {
        }

        #region 场景1: 成功的Excel到XML转换

        /// <summary>
        /// 场景: 成功的Excel到XML转换
        /// 当 我有一个包含Mod配置的Excel文件
        /// 并且 我选择将其转换为XML格式
        /// 那么 转换应该成功完成
        /// 并且 生成的XML文件应该包含所有原始数据
        /// </summary>
        [Fact]
        public async Task ExcelToXmlConversion_Success()
        {
            // Given - 准备测试数据
            string excelFilePath = null;
            string xmlFilePath = null;
            
            try
            {
                // Given 我有一个包含Mod配置的Excel文件
                excelFilePath = CreateTestExcelFile("mod_config.xlsx", @"Name,Value,Description
player_health,100,玩家生命值
player_stamina,150,玩家耐力值
player_mana,50,玩家法力值
move_speed,1.2,移动速度倍率
jump_force,1.5,跳跃力倍率");

                xmlFilePath = Path.Combine(TestTempDir, "mod_config.xml");

                // When 我选择将其转换为XML格式
                var result = await ConversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

                // Then 转换应该成功完成
                result.Success.Should().BeTrue("转换应该成功");
                result.Errors.ShouldBeEmpty("不应该有错误");
                result.RecordsProcessed.ShouldBeGreaterThan(0, "应该处理了记录");

                // And 生成的XML文件应该包含所有原始数据
                VerifyFileExistsAndNotEmpty(xmlFilePath);
                VerifyFileFormat(xmlFilePath, ".xml");

                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                xmlContent.Should().Contain("player_health", "XML应该包含player_health");
                xmlContent.Should().Contain("100", "XML应该包含值100");
                xmlContent.Should().Contain("玩家生命值", "XML应该包含中文描述");
                
                Output.WriteLine($"转换成功: {result.Message}");
                Output.WriteLine($"处理记录数: {result.RecordsProcessed}");
                Output.WriteLine($"耗时: {result.Duration.TotalMilliseconds:F2}ms");
            }
            finally
            {
                CleanupTestFiles(excelFilePath, xmlFilePath);
            }
        }

        #endregion

        #region 场景2: 成功的XML到Excel转换

        /// <summary>
        /// 场景: 成功的XML到Excel转换
        /// 当 我有一个XML格式的Mod配置文件
        /// 并且 我需要将其转换为Excel以便编辑
        /// 那么 转换应该成功完成
        /// 并且 生成的Excel文件应该保持数据完整性
        /// </summary>
        [Fact]
        public async Task XmlToExcelConversion_Success()
        {
            // Given - 准备测试数据
            string xmlFilePath = null;
            string excelFilePath = null;
            
            try
            {
                // Given 我有一个XML格式的Mod配置文件
                xmlFilePath = CreateTestXmlFile("weapon_stats.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<weapons>
    <weapon>
        <id>sword_iron</id>
        <name>铁剑</name>
        <damage>25</damage>
        <speed>1.2</speed>
        <weight>1.5</weight>
    </weapon>
    <weapon>
        <id>bow_wood</id>
        <name>木弓</name>
        <damage>18</damage>
        <speed>0.8</speed>
        <weight>0.8</weight>
    </weapon>
</weapons>");

                excelFilePath = Path.Combine(TestTempDir, "weapon_stats.xlsx");

                // When 我需要将其转换为Excel以便编辑
                var result = await ConversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);

                // Then 转换应该成功完成
                result.Success.Should().BeTrue("转换应该成功");
                result.Errors.ShouldBeEmpty("不应该有错误");
                result.RecordsProcessed.ShouldBeGreaterThan(0, "应该处理了记录");

                // And 生成的Excel文件应该保持数据完整性
                VerifyFileExistsAndNotEmpty(excelFilePath);
                VerifyFileFormat(excelFilePath, ".xlsx");

                var excelContent = await File.ReadAllTextAsync(excelFilePath);
                excelContent.Should().Contain("sword_iron", "Excel应该包含sword_iron");
                excelContent.Should().Contain("铁剑", "Excel应该包含中文武器名称");
                excelContent.Should().Contain("25", "Excel应该包含伤害值25");
                
                Output.WriteLine($"转换成功: {result.Message}");
                Output.WriteLine($"处理记录数: {result.RecordsProcessed}");
                Output.WriteLine($"耗时: {result.Duration.TotalMilliseconds:F2}ms");
            }
            finally
            {
                CleanupTestFiles(xmlFilePath, excelFilePath);
            }
        }

        #endregion

        #region 场景3: 处理大型文件

        /// <summary>
        /// 场景: 处理大型文件
        /// 当 我有一个包含大量数据的Excel文件
        /// 并且 我需要将其转换为XML格式
        /// 那么 系统应该能够处理大型文件
        /// 并且 在合理时间内完成转换
        /// </summary>
        [Fact]
        public async Task LargeFileConversion_Performance()
        {
            // Given - 准备大型测试数据
            string excelFilePath = null;
            string xmlFilePath = null;
            
            try
            {
                // Given 我有一个包含大量数据的Excel文件
                var largeContent = GenerateLargeExcelContent(1000); // 1000行数据
                excelFilePath = CreateTestExcelFile("large_mod_data.xlsx", largeContent);
                xmlFilePath = Path.Combine(TestTempDir, "large_mod_data.xml");

                var startTime = DateTime.UtcNow;

                // When 我需要将其转换为XML格式
                var result = await ConversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

                var endTime = DateTime.UtcNow;
                var duration = (endTime - startTime).TotalMilliseconds;

                // Then 系统应该能够处理大型文件
                result.Success.ShouldBeTrue("大型文件转换应该成功");
                result.Errors.ShouldBeEmpty("不应该有错误");
                result.RecordsProcessed.ShouldBe(1000, "应该处理1000条记录");

                // And 在合理时间内完成转换
                duration.Should().BeLessThan(10000, "1000条记录转换应该在10秒内完成"); // 10秒阈值

                // And 生成的文件应该包含所有数据
                VerifyFileExistsAndNotEmpty(xmlFilePath);
                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                xmlContent.Should().Contain("item_999", "XML应该包含最后一条记录");
                
                Output.WriteLine($"大型文件转换成功");
                Output.WriteLine($"记录数: {result.RecordsProcessed}");
                Output.WriteLine($"总耗时: {duration:F2}ms");
                Output.WriteLine($"平均每条记录: {duration / result.RecordsProcessed:F2}ms");
            }
            finally
            {
                CleanupTestFiles(excelFilePath, xmlFilePath);
            }
        }

        private string GenerateLargeExcelContent(int rowCount)
        {
            var content = new System.Text.StringBuilder();
            content.AppendLine("ID,Name,Value,Description,Category");

            for (int i = 0; i < rowCount; i++)
            {
                content.AppendLine($"item_{i},测试项{i},{i * 10},这是第{i}个测试项,类别{i % 10}");
            }

            return content.ToString();
        }

        #endregion

        #region 场景4: 处理特殊字符和多语言

        /// <summary>
        /// 场景: 处理特殊字符和多语言
        /// 当 我有一个包含中文和特殊字符的Excel文件
        /// 并且 我需要将其转换为XML格式
        /// 那么 转换应该正确处理Unicode字符
        /// 并且 保持文本的完整性
        /// </summary>
        [Fact]
        public async Task SpecialCharacterConversion_Integrity()
        {
            // Given - 准备包含特殊字符的测试数据
            string excelFilePath = null;
            string xmlFilePath = null;
            
            try
            {
                // Given 我有一个包含中文和特殊字符的Excel文件
                excelFilePath = CreateTestExcelFile("special_chars.xlsx", @"Name,Value,Description,Notes
火球术,50,发射一个火球攻击敌人,造成 <fire>50</fire> 点伤害
冰霜护甲,0,提供冰霜防护,增加 <ice>20%</ice> 防御力
治疗术,30,恢复生命值,恢复 <heal>30+等级×5</heal> 点生命值
闪电链,40,连锁闪电攻击,主要目标:100%,次要目标:50%");

                xmlFilePath = Path.Combine(TestTempDir, "special_chars.xml");

                // When 我需要将其转换为XML格式
                var result = await ConversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

                // Then 转换应该正确处理Unicode字符
                result.Success.ShouldBeTrue("特殊字符转换应该成功");
                result.Errors.ShouldBeEmpty("不应该有错误");

                // And 保持文本的完整性
                VerifyFileExistsAndNotEmpty(xmlFilePath);
                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                
                // 验证中文字符
                xmlContent.Should().Contain("火球术", "应该正确处理中文字符");
                xmlContent.Should().Contain("冰霜护甲", "应该正确处理中文字符");
                
                // 验证特殊字符
                xmlContent.Should().Contain("<fire>", "应该正确处理XML特殊字符");
                xmlContent.Should().Contain("<ice>", "应该正确处理XML特殊字符");
                xmlContent.Should().Contain("<heal>", "应该正确处理XML特殊字符");
                xmlContent.Should().Contain("20%", "应该正确处理百分号");
                xmlContent.Should().Contain("等级×5", "应该正确处理数学符号");
                
                Output.WriteLine($"特殊字符转换成功: {result.Message}");
                Output.WriteLine($"处理记录数: {result.RecordsProcessed}");
            }
            finally
            {
                CleanupTestFiles(excelFilePath, xmlFilePath);
            }
        }

        #endregion

        #region 场景5: 往返转换测试

        /// <summary>
        /// 场景: 往返转换测试
        /// 当 我将Excel文件转换为XML
        /// 然后再将XML转换回Excel
        /// 那么两次转换后的数据应该与原始数据保持一致
        /// </summary>
        [Fact]
        public async Task RoundTripConversion_DataIntegrity()
        {
            // Given - 准备原始数据
            string originalExcelPath = null;
            string intermediateXmlPath = null;
            string finalExcelPath = null;
            
            try
            {
                // Given 我有原始的Excel数据
                originalExcelPath = CreateTestExcelFile("original_data.xlsx", @"ID,Name,Type,Value,Description
001,health_base,integer,100,基础生命值
002,mana_base,integer,50,基础法力值
003,move_speed,float,1.2,移动速度倍率
004,jump_force,float,1.5,跳跃力倍率
005,defense_bonus,integer,0,防御力加成");

                intermediateXmlPath = Path.Combine(TestTempDir, "intermediate.xml");
                finalExcelPath = Path.Combine(TestTempDir, "final.xlsx");

                // When 我将Excel文件转换为XML
                var excelToXmlResult = await ConversionService.ExcelToXmlAsync(originalExcelPath, intermediateXmlPath);
                excelToXmlResult.Success.Should().BeTrue("Excel到XML转换应该成功");

                // 然后再将XML转换回Excel
                var xmlToExcelResult = await ConversionService.XmlToExcelAsync(intermediateXmlPath, finalExcelPath);
                xmlToExcelResult.Success.Should().BeTrue("XML到Excel转换应该成功");

                // Then 两次转换后的数据应该与原始数据保持一致
                VerifyFileExistsAndNotEmpty(finalExcelPath);

                var originalContent = await File.ReadAllTextAsync(originalExcelPath);
                var finalContent = await File.ReadAllTextAsync(finalExcelPath);

                // 验证关键数据点
                originalContent.Should().Contain("health_base", "原始数据应该包含health_base");
                finalContent.Should().Contain("health_base", "最终数据应该包含health_base");
                
                originalContent.Should().Contain("100", "原始数据应该包含值100");
                finalContent.Should().Contain("100", "最终数据应该包含值100");
                
                originalContent.Should().Contain("基础生命值", "原始数据应该包含中文描述");
                finalContent.Should().Contain("基础生命值", "最终数据应该包含中文描述");

                Output.WriteLine($"往返转换测试通过");
                Output.WriteLine($"Excel→XML: {excelToXmlResult.Message}");
                Output.WriteLine($"XML→Excel: {xmlToExcelResult.Message}");
            }
            finally
            {
                CleanupTestFiles(originalExcelPath, intermediateXmlPath, finalExcelPath);
            }
        }

        #endregion
    }
}