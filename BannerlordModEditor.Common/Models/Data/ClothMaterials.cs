using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ClothMaterialsRoot
    {
        [XmlElement("materials")]
        public ClothMaterials Materials { get; set; }

        public bool ShouldSerializeMaterials() => Materials != null;
    }

    public class ClothMaterials
    {
        [XmlElement("material")]
        public List<ClothMaterial> MaterialList { get; set; }

        public bool ShouldSerializeMaterialList() => MaterialList != null && MaterialList.Count > 0;
    }

    public class ClothMaterial
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("simulation")]
        public ClothMaterialSimulation Simulation { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSimulation() => Simulation != null;
    }

    public class ClothMaterialSimulation
    {
        [XmlAttribute("stretching_stiffness")]
        public double StretchingStiffness { get; set; }
        public bool ShouldSerializeStretchingStiffness() => true;

        [XmlAttribute("anchor_stiffness")]
        public double AnchorStiffness { get; set; }
        public bool ShouldSerializeAnchorStiffness() => true;

        [XmlAttribute("bending_stiffness")]
        public double BendingStiffness { get; set; }
        public bool ShouldSerializeBendingStiffness() => true;

        [XmlAttribute("shearing_stiffness_")]
        public double ShearingStiffness { get; set; }
        public bool ShouldSerializeShearingStiffness() => true;

        [XmlAttribute("damping")]
        public double Damping { get; set; }
        public bool ShouldSerializeDamping() => true;

        [XmlAttribute("gravity")]
        public double Gravity { get; set; }
        public bool ShouldSerializeGravity() => true;

        [XmlAttribute("linear_inertia")]
        public double LinearInertia { get; set; }
        public bool ShouldSerializeLinearInertia() => true;

        [XmlAttribute("max_linear_velocity")]
        public double MaxLinearVelocity { get; set; }
        public bool ShouldSerializeMaxLinearVelocity() => true;

        [XmlAttribute("linear_velocity_multiplier")]
        public double LinearVelocityMultiplier { get; set; }
        public bool ShouldSerializeLinearVelocityMultiplier() => true;

        [XmlAttribute("wind")]
        public double Wind { get; set; }
        public bool ShouldSerializeWind() => true;

        [XmlAttribute("air_drag_multiplier")]
        public double AirDragMultiplier { get; set; }
        public bool ShouldSerializeAirDragMultiplier() => true;
    }
}