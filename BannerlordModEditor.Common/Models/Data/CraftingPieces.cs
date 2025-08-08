using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class CraftingPieces
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "crafting_piece";

        [XmlElement("crafting_pieces")]
        public CraftingPiecesContainer CraftingPiecesContainer { get; set; } = new CraftingPiecesContainer();
    }

    public class CraftingPiecesContainer
    {
        [XmlElement("crafting_piece")]
        public List<CraftingPiece> Pieces { get; set; } = new List<CraftingPiece>();

        public bool ShouldSerializePieces() => Pieces != null && Pieces.Count > 0;
    }

    public class CraftingPiece : XmlModelBase
    {
        [XmlAttribute("id")]
        public string Id
        {
            get => _id;
            set { _id = value; MarkPropertyExists(nameof(Id)); }
        }
        private string _id;

        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set { _name = value; MarkPropertyExists(nameof(Name)); }
        }
        private string _name;

        [XmlAttribute("tier")]
        public NullableNumericProperty<int> Tier { get; set; } = new();

        [XmlAttribute("piece_type")]
        public string PieceType
        {
            get => _pieceType;
            set { _pieceType = value; MarkPropertyExists(nameof(PieceType)); }
        }
        private string _pieceType;

        [XmlAttribute("mesh")]
        public string Mesh
        {
            get => _mesh;
            set { _mesh = value; MarkPropertyExists(nameof(Mesh)); }
        }
        private string _mesh;

        [XmlAttribute("culture")]
        public string Culture
        {
            get => _culture;
            set { _culture = value; MarkPropertyExists(nameof(Culture)); }
        }
        private string _culture;

        [XmlAttribute("length")]
        public NullableNumericProperty<double> Length { get; set; } = new();

        [XmlAttribute("weight")]
        public NullableNumericProperty<double> Weight { get; set; } = new();

        [XmlAttribute("is_hidden")]
        public NullableNumericProperty<bool> IsHidden { get; set; } = new();

        [XmlAttribute("is_default")]
        public NullableNumericProperty<bool> IsDefault { get; set; } = new();

        [XmlAttribute("CraftingCost")]
        public NullableNumericProperty<int> CraftingCost { get; set; } = new();

        [XmlElement("BuildData")]
        public BuildData BuildData { get; set; }
        public bool ShouldSerializeBuildData() => BuildData != null;

        [XmlElement("BladeData")]
        public BladeData BladeData { get; set; }
        public bool ShouldSerializeBladeData() => BladeData != null;

        [XmlElement("Flags")]
        public Flags Flags { get; set; }
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList != null && Flags.FlagList.Count > 0;

        [XmlElement("Materials")]
        public Materials Materials { get; set; }
        public bool ShouldSerializeMaterials() => Materials != null && Materials.MaterialList != null && Materials.MaterialList.Count > 0;
    }

    public class BuildData
    {
        [XmlElement("previous_piece_offset")]
        public double? PreviousPieceOffset { get; set; }
        public bool ShouldSerializePreviousPieceOffset() => PreviousPieceOffset.HasValue;
    }

    public class BladeData
    {
        [XmlElement("stack_amount")]
        public int? StackAmount { get; set; }
        public bool ShouldSerializeStackAmount() => StackAmount.HasValue;

        [XmlAttribute("physics_material")]
        public string PhysicsMaterial { get; set; }

        [XmlAttribute("body_name")]
        public string BodyName { get; set; }

        [XmlAttribute("holster_mesh")]
        public string HolsterMesh { get; set; }
        public bool ShouldSerializeHolsterMesh() => !string.IsNullOrEmpty(HolsterMesh);

        [XmlElement("Thrust")]
        public Thrust Thrust { get; set; }
        public bool ShouldSerializeThrust() => Thrust != null;

        [XmlElement("Swing")]
        public Swing Swing { get; set; }
        public bool ShouldSerializeSwing() => Swing != null;
    }

    public class Thrust
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlElement("damage_factor")]
        public double? DamageFactor { get; set; }
        public bool ShouldSerializeDamageFactor() => DamageFactor.HasValue;
    }

    public class Swing
    {
        [XmlAttribute("damage_type")]
        public string DamageType { get; set; }

        [XmlElement("damage_factor")]
        public double? DamageFactor { get; set; }
        public bool ShouldSerializeDamageFactor() => DamageFactor.HasValue;
    }

    public class Flags
    {
        [XmlElement("Flag")]
        public List<Flag> FlagList { get; set; }
    }

    public class Flag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    }

    public class Materials
    {
        [XmlElement("Material")]
        public List<Material> MaterialList { get; set; }
    }

    public class Material
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("count")]
        public int? Count { get; set; }
        public bool ShouldSerializeCount() => Count.HasValue;
    }
}