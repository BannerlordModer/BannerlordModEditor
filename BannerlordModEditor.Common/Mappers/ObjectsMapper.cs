using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 对象配置的映射器
    /// 用于ObjectsDO和ObjectsDTO之间的双向转换
    /// </summary>
    public static class ObjectsMapper
    {
        public static ObjectsDTO ToDTO(ObjectsDO source)
        {
            if (source == null) return null;

            return new ObjectsDTO
            {
                Faction = ConvertFaction(source.Faction),
                Item = ConvertItem(source.Item),
                NPCCharacter = ConvertNPCCharacter(source.NPCCharacter),
                PlayerCharacter = ConvertPlayerCharacter(source.PlayerCharacter),
                HasFaction = source.HasFaction,
                HasItem = source.HasItem,
                HasNPCCharacter = source.HasNPCCharacter,
                HasPlayerCharacter = source.HasPlayerCharacter
            };
        }

        public static ObjectsDO ToDO(ObjectsDTO source)
        {
            if (source == null) return null;

            var item = ConvertItem(source.Item);
            var hasItem = item.Object.Count > 0;
            var hasPlayerCharacter = source.PlayerCharacter.Object.Count > 0;

            return new ObjectsDO
            {
                Faction = ConvertFaction(source.Faction),
                Item = item,
                NPCCharacter = ConvertNPCCharacter(source.NPCCharacter),
                PlayerCharacter = ConvertPlayerCharacter(source.PlayerCharacter),
                HasFaction = source.HasFaction,
                HasItem = hasItem,
                HasNPCCharacter = source.HasNPCCharacter,
                HasPlayerCharacter = hasPlayerCharacter
            };
        }

        private static FactionDTO ConvertFaction(Faction source)
        {
            if (source == null) return null;
            return new FactionDTO();
        }

        private static Faction ConvertFaction(FactionDTO source)
        {
            if (source == null) return null;
            return new Faction();
        }

        private static ObjectItemDTO ConvertItem(ObjectItem source)
        {
            if (source == null || source.Object.Count == 0)
                return new ObjectItemDTO();

            var result = new ObjectItemDTO();
            foreach (var obj in source.Object)
            {
                result.Object.Add(ConvertItemObject(obj));
            }
            return result;
        }

        private static ObjectItem ConvertItem(ObjectItemDTO source)
        {
            if (source == null || source.Object.Count == 0)
                return new ObjectItem();

            var result = new ObjectItem();
            foreach (var obj in source.Object)
            {
                result.Object.Add(ConvertItemObject(obj));
            }
            return result;
        }

        private static ItemObjectDTO ConvertItemObject(ItemObject source)
        {
            if (source == null) return null;

            return new ItemObjectDTO
            {
                ItemKind = source.ItemKind,
                Id = source.Id,
                Name = source.Name,
                Attributes = ConvertAttributes(source.Attributes)
            };
        }

        private static ItemObject ConvertItemObject(ItemObjectDTO source)
        {
            if (source == null) return null;

            return new ItemObject
            {
                ItemKind = source.ItemKind,
                Id = source.Id,
                Name = source.Name,
                Attributes = ConvertAttributes(source.Attributes)
            };
        }

        private static ObjectAttributesDTO ConvertAttributes(BannerlordModEditor.Common.Models.ObjectAttributes source)
        {
            if (source == null || source.Attribute.Count == 0)
                return new ObjectAttributesDTO();

            var result = new ObjectAttributesDTO();
            foreach (var attr in source.Attribute)
            {
                result.Attribute.Add(ConvertAttribute(attr));
            }
            return result;
        }

        private static BannerlordModEditor.Common.Models.ObjectAttributes ConvertAttributes(ObjectAttributesDTO source)
        {
            if (source == null || source.Attribute.Count == 0)
                return new BannerlordModEditor.Common.Models.ObjectAttributes();

            var result = new BannerlordModEditor.Common.Models.ObjectAttributes();
            foreach (var attr in source.Attribute)
            {
                result.Attribute.Add(ConvertAttribute(attr));
            }
            return result;
        }

        private static ObjectAttributeDTO ConvertAttribute(BannerlordModEditor.Common.Models.ObjectAttribute source)
        {
            if (source == null) return null;

            return new ObjectAttributeDTO
            {
                Code = source.Code,
                Value = source.Value
            };
        }

        private static BannerlordModEditor.Common.Models.ObjectAttribute ConvertAttribute(ObjectAttributeDTO source)
        {
            if (source == null) return null;

            return new BannerlordModEditor.Common.Models.ObjectAttribute
            {
                Code = source.Code,
                Value = source.Value
            };
        }

        private static NPCCharacterDTO ConvertNPCCharacter(BannerlordModEditor.Common.Models.DO.NPCCharacter source)
        {
            if (source == null) return null;
            return new NPCCharacterDTO();
        }

        private static BannerlordModEditor.Common.Models.DO.NPCCharacter ConvertNPCCharacter(NPCCharacterDTO source)
        {
            if (source == null) return null;
            return new BannerlordModEditor.Common.Models.DO.NPCCharacter();
        }

        private static PlayerCharacterDTO ConvertPlayerCharacter(BannerlordModEditor.Common.Models.DO.PlayerCharacter source)
        {
            if (source == null || source.Object.Count == 0)
                return new PlayerCharacterDTO();

            var result = new PlayerCharacterDTO();
            foreach (var obj in source.Object)
            {
                result.Object.Add(ConvertPlayerCharacterObject(obj));
            }
            return result;
        }

        private static BannerlordModEditor.Common.Models.DO.PlayerCharacter ConvertPlayerCharacter(PlayerCharacterDTO source)
        {
            if (source == null || source.Object.Count == 0)
                return new BannerlordModEditor.Common.Models.DO.PlayerCharacter();

            var result = new BannerlordModEditor.Common.Models.DO.PlayerCharacter();
            foreach (var obj in source.Object)
            {
                result.Object.Add(ConvertPlayerCharacterObject(obj));
            }
            return result;
        }

        private static PlayerCharacterObjectDTO ConvertPlayerCharacterObject(BannerlordModEditor.Common.Models.DO.PlayerCharacterObject source)
        {
            if (source == null) return null;

            return new PlayerCharacterObjectDTO
            {
                Id = source.Id,
                Name = source.Name,
                Attributes = ConvertPlayerCharacterAttributes(source.Attributes),
                Skills = ConvertPlayerCharacterSkills(source.Skills)
            };
        }

        private static BannerlordModEditor.Common.Models.DO.PlayerCharacterObject ConvertPlayerCharacterObject(PlayerCharacterObjectDTO source)
        {
            if (source == null) return null;

            return new BannerlordModEditor.Common.Models.DO.PlayerCharacterObject
            {
                Id = source.Id,
                Name = source.Name,
                Attributes = ConvertPlayerCharacterAttributes(source.Attributes),
                Skills = ConvertPlayerCharacterSkills(source.Skills)
            };
        }

        private static PlayerCharacterAttributesDTO ConvertPlayerCharacterAttributes(BannerlordModEditor.Common.Models.DO.PlayerCharacterAttributes source)
        {
            if (source == null || source.Attribute.Count == 0)
                return new PlayerCharacterAttributesDTO();

            var result = new PlayerCharacterAttributesDTO();
            foreach (var attr in source.Attribute)
            {
                result.Attribute.Add(ConvertPlayerCharacterAttribute(attr));
            }
            return result;
        }

        private static PlayerCharacterAttributes ConvertPlayerCharacterAttributes(PlayerCharacterAttributesDTO source)
        {
            if (source == null || source.Attribute.Count == 0)
                return new PlayerCharacterAttributes();

            var result = new PlayerCharacterAttributes();
            foreach (var attr in source.Attribute)
            {
                result.Attribute.Add(ConvertPlayerCharacterAttribute(attr));
            }
            return result;
        }

        private static PlayerCharacterAttributeDTO ConvertPlayerCharacterAttribute(PlayerCharacterAttribute source)
        {
            if (source == null) return null;

            return new PlayerCharacterAttributeDTO
            {
                Code = source.Code,
                Value = source.Value
            };
        }

        private static PlayerCharacterAttribute ConvertPlayerCharacterAttribute(PlayerCharacterAttributeDTO source)
        {
            if (source == null) return null;

            return new PlayerCharacterAttribute
            {
                Code = source.Code,
                Value = source.Value
            };
        }

        private static PlayerCharacterSkillsDTO ConvertPlayerCharacterSkills(PlayerCharacterSkills source)
        {
            if (source == null || source.Skill.Count == 0)
                return new PlayerCharacterSkillsDTO();

            var result = new PlayerCharacterSkillsDTO();
            foreach (var skill in source.Skill)
            {
                result.Skill.Add(ConvertPlayerCharacterSkill(skill));
            }
            return result;
        }

        private static PlayerCharacterSkills ConvertPlayerCharacterSkills(PlayerCharacterSkillsDTO source)
        {
            if (source == null || source.Skill.Count == 0)
                return new PlayerCharacterSkills();

            var result = new PlayerCharacterSkills();
            foreach (var skill in source.Skill)
            {
                result.Skill.Add(ConvertPlayerCharacterSkill(skill));
            }
            return result;
        }

        private static PlayerCharacterSkillDTO ConvertPlayerCharacterSkill(PlayerCharacterSkill source)
        {
            if (source == null) return null;

            return new PlayerCharacterSkillDTO
            {
                Code = source.Code
            };
        }

        private static PlayerCharacterSkill ConvertPlayerCharacterSkill(PlayerCharacterSkillDTO source)
        {
            if (source == null) return null;

            return new PlayerCharacterSkill
            {
                Code = source.Code
            };
        }
    }
}