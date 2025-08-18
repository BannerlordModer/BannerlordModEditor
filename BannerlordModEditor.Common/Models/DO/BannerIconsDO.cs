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
        public bool ShouldSerializeType() => !string.IsNullOrWhiteSpace(Type);

  
        /// <summary>
        /// 布尔值字符串解析方法
        /// 支持 "true"/"false"/"TRUE"/"FALSE" 和 "1"/"0" 格式
        /// </summary>
        /// <param name="value">要解析的字符串</param>
        /// <returns>解析结果，无法解析时返回null</returns>
        public static bool? ParseBoolString(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var trimmedValue = value.Trim();
            
            // 标准布尔值解析（支持标准格式）
            if (trimmedValue == "true" || trimmedValue == "TRUE" || trimmedValue == "True")
                return true;
            if (trimmedValue == "false" || trimmedValue == "FALSE" || trimmedValue == "False")
                return false;
            
            // 数字布尔值解析
            if (trimmedValue == "1")
                return true;
            if (trimmedValue == "0")
                return false;
            
            return null;
        }
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

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeIsPattern() => !string.IsNullOrWhiteSpace(IsPattern);
        public bool ShouldSerializeBackgrounds() => HasEmptyBackgrounds || (Backgrounds != null && Backgrounds.Count > 0);
        public bool ShouldSerializeIcons() => HasEmptyIcons || (Icons != null && Icons.Count > 0);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? IsPatternBool => BannerIconsDO.ParseBoolString(IsPattern);
    }

    public class BackgroundDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("mesh_name")]
        public string? MeshName { get; set; }

        [XmlAttribute("is_base_background")]
        public string? IsBaseBackground { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeMeshName() => !string.IsNullOrWhiteSpace(MeshName);
        public bool ShouldSerializeIsBaseBackground() => !string.IsNullOrWhiteSpace(IsBaseBackground);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? IsBaseBackgroundBool => BannerIconsDO.ParseBoolString(IsBaseBackground);
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

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeMaterialName() => !string.IsNullOrWhiteSpace(MaterialName);
        public bool ShouldSerializeTextureIndex() => !string.IsNullOrWhiteSpace(TextureIndex);
        public bool ShouldSerializeIsReserved() => !string.IsNullOrWhiteSpace(IsReserved);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public int? TextureIndexInt => int.TryParse(TextureIndex, out int index) && index >= 0 ? index : (int?)null;
        public bool? IsReservedBool => BannerIconsDO.ParseBoolString(IsReserved);
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

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeHex() => !string.IsNullOrWhiteSpace(Hex);
        public bool ShouldSerializePlayerCanChooseForBackground() => !string.IsNullOrWhiteSpace(PlayerCanChooseForBackground);
        public bool ShouldSerializePlayerCanChooseForSigil() => !string.IsNullOrWhiteSpace(PlayerCanChooseForSigil);

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? PlayerCanChooseForBackgroundBool => BannerIconsDO.ParseBoolString(PlayerCanChooseForBackground);
        public bool? PlayerCanChooseForSigilBool => BannerIconsDO.ParseBoolString(PlayerCanChooseForSigil);
    }
}