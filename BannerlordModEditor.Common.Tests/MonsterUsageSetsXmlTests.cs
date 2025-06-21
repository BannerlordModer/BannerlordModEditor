using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;
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
<monster_usage_sets xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
	type=""monster_usage_set"">
	<monster_usage_set
		id=""human""
		hit_object_action=""act_horse_hit_object""
		hit_object_falling_action=""act_horse_hit_object_while_falling""
		rear_action=""act_horse_rear""
		rear_damaged_action=""act_horse_rear_damaged""
		ladder_climb_action=""act_climb_ladder""
		strike_ladder_action=""act_strike_ladder"">
		<monster_usage_strikes>
			<monster_usage_strike
				is_heavy=""False""
				is_left_stance=""False""
				direction=""front""
				body_part=""chest""
				impact=""1""
				action=""act_strike_chest_front"" />
			<monster_usage_strike
				is_heavy=""True""
				is_left_stance=""True""
				direction=""back""
				body_part=""head""
				impact=""3""
				action=""act_strike_head_back_left_stance"" />
		</monster_usage_strikes>
		<monster_usage_movements>
			<monster_usage_movement
				is_left_foot=""False""
				pace=""1""
				direction=""front""
				turn_direction=""none""
				action=""act_horse_forward_walk"" />
			<monster_usage_movement
				is_left_foot=""True""
				pace=""2""
				direction=""left""
				turn_direction=""left""
				action=""act_horse_forward_walk_turn_left"" />
		</monster_usage_movements>
		<monster_usage_jumps>
			<monster_usage_jump
				jump_state=""start""
				direction=""none""
				action=""act_jump"" />
			<monster_usage_jump
				jump_state=""loop""
				direction=""none""
				action=""act_jump_loop""
				is_hard=""False"" />
		</monster_usage_jumps>
	</monster_usage_set>
</monster_usage_sets>";

            var serializer = new XmlSerializer(typeof(MonsterUsageSets));

            // Act
            MonsterUsageSets? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (MonsterUsageSets?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("monster_usage_set", result.Type);
            Assert.NotNull(result.MonsterUsageSet);
            Assert.Single(result.MonsterUsageSet);

            var monsterSet = result.MonsterUsageSet[0];
            Assert.Equal("human", monsterSet.Id);
            Assert.Equal("act_horse_hit_object", monsterSet.HitObjectAction);
            Assert.Equal("act_climb_ladder", monsterSet.LadderClimbAction);

            // Test strikes
            Assert.NotNull(monsterSet.MonsterUsageStrikes);
            Assert.NotNull(monsterSet.MonsterUsageStrikes.MonsterUsageStrike);
            Assert.Equal(2, monsterSet.MonsterUsageStrikes.MonsterUsageStrike.Length);

            var strike = monsterSet.MonsterUsageStrikes.MonsterUsageStrike[0];
            Assert.Equal("False", strike.IsHeavy);
            Assert.Equal("False", strike.IsLeftStance);
            Assert.Equal("front", strike.Direction);
            Assert.Equal("chest", strike.BodyPart);
            Assert.Equal("1", strike.Impact);
            Assert.Equal("act_strike_chest_front", strike.Action);

            // Test movements
            Assert.NotNull(monsterSet.MonsterUsageMovements);
            Assert.NotNull(monsterSet.MonsterUsageMovements.MonsterUsageMovement);
            Assert.Equal(2, monsterSet.MonsterUsageMovements.MonsterUsageMovement.Length);

            var movement = monsterSet.MonsterUsageMovements.MonsterUsageMovement[0];
            Assert.Equal("False", movement.IsLeftFoot);
            Assert.Equal("1", movement.Pace);
            Assert.Equal("front", movement.Direction);
            Assert.Equal("none", movement.TurnDirection);
            Assert.Equal("act_horse_forward_walk", movement.Action);

            // Test jumps
            Assert.NotNull(monsterSet.MonsterUsageJumps);
            Assert.NotNull(monsterSet.MonsterUsageJumps.MonsterUsageJump);
            Assert.Equal(2, monsterSet.MonsterUsageJumps.MonsterUsageJump.Length);

            var jump = monsterSet.MonsterUsageJumps.MonsterUsageJump[0];
            Assert.Equal("start", jump.JumpState);
            Assert.Equal("none", jump.Direction);
            Assert.Equal("act_jump", jump.Action);
        }

        [Fact]
        public void MonsterUsageSets_WithFallsAndUpperBodyMovements_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<monster_usage_sets type=""monster_usage_set"">
	<monster_usage_set id=""test"">
		<monster_usage_falls>
			<monster_usage_fall
				is_heavy=""False""
				is_left_stance=""False""
				direction=""right""
				body_part=""none""
				death_type=""other""
				action=""act_horse_fall_left"" />
		</monster_usage_falls>
		<monster_usage_upper_body_movements>
			<monster_usage_upper_body_movement
				pace=""1""
				direction=""front""
				action=""act_horse_idle_1"" />
		</monster_usage_upper_body_movements>
	</monster_usage_set>
