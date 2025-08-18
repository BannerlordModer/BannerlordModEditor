using BannerlordModEditor.Common.Models.DO.Configuration;
using BannerlordModEditor.Common.Models.DTO.Configuration;

namespace BannerlordModEditor.Common.Mappers
{
    public static class NativeStringsMapper
    {
        public static NativeStringsDTO ToDTO(NativeStringsDO source)
        {
            if (source == null) return null;

            return new NativeStringsDTO
            {
                Type = source.Type,
                Tags = NativeStringTagsMapper.ToDTO(source.Tags),
                Strings = NativeStringsCollectionMapper.ToDTO(source.Strings)
            };
        }

        public static NativeStringsDO ToDO(NativeStringsDTO source)
        {
            if (source == null) return null;

            return new NativeStringsDO
            {
                Type = source.Type,
                Tags = NativeStringTagsMapper.ToDO(source.Tags),
                Strings = NativeStringsCollectionMapper.ToDO(source.Strings)
            };
        }
    }

    public static class NativeStringTagsMapper
    {
        public static NativeStringTagsDTO ToDTO(NativeStringTagsDO source)
        {
            if (source == null) return null;

            return new NativeStringTagsDTO
            {
                TagList = source.TagList?
                    .Select(NativeStringTagsMapper.ToDTO)
                    .ToList() ?? new List<NativeStringTagDTO>()
            };
        }

        public static NativeStringTagsDO ToDO(NativeStringTagsDTO source)
        {
            if (source == null) return null;

            return new NativeStringTagsDO
            {
                TagList = source.TagList?
                    .Select(NativeStringTagsMapper.ToDO)
                    .ToList() ?? new List<NativeStringTagDO>()
            };
        }

        public static NativeStringTagDTO ToDTO(NativeStringTagDO source)
        {
            if (source == null) return null;

            return new NativeStringTagDTO
            {
                Language = source.Language
            };
        }

        public static NativeStringTagDO ToDO(NativeStringTagDTO source)
        {
            if (source == null) return null;

            return new NativeStringTagDO
            {
                Language = source.Language
            };
        }
    }

    public static class NativeStringsCollectionMapper
    {
        public static NativeStringsCollectionDTO ToDTO(NativeStringsCollectionDO source)
        {
            if (source == null) return null;

            return new NativeStringsCollectionDTO
            {
                StringList = source.StringList?
                    .Select(NativeStringsCollectionMapper.ToDTO)
                    .ToList() ?? new List<NativeStringItemDTO>()
            };
        }

        public static NativeStringsCollectionDO ToDO(NativeStringsCollectionDTO source)
        {
            if (source == null) return null;

            return new NativeStringsCollectionDO
            {
                StringList = source.StringList?
                    .Select(NativeStringsCollectionMapper.ToDO)
                    .ToList() ?? new List<NativeStringItemDO>()
            };
        }

        public static NativeStringItemDTO ToDTO(NativeStringItemDO source)
        {
            if (source == null) return null;

            return new NativeStringItemDTO
            {
                Id = source.Id,
                Text = source.Text
            };
        }

        public static NativeStringItemDO ToDO(NativeStringItemDTO source)
        {
            if (source == null) return null;

            return new NativeStringItemDO
            {
                Id = source.Id,
                Text = source.Text
            };
        }
    }
}