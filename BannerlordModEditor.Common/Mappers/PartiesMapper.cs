using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 队伍配置的映射器
    /// 用于PartiesDO和PartiesDTO之间的双向转换
    /// </summary>
    public static class PartiesMapper
    {
        public static PartiesDTO ToDTO(PartiesDO source)
        {
            if (source == null) return null;

            return new PartiesDTO
            {
                Type = source.Type,
                Parties = ConvertParties(source.Parties),
                HasParties = source.HasParties
            };
        }

        public static PartiesDO ToDO(PartiesDTO source)
        {
            if (source == null) return null;

            var parties = ConvertParties(source.Parties);
            var hasParties = parties.PartyList.Count > 0;

            return new PartiesDO
            {
                Type = source.Type,
                Parties = parties,
                HasParties = hasParties
            };
        }

        private static PartiesList ConvertParties(Parties source)
        {
            if (source == null || source.PartyList.Count == 0)
                return new PartiesList();

            var result = new PartiesList();
            foreach (var party in source.PartyList)
            {
                result.PartyList.Add(ConvertParty(party));
            }
            return result;
        }

        private static Parties ConvertParties(PartiesList source)
        {
            if (source == null || source.PartyList.Count == 0)
                return new Parties();

            var result = new Parties();
            foreach (var party in source.PartyList)
            {
                result.PartyList.Add(ConvertParty(party));
            }
            return result;
        }

        private static PartyDTO ConvertParty(Party source)
        {
            if (source == null) return null;

            return new PartyDTO
            {
                Id = source.Id,
                Name = source.Name,
                Flags = source.Flags,
                PartyTemplate = source.PartyTemplate,
                Position = source.Position,
                AverageBearingRot = source.AverageBearingRot,
                FieldList = ConvertFields(source.FieldList)
            };
        }

        private static Party ConvertParty(PartyDTO source)
        {
            if (source == null) return null;

            return new Party
            {
                Id = source.Id,
                Name = source.Name,
                Flags = source.Flags,
                PartyTemplate = source.PartyTemplate,
                Position = source.Position,
                AverageBearingRot = source.AverageBearingRot,
                FieldList = ConvertFields(source.FieldList)
            };
        }

        private static List<FieldDTO> ConvertFields(List<Field> source)
        {
            if (source == null || source.Count == 0)
                return new List<FieldDTO>();

            var result = new List<FieldDTO>();
            foreach (var field in source)
            {
                result.Add(ConvertField(field));
            }
            return result;
        }

        private static List<Field> ConvertFields(List<FieldDTO> source)
        {
            if (source == null || source.Count == 0)
                return new List<Field>();

            var result = new List<Field>();
            foreach (var field in source)
            {
                result.Add(ConvertField(field));
            }
            return result;
        }

        private static FieldDTO ConvertField(Field source)
        {
            if (source == null) return null;

            return new FieldDTO
            {
                FieldName = source.FieldName,
                FieldValue = source.FieldValue
            };
        }

        private static Field ConvertField(FieldDTO source)
        {
            if (source == null) return null;

            return new Field
            {
                FieldName = source.FieldName,
                FieldValue = source.FieldValue
            };
        }
    }
}