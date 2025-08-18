using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkillsXmlTests
    {
        [Fact]
        public void Skills_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ArrayOfSkillData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <SkillData
        id=""IronFlesh1""
        Name=""Iron Flesh 1"">
        <Modifiers>
            <AttributeModifier
                AttribCode=""AgentHitPoints""
                Modification=""Multiply""
                Value=""1.01"" />
        </Modifiers>
        <Documentation>Iron flesh increases hit points</Documentation>
    </SkillData>
    <SkillData
        id=""PowerStrike1""
        Name=""Power Strike"">
        <Modifiers>
            <AttributeModifier
                AttribCode=""WeaponSwingDamage""
                Modification=""Multiply""
                Value=""1.08"" />
            <AttributeModifier
                AttribCode=""WeaponThrustDamage""
                Modification=""Multiply""
                Value=""1.08"" />
        </Modifiers>
        <Documentation>Power Strike increases melee damage</Documentation>
    </SkillData>
    <SkillData
        id=""Runner""
        Name=""Runner"">
        <Modifiers>
            <AttributeModifier
                AttribCode=""AgentRunningSpeed""
                Modification=""Multiply""
                Value=""2"" />
        </Modifiers>
        <Documentation>Runner increases running speed</Documentation>
    </SkillData>
</ArrayOfSkillData>";

            // Act
            var result = XmlTestUtils.Deserialize<SkillsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SkillDataList);
            Assert.Equal(3, result.SkillDataList.Count);
            
            var ironFlesh = result.SkillDataList[0];
            Assert.Equal("IronFlesh1", ironFlesh.Id);
            Assert.Equal("Iron Flesh 1", ironFlesh.Name);
            Assert.True(ironFlesh.HasModifiers);
            Assert.NotNull(ironFlesh.Modifiers);
            Assert.Single(ironFlesh.Modifiers.AttributeModifiers);
            Assert.Equal("AgentHitPoints", ironFlesh.Modifiers.AttributeModifiers[0].AttribCode);
            Assert.Equal("Multiply", ironFlesh.Modifiers.AttributeModifiers[0].Modification);
            Assert.Equal("1.01", ironFlesh.Modifiers.AttributeModifiers[0].Value);
            Assert.Equal("Iron flesh increases hit points", ironFlesh.Documentation);
            
            var powerStrike = result.SkillDataList[1];
            Assert.Equal("PowerStrike1", powerStrike.Id);
            Assert.Equal("Power Strike", powerStrike.Name);
            Assert.True(powerStrike.HasModifiers);
            Assert.NotNull(powerStrike.Modifiers);
            Assert.Equal(2, powerStrike.Modifiers.AttributeModifiers.Count);
            Assert.Equal("WeaponSwingDamage", powerStrike.Modifiers.AttributeModifiers[0].AttribCode);
            Assert.Equal("WeaponThrustDamage", powerStrike.Modifiers.AttributeModifiers[1].AttribCode);
            
            var runner = result.SkillDataList[2];
            Assert.Equal("Runner", runner.Id);
            Assert.Equal("Runner", runner.Name);
            Assert.True(runner.HasModifiers);
            Assert.NotNull(runner.Modifiers);
            Assert.Single(runner.Modifiers.AttributeModifiers);
            Assert.Equal("AgentRunningSpeed", runner.Modifiers.AttributeModifiers[0].AttribCode);
            Assert.Equal("2", runner.Modifiers.AttributeModifiers[0].Value);
        }

        [Fact]
        public void Skills_CanSerializeToXml()
        {
            // Arrange
            var skills = new SkillsDO
            {
                SkillDataList = new List<SkillDataDO>
                {
                    new SkillDataDO
                    {
                        Id = "IronFlesh1",
                        Name = "Iron Flesh 1",
                        HasModifiers = true,
                        Modifiers = new ModifiersDO
                        {
                            AttributeModifiers = new List<AttributeModifierDO>
                            {
                                new AttributeModifierDO
                                {
                                    AttribCode = "AgentHitPoints",
                                    Modification = "Multiply",
                                    Value = "1.01"
                                }
                            }
                        },
                        Documentation = "Iron flesh increases hit points"
                    },
                    new SkillDataDO
                    {
                        Id = "Runner",
                        Name = "Runner",
                        HasModifiers = true,
                        Modifiers = new ModifiersDO
                        {
                            AttributeModifiers = new List<AttributeModifierDO>
                            {
                                new AttributeModifierDO
                                {
                                    AttribCode = "AgentRunningSpeed",
                                    Modification = "Multiply",
                                    Value = "2"
                                }
                            }
                        },
                        Documentation = "Runner increases running speed"
                    }
                }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(skills);

            // Assert
            Assert.NotNull(serializedXml);
            Assert.Contains("ArrayOfSkillData", serializedXml);
            Assert.Contains("id=\"IronFlesh1\"", serializedXml);
            Assert.Contains("Name=\"Iron Flesh 1\"", serializedXml);
            Assert.Contains("id=\"Runner\"", serializedXml);
            Assert.Contains("AttribCode=\"AgentHitPoints\"", serializedXml);
            Assert.Contains("AttribCode=\"AgentRunningSpeed\"", serializedXml);
            
            // 检查Documentation元素是否存在
            Assert.Contains("<Documentation>Iron flesh increases hit points</Documentation>", serializedXml);
        }

        [Fact]
        public void Skills_RoundTripSerialization()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ArrayOfSkillData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <SkillData
        id=""IronFlesh1""
        Name=""Iron Flesh 1"">
        <Modifiers>
            <AttributeModifier
                AttribCode=""AgentHitPoints""
                Modification=""Multiply""
                Value=""1.01"" />
        </Modifiers>
        <Documentation>Iron flesh increases hit points</Documentation>
    </SkillData>
</ArrayOfSkillData>";

            // Act
            var deserialized = XmlTestUtils.Deserialize<SkillsDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(deserialized);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Skills_EmptySkillList()
        {
            // Arrange
            var xmlContent = @"<ArrayOfSkillData />";

            // Act
            var result = XmlTestUtils.Deserialize<SkillsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SkillDataList);
            Assert.Empty(result.SkillDataList);
        }

        [Fact]
        public void Skills_SkillWithoutModifiers()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ArrayOfSkillData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <SkillData
        id=""BasicSkill""
        Name=""Basic Skill"">
        <Documentation>Basic skill without modifiers</Documentation>
    </SkillData>
