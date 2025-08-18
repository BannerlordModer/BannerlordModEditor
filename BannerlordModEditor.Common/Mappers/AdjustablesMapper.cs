using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class AdjustablesMapper
    {
        public static AdjustablesDTO ToDTO(AdjustablesDO source)
        {
            if (source == null) return null;
            
            return new AdjustablesDTO
            {
                Adjustables = source.Adjustables?
                    .Select(AdjustableMapper.ToDTO)
                    .ToList() ?? new List<AdjustableDTO>()
            };
        }

        public static AdjustablesDO ToDO(AdjustablesDTO source)
        {
            if (source == null) return null;
            
            return new AdjustablesDO
            {
                Adjustables = source.Adjustables?
                    .Select(AdjustableMapper.ToDO)
                    .ToList() ?? new List<AdjustableDO>()
            };
        }
    }

    public static class AdjustableMapper
    {
        public static AdjustableDTO ToDTO(AdjustableDO source)
        {
            if (source == null) return null;
            
            return new AdjustableDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static AdjustableDO ToDO(AdjustableDTO source)
        {
            if (source == null) return null;
            
            return new AdjustableDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }
    }
}