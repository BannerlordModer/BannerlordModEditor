using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Configuration
{
    /// <summary>
    /// Represents the native_strings.xml file structure containing localization strings
    /// </summary>
    [XmlRoot("base")]
    public class NativeStringsDTO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Language tags for the strings
        /// </summary>
        [XmlElement("tags")]
        public NativeStringTagsDTO? Tags { get; set; }

        /// <summary>
        /// Collection of localized strings
        /// </summary>
        [XmlElement("strings")]
        public NativeStringsCollectionDTO? Strings { get; set; }
    }

    /// <summary>
    /// Represents language tags for localization
    /// </summary>
    public class NativeStringTagsDTO
    {
        [XmlElement("tag")]
        public List<NativeStringTagDTO> TagList { get; set; } = new List<NativeStringTagDTO>();
    }

    /// <summary>
    /// Represents a language tag
    /// </summary>
    public class NativeStringTagDTO
    {
        [XmlAttribute("language")]
        public string? Language { get; set; }
    }

    /// <summary>
    /// Represents a collection of localized strings
    /// </summary>
    public class NativeStringsCollectionDTO
    {
        [XmlElement("string")]
        public List<NativeStringItemDTO> StringList { get; set; } = new List<NativeStringItemDTO>();
    }

    /// <summary>
    /// Represents an individual localized string
    /// </summary>
    public class NativeStringItemDTO
    {
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string? Text { get; set; }
    }
}