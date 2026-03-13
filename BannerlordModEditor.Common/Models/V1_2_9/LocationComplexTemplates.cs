using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

// location_complex_templates.xml - Location Complex Templates for Bannerlord 1.2.9
// Root element: <LocationComplexTemplates>
[XmlRoot("LocationComplexTemplates")]
public class LocationComplexTemplates
{
    [XmlElement("LocationComplexTemplate")]
    public List<LocationComplexTemplate> LocationComplexTemplateList { get; set; } = new List<LocationComplexTemplate>();
}

// LocationComplexTemplate attributes:
// - id
public class LocationComplexTemplate
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Child elements
    [XmlElement("Stages")]
    public Stages? Stages { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeStages() => Stages != null && Stages.StageList.Count > 0;
}

// Stages contains Stage elements
public class Stages
{
    [XmlElement("Stage")]
    public List<Stage> StageList { get; set; } = new List<Stage>();
}

// Stage attributes:
// - id
public class Stage
{
    // Required attributes
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    // Child elements
    [XmlElement("Scenes")]
    public Scenes? Scenes { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeScenes() => Scenes != null && Scenes.SceneList.Count > 0;
}

// Scenes contains Scene elements
public class Scenes
{
    [XmlElement("Scene")]
    public List<Scene> SceneList { get; set; } = new List<Scene>();
}

// Scene - no direct attributes, contains InteriorScenes
public class Scene
{
    // Child elements
    [XmlElement("InteriorScenes")]
    public InteriorScenes? InteriorScenes { get; set; }

    // ShouldSerialize methods
    public bool ShouldSerializeInteriorScenes() => InteriorScenes != null && InteriorScenes.InteriorSceneList.Count > 0;
}

// InteriorScenes contains InteriorScene elements
public class InteriorScenes
{
    [XmlElement("InteriorScene")]
    public List<InteriorScene> InteriorSceneList { get; set; } = new List<InteriorScene>();
}

// InteriorScene attributes:
// - scene
public class InteriorScene
{
    // Required attributes
    [XmlAttribute("scene")]
    public string Scene { get; set; } = string.Empty;

    // ShouldSerialize methods
    public bool ShouldSerializeScene() => !string.IsNullOrEmpty(Scene);
}
