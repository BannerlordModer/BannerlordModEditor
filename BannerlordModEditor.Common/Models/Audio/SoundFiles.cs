using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Audio
{
    /// <summary>
    /// Root element for soundfiles.xml - Contains sound bank and asset file definitions
    /// </summary>
    [XmlRoot("base")]
    public class SoundFilesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "sound";

        [XmlElement("bank_files")]
        public SoundBankFilesContainer? BankFiles { get; set; }

        [XmlElement("asset_files")]
        public SoundAssetFilesContainer? AssetFiles { get; set; }
    }

    /// <summary>
    /// Container for sound bank files
    /// </summary>
    public class SoundBankFilesContainer
    {
        [XmlElement("file")]
        public List<SoundFile> File { get; set; } = new List<SoundFile>();
    }

    /// <summary>
    /// Container for sound asset files
    /// </summary>
    public class SoundAssetFilesContainer
    {
        [XmlElement("file")]
        public List<SoundFile> File { get; set; } = new List<SoundFile>();
    }

    /// <summary>
    /// Individual sound file definition with compression settings
    /// </summary>
    public class SoundFile
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("decompress_samples")]
        public string DecompressSamples { get; set; } = string.Empty;
    }
}