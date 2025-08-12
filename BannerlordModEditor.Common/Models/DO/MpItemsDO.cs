using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("Items")]
    public class MpItemsDO
    {
        [XmlElement("Item")]
        public List<ItemDO> Items { get; set; } = new List<ItemDO>();

        [XmlElement("CraftedItem")]
        public List<CraftedItemDO> CraftedItems { get; set; } = new List<CraftedItemDO>();
    }

    public class ItemDO
    {
        [XmlAttribute("multiplayer_item")]
        public string? MultiplayerItem { get; set; }

        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("body_name")]
        public string? BodyName { get; set; }

        [XmlAttribute("shield_body_name")]
        public string? ShieldBodyName { get; set; }

        [XmlAttribute("holster_body_name")]
        public string? HolsterBodyName { get; set; }

        [XmlAttribute("subtype")]
        public string? Subtype { get; set; }

        [XmlAttribute("mesh")]
        public string? Mesh { get; set; }

        [XmlAttribute("holster_mesh")]
        public string? HolsterMesh { get; set; }

        [XmlAttribute("culture")]
        public string? Culture { get; set; }

        [XmlAttribute("using_tableau")]
        public string? UsingTableau { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("is_merchandise")]
        public string? IsMerchandise { get; set; }

        [XmlAttribute("weight")]
        public string? Weight { get; set; }

        [XmlAttribute("difficulty")]
        public string? Difficulty { get; set; }

        [XmlAttribute("appearance")]
        public string? Appearance { get; set; }

        [XmlAttribute("Type")]
        public string? Type { get; set; }

        [XmlAttribute("item_holsters")]
        public string? ItemHolsters { get; set; }

        [XmlAttribute("item_category")]
        public string? ItemCategory { get; set; }

        [XmlAttribute("recalculate_body")]
        public string? RecalculateBody { get; set; }

        [XmlAttribute("has_lower_holster_priority")]
        public string? HasLowerHolsterPriority { get; set; }

        [XmlAttribute("holster_position_shift")]
        public string? HolsterPositionShift { get; set; }

        [XmlAttribute("flying_mesh")]
        public string? FlyingMesh { get; set; }

        [XmlAttribute("holster_mesh_with_weapon")]
        public string? HolsterMeshWithWeapon { get; set; }

        [XmlAttribute("AmmoOffset")]
        public string? AmmoOffset { get; set; }

        [XmlAttribute("prefab")]
        public string? Prefab { get; set; }

        [XmlAttribute("lod_atlas_index")]
        public string? LodAtlasIndex { get; set; }

        [XmlElement("ItemComponent")]
        public ItemComponentDO? ItemComponent { get; set; }

        [XmlElement("Flags")]
        public ItemFlagsDO? Flags { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeMultiplayerItem() => !string.IsNullOrEmpty(MultiplayerItem);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeBodyName() => !string.IsNullOrEmpty(BodyName);
        public bool ShouldSerializeShieldBodyName() => !string.IsNullOrEmpty(ShieldBodyName);
        public bool ShouldSerializeHolsterBodyName() => !string.IsNullOrEmpty(HolsterBodyName);
        public bool ShouldSerializeSubtype() => !string.IsNullOrEmpty(Subtype);
        public bool ShouldSerializeMesh() => !string.IsNullOrEmpty(Mesh);
        public bool ShouldSerializeHolsterMesh() => !string.IsNullOrEmpty(HolsterMesh);
        public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
        public bool ShouldSerializeUsingTableau() => !string.IsNullOrEmpty(UsingTableau);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeIsMerchandise() => !string.IsNullOrEmpty(IsMerchandise);
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
        public bool ShouldSerializeDifficulty() => !string.IsNullOrEmpty(Difficulty);
        public bool ShouldSerializeAppearance() => !string.IsNullOrEmpty(Appearance);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeItemHolsters() => !string.IsNullOrEmpty(ItemHolsters);
        public bool ShouldSerializeItemCategory() => !string.IsNullOrEmpty(ItemCategory);
        public bool ShouldSerializeRecalculateBody() => !string.IsNullOrEmpty(RecalculateBody);
        public bool ShouldSerializeHasLowerHolsterPriority() => !string.IsNullOrEmpty(HasLowerHolsterPriority);
        public bool ShouldSerializeHolsterPositionShift() => !string.IsNullOrEmpty(HolsterPositionShift);
        public bool ShouldSerializeFlyingMesh() => !string.IsNullOrEmpty(FlyingMesh);
        public bool ShouldSerializeHolsterMeshWithWeapon() => !string.IsNullOrEmpty(HolsterMeshWithWeapon);
        public bool ShouldSerializeAmmoOffset() => !string.IsNullOrEmpty(AmmoOffset);
        public bool ShouldSerializePrefab() => !string.IsNullOrEmpty(Prefab);
        public bool ShouldSerializeLodAtlasIndex() => !string.IsNullOrEmpty(LodAtlasIndex);

        // 布尔值标准化方法
        public bool? GetMultiplayerItem() => ParseBoolean(MultiplayerItem);
        public bool? GetUsingTableau() => ParseBoolean(UsingTableau);
        public bool? GetIsMerchandise() => ParseBoolean(IsMerchandise);
        public bool? GetRecalculateBody() => ParseBoolean(RecalculateBody);
        public bool? GetHasLowerHolsterPriority() => ParseBoolean(HasLowerHolsterPriority);

        // 设置布尔值方法（标准化为小写）
        public void SetMultiplayerItem(bool? value) => MultiplayerItem = value?.ToString().ToLower();
        public void SetUsingTableau(bool? value) => UsingTableau = value?.ToString().ToLower();
        public void SetIsMerchandise(bool? value) => IsMerchandise = value?.ToString().ToLower();
        public void SetRecalculateBody(bool? value) => RecalculateBody = value?.ToString().ToLower();
        public void SetHasLowerHolsterPriority(bool? value) => HasLowerHolsterPriority = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class CraftedItemDO
    {
        [XmlAttribute("multiplayer_item")]
        public string? MultiplayerItem { get; set; }

        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("crafting_template")]
        public string? CraftingTemplate { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("is_merchandise")]
        public string? IsMerchandise { get; set; }

        [XmlAttribute("culture")]
        public string? Culture { get; set; }

        [XmlElement("Pieces")]
        public PiecesDO? Pieces { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeMultiplayerItem() => !string.IsNullOrEmpty(MultiplayerItem);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeCraftingTemplate() => !string.IsNullOrEmpty(CraftingTemplate);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeIsMerchandise() => !string.IsNullOrEmpty(IsMerchandise);
        public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);

        // 布尔值标准化方法
        public bool? GetMultiplayerItem() => ParseBoolean(MultiplayerItem);
        public bool? GetIsMerchandise() => ParseBoolean(IsMerchandise);

        // 设置布尔值方法（标准化为小写）
        public void SetMultiplayerItem(bool? value) => MultiplayerItem = value?.ToString().ToLower();
        public void SetIsMerchandise(bool? value) => IsMerchandise = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class PiecesDO
    {
        [XmlElement("Piece")]
        public List<PieceDO> PieceList { get; set; } = new List<PieceDO>();
    }

    public class PieceDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("Type")]
        public string? Type { get; set; }

        [XmlAttribute("scale_factor")]
        public string? ScaleFactor { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeScaleFactor() => !string.IsNullOrEmpty(ScaleFactor);
    }

    public class ItemComponentDO
    {
        [XmlElement(typeof(ArmorDO))]
        [XmlElement(typeof(WeaponDO))]
        [XmlElement(typeof(HorseDO))]
        [XmlElement(typeof(HorseHarnessDO))]
        public object? Component { get; set; }
    }

    public class ArmorDO
    {
        [XmlAttribute("head_armor")]
        public string? HeadArmor { get; set; }

        [XmlAttribute("body_armor")]
        public string? BodyArmor { get; set; }

        [XmlAttribute("leg_armor")]
        public string? LegArmor { get; set; }

        [XmlAttribute("arm_armor")]
        public string? ArmArmor { get; set; }

        [XmlAttribute("has_gender_variations")]
        public string? HasGenderVariations { get; set; }

        [XmlAttribute("hair_cover_type")]
        public string? HairCoverType { get; set; }

        [XmlAttribute("beard_cover_type")]
        public string? BeardCoverType { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }

        [XmlAttribute("covers_body")]
        public string? CoversBody { get; set; }

        [XmlAttribute("covers_legs")]
        public string? CoversLegs { get; set; }

        [XmlAttribute("covers_head")]
        public string? CoversHead { get; set; }

        [XmlAttribute("mane_cover_type")]
        public string? ManeCoverType { get; set; }

        [XmlAttribute("reins_mesh")]
        public string? ReinsMesh { get; set; }

        [XmlAttribute("maneuver_bonus")]
        public string? ManeuverBonus { get; set; }

        [XmlAttribute("speed_bonus")]
        public string? SpeedBonus { get; set; }

        [XmlAttribute("charge_bonus")]
        public string? ChargeBonus { get; set; }

        [XmlAttribute("family_type")]
        public string? FamilyType { get; set; }

        [XmlAttribute("covers_hands")]
        public string? CoversHands { get; set; }

        [XmlAttribute("body_mesh_type")]
        public string? BodyMeshType { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeHeadArmor() => !string.IsNullOrEmpty(HeadArmor);
        public bool ShouldSerializeBodyArmor() => !string.IsNullOrEmpty(BodyArmor);
        public bool ShouldSerializeLegArmor() => !string.IsNullOrEmpty(LegArmor);
        public bool ShouldSerializeArmArmor() => !string.IsNullOrEmpty(ArmArmor);
        public bool ShouldSerializeHasGenderVariations() => !string.IsNullOrEmpty(HasGenderVariations);
        public bool ShouldSerializeHairCoverType() => !string.IsNullOrEmpty(HairCoverType);
        public bool ShouldSerializeBeardCoverType() => !string.IsNullOrEmpty(BeardCoverType);
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);
        public bool ShouldSerializeMaterialType() => !string.IsNullOrEmpty(MaterialType);
        public bool ShouldSerializeCoversBody() => !string.IsNullOrEmpty(CoversBody);
        public bool ShouldSerializeCoversLegs() => !string.IsNullOrEmpty(CoversLegs);
        public bool ShouldSerializeCoversHead() => !string.IsNullOrEmpty(CoversHead);
        public bool ShouldSerializeManeCoverType() => !string.IsNullOrEmpty(ManeCoverType);
        public bool ShouldSerializeReinsMesh() => !string.IsNullOrEmpty(ReinsMesh);
        public bool ShouldSerializeManeuverBonus() => !string.IsNullOrEmpty(ManeuverBonus);
        public bool ShouldSerializeSpeedBonus() => !string.IsNullOrEmpty(SpeedBonus);
        public bool ShouldSerializeChargeBonus() => !string.IsNullOrEmpty(ChargeBonus);
        public bool ShouldSerializeFamilyType() => !string.IsNullOrEmpty(FamilyType);
        public bool ShouldSerializeCoversHands() => !string.IsNullOrEmpty(CoversHands);
        public bool ShouldSerializeBodyMeshType() => !string.IsNullOrEmpty(BodyMeshType);

        // 布尔值标准化方法
        public bool? GetHasGenderVariations() => ParseBoolean(HasGenderVariations);
        public bool? GetCoversBody() => ParseBoolean(CoversBody);
        public bool? GetCoversLegs() => ParseBoolean(CoversLegs);
        public bool? GetCoversHead() => ParseBoolean(CoversHead);
        public bool? GetCoversHands() => ParseBoolean(CoversHands);

        // 设置布尔值方法（标准化为小写）
        public void SetHasGenderVariations(bool? value) => HasGenderVariations = value?.ToString().ToLower();
        public void SetCoversBody(bool? value) => CoversBody = value?.ToString().ToLower();
        public void SetCoversLegs(bool? value) => CoversLegs = value?.ToString().ToLower();
        public void SetCoversHead(bool? value) => CoversHead = value?.ToString().ToLower();
        public void SetCoversHands(bool? value) => CoversHands = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class WeaponDO
    {
        [XmlAttribute("weapon_class")]
        public string? WeaponClass { get; set; }

        [XmlAttribute("ammo_class")]
        public string? AmmoClass { get; set; }

        [XmlAttribute("stack_amount")]
        public string? StackAmount { get; set; }

        [XmlAttribute("weapon_balance")]
        public string? WeaponBalance { get; set; }

        [XmlAttribute("thrust_speed")]
        public string? ThrustSpeed { get; set; }

        [XmlAttribute("speed_rating")]
        public string? SpeedRating { get; set; }

        [XmlAttribute("missile_speed")]
        public string? MissileSpeed { get; set; }

        [XmlAttribute("accuracy")]
        public string? Accuracy { get; set; }

        [XmlAttribute("physics_material")]
        public string? PhysicsMaterial { get; set; }

        [XmlAttribute("weapon_length")]
        public string? WeaponLength { get; set; }

        [XmlAttribute("swing_damage")]
        public string? SwingDamage { get; set; }

        [XmlAttribute("thrust_damage")]
        public string? ThrustDamage { get; set; }

        [XmlAttribute("swing_damage_type")]
        public string? SwingDamageType { get; set; }

        [XmlAttribute("thrust_damage_type")]
        public string? ThrustDamageType { get; set; }

        [XmlAttribute("item_usage")]
        public string? ItemUsage { get; set; }

        [XmlAttribute("flying_sound_code")]
        public string? FlyingSoundCode { get; set; }

        [XmlAttribute("sticking_rotation")]
        public string? StickingRotation { get; set; }

        [XmlAttribute("sticking_position")]
        public string? StickingPosition { get; set; }

        [XmlAttribute("center_of_mass")]
        public string? CenterOfMass { get; set; }

        [XmlAttribute("position")]
        public string? Position { get; set; }

        [XmlAttribute("rotation")]
        public string? Rotation { get; set; }

        [XmlAttribute("hit_points")]
        public string? HitPoints { get; set; }

        [XmlAttribute("ammo_limit")]
        public string? AmmoLimit { get; set; }

        [XmlAttribute("passby_sound_code")]
        public string? PassbySoundCode { get; set; }

        [XmlAttribute("reload_phase_count")]
        public string? ReloadPhaseCount { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("body_armor")]
        public string? BodyArmor { get; set; }

        [XmlAttribute("trail_particle_name")]
        public string? TrailParticleName { get; set; }

        [XmlAttribute("rotation_speed")]
        public string? RotationSpeed { get; set; }

        [XmlElement("WeaponFlags")]
        public WeaponFlagsDO? WeaponFlags { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeWeaponClass() => !string.IsNullOrEmpty(WeaponClass);
        public bool ShouldSerializeAmmoClass() => !string.IsNullOrEmpty(AmmoClass);
        public bool ShouldSerializeStackAmount() => !string.IsNullOrEmpty(StackAmount);
        public bool ShouldSerializeWeaponBalance() => !string.IsNullOrEmpty(WeaponBalance);
        public bool ShouldSerializeThrustSpeed() => !string.IsNullOrEmpty(ThrustSpeed);
        public bool ShouldSerializeSpeedRating() => !string.IsNullOrEmpty(SpeedRating);
        public bool ShouldSerializeMissileSpeed() => !string.IsNullOrEmpty(MissileSpeed);
        public bool ShouldSerializeAccuracy() => !string.IsNullOrEmpty(Accuracy);
        public bool ShouldSerializePhysicsMaterial() => !string.IsNullOrEmpty(PhysicsMaterial);
        public bool ShouldSerializeWeaponLength() => !string.IsNullOrEmpty(WeaponLength);
        public bool ShouldSerializeSwingDamage() => !string.IsNullOrEmpty(SwingDamage);
        public bool ShouldSerializeThrustDamage() => !string.IsNullOrEmpty(ThrustDamage);
        public bool ShouldSerializeSwingDamageType() => !string.IsNullOrEmpty(SwingDamageType);
        public bool ShouldSerializeThrustDamageType() => !string.IsNullOrEmpty(ThrustDamageType);
        public bool ShouldSerializeItemUsage() => !string.IsNullOrEmpty(ItemUsage);
        public bool ShouldSerializeFlyingSoundCode() => !string.IsNullOrEmpty(FlyingSoundCode);
        public bool ShouldSerializeStickingRotation() => !string.IsNullOrEmpty(StickingRotation);
        public bool ShouldSerializeStickingPosition() => !string.IsNullOrEmpty(StickingPosition);
        public bool ShouldSerializeCenterOfMass() => !string.IsNullOrEmpty(CenterOfMass);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeRotation() => !string.IsNullOrEmpty(Rotation);
        public bool ShouldSerializeHitPoints() => !string.IsNullOrEmpty(HitPoints);
        public bool ShouldSerializeAmmoLimit() => !string.IsNullOrEmpty(AmmoLimit);
        public bool ShouldSerializePassbySoundCode() => !string.IsNullOrEmpty(PassbySoundCode);
        public bool ShouldSerializeReloadPhaseCount() => !string.IsNullOrEmpty(ReloadPhaseCount);
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);
        public bool ShouldSerializeBodyArmor() => !string.IsNullOrEmpty(BodyArmor);
        public bool ShouldSerializeTrailParticleName() => !string.IsNullOrEmpty(TrailParticleName);
        public bool ShouldSerializeRotationSpeed() => !string.IsNullOrEmpty(RotationSpeed);
    }

    public class WeaponFlagsDO
    {
        [XmlAttribute("MeleeWeapon")]
        public string? MeleeWeapon { get; set; }

        [XmlAttribute("RangedWeapon")]
        public string? RangedWeapon { get; set; }

        [XmlAttribute("PenaltyWithShield")]
        public string? PenaltyWithShield { get; set; }

        [XmlAttribute("NotUsableWithOneHand")]
        public string? NotUsableWithOneHand { get; set; }

        [XmlAttribute("TwoHandIdleOnMount")]
        public string? TwoHandIdleOnMount { get; set; }

        [XmlAttribute("WideGrip")]
        public string? WideGrip { get; set; }

        [XmlAttribute("Consumable")]
        public string? Consumable { get; set; }

        [XmlAttribute("AmmoSticksWhenShot")]
        public string? AmmoSticksWhenShot { get; set; }

        [XmlAttribute("MultiplePenetration")]
        public string? MultiplePenetration { get; set; }

        [XmlAttribute("CanPenetrateShield")]
        public string? CanPenetrateShield { get; set; }

        [XmlAttribute("CanBlockRanged")]
        public string? CanBlockRanged { get; set; }

        [XmlAttribute("HasHitPoints")]
        public string? HasHitPoints { get; set; }

        [XmlAttribute("HasString")]
        public string? HasString { get; set; }

        [XmlAttribute("StringHeldByHand")]
        public string? StringHeldByHand { get; set; }

        [XmlAttribute("AutoReload")]
        public string? AutoReload { get; set; }

        [XmlAttribute("UnloadWhenSheathed")]
        public string? UnloadWhenSheathed { get; set; }

        [XmlAttribute("AmmoBreaksOnBounceBack")]
        public string? AmmoBreaksOnBounceBack { get; set; }

        [XmlAttribute("CantReloadOnHorseback")]
        public string? CantReloadOnHorseback { get; set; }

        [XmlAttribute("Burning")]
        public string? Burning { get; set; }

        [XmlAttribute("LeavesTrail")]
        public string? LeavesTrail { get; set; }

        [XmlAttribute("CanKnockDown")]
        public string? CanKnockDown { get; set; }

        [XmlAttribute("MissileWithPhysics")]
        public string? MissileWithPhysics { get; set; }

        [XmlAttribute("UseHandAsThrowBase")]
        public string? UseHandAsThrowBase { get; set; }

        [XmlAttribute("AffectsArea")]
        public string? AffectsArea { get; set; }

        [XmlAttribute("AmmoCanBreakOnBounceBack")]
        public string? AmmoCanBreakOnBounceBack { get; set; }

        [XmlAttribute("CanKillEvenIfBlunt")]
        public string? CanKillEvenIfBlunt { get; set; }

        [XmlAttribute("AffectsAreaBig")]
        public string? AffectsAreaBig { get; set; }

        [XmlAttribute("AttachAmmoToVisual")]
        public string? AttachAmmoToVisual { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeMeleeWeapon() => !string.IsNullOrEmpty(MeleeWeapon);
        public bool ShouldSerializeRangedWeapon() => !string.IsNullOrEmpty(RangedWeapon);
        public bool ShouldSerializePenaltyWithShield() => !string.IsNullOrEmpty(PenaltyWithShield);
        public bool ShouldSerializeNotUsableWithOneHand() => !string.IsNullOrEmpty(NotUsableWithOneHand);
        public bool ShouldSerializeTwoHandIdleOnMount() => !string.IsNullOrEmpty(TwoHandIdleOnMount);
        public bool ShouldSerializeWideGrip() => !string.IsNullOrEmpty(WideGrip);
        public bool ShouldSerializeConsumable() => !string.IsNullOrEmpty(Consumable);
        public bool ShouldSerializeAmmoSticksWhenShot() => !string.IsNullOrEmpty(AmmoSticksWhenShot);
        public bool ShouldSerializeMultiplePenetration() => !string.IsNullOrEmpty(MultiplePenetration);
        public bool ShouldSerializeCanPenetrateShield() => !string.IsNullOrEmpty(CanPenetrateShield);
        public bool ShouldSerializeCanBlockRanged() => !string.IsNullOrEmpty(CanBlockRanged);
        public bool ShouldSerializeHasHitPoints() => !string.IsNullOrEmpty(HasHitPoints);
        public bool ShouldSerializeHasString() => !string.IsNullOrEmpty(HasString);
        public bool ShouldSerializeStringHeldByHand() => !string.IsNullOrEmpty(StringHeldByHand);
        public bool ShouldSerializeAutoReload() => !string.IsNullOrEmpty(AutoReload);
        public bool ShouldSerializeUnloadWhenSheathed() => !string.IsNullOrEmpty(UnloadWhenSheathed);
        public bool ShouldSerializeAmmoBreaksOnBounceBack() => !string.IsNullOrEmpty(AmmoBreaksOnBounceBack);
        public bool ShouldSerializeCantReloadOnHorseback() => !string.IsNullOrEmpty(CantReloadOnHorseback);
        public bool ShouldSerializeBurning() => !string.IsNullOrEmpty(Burning);
        public bool ShouldSerializeLeavesTrail() => !string.IsNullOrEmpty(LeavesTrail);
        public bool ShouldSerializeCanKnockDown() => !string.IsNullOrEmpty(CanKnockDown);
        public bool ShouldSerializeMissileWithPhysics() => !string.IsNullOrEmpty(MissileWithPhysics);
        public bool ShouldSerializeUseHandAsThrowBase() => !string.IsNullOrEmpty(UseHandAsThrowBase);
        public bool ShouldSerializeAffectsArea() => !string.IsNullOrEmpty(AffectsArea);
        public bool ShouldSerializeAmmoCanBreakOnBounceBack() => !string.IsNullOrEmpty(AmmoCanBreakOnBounceBack);
        public bool ShouldSerializeCanKillEvenIfBlunt() => !string.IsNullOrEmpty(CanKillEvenIfBlunt);
        public bool ShouldSerializeAffectsAreaBig() => !string.IsNullOrEmpty(AffectsAreaBig);
        public bool ShouldSerializeAttachAmmoToVisual() => !string.IsNullOrEmpty(AttachAmmoToVisual);

        // 布尔值标准化方法
        public bool? GetMeleeWeapon() => ParseBoolean(MeleeWeapon);
        public bool? GetRangedWeapon() => ParseBoolean(RangedWeapon);
        public bool? GetPenaltyWithShield() => ParseBoolean(PenaltyWithShield);
        public bool? GetNotUsableWithOneHand() => ParseBoolean(NotUsableWithOneHand);
        public bool? GetTwoHandIdleOnMount() => ParseBoolean(TwoHandIdleOnMount);
        public bool? GetWideGrip() => ParseBoolean(WideGrip);
        public bool? GetConsumable() => ParseBoolean(Consumable);
        public bool? GetAmmoSticksWhenShot() => ParseBoolean(AmmoSticksWhenShot);
        public bool? GetMultiplePenetration() => ParseBoolean(MultiplePenetration);
        public bool? GetCanPenetrateShield() => ParseBoolean(CanPenetrateShield);
        public bool? GetCanBlockRanged() => ParseBoolean(CanBlockRanged);
        public bool? GetHasHitPoints() => ParseBoolean(HasHitPoints);
        public bool? GetHasString() => ParseBoolean(HasString);
        public bool? GetStringHeldByHand() => ParseBoolean(StringHeldByHand);
        public bool? GetAutoReload() => ParseBoolean(AutoReload);
        public bool? GetUnloadWhenSheathed() => ParseBoolean(UnloadWhenSheathed);
        public bool? GetAmmoBreaksOnBounceBack() => ParseBoolean(AmmoBreaksOnBounceBack);
        public bool? GetCantReloadOnHorseback() => ParseBoolean(CantReloadOnHorseback);
        public bool? GetBurning() => ParseBoolean(Burning);
        public bool? GetLeavesTrail() => ParseBoolean(LeavesTrail);
        public bool? GetCanKnockDown() => ParseBoolean(CanKnockDown);
        public bool? GetMissileWithPhysics() => ParseBoolean(MissileWithPhysics);
        public bool? GetUseHandAsThrowBase() => ParseBoolean(UseHandAsThrowBase);
        public bool? GetAffectsArea() => ParseBoolean(AffectsArea);
        public bool? GetAmmoCanBreakOnBounceBack() => ParseBoolean(AmmoCanBreakOnBounceBack);
        public bool? GetCanKillEvenIfBlunt() => ParseBoolean(CanKillEvenIfBlunt);
        public bool? GetAffectsAreaBig() => ParseBoolean(AffectsAreaBig);
        public bool? GetAttachAmmoToVisual() => ParseBoolean(AttachAmmoToVisual);

        // 设置布尔值方法（标准化为小写）
        public void SetMeleeWeapon(bool? value) => MeleeWeapon = value?.ToString().ToLower();
        public void SetRangedWeapon(bool? value) => RangedWeapon = value?.ToString().ToLower();
        public void SetPenaltyWithShield(bool? value) => PenaltyWithShield = value?.ToString().ToLower();
        public void SetNotUsableWithOneHand(bool? value) => NotUsableWithOneHand = value?.ToString().ToLower();
        public void SetTwoHandIdleOnMount(bool? value) => TwoHandIdleOnMount = value?.ToString().ToLower();
        public void SetWideGrip(bool? value) => WideGrip = value?.ToString().ToLower();
        public void SetConsumable(bool? value) => Consumable = value?.ToString().ToLower();
        public void SetAmmoSticksWhenShot(bool? value) => AmmoSticksWhenShot = value?.ToString().ToLower();
        public void SetMultiplePenetration(bool? value) => MultiplePenetration = value?.ToString().ToLower();
        public void SetCanPenetrateShield(bool? value) => CanPenetrateShield = value?.ToString().ToLower();
        public void SetCanBlockRanged(bool? value) => CanBlockRanged = value?.ToString().ToLower();
        public void SetHasHitPoints(bool? value) => HasHitPoints = value?.ToString().ToLower();
        public void SetHasString(bool? value) => HasString = value?.ToString().ToLower();
        public void SetStringHeldByHand(bool? value) => StringHeldByHand = value?.ToString().ToLower();
        public void SetAutoReload(bool? value) => AutoReload = value?.ToString().ToLower();
        public void SetUnloadWhenSheathed(bool? value) => UnloadWhenSheathed = value?.ToString().ToLower();
        public void SetAmmoBreaksOnBounceBack(bool? value) => AmmoBreaksOnBounceBack = value?.ToString().ToLower();
        public void SetCantReloadOnHorseback(bool? value) => CantReloadOnHorseback = value?.ToString().ToLower();
        public void SetBurning(bool? value) => Burning = value?.ToString().ToLower();
        public void SetLeavesTrail(bool? value) => LeavesTrail = value?.ToString().ToLower();
        public void SetCanKnockDown(bool? value) => CanKnockDown = value?.ToString().ToLower();
        public void SetMissileWithPhysics(bool? value) => MissileWithPhysics = value?.ToString().ToLower();
        public void SetUseHandAsThrowBase(bool? value) => UseHandAsThrowBase = value?.ToString().ToLower();
        public void SetAffectsArea(bool? value) => AffectsArea = value?.ToString().ToLower();
        public void SetAmmoCanBreakOnBounceBack(bool? value) => AmmoCanBreakOnBounceBack = value?.ToString().ToLower();
        public void SetCanKillEvenIfBlunt(bool? value) => CanKillEvenIfBlunt = value?.ToString().ToLower();
        public void SetAffectsAreaBig(bool? value) => AffectsAreaBig = value?.ToString().ToLower();
        public void SetAttachAmmoToVisual(bool? value) => AttachAmmoToVisual = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class HorseDO
    {
        [XmlAttribute("speed")]
        public string? Speed { get; set; }

        [XmlAttribute("maneuver")]
        public string? Maneuver { get; set; }

        [XmlAttribute("charge_damage")]
        public string? ChargeDamage { get; set; }

        [XmlAttribute("hit_points")]
        public string? HitPoints { get; set; }

        [XmlAttribute("body_length")]
        public string? BodyLength { get; set; }

        [XmlAttribute("is_mountable")]
        public string? IsMountable { get; set; }

        [XmlAttribute("monster")]
        public string? Monster { get; set; }

        [XmlAttribute("extra_health")]
        public string? ExtraHealth { get; set; }

        [XmlAttribute("skeleton_scale")]
        public string? SkeletonScale { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlElement("AdditionalMeshes")]
        public AdditionalMeshesDO? AdditionalMeshes { get; set; }

        [XmlElement("Materials")]
        public MaterialsDO? Materials { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeSpeed() => !string.IsNullOrEmpty(Speed);
        public bool ShouldSerializeManeuver() => !string.IsNullOrEmpty(Maneuver);
        public bool ShouldSerializeChargeDamage() => !string.IsNullOrEmpty(ChargeDamage);
        public bool ShouldSerializeHitPoints() => !string.IsNullOrEmpty(HitPoints);
        public bool ShouldSerializeBodyLength() => !string.IsNullOrEmpty(BodyLength);
        public bool ShouldSerializeIsMountable() => !string.IsNullOrEmpty(IsMountable);
        public bool ShouldSerializeMonster() => !string.IsNullOrEmpty(Monster);
        public bool ShouldSerializeExtraHealth() => !string.IsNullOrEmpty(ExtraHealth);
        public bool ShouldSerializeSkeletonScale() => !string.IsNullOrEmpty(SkeletonScale);
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);

        // 布尔值标准化方法
        public bool? GetIsMountable() => ParseBoolean(IsMountable);

        // 设置布尔值方法（标准化为小写）
        public void SetIsMountable(bool? value) => IsMountable = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class AdditionalMeshesDO
    {
        [XmlElement("Mesh")]
        public List<AdditionalMeshDO> Meshes { get; set; } = new List<AdditionalMeshDO>();
    }

    public class AdditionalMeshDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("affected_by_cover")]
        public string? AffectedByCover { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeAffectedByCover() => !string.IsNullOrEmpty(AffectedByCover);

        // 布尔值标准化方法
        public bool? GetAffectedByCover() => ParseBoolean(AffectedByCover);

        // 设置布尔值方法（标准化为小写）
        public void SetAffectedByCover(bool? value) => AffectedByCover = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }

    public class MaterialsDO
    {
        [XmlElement("Material")]
        public List<MaterialDO> MaterialList { get; set; } = new List<MaterialDO>();
    }

    public class MaterialDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("MeshMultipliers")]
        public MeshMultipliersDO? MeshMultipliers { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }

    public class MeshMultipliersDO
    {
        [XmlElement("MeshMultiplier")]
        public List<MeshMultiplierDO> Multipliers { get; set; } = new List<MeshMultiplierDO>();
    }

    public class MeshMultiplierDO
    {
        [XmlAttribute("mesh_multiplier")]
        public string? MeshMultiplierValue { get; set; }

        [XmlAttribute("percentage")]
        public string? Percentage { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeMeshMultiplierValue() => !string.IsNullOrEmpty(MeshMultiplierValue);
        public bool ShouldSerializePercentage() => !string.IsNullOrEmpty(Percentage);
    }

    public class HorseHarnessDO
    {
        [XmlAttribute("body_armor")]
        public string? BodyArmor { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeBodyArmor() => !string.IsNullOrEmpty(BodyArmor);
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);
        public bool ShouldSerializeMaterialType() => !string.IsNullOrEmpty(MaterialType);
    }

    public class ItemFlagsDO
    {
        [XmlAttribute("UseTeamColor")]
        public string? UseTeamColor { get; set; }

        [XmlAttribute("Civilian")]
        public string? Civilian { get; set; }

        [XmlAttribute("DoesNotHideChest")]
        public string? DoesNotHideChest { get; set; }

        [XmlAttribute("WoodenParry")]
        public string? WoodenParry { get; set; }

        [XmlAttribute("DropOnWeaponChange")]
        public string? DropOnWeaponChange { get; set; }

        [XmlAttribute("DoNotScaleBodyAccordingToWeaponLength")]
        public string? DoNotScaleBodyAccordingToWeaponLength { get; set; }

        [XmlAttribute("QuickFadeOut")]
        public string? QuickFadeOut { get; set; }

        [XmlAttribute("CannotBePickedUp")]
        public string? CannotBePickedUp { get; set; }

        [XmlAttribute("HeldInOffHand")]
        public string? HeldInOffHand { get; set; }

        [XmlAttribute("ForceAttachOffHandSecondaryItemBone")]
        public string? ForceAttachOffHandSecondaryItemBone { get; set; }

        [XmlAttribute("ForceAttachOffHandPrimaryItemBone")]
        public string? ForceAttachOffHandPrimaryItemBone { get; set; }

        [XmlAttribute("DropOnAnyAction")]
        public string? DropOnAnyAction { get; set; }

        // ShouldSerialize方法确保只有当属性有值时才序列化
        public bool ShouldSerializeUseTeamColor() => !string.IsNullOrEmpty(UseTeamColor);
        public bool ShouldSerializeCivilian() => !string.IsNullOrEmpty(Civilian);
        public bool ShouldSerializeDoesNotHideChest() => !string.IsNullOrEmpty(DoesNotHideChest);
        public bool ShouldSerializeWoodenParry() => !string.IsNullOrEmpty(WoodenParry);
        public bool ShouldSerializeDropOnWeaponChange() => !string.IsNullOrEmpty(DropOnWeaponChange);
        public bool ShouldSerializeDoNotScaleBodyAccordingToWeaponLength() => !string.IsNullOrEmpty(DoNotScaleBodyAccordingToWeaponLength);
        public bool ShouldSerializeQuickFadeOut() => !string.IsNullOrEmpty(QuickFadeOut);
        public bool ShouldSerializeCannotBePickedUp() => !string.IsNullOrEmpty(CannotBePickedUp);
        public bool ShouldSerializeHeldInOffHand() => !string.IsNullOrEmpty(HeldInOffHand);
        public bool ShouldSerializeForceAttachOffHandSecondaryItemBone() => !string.IsNullOrEmpty(ForceAttachOffHandSecondaryItemBone);
        public bool ShouldSerializeForceAttachOffHandPrimaryItemBone() => !string.IsNullOrEmpty(ForceAttachOffHandPrimaryItemBone);
        public bool ShouldSerializeDropOnAnyAction() => !string.IsNullOrEmpty(DropOnAnyAction);

        // 布尔值标准化方法
        public bool? GetUseTeamColor() => ParseBoolean(UseTeamColor);
        public bool? GetCivilian() => ParseBoolean(Civilian);
        public bool? GetDoesNotHideChest() => ParseBoolean(DoesNotHideChest);
        public bool? GetWoodenParry() => ParseBoolean(WoodenParry);
        public bool? GetDropOnWeaponChange() => ParseBoolean(DropOnWeaponChange);
        public bool? GetDoNotScaleBodyAccordingToWeaponLength() => ParseBoolean(DoNotScaleBodyAccordingToWeaponLength);
        public bool? GetQuickFadeOut() => ParseBoolean(QuickFadeOut);
        public bool? GetCannotBePickedUp() => ParseBoolean(CannotBePickedUp);
        public bool? GetHeldInOffHand() => ParseBoolean(HeldInOffHand);
        public bool? GetForceAttachOffHandSecondaryItemBone() => ParseBoolean(ForceAttachOffHandSecondaryItemBone);
        public bool? GetForceAttachOffHandPrimaryItemBone() => ParseBoolean(ForceAttachOffHandPrimaryItemBone);
        public bool? GetDropOnAnyAction() => ParseBoolean(DropOnAnyAction);

        // 设置布尔值方法（标准化为小写）
        public void SetUseTeamColor(bool? value) => UseTeamColor = value?.ToString().ToLower();
        public void SetCivilian(bool? value) => Civilian = value?.ToString().ToLower();
        public void SetDoesNotHideChest(bool? value) => DoesNotHideChest = value?.ToString().ToLower();
        public void SetWoodenParry(bool? value) => WoodenParry = value?.ToString().ToLower();
        public void SetDropOnWeaponChange(bool? value) => DropOnWeaponChange = value?.ToString().ToLower();
        public void SetDoNotScaleBodyAccordingToWeaponLength(bool? value) => DoNotScaleBodyAccordingToWeaponLength = value?.ToString().ToLower();
        public void SetQuickFadeOut(bool? value) => QuickFadeOut = value?.ToString().ToLower();
        public void SetCannotBePickedUp(bool? value) => CannotBePickedUp = value?.ToString().ToLower();
        public void SetHeldInOffHand(bool? value) => HeldInOffHand = value?.ToString().ToLower();
        public void SetForceAttachOffHandSecondaryItemBone(bool? value) => ForceAttachOffHandSecondaryItemBone = value?.ToString().ToLower();
        public void SetForceAttachOffHandPrimaryItemBone(bool? value) => ForceAttachOffHandPrimaryItemBone = value?.ToString().ToLower();
        public void SetDropOnAnyAction(bool? value) => DropOnAnyAction = value?.ToString().ToLower();

        // 通用布尔值解析方法
        private static bool? ParseBoolean(string? value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var normalized = value.ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" => true,
                "false" or "0" or "no" => false,
                _ => bool.TryParse(normalized, out var result) ? result : (bool?)null
            };
        }
    }
}