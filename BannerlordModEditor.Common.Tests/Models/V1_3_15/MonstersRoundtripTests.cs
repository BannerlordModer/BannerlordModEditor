using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Game;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class MonstersRoundtripTests
    {
        [Fact]
        public async Task Monsters_1_3_15_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            Assert.NotNull(monsters);
            Assert.NotNull(monsters!.MonsterList);
            Assert.NotEmpty(monsters.MonsterList);
        }

        [Fact]
        public async Task Monsters_1_3_15_Roundtrip_ShouldPreserveAllData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(originalXml);

            var serializedXml = XmlTestUtils.Serialize(monsters, originalXml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public async Task Monsters_1_3_15_ShouldHaveCorrectMonsterCount()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            Assert.NotNull(monsters);
            Assert.True(monsters!.MonsterList.Count == 1);
        }

        [Fact]
        public async Task Monsters_1_3_15_CoverCow_ShouldHaveCorrectProperties()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            var coverCow = monsters!.MonsterList[0];
            Assert.Equal("cover_cow", coverCow.Id);
            Assert.Equal("as_cow", coverCow.ActionSet);
            Assert.Equal("animals", coverCow.MonsterUsage);
            Assert.Equal("500", coverCow.Weight);
            Assert.Equal("150", coverCow.HitPoints);
            Assert.Equal("5", coverCow.NumPaces);
            Assert.Equal("0", coverCow.JumpAcceleration);
            Assert.Equal("bovine", coverCow.SoundAndCollisionInfoClass);
            Assert.Equal("1.50", coverCow.StandingEyeHeight);
            Assert.Equal("0.07, -0.05, 0.0", coverCow.EyeOffsetWrtHead);
            Assert.Equal("3.5", coverCow.JumpSpeedLimit);
            Assert.Equal("3", coverCow.FamilyType);
        }

        [Fact]
        public async Task Monsters_1_3_15_CoverCow_ShouldHaveCapsules()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            var coverCow = monsters!.MonsterList[0];
            Assert.NotNull(coverCow.Capsules);
            Assert.NotNull(coverCow.Capsules!.BodyCapsule);
            Assert.Equal("0.4", coverCow.Capsules.BodyCapsule!.Radius);
            Assert.Equal("0, 1.0, 1.15", coverCow.Capsules.BodyCapsule.Pos1);
            Assert.Equal("0, -0.57, 1.15", coverCow.Capsules.BodyCapsule.Pos2);
        }

        [Fact]
        public async Task Monsters_1_3_15_CoverCow_ShouldHaveFlags()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            var coverCow = monsters!.MonsterList[0];
            Assert.NotNull(coverCow.Flags);
            Assert.Equal("false", coverCow.Flags!.RunsAwayWhenHit);
            Assert.Equal("false", coverCow.Flags.CanWander);
            Assert.Equal("false", coverCow.Flags.MoveAsHerd);
            Assert.Equal("false", coverCow.Flags.MoveForwardOnly);
        }

        [Fact]
        public async Task Monsters_1_3_15_CoverCow_ShouldHaveRagdollBones()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "monsters.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var monsters = XmlTestUtils.Deserialize<MonstersDO>(xmlContent);

            var coverCow = monsters!.MonsterList[0];
            Assert.Equal("pelvis_0", coverCow.RagdollBoneToCheckForCorpses0);
            Assert.Equal("bn_spine_02_13", coverCow.RagdollBoneToCheckForCorpses1);
            Assert.Equal("bn_spine_03_14", coverCow.RagdollBoneToCheckForCorpses2);
            Assert.Equal("bn_neck_01_25", coverCow.RagdollBoneToCheckForCorpses3);
            Assert.Equal("bn_head_27", coverCow.RagdollBoneToCheckForCorpses4);
            
            Assert.Equal("pelvis_0", coverCow.RagdollFallSoundBone0);
            Assert.Equal("bn_spine_02_13", coverCow.RagdollFallSoundBone1);
            Assert.Equal("bn_neck_01_25", coverCow.RagdollFallSoundBone2);
            Assert.Equal("bn_head_27", coverCow.RagdollFallSoundBone3);
            
            Assert.Equal("bn_head_27", coverCow.HeadLookDirectionBone);
            Assert.Equal("bn_spine_03_14", coverCow.ThoraxLookDirectionBone);
            Assert.Equal("bn_neck_01_25", coverCow.NeckRootBone);
            Assert.Equal("pelvis_0", coverCow.PelvisBone);
            Assert.Equal("pelvis_0", coverCow.FallBlowDamageBone);
        }
    }
}
