using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("base")]
    public class ClothMaterialsBase
    {
        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public List<ClothMaterial> Materials { get; set; } = new List<ClothMaterial>();
    }

    public class ClothMaterial
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("simulation")]
        public Simulation Simulation { get; set; } = new Simulation();
    }

    public class Simulation
    {
        [XmlAttribute("stretching_stiffness")]
        public string StretchingStiffness { get; set; } = string.Empty;

        [XmlAttribute("anchor_stiffness")]
        public string AnchorStiffness { get; set; } = string.Empty;

        [XmlAttribute("bending_stiffness")]
        public string BendingStiffness { get; set; } = string.Empty;

        [XmlAttribute("shearing_stiffness_")]
        public string ShearingStiffness { get; set; } = string.Empty;

        [XmlAttribute("damping")]
        public string Damping { get; set; } = string.Empty;

        [XmlAttribute("gravity")]
        public string Gravity { get; set; } = string.Empty;

        [XmlAttribute("linear_inertia")]
        public string LinearInertia { get; set; } = string.Empty;

        [XmlAttribute("max_linear_velocity")]
        public string MaxLinearVelocity { get; set; } = string.Empty;

        [XmlAttribute("linear_velocity_multiplier")]
        public string LinearVelocityMultiplier { get; set; } = string.Empty;

        [XmlAttribute("wind")]
        public string Wind { get; set; } = string.Empty;

        [XmlAttribute("air_drag_multiplier")]
        public string AirDragMultiplier { get; set; } = string.Empty;
    }
} 