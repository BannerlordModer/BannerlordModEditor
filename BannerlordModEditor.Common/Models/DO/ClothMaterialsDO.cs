using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class ClothMaterialsDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "cloth_materials";

        [XmlElement("materials")]
        public ClothMaterialsContainerDO Materials { get; set; } = new ClothMaterialsContainerDO();

        [XmlIgnore]
        public bool HasMaterials { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeMaterials() => HasMaterials && Materials != null && Materials.MaterialList.Count > 0;
    }

    public class ClothMaterialsContainerDO
    {
        [XmlElement("material")]
        public List<ClothMaterialDO> MaterialList { get; set; } = new List<ClothMaterialDO>();
    }

    public class ClothMaterialDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("simulation")]
        public ClothMaterialSimulationDO? Simulation { get; set; }

        [XmlIgnore]
        public bool HasSimulation { get; set; } = false;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSimulation() => HasSimulation && Simulation != null;
    }

    public class ClothMaterialSimulationDO
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

        public bool ShouldSerializeStretchingStiffness() => !string.IsNullOrEmpty(StretchingStiffness);
        public bool ShouldSerializeAnchorStiffness() => !string.IsNullOrEmpty(AnchorStiffness);
        public bool ShouldSerializeBendingStiffness() => !string.IsNullOrEmpty(BendingStiffness);
        public bool ShouldSerializeShearingStiffness() => !string.IsNullOrEmpty(ShearingStiffness);
        public bool ShouldSerializeDamping() => !string.IsNullOrEmpty(Damping);
        public bool ShouldSerializeGravity() => !string.IsNullOrEmpty(Gravity);
        public bool ShouldSerializeLinearInertia() => !string.IsNullOrEmpty(LinearInertia);
        public bool ShouldSerializeMaxLinearVelocity() => !string.IsNullOrEmpty(MaxLinearVelocity);
        public bool ShouldSerializeLinearVelocityMultiplier() => !string.IsNullOrEmpty(LinearVelocityMultiplier);
        public bool ShouldSerializeWind() => !string.IsNullOrEmpty(Wind);
        public bool ShouldSerializeAirDragMultiplier() => !string.IsNullOrEmpty(AirDragMultiplier);
    }
}