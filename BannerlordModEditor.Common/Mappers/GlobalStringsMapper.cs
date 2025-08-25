using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class GlobalStringsMapper
    {
        public static GlobalStringsDTO ToDTO(GlobalStringsDO source)
        {
            if (source == null) return null;

            return new GlobalStringsDTO
            {
                Strings = source.Strings?.Select(StringItemToDTO).ToList() ?? new List<StringItemDTO>()
            };
        }

        public static GlobalStringsDO ToDO(GlobalStringsDTO source)
        {
            if (source == null) return null;

            return new GlobalStringsDO
            {
                Strings = source.Strings?.Select(StringItemToDo).ToList() ?? new List<StringItemDO>(),
                HasStrings = source.Strings != null && source.Strings.Count > 0
            };
        }

        public static StringItemDTO StringItemToDTO(StringItemDO source)
        {
            if (source == null) return null;

            return new StringItemDTO
            {
                Id = source.Id,
                Text = source.Text
            };
        }

        public static StringItemDO StringItemToDo(StringItemDTO source)
        {
            if (source == null) return null;

            return new StringItemDO
            {
                Id = source.Id,
                Text = source.Text
            };
        }
    }
}