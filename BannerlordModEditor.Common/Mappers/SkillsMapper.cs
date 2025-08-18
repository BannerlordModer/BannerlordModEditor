using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DTO.Game;

namespace BannerlordModEditor.Common.Mappers
{
    public static class SkillsMapper
    {
        public static SkillsDTO ToDTO(SkillsDO source)
        {
            if (source == null) return null;

            return new SkillsDTO
            {
                SkillDataList = source.SkillDataList?
                    .Select(SkillsMapper.ToDTO)
                    .ToList() ?? new List<SkillDataDTO>()
            };
        }

        public static SkillsDO ToDO(SkillsDTO source)
        {
            if (source == null) return null;

            return new SkillsDO
            {
                SkillDataList = source.SkillDataList?
                    .Select(SkillsMapper.ToDO)
                    .ToList() ?? new List<SkillDataDO>()
            };
        }

        public static SkillDataDTO ToDTO(SkillDataDO source)
        {
            if (source == null) return null;

            return new SkillDataDTO
            {
                Id = source.Id,
                Name = source.Name,
                Modifiers = SkillsMapper.ToDTO(source.Modifiers),
                Documentation = source.Documentation
            };
        }

        public static SkillDataDO ToDO(SkillDataDTO source)
        {
            if (source == null) return null;

            return new SkillDataDO
            {
                Id = source.Id,
                Name = source.Name,
                Modifiers = SkillsMapper.ToDO(source.Modifiers),
                Documentation = source.Documentation,
                HasModifiers = source.Modifiers != null && source.Modifiers.AttributeModifiers.Count > 0
            };
        }

        public static ModifiersDTO ToDTO(ModifiersDO source)
        {
            if (source == null) return null;

            return new ModifiersDTO
            {
                AttributeModifiers = source.AttributeModifiers?
                    .Select(SkillsMapper.ToDTO)
                    .ToList() ?? new List<AttributeModifierDTO>()
            };
        }

        public static ModifiersDO ToDO(ModifiersDTO source)
        {
            if (source == null) return null;

            return new ModifiersDO
            {
                AttributeModifiers = source.AttributeModifiers?
                    .Select(SkillsMapper.ToDO)
                    .ToList() ?? new List<AttributeModifierDO>()
            };
        }

        public static AttributeModifierDTO ToDTO(AttributeModifierDO source)
        {
            if (source == null) return null;

            return new AttributeModifierDTO
            {
                AttribCode = source.AttribCode,
                Modification = source.Modification,
                Value = source.Value
            };
        }

        public static AttributeModifierDO ToDO(AttributeModifierDTO source)
        {
            if (source == null) return null;

            return new AttributeModifierDO
            {
                AttribCode = source.AttribCode,
                Modification = source.Modification,
                Value = source.Value
            };
        }
    }
}