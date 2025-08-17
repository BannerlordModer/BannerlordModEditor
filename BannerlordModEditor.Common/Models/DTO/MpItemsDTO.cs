using System;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class MpItemsDTO
    {
        public List<MpItemDTO> Items { get; set; } = new List<MpItemDTO>();
        public List<CraftedItemDTO> CraftedItems { get; set; } = new List<CraftedItemDTO>();
    }

    public class MpItemDTO
    {
        public bool MultiplayerItem { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? BodyName { get; set; }
        public string? ShieldBodyName { get; set; }
        public string? HolsterBodyName { get; set; }
        public string? Subtype { get; set; }
        public string? Mesh { get; set; }
        public string? HolsterMesh { get; set; }
        public string? Culture { get; set; }
        public bool UsingTableau { get; set; }
        public string? Value { get; set; }
        public bool IsMerchandise { get; set; }
        public string? Weight { get; set; }
        public string? Difficulty { get; set; }
        public string? Appearance { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? ItemHolsters { get; set; }
        public string? ItemCategory { get; set; }
        public bool RecalculateBody { get; set; }
        public bool HasLowerHolsterPriority { get; set; }
        public string? HolsterPositionShift { get; set; }
        public string? FlyingMesh { get; set; }
        public string? HolsterMeshWithWeapon { get; set; }
        public string? AmmoOffset { get; set; }
        public string? Prefab { get; set; }
        public string? LodAtlasIndex { get; set; }

        public ItemComponentDTO? ItemComponent { get; set; }
        public ItemFlagsDTO? Flags { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? ValueInt => int.TryParse(Value, out int value) ? value : (int?)null;
        public double? WeightDouble => double.TryParse(Weight, out double weight) ? weight : (double?)null;
        public int? DifficultyInt => int.TryParse(Difficulty, out int difficulty) ? difficulty : (int?)null;
        public double? AppearanceDouble => double.TryParse(Appearance, out double appearance) ? appearance : (double?)null;
        public int? LodAtlasIndexInt => int.TryParse(LodAtlasIndex, out int lodAtlasIndex) ? lodAtlasIndex : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetValueInt(int? value) => Value = value?.ToString();
        public void SetWeightDouble(double? weight) => Weight = weight?.ToString();
        public void SetDifficultyInt(int? difficulty) => Difficulty = difficulty?.ToString();
        public void SetAppearanceDouble(double? appearance) => Appearance = appearance?.ToString();
        public void SetLodAtlasIndexInt(int? lodAtlasIndex) => LodAtlasIndex = lodAtlasIndex?.ToString();
    }

    public class CraftedItemDTO
    {
        public bool MultiplayerItem { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CraftingTemplate { get; set; } = string.Empty;
        public string? Value { get; set; }
        public bool IsMerchandise { get; set; }
        public string? Culture { get; set; }
        public PiecesDTO? Pieces { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? ValueInt => int.TryParse(Value, out int value) ? value : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetValueInt(int? value) => Value = value?.ToString();
    }

    public class PiecesDTO
    {
        public List<PieceDTO> PieceList { get; set; } = new List<PieceDTO>();
    }

    public class PieceDTO
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? ScaleFactor { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? ScaleFactorInt => int.TryParse(ScaleFactor, out int scaleFactor) ? scaleFactor : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetScaleFactorInt(int? scaleFactor) => ScaleFactor = scaleFactor?.ToString();
    }

    public class ItemComponentDTO
    {
        public object? Component { get; set; }
    }

    public class ArmorDTO
    {
        public string? HeadArmor { get; set; }
        public string? BodyArmor { get; set; }
        public string? LegArmor { get; set; }
        public string? ArmArmor { get; set; }
        public bool HasGenderVariations { get; set; }
        public string? HairCoverType { get; set; }
        public string? BeardCoverType { get; set; }
        public string? ModifierGroup { get; set; }
        public string? MaterialType { get; set; }
        public bool CoversBody { get; set; }
        public bool CoversLegs { get; set; }
        public bool CoversHead { get; set; }
        public string? ManeCoverType { get; set; }
        public string? ReinsMesh { get; set; }
        public string? ManeuverBonus { get; set; }
        public string? SpeedBonus { get; set; }
        public string? ChargeBonus { get; set; }
        public string? FamilyType { get; set; }
        public bool CoversHands { get; set; }
        public string? BodyMeshType { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? HeadArmorInt => int.TryParse(HeadArmor, out int headArmor) ? headArmor : (int?)null;
        public int? BodyArmorInt => int.TryParse(BodyArmor, out int bodyArmor) ? bodyArmor : (int?)null;
        public int? LegArmorInt => int.TryParse(LegArmor, out int legArmor) ? legArmor : (int?)null;
        public int? ArmArmorInt => int.TryParse(ArmArmor, out int armArmor) ? armArmor : (int?)null;
        public int? ManeuverBonusInt => int.TryParse(ManeuverBonus, out int maneuverBonus) ? maneuverBonus : (int?)null;
        public int? SpeedBonusInt => int.TryParse(SpeedBonus, out int speedBonus) ? speedBonus : (int?)null;
        public int? ChargeBonusInt => int.TryParse(ChargeBonus, out int chargeBonus) ? chargeBonus : (int?)null;
        public int? FamilyTypeInt => int.TryParse(FamilyType, out int familyType) ? familyType : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetHeadArmorInt(int? headArmor) => HeadArmor = headArmor?.ToString();
        public void SetBodyArmorInt(int? bodyArmor) => BodyArmor = bodyArmor?.ToString();
        public void SetLegArmorInt(int? legArmor) => LegArmor = legArmor?.ToString();
        public void SetArmArmorInt(int? armArmor) => ArmArmor = armArmor?.ToString();
        public void SetManeuverBonusInt(int? maneuverBonus) => ManeuverBonus = maneuverBonus?.ToString();
        public void SetSpeedBonusInt(int? speedBonus) => SpeedBonus = speedBonus?.ToString();
        public void SetChargeBonusInt(int? chargeBonus) => ChargeBonus = chargeBonus?.ToString();
        public void SetFamilyTypeInt(int? familyType) => FamilyType = familyType?.ToString();
    }

    public class WeaponDTO
    {
        public string? WeaponClass { get; set; }
        public string? AmmoClass { get; set; }
        public string? StackAmount { get; set; }
        public string? WeaponBalance { get; set; }
        public string? ThrustSpeed { get; set; }
        public string? SpeedRating { get; set; }
        public string? MissileSpeed { get; set; }
        public string? Accuracy { get; set; }
        public string? PhysicsMaterial { get; set; }
        public string? WeaponLength { get; set; }
        public string? SwingDamage { get; set; }
        public string? ThrustDamage { get; set; }
        public string? SwingDamageType { get; set; }
        public string? ThrustDamageType { get; set; }
        public string? ItemUsage { get; set; }
        public string? FlyingSoundCode { get; set; }
        public string? StickingRotation { get; set; }
        public string? StickingPosition { get; set; }
        public string? CenterOfMass { get; set; }
        public string? Position { get; set; }
        public string? Rotation { get; set; }
        public string? HitPoints { get; set; }
        public string? AmmoLimit { get; set; }
        public string? PassbySoundCode { get; set; }
        public string? ReloadPhaseCount { get; set; }
        public string? ModifierGroup { get; set; }
        public string? BodyArmor { get; set; }
        public string? TrailParticleName { get; set; }
        public string? RotationSpeed { get; set; }
        public WeaponFlagsDTO? WeaponFlags { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? StackAmountInt => int.TryParse(StackAmount, out int stackAmount) ? stackAmount : (int?)null;
        public int? WeaponBalanceInt => int.TryParse(WeaponBalance, out int weaponBalance) ? weaponBalance : (int?)null;
        public int? ThrustSpeedInt => int.TryParse(ThrustSpeed, out int thrustSpeed) ? thrustSpeed : (int?)null;
        public int? SpeedRatingInt => int.TryParse(SpeedRating, out int speedRating) ? speedRating : (int?)null;
        public int? MissileSpeedInt => int.TryParse(MissileSpeed, out int missileSpeed) ? missileSpeed : (int?)null;
        public int? AccuracyInt => int.TryParse(Accuracy, out int accuracy) ? accuracy : (int?)null;
        public int? WeaponLengthInt => int.TryParse(WeaponLength, out int weaponLength) ? weaponLength : (int?)null;
        public int? SwingDamageInt => int.TryParse(SwingDamage, out int swingDamage) ? swingDamage : (int?)null;
        public int? ThrustDamageInt => int.TryParse(ThrustDamage, out int thrustDamage) ? thrustDamage : (int?)null;
        public int? HitPointsInt => int.TryParse(HitPoints, out int hitPoints) ? hitPoints : (int?)null;
        public int? AmmoLimitInt => int.TryParse(AmmoLimit, out int ammoLimit) ? ammoLimit : (int?)null;
        public int? ReloadPhaseCountInt => int.TryParse(ReloadPhaseCount, out int reloadPhaseCount) ? reloadPhaseCount : (int?)null;
        public int? BodyArmorInt => int.TryParse(BodyArmor, out int bodyArmor) ? bodyArmor : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetStackAmountInt(int? stackAmount) => StackAmount = stackAmount?.ToString();
        public void SetWeaponBalanceInt(int? weaponBalance) => WeaponBalance = weaponBalance?.ToString();
        public void SetThrustSpeedInt(int? thrustSpeed) => ThrustSpeed = thrustSpeed?.ToString();
        public void SetSpeedRatingInt(int? speedRating) => SpeedRating = speedRating?.ToString();
        public void SetMissileSpeedInt(int? missileSpeed) => MissileSpeed = missileSpeed?.ToString();
        public void SetAccuracyInt(int? accuracy) => Accuracy = accuracy?.ToString();
        public void SetWeaponLengthInt(int? weaponLength) => WeaponLength = weaponLength?.ToString();
        public void SetSwingDamageInt(int? swingDamage) => SwingDamage = swingDamage?.ToString();
        public void SetThrustDamageInt(int? thrustDamage) => ThrustDamage = thrustDamage?.ToString();
        public void SetHitPointsInt(int? hitPoints) => HitPoints = hitPoints?.ToString();
        public void SetAmmoLimitInt(int? ammoLimit) => AmmoLimit = ammoLimit?.ToString();
        public void SetReloadPhaseCountInt(int? reloadPhaseCount) => ReloadPhaseCount = reloadPhaseCount?.ToString();
        public void SetBodyArmorInt(int? bodyArmor) => BodyArmor = bodyArmor?.ToString();
    }

    public class WeaponFlagsDTO
    {
        public bool MeleeWeapon { get; set; }
        public bool RangedWeapon { get; set; }
        public bool PenaltyWithShield { get; set; }
        public bool NotUsableWithOneHand { get; set; }
        public bool TwoHandIdleOnMount { get; set; }
        public bool WideGrip { get; set; }
        public bool Consumable { get; set; }
        public bool AmmoSticksWhenShot { get; set; }
        public bool MultiplePenetration { get; set; }
        public bool CanPenetrateShield { get; set; }
        public bool CanBlockRanged { get; set; }
        public bool HasHitPoints { get; set; }
        public bool HasString { get; set; }
        public bool StringHeldByHand { get; set; }
        public bool AutoReload { get; set; }
        public bool UnloadWhenSheathed { get; set; }
        public bool AmmoBreaksOnBounceBack { get; set; }
        public bool CantReloadOnHorseback { get; set; }
        public bool Burning { get; set; }
        public bool LeavesTrail { get; set; }
        public bool CanKnockDown { get; set; }
        public bool MissileWithPhysics { get; set; }
        public bool UseHandAsThrowBase { get; set; }
        public bool AffectsArea { get; set; }
        public bool AmmoCanBreakOnBounceBack { get; set; }
        public bool CanKillEvenIfBlunt { get; set; }
        public bool AffectsAreaBig { get; set; }
        public bool AttachAmmoToVisual { get; set; }
    }

    public class HorseDTO
    {
        public string? Speed { get; set; }
        public string? Maneuver { get; set; }
        public string? ChargeDamage { get; set; }
        public string? HitPoints { get; set; }
        public string? BodyLength { get; set; }
        public bool IsMountable { get; set; }
        public string? Monster { get; set; }
        public string? ExtraHealth { get; set; }
        public string? SkeletonScale { get; set; }
        public string? ModifierGroup { get; set; }
        public AdditionalMeshesDTO? AdditionalMeshes { get; set; }
        public MpItemMaterialsDTO? Materials { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? SpeedInt => int.TryParse(Speed, out int speed) ? speed : (int?)null;
        public int? ManeuverInt => int.TryParse(Maneuver, out int maneuver) ? maneuver : (int?)null;
        public int? ChargeDamageInt => int.TryParse(ChargeDamage, out int chargeDamage) ? chargeDamage : (int?)null;
        public int? HitPointsInt => int.TryParse(HitPoints, out int hitPoints) ? hitPoints : (int?)null;
        public int? BodyLengthInt => int.TryParse(BodyLength, out int bodyLength) ? bodyLength : (int?)null;
        public int? ExtraHealthInt => int.TryParse(ExtraHealth, out int extraHealth) ? extraHealth : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetSpeedInt(int? speed) => Speed = speed?.ToString();
        public void SetManeuverInt(int? maneuver) => Maneuver = maneuver?.ToString();
        public void SetChargeDamageInt(int? chargeDamage) => ChargeDamage = chargeDamage?.ToString();
        public void SetHitPointsInt(int? hitPoints) => HitPoints = hitPoints?.ToString();
        public void SetBodyLengthInt(int? bodyLength) => BodyLength = bodyLength?.ToString();
        public void SetExtraHealthInt(int? extraHealth) => ExtraHealth = extraHealth?.ToString();
    }

    public class AdditionalMeshesDTO
    {
        public List<AdditionalMeshDTO> Meshes { get; set; } = new List<AdditionalMeshDTO>();
    }

    public class AdditionalMeshDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool AffectedByCover { get; set; }
    }

    public class MpItemMaterialsDTO
    {
        public List<MpItemMaterialDTO> MaterialList { get; set; } = new List<MpItemMaterialDTO>();
    }

    public class MpItemMaterialDTO
    {
        public string? Name { get; set; }
        public MpItemMeshMultipliersDTO? MeshMultipliers { get; set; }
    }

    public class MpItemMeshMultipliersDTO
    {
        public List<MpItemMeshMultiplierDTO> Multipliers { get; set; } = new List<MpItemMeshMultiplierDTO>();
    }

    public class MpItemMeshMultiplierDTO
    {
        public string? MeshMultiplierValue { get; set; }
        public string? Percentage { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public double? PercentageDouble => double.TryParse(Percentage, out double percentage) ? percentage : (double?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetPercentageDouble(double? percentage) => Percentage = percentage?.ToString();
    }

    public class HorseHarnessDTO
    {
        public string? BodyArmor { get; set; }
        public string? ModifierGroup { get; set; }
        public string? MaterialType { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? BodyArmorInt => int.TryParse(BodyArmor, out int bodyArmor) ? bodyArmor : (int?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetBodyArmorInt(int? bodyArmor) => BodyArmor = bodyArmor?.ToString();
    }

    public class ItemFlagsDTO
    {
        public bool UseTeamColor { get; set; }
        public bool Civilian { get; set; }
        public bool DoesNotHideChest { get; set; }
        public bool WoodenParry { get; set; }
        public bool DropOnWeaponChange { get; set; }
        public bool DoNotScaleBodyAccordingToWeaponLength { get; set; }
        public bool QuickFadeOut { get; set; }
        public bool CannotBePickedUp { get; set; }
        public bool HeldInOffHand { get; set; }
        public bool ForceAttachOffHandSecondaryItemBone { get; set; }
        public bool ForceAttachOffHandPrimaryItemBone { get; set; }
        public bool DropOnAnyAction { get; set; }
    }
}