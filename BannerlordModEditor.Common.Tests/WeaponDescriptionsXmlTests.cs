using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class WeaponDescriptionsXmlTests
    {
        [Fact]
        public void WeaponDescriptionsDO_XmlSerialization_ShouldPreserveStructure()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "weapon_descriptions.xml");
            var xml = File.ReadAllText(xmlPath);

            // Act
            var obj = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(obj);

            // Assert
            Assert.NotNull(obj);
            Assert.NotNull(obj.Descriptions);
            Assert.True(obj.Descriptions.Count > 0);

            // Basic structural equality check
            Assert.Contains("WeaponDescriptions", serializedXml);
            Assert.Contains("WeaponDescription", serializedXml);
        }

        [Fact]
        public void WeaponDescriptionsDO_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "weapon_descriptions.xml");
            var xmlContent = File.ReadAllText(xmlPath);

            // Act
            var result = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xmlContent);
            result.InitializeIndexes();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Descriptions.Count > 0);
            Assert.True(result.IsValid());

            // Test first weapon
            var firstWeapon = result.Descriptions.FirstOrDefault();
            Assert.NotNull(firstWeapon);
            Assert.False(string.IsNullOrEmpty(firstWeapon.Id));
            Assert.False(string.IsNullOrEmpty(firstWeapon.WeaponClass));

            // Test business logic
            var oneHandedWeapons = result.GetOneHandedWeapons();
            var twoHandedWeapons = result.GetTwoHandedWeapons();
            var craftableWeapons = result.GetCraftableWeapons();

            Assert.True(oneHandedWeapons.Count > 0 || twoHandedWeapons.Count > 0);
            Assert.True(craftableWeapons.Count > 0);
        }

        [Fact]
        public void WeaponDescriptionsDO_WithBusinessLogic_ShouldWorkCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WeaponDescriptions>
    <WeaponDescription id=""test.sword"" weapon_class=""OneHandedSword"" item_usage_features=""onehanded:block:swing:thrust"">
        <WeaponFlags>
            <WeaponFlag value=""MeleeWeapon"" />
            <WeaponFlag value=""HasAlternativeSwing"" />
        </WeaponFlags>
        <AvailablePieces>
            <AvailablePiece id=""blade"" />
            <AvailablePiece id=""guard"" />
            <AvailablePiece id=""grip"" />
            <AvailablePiece id=""pommel"" />
        </AvailablePieces>
    </WeaponDescription>
    <WeaponDescription id=""test.axe"" weapon_class=""TwoHandedAxe"" item_usage_features=""twohanded:block:swing"">
        <WeaponFlags>
            <WeaponFlag value=""MeleeWeapon"" />
            <WeaponFlag value=""NotUsableWithOneHand"" />
        </WeaponFlags>
        <AvailablePieces>
            <AvailablePiece id=""handle"" />
            <AvailablePiece id=""head"" />
        </AvailablePieces>
    </WeaponDescription>
