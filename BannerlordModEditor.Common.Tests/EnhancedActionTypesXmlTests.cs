using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class EnhancedActionTypesXmlTests
    {
        [Fact]
        public void ActionTypes_LoadWithMissingTypeField_ShouldTrackExistenceCorrectly()
        {
            // Arrange - 创建一个只有name字段的XML（没有type字段）
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""act_jump"" />
	<action name=""act_jump_loop"" type=""actt_jump_loop"" />
</action_types>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, xmlContent, Encoding.UTF8);

            // Act
            var loader = new EnhancedXmlLoader<EnhancedActionTypesList>();
            var result = loader.Load(tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Actions.Count);

            // 第一个action - 只有name字段，没有type字段
            var firstAction = result.Actions[0];
            Assert.Equal("act_jump", firstAction.Name);
            Assert.False(firstAction.TypeExistsInXml); // type字段不存在于XML中
            Assert.Null(firstAction.Type);

            // 第二个action - 有name和type字段
            var secondAction = result.Actions[1];
            Assert.Equal("act_jump_loop", secondAction.Name);
            Assert.True(secondAction.TypeExistsInXml); // type字段存在于XML中
            Assert.Equal("actt_jump_loop", secondAction.Type);

            // 清理
            File.Delete(tempFile);
        }

        [Fact]
        public void ActionTypes_LoadWithEmptyTypeField_ShouldTrackExistenceCorrectly()
        {
            // Arrange - 创建一个type字段为空的XML
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""act_jump"" type="""" />
	<action name=""act_jump_loop"" type=""actt_jump_loop"" />
</action_types>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, xmlContent, Encoding.UTF8);

            // Act
            var loader = new EnhancedXmlLoader<EnhancedActionTypesList>();
            var result = loader.Load(tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Actions.Count);

            // 第一个action - type字段存在但为空
            var firstAction = result.Actions[0];
            Assert.Equal("act_jump", firstAction.Name);
            Assert.True(firstAction.TypeExistsInXml); // type字段存在于XML中但为空
            Assert.Equal(string.Empty, firstAction.Type);

            // 第二个action - type字段存在且有值
            var secondAction = result.Actions[1];
            Assert.Equal("act_jump_loop", secondAction.Name);
            Assert.True(secondAction.TypeExistsInXml); // type字段存在于XML中
            Assert.Equal("actt_jump_loop", secondAction.Type);

            // 清理
            File.Delete(tempFile);
        }

        [Fact]
        public void ActionTypes_PropertyExistenceTracking_ShouldWorkCorrectly()
        {
            // Arrange
            var action1 = new EnhancedActionType
            {
                Name = "act_jump"
                // 不设置Type，模拟字段不存在的情况
            };

            var action2 = new EnhancedActionType
            {
                Name = "act_jump_loop",
                Type = "" // 设置为空字符串，模拟字段存在但为空的情况
            };

            var action3 = new EnhancedActionType
            {
                Name = "act_jump_end",
                Type = "actt_jump_end" // 设置值，模拟字段存在且有值的情况
            };

            // Act & Assert
            // 验证第一个action - 字段不存在
            Assert.Equal("act_jump", action1.Name);
            Assert.False(action1.TypeExistsInXml); // type字段不存在
            Assert.Null(action1.Type);

            // 验证第二个action - 字段存在但为空
            Assert.Equal("act_jump_loop", action2.Name);
            Assert.True(action2.TypeExistsInXml); // type字段存在但为空
            Assert.Equal(string.Empty, action2.Type);

            // 验证第三个action - 字段存在且有值
            Assert.Equal("act_jump_end", action3.Name);
            Assert.True(action3.TypeExistsInXml); // type字段存在且有值
            Assert.Equal("actt_jump_end", action3.Type);

            // 验证ShouldSerialize行为
            Assert.False(action1.ShouldSerializeType()); // 字段不存在，不应序列化
            Assert.False(action2.ShouldSerializeType()); // 字段存在但为空，不应序列化
            Assert.True(action3.ShouldSerializeType()); // 字段存在且有值，应序列化
        }

        [Fact]
        public void ActionTypes_ShouldSerializeBehavior_ShouldWorkCorrectly()
        {
            // Arrange
            var action = new EnhancedActionType();

            // Act & Assert - 默认情况下，Type和UsageDirection都不应该被序列化
            Assert.False(action.ShouldSerializeType());
            Assert.False(action.ShouldSerializeUsageDirection());

            // 设置Type为空字符串
            action.Type = "";
            Assert.False(action.ShouldSerializeType()); // 空字符串不应该被序列化

            // 设置Type为有效值
            action.Type = "test_type";
            Assert.True(action.ShouldSerializeType()); // 有效值应该被序列化

            // 设置UsageDirection为有效值
            action.UsageDirection = "test_direction";
            Assert.True(action.ShouldSerializeUsageDirection()); // 有效值应该被序列化

            // 清空UsageDirection
            action.UsageDirection = "";
            Assert.False(action.ShouldSerializeUsageDirection()); // 空字符串不应该被序列化
        }

        [Fact]
        public void ActionTypes_ComplexScenario_ShouldHandleMixedFieldStates()
        {
            // Arrange - 创建一个复杂的XML场景，包含各种字段状态
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
	<action name=""act_jump"" />
	<action name=""act_jump_loop"" type="""" />
	<action name=""act_jump_end"" type=""actt_jump_end"" usage_direction=""forward"" />
	<action name=""act_jump_start"" type=""actt_jump_start"" usage_direction="""" />
</action_types>";

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, xmlContent, Encoding.UTF8);

            // Act
            var loader = new EnhancedXmlLoader<EnhancedActionTypesList>();
            var result = loader.Load(tempFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Actions.Count);

            // 第一个action - 只有name
            var action1 = result.Actions[0];
            Assert.Equal("act_jump", action1.Name);
            Assert.False(action1.TypeExistsInXml);
            Assert.False(action1.UsageDirectionExistsInXml);

            // 第二个action - name和空的type
            var action2 = result.Actions[1];
            Assert.Equal("act_jump_loop", action2.Name);
            Assert.True(action2.TypeExistsInXml);
            Assert.Equal("", action2.Type);
            Assert.False(action2.UsageDirectionExistsInXml);

            // 第三个action - 所有字段都有值
            var action3 = result.Actions[2];
            Assert.Equal("act_jump_end", action3.Name);
            Assert.True(action3.TypeExistsInXml);
            Assert.Equal("actt_jump_end", action3.Type);
            Assert.True(action3.UsageDirectionExistsInXml);
            Assert.Equal("forward", action3.UsageDirection);

            // 第四个action - name, 有值的type, 空的usage_direction
            var action4 = result.Actions[3];
            Assert.Equal("act_jump_start", action4.Name);
            Assert.True(action4.TypeExistsInXml);
            Assert.Equal("actt_jump_start", action4.Type);
            Assert.True(action4.UsageDirectionExistsInXml);
            Assert.Equal("", action4.UsageDirection);

            // 清理
            File.Delete(tempFile);
        }
    }
}