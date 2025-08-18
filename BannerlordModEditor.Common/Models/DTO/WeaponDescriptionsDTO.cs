using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 武器描述配置的数据传输对象
    /// 用于XML序列化/反序列化
    /// </summary>
    [XmlRoot("WeaponDescriptions")]
    public class WeaponDescriptionsDTO
    {
        [XmlElement("WeaponDescription")]
        public List<WeaponDescriptionDTO> Descriptions { get; set; } = new List<WeaponDescriptionDTO>();
    }

    /// <summary>
    /// 单个武器描述的DTO
    /// </summary>
    public class WeaponDescriptionDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("weapon_class")]
        public string? WeaponClass { get; set; }

        [XmlAttribute("item_usage_features")]
        public string? ItemUsageFeatures { get; set; }

        [XmlElement("WeaponFlags")]
        public WeaponFlagsDTO? WeaponFlags { get; set; }

        [XmlElement("AvailablePieces")]
        public AvailablePiecesDTO? AvailablePieces { get; set; }
    }

    /// <summary>
    /// 武器标志集合的DTO
    /// </summary>
    public class WeaponFlagsDTO
    {
        [XmlElement("WeaponFlag")]
        public List<WeaponFlagDTO> Flags { get; set; } = new List<WeaponFlagDTO>();
    }

    /// <summary>
    /// 单个武器标志的DTO
    /// </summary>
    public class WeaponFlagDTO
    {
        [XmlAttribute("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// 可用武器部件集合的DTO
    /// </summary>
    public class AvailablePiecesDTO
    {
        [XmlElement("AvailablePiece")]
        public List<AvailablePieceDTO> Pieces { get; set; } = new List<AvailablePieceDTO>();
    }

    /// <summary>
    /// 单个可用武器部件的DTO
    /// </summary>
    public class AvailablePieceDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }
    }
}