</WeaponDescriptions>";

            // Act
            var result = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xmlContent);
            result.InitializeIndexes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Descriptions.Count);
            Assert.True(result.IsValid());

            // Test weapon classification
            var oneHanded = result.GetOneHandedWeapons();
            var twoHanded = result.GetTwoHandedWeapons();
            Assert.Single(oneHanded);
            Assert.Single(twoHanded);

            // Test indexes
            var swords = result.GetDescriptionsByWeaponClass("OneHandedSword");
            var axes = result.GetDescriptionsByWeaponClass("TwoHandedAxe");
            Assert.Single(swords);
            Assert.Single(axes);

            // Test usage features
            var blockingWeapons = result.GetDescriptionsByUsageFeature("block");
            Assert.Equal(2, blockingWeapons.Count);

            var swingingWeapons = result.GetDescriptionsByUsageFeature("swing");
            Assert.Equal(2, swingingWeapons.Count);

            var thrustingWeapons = result.GetDescriptionsByUsageFeature("thrust");
            Assert.Single(thrustingWeapons);

            // Test flags
            var meleeWeapons = result.GetDescriptionsByFlag("MeleeWeapon");
            Assert.Equal(2, meleeWeapons.Count);

            var notOneHanded = result.GetDescriptionsByFlag("NotUsableWithOneHand");
            Assert.Single(notOneHanded);

            // Test pieces
            var bladeWeapons = result.GetDescriptionsByPiece("blade");
            Assert.Single(bladeWeapons);

            var headWeapons = result.GetDescriptionsByPiece("head");
            Assert.Single(headWeapons);

            // Test statistics
            Assert.Equal(2, result.GetTotalWeaponCount());
            Assert.Equal(2, result.GetWeaponClassCount());

            var distribution = result.GetWeaponClassDistribution();
            Assert.Equal(1, distribution["OneHandedSword"]);
            Assert.Equal(1, distribution["TwoHandedAxe"]);
        }

        [Fact]
        public void WeaponDescriptionsDO_RoundTrip_ShouldPreserveAllData()
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
                    }
                }
            };

            // Act
            var xml = XmlTestUtils.Serialize(original);
            var deserialized = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xml);
            deserialized.InitializeIndexes();

            // Assert
            Assert.NotNull(deserialized);
            Assert.Single(deserialized.Descriptions);

            var weapon = deserialized.Descriptions[0];
            Assert.Equal(original.Descriptions[0].Id, weapon.Id);
            Assert.Equal(original.Descriptions[0].WeaponClass, weapon.WeaponClass);
            Assert.Equal(original.Descriptions[0].ItemUsageFeatures, weapon.ItemUsageFeatures);
            Assert.Equal(original.Descriptions[0].WeaponFlags.Flags.Count, weapon.WeaponFlags.Flags.Count);
            Assert.Equal(original.Descriptions[0].AvailablePieces.Pieces.Count, weapon.AvailablePieces.Pieces.Count);

            // Test business logic still works
            Assert.True(weapon.IsOneHanded());
            Assert.True(weapon.CanBlock());
            Assert.True(weapon.CanThrust());
            Assert.True(weapon.CanSwing());
            Assert.True(weapon.IsCraftable());
            Assert.Equal(4, weapon.GetPieceCount());
        }

        [Fact]
        public void WeaponDescriptionsDO_EmptyFile_ShouldHandleGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WeaponDescriptions>
</WeaponDescriptions>";

            // Act
            var result = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Descriptions);
            Assert.True(result.HasEmptyDescriptions);
            Assert.False(result.IsValid());
        }

        [Fact]
        public void WeaponDescriptionsDO_WithMissingOptionalElements_ShouldWork()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WeaponDescriptions>
    <WeaponDescription id=""simple.dagger"" weapon_class=""Dagger"" />
    <WeaponDescription id=""simple.javelin"" weapon_class=""Javelin"" item_usage_features=""thrown"">
        <WeaponFlags>
            <WeaponFlag value=""CantUseOnHorseback"" />
        </WeaponFlags>
    </WeaponDescription>
</WeaponDescriptions>";

            // Act
            var result = XmlTestUtils.Deserialize<WeaponDescriptionsDO>(xmlContent);
            result.InitializeIndexes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Descriptions.Count);

            var dagger = result.GetDescriptionById("simple.dagger");
            Assert.NotNull(dagger);
            Assert.Equal("Dagger", dagger.WeaponClass);
            Assert.Null(dagger.WeaponFlags);
            Assert.Null(dagger.AvailablePieces);
            Assert.False(dagger.IsCraftable());

            var javelin = result.GetDescriptionById("simple.javelin");
            Assert.NotNull(javelin);
            Assert.Equal("Javelin", javelin.WeaponClass);
            Assert.NotNull(javelin.WeaponFlags);
            Assert.Single(javelin.WeaponFlags.Flags);
            Assert.True(javelin.WeaponFlags.Flags[0].CantUseOnHorsebackFlag());
            Assert.True(javelin.IsRanged());
        }
    }
} 