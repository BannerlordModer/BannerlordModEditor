using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class BadgesXmlTests
    {
        [Fact]
        public void Badges_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Badges>
    <Badge
        id=""badge_taleworlds_primary_dev""
        type=""Custom""
        name=""{=aubhe8Hk}TaleWorlds Developer""
        description=""{=GTmPMoYe}Employee of TaleWorlds""
        is_visible_only_when_earned=""true"" />
    <Badge
        id=""badge_alpha_tester""
        type=""Custom""
        name=""{=5IANKz8a}Alpha Tester""
        description=""{=qLpxrsFX}This badge has been awarded to the players who participated in the Multiplayer Alpha Test.""
        is_visible_only_when_earned=""true"">
        <Condition
            type=""PlayerDataNumeric""
            description=""{=j7bQmC5H}Total wins"">
            <Parameter
                name=""property""
                value=""WinCount"" />
            <Parameter
                name=""min_value""
                value=""5"" />
        </Condition>
    </Badge>
</Badges>";

            // Act
            var result = XmlTestUtils.Deserialize<BadgesDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BadgeList);
            Assert.Equal(2, result.BadgeList.Count);
            
            var firstBadge = result.BadgeList[0];
            Assert.Equal("badge_taleworlds_primary_dev", firstBadge.Id);
            Assert.Equal("Custom", firstBadge.Type);
            Assert.Equal("{=aubhe8Hk}TaleWorlds Developer", firstBadge.Name);
            Assert.Equal("{=GTmPMoYe}Employee of TaleWorlds", firstBadge.Description);
            Assert.Equal("true", firstBadge.IsVisibleOnlyWhenEarned);
            Assert.Empty(firstBadge.Conditions);
            
            var secondBadge = result.BadgeList[1];
            Assert.Equal("badge_alpha_tester", secondBadge.Id);
            Assert.Equal("Custom", secondBadge.Type);
            Assert.Equal("{=5IANKz8a}Alpha Tester", secondBadge.Name);
            Assert.Equal("{=qLpxrsFX}This badge has been awarded to the players who participated in the Multiplayer Alpha Test.", secondBadge.Description);
            Assert.Equal("true", secondBadge.IsVisibleOnlyWhenEarned);
            Assert.Single(secondBadge.Conditions);
            
            var condition = secondBadge.Conditions[0];
            Assert.Equal("PlayerDataNumeric", condition.Type);
            Assert.Equal("{=j7bQmC5H}Total wins", condition.Description);
            Assert.Equal(2, condition.Parameters.Count);
            
            var firstParam = condition.Parameters[0];
            Assert.Equal("property", firstParam.Name);
            Assert.Equal("WinCount", firstParam.Value);
            
            var secondParam = condition.Parameters[1];
            Assert.Equal("min_value", secondParam.Name);
            Assert.Equal("5", secondParam.Value);
        }

        [Fact]
        public void Badges_CanSerializeToXml()
        {
            // Arrange
            var badges = new BadgesDO
            {
                BadgeList = new List<BadgeDO>
                {
                    new BadgeDO
                    {
                        Id = "badge_taleworlds_primary_dev",
                        Type = "Custom",
                        Name = "{=aubhe8Hk}TaleWorlds Developer",
                        Description = "{=GTmPMoYe}Employee of TaleWorlds",
                        IsVisibleOnlyWhenEarned = "true"
                    },
                    new BadgeDO
                    {
                        Id = "badge_alpha_tester",
                        Type = "Custom",
                        Name = "{=5IANKz8a}Alpha Tester",
                        Description = "{=qLpxrsFX}This badge has been awarded to the players who participated in the Multiplayer Alpha Test.",
                        IsVisibleOnlyWhenEarned = "true",
                        Conditions = new List<BadgeConditionDO>
                        {
                            new BadgeConditionDO
                            {
                                Type = "PlayerDataNumeric",
                                Description = "{=j7bQmC5H}Total wins",
                                Parameters = new List<BadgeParameterDO>
                                {
                                    new BadgeParameterDO
                                    {
                                        Name = "property",
                                        Value = "WinCount"
                                    },
                                    new BadgeParameterDO
                                    {
                                        Name = "min_value",
                                        Value = "5"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(badges);

            // Assert
            Assert.NotNull(serializedXml);
            Assert.Contains("Badges", serializedXml);
            Assert.Contains("id=\"badge_taleworlds_primary_dev\"", serializedXml);
            Assert.Contains("id=\"badge_alpha_tester\"", serializedXml);
            Assert.Contains("type=\"Custom\"", serializedXml);
            Assert.Contains("name=\"{=aubhe8Hk}TaleWorlds Developer\"", serializedXml);
            Assert.Contains("type=\"PlayerDataNumeric\"", serializedXml);
            Assert.Contains("name=\"property\"", serializedXml);
            Assert.Contains("value=\"WinCount\"", serializedXml);
        }

        [Fact]
        public void Badges_RoundTripSerialization()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Badges>
    <Badge
        id=""badge_taleworlds_primary_dev""
        type=""Custom""
        name=""{=aubhe8Hk}TaleWorlds Developer""
        description=""{=GTmPMoYe}Employee of TaleWorlds""
        is_visible_only_when_earned=""true"" />
</Badges>";

            // Act
            var deserialized = XmlTestUtils.Deserialize<BadgesDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(deserialized);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Badges_EmptyConditionsHandling()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Badges>
    <Badge
        id=""badge_taleworlds_primary_dev""
        type=""Custom""
        name=""{=aubhe8Hk}TaleWorlds Developer""
        description=""{=GTmPMoYe}Employee of TaleWorlds""
        is_visible_only_when_earned=""true"" />
</Badges>";

            // Act
            var result = XmlTestUtils.Deserialize<BadgesDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BadgeList);
            Assert.Single(result.BadgeList);
            
            var badge = result.BadgeList[0];
            Assert.Equal("badge_taleworlds_primary_dev", badge.Id);
            Assert.Equal("Custom", badge.Type);
            Assert.NotNull(badge.Conditions);
            Assert.Empty(badge.Conditions);
        }

        [Fact]
        public void Badges_EmptyBadgeList()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Badges />";

            // Act
            var result = XmlTestUtils.Deserialize<BadgesDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BadgeList);
            Assert.Empty(result.BadgeList);
        }

        [Fact]
        public void Badges_ConditionalBadgeWithMultipleConditions()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Badges>
    <Badge
        id=""gamescom_2020_captain_champion_party""
        type=""Conditional""
        name=""{=ekDx9ObU}Captain Champion Gamescom 2020""
        period_start=""08/26/2020 23:00:00""
        period_end=""08/27/2020 23:00:00""
        description=""{=bFZBeN2j}This badge has been awarded to the player with most Captain kills during Gamescom 2020.""
        is_visible_only_when_earned=""true"">
        <Condition
            type=""PlayerDataNumeric""
            group_type=""Solo""
            description=""{=bNEEElAA}Solo kills"">
            <Parameter
                name=""property""
                value=""Stats.Captain.KillCount"" />
            <Parameter
                name=""is_best""
                value=""true"" />
        </Condition>
        <Condition
            type=""PlayerDataNumeric""
            group_type=""Party""
            description=""{=g4BnT9UP}Party kills"">
            <Parameter
                name=""property""
                value=""Stats.Captain.KillCount"" />
            <Parameter
                name=""is_best""
                value=""true"" />
        </Condition>
    </Badge>
</Badges>";

            // Act
            var result = XmlTestUtils.Deserialize<BadgesDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BadgeList);
            Assert.Single(result.BadgeList);
            
            var badge = result.BadgeList[0];
            Assert.Equal("gamescom_2020_captain_champion_party", badge.Id);
            Assert.Equal("Conditional", badge.Type);
            Assert.Equal("08/26/2020 23:00:00", badge.PeriodStart);
            Assert.Equal("08/27/2020 23:00:00", badge.PeriodEnd);
            Assert.Equal(2, badge.Conditions.Count);
            
            var firstCondition = badge.Conditions[0];
            Assert.Equal("PlayerDataNumeric", firstCondition.Type);
            Assert.Equal("Solo", firstCondition.GroupType);
            Assert.Equal("Stats.Captain.KillCount", firstCondition.Parameters[0].Value);
            Assert.Equal("true", firstCondition.Parameters[1].Value);
            
            var secondCondition = badge.Conditions[1];
            Assert.Equal("PlayerDataNumeric", secondCondition.Type);
            Assert.Equal("Party", secondCondition.GroupType);
            Assert.Equal("Stats.Captain.KillCount", secondCondition.Parameters[0].Value);
            Assert.Equal("true", secondCondition.Parameters[1].Value);
        }
    }
}