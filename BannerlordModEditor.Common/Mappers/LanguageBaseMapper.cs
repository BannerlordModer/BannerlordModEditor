using BannerlordModEditor.Common.Models.DO;
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
            Tags = LanguageTagsMapper.ToDTO(source.Tags),
            Strings = source.Strings?
                .Select(LanguageStringMapper.ToDTO)
                .ToList() ?? new List<StringItemDTO>(),
            Functions = source.Functions?
                .Select(LanguageFunctionMapper.ToDTO)
                .ToList() ?? new List<FunctionItemDTO>()
        };
    }

    public static LanguageBaseDO ToDO(LanguageBaseDTO source)
    {
        if (source == null) return null;

        return new LanguageBaseDO
        {
            Type = source.Type,
            Tags = LanguageTagsMapper.ToDO(source.Tags),
            Strings = source.Strings?
                .Select(LanguageStringMapper.ToDO)
                .ToList() ?? new List<LanguageStringDO>(),
            Functions = source.Functions?
                .Select(LanguageFunctionMapper.ToDO)
                .ToList() ?? new List<LanguageFunctionDO>()
        };
    }
}

public static class LanguageTagsMapper
{
    public static TagsDTO ToDTO(LanguageTagsDO source)
    {
        if (source == null) return null;

        return new TagsDTO
        {
            Tags = source.Tags?
                .Select(LanguageTagMapper.ToDTO)
                .ToList() ?? new List<TagDTO>()
        };
    }

    public static LanguageTagsDO ToDO(TagsDTO source)
    {
        if (source == null) return null;

        return new LanguageTagsDO
        {
            Tags = source.Tags?
                .Select(LanguageTagMapper.ToDO)
                .ToList() ?? new List<LanguageTagDO>()
        };
    }
}

public static class LanguageTagMapper
{
    public static TagDTO ToDTO(LanguageTagDO source)
    {
        if (source == null) return null;

        return new TagDTO
        {
            Language = source.Language
        };
    }

    public static LanguageTagDO ToDO(TagDTO source)
    {
        if (source == null) return null;

        return new LanguageTagDO
        {
            Language = source.Language
        };
    }
}

public static class LanguageStringMapper
{
    public static StringItemDTO ToDTO(LanguageStringDO source)
    {
        if (source == null) return null;

        return new StringItemDTO
        {
            Id = source.Id,
            Text = source.Text
        };
    }

    public static LanguageStringDO ToDO(StringItemDTO source)
    {
        if (source == null) return null;

        return new LanguageStringDO
        {
            Id = source.Id,
            Text = source.Text
        };
    }
}

public static class LanguageFunctionMapper
{
    public static FunctionItemDTO ToDTO(LanguageFunctionDO source)
    {
        if (source == null) return null;

        return new FunctionItemDTO
        {
            FunctionName = source.FunctionName,
            FunctionBody = source.FunctionBody
        };
    }

    public static LanguageFunctionDO ToDO(FunctionItemDTO source)
    {
        if (source == null) return null;

        return new LanguageFunctionDO
        {
            FunctionName = source.FunctionName,
            FunctionBody = source.FunctionBody
        };
    }
}