using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests;

public class ActionSetsXmlTests
{
    [Fact]
    public void ActionSets_LoadAndSave_ShouldBeLogicallyIdentical()
    {
        // Arrange
        var solutionRoot = TestUtils.GetSolutionRoot();
        var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "action_sets.xml");
        
        // Act & Assert - Load the XML
        var serializer = new XmlSerializer(typeof(ActionSetsBase));
        ActionSetsBase actionSets;
        using (var reader = new StreamReader(xmlPath))
        {
            actionSets = (ActionSetsBase)serializer.Deserialize(reader)!;
        }

        // Verify basic structure
        Assert.NotNull(actionSets);
        Assert.NotNull(actionSets.ActionSets);
        Assert.NotNull(actionSets.ActionSets.ActionSet);
        Assert.Equal("action_set", actionSets.Type);
        Assert.True(actionSets.ActionSets.ActionSet.Count > 0, "Should have at least one action set");

        // Test human male combat action set
        var maleCombatActionSet = actionSets.ActionSets.ActionSet.Find(a => a.Id == "human_male_combat");
        Assert.NotNull(maleCombatActionSet);
        Assert.Equal("Human Male Combat Actions", maleCombatActionSet.Name);
        Assert.Equal("human_male", maleCombatActionSet.Skeleton);
        Assert.Equal("male_warrior", maleCombatActionSet.VoiceDefinition);

        // Verify flags
        Assert.NotNull(maleCombatActionSet.Flags);
        Assert.NotNull(maleCombatActionSet.Flags.Flag);
        Assert.True(maleCombatActionSet.Flags.Flag.Count >= 3);
        
        var canInterruptFlag = maleCombatActionSet.Flags.Flag.Find(f => f.Name == "can_interrupt");
        Assert.NotNull(canInterruptFlag);
        Assert.Equal("true", canInterruptFlag.Value);

        // Verify actions
        Assert.NotNull(maleCombatActionSet.Actions);
        Assert.NotNull(maleCombatActionSet.Actions.Action);
        Assert.True(maleCombatActionSet.Actions.Action.Count >= 6);

        // Test attack_slash_left action
        var leftSlashAction = maleCombatActionSet.Actions.Action.Find(a => a.Id == "attack_slash_left");
        Assert.NotNull(leftSlashAction);
        Assert.Equal("Left Slash Attack", leftSlashAction.Name);
        Assert.Equal("attack_slash_left_anim", leftSlashAction.Animation);
        Assert.Equal("100", leftSlashAction.Priority);
        Assert.Equal("1.2", leftSlashAction.Duration);
        Assert.Equal("0.1", leftSlashAction.BlendInPeriod);
        Assert.Equal("0.2", leftSlashAction.BlendOutPeriod);
        Assert.Equal("melee_attack", leftSlashAction.ActionType);
        Assert.Equal("left", leftSlashAction.UsageDirection);
        Assert.Equal("one_handed_sword", leftSlashAction.WeaponUsage);
        Assert.Equal("stationary", leftSlashAction.MovementType);
        Assert.Equal("sword_slash", leftSlashAction.SoundCode);
        Assert.Equal("can_chamber,directional_attack", leftSlashAction.Flags);

        // Verify action parameters
        Assert.NotNull(leftSlashAction.Parameters);
        Assert.NotNull(leftSlashAction.Parameters.Parameter);
        Assert.True(leftSlashAction.Parameters.Parameter.Count >= 3);
        
        var damageFactorParam = leftSlashAction.Parameters.Parameter.Find(p => p.Name == "damage_factor");
        Assert.NotNull(damageFactorParam);
        Assert.Equal("1.0", damageFactorParam.Value);
        Assert.Equal("float", damageFactorParam.Type);

        var speedModifierParam = leftSlashAction.Parameters.Parameter.Find(p => p.Name == "speed_modifier");
        Assert.NotNull(speedModifierParam);
        Assert.Equal("1.2", speedModifierParam.Value);
        Assert.Equal("float", speedModifierParam.Type);

