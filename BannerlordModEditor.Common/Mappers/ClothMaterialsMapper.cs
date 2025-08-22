using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ClothMaterialsMapper
    {
        public static ClothMaterialsDTO ToDTO(ClothMaterialsDO source)
        {
            if (source == null) return null;

            return new ClothMaterialsDTO
            {
                Type = source.Type,
                Materials = ClothMaterialsContainerToDTO(source.Materials)
            };
        }

        public static ClothMaterialsDO ToDO(ClothMaterialsDTO source)
        {
            if (source == null) return null;

            return new ClothMaterialsDO
            {
                Type = source.Type,
                Materials = ClothMaterialsContainerToDo(source.Materials),
                HasMaterials = source.Materials != null && source.Materials.MaterialList.Count > 0
            };
        }

        public static ClothMaterialsContainerDTO ClothMaterialsContainerToDTO(ClothMaterialsContainerDO source)
        {
            if (source == null) return null;

            return new ClothMaterialsContainerDTO
            {
                MaterialList = source.MaterialList?.Select(ClothMaterialToDTO).ToList() ?? new List<ClothMaterialDTO>()
            };
        }

        public static ClothMaterialsContainerDO ClothMaterialsContainerToDo(ClothMaterialsContainerDTO source)
        {
            if (source == null) return null;

            return new ClothMaterialsContainerDO
            {
                MaterialList = source.MaterialList?.Select(ClothMaterialToDo).ToList() ?? new List<ClothMaterialDO>()
            };
        }

        public static ClothMaterialDTO ClothMaterialToDTO(ClothMaterialDO source)
        {
            if (source == null) return null;

            return new ClothMaterialDTO
            {
                Name = source.Name,
                Simulation = ClothMaterialSimulationToDTO(source.Simulation)
            };
        }

        public static ClothMaterialDO ClothMaterialToDo(ClothMaterialDTO source)
        {
            if (source == null) return null;

            return new ClothMaterialDO
            {
                Name = source.Name,
                Simulation = ClothMaterialSimulationToDo(source.Simulation),
                HasSimulation = source.Simulation != null
            };
        }

        public static ClothMaterialSimulationDTO ClothMaterialSimulationToDTO(ClothMaterialSimulationDO source)
        {
            if (source == null) return null;

            return new ClothMaterialSimulationDTO
            {
                StretchingStiffness = source.StretchingStiffness,
                AnchorStiffness = source.AnchorStiffness,
                BendingStiffness = source.BendingStiffness,
                ShearingStiffness = source.ShearingStiffness,
                Damping = source.Damping,
                Gravity = source.Gravity,
                LinearInertia = source.LinearInertia,
                MaxLinearVelocity = source.MaxLinearVelocity,
                LinearVelocityMultiplier = source.LinearVelocityMultiplier,
                Wind = source.Wind,
                AirDragMultiplier = source.AirDragMultiplier
            };
        }

        public static ClothMaterialSimulationDO ClothMaterialSimulationToDo(ClothMaterialSimulationDTO source)
        {
            if (source == null) return null;

            return new ClothMaterialSimulationDO
            {
                StretchingStiffness = source.StretchingStiffness,
                AnchorStiffness = source.AnchorStiffness,
                BendingStiffness = source.BendingStiffness,
                ShearingStiffness = source.ShearingStiffness,
                Damping = source.Damping,
                Gravity = source.Gravity,
                LinearInertia = source.LinearInertia,
                MaxLinearVelocity = source.MaxLinearVelocity,
                LinearVelocityMultiplier = source.LinearVelocityMultiplier,
                Wind = source.Wind,
                AirDragMultiplier = source.AirDragMultiplier
            };
        }
    }
}