</ArrayOfSkillData>";

            // Act
            var result = XmlTestUtils.Deserialize<SkillsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.SkillDataList);
            
            var skill = result.SkillDataList[0];
            Assert.Equal("BasicSkill", skill.Id);
            Assert.Equal("Basic Skill", skill.Name);
            Assert.False(skill.HasModifiers);
            Assert.Null(skill.Modifiers);
            Assert.Equal("Basic skill without modifiers", skill.Documentation);
        }

        [Fact]
        public void Skills_SkillWithEmptyModifiers()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ArrayOfSkillData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <SkillData
        id=""EmptyModifiersSkill""
        Name=""Empty Modifiers Skill"">
        <Modifiers />
        <Documentation>Skill with empty modifiers</Documentation>
    </SkillData>
</ArrayOfSkillData>";

            // Act
            var result = XmlTestUtils.Deserialize<SkillsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.SkillDataList);
            
            var skill = result.SkillDataList[0];
            Assert.Equal("EmptyModifiersSkill", skill.Id);
            Assert.Equal("Empty Modifiers Skill", skill.Name);
            Assert.True(skill.HasModifiers);
            Assert.NotNull(skill.Modifiers);
            Assert.Empty(skill.Modifiers.AttributeModifiers);
            Assert.Equal("Skill with empty modifiers", skill.Documentation);
        }

        [Fact]
        public void Skills_ComplexModifiers()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ArrayOfSkillData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <SkillData
        id=""ComplexSkill""
        Name=""Complex Skill"">
        <Modifiers>
            <AttributeModifier
                AttribCode=""AgentHitPoints""
                Modification=""Multiply""
                Value=""1.05"" />
            <AttributeModifier
                AttribCode=""WeaponSwingDamage""
                Modification=""Add""
                Value=""10"" />
            <AttributeModifier
                AttribCode=""AgentRunningSpeed""
                Modification=""Multiply""
                Value=""1.2"" />
        </Modifiers>
        <Documentation>Complex skill with multiple modifiers</Documentation>
    </SkillData>
</ArrayOfSkillData>";

            // Act
            var result = XmlTestUtils.Deserialize<SkillsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.SkillDataList);
            
            var skill = result.SkillDataList[0];
            Assert.Equal("ComplexSkill", skill.Id);
            Assert.Equal("Complex Skill", skill.Name);
            Assert.True(skill.HasModifiers);
            Assert.NotNull(skill.Modifiers);
            Assert.Equal(3, skill.Modifiers.AttributeModifiers.Count);
            
            var modifiers = skill.Modifiers.AttributeModifiers;
            Assert.Contains(modifiers, m => m.AttribCode == "AgentHitPoints" && m.Modification == "Multiply" && m.Value == "1.05");
            Assert.Contains(modifiers, m => m.AttribCode == "WeaponSwingDamage" && m.Modification == "Add" && m.Value == "10");
            Assert.Contains(modifiers, m => m.AttribCode == "AgentRunningSpeed" && m.Modification == "Multiply" && m.Value == "1.2");
        }

        [Fact]
        public void Skills_SerializationControl_HandlesEmptyModifiers()
        {
            // Arrange
            var skill = new SkillDataDO
            {
                Id = "TestSkill",
                Name = "Test Skill",
                HasModifiers = true,
                Modifiers = new ModifiersDO
                {
                    AttributeModifiers = new List<AttributeModifierDO>()
                },
                Documentation = "Test documentation"
            };

            var skills = new SkillsDO
            {
                SkillDataList = new List<SkillDataDO> { skill }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(skills);

            // Assert
            Assert.NotNull(serializedXml);
            // 当HasModifiers为true但AttributeModifiers为空时，应该序列化空的Modifiers元素
            Assert.Contains("<Modifiers />", serializedXml);
        }

        [Fact]
        public void Skills_SerializationControl_SkipsModifiersWhenHasModifiersIsFalse()
        {
            // Arrange
            var skill = new SkillDataDO
            {
                Id = "TestSkill",
                Name = "Test Skill",
                HasModifiers = false,
                Modifiers = new ModifiersDO
                {
                    AttributeModifiers = new List<AttributeModifierDO>
                    {
                        new AttributeModifierDO
                        {
                            AttribCode = "TestCode",
                            Modification = "Multiply",
                            Value = "1.0"
                        }
                    }
                },
                Documentation = "Test documentation"
            };

            var skills = new SkillsDO
            {
                SkillDataList = new List<SkillDataDO> { skill }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(skills);

            // Assert
            Assert.NotNull(serializedXml);
            // 当HasModifiers为false时，即使Modifiers有数据也不应该序列化
            Assert.DoesNotContain("<Modifiers>", serializedXml);
            Assert.DoesNotContain("TestCode", serializedXml);
        }
    }
}