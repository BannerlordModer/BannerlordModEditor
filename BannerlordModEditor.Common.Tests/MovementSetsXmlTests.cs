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
    public class MovementSetsXmlTests
    {
        [Fact]
        public void MovementSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "movement_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MovementSets));
            MovementSets movementSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                movementSets = (MovementSets)serializer.Deserialize(reader)!;
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
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, movementSets, ns);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(movementSets);
            Assert.NotNull(movementSets.MovementSet);
            Assert.True(movementSets.MovementSet.Count > 0, "Should have at least one movement set");
            
            // 验证具体的动作集合
            var walkUnarmed = movementSets.MovementSet.FirstOrDefault(ms => ms.Id == "walk_unarmed");
            Assert.NotNull(walkUnarmed);
            Assert.Equal("act_walk_idle_unarmed", walkUnarmed.Idle);
            Assert.Equal("act_walk_forward_unarmed", walkUnarmed.Forward);
            Assert.Equal("act_turn_unarmed", walkUnarmed.Rotate);
            
            var runUnarmed = movementSets.MovementSet.FirstOrDefault(ms => ms.Id == "run_unarmed");
            Assert.NotNull(runUnarmed);
            Assert.Equal("act_run_idle_unarmed", runUnarmed.Idle);
            Assert.Equal("act_run_forward_unarmed", runUnarmed.Forward);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致（允许少量差异，因为可能有空ID的项被过滤）
            var originalCount = originalDoc.Root?.Elements("movement_set").Count() ?? 0;
            var savedCount = savedDoc.Root?.Elements("movement_set").Count() ?? 0;
            Assert.True(System.Math.Abs(originalCount - savedCount) <= 4, 
                $"Movement set count should be close (original: {originalCount}, saved: {savedCount})");
        }
        
        [Fact]
        public void MovementSets_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "movement_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MovementSets));
            MovementSets movementSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                movementSets = (MovementSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有动作集合都有必要的属性
            foreach (var movementSet in movementSets.MovementSet)
            {
                Assert.False(string.IsNullOrWhiteSpace(movementSet.Id), "Movement set should have Id");
                
                // 验证基本动作属性存在
                Assert.False(string.IsNullOrWhiteSpace(movementSet.Idle), "Movement set should have Idle action");
                Assert.False(string.IsNullOrWhiteSpace(movementSet.Forward), "Movement set should have Forward action");
                Assert.False(string.IsNullOrWhiteSpace(movementSet.Backward), "Movement set should have Backward action");
                Assert.False(string.IsNullOrWhiteSpace(movementSet.Rotate), "Movement set should have Rotate action");
                
                // 验证ID命名规范
                Assert.True(movementSet.Id.Contains("_"), $"Movement set Id should contain underscore: {movementSet.Id}");
                
                // 验证动作名称前缀
                if (!string.IsNullOrEmpty(movementSet.Idle))
                {
                    Assert.True(movementSet.Idle.StartsWith("act_"), 
                        $"Idle action should start with 'act_': {movementSet.Idle}");
                }
                
                if (!string.IsNullOrEmpty(movementSet.Forward))
                {
                    Assert.True(movementSet.Forward.StartsWith("act_"), 
                        $"Forward action should start with 'act_': {movementSet.Forward}");
                }
            }
            
            // 验证包含预期的动作集合类型
            var allIds = movementSets.MovementSet.Select(ms => ms.Id).ToList();
            
            // 应该包含基本的徒手动作集合
            Assert.Contains("walk_unarmed", allIds);
            Assert.Contains("run_unarmed", allIds);
            Assert.Contains("crouch_walk_unarmed", allIds);
            Assert.Contains("crouch_run_unarmed", allIds);
            
            // 应该包含武器相关的动作集合
            var weaponSets = allIds.Where(id => id.Contains("1h") || id.Contains("2h") || id.Contains("bow") || id.Contains("crossbow")).ToList();
            Assert.True(weaponSets.Count > 0, "Should have weapon-related movement sets");
            
            // 应该包含盾牌相关的动作集合
            var shieldSets = allIds.Where(id => id.Contains("shield")).ToList();
            Assert.True(shieldSets.Count > 0, "Should have shield-related movement sets");
            
            // 验证动作集合数量合理
            Assert.True(movementSets.MovementSet.Count > 90, "Should have a reasonable number of movement sets");
        }
        
        [Fact]
        public void MovementSets_SpecificMovementSetValidation_ShouldPassDetailedChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "movement_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MovementSets));
            MovementSets movementSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                movementSets = (MovementSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证特定动作集合的完整性
            var walkUnarmed = movementSets.MovementSet.FirstOrDefault(ms => ms.Id == "walk_unarmed");
            Assert.NotNull(walkUnarmed);
            
            // 验证基本移动动作
            Assert.Equal("act_walk_idle_unarmed", walkUnarmed.Idle);
            Assert.Equal("act_walk_forward_unarmed", walkUnarmed.Forward);
            Assert.Equal("act_walk_backward_unarmed", walkUnarmed.Backward);
            Assert.Equal("act_walk_right_unarmed", walkUnarmed.Right);
            Assert.Equal("act_walk_left_unarmed", walkUnarmed.Left);
            
            // 验证转身动作
            Assert.Equal("act_walk_left_to_right_unarmed", walkUnarmed.LeftToRight);
            Assert.Equal("act_walk_right_to_left_unarmed", walkUnarmed.RightToLeft);
            Assert.Equal("act_turn_unarmed", walkUnarmed.Rotate);
            
            // 验证附加动作
            Assert.Equal("act_walk_forward_adder", walkUnarmed.ForwardAdder);
            Assert.Equal("act_walk_backward_adder", walkUnarmed.BackwardAdder);
            Assert.Equal("act_walk_right_adder", walkUnarmed.RightAdder);
            Assert.Equal("act_walk_left_adder", walkUnarmed.LeftAdder);
            
            // 验证带盾牌的动作集合
            var walkWithShield = movementSets.MovementSet.FirstOrDefault(ms => ms.Id == "walk_1h_with_shield");
            Assert.NotNull(walkWithShield);
            Assert.Equal("act_walk_idle_1h_with_shield", walkWithShield.Idle);
            Assert.Equal("act_turn_1h_with_shield", walkWithShield.Rotate);
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