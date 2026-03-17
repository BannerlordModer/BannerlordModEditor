using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_3_15;

// settlements.xml - Settlements for Bannerlord 1.3.15+
// Root element: <Settlements>
// Note: V1_3_15 adds Buildings element to Town class
[XmlRoot("Settlements")]
public class Settlements
{
    [XmlElement("Settlement")]
    public List<Settlement> SettlementList { get; set; } = new List<Settlement>();
}

// Settlement attributes:
// - id, name, owner, posX, posY, culture, gate_posX, gate_posY, text
// Settlement can be of type: town, castle, village
public class Settlement
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("posX")]
    public string PosX { get; set; } = string.Empty;

    [XmlAttribute("posY")]
    public string PosY { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("owner")]
    public string? Owner { get; set; }

    [XmlAttribute("culture")]
    public string? Culture { get; set; }

    [XmlAttribute("gate_posX")]
    public string? GatePosX { get; set; }

    [XmlAttribute("gate_posY")]
    public string? GatePosY { get; set; }

    [XmlAttribute("text")]
    public string? Text { get; set; }

    // Child elements
    // Components contains either Town (for towns/castles) or Village (for villages)
    [XmlElement("Components")]
    public Components? Components { get; set; }

    [XmlElement("Locations")]
    public Locations? Locations { get; set; }

    [XmlElement("CommonAreas")]
    public CommonAreas? CommonAreas { get; set; }

    // ShouldSerialize methods for optional attributes - only serialize if value is not null/empty
    public bool ShouldSerializeOwner() => !string.IsNullOrEmpty(Owner);
    public bool ShouldSerializeCulture() => !string.IsNullOrEmpty(Culture);
    public bool ShouldSerializeGatePosX() => !string.IsNullOrEmpty(GatePosX);
    public bool ShouldSerializeGatePosY() => !string.IsNullOrEmpty(GatePosY);
    public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
    public bool ShouldSerializeComponents() => Components != null && (Components.HasTown || Components.HasVillage);
    public bool ShouldSerializeLocations() => Locations != null && Locations.LocationList.Count > 0;
    public bool ShouldSerializeCommonAreas() => CommonAreas != null && CommonAreas.AreaList.Count > 0;
}

// Components contains either Town or Village (polymorphic)
public class Components
{
    // Using XmlElement with type info for polymorphic deserialization
    // Town for towns/castles, Village for villages
    [XmlElement(typeof(Town))]
    [XmlElement(typeof(Village))]
    public object? Component { get; set; }

    // Helper properties to check which type is present
    [XmlIgnore]
    public bool HasTown => Component is Town;

    [XmlIgnore]
    public bool HasVillage => Component is Village;

    // Helper to get/set Town
    public Town? Town => Component as Town;

    public void SetTown(Town town) => Component = town;

    // Helper to get/set Village
    public Village? Village => Component as Village;

    public void SetVillage(Village village) => Component = village;
}

// Town/castle attributes:
// id, is_castle, level, background_crop_position, background_mesh, wait_mesh, gate_rotation, prosperity, castle_background_mesh
// V1_3_15+: Added Buildings element
public class Town
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("is_castle")]
    public string IsCastle { get; set; } = string.Empty;

    [XmlAttribute("level")]
    public string Level { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("background_crop_position")]
    public string? BackgroundCropPosition { get; set; }

    [XmlAttribute("background_mesh")]
    public string? BackgroundMesh { get; set; }

    [XmlAttribute("wait_mesh")]
    public string? WaitMesh { get; set; }

    [XmlAttribute("gate_rotation")]
    public string? GateRotation { get; set; }

    [XmlAttribute("prosperity")]
    public string? Prosperity { get; set; }

    [XmlAttribute("castle_background_mesh")]
    public string? CastleBackgroundMesh { get; set; }

    // V1_3_15+ Buildings element
    [XmlElement("Buildings")]
    public Buildings? Buildings { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeBackgroundCropPosition() => !string.IsNullOrEmpty(BackgroundCropPosition);
    public bool ShouldSerializeBackgroundMesh() => !string.IsNullOrEmpty(BackgroundMesh);
    public bool ShouldSerializeWaitMesh() => !string.IsNullOrEmpty(WaitMesh);
    public bool ShouldSerializeGateRotation() => !string.IsNullOrEmpty(GateRotation);
    public bool ShouldSerializeProsperity() => !string.IsNullOrEmpty(Prosperity);
    public bool ShouldSerializeCastleBackgroundMesh() => !string.IsNullOrEmpty(CastleBackgroundMesh);
    public bool ShouldSerializeBuildings() => Buildings != null && Buildings.BuildingList.Count > 0;
}

// V1_3_15+ Buildings container
public class Buildings
{
    [XmlElement("Building")]
    public List<Building> BuildingList { get; set; } = new List<Building>();
}

// V1_3_15+ Building element
public class Building
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("level")]
    public string Level { get; set; } = string.Empty;

    // ShouldSerialize methods
    public bool ShouldSerializeLevel() => !string.IsNullOrEmpty(Level);
}

// Village attributes:
// id, village_type, hearth, bound
public class Village
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("village_type")]
    public string VillageType { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("hearth")]
    public string? Hearth { get; set; }

    [XmlAttribute("bound")]
    public string? Bound { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeHearth() => !string.IsNullOrEmpty(Hearth);
    public bool ShouldSerializeBound() => !string.IsNullOrEmpty(Bound);
}

// Location attributes:
// id, scene_name, scene_name_1, scene_name_2, scene_name_3, max_prosperity
public class Locations
{
    [XmlElement("Location")]
    public List<Location> LocationList { get; set; } = new List<Location>();
}

public class Location
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Optional attributes
    [XmlAttribute("scene_name")]
    public string? SceneName { get; set; }

    [XmlAttribute("scene_name_1")]
    public string? SceneName1 { get; set; }

    [XmlAttribute("scene_name_2")]
    public string? SceneName2 { get; set; }

    [XmlAttribute("scene_name_3")]
    public string? SceneName3 { get; set; }

    [XmlAttribute("max_prosperity")]
    public string? MaxProsperity { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeSceneName() => !string.IsNullOrEmpty(SceneName);
    public bool ShouldSerializeSceneName1() => !string.IsNullOrEmpty(SceneName1);
    public bool ShouldSerializeSceneName2() => !string.IsNullOrEmpty(SceneName2);
    public bool ShouldSerializeSceneName3() => !string.IsNullOrEmpty(SceneName3);
    public bool ShouldSerializeMaxProsperity() => !string.IsNullOrEmpty(MaxProsperity);
}

// CommonAreas - contains Area elements
public class CommonAreas
{
    [XmlElement("Area")]
    public List<Area> AreaList { get; set; } = new List<Area>();
}

public class Area
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("focus")]
    public string? Focus { get; set; }

    [XmlAttribute("cultures")]
    public string? Cultures { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeFocus() => !string.IsNullOrEmpty(Focus);
    public bool ShouldSerializeCultures() => !string.IsNullOrEmpty(Cultures);
}
