using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Models.DTO.Multiplayer;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MPCharactersMapper
    {
        public static MPCharactersDTO ToDTO(MPCharactersDO source)
        {
            if (source == null) return null;

            return new MPCharactersDTO
            {
                NPCCharacterList = source.NPCCharacterList?
                    .Select(MPCharactersMapper.ToDTO)
                    .ToList() ?? new List<NPCCharacterDTO>()
            };
        }

        public static MPCharactersDO ToDO(MPCharactersDTO source)
        {
            if (source == null) return null;

            return new MPCharactersDO
            {
                NPCCharacterList = source.NPCCharacterList?
                    .Select(MPCharactersMapper.ToDO)
                    .ToList() ?? new List<NPCCharacterDO>()
            };
        }

        public static NPCCharacterDTO ToDTO(NPCCharacterDO source)
        {
            if (source == null) return null;

            return new NPCCharacterDTO
            {
                // Basic Identity Properties
                Id = source.Id,
                Name = source.Name,
                Level = source.Level,
                Age = source.Age,
                Voice = source.Voice,
                
                // Character Classification
                DefaultGroup = source.DefaultGroup,
                Occupation = source.Occupation,
                Culture = source.Culture,
                IsHero = source.IsHero,
                
                // Complex objects
                Face = CharacterFaceMapper.ToDTO(source.Face),
                Skills = CharacterSkillsMapper.ToDTO(source.Skills),
                Equipments = CharacterEquipmentsMapper.ToDTO(source.Equipments)
            };
        }

        public static NPCCharacterDO ToDO(NPCCharacterDTO source)
        {
            if (source == null) return null;

            return new NPCCharacterDO
            {
                // Basic Identity Properties
                Id = source.Id,
                Name = source.Name,
                Level = source.Level,
                Age = source.Age,
                Voice = source.Voice,
                
                // Character Classification
                DefaultGroup = source.DefaultGroup,
                Occupation = source.Occupation,
                Culture = source.Culture,
                IsHero = source.IsHero,
                
                // Complex objects
                Face = CharacterFaceMapper.ToDO(source.Face),
                Skills = CharacterSkillsMapper.ToDO(source.Skills),
                Equipments = CharacterEquipmentsMapper.ToDO(source.Equipments)
            };
        }
    }

    public static class CharacterFaceMapper
    {
        public static CharacterFaceDTO ToDTO(CharacterFaceDO source)
        {
            if (source == null) return null;

            return new CharacterFaceDTO
            {
                BodyProperties = BodyPropertiesMapper.ToDTO(source.BodyProperties),
                BodyPropertiesMax = BodyPropertiesMapper.ToDTO(source.BodyPropertiesMax)
            };
        }

        public static CharacterFaceDO ToDO(CharacterFaceDTO source)
        {
            if (source == null) return null;

            return new CharacterFaceDO
            {
                BodyProperties = BodyPropertiesMapper.ToDO(source.BodyProperties),
                BodyPropertiesMax = BodyPropertiesMapper.ToDO(source.BodyPropertiesMax)
            };
        }
    }

    public static class BodyPropertiesMapper
    {
        public static BodyPropertiesDTO ToDTO(BodyPropertiesDO source)
        {
            if (source == null) return null;

            return new BodyPropertiesDTO
            {
                Version = source.Version,
                Age = source.Age,
                Weight = source.Weight,
                Build = source.Build,
                Key = source.Key
            };
        }

        public static BodyPropertiesDO ToDO(BodyPropertiesDTO source)
        {
            if (source == null) return null;

            return new BodyPropertiesDO
            {
                Version = source.Version,
                Age = source.Age,
                Weight = source.Weight,
                Build = source.Build,
                Key = source.Key
            };
        }
    }

    public static class CharacterSkillsMapper
    {
        public static CharacterSkillsDTO ToDTO(CharacterSkillsDO source)
        {
            if (source == null) return null;

            return new CharacterSkillsDTO
            {
                SkillList = source.SkillList?
                    .Select(CharacterSkillsMapper.ToDTO)
                    .ToList() ?? new List<CharacterSkillDTO>()
            };
        }

        public static CharacterSkillsDO ToDO(CharacterSkillsDTO source)
        {
            if (source == null) return null;

            return new CharacterSkillsDO
            {
                SkillList = source.SkillList?
                    .Select(CharacterSkillsMapper.ToDO)
                    .ToList() ?? new List<CharacterSkillDO>()
            };
        }

        public static CharacterSkillDTO ToDTO(CharacterSkillDO source)
        {
            if (source == null) return null;

            return new CharacterSkillDTO
            {
                Id = source.Id,
                Value = source.Value
            };
        }

        public static CharacterSkillDO ToDO(CharacterSkillDTO source)
        {
            if (source == null) return null;

            return new CharacterSkillDO
            {
                Id = source.Id,
                Value = source.Value
            };
        }
    }

    public static class CharacterEquipmentsMapper
    {
        public static CharacterEquipmentsDTO ToDTO(CharacterEquipmentsDO source)
        {
            if (source == null) return null;

            return new CharacterEquipmentsDTO
            {
                EquipmentRosterList = source.EquipmentRosterList?
                    .Select(CharacterEquipmentsMapper.ToDTO)
                    .ToList() ?? new List<EquipmentRosterDTO>()
            };
        }

        public static CharacterEquipmentsDO ToDO(CharacterEquipmentsDTO source)
        {
            if (source == null) return null;

            return new CharacterEquipmentsDO
            {
                EquipmentRosterList = source.EquipmentRosterList?
                    .Select(CharacterEquipmentsMapper.ToDO)
                    .ToList() ?? new List<EquipmentRosterDO>()
            };
        }

        public static EquipmentRosterDTO ToDTO(EquipmentRosterDO source)
        {
            if (source == null) return null;

            return new EquipmentRosterDTO
            {
                EquipmentList = source.EquipmentList?
                    .Select(CharacterEquipmentsMapper.ToDTO)
                    .ToList() ?? new List<CharacterEquipmentDTO>()
            };
        }

        public static EquipmentRosterDO ToDO(EquipmentRosterDTO source)
        {
            if (source == null) return null;

            return new EquipmentRosterDO
            {
                EquipmentList = source.EquipmentList?
                    .Select(CharacterEquipmentsMapper.ToDO)
                    .ToList() ?? new List<CharacterEquipmentDO>()
            };
        }

        public static CharacterEquipmentDTO ToDTO(CharacterEquipmentDO source)
        {
            if (source == null) return null;

            return new CharacterEquipmentDTO
            {
                Slot = source.Slot,
                Id = source.Id
            };
        }

        public static CharacterEquipmentDO ToDO(CharacterEquipmentDTO source)
        {
            if (source == null) return null;

            return new CharacterEquipmentDO
            {
                Slot = source.Slot,
                Id = source.Id
            };
        }
    }
}