using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class CreditsMapper
    {
        #region DO to DTO

        public static CreditsDTO ToDTO(CreditsDO source)
        {
            if (source == null) return null;

            return new CreditsDTO
            {
                Categories = MapList(source.Categories, CategoryToDTO),
                LoadFromFile = MapList(source.LoadFromFile, LoadFromFileToDTO)
            };
        }

        public static CreditsCategoryDTO CategoryToDTO(CreditsCategoryDO source)
        {
            if (source == null) return null;

            return new CreditsCategoryDTO
            {
                Text = source.Text,
                Sections = MapList(source.Sections, SectionToDTO),
                Entries = MapList(source.Entries, EntryToDTO),
                EmptyLines = MapList(source.EmptyLines, EmptyLineToDTO),
                LoadFromFile = MapList(source.LoadFromFile, LoadFromFileToDTO),
                Images = MapList(source.Images, ImageToDTO)
            };
        }

        public static CreditsSectionDTO SectionToDTO(CreditsSectionDO source)
        {
            if (source == null) return null;

            return new CreditsSectionDTO
            {
                Text = source.Text,
                Entries = MapList(source.Entries, EntryToDTO),
                EmptyLines = MapList(source.EmptyLines, EmptyLineToDTO)
            };
        }

        public static CreditsEntryDTO EntryToDTO(CreditsEntryDO source)
        {
            if (source == null) return null;

            return new CreditsEntryDTO
            {
                Text = source.Text,
                EmptyLines = MapList(source.EmptyLines, EmptyLineToDTO)
            };
        }

        public static CreditsEmptyLineDTO EmptyLineToDTO(CreditsEmptyLineDO source)
        {
            if (source == null) return null;

            return new CreditsEmptyLineDTO();
        }

        public static CreditsLoadFromFileDTO LoadFromFileToDTO(CreditsLoadFromFileDO source)
        {
            if (source == null) return null;

            return new CreditsLoadFromFileDTO
            {
                Name = source.Name,
                PlatformSpecific = source.PlatformSpecific,
                ConsoleSpecific = source.ConsoleSpecific
            };
        }

        public static CreditsImageDTO ImageToDTO(CreditsImageDO source)
        {
            if (source == null) return null;

            return new CreditsImageDTO
            {
                Text = source.Text
            };
        }

        #endregion

        #region DTO to DO

        public static CreditsDO ToDO(CreditsDTO source)
        {
            if (source == null) return null;

            return new CreditsDO
            {
                Categories = MapList(source.Categories, CategoryToDO),
                LoadFromFile = MapList(source.LoadFromFile, LoadFromFileToDO)
            };
        }

        public static CreditsCategoryDO CategoryToDO(CreditsCategoryDTO source)
        {
            if (source == null) return null;

            var category = new CreditsCategoryDO
            {
                Text = source.Text
            };

            // 按照原始顺序添加元素到Elements列表
            foreach (var section in MapList(source.Sections, SectionToDO))
            {
                category.Elements.Add(section);
            }
            foreach (var entry in MapList(source.Entries, EntryToDO))
            {
                category.Elements.Add(entry);
            }
            foreach (var emptyLine in MapList(source.EmptyLines, EmptyLineToDO))
            {
                category.Elements.Add(emptyLine);
            }
            foreach (var loadFromFile in MapList(source.LoadFromFile, LoadFromFileToDO))
            {
                category.Elements.Add(loadFromFile);
            }
            foreach (var image in MapList(source.Images, ImageToDO))
            {
                category.Elements.Add(image);
            }

            return category;
        }

        public static CreditsSectionDO SectionToDO(CreditsSectionDTO source)
        {
            if (source == null) return null;

            return new CreditsSectionDO
            {
                Text = source.Text,
                Entries = MapList(source.Entries, EntryToDO),
                EmptyLines = MapList(source.EmptyLines, EmptyLineToDO)
            };
        }

        public static CreditsEntryDO EntryToDO(CreditsEntryDTO source)
        {
            if (source == null) return null;

            return new CreditsEntryDO
            {
                Text = source.Text,
                EmptyLines = MapList(source.EmptyLines, EmptyLineToDO)
            };
        }

        public static CreditsEmptyLineDO EmptyLineToDO(CreditsEmptyLineDTO source)
        {
            if (source == null) return null;

            return new CreditsEmptyLineDO();
        }

        public static CreditsLoadFromFileDO LoadFromFileToDO(CreditsLoadFromFileDTO source)
        {
            if (source == null) return null;

            return new CreditsLoadFromFileDO
            {
                Name = source.Name,
                PlatformSpecific = source.PlatformSpecific,
                ConsoleSpecific = source.ConsoleSpecific
            };
        }

        public static CreditsImageDO ImageToDO(CreditsImageDTO source)
        {
            if (source == null) return null;

            return new CreditsImageDO
            {
                Text = source.Text
            };
        }

        #endregion

        #region Helper Methods

        private static List<TTarget> MapList<TSource, TTarget>(List<TSource> source, System.Func<TSource, TTarget> mapper)
        {
            return source?.Select(mapper).ToList() ?? new List<TTarget>();
        }

        #endregion
    }
}