using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

[XmlRoot("Scales")]
public class SkeletonScales
{
    [XmlElement("SkeletonScale")]
    public List<SkeletonScale> SkeletonScaleList { get; set; } = new List<SkeletonScale>();

    public bool ShouldSerializeSkeletonScaleList() => SkeletonScaleList.Count > 0;
}

public class SkeletonScale
{
    [XmlAttribute("skeleton")]
    public string Skeleton { get; set; } = string.Empty;

    public bool ShouldSerializeSkeleton() => !string.IsNullOrEmpty(Skeleton);
}
