using BannerlordModEditor.Common.Mappers;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using System;
using System.Collections.Generic;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MpItemsDoDtoMappingTests
    {
        [Fact]
        public void MpItems_Mapping_DoToDtoAndBack_ShouldPreserveData()
        {
            // Arrange
            var doItem = new MpItemsDO
            {
                Items = new List<MpItemDO>
                {
                    new MpItemDO
                    {
                        Id = "test_item_1",
                        Name = "Test Item 1",
                        MultiplayerItem = "true",
                        Type = "Weapon",
                        UsingTableau = "false",
                        IsMerchandise = "1",
                        RecalculateBody = "yes",
                        HasLowerHolsterPriority = "no"
                    }
                },
                CraftedItems = new List<CraftedItemDO>
                {
                    new CraftedItemDO
                    {
                        Id = "crafted_item_1",
                        Name = "Crafted Item 1",
                        CraftingTemplate = "template_1",
                        MultiplayerItem = "false",
                        IsMerchandise = "0"
                    }
                }
            };

            // Act
            var dto = MpItemsMapper.ToDTO(doItem);
            var doResult = MpItemsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(doResult);
            Assert.Single(dto.Items);
            Assert.Single(dto.CraftedItems);
            Assert.Single(doResult.Items);
            Assert.Single(doResult.CraftedItems);

            // Check Item mapping
            var originalItem = doItem.Items[0];
            var mappedItem = dto.Items[0];
            var resultItem = doResult.Items[0];

            Assert.Equal(originalItem.Id, mappedItem.Id);
            Assert.Equal(originalItem.Name, mappedItem.Name);
            Assert.True(mappedItem.MultiplayerItem);
            Assert.False(mappedItem.UsingTableau);
            Assert.True(mappedItem.IsMerchandise);
            Assert.True(mappedItem.RecalculateBody);
            Assert.False(mappedItem.HasLowerHolsterPriority);

            Assert.Equal(originalItem.Id, resultItem.Id);
            Assert.Equal(originalItem.Name, resultItem.Name);
            Assert.Equal(originalItem.MultiplayerItem, resultItem.MultiplayerItem);
            Assert.Equal(originalItem.Type, resultItem.Type);
            Assert.Equal(originalItem.UsingTableau, resultItem.UsingTableau);
            Assert.Equal("true", resultItem.IsMerchandise); // Standardized form to "true"
            Assert.Equal("true", resultItem.RecalculateBody); // Standardized form to "true"
            Assert.Equal("false", resultItem.HasLowerHolsterPriority); // Standardized form to "false"

            // Check CraftedItem mapping
            var originalCraftedItem = doItem.CraftedItems[0];
            var mappedCraftedItem = dto.CraftedItems[0];
            var resultCraftedItem = doResult.CraftedItems[0];

            Assert.Equal(originalCraftedItem.Id, mappedCraftedItem.Id);
            Assert.Equal(originalCraftedItem.Name, mappedCraftedItem.Name);
            Assert.Equal(originalCraftedItem.CraftingTemplate, mappedCraftedItem.CraftingTemplate);
            Assert.False(mappedCraftedItem.MultiplayerItem);
            Assert.False(mappedCraftedItem.IsMerchandise);

            Assert.Equal(originalCraftedItem.Id, resultCraftedItem.Id);
            Assert.Equal(originalCraftedItem.Name, resultCraftedItem.Name);
            Assert.Equal(originalCraftedItem.CraftingTemplate, resultCraftedItem.CraftingTemplate);
            Assert.Equal("false", resultCraftedItem.MultiplayerItem); // Standardized form to "false"
            Assert.Equal("false", resultCraftedItem.IsMerchandise); // Standardized form to "false"
        }

        [Fact]
        public void MpItemDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var item = new MpItemDO
            {
                MultiplayerItem = "true",
                UsingTableau = "1",
                IsMerchandise = "yes",
                RecalculateBody = "false",
                HasLowerHolsterPriority = "0",
                // Test case sensitivity
                Culture = "TRUE",
                Mesh = "False"
            };

            // Act
            var dto = MpItemsMapper.ToDTO(item);

            // Assert
            Assert.True(dto.MultiplayerItem);
            Assert.True(dto.UsingTableau);
            Assert.True(dto.IsMerchandise);
            Assert.False(dto.RecalculateBody);
            Assert.False(dto.HasLowerHolsterPriority);
            // Culture and Mesh are not boolean properties in DTO, so they remain as strings
        }

        [Fact]
        public void ArmorDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var armorDO = new ArmorDO
            {
                HasGenderVariations = "true",
                CoversBody = "1",
                CoversLegs = "yes",
                CoversHead = "false",
                CoversHands = "0"
            };

            // Act
            var armorDTO = MpItemsMapper.ToDTO(armorDO);
            var armorDOResult = MpItemsMapper.ToDO(armorDTO);

            // Assert
            Assert.True(armorDTO.HasGenderVariations);
            Assert.True(armorDTO.CoversBody);
            Assert.True(armorDTO.CoversLegs);
            Assert.False(armorDTO.CoversHead);
            Assert.False(armorDTO.CoversHands);

            Assert.Equal("true", armorDOResult.HasGenderVariations); // Standardized form
            Assert.Equal("true", armorDOResult.CoversBody); // Standardized form
            Assert.Equal("true", armorDOResult.CoversLegs); // Standardized form to "true"
            Assert.Equal("false", armorDOResult.CoversHead); // Standardized form to "false"
            Assert.Equal("false", armorDOResult.CoversHands); // Standardized form to "false"
        }

        [Fact]
        public void WeaponFlagsDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var weaponFlagsDO = new MpWeaponFlagsDO
            {
                MeleeWeapon = "true",
                RangedWeapon = "1",
                PenaltyWithShield = "yes",
                NotUsableWithOneHand = "false",
                TwoHandIdleOnMount = "0"
            };

            // Act
            var weaponFlagsDTO = MpItemsMapper.ToDTO(weaponFlagsDO);
            var weaponFlagsDOResult = MpItemsMapper.ToDO(weaponFlagsDTO);

            // Assert
            Assert.True(weaponFlagsDTO.MeleeWeapon);
            Assert.True(weaponFlagsDTO.RangedWeapon);
            Assert.True(weaponFlagsDTO.PenaltyWithShield);
            Assert.False(weaponFlagsDTO.NotUsableWithOneHand);
            Assert.False(weaponFlagsDTO.TwoHandIdleOnMount);

            Assert.Equal("true", weaponFlagsDOResult.MeleeWeapon); // Standardized form
            Assert.Equal("true", weaponFlagsDOResult.RangedWeapon); // Standardized form
            Assert.Equal("true", weaponFlagsDOResult.PenaltyWithShield); // Standardized form to "true"
            Assert.Equal("false", weaponFlagsDOResult.NotUsableWithOneHand); // Standardized form to "false"
            Assert.Equal("false", weaponFlagsDOResult.TwoHandIdleOnMount); // Standardized form to "false"
        }

        [Fact]
        public void HorseDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var horseDO = new HorseDO
            {
                IsMountable = "true"
            };

            // Act
            var horseDTO = MpItemsMapper.ToDTO(horseDO);
            var horseDOResult = MpItemsMapper.ToDO(horseDTO);

            // Assert
            Assert.True(horseDTO.IsMountable);
            Assert.Equal(horseDO.IsMountable, horseDOResult.IsMountable);
        }

        [Fact]
        public void AdditionalMeshDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var meshDO = new AdditionalMeshDO
            {
                Name = "test_mesh",
                AffectedByCover = "true"
            };

            // Act
            var meshDTO = MpItemsMapper.ToDTO(meshDO);
            var meshDOResult = MpItemsMapper.ToDO(meshDTO);

            // Assert
            Assert.Equal(meshDO.Name, meshDTO.Name);
            Assert.True(meshDTO.AffectedByCover);
            Assert.Equal(meshDO.Name, meshDOResult.Name);
            Assert.Equal(meshDO.AffectedByCover, meshDOResult.AffectedByCover);
        }

        [Fact]
        public void ItemFlagsDO_BooleanValueParsing_ShouldHandleVariousFormats()
        {
            // Arrange
            var flagsDO = new ItemFlagsDO
            {
                UseTeamColor = "true",
                Civilian = "1",
                DoesNotHideChest = "yes",
                WoodenParry = "false",
                DropOnWeaponChange = "0"
            };

            // Act
            var flagsDTO = MpItemsMapper.ToDTO(flagsDO);
            var flagsDOResult = MpItemsMapper.ToDO(flagsDTO);

            // Assert
            Assert.True(flagsDTO.UseTeamColor);
            Assert.True(flagsDTO.Civilian);
            Assert.True(flagsDTO.DoesNotHideChest);
            Assert.False(flagsDTO.WoodenParry);
            Assert.False(flagsDTO.DropOnWeaponChange);

            Assert.Equal("true", flagsDOResult.UseTeamColor); // Standardized form
            Assert.Equal("true", flagsDOResult.Civilian); // Standardized form
            Assert.Equal("true", flagsDOResult.DoesNotHideChest); // Standardized form to "true"
            Assert.Equal("false", flagsDOResult.WoodenParry); // Standardized form to "false"
            Assert.Equal("false", flagsDOResult.DropOnWeaponChange); // Standardized form to "false"
        }

        [Fact]
        public void MpItems_Mapping_WithNullValues_ShouldHandleGracefully()
        {
            // Arrange
            var doItem = new MpItemsDO
            {
                Items = new List<MpItemDO>
                {
                    new MpItemDO
                    {
                        Id = null,
                        Name = null,
                        MultiplayerItem = null,
                        Type = null
                    }
                }
            };

            // Act
            var dto = MpItemsMapper.ToDTO(doItem);
            var doResult = MpItemsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.Items);
            Assert.Single(dto.Items);
            Assert.NotNull(doResult);
            Assert.NotNull(doResult.Items);
            Assert.Single(doResult.Items);

            var itemDto = dto.Items[0];
            var itemDo = doResult.Items[0];

            Assert.Equal(string.Empty, itemDto.Id);
            Assert.Equal(string.Empty, itemDto.Name);
            Assert.Equal(string.Empty, itemDto.Type);
            Assert.False(itemDto.MultiplayerItem); // Default value for boolean

            Assert.Equal(string.Empty, itemDo.Id);
            Assert.Equal(string.Empty, itemDo.Name);
            Assert.Equal(string.Empty, itemDo.Type);
            Assert.Equal("false", itemDo.MultiplayerItem); // DO converts null to "false"
        }
    }
}