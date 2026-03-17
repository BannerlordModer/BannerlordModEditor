using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.V1_2_9;

[XmlRoot("Characters")]
public class Characters
{
    [XmlElement("Character")]
    public List<Character> CharacterList { get; set; } = new List<Character>();
}

public class Character
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("level")]
    public string Level { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("occupation")]
    public string? Occupation { get; set; }

    [XmlElement("face_mesh")]
    public FaceMesh? FaceMesh { get; set; }

    [XmlElement("skills")]
    public Skills? Skills { get; set; }

    [XmlElement("equipmentSet")]
    public EquipmentSet? EquipmentSet { get; set; }

    [XmlElement("formation_class")]
    public FormationClass? FormationClass { get; set; }

    public bool ShouldSerializeOccupation() => !string.IsNullOrEmpty(Occupation);
    public bool ShouldSerializeFaceMesh() => FaceMesh != null && FaceMesh.HasContent;
    public bool ShouldSerializeSkills() => Skills != null && Skills.HasContent;
    public bool ShouldSerializeEquipmentSet() => EquipmentSet != null && EquipmentSet.HasContent;
    public bool ShouldSerializeFormationClass() => FormationClass != null && FormationClass.HasContent;
}

public class FaceMesh
{
    [XmlElement("body_parent")]
    public BodyParent? BodyParent { get; set; }

    [XmlIgnore]
    public bool HasContent => BodyParent != null && BodyParent.HasContent;
}

public class BodyParent
{
    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlIgnore]
    public bool HasContent => !string.IsNullOrEmpty(Age);

    public bool ShouldSerializeAge() => !string.IsNullOrEmpty(Age);
}

public class Skills
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class EquipmentSet
{
    [XmlIgnore]
    public bool HasContent => false;
}

public class FormationClass
{
    [XmlIgnore]
    public bool HasContent => false;
}