        // Verify action transitions
        Assert.NotNull(leftSlashAction.Transitions);
        Assert.NotNull(leftSlashAction.Transitions.Transition);
        Assert.True(leftSlashAction.Transitions.Transition.Count >= 3);
        
        var rightSlashTransition = leftSlashAction.Transitions.Transition.Find(t => t.ToAction == "attack_slash_right");
        Assert.NotNull(rightSlashTransition);
        Assert.Equal("input_right", rightSlashTransition.Condition);
        Assert.Equal("0.8", rightSlashTransition.Probability);
        Assert.Equal("0.15", rightSlashTransition.BlendDuration);

        // Test attack_thrust action
        var thrustAction = maleCombatActionSet.Actions.Action.Find(a => a.Id == "attack_thrust");
        Assert.NotNull(thrustAction);
        Assert.Equal("Thrust Attack", thrustAction.Name);
        Assert.Equal("attack_thrust_anim", thrustAction.Animation);
        Assert.Equal("95", thrustAction.Priority);
        Assert.Equal("0.9", thrustAction.Duration);
        Assert.Equal("forward", thrustAction.UsageDirection);
        Assert.Equal("forward_step", thrustAction.MovementType);
        Assert.Equal("can_chamber,thrust_attack", thrustAction.Flags);

        // Verify thrust action parameters
        Assert.NotNull(thrustAction.Parameters);
        Assert.NotNull(thrustAction.Parameters.Parameter);
        Assert.True(thrustAction.Parameters.Parameter.Count >= 2);
        
        var armorPierceParam = thrustAction.Parameters.Parameter.Find(p => p.Name == "armor_pierce");
        Assert.NotNull(armorPierceParam);
        Assert.Equal("0.2", armorPierceParam.Value);
        Assert.Equal("float", armorPierceParam.Type);

        // Test block_left action
        var blockLeftAction = maleCombatActionSet.Actions.Action.Find(a => a.Id == "block_left");
        Assert.NotNull(blockLeftAction);
        Assert.Equal("Left Block", blockLeftAction.Name);
        Assert.Equal("block", blockLeftAction.ActionType);
        Assert.Equal("shield", blockLeftAction.WeaponUsage);
        Assert.Equal("defensive,directional_block", blockLeftAction.Flags);

        // Verify action groups
        Assert.NotNull(maleCombatActionSet.ActionGroups);
        Assert.NotNull(maleCombatActionSet.ActionGroups.ActionGroup);
        Assert.True(maleCombatActionSet.ActionGroups.ActionGroup.Count >= 2);

        // Test melee_attacks group
        var meleeAttacksGroup = maleCombatActionSet.ActionGroups.ActionGroup.Find(g => g.Id == "melee_attacks");
        Assert.NotNull(meleeAttacksGroup);
        Assert.Equal("Melee Attack Group", meleeAttacksGroup.Name);
        Assert.Equal("100", meleeAttacksGroup.Priority);

        // Verify group actions
        Assert.NotNull(meleeAttacksGroup.GroupActions);
        Assert.NotNull(meleeAttacksGroup.GroupActions.GroupAction);
        Assert.True(meleeAttacksGroup.GroupActions.GroupAction.Count >= 4);
        
        var leftSlashGroupAction = meleeAttacksGroup.GroupActions.GroupAction.Find(ga => ga.ActionId == "attack_slash_left");
        Assert.NotNull(leftSlashGroupAction);
        Assert.Equal("30", leftSlashGroupAction.Weight);
        Assert.Equal("enemy_left", leftSlashGroupAction.Condition);

        // Test human female civilian action set
        var femaleCivilianActionSet = actionSets.ActionSets.ActionSet.Find(a => a.Id == "human_female_civilian");
        Assert.NotNull(femaleCivilianActionSet);
        Assert.Equal("Human Female Civilian Actions", femaleCivilianActionSet.Name);
        Assert.Equal("human_female", femaleCivilianActionSet.Skeleton);
        Assert.Equal("female_civilian", femaleCivilianActionSet.VoiceDefinition);

