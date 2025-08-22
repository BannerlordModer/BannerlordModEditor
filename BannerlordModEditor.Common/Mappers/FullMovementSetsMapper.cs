using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class FullMovementSetsMapper
    {
        public static FullMovementSetsDTO ToDTO(FullMovementSetsDO source)
        {
            if (source == null) return null;

            return new FullMovementSetsDTO
            {
                FullMovementSet = source.FullMovementSet?.Select(FullMovementSetToDTO).ToList() ?? new List<FullMovementSetDTO>()
            };
        }

        public static FullMovementSetsDO ToDO(FullMovementSetsDTO source)
        {
            if (source == null) return null;

            return new FullMovementSetsDO
            {
                FullMovementSet = source.FullMovementSet?.Select(FullMovementSetToDo).ToList() ?? new List<FullMovementSetDO>()
            };
        }

        public static FullMovementSetDTO FullMovementSetToDTO(FullMovementSetDO source)
        {
            if (source == null) return null;

            return new FullMovementSetDTO
            {
                Id = source.Id,
                BaseSet = source.BaseSet,
                MovementSet = source.MovementSet?.Select(FullMovementSetDataToDTO).ToList() ?? new List<FullMovementSetDataDTO>()
            };
        }

        public static FullMovementSetDO FullMovementSetToDo(FullMovementSetDTO source)
        {
            if (source == null) return null;

            return new FullMovementSetDO
            {
                Id = source.Id,
                BaseSet = source.BaseSet,
                MovementSet = source.MovementSet?.Select(FullMovementSetDataToDo).ToList() ?? new List<FullMovementSetDataDO>()
            };
        }

        public static FullMovementSetDataDTO FullMovementSetDataToDTO(FullMovementSetDataDO source)
        {
            if (source == null) return null;

            return new FullMovementSetDataDTO
            {
                Id = source.Id,
                LeftStance = source.LeftStance,
                MovementMode = source.MovementMode
            };
        }

        public static FullMovementSetDataDO FullMovementSetDataToDo(FullMovementSetDataDTO source)
        {
            if (source == null) return null;

            return new FullMovementSetDataDO
            {
                Id = source.Id,
                LeftStance = source.LeftStance,
                MovementMode = source.MovementMode
            };
        }
    }
}