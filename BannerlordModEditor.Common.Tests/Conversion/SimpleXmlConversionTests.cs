using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Conversion;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Conversion
{
    public class SimpleXmlConversionTests
    {
        private readonly string _testDataPath = Path.Combine("TestData", "Conversion");

        public SimpleXmlConversionTests()
        {
            // 确保测试数据目录存在
            Directory.CreateDirectory(_testDataPath);
        }

        [Fact]
        public async Task XmlToTable_SimpleAttributes_ShouldConvertCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Attributes>
    <Attribute id=""AgentHitPoints"" name=""HitPoints"" source=""Character"">
        <Documentation>Hit points of an agent.</Documentation>
    </Attribute>
    <Attribute id=""AgentHealth"" name=""Health"" source=""Character"">
        <Documentation>Health of an agent.</Documentation>
    </Attribute>
</Attributes>";

            var xmlFilePath = Path.Combine(_testDataPath, "test_attributes.xml");
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);

            // Act
            var tableData = await SimpleXmlConversionFramework.XmlToTableAsync(xmlFilePath);

            // Assert
            Assert.NotNull(tableData);
            Assert.Equal(2, tableData.Rows.Count);
            Assert.Contains("id", tableData.Columns);
            Assert.Contains("name", tableData.Columns);
            Assert.Contains("source", tableData.Columns);
            Assert.Contains("Documentation", tableData.Columns);

            // 验证第一行数据
            var firstRow = tableData.Rows[0];
            Assert.Equal("AgentHitPoints", firstRow.GetValue<string>("id"));
            Assert.Equal("HitPoints", firstRow.GetValue<string>("name"));
            Assert.Equal("Character", firstRow.GetValue<string>("source"));
            Assert.Equal("Hit points of an agent.", firstRow.GetValue<string>("Documentation"));

            // 验证元数据
            Assert.Equal("Attributes", tableData.Metadata["XmlType"]);
            Assert.Equal("2", tableData.Metadata["RecordCount"]);
        }

        [Fact]
        public async Task TableToXml_SimpleData_ShouldConvertCorrectly()
        {
            // Arrange
            var tableData = new TableData
            {
                Columns = { "id", "name", "value" },
                Rows = 
                {
                    new TableRow
                    {
                        ["id"] = "attr1",
                        ["name"] = "Attribute1",
                        ["value"] = "100"
                    },
                    new TableRow
                    {
                        ["id"] = "attr2", 
                        ["name"] = "Attribute2",
                        ["value"] = "200"
                    }
                },
                Metadata = { ["XmlType"] = "TestAttributes" }
            };

            var xmlFilePath = Path.Combine(_testDataPath, "test_output.xml");

            // Act
            var result = await SimpleXmlConversionFramework.TableToXmlAsync(tableData, xmlFilePath);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(xmlFilePath));

            var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
            Assert.Contains("TestAttributes", xmlContent);
            Assert.Contains("attr1", xmlContent);
            Assert.Contains("Attribute1", xmlContent);
            Assert.Contains("100", xmlContent);
        }

        [Fact]
        public async Task XmlToCsv_SimpleAttributes_ShouldConvertCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Attributes>
    <Attribute id=""AgentHitPoints"" name=""HitPoints"" source=""Character"" />
    <Attribute id=""AgentHealth"" name=""Health"" source=""Character"" />
</Attributes>";

            var xmlFilePath = Path.Combine(_testDataPath, "test_attributes_csv.xml");
            var csvFilePath = Path.Combine(_testDataPath, "test_attributes.csv");

            await File.WriteAllTextAsync(xmlFilePath, xmlContent);

            // Act
            var result = await SimpleXmlConversionFramework.XmlToCsvAsync(xmlFilePath, csvFilePath);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(csvFilePath));

            var csvContent = await File.ReadAllTextAsync(csvFilePath);
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            Assert.Equal(3, lines.Length); // 标题行 + 2数据行
            Assert.Contains("id,name,source", lines[0]);
            Assert.Contains("AgentHitPoints,HitPoints,Character", lines[1]);
            Assert.Contains("AgentHealth,Health,Character", lines[2]);
        }

        [Fact]
        public async Task CsvToXml_SimpleData_ShouldConvertCorrectly()
        {
            // Arrange
            var csvContent = @"id,name,value
attr1,Attribute1,100
attr2,Attribute2,200";

            var csvFilePath = Path.Combine(_testDataPath, "test_simple.csv");
            var xmlFilePath = Path.Combine(_testDataPath, "test_simple_output.xml");

            await File.WriteAllTextAsync(csvFilePath, csvContent);

            // Act
            var result = await SimpleXmlConversionFramework.CsvToXmlAsync(csvFilePath, xmlFilePath);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(xmlFilePath));

            var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
            Assert.Contains("Root", xmlContent);
            Assert.Contains("attr1", xmlContent);
            Assert.Contains("Attribute1", xmlContent);
            Assert.Contains("100", xmlContent);
        }

        [Fact]
        public async Task AnalyzeXmlStructure_ComplexXml_ShouldReturnCorrectInfo()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CombatParameters>
    <definitions>
        <def name=""test1"" val=""value1"" />
        <def name=""test2"" val=""value2"" />
    </definitions>
    <combat_parameters>
        <combat_parameter id=""param1"" value=""test"" />
    </combat_parameters>