        // Verify female action set flags
        Assert.NotNull(femaleCivilianActionSet.Flags);
        Assert.NotNull(femaleCivilianActionSet.Flags.Flag);
        Assert.True(femaleCivilianActionSet.Flags.Flag.Count >= 2);
        
        var peacefulFlag = femaleCivilianActionSet.Flags.Flag.Find(f => f.Name == "peaceful");
        Assert.NotNull(peacefulFlag);
        Assert.Equal("true", peacefulFlag.Value);

        // Verify female actions
        Assert.NotNull(femaleCivilianActionSet.Actions);
        Assert.NotNull(femaleCivilianActionSet.Actions.Action);
        Assert.True(femaleCivilianActionSet.Actions.Action.Count >= 3);

        // Test walk_normal action
        var walkAction = femaleCivilianActionSet.Actions.Action.Find(a => a.Id == "walk_normal");
        Assert.NotNull(walkAction);
        Assert.Equal("Normal Walk", walkAction.Name);
        Assert.Equal("movement", walkAction.ActionType);
        Assert.Equal("walking", walkAction.MovementType);
        Assert.Equal("looping,movement_action", walkAction.Flags);

        // Test conversation action
        var conversationAction = femaleCivilianActionSet.Actions.Action.Find(a => a.Id == "conversation");
        Assert.NotNull(conversationAction);
        Assert.Equal("Conversation Gesture", conversationAction.Name);
        Assert.Equal("idle_civilian", conversationAction.ContinueToAction);
        Assert.Equal("social_action", conversationAction.Flags);

        // Test serialization
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, actionSets);
            var serializedXml = stringWriter.ToString();
            Assert.Contains("action_set", serializedXml);
            Assert.Contains("human_male_combat", serializedXml);
            Assert.Contains("human_female_civilian", serializedXml);
        }
    }

    [Fact]
    public void ActionSets_ShouldHandleOptionalFields()
    {
        // Arrange
        var actionSet = new ActionSet
        {
            Id = "test_action_set",
            Name = "Test Action Set",
            Skeleton = "test_skeleton"
        };

        var actionSets = new ActionSetsBase
        {
            ActionSets = new ActionSetsContainer
            {
                ActionSet = new List<ActionSet> { actionSet }
            }
        };

        // Act & Assert - Serialize with minimal data
        var serializer = new XmlSerializer(typeof(ActionSetsBase));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, actionSets);
            var xml = stringWriter.ToString();
            
            // Should not contain null fields
            Assert.DoesNotContain("voice_definition", xml.ToLower());
            Assert.DoesNotContain("actions", xml.ToLower());
            Assert.DoesNotContain("action_groups", xml.ToLower());
        }
    }

    [Fact]
    public void ActionSets_ShouldHandleComplexActionStructure()
    {
        // Arrange
        var action = new BannerlordModEditor.Common.Models.Engine.Action
        {
            Id = "test_action",
            Name = "Test Action",
            Priority = "100",
            Parameters = new ActionParameters
            {
                Parameter = new List<ActionParameter>
                {
                    new ActionParameter { Name = "test_param", Value = "1.0", Type = "float" }
                }
            },
            Transitions = new ActionTransitions
            {
                Transition = new List<ActionTransition>
                {
                    new ActionTransition { ToAction = "next_action", Condition = "test_condition", Probability = "0.5" }
                }
            }
        };

        var actionSet = new ActionSet
        {
            Id = "test_set",
            Actions = new ActionsContainer
            {
                Action = new List<BannerlordModEditor.Common.Models.Engine.Action> { action }
            }
        };

        var actionSets = new ActionSetsBase
        {
            ActionSets = new ActionSetsContainer
            {
                ActionSet = new List<ActionSet> { actionSet }
            }
        };

        // Act & Assert
        var serializer = new XmlSerializer(typeof(ActionSetsBase));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, actionSets);
            var xml = stringWriter.ToString();
            
            // Should contain complex structure
            Assert.Contains("test_action", xml);
            Assert.Contains("parameters", xml);
            Assert.Contains("transitions", xml);
            Assert.Contains("test_param", xml);
            Assert.Contains("next_action", xml);
        }
    }
} 