using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MPClassDivisionsXmlTests
    {
        [Fact]
        public void MPClassDivisions_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPClassDivisions>
    <MPClassDivision
        id=""mp_light_infantry_vlandia""
        hero=""mp_light_infantry_vlandia_hero""
        troop=""mp_light_infantry_vlandia_troop""
        hero_idle_anim=""act_vlandia_mp_peasant_levy_idle""
        troop_idle_anim=""act_idle_unarmed_1""
        multiplier=""0.92""
        cost=""80""
        casual_cost=""90""
        icon=""Infantry_Light""
        melee_ai=""30""
        ranged_ai=""30""
        armor=""7""
        movement_speed=""0.82""
        combat_movement_speed=""0.9""
        acceleration=""1.8"">
        <Perks>
            <Perk
                game_mode=""all""
                name=""{=DlKYZgA3}Improved Armor""
                description=""{=IUsT27oV}Provides +9 armor.""
                icon=""PerkToughness""
                hero_idle_anim=""act_vlandia_mp_peasant_levy_armorperk_idle""
                perk_list=""1"">
                <OnSpawnEffect
                    type=""ArmorOnSpawn""
                    value=""9"" />
                <OnSpawnEffect
                    type=""AlternativeEquipmentOnSpawn""
                    slot=""Cape""
                    item=""mp_peasant_hood""
                    target=""Player"" />
            </Perk>
        </Perks>
    </MPClassDivision>
</MPClassDivisions>";

            var result = XmlTestUtils.Deserialize<MPClassDivisionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.MPClassDivisions);
            
            var division = result.MPClassDivisions[0];
            Assert.Equal("mp_light_infantry_vlandia", division.Id);
            Assert.Equal("mp_light_infantry_vlandia_hero", division.Hero);
            Assert.Equal("mp_light_infantry_vlandia_troop", division.Troop);
            Assert.Equal("80", division.Cost);
            Assert.Equal("7", division.Armor);
            
            Assert.True(division.HasPerks);
            Assert.NotEmpty(division.Perks.PerksList);
            
            var perk = division.Perks.PerksList[0];
            Assert.Equal("all", perk.GameMode);
            Assert.Equal("{=DlKYZgA3}Improved Armor", perk.Name);
            Assert.Equal("PerkToughness", perk.Icon);
            Assert.NotEmpty(perk.OnSpawnEffects);
            
            var effect = perk.OnSpawnEffects[0];
            Assert.Equal("ArmorOnSpawn", effect.Type);
            Assert.Equal("9", effect.Value);
        }

        [Fact]
        public void MPClassDivisions_CanSerializeToXml()
        {
            // Arrange
            var model = new MPClassDivisionsDO
            {
                MPClassDivisions = new List<MPClassDivisionDO>
                {
                    new MPClassDivisionDO
                    {
                        Id = "test_division",
                        Cost = "100",
                        Armor = "5",
                        HasPerks = true,
                        Perks = new PerksDO
                        {
                            PerksList = new List<PerkDO>
                            {
                                new PerkDO
                                {
                                    GameMode = "all",
                                    Name = "Test Perk",
                                    OnSpawnEffects = new List<OnSpawnEffectDO>
                                    {
                                        new OnSpawnEffectDO
                                        {
                                            Type = "TestEffect",
                                            Value = "1"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var xml = XmlTestUtils.Serialize(model);

            // Assert
            Assert.Contains("test_division", xml);
            Assert.Contains("Test Perk", xml);
            Assert.Contains("TestEffect", xml);
        }

        [Fact]
        public void MPClassDivisions_RoundTripPreservesData()
        {
            // Arrange
            var originalModel = new MPClassDivisionsDO
            {
                MPClassDivisions = new List<MPClassDivisionDO>
                {
                    new MPClassDivisionDO
                    {
                        Id = "test_division",
                        Hero = "test_hero",
                        Troop = "test_troop",
                        Cost = "150",
                        Armor = "10",
                        MovementSpeed = "0.9",
                        HasPerks = true,
                        Perks = new PerksDO
                        {
                            PerksList = new List<PerkDO>
                            {
                                new PerkDO
                                {
                                    GameMode = "skirmish",
                                    Name = "Test Perk",
                                    Description = "Test Description",
                                    Icon = "TestIcon",
                                    OnSpawnEffects = new List<OnSpawnEffectDO>
                                    {
                                        new OnSpawnEffectDO
                                        {
                                            Type = "ArmorOnSpawn",
                                            Value = "5"
                                        }
                                    },
                                    Effects = new List<EffectDO>
                                    {
                                        new EffectDO
                                        {
                                            Type = "MountDamage",
                                            Value = "0.5"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act - Serialize
            var xml = XmlTestUtils.Serialize(originalModel);

            // Act - Deserialize
            var deserializedModel = XmlTestUtils.Deserialize<MPClassDivisionsDO>(xml);

            // Assert
            Assert.NotNull(deserializedModel);
            Assert.Equal(originalModel.MPClassDivisions.Count, deserializedModel.MPClassDivisions.Count);
            
            var original = originalModel.MPClassDivisions[0];
            var deserialized = deserializedModel.MPClassDivisions[0];
            
            Assert.Equal(original.Id, deserialized.Id);
            Assert.Equal(original.Hero, deserialized.Hero);
            Assert.Equal(original.Troop, deserialized.Troop);
            Assert.Equal(original.Cost, deserialized.Cost);
            Assert.Equal(original.Armor, deserialized.Armor);
            Assert.Equal(original.MovementSpeed, deserialized.MovementSpeed);
            
            Assert.Equal(original.Perks.PerksList.Count, deserialized.Perks.PerksList.Count);
            
            var originalPerk = original.Perks.PerksList[0];
            var deserializedPerk = deserialized.Perks.PerksList[0];
            
            Assert.Equal(originalPerk.GameMode, deserializedPerk.GameMode);
            Assert.Equal(originalPerk.Name, deserializedPerk.Name);
            Assert.Equal(originalPerk.Description, deserializedPerk.Description);
            Assert.Equal(originalPerk.Icon, deserializedPerk.Icon);
            
            Assert.Equal(originalPerk.OnSpawnEffects.Count, deserializedPerk.OnSpawnEffects.Count);
            Assert.Equal(originalPerk.Effects.Count, deserializedPerk.Effects.Count);
        }

        [Fact]
        public void MPClassDivisions_EmptyXmlHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<MPClassDivisions></MPClassDivisions>";

            var result = XmlTestUtils.Deserialize<MPClassDivisionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.MPClassDivisions);
        }

        [Fact]
        public void MPClassDivisions_ComplexPerkStructureHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<MPClassDivisions>
    <MPClassDivision id=""test"">
        <Perks>
            <Perk game_mode=""all"" name=""Farmer"">
                <Effect type=""MountDamage"" value=""0.50"" />
                <RandomOnSpawnEffect type=""RandomEquipmentOnSpawn"" target=""Troops"">
                    <Group>
                        <Item slot=""Item3"" item=""mp_western_pitchfork_metal"" />
                    </Group>
                    <Group>
                        <Item slot=""Item3"" item=""mp_western_pitchfork_wood"" />
                    </Group>
                </RandomOnSpawnEffect>
            </Perk>
        </Perks>
    </MPClassDivision>
</MPClassDivisions>";

            var result = XmlTestUtils.Deserialize<MPClassDivisionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.MPClassDivisions);
            
            var division = result.MPClassDivisions[0];
            Assert.True(division.HasPerks);
            Assert.Single(division.Perks.PerksList);
            
            var perk = division.Perks.PerksList[0];
            Assert.Equal("all", perk.GameMode);
            Assert.Equal("Farmer", perk.Name);
            Assert.Single(perk.Effects);
            Assert.Single(perk.RandomOnSpawnEffects);
            
            var effect = perk.Effects[0];
            Assert.Equal("MountDamage", effect.Type);
            Assert.Equal("0.50", effect.Value);
            
            var randomEffect = perk.RandomOnSpawnEffects[0];
            Assert.Equal("RandomEquipmentOnSpawn", randomEffect.Type);
            Assert.Equal("Troops", randomEffect.Target);
            Assert.Equal(2, randomEffect.Groups.Count);
            
            Assert.Single(randomEffect.Groups[0].Items);
            var firstItem = randomEffect.Groups[0].Items[0];
            Assert.Equal("Item3", firstItem.Slot);
            Assert.Equal("mp_western_pitchfork_metal", firstItem.Item);
        }
    }
}