</monster_usage_sets>";

            var serializer = new XmlSerializer(typeof(MonsterUsageSets));

            // Act
            MonsterUsageSets? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (MonsterUsageSets?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonsterUsageSet);
            Assert.Single(result.MonsterUsageSet);

            var monsterSet = result.MonsterUsageSet[0];
            Assert.Equal("test", monsterSet.Id);

            // Test falls
            Assert.NotNull(monsterSet.MonsterUsageFalls);
            Assert.NotNull(monsterSet.MonsterUsageFalls.MonsterUsageFall);
            Assert.Single(monsterSet.MonsterUsageFalls.MonsterUsageFall);

            var fall = monsterSet.MonsterUsageFalls.MonsterUsageFall[0];
            Assert.Equal("False", fall.IsHeavy);
            Assert.Equal("right", fall.Direction);
            Assert.Equal("other", fall.DeathType);
            Assert.Equal("act_horse_fall_left", fall.Action);

            // Test upper body movements
            Assert.NotNull(monsterSet.MonsterUsageUpperBodyMovements);
            Assert.NotNull(monsterSet.MonsterUsageUpperBodyMovements.MonsterUsageUpperBodyMovement);
            Assert.Single(monsterSet.MonsterUsageUpperBodyMovements.MonsterUsageUpperBodyMovement);

            var upperBodyMovement = monsterSet.MonsterUsageUpperBodyMovements.MonsterUsageUpperBodyMovement[0];
            Assert.Equal("1", upperBodyMovement.Pace);
            Assert.Equal("front", upperBodyMovement.Direction);
            Assert.Equal("act_horse_idle_1", upperBodyMovement.Action);
        }

        [Fact]
        public void MonsterUsageSets_CanSerializeToXml()
        {
            // Arrange
            var monsterUsageSets = new MonsterUsageSets
            {
                Type = "monster_usage_set",
                MonsterUsageSet = new[]
                {
                    new MonsterUsageSet
                    {
                        Id = "test_monster",
                        HitObjectAction = "test_hit_action",
                        LadderClimbAction = "test_climb_action",
                        MonsterUsageStrikes = new MonsterUsageStrikes
                        {
                            MonsterUsageStrike = new[]
                            {
                                new MonsterUsageStrike
                                {
                                    IsHeavy = "True",
                                    Direction = "front",
                                    BodyPart = "chest",
                                    Impact = "2",
                                    Action = "test_strike_action"
                                }
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(MonsterUsageSets));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, monsterUsageSets);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("type=\"monster_usage_set\"", result);
            Assert.Contains("id=\"test_monster\"", result);
            Assert.Contains("hit_object_action=\"test_hit_action\"", result);
            Assert.Contains("is_heavy=\"True\"", result);
            Assert.Contains("action=\"test_strike_action\"", result);
        }

        [Fact]
        public void MonsterUsageSets_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new MonsterUsageSets
            {
                Type = "monster_usage_set",
                MonsterUsageSet = new[]
                {
                    new MonsterUsageSet
                    {
                        Id = "test_monster",
                        HitObjectAction = "test_action",
                        MonsterUsageMovements = new MonsterUsageMovements
                        {
                            MonsterUsageMovement = new[]
                            {
                                new MonsterUsageMovement
                                {
                                    IsLeftFoot = "True",
                                    Pace = "3",
                                    Direction = "front",
                                    TurnDirection = "left",
                                    Action = "test_movement_action"
                                }
                            }
                        },
                        MonsterUsageJumps = new MonsterUsageJumps
                        {
                            MonsterUsageJump = new[]
                            {
                                new MonsterUsageJump
                                {
                                    JumpState = "loop",
                                    Direction = "up",
                                    Action = "test_jump_action",
                                    IsHard = "False"
                                }
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(MonsterUsageSets));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            MonsterUsageSets? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (MonsterUsageSets?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("monster_usage_set", result.Type);
            Assert.NotNull(result.MonsterUsageSet);
            Assert.Single(result.MonsterUsageSet);

            var monsterSet = result.MonsterUsageSet[0];
            Assert.Equal("test_monster", monsterSet.Id);
            Assert.Equal("test_action", monsterSet.HitObjectAction);

            // Verify movements
            Assert.NotNull(monsterSet.MonsterUsageMovements);
            Assert.NotNull(monsterSet.MonsterUsageMovements.MonsterUsageMovement);
            Assert.Single(monsterSet.MonsterUsageMovements.MonsterUsageMovement);

            var movement = monsterSet.MonsterUsageMovements.MonsterUsageMovement[0];
            Assert.Equal("True", movement.IsLeftFoot);
            Assert.Equal("3", movement.Pace);
            Assert.Equal("front", movement.Direction);
            Assert.Equal("left", movement.TurnDirection);
            Assert.Equal("test_movement_action", movement.Action);

            // Verify jumps
            Assert.NotNull(monsterSet.MonsterUsageJumps);
            Assert.NotNull(monsterSet.MonsterUsageJumps.MonsterUsageJump);
            Assert.Single(monsterSet.MonsterUsageJumps.MonsterUsageJump);

            var jump = monsterSet.MonsterUsageJumps.MonsterUsageJump[0];
            Assert.Equal("loop", jump.JumpState);
            Assert.Equal("up", jump.Direction);
            Assert.Equal("test_jump_action", jump.Action);
            Assert.Equal("False", jump.IsHard);
        }

        [Fact]
        public void MonsterUsageSets_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<monster_usage_sets type=""monster_usage_set"">
</monster_usage_sets>";

            var serializer = new XmlSerializer(typeof(MonsterUsageSets));

            // Act
            MonsterUsageSets? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (MonsterUsageSets?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("monster_usage_set", result.Type);
        }
    }
} 