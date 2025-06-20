using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Represents the weapon_descriptions.xml file structure containing weapon crafting descriptions
    /// </summary>
    [XmlRoot("WeaponDescriptions")]
    public class WeaponDescriptions
    {
        /// <summary>
        /// Collection of weapon description definitions
        /// </summary>
        [XmlElement("WeaponDescription")]
        public WeaponDescription[]? Descriptions { get; set; }
    }

    /// <summary>
    /// Represents a weapon description for crafting system
    /// </summary>
    public class WeaponDescription
    {
        /// <summary>
        /// Unique identifier for the weapon description
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Weapon class (OneHandedSword, TwoHandedMace, etc.)
        /// </summary>
        [XmlAttribute("weapon_class")]
        public string? WeaponClass { get; set; }

        /// <summary>
        /// Item usage features (onehanded:block:shield:swing:thrust, etc.)
        /// </summary>
        [XmlAttribute("item_usage_features")]
        public string? ItemUsageFeatures { get; set; }

        /// <summary>
        /// Weapon flags collection
        /// </summary>
        [XmlArray("WeaponFlags")]
        [XmlArrayItem("WeaponFlag")]
        public WeaponFlag[]? WeaponFlags { get; set; }

        /// <summary>
        /// Available pieces for weapon crafting
        /// </summary>
        [XmlArray("AvailablePieces")]
        [XmlArrayItem("AvailablePiece")]
        public AvailablePiece[]? AvailablePieces { get; set; }
    }

    /// <summary>
    /// Represents a collection of weapon flags
    /// </summary>
    public class WeaponFlags
    {
        /// <summary>
        /// List of individual weapon flags
        /// </summary>
        [XmlElement("WeaponFlag")]
        public List<WeaponFlag> Flags { get; set; } = new List<WeaponFlag>();
    }

    /// <summary>
    /// Represents an individual weapon flag
    /// </summary>
    public class WeaponFlag
    {
        /// <summary>
        /// Flag value (MeleeWeapon, NotUsableWithOneHand, TwoHandIdleOnMount, etc.)
        /// </summary>
        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Represents a collection of available weapon pieces for crafting
    /// </summary>
    public class AvailablePieces
    {
        /// <summary>
        /// List of available pieces for weapon crafting
        /// </summary>
        [XmlElement("AvailablePiece")]
        public List<AvailablePiece> Pieces { get; set; } = new List<AvailablePiece>();
    }

    /// <summary>
    /// Represents an individual available weapon piece
    /// </summary>
    public class AvailablePiece
    {
        /// <summary>
        /// Piece identifier (blade, guard, grip, pommel, handle, head, etc.)
        /// </summary>
        [XmlAttribute("id")]
        public string? Id { get; set; }
    }
} 