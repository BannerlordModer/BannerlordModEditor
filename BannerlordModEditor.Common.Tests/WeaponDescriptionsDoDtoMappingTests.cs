using System;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class WeaponDescriptionsDoDtoMappingTests
    {
        [Fact]
        public void WeaponDescriptionsDO_Mapping_DoToDtoAndBack_ShouldPreserveData()
        {
            // Arrange
            var original = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "one_handed_sword.imperial_gladius",
                        WeaponClass = "OneHandedSword",
                        ItemUsageFeatures = "onehanded:block:swing:thrust",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon },
                                new WeaponFlagDO { Value = WeaponFlagDO.HasAlternativeSwing }
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Blade },
                                new AvailablePieceDO { Id = AvailablePieceDO.Guard },
                                new AvailablePieceDO { Id = AvailablePieceDO.Grip },
                                new AvailablePieceDO { Id = AvailablePieceDO.Pommel }
                            }
                        }
                    },
                    new WeaponDescriptionDO
                    {
                        Id = "two_handed_axe.vlandian_battleaxe",
                        WeaponClass = "TwoHandedAxe",
                        ItemUsageFeatures = "twohanded:block:swing",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon },
                                new WeaponFlagDO { Value = WeaponFlagDO.NotUsableWithOneHand }
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Handle },
                                new AvailablePieceDO { Id = AvailablePieceDO.Head }
                            }
                        }
                    }
                }
            };

            // 初始化索引
            original.InitializeIndexes();

            // Act
            var dto = WeaponDescriptionsMapper.ToDTO(original);
            var back = WeaponDescriptionsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(back);
            
            Assert.Equal(original.Descriptions.Count, back.Descriptions.Count);
            Assert.Equal(2, back.Descriptions.Count);

            var firstOriginal = original.Descriptions[0];
            var firstBack = back.Descriptions[0];
            
            Assert.Equal(firstOriginal.Id, firstBack.Id);
            Assert.Equal(firstOriginal.WeaponClass, firstBack.WeaponClass);
            Assert.Equal(firstOriginal.ItemUsageFeatures, firstBack.ItemUsageFeatures);
            Assert.Equal(firstOriginal.WeaponFlags.Flags.Count, firstBack.WeaponFlags.Flags.Count);
            Assert.Equal(firstOriginal.AvailablePieces.Pieces.Count, firstBack.AvailablePieces.Pieces.Count);

            // 测试业务逻辑保持
            Assert.True(firstBack.IsOneHanded());
            Assert.False(firstBack.IsTwoHanded());
            Assert.True(firstBack.CanBlock());
            Assert.True(firstBack.CanThrust());
            Assert.True(firstBack.CanSwing());
            Assert.True(firstBack.IsCraftable());
            Assert.Equal(4, firstBack.GetPieceCount());
        }

        [Fact]
        public void WeaponDescriptionsMapper_WithNullValues_ShouldHandleGracefully()
        {
            // Arrange
            WeaponDescriptionsDO nullDo = null;
            WeaponDescriptionsDTO nullDto = null;

            // Act & Assert
            Assert.Null(WeaponDescriptionsMapper.ToDTO(nullDo));
            Assert.Null(WeaponDescriptionsMapper.ToDO(nullDto));
            Assert.NotNull(WeaponDescriptionsMapper.ToDTO(new WeaponDescriptionsDO()));
            Assert.NotNull(WeaponDescriptionsMapper.ToDO(new WeaponDescriptionsDTO()));
        }

        [Fact]
        public void WeaponDescriptionsMapper_Validation_ShouldWorkCorrectly()
        {
            // Arrange
            var validWeaponDescriptions = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "test.sword",
                        WeaponClass = "OneHandedSword",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon }
                            }
                        }
                    }
                }
            };

            var invalidWeaponDescriptions = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "", // Invalid: empty ID
                        WeaponClass = "", // Invalid: empty class
                        HasWeaponFlags = false // Invalid: no flags
                    }
                }
            };

            // Act & Assert
            Assert.True(WeaponDescriptionsMapper.Validate(validWeaponDescriptions));
            Assert.False(WeaponDescriptionsMapper.Validate(invalidWeaponDescriptions));

            var errors = WeaponDescriptionsMapper.GetValidationErrors(invalidWeaponDescriptions);
            Assert.Contains("Weapon '': Weapon ID is required", errors);
            Assert.Contains("Weapon '': Weapon class is recommended", errors);
        }

        [Fact]
        public void WeaponDescriptionsMapper_DeepCopy_ShouldCreateIndependentCopy()
        {
            // Arrange
            var original = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "test.sword",
                        WeaponClass = "OneHandedSword",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon }
                            }
                        }
                    }
                }
            };

            // Act
            var copy = WeaponDescriptionsMapper.DeepCopy(original);

            // Modify original
            original.Descriptions[0].WeaponClass = "TwoHandedSword";
            original.Descriptions[0].WeaponFlags.Flags[0].Value = "ModifiedFlag";

            // Assert
            Assert.NotEqual(original.Descriptions[0].WeaponClass, copy.Descriptions[0].WeaponClass);
            Assert.NotEqual(original.Descriptions[0].WeaponFlags.Flags[0].Value, copy.Descriptions[0].WeaponFlags.Flags[0].Value);
        }

        [Fact]
        public void WeaponDescriptionsMapper_Merge_ShouldCombineObjectsCorrectly()
        {
            // Arrange
            var target = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "existing.sword",
                        WeaponClass = "OneHandedSword",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon }
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Blade }
                            }
                        }
                    }
                }
            };

            var source = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "existing.sword", // Same ID as target - should merge
                        WeaponClass = "TwoHandedSword", // Should override
                        ItemUsageFeatures = "twohanded:block:swing", // Should add
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon },
                                new WeaponFlagDO { Value = WeaponFlagDO.NotUsableWithOneHand } // New flag
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Blade },
                                new AvailablePieceDO { Id = AvailablePieceDO.Guard } // New piece
                            }
                        }
                    },
                    new WeaponDescriptionDO
                    {
                        Id = "new.axe", // New ID - should be added
                        WeaponClass = "TwoHandedAxe"
                    }
                }
            };

            // Act
            var merged = WeaponDescriptionsMapper.Merge(target, source);

            // Assert
            Assert.Equal(2, merged.Descriptions.Count); // Should have both weapons

            var existingWeapon = merged.Descriptions.FirstOrDefault(w => w.Id == "existing.sword");
            Assert.NotNull(existingWeapon);
            Assert.Equal("TwoHandedSword", existingWeapon.WeaponClass); // Source class should override
            Assert.Equal("twohanded:block:swing", existingWeapon.ItemUsageFeatures); // Should have new features
            Assert.Equal(2, existingWeapon.WeaponFlags.Flags.Count); // Should have both flags
            Assert.Equal(2, existingWeapon.AvailablePieces.Pieces.Count); // Should have both pieces

            var newWeapon = merged.Descriptions.FirstOrDefault(w => w.Id == "new.axe");
            Assert.NotNull(newWeapon);
            Assert.Equal("TwoHandedAxe", newWeapon.WeaponClass);
        }

        [Fact]
        public void WeaponDescriptionDO_BusinessLogic_ShouldWorkAfterMapping()
        {
            // Arrange
            var original = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "one_handed_sword.test",
                        WeaponClass = "OneHandedSword",
                        ItemUsageFeatures = "onehanded:block:swing:thrust:shield",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon },
                                new WeaponFlagDO { Value = WeaponFlagDO.HasAlternativeSwing }
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Blade },
                                new AvailablePieceDO { Id = AvailablePieceDO.Guard },
                                new AvailablePieceDO { Id = AvailablePieceDO.Grip },
                                new AvailablePieceDO { Id = AvailablePieceDO.Pommel }
                            }
                        }
                    }
                }
            };

            // Act
            var dto = WeaponDescriptionsMapper.ToDTO(original);
            var back = WeaponDescriptionsMapper.ToDO(dto);

            // Assert
            Assert.Single(back.Descriptions);
            var weaponDesc = back.Descriptions[0];
            Assert.True(weaponDesc.IsOneHanded());
            Assert.False(weaponDesc.IsTwoHanded());
            Assert.True(weaponDesc.IsMelee());
            Assert.False(weaponDesc.IsRanged());
            Assert.True(weaponDesc.CanBlock());
            Assert.True(weaponDesc.CanThrust());
            Assert.True(weaponDesc.CanSwing());
            Assert.True(weaponDesc.HasShield());
            Assert.True(weaponDesc.IsCraftable());
            Assert.Equal(4, weaponDesc.GetPieceCount());
            Assert.True(weaponDesc.HasFlag(WeaponFlagDO.MeleeWeapon));
            Assert.True(weaponDesc.HasPiece(AvailablePieceDO.Blade));
            Assert.True(weaponDesc.HasPiece(AvailablePieceDO.Guard));
            Assert.Equal("Sword", weaponDesc.GetWeaponType());

            var features = weaponDesc.GetUsageFeatures();
            Assert.Contains("onehanded", features);
            Assert.Contains("block", features);
            Assert.Contains("swing", features);
            Assert.Contains("thrust", features);
            Assert.Contains("shield", features);
        }

        [Fact]
        public void WeaponFlagDO_Constants_ShouldBeCorrect()
        {
            // Arrange
            var flag = new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon };

            // Act & Assert
            Assert.True(flag.IsMeleeWeapon());
            Assert.False(flag.IsNotUsableWithOneHand());
            Assert.False(flag.RequiresTwoHandsOnMount());
            Assert.False(flag.HasAlternativeSwingFlag());
            Assert.False(flag.CantUseOnHorsebackFlag());
            Assert.False(flag.IsCivilianFlag());
        }

        [Fact]
        public void AvailablePieceDO_Constants_ShouldBeCorrect()
        {
            // Arrange
            var blade = new AvailablePieceDO { Id = AvailablePieceDO.Blade };
            var guard = new AvailablePieceDO { Id = AvailablePieceDO.Guard };
            var grip = new AvailablePieceDO { Id = AvailablePieceDO.Grip };
            var pommel = new AvailablePieceDO { Id = AvailablePieceDO.Pommel };
            var handle = new AvailablePieceDO { Id = AvailablePieceDO.Handle };
            var head = new AvailablePieceDO { Id = AvailablePieceDO.Head };

            // Act & Assert
            Assert.True(blade.IsBlade());
            Assert.Equal("Blade", blade.GetPieceType());

            Assert.True(guard.IsGuard());
            Assert.Equal("Guard", guard.GetPieceType());

            Assert.True(grip.IsGrip());
            Assert.Equal("Grip", grip.GetPieceType());

            Assert.True(pommel.IsPommel());
            Assert.Equal("Pommel", pommel.GetPieceType());

            Assert.True(handle.IsHandle());
            Assert.Equal("Handle", handle.GetPieceType());

            Assert.True(head.IsHead());
            Assert.Equal("Head", head.GetPieceType());
        }

        [Fact]
        public void WeaponDescriptionsDO_Indexes_ShouldWorkCorrectly()
        {
            // Arrange
            var weaponDescriptions = new WeaponDescriptionsDO
            {
                Descriptions = new List<WeaponDescriptionDO>
                {
                    new WeaponDescriptionDO
                    {
                        Id = "sword1",
                        WeaponClass = "OneHandedSword",
                        ItemUsageFeatures = "onehanded:block:swing",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon }
                            }
                        },
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Blade }
                            }
                        }
                    },
                    new WeaponDescriptionDO
                    {
                        Id = "sword2",
                        WeaponClass = "OneHandedSword",
                        ItemUsageFeatures = "onehanded:block:thrust",
                        HasWeaponFlags = true,
                        WeaponFlags = new WeaponFlagsDO
                        {
                            Flags = new List<WeaponFlagDO>
                            {
                                new WeaponFlagDO { Value = WeaponFlagDO.MeleeWeapon },
                                new WeaponFlagDO { Value = WeaponFlagDO.HasAlternativeSwing }
                            }
                        }
                    },
                    new WeaponDescriptionDO
                    {
                        Id = "axe1",
                        WeaponClass = "TwoHandedAxe",
                        ItemUsageFeatures = "twohanded:block:swing",
                        HasAvailablePieces = true,
                        AvailablePieces = new AvailablePiecesDO
                        {
                            Pieces = new List<AvailablePieceDO>
                            {
                                new AvailablePieceDO { Id = AvailablePieceDO.Handle },
                                new AvailablePieceDO { Id = AvailablePieceDO.Head }
                            }
                        }
                    }
                }
            };

            // Act
            weaponDescriptions.InitializeIndexes();

            // Assert
            // 测试按武器类型索引
            var swords = weaponDescriptions.GetDescriptionsByWeaponClass("OneHandedSword");
            Assert.Equal(2, swords.Count);

            var axes = weaponDescriptions.GetDescriptionsByWeaponClass("TwoHandedAxe");
            Assert.Single(axes);

            // 测试按使用特性索引
            var blockingWeapons = weaponDescriptions.GetDescriptionsByUsageFeature("block");
            Assert.Equal(3, blockingWeapons.Count);

            var swingingWeapons = weaponDescriptions.GetDescriptionsByUsageFeature("swing");
            Assert.Equal(2, swingingWeapons.Count);

            var thrustingWeapons = weaponDescriptions.GetDescriptionsByUsageFeature("thrust");
            Assert.Single(thrustingWeapons);

            // 测试按标志索引
            var meleeWeapons = weaponDescriptions.GetDescriptionsByFlag(WeaponFlagDO.MeleeWeapon);
            Assert.Equal(2, meleeWeapons.Count);

            var altSwingWeapons = weaponDescriptions.GetDescriptionsByFlag(WeaponFlagDO.HasAlternativeSwing);
            Assert.Single(altSwingWeapons);

            // 测试按部件索引
            var bladeWeapons = weaponDescriptions.GetDescriptionsByPiece(AvailablePieceDO.Blade);
            Assert.Single(bladeWeapons);

            var handleWeapons = weaponDescriptions.GetDescriptionsByPiece(AvailablePieceDO.Handle);
            Assert.Single(handleWeapons);

            // 测试特殊查询方法
            var oneHanded = weaponDescriptions.GetOneHandedWeapons();
            Assert.Equal(2, oneHanded.Count);

            var twoHanded = weaponDescriptions.GetTwoHandedWeapons();
            Assert.Single(twoHanded);

            var ranged = weaponDescriptions.GetRangedWeapons();
            Assert.Empty(ranged);

            var craftable = weaponDescriptions.GetCraftableWeapons();
            Assert.Equal(2, craftable.Count);

            // 测试统计方法
            Assert.Equal(3, weaponDescriptions.GetTotalWeaponCount());
            Assert.Equal(2, weaponDescriptions.GetWeaponClassCount());

            var distribution = weaponDescriptions.GetWeaponClassDistribution();
            Assert.Equal(2, distribution["OneHandedSword"]);
            Assert.Equal(1, distribution["TwoHandedAxe"]);
        }
    }
}