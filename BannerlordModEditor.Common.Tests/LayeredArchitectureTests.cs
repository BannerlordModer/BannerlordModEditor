using System;
using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests
{
    public class LayeredArchitectureTests
    {
        private readonly ITestOutputHelper _output;

        public LayeredArchitectureTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DO_Layer_Should_Serialize_Correctly()
        {
            // Arrange
            var itemDO = new MpItemDO
            {
                Id = "test_item",
                Name = "Test Item",
                Value = "100",
                Weight = "2.5",
                Difficulty = "3"
            };

            // Act
            var serializer = new XmlSerializer(typeof(MpItemDO));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, itemDO);
            var xml = stringWriter.ToString();

            // Assert
            Assert.Contains("test_item", xml);
            Assert.Contains("Test Item", xml);
            Assert.Contains("100", xml);
            Assert.Contains("2.5", xml);
            Assert.Contains("3", xml);
            
            _output.WriteLine("Serialized XML:");
            _output.WriteLine(xml);
        }

        [Fact]
        public void DTO_Layer_Should_Provide_Numeric_Properties()
        {
            // Arrange
            var itemDTO = new MpItemDTO
            {
                Id = "test_item",
                Name = "Test Item",
                Value = "100",
                Weight = "2.5",
                Difficulty = "3"
            };

            // Act & Assert
            Assert.Equal(100, itemDTO.ValueInt);
            Assert.Equal(2.5, itemDTO.WeightDouble);
            Assert.Equal(3, itemDTO.DifficultyInt);
            
            _output.WriteLine($"ValueInt: {itemDTO.ValueInt}");
            _output.WriteLine($"WeightDouble: {itemDTO.WeightDouble}");
            _output.WriteLine($"DifficultyInt: {itemDTO.DifficultyInt}");
        }

        [Fact]
        public void Mapping_Between_DO_And_DTO_Should_Work()
        {
            // Arrange
            var itemDO = new MpItemDO
            {
                Id = "test_item",
                Name = "Test Item",
                Value = "100",
                Weight = "2.5",
                Difficulty = "3",
                MultiplayerItem = "true"
            };

            // Act
            var itemDTO = MpItemsMapper.ToDTO(itemDO);
            var mappedBackDO = MpItemsMapper.ToDO(itemDTO);

            // Assert
            Assert.NotNull(itemDTO);
            Assert.NotNull(mappedBackDO);
            Assert.Equal(itemDO.Id, itemDTO.Id);
            Assert.Equal(itemDO.Name, itemDTO.Name);
            Assert.Equal(itemDO.Value, itemDTO.Value);
            Assert.Equal(itemDO.Weight, itemDTO.Weight);
            Assert.Equal(itemDO.Difficulty, itemDTO.Difficulty);
            Assert.Equal(itemDO.MultiplayerItem, StringFromBool(itemDTO.MultiplayerItem));
            
            Assert.Equal(itemDTO.Id, mappedBackDO.Id);
            Assert.Equal(itemDTO.Name, mappedBackDO.Name);
            Assert.Equal(itemDTO.Value, mappedBackDO.Value);
            Assert.Equal(itemDTO.Weight, mappedBackDO.Weight);
            Assert.Equal(itemDTO.Difficulty, mappedBackDO.Difficulty);
            
            _output.WriteLine($"Original DO Id: {itemDO.Id}");
            _output.WriteLine($"Mapped DTO Id: {itemDTO.Id}");
            _output.WriteLine($"Mapped back DO Id: {mappedBackDO.Id}");
        }

        [Fact]
        public void DTO_Setter_Methods_Should_Convert_To_String()
        {
            // Arrange
            var itemDTO = new MpItemDTO();

            // Act
            itemDTO.SetValueInt(150);
            itemDTO.SetWeightDouble(3.75);
            itemDTO.SetDifficultyInt(5);

            // Assert
            Assert.Equal("150", itemDTO.Value);
            Assert.Equal("3.75", itemDTO.Weight);
            Assert.Equal("5", itemDTO.Difficulty);
            Assert.Equal(150, itemDTO.ValueInt);
            Assert.Equal(3.75, itemDTO.WeightDouble);
            Assert.Equal(5, itemDTO.DifficultyInt);
            
            _output.WriteLine($"Value: {itemDTO.Value}");
            _output.WriteLine($"Weight: {itemDTO.Weight}");
            _output.WriteLine($"Difficulty: {itemDTO.Difficulty}");
        }

        [Fact]
        public void DO_Layer_Should_Handle_Null_Values_Correctly()
        {
            // Arrange
            var itemDO = new MpItemDO
            {
                Id = "test_item"
                // Other properties are null
            };

            // Act
            var serializer = new XmlSerializer(typeof(MpItemDO));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, itemDO);
            var xml = stringWriter.ToString();

            // Assert
            // Should not contain empty tags for null values
            Assert.DoesNotContain("MultiplayerItem", xml);
            Assert.DoesNotContain("Name", xml);
            Assert.DoesNotContain("Value", xml);
            
            _output.WriteLine("Serialized XML with null values:");
            _output.WriteLine(xml);
        }

        [Fact]
        public void Full_MpItems_Structure_Should_Map_Correctly()
        {
            // Arrange
            var mpItemsDO = new MpItemsDO
            {
                Items = new()
                {
                    new MpItemDO
                    {
                        Id = "test_sword",
                        Name = "Test Sword",
                        Value = "500",
                        Weight = "3.2",
                        Type = "OneHandedWeapon",
                        ItemComponent = new ItemComponentDO
                        {
                            Component = new WeaponDO
                            {
                                WeaponClass = "OneHandedSword",
                                WeaponBalance = "100",
                                ThrustSpeed = "90",
                                SpeedRating = "85",
                                WeaponLength = "100",
                                SwingDamage = "25",
                                ThrustDamage = "20",
                                PhysicsMaterial = "metal_weapon",
                                WeaponFlags = new WeaponFlagsDO
                                {
                                    MeleeWeapon = "true",
                                    RangedWeapon = "false",
                                    PenaltyWithShield = "false"
                                }
                            }
                        },
                        Flags = new ItemFlagsDO
                        {
                            UseTeamColor = "true",
                            Civilian = "false"
                        }
                    }
                },
                CraftedItems = new()
                {
                    new CraftedItemDO
                    {
                        Id = "crafted_sword",
                        Name = "Crafted Sword",
                        Value = "750",
                        CraftingTemplate = "OneHandedSword",
                        Pieces = new PiecesDO
                        {
                            PieceList = new()
                            {
                                new PieceDO
                                {
                                    Id = "blade_piece",
                                    Type = "Blade",
                                    ScaleFactor = "100"
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var mpItemsDTO = MpItemsMapper.ToDTO(mpItemsDO);
            var mappedBackDO = MpItemsMapper.ToDO(mpItemsDTO);

            // Assert
            Assert.NotNull(mpItemsDTO);
            Assert.Single(mpItemsDTO.Items);
            Assert.Single(mpItemsDTO.CraftedItems);
            
            var itemDTO = mpItemsDTO.Items[0];
            Assert.Equal("test_sword", itemDTO.Id);
            Assert.Equal("Test Sword", itemDTO.Name);
            Assert.Equal("500", itemDTO.Value);
            Assert.Equal(500, itemDTO.ValueInt);
            
            // Check weapon component
            Assert.NotNull(itemDTO.ItemComponent);
            Assert.IsType<WeaponDTO>(itemDTO.ItemComponent.Component);
            var weaponDTO = (WeaponDTO)itemDTO.ItemComponent.Component;
            Assert.Equal("OneHandedSword", weaponDTO.WeaponClass);
            Assert.Equal("100", weaponDTO.WeaponBalance);
            Assert.Equal(100, weaponDTO.WeaponBalanceInt);
            
            // Check weapon flags
            Assert.NotNull(weaponDTO.WeaponFlags);
            Assert.True(weaponDTO.WeaponFlags.MeleeWeapon);
            
            _output.WriteLine("Full structure mapping test passed");
        }

        #region Helper Methods

        private static string StringFromBool(bool? value)
        {
            return value?.ToString().ToLower();
        }

        #endregion
    }
}