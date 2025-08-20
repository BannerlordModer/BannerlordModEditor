using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

[XmlRoot("Credits")]
public class CreditsExternalPartnersPlayStationDTO
{
    [XmlElement("Category")]
    public List<CreditsCategoryDTO> Categories { get; set; } = new List<CreditsCategoryDTO>();

    [XmlElement("LoadFromFile")]
    public List<CreditsLoadFromFileDTO> LoadFromFile { get; set; } = new List<CreditsLoadFromFileDTO>();

    public bool ShouldSerializeCategories() => Categories != null && Categories.Count > 0;
    public bool ShouldSerializeLoadFromFile() => LoadFromFile != null && LoadFromFile.Count > 0;
}