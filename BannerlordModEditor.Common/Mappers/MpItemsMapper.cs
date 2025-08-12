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
                MultiplayerItem = BoolFromString(source.MultiplayerItem) ?? false,
                Id = source.Id ?? string.Empty,
                Name = source.Name ?? string.Empty,
                BodyName = source.BodyName,
                ShieldBodyName = source.ShieldBodyName,
                HolsterBodyName = source.HolsterBodyName,
                Subtype = source.Subtype,
                Mesh = source.Mesh,
                HolsterMesh = source.HolsterMesh,
                Culture = source.Culture,
                UsingTableau = BoolFromString(source.UsingTableau) ?? false,
                Value = source.Value,
                IsMerchandise = BoolFromString(source.IsMerchandise) ?? false,
                Weight = source.Weight,
                Difficulty = source.Difficulty,
                Appearance = source.Appearance,
                Type = source.Type ?? string.Empty,
                ItemHolsters = source.ItemHolsters,
                ItemCategory = source.ItemCategory,
                RecalculateBody = BoolFromString(source.RecalculateBody) ?? false,
                HasLowerHolsterPriority = BoolFromString(source.HasLowerHolsterPriority) ?? false,
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
                MultiplayerItem = BoolFromString(source.MultiplayerItem) ?? false,
                Id = source.Id ?? string.Empty,
                Name = source.Name ?? string.Empty,
                CraftingTemplate = source.CraftingTemplate ?? string.Empty,
                Value = source.Value,
                IsMerchandise = BoolFromString(source.IsMerchandise) ?? false,
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
                HasGenderVariations = BoolFromString(source.HasGenderVariations) ?? false,
                HairCoverType = source.HairCoverType,
                BeardCoverType = source.BeardCoverType,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType,
                CoversBody = BoolFromString(source.CoversBody) ?? false,
                CoversLegs = BoolFromString(source.CoversLegs) ?? false,
                CoversHead = BoolFromString(source.CoversHead) ?? false,
                ManeCoverType = source.ManeCoverType,
                ReinsMesh = source.ReinsMesh,
                ManeuverBonus = source.ManeuverBonus,
                SpeedBonus = source.SpeedBonus,
                ChargeBonus = source.ChargeBonus,
                FamilyType = source.FamilyType,
                CoversHands = BoolFromString(source.CoversHands) ?? false,
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
                MeleeWeapon = BoolFromString(source.MeleeWeapon) ?? false,
                RangedWeapon = BoolFromString(source.RangedWeapon) ?? false,
                PenaltyWithShield = BoolFromString(source.PenaltyWithShield) ?? false,
                NotUsableWithOneHand = BoolFromString(source.NotUsableWithOneHand) ?? false,
                TwoHandIdleOnMount = BoolFromString(source.TwoHandIdleOnMount) ?? false,
                WideGrip = BoolFromString(source.WideGrip) ?? false,
                Consumable = BoolFromString(source.Consumable) ?? false,
                AmmoSticksWhenShot = BoolFromString(source.AmmoSticksWhenShot) ?? false,
                MultiplePenetration = BoolFromString(source.MultiplePenetration) ?? false,
                CanPenetrateShield = BoolFromString(source.CanPenetrateShield) ?? false,
                CanBlockRanged = BoolFromString(source.CanBlockRanged) ?? false,
                HasHitPoints = BoolFromString(source.HasHitPoints) ?? false,
                HasString = BoolFromString(source.HasString) ?? false,
                StringHeldByHand = BoolFromString(source.StringHeldByHand) ?? false,
                AutoReload = BoolFromString(source.AutoReload) ?? false,
                UnloadWhenSheathed = BoolFromString(source.UnloadWhenSheathed) ?? false,
                AmmoBreaksOnBounceBack = BoolFromString(source.AmmoBreaksOnBounceBack) ?? false,
                CantReloadOnHorseback = BoolFromString(source.CantReloadOnHorseback) ?? false,
                Burning = BoolFromString(source.Burning) ?? false,
                LeavesTrail = BoolFromString(source.LeavesTrail) ?? false,
                CanKnockDown = BoolFromString(source.CanKnockDown) ?? false,
                MissileWithPhysics = BoolFromString(source.MissileWithPhysics) ?? false,
                UseHandAsThrowBase = BoolFromString(source.UseHandAsThrowBase) ?? false,
                AffectsArea = BoolFromString(source.AffectsArea) ?? false,
                AmmoCanBreakOnBounceBack = BoolFromString(source.AmmoCanBreakOnBounceBack) ?? false,
                CanKillEvenIfBlunt = BoolFromString(source.CanKillEvenIfBlunt) ?? false,
                AffectsAreaBig = BoolFromString(source.AffectsAreaBig) ?? false,
                AttachAmmoToVisual = BoolFromString(source.AttachAmmoToVisual) ?? false
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
                IsMountable = BoolFromString(source.IsMountable) ?? false,
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
                Name = source.Name ?? string.Empty,
                AffectedByCover = BoolFromString(source.AffectedByCover) ?? false
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
                UseTeamColor = BoolFromString(source.UseTeamColor) ?? false,
                Civilian = BoolFromString(source.Civilian) ?? false,
                DoesNotHideChest = BoolFromString(source.DoesNotHideChest) ?? false,
                WoodenParry = BoolFromString(source.WoodenParry) ?? false,
                DropOnWeaponChange = BoolFromString(source.DropOnWeaponChange) ?? false,
                DoNotScaleBodyAccordingToWeaponLength = BoolFromString(source.DoNotScaleBodyAccordingToWeaponLength) ?? false,
                QuickFadeOut = BoolFromString(source.QuickFadeOut) ?? false,
                CannotBePickedUp = BoolFromString(source.CannotBePickedUp) ?? false,
                HeldInOffHand = BoolFromString(source.HeldInOffHand) ?? false,
                ForceAttachOffHandSecondaryItemBone = BoolFromString(source.ForceAttachOffHandSecondaryItemBone) ?? false,
                ForceAttachOffHandPrimaryItemBone = BoolFromString(source.ForceAttachOffHandPrimaryItemBone) ?? false,
                DropOnAnyAction = BoolFromString(source.DropOnAnyAction) ?? false
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
                HasGenderVariations = StringFromBool(source.HasGenderVariations),
                HairCoverType = source.HairCoverType,
                BeardCoverType = source.BeardCoverType,
                ModifierGroup = source.ModifierGroup,
                MaterialType = source.MaterialType,
                CoversBody = StringFromBool(source.CoversBody),
                CoversLegs = StringFromBool(source.CoversLegs),
                CoversHead = StringFromBool(source.CoversHead),
                ManeCoverType = source.ManeCoverType,
                ReinsMesh = source.ReinsMesh,
                ManeuverBonus = source.ManeuverBonus,
                SpeedBonus = source.SpeedBonus,
                ChargeBonus = source.ChargeBonus,
                FamilyType = source.FamilyType,
                CoversHands = StringFromBool(source.CoversHands),
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
                MeleeWeapon = StringFromBool(source.MeleeWeapon),
                RangedWeapon = StringFromBool(source.RangedWeapon),
                PenaltyWithShield = StringFromBool(source.PenaltyWithShield),
                NotUsableWithOneHand = StringFromBool(source.NotUsableWithOneHand),
                TwoHandIdleOnMount = StringFromBool(source.TwoHandIdleOnMount),
                WideGrip = StringFromBool(source.WideGrip),
                Consumable = StringFromBool(source.Consumable),
                AmmoSticksWhenShot = StringFromBool(source.AmmoSticksWhenShot),
                MultiplePenetration = StringFromBool(source.MultiplePenetration),
                CanPenetrateShield = StringFromBool(source.CanPenetrateShield),
                CanBlockRanged = StringFromBool(source.CanBlockRanged),
                HasHitPoints = StringFromBool(source.HasHitPoints),
                HasString = StringFromBool(source.HasString),
                StringHeldByHand = StringFromBool(source.StringHeldByHand),
                AutoReload = StringFromBool(source.AutoReload),
                UnloadWhenSheathed = StringFromBool(source.UnloadWhenSheathed),
                AmmoBreaksOnBounceBack = StringFromBool(source.AmmoBreaksOnBounceBack),
                CantReloadOnHorseback = StringFromBool(source.CantReloadOnHorseback),
                Burning = StringFromBool(source.Burning),
                LeavesTrail = StringFromBool(source.LeavesTrail),
                CanKnockDown = StringFromBool(source.CanKnockDown),
                MissileWithPhysics = StringFromBool(source.MissileWithPhysics),
                UseHandAsThrowBase = StringFromBool(source.UseHandAsThrowBase),
                AffectsArea = StringFromBool(source.AffectsArea),
                AmmoCanBreakOnBounceBack = StringFromBool(source.AmmoCanBreakOnBounceBack),
                CanKillEvenIfBlunt = StringFromBool(source.CanKillEvenIfBlunt),
                AffectsAreaBig = StringFromBool(source.AffectsAreaBig),
                AttachAmmoToVisual = StringFromBool(source.AttachAmmoToVisual)
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
                IsMountable = StringFromBool(source.IsMountable),
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
                AffectedByCover = StringFromBool(source.AffectedByCover)
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
                UseTeamColor = StringFromBool(source.UseTeamColor),
                Civilian = StringFromBool(source.Civilian),
                DoesNotHideChest = StringFromBool(source.DoesNotHideChest),
                WoodenParry = StringFromBool(source.WoodenParry),
                DropOnWeaponChange = StringFromBool(source.DropOnWeaponChange),
                DoNotScaleBodyAccordingToWeaponLength = StringFromBool(source.DoNotScaleBodyAccordingToWeaponLength),
                QuickFadeOut = StringFromBool(source.QuickFadeOut),
                CannotBePickedUp = StringFromBool(source.CannotBePickedUp),
                HeldInOffHand = StringFromBool(source.HeldInOffHand),
                ForceAttachOffHandSecondaryItemBone = StringFromBool(source.ForceAttachOffHandSecondaryItemBone),
                ForceAttachOffHandPrimaryItemBone = StringFromBool(source.ForceAttachOffHandPrimaryItemBone),
                DropOnAnyAction = StringFromBool(source.DropOnAnyAction)
            };
        }

        #endregion

        #region Helper Methods

        private static bool? BoolFromString(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out bool result) ? result : (bool?)null
            };
        }

        private static string StringFromBool(bool value)
        {
            return value.ToString().ToLower();
        }

        #endregion
    }
}