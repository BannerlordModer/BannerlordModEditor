using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Game;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MonsterUsageSetsXmlTests
    {
        [Fact]
        public void MonsterUsageSets_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<monster_usage_sets>
    <monster_usage_set
        id=""test_monster""
        hit_object_action=""act_hit_object""
        hit_object_falling_action=""act_hit_object_falling""
        rear_action=""act_rear""
        rear_damaged_action=""act_rear_damaged""
        ladder_climb_action=""act_ladder_climb""
        strike_ladder_action=""act_strike_ladder"">
        <MonsterUsageStrikes>
            <MonsterUsageStrike
                is_heavy=""true""
                is_left_stance=""false""
                direction=""left""
                body_part=""head""
                impact=""blunt""
                action=""act_strike_left"" />
            <MonsterUsageStrike
                is_heavy=""false""
                is_left_stance=""true""
                direction=""right""
                body_part=""body""
                impact=""cut""
                action=""act_strike_right"" />
        </MonsterUsageStrikes>
    </monster_usage_set>
</monster_usage_sets>";

            var result = XmlTestUtils.Deserialize<MonsterUsageSetsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.MonsterUsageSets);
            
            var monsterSet = result.MonsterUsageSets[0];
            Assert.Equal("test_monster", monsterSet.Id);
            Assert.Equal("act_hit_object", monsterSet.HitObjectAction);
            Assert.Equal("act_rear", monsterSet.RearAction);
            Assert.Equal("act_ladder_climb", monsterSet.LadderClimbAction);
            
            Assert.True(monsterSet.HasMonsterUsageStrikes);
            Assert.NotNull(monsterSet.MonsterUsageStrikes);
            Assert.Equal(2, monsterSet.MonsterUsageStrikes.Strikes.Count);
            
            var firstStrike = monsterSet.MonsterUsageStrikes.Strikes[0];
            Assert.Equal("True", firstStrike.IsHeavy);
            Assert.Equal("False", firstStrike.IsLeftStance);
            Assert.Equal("left", firstStrike.Direction);
            Assert.Equal("head", firstStrike.BodyPart);
            Assert.Equal("blunt", firstStrike.Impact);
            Assert.Equal("act_strike_left", firstStrike.Action);
        }

        [Fact]
        public void MonsterUsageSets_CanSerializeToXml()
        {
            // Arrange
            var model = new MonsterUsageSetsDO
            {
                MonsterUsageSets = new List<MonsterUsageSetDO>
                {
                    new MonsterUsageSetDO
                    {
                        Id = "test_monster",
                        HitObjectAction = "act_hit_object",
                        RearAction = "act_rear",
                        HasMonsterUsageStrikes = true,
                        MonsterUsageStrikes = new MonsterUsageStrikesDO
                        {
                            Strikes = new List<MonsterUsageStrikeDO>
                            {
                                new MonsterUsageStrikeDO
                                {
                                    IsHeavy = "True",
                                    Direction = "left",
                                    BodyPart = "head",
                                    Impact = "blunt",
                                    Action = "act_strike_left"
                                }
                            }
                        }
                    }
                }
            };

            var xml = XmlTestUtils.Serialize(model);

            // Assert
            Assert.Contains("test_monster", xml);
            Assert.Contains("act_hit_object", xml);
            Assert.Contains("monster_usage_strike", xml);
            Assert.Contains("blunt", xml);
        }

        [Fact]
        public void MonsterUsageSets_RoundTripPreservesData()
        {
            // Arrange
            var originalModel = new MonsterUsageSetsDO
            {
                MonsterUsageSets = new List<MonsterUsageSetDO>
                {
                    new MonsterUsageSetDO
                    {
                        Id = "test_monster",
                        HitObjectAction = "act_hit_object",
                        HitObjectFallingAction = "act_hit_object_falling",
                        RearAction = "act_rear",
                        RearDamagedAction = "act_rear_damaged",
                        LadderClimbAction = "act_ladder_climb",
                        StrikeLadderAction = "act_strike_ladder",
                        HasMonsterUsageStrikes = true,
                        MonsterUsageStrikes = new MonsterUsageStrikesDO
                        {
                            Strikes = new List<MonsterUsageStrikeDO>
                            {
                                new MonsterUsageStrikeDO
                                {
                                    IsHeavy = "True",
                                    IsLeftStance = "False",
                                    Direction = "left",
                                    BodyPart = "head",
                                    Impact = "blunt",
                                    Action = "act_strike_left"
                                },
                                new MonsterUsageStrikeDO
                                {
                                    IsHeavy = "False",
                                    IsLeftStance = "True",
                                    Direction = "right",
                                    BodyPart = "body",
                                    Impact = "cut",
                                    Action = "act_strike_right"
                                }
                            }
                        }
                    }
                }
            };

            // Act - Serialize
            var xml = XmlTestUtils.Serialize(originalModel);

            // Act - Deserialize
            var deserializedModel = XmlTestUtils.Deserialize<MonsterUsageSetsDO>(xml);

            // Assert
            Assert.NotNull(deserializedModel);
            Assert.Equal(originalModel.MonsterUsageSets.Count, deserializedModel.MonsterUsageSets.Count);
            
            var original = originalModel.MonsterUsageSets[0];
            var deserialized = deserializedModel.MonsterUsageSets[0];
            
            Assert.Equal(original.Id, deserialized.Id);
            Assert.Equal(original.HitObjectAction, deserialized.HitObjectAction);
            Assert.Equal(original.HitObjectFallingAction, deserialized.HitObjectFallingAction);
            Assert.Equal(original.RearAction, deserialized.RearAction);
            Assert.Equal(original.RearDamagedAction, deserialized.RearDamagedAction);
            Assert.Equal(original.LadderClimbAction, deserialized.LadderClimbAction);
            Assert.Equal(original.StrikeLadderAction, deserialized.StrikeLadderAction);
            
            Assert.Equal(original.HasMonsterUsageStrikes, deserialized.HasMonsterUsageStrikes);
            Assert.Equal(original.MonsterUsageStrikes.Strikes.Count, deserialized.MonsterUsageStrikes.Strikes.Count);
            
            var originalStrike = original.MonsterUsageStrikes.Strikes[0];
            var deserializedStrike = deserialized.MonsterUsageStrikes.Strikes[0];
            
            Assert.Equal(originalStrike.IsHeavy, deserializedStrike.IsHeavy);
            Assert.Equal(originalStrike.IsLeftStance, deserializedStrike.IsLeftStance);
            Assert.Equal(originalStrike.Direction, deserializedStrike.Direction);
            Assert.Equal(originalStrike.BodyPart, deserializedStrike.BodyPart);
            Assert.Equal(originalStrike.Impact, deserializedStrike.Impact);
            Assert.Equal(originalStrike.Action, deserializedStrike.Action);
        }

        [Fact]
        public void MonsterUsageSets_EmptyXmlHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<monster_usage_sets></monster_usage_sets>";

            var result = XmlTestUtils.Deserialize<MonsterUsageSetsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.MonsterUsageSets);
        }

        [Fact]
        public void MonsterUsageSets_ComplexStructureHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<monster_usage_sets>
    <monster_usage_set id=""horse"">
        <MonsterUsageStrikes>
            <MonsterUsageStrike is_heavy=""true"" direction=""left"" body_part=""head"" impact=""blunt"" action=""act_strike_left"" />
            <MonsterUsageStrike is_heavy=""false"" direction=""right"" body_part=""body"" impact=""cut"" action=""act_strike_right"" />
            <MonsterUsageStrike is_heavy=""true"" direction=""thrust"" body_part=""legs"" impact=""pierce"" action=""act_strike_thrust"" />
        </MonsterUsageStrikes>
    </monster_usage_set>
    <monster_usage_set id=""camel"" ladder_climb_action=""act_camel_climb"">
        <MonsterUsageStrikes>
            <MonsterUsageStrike is_heavy=""false"" direction=""left"" body_part=""head"" impact=""blunt"" action=""act_camel_strike"" />
        </MonsterUsageStrikes>
    </monster_usage_set>
</monster_usage_sets>";

            var result = XmlTestUtils.Deserialize<MonsterUsageSetsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.MonsterUsageSets.Count);
            
            var horseSet = result.MonsterUsageSets[0];
            Assert.Equal("horse", horseSet.Id);
            Assert.True(horseSet.HasMonsterUsageStrikes);
            Assert.Equal(3, horseSet.MonsterUsageStrikes.Strikes.Count);
            
            var camelSet = result.MonsterUsageSets[1];
            Assert.Equal("camel", camelSet.Id);
            Assert.Equal("act_camel_climb", camelSet.LadderClimbAction);
            Assert.True(camelSet.HasMonsterUsageStrikes);
            Assert.Single(camelSet.MonsterUsageStrikes.Strikes);
            
            var camelStrike = camelSet.MonsterUsageStrikes.Strikes[0];
            Assert.Equal("False", camelStrike.IsHeavy);
            Assert.Equal("left", camelStrike.Direction);
            Assert.Equal("head", camelStrike.BodyPart);
            Assert.Equal("blunt", camelStrike.Impact);
            Assert.Equal("act_camel_strike", camelStrike.Action);
        }
    }
}