using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Game;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemHolstersXmlTests
    {
        [Fact]
        public void ItemHolsters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base>
	<item_holsters>
		<item_holster
			id=""default""
			equip_action=""act_equip_default""
			equip_action_left_stance=""act_equip_default_left_stance""
			unequip_action=""act_unequip_default""
			unequip_action_left_stance=""act_unequip_default_left_stance""
			show_holster_when_drawn=""false""
			group_name=""default""
			holster_skeleton=""holster_skeleton""
			holster_bone=""biped_thorax""
			holster_position=""-0.440, -0.040, 0.230""
			holster_rotation_yaw_pitch_roll=""85.560, 17.520, -94.200"" />
		<item_holster
			id=""thorax""
			equip_action=""""
			equip_action_left_stance=""""
			unequip_action=""""
			unequip_action_left_stance=""""
			show_holster_when_drawn=""false""
			group_name=""thorax""
			holster_skeleton=""holster_skeleton""
			holster_bone=""biped_thorax""
			holster_position=""0.000, 0.000, 0.000""
			holster_rotation_yaw_pitch_roll=""0.000, 0.000, 0.000"" />
	</item_holsters>
</base>";

            var serializer = new XmlSerializer(typeof(ItemHolstersRoot));

            // Act
            ItemHolstersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemHolstersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemHolsters);
            Assert.NotNull(result.ItemHolsters.ItemHolster);
            Assert.Equal(2, result.ItemHolsters.ItemHolster.Length);

            var defaultHolster = result.ItemHolsters.ItemHolster[0];
            Assert.Equal("default", defaultHolster.Id);
            Assert.Equal("act_equip_default", defaultHolster.EquipAction);
            Assert.Equal("act_equip_default_left_stance", defaultHolster.EquipActionLeftStance);
            Assert.Equal("act_unequip_default", defaultHolster.UnequipAction);
            Assert.Equal("act_unequip_default_left_stance", defaultHolster.UnequipActionLeftStance);
            Assert.Equal("false", defaultHolster.ShowHolsterWhenDrawn);
            Assert.Equal("default", defaultHolster.GroupName);
            Assert.Equal("holster_skeleton", defaultHolster.HolsterSkeleton);
            Assert.Equal("biped_thorax", defaultHolster.HolsterBone);
            Assert.Equal("-0.440, -0.040, 0.230", defaultHolster.HolsterPosition);
            Assert.Equal("85.560, 17.520, -94.200", defaultHolster.HolsterRotationYawPitchRoll);
        }

        [Fact]
        public void ItemHolsters_WithBaseSet_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base>
	<item_holsters>
		<item_holster
			id=""thorax_back_bow""
			equip_action=""""
			equip_action_left_stance=""""
			unequip_action=""""
			unequip_action_left_stance=""""
			show_holster_when_drawn=""false""
			group_name=""thorax_back_bow""
			holster_skeleton=""holster_skeleton""
			base_set=""thorax""
			holster_position=""0.000, 0.000, 0.000""
			holster_rotation_yaw_pitch_roll=""0.000, 0.000, 0.000"" />
	</item_holsters>
</base>";

            var serializer = new XmlSerializer(typeof(ItemHolstersRoot));

            // Act
            ItemHolstersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemHolstersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemHolsters);
            Assert.NotNull(result.ItemHolsters.ItemHolster);
            Assert.Single(result.ItemHolsters.ItemHolster);

            var holster = result.ItemHolsters.ItemHolster[0];
            Assert.Equal("thorax_back_bow", holster.Id);
            Assert.Equal("thorax", holster.BaseSet);
            Assert.Equal("", holster.EquipAction);
            Assert.Equal("false", holster.ShowHolsterWhenDrawn);
        }

        [Fact]
        public void ItemHolsters_CanSerializeToXml()
        {
            // Arrange
            var itemHolsters = new ItemHolstersRoot
            {
                ItemHolsters = new ItemHolsters
                {
                    ItemHolster = new[]
                    {
                        new ItemHolster
                        {
                            Id = "default",
                            EquipAction = "act_equip_default",
                            EquipActionLeftStance = "act_equip_default_left_stance",
                            UnequipAction = "act_unequip_default",
                            UnequipActionLeftStance = "act_unequip_default_left_stance",
                            ShowHolsterWhenDrawn = "false",
                            GroupName = "default",
                            HolsterSkeleton = "holster_skeleton",
                            HolsterBone = "biped_thorax",
                            HolsterPosition = "-0.440, -0.040, 0.230",
                            HolsterRotationYawPitchRoll = "85.560, 17.520, -94.200"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(ItemHolstersRoot));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, itemHolsters);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("id=\"default\"", result);
            Assert.Contains("equip_action=\"act_equip_default\"", result);
            Assert.Contains("holster_position=\"-0.440, -0.040, 0.230\"", result);
            Assert.Contains("holster_rotation_yaw_pitch_roll=\"85.560, 17.520, -94.200\"", result);
        }

        [Fact]
        public void ItemHolsters_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new ItemHolstersRoot
            {
                ItemHolsters = new ItemHolsters
                {
                    ItemHolster = new[]
                    {
                        new ItemHolster
                        {
                            Id = "test_holster",
                            EquipAction = "test_action",
                            ShowHolsterWhenDrawn = "true",
                            GroupName = "test_group",
                            HolsterSkeleton = "test_skeleton",
                            BaseSet = "test_base",
                            HolsterPosition = "1.0, 2.0, 3.0",
                            HolsterRotationYawPitchRoll = "90.0, 45.0, 0.0"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(ItemHolstersRoot));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            ItemHolstersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemHolstersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemHolsters);
            Assert.NotNull(result.ItemHolsters.ItemHolster);
            Assert.Single(result.ItemHolsters.ItemHolster);

            var holster = result.ItemHolsters.ItemHolster[0];
            Assert.Equal("test_holster", holster.Id);
            Assert.Equal("test_action", holster.EquipAction);
            Assert.Equal("true", holster.ShowHolsterWhenDrawn);
            Assert.Equal("test_group", holster.GroupName);
            Assert.Equal("test_skeleton", holster.HolsterSkeleton);
            Assert.Equal("test_base", holster.BaseSet);
            Assert.Equal("1.0, 2.0, 3.0", holster.HolsterPosition);
            Assert.Equal("90.0, 45.0, 0.0", holster.HolsterRotationYawPitchRoll);
        }

        [Fact]
        public void ItemHolsters_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base>
	<item_holsters>
	</item_holsters>
</base>";

            var serializer = new XmlSerializer(typeof(ItemHolstersRoot));

            // Act
            ItemHolstersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemHolstersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemHolsters);
        }
    }
} 