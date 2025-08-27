using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ModuleStringsMapper
    {
        public static ModuleStringsDTO ToDTO(ModuleStringsDO source)
        {
            if (source == null) return null;

            return new ModuleStringsDTO
            {
                Strings = source.Strings?.Select(ModuleStringItemToDTO).ToList() ?? new List<ModuleStringItemDTO>()
            };
        }

        public static ModuleStringsDO ToDO(ModuleStringsDTO source)
        {
            if (source == null) return null;

            return new ModuleStringsDO
            {
                Strings = source.Strings?.Select(ModuleStringItemToDo).ToList() ?? new List<ModuleStringItemDO>()
            };
        }

        public static ModuleStringItemDTO ModuleStringItemToDTO(ModuleStringItemDO source)
        {
            if (source == null) return null;

            return new ModuleStringItemDTO
            {
                Id = source.Id,
                Text = source.Text
            };
        }

        public static ModuleStringItemDO ModuleStringItemToDo(ModuleStringItemDTO source)
        {
            if (source == null) return null;

            return new ModuleStringItemDO
            {
                Id = source.Id,
                Text = source.Text
            };
        }
    }
}