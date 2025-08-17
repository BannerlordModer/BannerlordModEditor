using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class BannerIconsDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlElement("BannerIconData")]
        public BannerIconDataDO? BannerIconData { get; set; }
        
        [XmlIgnore]
        public bool HasBannerIconData { get; set; } = false;
        
        public bool ShouldSerializeBannerIconData() => HasBannerIconData && BannerIconData != null;
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }

    public class BannerIconDataDO
    {
        [XmlElement("BannerIconGroup")]
        public List<BannerIconGroupDO> BannerIconGroups { get; set; } = new List<BannerIconGroupDO>();

        [XmlElement("BannerColors")]
        public BannerColorsDO? BannerColors { get; set; }
        
        [XmlIgnore]
        public bool HasEmptyBannerIconGroups { get; set; } = false;
        [XmlIgnore]
        public bool HasBannerColors { get; set; } = false;

        public bool ShouldSerializeBannerIconGroups() => HasEmptyBannerIconGroups || (BannerIconGroups != null && BannerIconGroups.Count > 0);
        public bool ShouldSerializeBannerColors() => HasBannerColors && BannerColors != null;
    }

    public class BannerIconGroupDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("is_pattern")]
        public string? IsPattern { get; set; }

        [XmlElement("Background")]
        public List<BackgroundDO> Backgrounds { get; set; } = new List<BackgroundDO>();

        [XmlElement("Icon")]
        public List<IconDO> Icons { get; set; } = new List<IconDO>();
        
        [XmlIgnore]
        public bool HasEmptyBackgrounds { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyIcons { get; set; } = false;

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIsPattern() => !string.IsNullOrEmpty(IsPattern);
        public bool ShouldSerializeBackgrounds() => HasEmptyBackgrounds || (Backgrounds != null && Backgrounds.Count > 0);
        public bool ShouldSerializeIcons() => HasEmptyIcons || (Icons != null && Icons.Count > 0);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? IsPatternBool => bool.TryParse(IsPattern, out bool pattern) ? pattern : (bool?)null;
    }

    public class BackgroundDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("mesh_name")]
        public string? MeshName { get; set; }

        [XmlAttribute("is_base_background")]
        public string? IsBaseBackground { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeMeshName() => !string.IsNullOrEmpty(MeshName);
        public bool ShouldSerializeIsBaseBackground() => !string.IsNullOrEmpty(IsBaseBackground);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? IsBaseBackgroundBool => bool.TryParse(IsBaseBackground, out bool isBase) ? isBase : (bool?)null;
    }

    public class IconDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("material_name")]
        public string? MaterialName { get; set; }

        [XmlAttribute("texture_index")]
        public string? TextureIndex { get; set; }

        [XmlAttribute("is_reserved")]
        public string? IsReserved { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeMaterialName() => !string.IsNullOrEmpty(MaterialName);
        public bool ShouldSerializeTextureIndex() => !string.IsNullOrEmpty(TextureIndex);
        public bool ShouldSerializeIsReserved() => !string.IsNullOrEmpty(IsReserved);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public int? TextureIndexInt => int.TryParse(TextureIndex, out int index) ? index : (int?)null;
        public bool? IsReservedBool => bool.TryParse(IsReserved, out bool reserved) ? reserved : (bool?)null;
    }

    public class BannerColorsDO
    {
        [XmlElement("Color")]
        public List<ColorEntryDO> Colors { get; set; } = new List<ColorEntryDO>();
        
        [XmlIgnore]
        public bool HasEmptyColors { get; set; } = false;

        public bool ShouldSerializeColors() => HasEmptyColors || (Colors != null && Colors.Count > 0);
    }

    public class ColorEntryDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("hex")]
        public string? Hex { get; set; }

        [XmlAttribute("player_can_choose_for_background")]
        public string? PlayerCanChooseForBackground { get; set; }

        [XmlAttribute("player_can_choose_for_sigil")]
        public string? PlayerCanChooseForSigil { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeHex() => !string.IsNullOrEmpty(Hex);
        public bool ShouldSerializePlayerCanChooseForBackground() => !string.IsNullOrEmpty(PlayerCanChooseForBackground);
        public bool ShouldSerializePlayerCanChooseForSigil() => !string.IsNullOrEmpty(PlayerCanChooseForSigil);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? PlayerCanChooseForBackgroundBool => bool.TryParse(PlayerCanChooseForBackground, out bool canChoose) ? canChoose : (bool?)null;
        public bool? PlayerCanChooseForSigilBool => bool.TryParse(PlayerCanChooseForSigil, out bool canChoose) ? canChoose : (bool?)null;
    }
}