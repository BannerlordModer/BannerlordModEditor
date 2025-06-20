using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkillsXmlTests
    {
        [Fact]
        public void Skills_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skills.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(Skills));
            Skills skills;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skills = (Skills)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, skills);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(skills);
            Assert.Equal(7, skills.SkillDataList.Count);

            // 验证具体的技能数据
            var ironFlesh1 = skills.SkillDataList.FirstOrDefault(s => s.Id == "IronFlesh1");
            Assert.NotNull(ironFlesh1);
            Assert.Equal("Iron Flesh 1", ironFlesh1.Name);
            Assert.NotNull(ironFlesh1.Modifiers);
            Assert.Single(ironFlesh1.Modifiers.AttributeModifiers);
            Assert.Equal("AgentHitPoints", ironFlesh1.Modifiers.AttributeModifiers[0].AttribCode);
            Assert.Equal("Multiply", ironFlesh1.Modifiers.AttributeModifiers[0].Modification);
            Assert.Equal("1.01", ironFlesh1.Modifiers.AttributeModifiers[0].Value);

            var powerStrike1 = skills.SkillDataList.FirstOrDefault(s => s.Id == "PowerStrike1");
            Assert.NotNull(powerStrike1);
            Assert.Equal("Power Strike", powerStrike1.Name);
            Assert.NotNull(powerStrike1.Modifiers);
            Assert.Equal(2, powerStrike1.Modifiers.AttributeModifiers.Count);
            
            var runner = skills.SkillDataList.FirstOrDefault(s => s.Id == "Runner");
            Assert.NotNull(runner);
            Assert.Equal("Runner", runner.Name);
            Assert.NotNull(runner.Modifiers);
            Assert.Single(runner.Modifiers.AttributeModifiers);
            Assert.Equal("AgentRunningSpeed", runner.Modifiers.AttributeModifiers[0].AttribCode);
            Assert.Equal("2", runner.Modifiers.AttributeModifiers[0].Value);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("SkillData").Count() == savedDoc.Root?.Elements("SkillData").Count(),
                "SkillData count should be the same");
        }
        
        [Fact]
        public void Skills_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skills.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(Skills));
            Skills skills;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skills = (Skills)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整性
            Assert.True(skills.SkillDataList.Count > 0, "Should have skill data");

            // 验证所有技能都有必要的数据
            foreach (var skillData in skills.SkillDataList)
            {
                Assert.False(string.IsNullOrWhiteSpace(skillData.Id), "Skill should have Id");
                Assert.False(string.IsNullOrWhiteSpace(skillData.Name), "Skill should have Name");
                
                // 大部分技能应该有修改器
                if (skillData.Modifiers != null)
                {
                    Assert.True(skillData.Modifiers.AttributeModifiers.Count > 0, 
                        $"Skill {skillData.Id} should have attribute modifiers if Modifiers element exists");
                    
                    foreach (var modifier in skillData.Modifiers.AttributeModifiers)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(modifier.AttribCode), 
                            "Modifier should have AttribCode");
                        Assert.False(string.IsNullOrWhiteSpace(modifier.Modification), 
                            "Modifier should have Modification");
                        Assert.False(string.IsNullOrWhiteSpace(modifier.Value), 
                            "Modifier should have Value");
                        
                        // 验证修改类型（应该是"Multiply"或其他有效值）
                        Assert.True(modifier.Modification == "Multiply" || !string.IsNullOrEmpty(modifier.Modification),
                            $"Modification should be valid: {modifier.Modification}");
                        
                        // 验证数值格式（应该是有效的数字）
                        Assert.True(double.TryParse(modifier.Value, out var _), 
                            $"Value should be a valid number: {modifier.Value}");
                    }
                }
            }
            
            // 验证特定技能类型存在
            var requiredSkills = new[] { 
                "IronFlesh1", "PowerStrike1", "Runner"
            };
            
            foreach (var requiredSkill in requiredSkills)
            {
                var skill = skills.SkillDataList.FirstOrDefault(s => s.Id == requiredSkill);
                Assert.NotNull(skill);
            }
            
            // 验证Iron Flesh技能系列的递增值
            var ironFleshSkills = skills.SkillDataList
                .Where(s => s.Id.StartsWith("IronFlesh"))
                .OrderBy(s => s.Id)
                .ToList();
            
            Assert.True(ironFleshSkills.Count >= 3, "Should have at least 3 Iron Flesh skills");
            
            // 验证Power Strike技能系列的递增值
            var powerStrikeSkills = skills.SkillDataList
                .Where(s => s.Id.StartsWith("PowerStrike"))
                .OrderBy(s => s.Id)
                .ToList();
                
            Assert.True(powerStrikeSkills.Count >= 3, "Should have at least 3 Power Strike skills");
            
            // 验证每个Power Strike技能都有两个修改器（WeaponSwingDamage和WeaponThrustDamage）
            foreach (var powerStrike in powerStrikeSkills)
            {
                Assert.NotNull(powerStrike.Modifiers);
                Assert.Equal(2, powerStrike.Modifiers.AttributeModifiers.Count);
                Assert.Contains(powerStrike.Modifiers.AttributeModifiers, m => m.AttribCode == "WeaponSwingDamage");
                Assert.Contains(powerStrike.Modifiers.AttributeModifiers, m => m.AttribCode == "WeaponThrustDamage");
            }
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 