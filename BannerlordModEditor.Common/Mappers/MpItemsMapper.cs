using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MpItemsMapper
    {
        #region DO to DTO

        public static MpItemsDTO ToDTO(MpItemsDO source)
        {
            if (source == null) return null;

            return new MpItemsDTO
            {
                Items = source.Items?.Select(ToDTO).ToList() ?? new List<ItemDTO>(),
                CraftedItems = source.CraftedItems?.Select(ToDTO).ToList() ?? new List<CraftedItemDTO>()
            };
        }

        public static ItemDTO ToDTO(ItemDO source)
        {
            if (source == null) return null;

            return new ItemDTO
            {
                MultiplayerItem = BoolFromString(source.MultiplayerItem),
                Id = source.Id,
                Name = source.Name,
                BodyName = source.BodyName,
                ShieldBodyName = source.ShieldBodyName,
                HolsterBodyName = source.HolsterBodyName,
                Subtype = source.Subtype,
                Mesh = source.Mesh,
                HolsterMesh = source.HolsterMesh,
                Culture = source.Culture,
                UsingTableau = BoolFromString(source.UsingTableau),
                Value = source.Value,
                IsMerchandise = BoolFromString(source.IsMerchandise),
                Weight = source.Weight,
                Difficulty = source.Difficulty,
                Appearance = source.Appearance,
                Type = source.Type,
                ItemHolsters = source.ItemHolsters,
                ItemCategory = source.ItemCategory,
                RecalculateBody = BoolFromString(source.RecalculateBody),
                HasLowerHolsterPriority = BoolFromString(source.HasLowerHolsterPriority),
                HolsterPositionShift = source.HolsterPositionShift,
                FlyingMesh = source.FlyingMesh,
                HolsterMeshWithWeapon = source.HolsterMeshWithWeapon,
                AmmoOffset = source.AmmoOffset,
                Prefab = source.Prefab,
                LodAtlasIndex = source.LodAtlasIndex,
                ItemComponent = ToDTO(source.ItemComponent),
                Flags = ToDTO(source.Flags)
            };
        }

        public static CraftedItemDTO ToDTO(CraftedItemDO source)
        {
            if (source == null) return null;

            return new CraftedItemDTO
            {
                MultiplayerItem = BoolFromString(source.MultiplayerItem),
                Id = source.Id,
                Name = source.Name,
                CraftingTemplate = source.CraftingTemplate,
                Value = source.Value,
                IsMerchandise = BoolFromString(source.IsMerchandise),
                Culture = source.Culture,
                Pieces = ToDTO(source.Pieces)
            };
        }

        public static PiecesDTO ToDTO(PiecesDO source)
        {
            if (source == null) return null;

            return new PiecesDTO
            {
                PieceList = source.PieceList?.Select(ToDTO).ToList() ?? new List<PieceDTO>()
            };
        }

        public static PieceDTO ToDTO(PieceDO source)
        {
            if (source == null) return null;

            return new PieceDTO
            {
                Id = source.Id,
                Type = source.Type,
                ScaleFactor = source.ScaleFactor
            };
        }

        public static ItemComponentDTO ToDTO(ItemComponentDO source)
        {
            if (source == null) return null;

            return new ItemComponentDTO
            {
                Component = source.Component switch
                {
                    ArmorDO armor => ToDTO(armor),
                    WeaponDO weapon => ToDTO(weapon),
                    HorseDO horse => ToDTO(horse),
                    HorseHarnessDO horseHarness => ToDTO(horseHarness),
                    _ => source.Component
                }
            };
        }

        public static ArmorDTO ToDTO(ArmorDO source)
        {
            if (source == null) return null;

            return new ArmorDTO
            {
                HeadArmor = source.HeadArmor,
                BodyArmor = source.BodyArmor,
                LegArmor = source.LegArmor,
                ArmArmor = source.ArmArmor,
                HasGenderVariations = source.HasGenderVariations,
                HairCoverType = source.HairCoverType,
                BeardCoverType = source.BeardCoverType,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType,
                CoversBody = source.CoversBody,
                CoversLegs = source.CoversLegs,
                CoversHead = source.CoversHead,
                ManeCoverType = source.ManeCoverType,
                ReinsMesh = source.ReinsMesh,
                ManeuverBonus = source.ManeuverBonus,
                SpeedBonus = source.SpeedBonus,
                ChargeBonus = source.ChargeBonus,
                FamilyType = source.FamilyType,
                CoversHands = source.CoversHands,
                BodyMeshType = source.BodyMeshType
            };
        }

        public static WeaponDTO ToDTO(WeaponDO source)
        {
            if (source == null) return null;

            return new WeaponDTO
            {
                WeaponClass = source.WeaponClass,
                AmmoClass = source.AmmoClass,
                StackAmount = source.StackAmount,
                WeaponBalance = source.WeaponBalance,
                ThrustSpeed = source.ThrustSpeed,
                SpeedRating = source.SpeedRating,
                MissileSpeed = source.MissileSpeed,
                Accuracy = source.Accuracy,
                PhysicsMaterial = source.PhysicsMaterial,
                WeaponLength = source.WeaponLength,
                SwingDamage = source.SwingDamage,
                ThrustDamage = source.ThrustDamage,
                SwingDamageType = source.SwingDamageType,
                ThrustDamageType = source.ThrustDamageType,
                ItemUsage = source.ItemUsage,
                FlyingSoundCode = source.FlyingSoundCode,
                StickingRotation = source.StickingRotation,
                StickingPosition = source.StickingPosition,
                CenterOfMass = source.CenterOfMass,
                Position = source.Position,
                Rotation = source.Rotation,
                HitPoints = source.HitPoints,
                AmmoLimit = source.AmmoLimit,
                PassbySoundCode = source.PassbySoundCode,
                ReloadPhaseCount = source.ReloadPhaseCount,
                ModifierGroup = source.ModifierGroup,
                BodyArmor = source.BodyArmor,
                TrailParticleName = source.TrailParticleName,
                RotationSpeed = source.RotationSpeed,
                WeaponFlags = ToDTO(source.WeaponFlags)
            };
        }

        public static WeaponFlagsDTO ToDTO(WeaponFlagsDO source)
        {
            if (source == null) return null;

            return new WeaponFlagsDTO
            {
                MeleeWeapon = source.MeleeWeapon,
                RangedWeapon = source.RangedWeapon,
                PenaltyWithShield = source.PenaltyWithShield,
                NotUsableWithOneHand = source.NotUsableWithOneHand,
                TwoHandIdleOnMount = source.TwoHandIdleOnMount,
                WideGrip = source.WideGrip,
                Consumable = source.Consumable,
                AmmoSticksWhenShot = source.AmmoSticksWhenShot,
                MultiplePenetration = source.MultiplePenetration,
                CanPenetrateShield = source.CanPenetrateShield,
                CanBlockRanged = source.CanBlockRanged,
                HasHitPoints = source.HasHitPoints,
                HasString = source.HasString,
                StringHeldByHand = source.StringHeldByHand,
                AutoReload = source.AutoReload,
                UnloadWhenSheathed = source.UnloadWhenSheathed,
                AmmoBreaksOnBounceBack = source.AmmoBreaksOnBounceBack,
                CantReloadOnHorseback = source.CantReloadOnHorseback,
                Burning = source.Burning,
                LeavesTrail = source.LeavesTrail,
                CanKnockDown = source.CanKnockDown,
                MissileWithPhysics = source.MissileWithPhysics,
                UseHandAsThrowBase = source.UseHandAsThrowBase,
                AffectsArea = source.AffectsArea,
                AmmoCanBreakOnBounceBack = source.AmmoCanBreakOnBounceBack,
                CanKillEvenIfBlunt = source.CanKillEvenIfBlunt,
                AffectsAreaBig = source.AffectsAreaBig,
                AttachAmmoToVisual = source.AttachAmmoToVisual
            };
        }

        public static HorseDTO ToDTO(HorseDO source)
        {
            if (source == null) return null;

            return new HorseDTO
            {
                Speed = source.Speed,
                Maneuver = source.Maneuver,
                ChargeDamage = source.ChargeDamage,
                HitPoints = source.HitPoints,
                BodyLength = source.BodyLength,
                IsMountable = source.IsMountable,
                Monster = source.Monster,
                ExtraHealth = source.ExtraHealth,
                SkeletonScale = source.SkeletonScale,
                ModifierGroup = source.ModifierGroup,
                AdditionalMeshes = ToDTO(source.AdditionalMeshes),
                Materials = ToDTO(source.Materials)
            };
        }

        public static AdditionalMeshesDTO ToDTO(AdditionalMeshesDO source)
        {
            if (source == null) return null;

            return new AdditionalMeshesDTO
            {
                Meshes = source.Meshes?.Select(ToDTO).ToList() ?? new List<AdditionalMeshDTO>()
            };
        }

        public static AdditionalMeshDTO ToDTO(AdditionalMeshDO source)
        {
            if (source == null) return null;

            return new AdditionalMeshDTO
            {
                Name = source.Name,
                AffectedByCover = source.AffectedByCover
            };
        }

        public static MaterialsDTO ToDTO(MaterialsDO source)
        {
            if (source == null) return null;

            return new MaterialsDTO
            {
                MaterialList = source.MaterialList?.Select(ToDTO).ToList() ?? new List<MaterialDTO>()
            };
        }

        public static MaterialDTO ToDTO(MaterialDO source)
        {
            if (source == null) return null;

            return new MaterialDTO
            {
                Name = source.Name,
                MeshMultipliers = ToDTO(source.MeshMultipliers)
            };
        }

        public static MeshMultipliersDTO ToDTO(MeshMultipliersDO source)
        {
            if (source == null) return null;

            return new MeshMultipliersDTO
            {
                Multipliers = source.Multipliers?.Select(ToDTO).ToList() ?? new List<MeshMultiplierDTO>()
            };
        }

        public static MeshMultiplierDTO ToDTO(MeshMultiplierDO source)
        {
            if (source == null) return null;

            return new MeshMultiplierDTO
            {
                MeshMultiplierValue = source.MeshMultiplierValue,
                Percentage = source.Percentage
            };
        }

        public static HorseHarnessDTO ToDTO(HorseHarnessDO source)
        {
            if (source == null) return null;

            return new HorseHarnessDTO
            {
                BodyArmor = source.BodyArmor,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType
            };
        }

        public static ItemFlagsDTO ToDTO(ItemFlagsDO source)
        {
            if (source == null) return null;

            return new ItemFlagsDTO
            {
                UseTeamColor = source.UseTeamColor,
                Civilian = source.Civilian,
                DoesNotHideChest = source.DoesNotHideChest,
                WoodenParry = source.WoodenParry,
                DropOnWeaponChange = source.DropOnWeaponChange,
                DoNotScaleBodyAccordingToWeaponLength = source.DoNotScaleBodyAccordingToWeaponLength,
                QuickFadeOut = source.QuickFadeOut,
                CannotBePickedUp = source.CannotBePickedUp,
                HeldInOffHand = source.HeldInOffHand,
                ForceAttachOffHandSecondaryItemBone = source.ForceAttachOffHandSecondaryItemBone,
                ForceAttachOffHandPrimaryItemBone = source.ForceAttachOffHandPrimaryItemBone,
                DropOnAnyAction = source.DropOnAnyAction
            };
        }

        #endregion

        #region DTO to DO

        public static MpItemsDO ToDO(MpItemsDTO source)
        {
            if (source == null) return null;

            return new MpItemsDO
            {
                Items = source.Items?.Select(ToDO).ToList() ?? new List<ItemDO>(),
                CraftedItems = source.CraftedItems?.Select(ToDO).ToList() ?? new List<CraftedItemDO>()
            };
        }

        public static ItemDO ToDO(ItemDTO source)
        {
            if (source == null) return null;

            return new ItemDO
            {
                MultiplayerItem = StringFromBool(source.MultiplayerItem),
                Id = source.Id,
                Name = source.Name,
                BodyName = source.BodyName,
                ShieldBodyName = source.ShieldBodyName,
                HolsterBodyName = source.HolsterBodyName,
                Subtype = source.Subtype,
                Mesh = source.Mesh,
                HolsterMesh = source.HolsterMesh,
                Culture = source.Culture,
                UsingTableau = StringFromBool(source.UsingTableau),
                Value = source.Value,
                IsMerchandise = StringFromBool(source.IsMerchandise),
                Weight = source.Weight,
                Difficulty = source.Difficulty,
                Appearance = source.Appearance,
                Type = source.Type,
                ItemHolsters = source.ItemHolsters,
                ItemCategory = source.ItemCategory,
                RecalculateBody = StringFromBool(source.RecalculateBody),
                HasLowerHolsterPriority = StringFromBool(source.HasLowerHolsterPriority),
                HolsterPositionShift = source.HolsterPositionShift,
                FlyingMesh = source.FlyingMesh,
                HolsterMeshWithWeapon = source.HolsterMeshWithWeapon,
                AmmoOffset = source.AmmoOffset,
                Prefab = source.Prefab,
                LodAtlasIndex = source.LodAtlasIndex,
                ItemComponent = ToDO(source.ItemComponent),
                Flags = ToDO(source.Flags)
            };
        }

        public static CraftedItemDO ToDO(CraftedItemDTO source)
        {
            if (source == null) return null;

            return new CraftedItemDO
            {
                MultiplayerItem = StringFromBool(source.MultiplayerItem),
                Id = source.Id,
                Name = source.Name,
                CraftingTemplate = source.CraftingTemplate,
                Value = source.Value,
                IsMerchandise = StringFromBool(source.IsMerchandise),
                Culture = source.Culture,
                Pieces = ToDO(source.Pieces)
            };
        }

        public static PiecesDO ToDO(PiecesDTO source)
        {
            if (source == null) return null;

            return new PiecesDO
            {
                PieceList = source.PieceList?.Select(ToDO).ToList() ?? new List<PieceDO>()
            };
        }

        public static PieceDO ToDO(PieceDTO source)
        {
            if (source == null) return null;

            return new PieceDO
            {
                Id = source.Id,
                Type = source.Type,
                ScaleFactor = source.ScaleFactor
            };
        }

        public static ItemComponentDO ToDO(ItemComponentDTO source)
        {
            if (source == null) return null;

            return new ItemComponentDO
            {
                Component = source.Component switch
                {
                    ArmorDTO armor => ToDO(armor),
                    WeaponDTO weapon => ToDO(weapon),
                    HorseDTO horse => ToDO(horse),
                    HorseHarnessDTO horseHarness => ToDO(horseHarness),
                    _ => source.Component
                }
            };
        }

        public static ArmorDO ToDO(ArmorDTO source)
        {
            if (source == null) return null;

            return new ArmorDO
            {
                HeadArmor = source.HeadArmor,
                BodyArmor = source.BodyArmor,
                LegArmor = source.LegArmor,
                ArmArmor = source.ArmArmor,
                HasGenderVariations = source.HasGenderVariations,
                HairCoverType = source.HairCoverType,
                BeardCoverType = source.BeardCoverType,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType,
                CoversBody = source.CoversBody,
                CoversLegs = source.CoversLegs,
                CoversHead = source.CoversHead,
                ManeCoverType = source.ManeCoverType,
                ReinsMesh = source.ReinsMesh,
                ManeuverBonus = source.ManeuverBonus,
                SpeedBonus = source.SpeedBonus,
                ChargeBonus = source.ChargeBonus,
                FamilyType = source.FamilyType,
                CoversHands = source.CoversHands,
                BodyMeshType = source.BodyMeshType
            };
        }

        public static WeaponDO ToDO(WeaponDTO source)
        {
            if (source == null) return null;

            return new WeaponDO
            {
                WeaponClass = source.WeaponClass,
                AmmoClass = source.AmmoClass,
                StackAmount = source.StackAmount,
                WeaponBalance = source.WeaponBalance,
                ThrustSpeed = source.ThrustSpeed,
                SpeedRating = source.SpeedRating,
                MissileSpeed = source.MissileSpeed,
                Accuracy = source.Accuracy,
                PhysicsMaterial = source.PhysicsMaterial,
                WeaponLength = source.WeaponLength,
                SwingDamage = source.SwingDamage,
                ThrustDamage = source.ThrustDamage,
                SwingDamageType = source.SwingDamageType,
                ThrustDamageType = source.ThrustDamageType,
                ItemUsage = source.ItemUsage,
                FlyingSoundCode = source.FlyingSoundCode,
                StickingRotation = source.StickingRotation,
                StickingPosition = source.StickingPosition,
                CenterOfMass = source.CenterOfMass,
                Position = source.Position,
                Rotation = source.Rotation,
                HitPoints = source.HitPoints,
                AmmoLimit = source.AmmoLimit,
                PassbySoundCode = source.PassbySoundCode,
                ReloadPhaseCount = source.ReloadPhaseCount,
                ModifierGroup = source.ModifierGroup,
                BodyArmor = source.BodyArmor,
                TrailParticleName = source.TrailParticleName,
                RotationSpeed = source.RotationSpeed,
                WeaponFlags = ToDO(source.WeaponFlags)
            };
        }

        public static WeaponFlagsDO ToDO(WeaponFlagsDTO source)
        {
            if (source == null) return null;

            return new WeaponFlagsDO
            {
                MeleeWeapon = source.MeleeWeapon,
                RangedWeapon = source.RangedWeapon,
                PenaltyWithShield = source.PenaltyWithShield,
                NotUsableWithOneHand = source.NotUsableWithOneHand,
                TwoHandIdleOnMount = source.TwoHandIdleOnMount,
                WideGrip = source.WideGrip,
                Consumable = source.Consumable,
                AmmoSticksWhenShot = source.AmmoSticksWhenShot,
                MultiplePenetration = source.MultiplePenetration,
                CanPenetrateShield = source.CanPenetrateShield,
                CanBlockRanged = source.CanBlockRanged,
                HasHitPoints = source.HasHitPoints,
                HasString = source.HasString,
                StringHeldByHand = source.StringHeldByHand,
                AutoReload = source.AutoReload,
                UnloadWhenSheathed = source.UnloadWhenSheathed,
                AmmoBreaksOnBounceBack = source.AmmoBreaksOnBounceBack,
                CantReloadOnHorseback = source.CantReloadOnHorseback,
                Burning = source.Burning,
                LeavesTrail = source.LeavesTrail,
                CanKnockDown = source.CanKnockDown,
                MissileWithPhysics = source.MissileWithPhysics,
                UseHandAsThrowBase = source.UseHandAsThrowBase,
                AffectsArea = source.AffectsArea,
                AmmoCanBreakOnBounceBack = source.AmmoCanBreakOnBounceBack,
                CanKillEvenIfBlunt = source.CanKillEvenIfBlunt,
                AffectsAreaBig = source.AffectsAreaBig,
                AttachAmmoToVisual = source.AttachAmmoToVisual
            };
        }

        public static HorseDO ToDO(HorseDTO source)
        {
            if (source == null) return null;

            return new HorseDO
            {
                Speed = source.Speed,
                Maneuver = source.Maneuver,
                ChargeDamage = source.ChargeDamage,
                HitPoints = source.HitPoints,
                BodyLength = source.BodyLength,
                IsMountable = source.IsMountable,
                Monster = source.Monster,
                ExtraHealth = source.ExtraHealth,
                SkeletonScale = source.SkeletonScale,
                ModifierGroup = source.ModifierGroup,
                AdditionalMeshes = ToDO(source.AdditionalMeshes),
                Materials = ToDO(source.Materials)
            };
        }

        public static AdditionalMeshesDO ToDO(AdditionalMeshesDTO source)
        {
            if (source == null) return null;

            return new AdditionalMeshesDO
            {
                Meshes = source.Meshes?.Select(ToDO).ToList() ?? new List<AdditionalMeshDO>()
            };
        }

        public static AdditionalMeshDO ToDO(AdditionalMeshDTO source)
        {
            if (source == null) return null;

            return new AdditionalMeshDO
            {
                Name = source.Name,
                AffectedByCover = source.AffectedByCover
            };
        }

        public static MaterialsDO ToDO(MaterialsDTO source)
        {
            if (source == null) return null;

            return new MaterialsDO
            {
                MaterialList = source.MaterialList?.Select(ToDO).ToList() ?? new List<MaterialDO>()
            };
        }

        public static MaterialDO ToDO(MaterialDTO source)
        {
            if (source == null) return null;

            return new MaterialDO
            {
                Name = source.Name,
                MeshMultipliers = ToDO(source.MeshMultipliers)
            };
        }

        public static MeshMultipliersDO ToDO(MeshMultipliersDTO source)
        {
            if (source == null) return null;

            return new MeshMultipliersDO
            {
                Multipliers = source.Multipliers?.Select(ToDO).ToList() ?? new List<MeshMultiplierDO>()
            };
        }

        public static MeshMultiplierDO ToDO(MeshMultiplierDTO source)
        {
            if (source == null) return null;

            return new MeshMultiplierDO
            {
                MeshMultiplierValue = source.MeshMultiplierValue,
                Percentage = source.Percentage
            };
        }

        public static HorseHarnessDO ToDO(HorseHarnessDTO source)
        {
            if (source == null) return null;

            return new HorseHarnessDO
            {
                BodyArmor = source.BodyArmor,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType
            };
        }

        public static ItemFlagsDO ToDO(ItemFlagsDTO source)
        {
            if (source == null) return null;

            return new ItemFlagsDO
            {
                UseTeamColor = source.UseTeamColor,
                Civilian = source.Civilian,
                DoesNotHideChest = source.DoesNotHideChest,
                WoodenParry = source.WoodenParry,
                DropOnWeaponChange = source.DropOnWeaponChange,
                DoNotScaleBodyAccordingToWeaponLength = source.DoNotScaleBodyAccordingToWeaponLength,
                QuickFadeOut = source.QuickFadeOut,
                CannotBePickedUp = source.CannotBePickedUp,
                HeldInOffHand = source.HeldInOffHand,
                ForceAttachOffHandSecondaryItemBone = source.ForceAttachOffHandSecondaryItemBone,
                ForceAttachOffHandPrimaryItemBone = source.ForceAttachOffHandPrimaryItemBone,
                DropOnAnyAction = source.DropOnAnyAction
            };
        }

        #endregion

        #region Helper Methods

        private static bool? BoolFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return bool.TryParse(value, out bool result) ? result : (bool?)null;
        }

        private static string StringFromBool(bool? value)
        {
            return value?.ToString().ToLower();
        }

        #endregion
    }
}