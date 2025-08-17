using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class AttributesMapper
    {
        public static AttributesDTO ToDTO(AttributesDO source)
        {
            if (source == null) return null;
            
            return new AttributesDTO
            {
                Attributes = source.Attributes?
                    .Select(AttributeDataMapper.ToDTO)
                    .ToList() ?? new List<AttributeDataDTO>()
            };
        }
        
        public static AttributesDO ToDO(AttributesDTO source)
        {
            if (source == null) return null;
            
            return new AttributesDO
            {
                Attributes = source.Attributes?
                    .Select(AttributeDataMapper.ToDO)
                    .ToList() ?? new List<AttributeDataDO>()
            };
        }
    }

    public static class AttributeDataMapper
    {
        public static AttributeDataDTO ToDTO(AttributeDataDO source)
        {
            if (source == null) return null;
            
            return new AttributeDataDTO
            {
                Id = source.Id,
                Name = source.Name,
                Source = source.Source,
                Documentation = source.Documentation
            };
        }
        
        public static AttributeDataDO ToDO(AttributeDataDTO source)
        {
            if (source == null) return null;
            
            return new AttributeDataDO
            {
                Id = source.Id,
                Name = source.Name,
                Source = source.Source,
                Documentation = source.Documentation
            };
        }
    }
}