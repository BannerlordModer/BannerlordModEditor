using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MonstersXmlTests
    {
        [Fact]
        public void Monsters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Monsters>
    <Monster
        id=""human""
        action_set=""as_human_warrior""
        female_action_set=""as_human_female_warrior""
        monster_usage=""human""
        weight=""80""
        hit_points=""100""
        absorbed_damage_ratio=""1.0""
        walking_speed_limit=""1.8""
        crouch_walking_speed_limit=""1.3""
        jump_acceleration=""4.0""
        sound_and_collision_info_class=""human""
        standing_chest_height=""1.31""
        standing_pelvis_height=""0.91""
        standing_eye_height=""1.70""
        crouch_eye_height=""1.10""
        mounted_eye_height=""0.75""
        eye_offset_wrt_head=""0.13, 0.1, 0.0""
        first_person_camera_offset_wrt_head=""0.136, 0.1, 0.0""
        arm_length=""0.9""
        arm_weight=""5.0""
        jump_speed_limit=""0""
        family_type=""0""
        ragdoll_bone_to_check_for_corpses_0=""head""
        ragdoll_bone_to_check_for_corpses_1=""neck""
        ragdoll_fall_sound_bone_0=""spine2""
        ragdoll_fall_sound_bone_1=""l_upperarm_twist""
        head_look_direction_bone=""head""
        spine_lower_bone=""spine""
        spine_upper_bone=""spine1"">
        <Flags
            CanAttack=""true""
            CanDefend=""true""
            CanKick=""true""
            CanBeCharged=""true""
            CanClimbLadders=""true""
            CanBeInGroup=""true""
            CanSprint=""true""
            IsHumanoid=""true""
            CanRide=""true""
            CanWieldWeapon=""true""
            CanCrouch=""true""
            CanRetreat=""true"" />
    </Monster>
</Monsters>";

            // Act
            var result = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonsterList);
            Assert.Single(result.MonsterList);
            
            var monster = result.MonsterList[0];
            Assert.Equal("human", monster.Id);
            Assert.Equal("as_human_warrior", monster.ActionSet);
            Assert.Equal("as_human_female_warrior", monster.FemaleActionSet);
            Assert.Equal("human", monster.MonsterUsage);
            Assert.Equal("80", monster.Weight);
            Assert.Equal("100", monster.HitPoints);
            Assert.Equal("1.0", monster.AbsorbedDamageRatio);
            Assert.Equal("1.8", monster.WalkingSpeedLimit);
            Assert.Equal("1.3", monster.CrouchWalkingSpeedLimit);
            Assert.Equal("4.0", monster.JumpAcceleration);
            Assert.Equal("human", monster.SoundAndCollisionInfoClass);
            Assert.Equal("1.31", monster.StandingChestHeight);
            Assert.Equal("0.91", monster.StandingPelvisHeight);
            Assert.Equal("1.70", monster.StandingEyeHeight);
            Assert.Equal("1.10", monster.CrouchEyeHeight);
            Assert.Equal("0.75", monster.MountedEyeHeight);
            Assert.Equal("0.13, 0.1, 0.0", monster.EyeOffsetWrtHead);
            Assert.Equal("0.136, 0.1, 0.0", monster.FirstPersonCameraOffsetWrtHead);
            Assert.Equal("0.9", monster.ArmLength);
            Assert.Equal("5.0", monster.ArmWeight);
            Assert.Equal("0", monster.JumpSpeedLimit);
            Assert.Equal("0", monster.FamilyType);
            Assert.Equal("head", monster.RagdollBoneToCheckForCorpses0);
            Assert.Equal("neck", monster.RagdollBoneToCheckForCorpses1);
            Assert.Equal("spine2", monster.RagdollFallSoundBone0);
            Assert.Equal("l_upperarm_twist", monster.RagdollFallSoundBone1);
            Assert.Equal("head", monster.HeadLookDirectionBone);
            Assert.Equal("spine", monster.SpineLowerBone);
            Assert.Equal("spine1", monster.SpineUpperBone);
            
            Assert.NotNull(monster.Flags);
            Assert.Equal("true", monster.Flags.CanAttack);
            Assert.Equal("true", monster.Flags.CanDefend);
            Assert.Equal("true", monster.Flags.CanKick);
            Assert.Equal("true", monster.Flags.CanBeCharged);
            Assert.Equal("true", monster.Flags.CanClimbLadders);
            Assert.Equal("true", monster.Flags.CanBeInGroup);
            Assert.Equal("true", monster.Flags.CanSprint);
            Assert.Equal("true", monster.Flags.IsHumanoid);
            Assert.Equal("true", monster.Flags.CanRide);
            Assert.Equal("true", monster.Flags.CanWieldWeapon);
            Assert.Equal("true", monster.Flags.CanCrouch);
            Assert.Equal("true", monster.Flags.CanRetreat);
        }

        [Fact]
        public void Monsters_CanSerializeToXml()
        {
            // Arrange
            var monsters = new MonstersDO
            {
                MonsterList = new List<MonsterDO>
                {
                    new MonsterDO
                    {
                        Id = "human",
                        ActionSet = "as_human_warrior",
                        FemaleActionSet = "as_human_female_warrior",
                        MonsterUsage = "human",
                        Weight = "80",
                        HitPoints = "100",
                        AbsorbedDamageRatio = "1.0",
                        WalkingSpeedLimit = "1.8",
                        StandingEyeHeight = "1.70",
                        FamilyType = "0",
                        Flags = new MonsterFlagsDO
                        {
                            CanAttack = "true",
                            CanDefend = "true",
                            IsHumanoid = "true",
                            CanRide = "true"
                        }
                    }
                }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(monsters);

            // Assert
            Assert.NotNull(serializedXml);
            Assert.Contains("Monsters", serializedXml);
            Assert.Contains("id=\"human\"", serializedXml);
            Assert.Contains("action_set=\"as_human_warrior\"", serializedXml);
            Assert.Contains("monster_usage=\"human\"", serializedXml);
            Assert.Contains("weight=\"80\"", serializedXml);
            Assert.Contains("hit_points=\"100\"", serializedXml);
            Assert.Contains("CanAttack=\"true\"", serializedXml);
            Assert.Contains("IsHumanoid=\"true\"", serializedXml);
        }

        [Fact]
        public void Monsters_RoundTripSerialization()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Monsters>
    <Monster
        id=""human""
        action_set=""as_human_warrior""
        monster_usage=""human""
        weight=""80""
        hit_points=""100""
        family_type=""0"">
        <Flags
            CanAttack=""true""
            CanDefend=""true""
            IsHumanoid=""true"" />
    </Monster>
</Monsters>";

            // Act
            var deserialized = XmlTestUtils.Deserialize<MonstersDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(deserialized);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Monsters_EmptyFlagsHandling()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Monsters>
    <Monster
        id=""human""
        action_set=""as_human_warrior""
        monster_usage=""human"" />
</Monsters>";

            // Act
            var result = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonsterList);
            Assert.Single(result.MonsterList);
            
            var monster = result.MonsterList[0];
            Assert.Equal("human", monster.Id);
            Assert.Equal("as_human_warrior", monster.ActionSet);
            Assert.Equal("human", monster.MonsterUsage);
            Assert.Null(monster.Flags); // Flags should be null when not present
        }

        [Fact]
        public void Monsters_EmptyMonsterList()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Monsters />";

            // Act
            var result = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonsterList);
            Assert.Empty(result.MonsterList);
        }
    }
}