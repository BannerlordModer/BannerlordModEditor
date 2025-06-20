using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// skeleton_scales.xml - Skeleton scaling data for different mounts/characters
[XmlRoot("Scales")]
public class SkeletonScales
{
    [XmlElement("Scale")]
    public List<Scale> Scale { get; set; } = new();
}

public class Scale
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("skeleton")]
    public string? Skeleton { get; set; }

    [XmlAttribute("mount_sit_bone_scale")]
    public string? MountSitBoneScale { get; set; }

    [XmlAttribute("mount_radius_adder")]
    public string? MountRadiusAdder { get; set; }

    [XmlElement("BoneScales")]
    public BoneScales? BoneScales { get; set; }
}

public class BoneScales
{
    [XmlElement("BoneScale")]
    public List<BoneScale> BoneScale { get; set; } = new();
}

public class BoneScale
{
    [XmlAttribute("bone_name")]
    public string? BoneName { get; set; }

    [XmlAttribute("scale")]
    public string? Scale { get; set; }
} 