using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Audio
{
    /// <summary>
    /// Root element for soundfiles.xml - Contains sound bank and asset file definitions
    /// </summary>
    [XmlRoot("base")]
    public class SoundFilesBase
    {
       private string? _type;
       private bool _typeExists;

       [XmlAttribute("type")]
       public string? Type
       {
           get => _typeExists ? _type : null;
           set
           {
               _type = value;
               _typeExists = true;
           }
       }

       public bool ShouldSerializeType() => _typeExists;

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
       private string? _name;
       private bool _nameExists;
       private string? _decompressSamples;
       private bool _decompressSamplesExists;

       [XmlAttribute("name")]
       public string? Name
       {
           get => _nameExists ? _name : null;
           set
           {
               _name = value;
               _nameExists = true;
           }
       }

       [XmlAttribute("decompress_samples")]
       public string? DecompressSamples
       {
           get => _decompressSamplesExists ? _decompressSamples : null;
           set
           {
               _decompressSamples = value;
               _decompressSamplesExists = true;
           }
       }

       public bool ShouldSerializeName() => _nameExists;
       public bool ShouldSerializeDecompressSamples() => _decompressSamplesExists;
    }
}