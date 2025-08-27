using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DTO.Game;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ItemModifierGroupsMapper
    {
        public static ItemModifierGroupsDTO ToDTO(ItemModifierGroupsDO source)
        {
            if (source == null) return null;

            return new ItemModifierGroupsDTO
            {
                Groups = source.Groups?.Select(ItemModifierGroupToDTO).ToList() ?? new List<ItemModifierGroupDTO>()
            };
        }

        public static ItemModifierGroupsDO ToDO(ItemModifierGroupsDTO source)
        {
            if (source == null) return null;

            return new ItemModifierGroupsDO
            {
                Groups = source.Groups?.Select(ItemModifierGroupToDo).ToList() ?? new List<ItemModifierGroupDO>(),
                HasGroups = source.Groups != null && source.Groups.Count > 0
            };
        }

        public static ItemModifierGroupDTO ItemModifierGroupToDTO(ItemModifierGroupDO source)
        {
            if (source == null) return null;

            return new ItemModifierGroupDTO
            {
                Id = source.Id,
                NoModifierLootScore = source.NoModifierLootScore,
                NoModifierProductionScore = source.NoModifierProductionScore
            };
        }

        public static ItemModifierGroupDO ItemModifierGroupToDo(ItemModifierGroupDTO source)
        {
            if (source == null) return null;

            return new ItemModifierGroupDO
            {
                Id = source.Id,
                NoModifierLootScore = source.NoModifierLootScore,
                NoModifierProductionScore = source.NoModifierProductionScore
            };
        }
    }
}