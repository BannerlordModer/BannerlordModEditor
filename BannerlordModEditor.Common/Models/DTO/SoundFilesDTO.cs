using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class SoundFilesDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "sound";

        [XmlElement("bank_files")]
        public BankFilesDTO BankFiles { get; set; } = new BankFilesDTO();

        [XmlElement("asset_files")]
        public AssetFilesDTO AssetFiles { get; set; } = new AssetFilesDTO();
    }

    public class BankFilesDTO
    {
        [XmlElement("file")]
        public List<SoundFileDTO> File { get; set; } = new List<SoundFileDTO>();
    }

    public class AssetFilesDTO
    {
        [XmlElement("file")]
        public List<SoundFileDTO> File { get; set; } = new List<SoundFileDTO>();
    }

    public class SoundFileDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("decompress_samples")]
        public string DecompressSamples { get; set; } = string.Empty;
    }
}