using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SoundFilesRoot
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("bank_files")]
        public BankFiles? BankFiles { get; set; }

        [XmlElement("asset_files")]
        public AssetFiles? AssetFiles { get; set; }
    }

    public class BankFiles
    {
        [XmlElement("file")]
        public SoundFile[]? File { get; set; }
    }

    public class AssetFiles
    {
        [XmlElement("file")]
        public SoundFile[]? File { get; set; }
    }

    public class SoundFile
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("decompress_samples")]
        public string? DecompressSamples { get; set; }
    }
} 