using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ActionSetsMapper
    {
        #region DO to DTO

        public static ActionSetsDTO ToDTO(ActionSetsDO source)
        {
            if (source == null) return null;

            return new ActionSetsDTO
            {
                ActionSets = source.ActionSets?.Select(ToDTO).ToList() ?? new List<ActionSetDTO>()
            };
        }

        public static ActionSetDTO ToDTO(ActionSetDO source)
        {
            if (source == null) return null;

            return new ActionSetDTO
            {
                Id = source.Id,
                Skeleton = source.Skeleton,
                MovementSystem = source.MovementSystem,
                BaseSet = source.BaseSet,
                Actions = source.Actions?.Select(ToDTO).ToList() ?? new List<ActionDTO>()
            };
        }

        public static ActionDTO ToDTO(ActionDO source)
        {
            if (source == null) return null;

            return new ActionDTO
            {
                Type = source.Type,
                Animation = source.Animation,
                AlternativeGroup = source.AlternativeGroup
            };
        }

        #endregion

        #region DTO to DO

        public static ActionSetsDO ToDO(ActionSetsDTO source)
        {
            if (source == null) return null;

            return new ActionSetsDO
            {
                ActionSets = source.ActionSets?.Select(ToDO).ToList() ?? new List<ActionSetDO>()
            };
        }

        public static ActionSetDO ToDO(ActionSetDTO source)
        {
            if (source == null) return null;

            return new ActionSetDO
            {
                Id = source.Id,
                Skeleton = source.Skeleton,
                MovementSystem = source.MovementSystem,
                BaseSet = source.BaseSet,
                Actions = source.Actions?.Select(ToDO).ToList() ?? new List<ActionDO>()
            };
        }

        public static ActionDO ToDO(ActionDTO source)
        {
            if (source == null) return null;

            return new ActionDO
            {
                Type = source.Type,
                Animation = source.Animation,
                AlternativeGroup = source.AlternativeGroup
            };
        }

        #endregion
    }
}