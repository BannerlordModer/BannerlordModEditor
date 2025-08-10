using System;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class MpItemsDTO
    {
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<CraftedItemDTO> CraftedItems { get; set; } = new List<CraftedItemDTO>();
    }

    public class ItemDTO
    {
        public bool? MultiplayerItem { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? BodyName { get; set; }
        public string? ShieldBodyName { get; set; }
        public string? HolsterBodyName { get; set; }
        public string? Subtype { get; set; }
        public string? Mesh { get; set; }
        public string? HolsterMesh { get; set; }
        public string? Culture { get; set; }
        public bool? UsingTableau { get; set; }
        public string? Value { get; set; }
        public bool? IsMerchandise { get; set; }
        public string? Weight { get; set; }
        public string? Difficulty { get; set; }
        public string? Appearance { get; set; }
        public string? Type { get; set; }
        public string? ItemHolsters { get; set; }
        public string? ItemCategory { get; set; }
        public bool? RecalculateBody { get; set; }
        public bool? HasLowerHolsterPriority { get; set; }
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
        public bool? MultiplayerItem { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? CraftingTemplate { get; set; }
        public string? Value { get; set; }
        public bool? IsMerchandise { get; set; }
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
        public string? HasGenderVariations { get; set; }
        public string? HairCoverType { get; set; }
        public string? BeardCoverType { get; set; }
        public string? ModifierGroup { get; set; }
        public string? MaterialType { get; set; }
        public string? CoversBody { get; set; }
        public string? CoversLegs { get; set; }
        public string? CoversHead { get; set; }
        public string? ManeCoverType { get; set; }
        public string? ReinsMesh { get; set; }
        public string? ManeuverBonus { get; set; }
        public string? SpeedBonus { get; set; }
        public string? ChargeBonus { get; set; }
        public string? FamilyType { get; set; }
        public string? CoversHands { get; set; }
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

        // 布尔类型的便捷属性（基于字符串属性）
        public bool? HasGenderVariationsBool => bool.TryParse(HasGenderVariations, out bool hasGenderVariations) ? hasGenderVariations : (bool?)null;
        public bool? CoversBodyBool => bool.TryParse(CoversBody, out bool coversBody) ? coversBody : (bool?)null;
        public bool? CoversLegsBool => bool.TryParse(CoversLegs, out bool coversLegs) ? coversLegs : (bool?)null;
        public bool? CoversHeadBool => bool.TryParse(CoversHead, out bool coversHead) ? coversHead : (bool?)null;
        public bool? CoversHandsBool => bool.TryParse(CoversHands, out bool coversHands) ? coversHands : (bool?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetHeadArmorInt(int? headArmor) => HeadArmor = headArmor?.ToString();
        public void SetBodyArmorInt(int? bodyArmor) => BodyArmor = bodyArmor?.ToString();
        public void SetLegArmorInt(int? legArmor) => LegArmor = legArmor?.ToString();
        public void SetArmArmorInt(int? armArmor) => ArmArmor = armArmor?.ToString();
        public void SetManeuverBonusInt(int? maneuverBonus) => ManeuverBonus = maneuverBonus?.ToString();
        public void SetSpeedBonusInt(int? speedBonus) => SpeedBonus = speedBonus?.ToString();
        public void SetChargeBonusInt(int? chargeBonus) => ChargeBonus = chargeBonus?.ToString();
        public void SetFamilyTypeInt(int? familyType) => FamilyType = familyType?.ToString();

        // 设置布尔属性的方法（自动转换为字符串）
        public void SetHasGenderVariationsBool(bool? hasGenderVariations) => HasGenderVariations = hasGenderVariations?.ToString().ToLower();
        public void SetCoversBodyBool(bool? coversBody) => CoversBody = coversBody?.ToString().ToLower();
        public void SetCoversLegsBool(bool? coversLegs) => CoversLegs = coversLegs?.ToString().ToLower();
        public void SetCoversHeadBool(bool? coversHead) => CoversHead = coversHead?.ToString().ToLower();
        public void SetCoversHandsBool(bool? coversHands) => CoversHands = coversHands?.ToString().ToLower();
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
        public string? MeleeWeapon { get; set; }
        public string? RangedWeapon { get; set; }
        public string? PenaltyWithShield { get; set; }
        public string? NotUsableWithOneHand { get; set; }
        public string? TwoHandIdleOnMount { get; set; }
        public string? WideGrip { get; set; }
        public string? Consumable { get; set; }
        public string? AmmoSticksWhenShot { get; set; }
        public string? MultiplePenetration { get; set; }
        public string? CanPenetrateShield { get; set; }
        public string? CanBlockRanged { get; set; }
        public string? HasHitPoints { get; set; }
        public string? HasString { get; set; }
        public string? StringHeldByHand { get; set; }
        public string? AutoReload { get; set; }
        public string? UnloadWhenSheathed { get; set; }
        public string? AmmoBreaksOnBounceBack { get; set; }
        public string? CantReloadOnHorseback { get; set; }
        public string? Burning { get; set; }
        public string? LeavesTrail { get; set; }
        public string? CanKnockDown { get; set; }
        public string? MissileWithPhysics { get; set; }
        public string? UseHandAsThrowBase { get; set; }
        public string? AffectsArea { get; set; }
        public string? AmmoCanBreakOnBounceBack { get; set; }
        public string? CanKillEvenIfBlunt { get; set; }
        public string? AffectsAreaBig { get; set; }
        public string? AttachAmmoToVisual { get; set; }

        // 布尔类型的便捷属性（基于字符串属性）
        public bool? MeleeWeaponBool => bool.TryParse(MeleeWeapon, out bool meleeWeapon) ? meleeWeapon : (bool?)null;
        public bool? RangedWeaponBool => bool.TryParse(RangedWeapon, out bool rangedWeapon) ? rangedWeapon : (bool?)null;
        public bool? PenaltyWithShieldBool => bool.TryParse(PenaltyWithShield, out bool penaltyWithShield) ? penaltyWithShield : (bool?)null;
        public bool? NotUsableWithOneHandBool => bool.TryParse(NotUsableWithOneHand, out bool notUsableWithOneHand) ? notUsableWithOneHand : (bool?)null;
        public bool? TwoHandIdleOnMountBool => bool.TryParse(TwoHandIdleOnMount, out bool twoHandIdleOnMount) ? twoHandIdleOnMount : (bool?)null;
        public bool? WideGripBool => bool.TryParse(WideGrip, out bool wideGrip) ? wideGrip : (bool?)null;
        public bool? ConsumableBool => bool.TryParse(Consumable, out bool consumable) ? consumable : (bool?)null;
        public bool? AmmoSticksWhenShotBool => bool.TryParse(AmmoSticksWhenShot, out bool ammoSticksWhenShot) ? ammoSticksWhenShot : (bool?)null;
        public bool? MultiplePenetrationBool => bool.TryParse(MultiplePenetration, out bool multiplePenetration) ? multiplePenetration : (bool?)null;
        public bool? CanPenetrateShieldBool => bool.TryParse(CanPenetrateShield, out bool canPenetrateShield) ? canPenetrateShield : (bool?)null;
        public bool? CanBlockRangedBool => bool.TryParse(CanBlockRanged, out bool canBlockRanged) ? canBlockRanged : (bool?)null;
        public bool? HasHitPointsBool => bool.TryParse(HasHitPoints, out bool hasHitPoints) ? hasHitPoints : (bool?)null;
        public bool? HasStringBool => bool.TryParse(HasString, out bool hasString) ? hasString : (bool?)null;
        public bool? StringHeldByHandBool => bool.TryParse(StringHeldByHand, out bool stringHeldByHand) ? stringHeldByHand : (bool?)null;
        public bool? AutoReloadBool => bool.TryParse(AutoReload, out bool autoReload) ? autoReload : (bool?)null;
        public bool? UnloadWhenSheathedBool => bool.TryParse(UnloadWhenSheathed, out bool unloadWhenSheathed) ? unloadWhenSheathed : (bool?)null;
        public bool? AmmoBreaksOnBounceBackBool => bool.TryParse(AmmoBreaksOnBounceBack, out bool ammoBreaksOnBounceBack) ? ammoBreaksOnBounceBack : (bool?)null;
        public bool? CantReloadOnHorsebackBool => bool.TryParse(CantReloadOnHorseback, out bool cantReloadOnHorseback) ? cantReloadOnHorseback : (bool?)null;
        public bool? BurningBool => bool.TryParse(Burning, out bool burning) ? burning : (bool?)null;
        public bool? LeavesTrailBool => bool.TryParse(LeavesTrail, out bool leavesTrail) ? leavesTrail : (bool?)null;
        public bool? CanKnockDownBool => bool.TryParse(CanKnockDown, out bool canKnockDown) ? canKnockDown : (bool?)null;
        public bool? MissileWithPhysicsBool => bool.TryParse(MissileWithPhysics, out bool missileWithPhysics) ? missileWithPhysics : (bool?)null;
        public bool? UseHandAsThrowBaseBool => bool.TryParse(UseHandAsThrowBase, out bool useHandAsThrowBase) ? useHandAsThrowBase : (bool?)null;
        public bool? AffectsAreaBool => bool.TryParse(AffectsArea, out bool affectsArea) ? affectsArea : (bool?)null;
        public bool? AmmoCanBreakOnBounceBackBool => bool.TryParse(AmmoCanBreakOnBounceBack, out bool ammoCanBreakOnBounceBack) ? ammoCanBreakOnBounceBack : (bool?)null;
        public bool? CanKillEvenIfBluntBool => bool.TryParse(CanKillEvenIfBlunt, out bool canKillEvenIfBlunt) ? canKillEvenIfBlunt : (bool?)null;
        public bool? AffectsAreaBigBool => bool.TryParse(AffectsAreaBig, out bool affectsAreaBig) ? affectsAreaBig : (bool?)null;
        public bool? AttachAmmoToVisualBool => bool.TryParse(AttachAmmoToVisual, out bool attachAmmoToVisual) ? attachAmmoToVisual : (bool?)null;

        // 设置布尔属性的方法（自动转换为字符串）
        public void SetMeleeWeaponBool(bool? meleeWeapon) => MeleeWeapon = meleeWeapon?.ToString().ToLower();
        public void SetRangedWeaponBool(bool? rangedWeapon) => RangedWeapon = rangedWeapon?.ToString().ToLower();
        public void SetPenaltyWithShieldBool(bool? penaltyWithShield) => PenaltyWithShield = penaltyWithShield?.ToString().ToLower();
        public void SetNotUsableWithOneHandBool(bool? notUsableWithOneHand) => NotUsableWithOneHand = notUsableWithOneHand?.ToString().ToLower();
        public void SetTwoHandIdleOnMountBool(bool? twoHandIdleOnMount) => TwoHandIdleOnMount = twoHandIdleOnMount?.ToString().ToLower();
        public void SetWideGripBool(bool? wideGrip) => WideGrip = wideGrip?.ToString().ToLower();
        public void SetConsumableBool(bool? consumable) => Consumable = consumable?.ToString().ToLower();
        public void SetAmmoSticksWhenShotBool(bool? ammoSticksWhenShot) => AmmoSticksWhenShot = ammoSticksWhenShot?.ToString().ToLower();
        public void SetMultiplePenetrationBool(bool? multiplePenetration) => MultiplePenetration = multiplePenetration?.ToString().ToLower();
        public void SetCanPenetrateShieldBool(bool? canPenetrateShield) => CanPenetrateShield = canPenetrateShield?.ToString().ToLower();
        public void SetCanBlockRangedBool(bool? canBlockRanged) => CanBlockRanged = canBlockRanged?.ToString().ToLower();
        public void SetHasHitPointsBool(bool? hasHitPoints) => HasHitPoints = hasHitPoints?.ToString().ToLower();
        public void SetHasStringBool(bool? hasString) => HasString = hasString?.ToString().ToLower();
        public void SetStringHeldByHandBool(bool? stringHeldByHand) => StringHeldByHand = stringHeldByHand?.ToString().ToLower();
        public void SetAutoReloadBool(bool? autoReload) => AutoReload = autoReload?.ToString().ToLower();
        public void SetUnloadWhenSheathedBool(bool? unloadWhenSheathed) => UnloadWhenSheathed = unloadWhenSheathed?.ToString().ToLower();
        public void SetAmmoBreaksOnBounceBackBool(bool? ammoBreaksOnBounceBack) => AmmoBreaksOnBounceBack = ammoBreaksOnBounceBack?.ToString().ToLower();
        public void SetCantReloadOnHorsebackBool(bool? cantReloadOnHorseback) => CantReloadOnHorseback = cantReloadOnHorseback?.ToString().ToLower();
        public void SetBurningBool(bool? burning) => Burning = burning?.ToString().ToLower();
        public void SetLeavesTrailBool(bool? leavesTrail) => LeavesTrail = leavesTrail?.ToString().ToLower();
        public void SetCanKnockDownBool(bool? canKnockDown) => CanKnockDown = canKnockDown?.ToString().ToLower();
        public void SetMissileWithPhysicsBool(bool? missileWithPhysics) => MissileWithPhysics = missileWithPhysics?.ToString().ToLower();
        public void SetUseHandAsThrowBaseBool(bool? useHandAsThrowBase) => UseHandAsThrowBase = useHandAsThrowBase?.ToString().ToLower();
        public void SetAffectsAreaBool(bool? affectsArea) => AffectsArea = affectsArea?.ToString().ToLower();
        public void SetAmmoCanBreakOnBounceBackBool(bool? ammoCanBreakOnBounceBack) => AmmoCanBreakOnBounceBack = ammoCanBreakOnBounceBack?.ToString().ToLower();
        public void SetCanKillEvenIfBluntBool(bool? canKillEvenIfBlunt) => CanKillEvenIfBlunt = canKillEvenIfBlunt?.ToString().ToLower();
        public void SetAffectsAreaBigBool(bool? affectsAreaBig) => AffectsAreaBig = affectsAreaBig?.ToString().ToLower();
        public void SetAttachAmmoToVisualBool(bool? attachAmmoToVisual) => AttachAmmoToVisual = attachAmmoToVisual?.ToString().ToLower();
    }

    public class HorseDTO
    {
        public string? Speed { get; set; }
        public string? Maneuver { get; set; }
        public string? ChargeDamage { get; set; }
        public string? HitPoints { get; set; }
        public string? BodyLength { get; set; }
        public string? IsMountable { get; set; }
        public string? Monster { get; set; }
        public string? ExtraHealth { get; set; }
        public string? SkeletonScale { get; set; }
        public string? ModifierGroup { get; set; }
        public AdditionalMeshesDTO? AdditionalMeshes { get; set; }
        public MaterialsDTO? Materials { get; set; }

        // 数值类型的便捷属性（基于字符串属性）
        public int? SpeedInt => int.TryParse(Speed, out int speed) ? speed : (int?)null;
        public int? ManeuverInt => int.TryParse(Maneuver, out int maneuver) ? maneuver : (int?)null;
        public int? ChargeDamageInt => int.TryParse(ChargeDamage, out int chargeDamage) ? chargeDamage : (int?)null;
        public int? HitPointsInt => int.TryParse(HitPoints, out int hitPoints) ? hitPoints : (int?)null;
        public int? BodyLengthInt => int.TryParse(BodyLength, out int bodyLength) ? bodyLength : (int?)null;
        public int? ExtraHealthInt => int.TryParse(ExtraHealth, out int extraHealth) ? extraHealth : (int?)null;

        // 布尔类型的便捷属性（基于字符串属性）
        public bool? IsMountableBool => bool.TryParse(IsMountable, out bool isMountable) ? isMountable : (bool?)null;

        // 设置数值属性的方法（自动转换为字符串）
        public void SetSpeedInt(int? speed) => Speed = speed?.ToString();
        public void SetManeuverInt(int? maneuver) => Maneuver = maneuver?.ToString();
        public void SetChargeDamageInt(int? chargeDamage) => ChargeDamage = chargeDamage?.ToString();
        public void SetHitPointsInt(int? hitPoints) => HitPoints = hitPoints?.ToString();
        public void SetBodyLengthInt(int? bodyLength) => BodyLength = bodyLength?.ToString();
        public void SetExtraHealthInt(int? extraHealth) => ExtraHealth = extraHealth?.ToString();

        // 设置布尔属性的方法（自动转换为字符串）
        public void SetIsMountableBool(bool? isMountable) => IsMountable = isMountable?.ToString().ToLower();
    }

    public class AdditionalMeshesDTO
    {
        public List<AdditionalMeshDTO> Meshes { get; set; } = new List<AdditionalMeshDTO>();
    }

    public class AdditionalMeshDTO
    {
        public string? Name { get; set; }
        public string? AffectedByCover { get; set; }

        // 布尔类型的便捷属性（基于字符串属性）
        public bool? AffectedByCoverBool => bool.TryParse(AffectedByCover, out bool affectedByCover) ? affectedByCover : (bool?)null;

        // 设置布尔属性的方法（自动转换为字符串）
        public void SetAffectedByCoverBool(bool? affectedByCover) => AffectedByCover = affectedByCover?.ToString().ToLower();
    }

    public class MaterialsDTO
    {
        public List<MaterialDTO> MaterialList { get; set; } = new List<MaterialDTO>();
    }

    public class MaterialDTO
    {
        public string? Name { get; set; }
        public MeshMultipliersDTO? MeshMultipliers { get; set; }
    }

    public class MeshMultipliersDTO
    {
        public List<MeshMultiplierDTO> Multipliers { get; set; } = new List<MeshMultiplierDTO>();
    }

    public class MeshMultiplierDTO
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
        public string? UseTeamColor { get; set; }
        public string? Civilian { get; set; }
        public string? DoesNotHideChest { get; set; }
        public string? WoodenParry { get; set; }
        public string? DropOnWeaponChange { get; set; }
        public string? DoNotScaleBodyAccordingToWeaponLength { get; set; }
        public string? QuickFadeOut { get; set; }
        public string? CannotBePickedUp { get; set; }
        public string? HeldInOffHand { get; set; }
        public string? ForceAttachOffHandSecondaryItemBone { get; set; }
        public string? ForceAttachOffHandPrimaryItemBone { get; set; }
        public string? DropOnAnyAction { get; set; }

        // 布尔类型的便捷属性（基于字符串属性）
        public bool? UseTeamColorBool => bool.TryParse(UseTeamColor, out bool useTeamColor) ? useTeamColor : (bool?)null;
        public bool? CivilianBool => bool.TryParse(Civilian, out bool civilian) ? civilian : (bool?)null;
        public bool? DoesNotHideChestBool => bool.TryParse(DoesNotHideChest, out bool doesNotHideChest) ? doesNotHideChest : (bool?)null;
        public bool? WoodenParryBool => bool.TryParse(WoodenParry, out bool woodenParry) ? woodenParry : (bool?)null;
        public bool? DropOnWeaponChangeBool => bool.TryParse(DropOnWeaponChange, out bool dropOnWeaponChange) ? dropOnWeaponChange : (bool?)null;
        public bool? DoNotScaleBodyAccordingToWeaponLengthBool => bool.TryParse(DoNotScaleBodyAccordingToWeaponLength, out bool doNotScaleBodyAccordingToWeaponLength) ? doNotScaleBodyAccordingToWeaponLength : (bool?)null;
        public bool? QuickFadeOutBool => bool.TryParse(QuickFadeOut, out bool quickFadeOut) ? quickFadeOut : (bool?)null;
        public bool? CannotBePickedUpBool => bool.TryParse(CannotBePickedUp, out bool cannotBePickedUp) ? cannotBePickedUp : (bool?)null;
        public bool? HeldInOffHandBool => bool.TryParse(HeldInOffHand, out bool heldInOffHand) ? heldInOffHand : (bool?)null;
        public bool? ForceAttachOffHandSecondaryItemBoneBool => bool.TryParse(ForceAttachOffHandSecondaryItemBone, out bool forceAttachOffHandSecondaryItemBone) ? forceAttachOffHandSecondaryItemBone : (bool?)null;
        public bool? ForceAttachOffHandPrimaryItemBoneBool => bool.TryParse(ForceAttachOffHandPrimaryItemBone, out bool forceAttachOffHandPrimaryItemBone) ? forceAttachOffHandPrimaryItemBone : (bool?)null;
        public bool? DropOnAnyActionBool => bool.TryParse(DropOnAnyAction, out bool dropOnAnyAction) ? dropOnAnyAction : (bool?)null;

        // 设置布尔属性的方法（自动转换为字符串）
        public void SetUseTeamColorBool(bool? useTeamColor) => UseTeamColor = useTeamColor?.ToString().ToLower();
        public void SetCivilianBool(bool? civilian) => Civilian = civilian?.ToString().ToLower();
        public void SetDoesNotHideChestBool(bool? doesNotHideChest) => DoesNotHideChest = doesNotHideChest?.ToString().ToLower();
        public void SetWoodenParryBool(bool? woodenParry) => WoodenParry = woodenParry?.ToString().ToLower();
        public void SetDropOnWeaponChangeBool(bool? dropOnWeaponChange) => DropOnWeaponChange = dropOnWeaponChange?.ToString().ToLower();
        public void SetDoNotScaleBodyAccordingToWeaponLengthBool(bool? doNotScaleBodyAccordingToWeaponLength) => DoNotScaleBodyAccordingToWeaponLength = doNotScaleBodyAccordingToWeaponLength?.ToString().ToLower();
        public void SetQuickFadeOutBool(bool? quickFadeOut) => QuickFadeOut = quickFadeOut?.ToString().ToLower();
        public void SetCannotBePickedUpBool(bool? cannotBePickedUp) => CannotBePickedUp = cannotBePickedUp?.ToString().ToLower();
        public void SetHeldInOffHandBool(bool? heldInOffHand) => HeldInOffHand = heldInOffHand?.ToString().ToLower();
        public void SetForceAttachOffHandSecondaryItemBoneBool(bool? forceAttachOffHandSecondaryItemBone) => ForceAttachOffHandSecondaryItemBone = forceAttachOffHandSecondaryItemBone?.ToString().ToLower();
        public void SetForceAttachOffHandPrimaryItemBoneBool(bool? forceAttachOffHandPrimaryItemBone) => ForceAttachOffHandPrimaryItemBone = forceAttachOffHandPrimaryItemBone?.ToString().ToLower();
        public void SetDropOnAnyActionBool(bool? dropOnAnyAction) => DropOnAnyAction = dropOnAnyAction?.ToString().ToLower();
    }
}