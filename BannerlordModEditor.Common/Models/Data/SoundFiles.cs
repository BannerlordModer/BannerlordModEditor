using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SoundFiles
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("bank_files")]
        public BankFiles BankFiles { get; set; } = new BankFiles();

        [XmlElement("asset_files")]
        public AssetFiles AssetFiles { get; set; } = new AssetFiles();
    }

    public class BankFiles
    {
        [XmlElement("file")]
        public List<SoundFile> Files { get; set; } = new List<SoundFile>();
    }

    public class AssetFiles
    {
        [XmlElement("file")]
        public List<SoundFile> Files { get; set; } = new List<SoundFile>();
    }

    public class SoundFile
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("decompress_samples")]
        public string DecompressSamples { get; set; } = string.Empty;
    }
} 