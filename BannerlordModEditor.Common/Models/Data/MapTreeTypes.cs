using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class MapTreeTypes
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("tree_types")]
        public TreeTypes TreeTypes { get; set; } = new TreeTypes();
    }

    public class TreeTypes
    {
        [XmlElement("tree_type")]
        public List<TreeType> TreeTypeList { get; set; } = new List<TreeType>();
    }

    public class TreeType
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("tree")]
        public List<Tree> Trees { get; set; } = new List<Tree>();
    }

    public class Tree
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("lod_mesh_name")]
        public string? LodMeshName { get; set; }

        public bool ShouldSerializeLodMeshName()
        {
            return !string.IsNullOrEmpty(LodMeshName);
        }
    }
} 