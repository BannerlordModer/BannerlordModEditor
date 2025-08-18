using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class BannerIconsDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
        
        [XmlElement("BannerIconData")]
        public BannerIconDataDTO? BannerIconData { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeBannerIconData() => BannerIconData != null;

        // 便捷属性
        public bool HasType => !string.IsNullOrEmpty(Type);
        public bool HasBannerIconData => BannerIconData != null;
    }

    public class BannerIconDataDTO
    {
        [XmlElement("BannerIconGroup")]
        public List<BannerIconGroupDTO> BannerIconGroups { get; set; } = new List<BannerIconGroupDTO>();

        [XmlElement("BannerColors")]
        public BannerColorsDTO? BannerColors { get; set; }

        public bool ShouldSerializeBannerIconGroups() => BannerIconGroups != null && BannerIconGroups.Count > 0;
        public bool ShouldSerializeBannerColors() => BannerColors != null;

        // 便捷属性
        public int BannerIconGroupsCount => BannerIconGroups?.Count ?? 0;
        public bool HasBannerIconGroups => BannerIconGroups != null && BannerIconGroups.Count > 0;
        public bool HasBannerColors => BannerColors != null;
        public int ColorsCount => BannerColors?.Colors?.Count ?? 0;
    }

    public class BannerIconGroupDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("is_pattern")]
        public string? IsPattern { get; set; }

        [XmlElement("Background")]
        public List<BackgroundDTO> Backgrounds { get; set; } = new List<BackgroundDTO>();

        [XmlElement("Icon")]
        public List<IconDTO> Icons { get; set; } = new List<IconDTO>();

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIsPattern() => !string.IsNullOrEmpty(IsPattern);
        public bool ShouldSerializeBackgrounds() => Backgrounds != null && Backgrounds.Count > 0;
        public bool ShouldSerializeIcons() => Icons != null && Icons.Count > 0;

        // 类型安全的便捷属性
        public int? IdInt => int.TryParse(Id, out int id) ? id : (int?)null;
        public bool? IsPatternBool => bool.TryParse(IsPattern, out bool pattern) ? pattern : (bool?)null;
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasName => !string.IsNullOrEmpty(Name);
        public bool HasIsPattern => !string.IsNullOrEmpty(IsPattern);
        public int BackgroundsCount => Backgrounds?.Count ?? 0;
        public int IconsCount => Icons?.Count ?? 0;

        // 设置方法
        public void SetIdInt(int? value) => Id = value?.ToString();
        public void SetIsPatternBool(bool? value) => IsPattern = value?.ToString().ToLower();
    }

    public class BackgroundDTO
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
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasMeshName => !string.IsNullOrEmpty(MeshName);
        public bool HasIsBaseBackground => !string.IsNullOrEmpty(IsBaseBackground);

        // 设置方法
        public void SetIdInt(int? value) => Id = value?.ToString();
        public void SetIsBaseBackgroundBool(bool? value) => IsBaseBackground = value?.ToString().ToLower();
    }

    public class IconDTO
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
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasMaterialName => !string.IsNullOrEmpty(MaterialName);
        public bool HasTextureIndex => !string.IsNullOrEmpty(TextureIndex);
        public bool HasIsReserved => !string.IsNullOrEmpty(IsReserved);

        // 设置方法
        public void SetIdInt(int? value) => Id = value?.ToString();
        public void SetTextureIndexInt(int? value) => TextureIndex = value?.ToString();
        public void SetIsReservedBool(bool? value) => IsReserved = value?.ToString().ToLower();
    }

    public class BannerColorsDTO
    {
        [XmlElement("Color")]
        public List<ColorEntryDTO> Colors { get; set; } = new List<ColorEntryDTO>();

        public bool ShouldSerializeColors() => Colors != null && Colors.Count > 0;

        // 便捷属性
        public int ColorsCount => Colors?.Count ?? 0;
        public bool HasColors => Colors != null && Colors.Count > 0;
    }

    public class ColorEntryDTO
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
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasHex => !string.IsNullOrEmpty(Hex);
        public bool HasPlayerCanChooseForBackground => !string.IsNullOrEmpty(PlayerCanChooseForBackground);
        public bool HasPlayerCanChooseForSigil => !string.IsNullOrEmpty(PlayerCanChooseForSigil);

        // 设置方法
        public void SetIdInt(int? value) => Id = value?.ToString();
        public void SetPlayerCanChooseForBackgroundBool(bool? value) => PlayerCanChooseForBackground = value?.ToString().ToLower();
        public void SetPlayerCanChooseForSigilBool(bool? value) => PlayerCanChooseForSigil = value?.ToString().ToLower();
    }
}