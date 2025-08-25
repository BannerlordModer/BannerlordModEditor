using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Models.DTO.Language;

namespace BannerlordModEditor.Common.Mappers;

public static class LanguageDataMapper
{
    public static LanguageDataDTO ToDTO(LanguageDataDO source)
    {
        if (source == null) return null;

        return new LanguageDataDTO
        {
            Id = source.Id,
            Name = source.Name,
            SubtitleExtension = source.SubtitleExtension,
            SupportedIso = source.SupportedIso,
            TextProcessor = source.TextProcessor,
            UnderDevelopment = source.UnderDevelopment,
            LanguageFiles = source.LanguageFiles?
                .Select(LanguageFileMapper.ToDTO)
                .ToList() ?? new List<LanguageFileDTO>()
        };
    }

    public static LanguageDataDO ToDO(LanguageDataDTO source)
    {
        if (source == null) return null;

        return new LanguageDataDO
        {
            Id = source.Id,
            Name = source.Name,
            SubtitleExtension = source.SubtitleExtension,
            SupportedIso = source.SupportedIso,
            TextProcessor = source.TextProcessor,
            UnderDevelopment = source.UnderDevelopment,
            LanguageFiles = source.LanguageFiles?
                .Select(LanguageFileMapper.ToDO)
                .ToList() ?? new List<LanguageFileDO>()
        };
    }
}

public static class LanguageFileMapper
{
    public static LanguageFileDTO ToDTO(LanguageFileDO source)
    {
        if (source == null) return null;

        return new LanguageFileDTO
        {
            XmlPath = source.XmlPath
        };
    }

    public static LanguageFileDO ToDO(LanguageFileDTO source)
    {
        if (source == null) return null;

        return new LanguageFileDO
        {
            XmlPath = source.XmlPath
        };
    }
}