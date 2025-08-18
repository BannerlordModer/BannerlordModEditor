using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Configuration
{
    /// <summary>
    /// Represents the native_strings.xml file structure containing localization strings
    /// </summary>
    [XmlRoot("base")]
    public class NativeStringsDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Language tags for the strings
        /// </summary>
        [XmlElement("tags")]
        public NativeStringTagsDO? Tags { get; set; }

        /// <summary>
        /// Collection of localized strings
        /// </summary>
        [XmlElement("strings")]
        public NativeStringsCollectionDO? Strings { get; set; }

        // Serialization control methods
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeTags() => Tags != null;
        public bool ShouldSerializeStrings() => Strings != null;
    }

    /// <summary>
    /// Represents language tags for localization
    /// </summary>
    public class NativeStringTagsDO
    {
        [XmlElement("tag")]
        public List<NativeStringTagDO> TagList { get; set; } = new List<NativeStringTagDO>();

        [XmlIgnore]
        public bool HasTagList => TagList != null && TagList.Count > 0;
    }

    /// <summary>
    /// Represents a language tag
    /// </summary>
    public class NativeStringTagDO
    {
        [XmlAttribute("language")]
        public string? Language { get; set; }

        public bool ShouldSerializeLanguage() => !string.IsNullOrEmpty(Language);
    }

    /// <summary>
    /// Represents a collection of localized strings
    /// </summary>
    public class NativeStringsCollectionDO
    {
        [XmlElement("string")]
        public List<NativeStringItemDO> StringList { get; set; } = new List<NativeStringItemDO>();

        [XmlIgnore]
        public bool HasStringList => StringList != null && StringList.Count > 0;
    }

    /// <summary>
    /// Represents an individual localized string
    /// </summary>
    public class NativeStringItemDO
    {
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("text")]
        public string? Text { get; set; }

        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    }
}