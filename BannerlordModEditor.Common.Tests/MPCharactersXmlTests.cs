using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MPCharactersXmlTests
    {
        [Fact]
        public void MPCharacters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPCharacters>
    <NPCCharacter
        id=""mp_character""
        level=""36""
        name=""{=eFWJjaBC}Multiplayer Character"">
        <face>
            <BodyProperties
                version=""4""
                age=""25""
                weight=""0.5""
                build=""0.6""
                key=""00000C07000000010011111211161111000701000010000000111011000101000000000207111110000000000000000000000000000000000000000000500000"" />
            <BodyPropertiesMax
                version=""4""
                age=""25""
                weight=""0.5""
                build=""0.6""
                key=""0011FC0FC03C384FFFFFFFFEFFFBEFFFFFFFFFA97EF7FEFFFFEFFFFFFFFFFFFF00000FD20AFFFFFF000000000000000000000000000000000000000000C89101"" />
        </face>
        <skills>
            <skill
                id=""Riding""
                value=""200"" />
            <skill
                id=""OneHanded""
                value=""5"" />
            <skill
                id=""TwoHanded""
                value=""5"" />
            <skill
                id=""Polearm""
                value=""5"" />
            <skill
                id=""Crossbow""
                value=""100"" />
            <skill
                id=""Bow""
                value=""100"" />
            <skill
                id=""Throwing""
                value=""100"" />
        </skills>
        <Equipments>
            <EquipmentRoster>
                <equipment
                    slot=""Body""
                    id=""Item.mp_layered_leather_tunic"" />
                <equipment
                    slot=""Leg""
                    id=""Item.mp_leather_cavalier_boots"" />
                <equipment
                    slot=""Cape""
                    id=""Item.mp_scarf"" />
            </EquipmentRoster>
        </Equipments>
    </NPCCharacter>
</MPCharacters>";

            // Act
            var result = XmlTestUtils.Deserialize<MPCharactersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.NPCCharacterList);
            Assert.Single(result.NPCCharacterList);
            
            var character = result.NPCCharacterList[0];
            Assert.Equal("mp_character", character.Id);
            Assert.Equal("36", character.Level);
            Assert.Equal("{=eFWJjaBC}Multiplayer Character", character.Name);
            
            Assert.NotNull(character.Face);
            Assert.NotNull(character.Face.BodyProperties);
            Assert.Equal("4", character.Face.BodyProperties.Version);
            Assert.Equal("25", character.Face.BodyProperties.Age);
            Assert.Equal("0.5", character.Face.BodyProperties.Weight);
            Assert.Equal("0.6", character.Face.BodyProperties.Build);
            
            Assert.NotNull(character.Face.BodyPropertiesMax);
            Assert.Equal("4", character.Face.BodyPropertiesMax.Version);
            Assert.Equal("25", character.Face.BodyPropertiesMax.Age);
            Assert.Equal("0.5", character.Face.BodyPropertiesMax.Weight);
            Assert.Equal("0.6", character.Face.BodyPropertiesMax.Build);
            
            Assert.NotNull(character.Skills);
            Assert.Equal(7, character.Skills.SkillList.Count);
            
            var ridingSkill = character.Skills.SkillList[0];
            Assert.Equal("Riding", ridingSkill.Id);
            Assert.Equal("200", ridingSkill.Value);
            
            var oneHandedSkill = character.Skills.SkillList[1];
            Assert.Equal("OneHanded", oneHandedSkill.Id);
            Assert.Equal("5", oneHandedSkill.Value);
            
            Assert.NotNull(character.Equipments);
            Assert.Single(character.Equipments.EquipmentRosterList);
            
            var equipmentRoster = character.Equipments.EquipmentRosterList[0];
            Assert.Equal(3, equipmentRoster.EquipmentList.Count);
            
            var bodyEquipment = equipmentRoster.EquipmentList[0];
            Assert.Equal("Body", bodyEquipment.Slot);
            Assert.Equal("Item.mp_layered_leather_tunic", bodyEquipment.Id);
            
            var legEquipment = equipmentRoster.EquipmentList[1];
            Assert.Equal("Leg", legEquipment.Slot);
            Assert.Equal("Item.mp_leather_cavalier_boots", legEquipment.Id);
            
            var capeEquipment = equipmentRoster.EquipmentList[2];
            Assert.Equal("Cape", capeEquipment.Slot);
            Assert.Equal("Item.mp_scarf", capeEquipment.Id);
        }

        [Fact]
        public void MPCharacters_CanSerializeToXml()
        {
            // Arrange
            var characters = new MPCharactersDO
            {
                NPCCharacterList = new List<NPCCharacterDO>
                {
                    new NPCCharacterDO
                    {
                        Id = "mp_character",
                        Level = "36",
                        Name = "{=eFWJjaBC}Multiplayer Character",
                        Face = new CharacterFaceDO
                        {
                            BodyProperties = new BodyPropertiesDO
                            {
                                Version = "4",
                                Age = "25",
                                Weight = "0.5",
                                Build = "0.6"
                            }
                        },
                        Skills = new CharacterSkillsDO
                        {
                            SkillList = new List<CharacterSkillDO>
                            {
                                new CharacterSkillDO
                                {
                                    Id = "Riding",
                                    Value = "200"
                                },
                                new CharacterSkillDO
                                {
                                    Id = "OneHanded",
                                    Value = "5"
                                }
                            }
                        },
                        Equipments = new CharacterEquipmentsDO
                        {
                            EquipmentRosterList = new List<EquipmentRosterDO>
                            {
                                new EquipmentRosterDO
                                {
                                    EquipmentList = new List<CharacterEquipmentDO>
                                    {
                                        new CharacterEquipmentDO
                                        {
                                            Slot = "Body",
                                            Id = "Item.mp_layered_leather_tunic"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(characters);

            // Assert
            Assert.NotNull(serializedXml);
            Assert.Contains("MPCharacters", serializedXml);
            Assert.Contains("id=\"mp_character\"", serializedXml);
            Assert.Contains("level=\"36\"", serializedXml);
            Assert.Contains("name=\"{=eFWJjaBC}Multiplayer Character\"", serializedXml);
            Assert.Contains("id=\"Riding\"", serializedXml);
            Assert.Contains("value=\"200\"", serializedXml);
            Assert.Contains("slot=\"Body\"", serializedXml);
            Assert.Contains("id=\"Item.mp_layered_leather_tunic\"", serializedXml);
        }

        [Fact]
        public void MPCharacters_RoundTripSerialization()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPCharacters>
    <NPCCharacter
        id=""mp_character""
        level=""36""
        name=""{=eFWJjaBC}Multiplayer Character"">
        <face>
            <BodyProperties
                version=""4""
                age=""25""
                weight=""0.5""
                build=""0.6"" />
        </face>
        <skills>
            <skill
                id=""Riding""
                value=""200"" />
        </skills>
        <Equipments>
            <EquipmentRoster>
                <equipment
                    slot=""Body""
                    id=""Item.mp_layered_leather_tunic"" />
            </EquipmentRoster>
        </Equipments>
    </NPCCharacter>
</MPCharacters>";

            // Act
            var deserialized = XmlTestUtils.Deserialize<MPCharactersDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(deserialized);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void MPCharacters_MinimalCharacter()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPCharacters>
    <NPCCharacter
        id=""dummy_no_armor""
        name=""{=!}Min Armor (0)""
        age=""48""
        voice=""curt""
        default_group=""Infantry""
        is_hero=""false""
        culture=""Culture.khuzait"">
        <face>
            <BodyProperties
                version=""4""
                age=""38.5""
                weight=""0.3432""
                build=""0.2654""
                key=""000B5C0E55BC34423BA3A9598495DD786927950D8394A37E6BD97A7C65E1865600045005043B8488000000000000000000000000000000000000000000C81001"" />
        </face>
        <Equipments>
            <EquipmentRoster>
                <equipment
                    slot=""Body""
                    id=""Item.mp_dummy_armor_min"" />
            </EquipmentRoster>
        </Equipments>
    </NPCCharacter>
</MPCharacters>";

            // Act
            var result = XmlTestUtils.Deserialize<MPCharactersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.NPCCharacterList);
            Assert.Single(result.NPCCharacterList);
            
            var character = result.NPCCharacterList[0];
            Assert.Equal("dummy_no_armor", character.Id);
            Assert.Equal("{=!}Min Armor (0)", character.Name);
            Assert.Equal("48", character.Age);
            Assert.Equal("curt", character.Voice);
            Assert.Equal("Infantry", character.DefaultGroup);
            Assert.Equal("false", character.IsHero);
            Assert.Equal("Culture.khuzait", character.Culture);
            
            Assert.NotNull(character.Face);
            Assert.NotNull(character.Face.BodyProperties);
            Assert.Equal("4", character.Face.BodyProperties.Version);
            Assert.Equal("38.5", character.Face.BodyProperties.Age);
            Assert.Null(character.Face.BodyPropertiesMax);
            
            Assert.Null(character.Skills);
            
            Assert.NotNull(character.Equipments);
            Assert.Single(character.Equipments.EquipmentRosterList);
            Assert.Single(character.Equipments.EquipmentRosterList[0].EquipmentList);
        }

        [Fact]
        public void MPCharacters_EmptyCharacterList()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPCharacters />";

            // Act
            var result = XmlTestUtils.Deserialize<MPCharactersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.NPCCharacterList);
            Assert.Empty(result.NPCCharacterList);
        }

        [Fact]
        public void MPCharacters_MultipleEquipmentRosters()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPCharacters>
    <NPCCharacter
        id=""test_character""
        name=""Test Character"">
        <Equipments>
            <EquipmentRoster>
                <equipment slot=""Body"" id=""Item.armor1"" />
            </EquipmentRoster>
            <EquipmentRoster>
                <equipment slot=""Body"" id=""Item.armor2"" />
                <equipment slot=""Head"" id=""Item.helmet1"" />
            </EquipmentRoster>
        </Equipments>
    </NPCCharacter>
</MPCharacters>";

            // Act
            var result = XmlTestUtils.Deserialize<MPCharactersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.NPCCharacterList);
            Assert.Single(result.NPCCharacterList);
            
            var character = result.NPCCharacterList[0];
            Assert.NotNull(character.Equipments);
            Assert.Equal(2, character.Equipments.EquipmentRosterList.Count);
            
            var firstRoster = character.Equipments.EquipmentRosterList[0];
            Assert.Single(firstRoster.EquipmentList);
            Assert.Equal("Item.armor1", firstRoster.EquipmentList[0].Id);
            
            var secondRoster = character.Equipments.EquipmentRosterList[1];
            Assert.Equal(2, secondRoster.EquipmentList.Count);
            Assert.Equal("Item.armor2", secondRoster.EquipmentList[0].Id);
            Assert.Equal("Item.helmet1", secondRoster.EquipmentList[1].Id);
        }
    }
}