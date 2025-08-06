using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Text;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class GogAchievementDataXmlTests
    {
        [Fact]
        public void GogAchievementData_LoadAndSave_ShouldPreserveData()
        {
            // Arrange - 使用测试数据文件
            var solutionRoot = TestUtils.GetSolutionRoot();
            var testFile = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "AchievementData", "gog_achievement_data.xml");
            
            // 确保测试文件存在
            Assert.True(File.Exists(testFile), "Test file should exist");

            // Act - 加载XML
            var loader = new EnhancedXmlLoader<GogAchievements>();
            var model = loader.Load(testFile);
            
            Assert.NotNull(model);
            Assert.NotNull(model.Achievements);
            Assert.True(model.Achievements.Count > 0);

            // 验证第一个成就
            var firstAchievement = model.Achievements[0];
            Assert.Equal("Entrepreneur", firstAchievement.Name);
            Assert.True(firstAchievement.NameExistsInXml);
            Assert.True(firstAchievement.RequirementsExistsInXml);
            
            // 验证要求
            Assert.NotNull(firstAchievement.Requirements);
            Assert.Single(firstAchievement.Requirements);
            
            var requirement = firstAchievement.Requirements[0];
            Assert.Equal("HasOwnedCaravanAndWorkshop", requirement.StatName);
            Assert.True(requirement.StatNameExistsInXml);
            Assert.Equal("1", requirement.Threshold);
            Assert.True(requirement.ThresholdExistsInXml);

            // 验证第二个成就
            var secondAchievement = model.Achievements[1];
            Assert.Equal("Apple_of_my_eye", secondAchievement.Name);
            Assert.True(secondAchievement.NameExistsInXml);
            Assert.True(secondAchievement.RequirementsExistsInXml);
            
            // 验证要求
            Assert.NotNull(secondAchievement.Requirements);
            Assert.Single(secondAchievement.Requirements);
            
            var secondRequirement = secondAchievement.Requirements[0];
            Assert.Equal("NumberOfChildrenBorn", secondRequirement.StatName);
            Assert.True(secondRequirement.StatNameExistsInXml);
            Assert.Equal("1", secondRequirement.Threshold);
            Assert.True(secondRequirement.ThresholdExistsInXml);

            // 保存并重新加载验证
            var tempFile = Path.GetTempFileName();
            loader.Save(model, tempFile);
            
            var reloadedModel = loader.Load(tempFile);
            Assert.NotNull(reloadedModel);
            Assert.Equal(model.Achievements.Count, reloadedModel.Achievements.Count);
            
            // 验证数据一致性
            for (int i = 0; i < model.Achievements.Count; i++)
            {
                var original = model.Achievements[i];
                var reloaded = reloadedModel.Achievements[i];
                
                Assert.Equal(original.Name, reloaded.Name);
                Assert.Equal(original.NameExistsInXml, reloaded.NameExistsInXml);
                
                if (original.Requirements != null && reloaded.Requirements != null)
                {
                    Assert.Equal(original.Requirements.Count, reloaded.Requirements.Count);
                    for (int j = 0; j < original.Requirements.Count; j++)
                    {
                        var origReq = original.Requirements[j];
                        var reloadReq = reloaded.Requirements[j];
                        
                        Assert.Equal(origReq.StatName, reloadReq.StatName);
                        Assert.Equal(origReq.Threshold, reloadReq.Threshold);
                        Assert.Equal(origReq.StatNameExistsInXml, reloadReq.StatNameExistsInXml);
                        Assert.Equal(origReq.ThresholdExistsInXml, reloadReq.ThresholdExistsInXml);
                    }
                }
            }

            // 清理
            File.Delete(tempFile);
        }

        [Fact]
        public void GogAchievementData_CreateObjectsDirectly_ShouldTrackExistenceCorrectly()
        {
            // Arrange - 直接创建对象来测试字段存在性追踪
            var achievements = new GogAchievements();
            
            var achievement1 = new GogAchievement
            {
                Name = "TestAchievement"
            };

            var requirements = new GogRequirements();
            var requirement = new GogRequirement
            {
                StatName = "TestStat",
                Threshold = "5"
            };
            requirements.Add(requirement);
            achievement1.Requirements = requirements;

            achievements.Achievements.Add(achievement1);

            // Act & Assert
            Assert.Single(achievements.Achievements);
            
            var achievement = achievements.Achievements[0];
            Assert.Equal("TestAchievement", achievement.Name);
            Assert.True(achievement.NameExistsInXml); // Name字段存在
            Assert.True(achievement.RequirementsExistsInXml); // Requirements字段存在
            
            // 验证要求
            Assert.NotNull(achievement.Requirements);
            Assert.Single(achievement.Requirements);
            
            var req = achievement.Requirements[0];
            Assert.Equal("TestStat", req.StatName);
            Assert.True(req.StatNameExistsInXml);
            Assert.Equal("5", req.Threshold);
            Assert.True(req.ThresholdExistsInXml);
        }
    }
}