</CombatParameters>";

            var xmlFilePath = Path.Combine(_testDataPath, "test_structure.xml");
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);

            // Act
            var structureInfo = await SimpleXmlConversionFramework.AnalyzeXmlStructureAsync(xmlFilePath);

            // Assert
            Assert.NotNull(structureInfo);
            Assert.Equal("CombatParameters", structureInfo.RootElement);
            Assert.Equal(2, structureInfo.EstimatedRecordCount); // definitions + combat_parameters
            Assert.Contains("definitions", structureInfo.Elements);
            Assert.Contains("def", structureInfo.Elements);
            Assert.Contains("combat_parameters", structureInfo.Elements);
            Assert.Contains("combat_parameter", structureInfo.Elements);
            Assert.True(structureInfo.Complexity >= XmlComplexity.Medium);
        }

        [Fact]
        public async Task XmlToTable_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""sword_001"" name=""Sword of Destiny"" description=""A legendary sword with +10 attack"" />
    <Item id=""bow_001"" name=""Elven Bow"" description=""Precision bow with 15% crit chance"" />
</Items>";

            var xmlFilePath = Path.Combine(_testDataPath, "test_special_chars.xml");
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);

            // Act
            var tableData = await SimpleXmlConversionFramework.XmlToTableAsync(xmlFilePath);

            // Assert
            Assert.NotNull(tableData);
            Assert.Equal(2, tableData.Rows.Count);
            
            var firstRow = tableData.Rows[0];
            Assert.Equal("sword_001", firstRow.GetValue<string>("id"));
            Assert.Equal("Sword of Destiny", firstRow.GetValue<string>("name"));
            Assert.Equal("A legendary sword with +10 attack", firstRow.GetValue<string>("description"));
        }

        [Fact]
        public async Task XmlToTable_NonExistentFile_ShouldThrowException()
        {
            // Arrange
            var nonExistentPath = Path.Combine(_testDataPath, "non_existent.xml");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => SimpleXmlConversionFramework.XmlToTableAsync(nonExistentPath));
            
            Assert.Contains("XML文件不存在", exception.Message);
        }

        [Fact]
        public async Task XmlToCsv_WithCommaInData_ShouldEscapeCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""item1"" name=""Sword, Shield"" description=""A weapon"" />
    <Item id=""item2"" name=""Bow"" description=""Long range weapon"" />
</Items>";

            var xmlFilePath = Path.Combine(_testDataPath, "test_comma.xml");
            var csvFilePath = Path.Combine(_testDataPath, "test_comma.csv");

            await File.WriteAllTextAsync(xmlFilePath, xmlContent);

            // Act
            var result = await SimpleXmlConversionFramework.XmlToCsvAsync(xmlFilePath, csvFilePath);

            // Assert
            Assert.True(result);
            var csvContent = await File.ReadAllTextAsync(csvFilePath);
            
            // 验证逗号被正确转义
            Assert.Contains("\"Sword, Shield\"", csvContent);
        }

        [Fact]
        public async Task RoundTripConversion_XmlToTableToXml_ShouldPreserveData()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Attributes>
    <Attribute id=""test1"" name=""Test Attribute"" value=""100"" />
    <Attribute id=""test2"" name=""Another Attribute"" value=""200"" />
</Attributes>";

            var originalPath = Path.Combine(_testDataPath, "original.xml");
            var tablePath = Path.Combine(_testDataPath, "intermediate.table");
            var finalPath = Path.Combine(_testDataPath, "final.xml");

            await File.WriteAllTextAsync(originalPath, originalXml);

            // Act
            var tableData = await SimpleXmlConversionFramework.XmlToTableAsync(originalPath);
            var conversionResult = await SimpleXmlConversionFramework.TableToXmlAsync(tableData, finalPath);

            // Assert
            Assert.True(conversionResult);
            Assert.True(File.Exists(finalPath));

            var finalXml = await File.ReadAllTextAsync(finalPath);
            Assert.Contains("test1", finalXml);
            Assert.Contains("Test Attribute", finalXml);
            Assert.Contains("100", finalXml);
            Assert.Contains("test2", finalXml);
            Assert.Contains("Another Attribute", finalXml);
            Assert.Contains("200", finalXml);
        }
    }
}