using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class MovementSetsXmlTests
    {
        [Fact]
        public void Deserialize_MovementSetsXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MovementSetList);
            Assert.NotEmpty(result.MovementSetList);
            
            // 验证有100个movement_set元素
            Assert.Equal(100, result.MovementSetList.Count);
            
            // 验证第一个movement_set (walk_unarmed)
            var firstMovementSet = result.MovementSetList[0];
            Assert.Equal("walk_unarmed", firstMovementSet.Id);
            Assert.Equal("act_walk_idle_unarmed", firstMovementSet.Idle);
            Assert.Equal("act_walk_forward_unarmed", firstMovementSet.Forward);
            Assert.Equal("act_walk_backward_unarmed", firstMovementSet.Backward);
            Assert.Equal("act_walk_right_unarmed", firstMovementSet.Right);
            Assert.Equal("act_walk_right_back_unarmed", firstMovementSet.RightBack);
            Assert.Equal("act_walk_left_unarmed", firstMovementSet.Left);
            Assert.Equal("act_walk_left_back_unarmed", firstMovementSet.LeftBack);
            Assert.Equal("act_walk_left_to_right_unarmed", firstMovementSet.LeftToRight);
            Assert.Equal("act_walk_right_to_left_unarmed", firstMovementSet.RightToLeft);
            Assert.Equal("act_turn_unarmed", firstMovementSet.Rotate);
            Assert.Equal("act_walk_forward_adder", firstMovementSet.ForwardAdder);
            Assert.Equal("act_walk_backward_adder", firstMovementSet.BackwardAdder);
            Assert.Equal("act_walk_right_adder", firstMovementSet.RightAdder);
            Assert.Equal("act_walk_right_back_adder", firstMovementSet.RightBackAdder);
            Assert.Equal("act_walk_left_adder", firstMovementSet.LeftAdder);
            Assert.Equal("act_walk_left_back_adder", firstMovementSet.LeftBackAdder);
            Assert.Equal("act_walk_left_to_right_adder", firstMovementSet.LeftToRightAdder);
            Assert.Equal("act_walk_right_to_left_adder", firstMovementSet.RightToLeftAdder);
        }

        [Fact]
        public void Serialize_MovementSetsDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<MovementSetsDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_MovementSetsDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Act
            var dto = MovementSetsMapper.ToDTO(originalDO);
            var convertedDO = MovementSetsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.HasMovementSetList, convertedDO.HasMovementSetList);
            
            // 验证MovementSetList结构
            Assert.Equal(originalDO.MovementSetList.Count, convertedDO.MovementSetList.Count);
            
            // 验证第一个movement_set
            var originalMovementSet = originalDO.MovementSetList[0];
            var convertedMovementSet = convertedDO.MovementSetList[0];
            
            Assert.Equal(originalMovementSet.Id, convertedMovementSet.Id);
            Assert.Equal(originalMovementSet.Idle, convertedMovementSet.Idle);
            Assert.Equal(originalMovementSet.Forward, convertedMovementSet.Forward);
            Assert.Equal(originalMovementSet.Backward, convertedMovementSet.Backward);
            Assert.Equal(originalMovementSet.Right, convertedMovementSet.Right);
            Assert.Equal(originalMovementSet.RightBack, convertedMovementSet.RightBack);
            Assert.Equal(originalMovementSet.Left, convertedMovementSet.Left);
            Assert.Equal(originalMovementSet.LeftBack, convertedMovementSet.LeftBack);
            Assert.Equal(originalMovementSet.LeftToRight, convertedMovementSet.LeftToRight);
            Assert.Equal(originalMovementSet.RightToLeft, convertedMovementSet.RightToLeft);
            Assert.Equal(originalMovementSet.Rotate, convertedMovementSet.Rotate);
            Assert.Equal(originalMovementSet.ForwardAdder, convertedMovementSet.ForwardAdder);
            Assert.Equal(originalMovementSet.BackwardAdder, convertedMovementSet.BackwardAdder);
            Assert.Equal(originalMovementSet.RightAdder, convertedMovementSet.RightAdder);
            Assert.Equal(originalMovementSet.RightBackAdder, convertedMovementSet.RightBackAdder);
            Assert.Equal(originalMovementSet.LeftAdder, convertedMovementSet.LeftAdder);
            Assert.Equal(originalMovementSet.LeftBackAdder, convertedMovementSet.LeftBackAdder);
            Assert.Equal(originalMovementSet.LeftToRightAdder, convertedMovementSet.LeftToRightAdder);
            Assert.Equal(originalMovementSet.RightToLeftAdder, convertedMovementSet.RightToLeftAdder);
        }

        [Fact]
        public void MovementSetsDO_ShouldHandleEmptyElements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            // 验证Has属性
            Assert.Equal(result.HasMovementSetList, result.MovementSetList.Count > 0);
            
            // 验证ShouldSerialize方法
            if (result.HasMovementSetList)
            {
                Assert.True(result.ShouldSerializeMovementSetList());
            }
            else
            {
                Assert.False(result.ShouldSerializeMovementSetList());
            }
            
            // 验证第一个movement_set的ShouldSerialize方法
            var firstMovementSet = result.MovementSetList[0];
            Assert.True(firstMovementSet.ShouldSerializeId());
            Assert.True(firstMovementSet.ShouldSerializeIdle());
            Assert.True(firstMovementSet.ShouldSerializeForward());
            Assert.True(firstMovementSet.ShouldSerializeBackward());
            Assert.True(firstMovementSet.ShouldSerializeRight());
            Assert.True(firstMovementSet.ShouldSerializeRightBack());
            Assert.True(firstMovementSet.ShouldSerializeLeft());
            Assert.True(firstMovementSet.ShouldSerializeLeftBack());
            Assert.True(firstMovementSet.ShouldSerializeLeftToRight());
            Assert.True(firstMovementSet.ShouldSerializeRightToLeft());
            Assert.True(firstMovementSet.ShouldSerializeRotate());
            Assert.True(firstMovementSet.ShouldSerializeForwardAdder());
            Assert.True(firstMovementSet.ShouldSerializeBackwardAdder());
            Assert.True(firstMovementSet.ShouldSerializeRightAdder());
            Assert.True(firstMovementSet.ShouldSerializeRightBackAdder());
            Assert.True(firstMovementSet.ShouldSerializeLeftAdder());
            Assert.True(firstMovementSet.ShouldSerializeLeftBackAdder());
            Assert.True(firstMovementSet.ShouldSerializeLeftToRightAdder());
            Assert.True(firstMovementSet.ShouldSerializeRightToLeftAdder());
        }

        [Fact]
        public void MovementSetsDO_ShouldContainSpecificMovementSets()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证特定的movement_set存在
            Assert.Contains(movementSets, m => m.Id == "walk_unarmed");
            Assert.Contains(movementSets, m => m.Id == "walk_unarmed_left_stance");
            Assert.Contains(movementSets, m => m.Id == "run_unarmed");
            Assert.Contains(movementSets, m => m.Id == "run_unarmed_left_stance");
            Assert.Contains(movementSets, m => m.Id == "walk_1h");
            Assert.Contains(movementSets, m => m.Id == "walk_1h_with_shield");
            Assert.Contains(movementSets, m => m.Id == "run_1h");
            Assert.Contains(movementSets, m => m.Id == "run_1h_with_shield");
            Assert.Contains(movementSets, m => m.Id == "walk_2h");
            Assert.Contains(movementSets, m => m.Id == "run_2h");
            Assert.Contains(movementSets, m => m.Id == "crouch_walk_unarmed");
            Assert.Contains(movementSets, m => m.Id == "crouch_run_unarmed");
            
            // 验证特定movement_set的属性
            var walkUnarmed = movementSets.FirstOrDefault(m => m.Id == "walk_unarmed");
            Assert.NotNull(walkUnarmed);
            Assert.Equal("act_walk_idle_unarmed", walkUnarmed.Idle);
            Assert.Equal("act_walk_forward_unarmed", walkUnarmed.Forward);
            Assert.Equal("act_walk_backward_unarmed", walkUnarmed.Backward);
            Assert.Equal("act_walk_right_unarmed", walkUnarmed.Right);
            Assert.Equal("act_walk_right_back_unarmed", walkUnarmed.RightBack);
            Assert.Equal("act_walk_left_unarmed", walkUnarmed.Left);
            Assert.Equal("act_walk_left_back_unarmed", walkUnarmed.LeftBack);
            Assert.Equal("act_walk_left_to_right_unarmed", walkUnarmed.LeftToRight);
            Assert.Equal("act_walk_right_to_left_unarmed", walkUnarmed.RightToLeft);
            Assert.Equal("act_turn_unarmed", walkUnarmed.Rotate);
            Assert.Equal("act_walk_forward_adder", walkUnarmed.ForwardAdder);
            Assert.Equal("act_walk_backward_adder", walkUnarmed.BackwardAdder);
            Assert.Equal("act_walk_right_adder", walkUnarmed.RightAdder);
            Assert.Equal("act_walk_right_back_adder", walkUnarmed.RightBackAdder);
            Assert.Equal("act_walk_left_adder", walkUnarmed.LeftAdder);
            Assert.Equal("act_walk_left_back_adder", walkUnarmed.LeftBackAdder);
            Assert.Equal("act_walk_left_to_right_adder", walkUnarmed.LeftToRightAdder);
            Assert.Equal("act_walk_right_to_left_adder", walkUnarmed.RightToLeftAdder);
        }

        [Fact]
        public void MovementSetsDO_ShouldHandleLeftStanceVariations()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证左站姿变体存在
            var walkUnarmedLeftStance = movementSets.FirstOrDefault(m => m.Id == "walk_unarmed_left_stance");
            Assert.NotNull(walkUnarmedLeftStance);
            Assert.Equal("act_walk_idle_unarmed_left_stance", walkUnarmedLeftStance.Idle);
            Assert.Equal("act_walk_forward_unarmed_left_stance", walkUnarmedLeftStance.Forward);
            Assert.Equal("act_walk_backward_unarmed_left_stance", walkUnarmedLeftStance.Backward);
            Assert.Equal("act_walk_right_unarmed_left_stance", walkUnarmedLeftStance.Right);
            Assert.Equal("act_walk_right_back_unarmed_left_stance", walkUnarmedLeftStance.RightBack);
            Assert.Equal("act_walk_left_unarmed_left_stance", walkUnarmedLeftStance.Left);
            Assert.Equal("act_walk_left_back_unarmed_left_stance", walkUnarmedLeftStance.LeftBack);
            Assert.Equal("act_walk_left_to_right_unarmed_left_stance", walkUnarmedLeftStance.LeftToRight);
            Assert.Equal("act_walk_right_to_left_unarmed_left_stance", walkUnarmedLeftStance.RightToLeft);
            Assert.Equal("act_turn_unarmed_left_stance", walkUnarmedLeftStance.Rotate);
            
            var runUnarmedLeftStance = movementSets.FirstOrDefault(m => m.Id == "run_unarmed_left_stance");
            Assert.NotNull(runUnarmedLeftStance);
            Assert.Equal("act_run_idle_unarmed_left_stance", runUnarmedLeftStance.Idle);
            Assert.Equal("act_run_forward_unarmed_left_stance", runUnarmedLeftStance.Forward);
            Assert.Equal("act_run_backward_unarmed_left_stance", runUnarmedLeftStance.Backward);
            Assert.Equal("act_run_right_unarmed_left_stance", runUnarmedLeftStance.Right);
            Assert.Equal("act_run_right_back_unarmed_left_stance", runUnarmedLeftStance.RightBack);
            Assert.Equal("act_run_left_unarmed_left_stance", runUnarmedLeftStance.Left);
            Assert.Equal("act_run_left_back_unarmed_left_stance", runUnarmedLeftStance.LeftBack);
            Assert.Equal("act_run_left_to_right_unarmed_left_stance", runUnarmedLeftStance.LeftToRight);
            Assert.Equal("act_run_right_to_left_unarmed_left_stance", runUnarmedLeftStance.RightToLeft);
            Assert.Equal("act_turn_unarmed_left_stance", runUnarmedLeftStance.Rotate);
        }

        [Fact]
        public void MovementSetsDO_ShouldHandleWeaponMovementSets()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证武器相关的movement_set存在
            var walk1h = movementSets.FirstOrDefault(m => m.Id == "walk_1h");
            Assert.NotNull(walk1h);
            Assert.Equal("act_walk_idle_1h", walk1h.Idle);
            Assert.Equal("act_walk_forward_1h", walk1h.Forward);
            Assert.Equal("act_walk_backward_1h", walk1h.Backward);
            Assert.Equal("act_walk_right_1h", walk1h.Right);
            Assert.Equal("act_walk_right_back_1h", walk1h.RightBack);
            Assert.Equal("act_walk_left_1h", walk1h.Left);
            Assert.Equal("act_walk_left_back_1h", walk1h.LeftBack);
            Assert.Equal("act_walk_left_to_right_1h", walk1h.LeftToRight);
            Assert.Equal("act_walk_right_to_left_1h", walk1h.RightToLeft);
            Assert.Equal("act_turn_1h", walk1h.Rotate);
            
            var run2h = movementSets.FirstOrDefault(m => m.Id == "run_2h");
            Assert.NotNull(run2h);
            Assert.Equal("act_run_idle_2h", run2h.Idle);
            Assert.Equal("act_run_forward_2h", run2h.Forward);
            Assert.Equal("act_run_backward_2h", run2h.Backward);
            Assert.Equal("act_run_right_2h", run2h.Right);
            Assert.Equal("act_run_right_back_2h", run2h.RightBack);
            Assert.Equal("act_run_left_2h", run2h.Left);
            Assert.Equal("act_run_left_back_2h", run2h.LeftBack);
            Assert.Equal("act_run_left_to_right_2h", run2h.LeftToRight);
            Assert.Equal("act_run_right_to_left_2h", run2h.RightToLeft);
            Assert.Equal("act_turn_2h", run2h.Rotate);
        }

        [Fact]
        public void MovementSetsDO_ShouldHandleMovementSetCount()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证总共有100个movement_set
            Assert.Equal(100, movementSets.Count);
            
            // 验证一些特定的movement_set存在
            var expectedIds = new[]
            {
                "walk_unarmed", "walk_unarmed_left_stance", "run_unarmed", "run_unarmed_left_stance",
                "walk_1h", "walk_1h_with_shield", "run_1h", "run_1h_with_shield",
                "walk_2h", "run_2h", "walk_bow", "run_bow",
                "crouch_walk_unarmed", "crouch_run_unarmed", "crouch_walk_1h", "crouch_run_1h",
                "walk_polearm", "run_polearm", "walk_thrown", "run_thrown",
                "walk_torch", "run_torch", "walk_crossbow", "run_crossbow",
                "walk_heavy_thrown", "run_heavy_thrown", "walk_2h_axe", "run_2h_axe"
            };
            
            foreach (var expectedId in expectedIds)
            {
                Assert.Contains(movementSets, m => m.Id == expectedId);
            }
        }

        [Fact]
        public void MovementSetsDO_ShouldHaveCorrectAdderActions()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证所有movement_set都有adder动作
            foreach (var movementSet in movementSets)
            {
                // 验证所有adder动作都以"_adder"结尾
                if (movementSet.ForwardAdder != null)
                    Assert.EndsWith("_adder", movementSet.ForwardAdder);
                if (movementSet.BackwardAdder != null)
                    Assert.EndsWith("_adder", movementSet.BackwardAdder);
                if (movementSet.RightAdder != null)
                    Assert.EndsWith("_adder", movementSet.RightAdder);
                if (movementSet.RightBackAdder != null)
                    Assert.EndsWith("_adder", movementSet.RightBackAdder);
                if (movementSet.LeftAdder != null)
                    Assert.EndsWith("_adder", movementSet.LeftAdder);
                if (movementSet.LeftBackAdder != null)
                    Assert.EndsWith("_adder", movementSet.LeftBackAdder);
                if (movementSet.LeftToRightAdder != null)
                    Assert.EndsWith("_adder", movementSet.LeftToRightAdder);
                if (movementSet.RightToLeftAdder != null)
                    Assert.EndsWith("_adder", movementSet.RightToLeftAdder);
            }
            
            // 验证特定的adder动作
            var walkUnarmed = movementSets.FirstOrDefault(m => m.Id == "walk_unarmed");
            Assert.NotNull(walkUnarmed);
            Assert.Equal("act_walk_forward_adder", walkUnarmed.ForwardAdder);
            Assert.Equal("act_walk_backward_adder", walkUnarmed.BackwardAdder);
            Assert.Equal("act_walk_right_adder", walkUnarmed.RightAdder);
            Assert.Equal("act_walk_right_back_adder", walkUnarmed.RightBackAdder);
            Assert.Equal("act_walk_left_adder", walkUnarmed.LeftAdder);
            Assert.Equal("act_walk_left_back_adder", walkUnarmed.LeftBackAdder);
            Assert.Equal("act_walk_left_to_right_adder", walkUnarmed.LeftToRightAdder);
            Assert.Equal("act_walk_right_to_left_adder", walkUnarmed.RightToLeftAdder);
        }

        [Fact]
        public void MovementSetsDO_ShouldHandleOptionalAttributes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证所有movement_set都有必需的id属性
            foreach (var movementSet in movementSets)
            {
                Assert.False(string.IsNullOrEmpty(movementSet.Id));
            }
            
            // 验证所有movement_set都有idle属性
            foreach (var movementSet in movementSets)
            {
                Assert.NotNull(movementSet.Idle);
                Assert.False(string.IsNullOrEmpty(movementSet.Idle));
            }
            
            // 验证可选属性存在但不为空字符串
            foreach (var movementSet in movementSets)
            {
                if (movementSet.Forward != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.Forward));
                if (movementSet.Backward != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.Backward));
                if (movementSet.Right != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.Right));
                if (movementSet.RightBack != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.RightBack));
                if (movementSet.Left != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.Left));
                if (movementSet.LeftBack != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.LeftBack));
                if (movementSet.LeftToRight != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.LeftToRight));
                if (movementSet.RightToLeft != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.RightToLeft));
                if (movementSet.Rotate != null)
                    Assert.False(string.IsNullOrEmpty(movementSet.Rotate));
            }
        }

        [Fact]
        public void MovementSetsDO_ShouldPreserveActionNamingConvention()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "movement_sets.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<MovementSetsDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var movementSets = result.MovementSetList;
            
            // 验证动作命名约定
            foreach (var movementSet in movementSets)
            {
                // 验证idle动作以"act_"开头
                Assert.StartsWith("act_", movementSet.Idle);
                
                // 验证其他动作（如果存在）以"act_"开头
                if (movementSet.Forward != null)
                    Assert.StartsWith("act_", movementSet.Forward);
                if (movementSet.Backward != null)
                    Assert.StartsWith("act_", movementSet.Backward);
                if (movementSet.Right != null)
                    Assert.StartsWith("act_", movementSet.Right);
                if (movementSet.RightBack != null)
                    Assert.StartsWith("act_", movementSet.RightBack);
                if (movementSet.Left != null)
                    Assert.StartsWith("act_", movementSet.Left);
                if (movementSet.LeftBack != null)
                    Assert.StartsWith("act_", movementSet.LeftBack);
                if (movementSet.LeftToRight != null)
                    Assert.StartsWith("act_", movementSet.LeftToRight);
                if (movementSet.RightToLeft != null)
                    Assert.StartsWith("act_", movementSet.RightToLeft);
                if (movementSet.Rotate != null)
                    Assert.StartsWith("act_", movementSet.Rotate);
                
                // 验证adder动作以"act_"开头
                if (movementSet.ForwardAdder != null)
                    Assert.StartsWith("act_", movementSet.ForwardAdder);
                if (movementSet.BackwardAdder != null)
                    Assert.StartsWith("act_", movementSet.BackwardAdder);
                if (movementSet.RightAdder != null)
                    Assert.StartsWith("act_", movementSet.RightAdder);
                if (movementSet.RightBackAdder != null)
                    Assert.StartsWith("act_", movementSet.RightBackAdder);
                if (movementSet.LeftAdder != null)
                    Assert.StartsWith("act_", movementSet.LeftAdder);
                if (movementSet.LeftBackAdder != null)
                    Assert.StartsWith("act_", movementSet.LeftBackAdder);
                if (movementSet.LeftToRightAdder != null)
                    Assert.StartsWith("act_", movementSet.LeftToRightAdder);
                if (movementSet.RightToLeftAdder != null)
                    Assert.StartsWith("act_", movementSet.RightToLeftAdder);
            }
        }
    }
}