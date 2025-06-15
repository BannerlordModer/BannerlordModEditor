using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("Items")]
    public class MpItems
    {
        [XmlElement("Item")]
        public List<Item> Items { get; set; } = new List<Item>();

        [XmlElement("CraftedItem")]
        public List<CraftedItem> CraftedItems { get; set; } = new List<CraftedItem>();
    }

    public class Item
    {
        [XmlAttribute("multiplayer_item")]
        public bool MultiplayerItem { get; set; }

        [XmlIgnore]
        public bool MultiplayerItemSpecified { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

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
        public bool UsingTableau { get; set; }

        [XmlIgnore]
        public bool UsingTableauSpecified { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlIgnore]
        public bool ValueSpecified { get; set; }

        [XmlAttribute("is_merchandise")]
        public bool IsMerchandise { get; set; }

        [XmlIgnore]
        public bool IsMerchandiseSpecified { get; set; }

        [XmlAttribute("weight")]
        public double Weight { get; set; }

        [XmlIgnore]
        public bool WeightSpecified { get; set; }

        [XmlAttribute("difficulty")]
        public int Difficulty { get; set; }

        [XmlIgnore]
        public bool DifficultySpecified { get; set; }

        [XmlAttribute("appearance")]
        public double Appearance { get; set; }

        [XmlIgnore]
        public bool AppearanceSpecified { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("item_holsters")]
        public string? ItemHolsters { get; set; }

        [XmlAttribute("item_category")]
        public string? ItemCategory { get; set; }

        [XmlAttribute("recalculate_body")]
        public bool RecalculateBody { get; set; }

        [XmlIgnore]
        public bool RecalculateBodySpecified { get; set; }

        [XmlAttribute("has_lower_holster_priority")]
        public bool HasLowerHolsterPriority { get; set; }

        [XmlIgnore]
        public bool HasLowerHolsterPrioritySpecified { get; set; }

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
        public int LodAtlasIndex { get; set; }

        [XmlIgnore]
        public bool LodAtlasIndexSpecified { get; set; }

        [XmlElement("ItemComponent")]
        public ItemComponent? ItemComponent { get; set; }

        [XmlElement("Flags")]
        public ItemFlags? Flags { get; set; }
    }

    public class CraftedItem
    {
        [XmlAttribute("multiplayer_item")]
        public bool MultiplayerItem { get; set; }

        [XmlIgnore]
        public bool MultiplayerItemSpecified { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("crafting_template")]
        public string CraftingTemplate { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlIgnore]
        public bool ValueSpecified { get; set; }

        [XmlAttribute("is_merchandise")]
        public bool IsMerchandise { get; set; }

        [XmlIgnore]
        public bool IsMerchandiseSpecified { get; set; }

        [XmlAttribute("culture")]
        public string? Culture { get; set; }

        [XmlElement("Pieces")]
        public Pieces? Pieces { get; set; }
    }

    public class Pieces
    {
        [XmlElement("Piece")]
        public List<Piece> PieceList { get; set; } = new List<Piece>();
    }

    public class Piece
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("scale_factor")]
        public int ScaleFactor { get; set; }

        [XmlIgnore]
        public bool ScaleFactorSpecified { get; set; }
    }

    public class ItemComponent
    {
        [XmlElement(typeof(Armor))]
        [XmlElement(typeof(Weapon))]
        [XmlElement(typeof(Horse))]
        [XmlElement(typeof(HorseHarness))]
        public object? Component { get; set; }
    }

    public class Armor
    {
        [XmlAttribute("head_armor")]
        public int HeadArmor { get; set; }

        [XmlIgnore]
        public bool HeadArmorSpecified { get; set; }

        [XmlAttribute("body_armor")]
        public int BodyArmor { get; set; }

        [XmlIgnore]
        public bool BodyArmorSpecified { get; set; }

        [XmlAttribute("leg_armor")]
        public int LegArmor { get; set; }

        [XmlIgnore]
        public bool LegArmorSpecified { get; set; }

        [XmlAttribute("arm_armor")]
        public int ArmArmor { get; set; }

        [XmlIgnore]
        public bool ArmArmorSpecified { get; set; }

        [XmlAttribute("has_gender_variations")]
        public bool HasGenderVariations { get; set; }

        [XmlIgnore]
        public bool HasGenderVariationsSpecified { get; set; }

        [XmlAttribute("hair_cover_type")]
        public string? HairCoverType { get; set; }

        [XmlAttribute("beard_cover_type")]
        public string? BeardCoverType { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }

        [XmlAttribute("covers_body")]
        public bool CoversBody { get; set; }

        [XmlIgnore]
        public bool CoversBodySpecified { get; set; }

        [XmlAttribute("covers_legs")]
        public bool CoversLegs { get; set; }

        [XmlIgnore]
        public bool CoversLegsSpecified { get; set; }

        [XmlAttribute("covers_head")]
        public bool CoversHead { get; set; }

        [XmlIgnore]
        public bool CoversHeadSpecified { get; set; }

        [XmlAttribute("mane_cover_type")]
        public string? ManeCoverType { get; set; }

        [XmlAttribute("reins_mesh")]
        public string? ReinsMesh { get; set; }

        [XmlAttribute("maneuver_bonus")]
        public int ManeuverBonus { get; set; }

        [XmlIgnore]
        public bool ManeuverBonusSpecified { get; set; }

        [XmlAttribute("speed_bonus")]
        public int SpeedBonus { get; set; }

        [XmlIgnore]
        public bool SpeedBonusSpecified { get; set; }

        [XmlAttribute("charge_bonus")]
        public int ChargeBonus { get; set; }

        [XmlIgnore]
        public bool ChargeBonusSpecified { get; set; }

        [XmlAttribute("family_type")]
        public int FamilyType { get; set; }

        [XmlIgnore]
        public bool FamilyTypeSpecified { get; set; }

        [XmlAttribute("covers_hands")]
        public bool CoversHands { get; set; }

        [XmlIgnore]
        public bool CoversHandsSpecified { get; set; }

        [XmlAttribute("body_mesh_type")]
        public string? BodyMeshType { get; set; }
    }

    public class Weapon
    {
        [XmlAttribute("weapon_class")]
        public string? WeaponClass { get; set; }

        [XmlAttribute("ammo_class")]
        public string? AmmoClass { get; set; }

        [XmlAttribute("stack_amount")]
        public int StackAmount { get; set; }

        [XmlIgnore]
        public bool StackAmountSpecified { get; set; }

        [XmlAttribute("weapon_balance")]
        public int WeaponBalance { get; set; }

        [XmlIgnore]
        public bool WeaponBalanceSpecified { get; set; }

        [XmlAttribute("thrust_speed")]
        public int ThrustSpeed { get; set; }

        [XmlIgnore]
        public bool ThrustSpeedSpecified { get; set; }

        [XmlAttribute("speed_rating")]
        public int SpeedRating { get; set; }

        [XmlIgnore]
        public bool SpeedRatingSpecified { get; set; }

        [XmlAttribute("missile_speed")]
        public int MissileSpeed { get; set; }

        [XmlIgnore]
        public bool MissileSpeedSpecified { get; set; }

        [XmlAttribute("accuracy")]
        public int Accuracy { get; set; }

        [XmlIgnore]
        public bool AccuracySpecified { get; set; }

        [XmlAttribute("physics_material")]
        public string? PhysicsMaterial { get; set; }

        [XmlAttribute("weapon_length")]
        public int WeaponLength { get; set; }

        [XmlIgnore]
        public bool WeaponLengthSpecified { get; set; }

        [XmlAttribute("swing_damage")]
        public int SwingDamage { get; set; }

        [XmlIgnore]
        public bool SwingDamageSpecified { get; set; }

        [XmlAttribute("thrust_damage")]
        public int ThrustDamage { get; set; }

        [XmlIgnore]
        public bool ThrustDamageSpecified { get; set; }

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
        public int HitPoints { get; set; }

        [XmlIgnore]
        public bool HitPointsSpecified { get; set; }

        [XmlAttribute("ammo_limit")]
        public int AmmoLimit { get; set; }

        [XmlIgnore]
        public bool AmmoLimitSpecified { get; set; }

        [XmlAttribute("passby_sound_code")]
        public string? PassbySoundCode { get; set; }

        [XmlAttribute("reload_phase_count")]
        public int ReloadPhaseCount { get; set; }

        [XmlIgnore]
        public bool ReloadPhaseCountSpecified { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("body_armor")]
        public int BodyArmor { get; set; }

        [XmlIgnore]
        public bool BodyArmorSpecified { get; set; }

        [XmlAttribute("trail_particle_name")]
        public string? TrailParticleName { get; set; }

        [XmlAttribute("rotation_speed")]
        public string? RotationSpeed { get; set; }

        [XmlElement("WeaponFlags")]
        public WeaponFlags? WeaponFlags { get; set; }
    }

    public class WeaponFlags
    {
        [XmlAttribute("MeleeWeapon")]
        public bool MeleeWeapon { get; set; }

        [XmlIgnore]
        public bool MeleeWeaponSpecified { get; set; }

        [XmlAttribute("RangedWeapon")]
        public bool RangedWeapon { get; set; }

        [XmlIgnore]
        public bool RangedWeaponSpecified { get; set; }

        [XmlAttribute("PenaltyWithShield")]
        public bool PenaltyWithShield { get; set; }

        [XmlIgnore]
        public bool PenaltyWithShieldSpecified { get; set; }

        [XmlAttribute("NotUsableWithOneHand")]
        public bool NotUsableWithOneHand { get; set; }

        [XmlIgnore]
        public bool NotUsableWithOneHandSpecified { get; set; }

        [XmlAttribute("TwoHandIdleOnMount")]
        public bool TwoHandIdleOnMount { get; set; }

        [XmlIgnore]
        public bool TwoHandIdleOnMountSpecified { get; set; }

        [XmlAttribute("WideGrip")]
        public bool WideGrip { get; set; }

        [XmlIgnore]
        public bool WideGripSpecified { get; set; }

        [XmlAttribute("Consumable")]
        public bool Consumable { get; set; }

        [XmlIgnore]
        public bool ConsumableSpecified { get; set; }

        [XmlAttribute("AmmoSticksWhenShot")]
        public bool AmmoSticksWhenShot { get; set; }

        [XmlIgnore]
        public bool AmmoSticksWhenShotSpecified { get; set; }

        [XmlAttribute("MultiplePenetration")]
        public bool MultiplePenetration { get; set; }

        [XmlIgnore]
        public bool MultiplePenetrationSpecified { get; set; }

        [XmlAttribute("CanPenetrateShield")]
        public bool CanPenetrateShield { get; set; }

        [XmlIgnore]
        public bool CanPenetrateShieldSpecified { get; set; }

        [XmlAttribute("CanBlockRanged")]
        public bool CanBlockRanged { get; set; }

        [XmlIgnore]
        public bool CanBlockRangedSpecified { get; set; }

        [XmlAttribute("HasHitPoints")]
        public bool HasHitPoints { get; set; }

        [XmlIgnore]
        public bool HasHitPointsSpecified { get; set; }

        [XmlAttribute("HasString")]
        public bool HasString { get; set; }

        [XmlIgnore]
        public bool HasStringSpecified { get; set; }

        [XmlAttribute("StringHeldByHand")]
        public bool StringHeldByHand { get; set; }

        [XmlIgnore]
        public bool StringHeldByHandSpecified { get; set; }

        [XmlAttribute("AutoReload")]
        public bool AutoReload { get; set; }

        [XmlIgnore]
        public bool AutoReloadSpecified { get; set; }

        [XmlAttribute("UnloadWhenSheathed")]
        public bool UnloadWhenSheathed { get; set; }

        [XmlIgnore]
        public bool UnloadWhenSheathedSpecified { get; set; }

        [XmlAttribute("AmmoBreaksOnBounceBack")]
        public bool AmmoBreaksOnBounceBack { get; set; }

        [XmlIgnore]
        public bool AmmoBreaksOnBounceBackSpecified { get; set; }

        [XmlAttribute("CantReloadOnHorseback")]
        public bool CantReloadOnHorseback { get; set; }

        [XmlIgnore]
        public bool CantReloadOnHorsebackSpecified { get; set; }

        [XmlAttribute("Burning")]
        public bool Burning { get; set; }

        [XmlIgnore]
        public bool BurningSpecified { get; set; }

        [XmlAttribute("LeavesTrail")]
        public bool LeavesTrail { get; set; }

        [XmlIgnore]
        public bool LeavesTrailSpecified { get; set; }

        [XmlAttribute("CanKnockDown")]
        public bool CanKnockDown { get; set; }

        [XmlIgnore]
        public bool CanKnockDownSpecified { get; set; }

        [XmlAttribute("MissileWithPhysics")]
        public bool MissileWithPhysics { get; set; }

        [XmlIgnore]
        public bool MissileWithPhysicsSpecified { get; set; }

        [XmlAttribute("UseHandAsThrowBase")]
        public bool UseHandAsThrowBase { get; set; }

        [XmlIgnore]
        public bool UseHandAsThrowBaseSpecified { get; set; }

        [XmlAttribute("AffectsArea")]
        public bool AffectsArea { get; set; }

        [XmlIgnore]
        public bool AffectsAreaSpecified { get; set; }

        [XmlAttribute("AmmoCanBreakOnBounceBack")]
        public bool AmmoCanBreakOnBounceBack { get; set; }

        [XmlIgnore]
        public bool AmmoCanBreakOnBounceBackSpecified { get; set; }

        [XmlAttribute("CanKillEvenIfBlunt")]
        public bool CanKillEvenIfBlunt { get; set; }

        [XmlIgnore]
        public bool CanKillEvenIfBluntSpecified { get; set; }

        [XmlAttribute("AffectsAreaBig")]
        public bool AffectsAreaBig { get; set; }

        [XmlIgnore]
        public bool AffectsAreaBigSpecified { get; set; }

        [XmlAttribute("AttachAmmoToVisual")]
        public bool AttachAmmoToVisual { get; set; }

        [XmlIgnore]
        public bool AttachAmmoToVisualSpecified { get; set; }
    }

    public class Horse
    {
        [XmlAttribute("speed")]
        public int Speed { get; set; }

        [XmlIgnore]
        public bool SpeedSpecified { get; set; }

        [XmlAttribute("maneuver")]
        public int Maneuver { get; set; }

        [XmlIgnore]
        public bool ManeuverSpecified { get; set; }

        [XmlAttribute("charge_damage")]
        public int ChargeDamage { get; set; }

        [XmlIgnore]
        public bool ChargeDamageSpecified { get; set; }

        [XmlAttribute("hit_points")]
        public int HitPoints { get; set; }

        [XmlIgnore]
        public bool HitPointsSpecified { get; set; }

        [XmlAttribute("body_length")]
        public int BodyLength { get; set; }

        [XmlIgnore]
        public bool BodyLengthSpecified { get; set; }

        [XmlAttribute("is_mountable")]
        public bool IsMountable { get; set; }

        [XmlIgnore]
        public bool IsMountableSpecified { get; set; }

        [XmlAttribute("monster")]
        public string? Monster { get; set; }

        [XmlAttribute("extra_health")]
        public int ExtraHealth { get; set; }

        [XmlIgnore]
        public bool ExtraHealthSpecified { get; set; }

        [XmlAttribute("skeleton_scale")]
        public string? SkeletonScale { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlElement("AdditionalMeshes")]
        public AdditionalMeshes? AdditionalMeshes { get; set; }

        [XmlElement("Materials")]
        public Materials? Materials { get; set; }
    }

    public class AdditionalMeshes
    {
        [XmlElement("Mesh")]
        public List<AdditionalMesh> Meshes { get; set; } = new List<AdditionalMesh>();
    }

    public class AdditionalMesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("affected_by_cover")]
        public bool AffectedByCover { get; set; }

        [XmlIgnore]
        public bool AffectedByCoverSpecified { get; set; }
    }

    public class Materials
    {
        [XmlElement("Material")]
        public List<Material> MaterialList { get; set; } = new List<Material>();
    }

    public class Material
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("MeshMultipliers")]
        public MeshMultipliers? MeshMultipliers { get; set; }
    }

    public class MeshMultipliers
    {
        [XmlElement("MeshMultiplier")]
        public List<MeshMultiplier> Multipliers { get; set; } = new List<MeshMultiplier>();
    }

    public class MeshMultiplier
    {
        [XmlAttribute("mesh_multiplier")]
        public string MeshMultiplierValue { get; set; } = string.Empty;

        [XmlAttribute("percentage")]
        public double Percentage { get; set; }

        [XmlIgnore]
        public bool PercentageSpecified { get; set; }
    }

    public class HorseHarness
    {
        [XmlAttribute("body_armor")]
        public int BodyArmor { get; set; }

        [XmlIgnore]
        public bool BodyArmorSpecified { get; set; }

        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }

        [XmlAttribute("material_type")]
        public string? MaterialType { get; set; }
    }

    public class ItemFlags
    {
        [XmlAttribute("UseTeamColor")]
        public bool UseTeamColor { get; set; }

        [XmlIgnore]
        public bool UseTeamColorSpecified { get; set; }

        [XmlAttribute("Civilian")]
        public bool Civilian { get; set; }

        [XmlIgnore]
        public bool CivilianSpecified { get; set; }

        [XmlAttribute("DoesNotHideChest")]
        public bool DoesNotHideChest { get; set; }

        [XmlIgnore]
        public bool DoesNotHideChestSpecified { get; set; }

        [XmlAttribute("WoodenParry")]
        public bool WoodenParry { get; set; }

        [XmlIgnore]
        public bool WoodenParrySpecified { get; set; }

        [XmlAttribute("DropOnWeaponChange")]
        public bool DropOnWeaponChange { get; set; }

        [XmlIgnore]
        public bool DropOnWeaponChangeSpecified { get; set; }

        [XmlAttribute("DoNotScaleBodyAccordingToWeaponLength")]
        public bool DoNotScaleBodyAccordingToWeaponLength { get; set; }

        [XmlIgnore]
        public bool DoNotScaleBodyAccordingToWeaponLengthSpecified { get; set; }

        [XmlAttribute("QuickFadeOut")]
        public bool QuickFadeOut { get; set; }

        [XmlIgnore]
        public bool QuickFadeOutSpecified { get; set; }

        [XmlAttribute("CannotBePickedUp")]
        public bool CannotBePickedUp { get; set; }

        [XmlIgnore]
        public bool CannotBePickedUpSpecified { get; set; }

        [XmlAttribute("HeldInOffHand")]
        public bool HeldInOffHand { get; set; }

        [XmlIgnore]
        public bool HeldInOffHandSpecified { get; set; }

        [XmlAttribute("ForceAttachOffHandSecondaryItemBone")]
        public bool ForceAttachOffHandSecondaryItemBone { get; set; }

        [XmlIgnore]
        public bool ForceAttachOffHandSecondaryItemBoneSpecified { get; set; }

        [XmlAttribute("ForceAttachOffHandPrimaryItemBone")]
        public bool ForceAttachOffHandPrimaryItemBone { get; set; }

        [XmlIgnore]
        public bool ForceAttachOffHandPrimaryItemBoneSpecified { get; set; }

        [XmlAttribute("DropOnAnyAction")]
        public bool DropOnAnyAction { get; set; }

        [XmlIgnore]
        public bool DropOnAnyActionSpecified { get; set; }
    }
} 