using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class SoundFilesDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "sound";

        [XmlElement("bank_files")]
        public BankFilesDO BankFiles { get; set; } = new BankFilesDO();

        [XmlElement("asset_files")]
        public AssetFilesDO AssetFiles { get; set; } = new AssetFilesDO();

        [XmlIgnore]
        public bool HasBankFiles { get; set; } = false;

        [XmlIgnore]
        public bool HasAssetFiles { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeBankFiles() => HasBankFiles && BankFiles != null && BankFiles.File.Count > 0;
        public bool ShouldSerializeAssetFiles() => HasAssetFiles && AssetFiles != null && AssetFiles.File.Count > 0;
    }

    public class BankFilesDO
    {
        [XmlElement("file")]
        public List<SoundFileDO> File { get; set; } = new List<SoundFileDO>();
    }

    public class AssetFilesDO
    {
        [XmlElement("file")]
        public List<SoundFileDO> File { get; set; } = new List<SoundFileDO>();
    }

    public class SoundFileDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("decompress_samples")]
        public string DecompressSamples { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeDecompressSamples() => !string.IsNullOrEmpty(DecompressSamples);
    }
}