using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class PartiesXmlTests
    {
        [Fact]
        public void Deserialize_PartiesXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "parties.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PartiesDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Parties);
            Assert.NotNull(result.Parties.PartyList);
            Assert.NotEmpty(result.Parties.PartyList);
            
            // 验证基本属性
            Assert.Equal("party", result.Type);
            
            // 验证第一个party
            var firstParty = result.Parties.PartyList[0];
            Assert.Equal("p_temp_party", firstParty.Id);
            Assert.Equal("{!}temp party", firstParty.Name);
            Assert.Equal("0x100", firstParty.Flags);
            Assert.Equal("pt_none", firstParty.PartyTemplate);
            Assert.Equal("0.000000, 0.000000, 0.000000", firstParty.Position);
            Assert.Equal("0", firstParty.AverageBearingRot);
        }

        [Fact]
        public void Serialize_PartiesDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "parties.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<PartiesDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_PartiesDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "parties.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<PartiesDO>(xml);

            // Act
            var dto = PartiesMapper.ToDTO(originalDO);
            var convertedDO = PartiesMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.Type, convertedDO.Type);
            Assert.Equal(originalDO.HasParties, convertedDO.HasParties);
            
            // 验证Parties结构
            Assert.Equal(originalDO.Parties.PartyList.Count, convertedDO.Parties.PartyList.Count);
            
            // 验证第一个party
            var originalParty = originalDO.Parties.PartyList[0];
            var convertedParty = convertedDO.Parties.PartyList[0];
            
            Assert.Equal(originalParty.Id, convertedParty.Id);
            Assert.Equal(originalParty.Name, convertedParty.Name);
            Assert.Equal(originalParty.Flags, convertedParty.Flags);
            Assert.Equal(originalParty.PartyTemplate, convertedParty.PartyTemplate);
            Assert.Equal(originalParty.Position, convertedParty.Position);
            Assert.Equal(originalParty.AverageBearingRot, convertedParty.AverageBearingRot);
        }

        [Fact]
        public void PartiesDO_ShouldContainSpecificParties()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "parties.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<PartiesDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var parties = result.Parties.PartyList;
            
            // 验证特定的party存在
            Assert.Contains(parties, p => p.Id == "p_temp_party");
            Assert.Contains(parties, p => p.Id == "p_temp_casualties");
            
            // 验证特定party的属性
            var tempParty = parties.FirstOrDefault(p => p.Id == "p_temp_party");
            Assert.NotNull(tempParty);
            Assert.Equal("{!}temp party", tempParty.Name);
            Assert.Equal("0x100", tempParty.Flags);
            Assert.Equal("pt_none", tempParty.PartyTemplate);
            Assert.Equal("0.000000, 0.000000, 0.000000", tempParty.Position);
            Assert.Equal("0", tempParty.AverageBearingRot);
            
            // 验证带fields的party
            var casualtiesParty = parties.FirstOrDefault(p => p.Id == "p_temp_casualties");
            Assert.NotNull(casualtiesParty);
            Assert.Equal("{!}casualties", casualtiesParty.Name);
            Assert.Equal("0x80100", casualtiesParty.Flags);
            
            // 验证fields
            Assert.NotNull(casualtiesParty.FieldList);
            Assert.NotEmpty(casualtiesParty.FieldList);
            Assert.Contains(casualtiesParty.FieldList, f => f.FieldName == "ThinkFrequencyMin" && f.FieldValue == "3");
            Assert.Contains(casualtiesParty.FieldList, f => f.FieldName == "ThinkFrequencyMax" && f.FieldValue == "21");
        }
    }
}