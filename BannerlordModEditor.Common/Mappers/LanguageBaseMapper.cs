using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Models.DTO.Language;

namespace BannerlordModEditor.Common.Mappers;

public static class LanguageBaseMapper
{
    public static LanguageBaseDTO ToDTO(LanguageBaseDO source)
    {
        if (source == null) return null;

        return new LanguageBaseDTO
        {
            Type = source.Type,
            Tags = TagsMapper.ToDTO(source.Tags),
            Strings = source.Strings?
                .Select(StringItemMapper.ToDTO)
                .ToList() ?? new List<StringItemDTO>(),
            Functions = source.Functions?
                .Select(FunctionItemMapper.ToDTO)
                .ToList() ?? new List<FunctionItemDTO>()
        };
    }

    public static LanguageBaseDO ToDO(LanguageBaseDTO source)
    {
        if (source == null) return null;

        return new LanguageBaseDO
        {
            Type = source.Type,
            Tags = TagsMapper.ToDO(source.Tags),
            Strings = source.Strings?
                .Select(StringItemMapper.ToDO)
                .ToList() ?? new List<StringItemDO>(),
            Functions = source.Functions?
                .Select(FunctionItemMapper.ToDO)
                .ToList() ?? new List<FunctionItemDO>()
        };
    }
}

public static class TagsMapper
{
    public static TagsDTO ToDTO(TagsDO source)
    {
        if (source == null) return null;

        return new TagsDTO
        {
            Tags = source.Tags?
                .Select(TagMapper.ToDTO)
                .ToList() ?? new List<TagDTO>()
        };
    }

    public static TagsDO ToDO(TagsDTO source)
    {
        if (source == null) return null;

        return new TagsDO
        {
            Tags = source.Tags?
                .Select(TagMapper.ToDO)
                .ToList() ?? new List<TagDO>()
        };
    }
}

public static class TagMapper
{
    public static TagDTO ToDTO(TagDO source)
    {
        if (source == null) return null;

        return new TagDTO
        {
            Language = source.Language
        };
    }

    public static TagDO ToDO(TagDTO source)
    {
        if (source == null) return null;

        return new TagDO
        {
            Language = source.Language
        };
    }
}

public static class StringItemMapper
{
    public static StringItemDTO ToDTO(StringItemDO source)
    {
        if (source == null) return null;

        return new StringItemDTO
        {
            Id = source.Id,
            Text = source.Text
        };
    }

    public static StringItemDO ToDO(StringItemDTO source)
    {
        if (source == null) return null;

        return new StringItemDO
        {
            Id = source.Id,
            Text = source.Text
        };
    }
}

public static class FunctionItemMapper
{
    public static FunctionItemDTO ToDTO(FunctionItemDO source)
    {
        if (source == null) return null;

        return new FunctionItemDTO
        {
            FunctionName = source.FunctionName,
            FunctionBody = source.FunctionBody
        };
    }

    public static FunctionItemDO ToDO(FunctionItemDTO source)
    {
        if (source == null) return null;

        return new FunctionItemDO
        {
            FunctionName = source.FunctionName,
            FunctionBody = source.FunctionBody
        };
    }
}