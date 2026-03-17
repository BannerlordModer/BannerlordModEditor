using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("WorkshopTypes")]
public class SPWorkshops
{
    [XmlElement("WorkshopType")]
    public List<WorkshopType> WorkshopTypeList { get; set; } = new List<WorkshopType>();
}

public class WorkshopType
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("equipment_cost")]
    public string EquipmentCost { get; set; } = string.Empty;

    [XmlAttribute("frequency")]
    public string Frequency { get; set; } = string.Empty;

    [XmlAttribute("jobname")]
    public string JobName { get; set; } = string.Empty;

    [XmlAttribute("description")]
    public string Description { get; set; } = string.Empty;

    [XmlAttribute("isHidden")]
    public string? IsHidden { get; set; }

    [XmlElement("Meshes")]
    public Meshes? Meshes { get; set; }

    [XmlElement("Production")]
    public List<Production> ProductionList { get; set; } = new List<Production>();

    public bool ShouldSerializeIsHidden() => !string.IsNullOrEmpty(IsHidden);
}

public class Meshes
{
    [XmlAttribute("sign_mesh_name")]
    public string? SignMeshName { get; set; }

    [XmlAttribute("shop_prop_mesh_name_1")]
    public string? ShopPropMeshName1 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_2")]
    public string? ShopPropMeshName2 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_3_1")]
    public string? ShopPropMeshName31 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_3_2")]
    public string? ShopPropMeshName32 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_3_3")]
    public string? ShopPropMeshName33 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_4")]
    public string? ShopPropMeshName4 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_5")]
    public string? ShopPropMeshName5 { get; set; }

    [XmlAttribute("shop_prop_mesh_name_6")]
    public string? ShopPropMeshName6 { get; set; }

    public bool ShouldSerializeShopPropMeshName6() => !string.IsNullOrEmpty(ShopPropMeshName6);
}

public class Production
{
    [XmlAttribute("conversion_speed")]
    public string ConversionSpeed { get; set; } = string.Empty;

    [XmlElement("Inputs")]
    public Inputs? Inputs { get; set; }

    [XmlElement("Outputs")]
    public Outputs? Outputs { get; set; }
}

public class Inputs
{
    [XmlElement("Input")]
    public List<InputItem> InputList { get; set; } = new List<InputItem>();
}

public class InputItem
{
    [XmlAttribute("input_item")]
    public string InputItemValue { get; set; } = string.Empty;
}

public class Outputs
{
    [XmlElement("Output")]
    public List<OutputItem> OutputList { get; set; } = new List<OutputItem>();
}

public class OutputItem
{
    [XmlAttribute("output")]
    public string OutputValue { get; set; } = string.Empty;

    [XmlAttribute("output_count")]
    public string? OutputCount { get; set; }

    public bool ShouldSerializeOutputCount() => !string.IsNullOrEmpty(OutputCount);
}
