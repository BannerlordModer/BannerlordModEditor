using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("CraftingPieces")]
    public class MpCraftingPiecesDO
    {
        [XmlElement("CraftingPiece")]
        public List<MpCraftingPieceDO> CraftingPieceList { get; set; } = new List<MpCraftingPieceDO>();

        public bool ShouldSerializeCraftingPieceList() => CraftingPieceList != null && CraftingPieceList.Count > 0;
    }

    public class MpCraftingPieceDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tier")]
        public string Tier { get; set; }

        [XmlAttribute("piece_type")]
        public string PieceType { get; set; }

        [XmlAttribute("mesh")]
        public string Mesh { get; set; }

        [XmlAttribute("scale")]
        public string Scale { get; set; }

        [XmlAttribute("is_craftable")]
        public string IsCraftable { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTier() => !string.IsNullOrEmpty(Tier);
        public bool ShouldSerializePieceType() => !string.IsNullOrEmpty(PieceType);
        public bool ShouldSerializeMesh() => !string.IsNullOrEmpty(Mesh);
        public bool ShouldSerializeScale() => !string.IsNullOrEmpty(Scale);
        public bool ShouldSerializeIsCraftable() => !string.IsNullOrEmpty(IsCraftable);
    }
}