using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("EquipmentRosters")]
    public class NativeEquipmentSets
    {
        [XmlElement("EquipmentRoster")]
        public List<NativeEquipmentRoster> EquipmentRosters { get; set; } = new List<NativeEquipmentRoster>();
    }

    public class NativeEquipmentRoster
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("equipment")]
        public List<NativeEquipment> Equipment { get; set; } = new List<NativeEquipment>();

        // ShouldSerialize methods to control serialization
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeEquipment() => Equipment.Count > 0;
    }

    public class NativeEquipment
    {
        [XmlAttribute("slot")]
        public string Slot { get; set; } = string.Empty;

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        // ShouldSerialize methods to control serialization
        public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
}