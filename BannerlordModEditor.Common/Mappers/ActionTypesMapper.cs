using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ActionTypesMapper
    {
        #region DO to DTO

        public static ActionTypesDTO ToDTO(ActionTypesDO source)
        {
            if (source == null) return null;

            return new ActionTypesDTO
            {
                Actions = source.Actions?.Select(ToDTO).ToList() ?? new List<ActionTypeDTO>()
            };
        }

        public static ActionTypeDTO ToDTO(ActionTypeDO source)
        {
            if (source == null) return null;

            return new ActionTypeDTO
            {
                Name = source.Name,
                Type = source.Type,
                UsageDirection = source.UsageDirection,
                ActionStage = source.ActionStage
            };
        }

        #endregion

        #region DTO to DO

        public static ActionTypesDO ToDO(ActionTypesDTO source)
        {
            if (source == null) return null;

            return new ActionTypesDO
            {
                Actions = source.Actions?.Select(ToDO).ToList() ?? new List<ActionTypeDO>()
            };
        }

        public static ActionTypeDO ToDO(ActionTypeDTO source)
        {
            if (source == null) return null;

            return new ActionTypeDO
            {
                Name = source.Name,
                Type = source.Type,
                UsageDirection = source.UsageDirection,
                ActionStage = source.ActionStage
            };
        }

        #endregion
    }
}