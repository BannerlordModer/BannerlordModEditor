using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class CreditsDTO
    {
        public List<CreditsCategoryDTO> Categories { get; set; } = new();
        public List<CreditsLoadFromFileDTO> LoadFromFile { get; set; } = new();

        // 业务逻辑属性
        public bool HasCategories => Categories?.Count > 0;
        public bool HasExternalFiles => LoadFromFile?.Count > 0;
    }

    public class CreditsCategoryDTO
    {
        public string Text { get; set; }
        public List<CreditsSectionDTO> Sections { get; set; } = new();
        public List<CreditsEntryDTO> Entries { get; set; } = new();
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();
        public List<CreditsLoadFromFileDTO> LoadFromFile { get; set; } = new();
        public List<CreditsImageDTO> Images { get; set; } = new();

        // 业务逻辑方法
        public bool HasContent => Sections?.Count > 0 || Entries?.Count > 0;
        public bool HasFormatting => EmptyLines?.Count > 0;
        public bool HasImages => Images?.Count > 0;
    }

    public class CreditsSectionDTO
    {
        public string Text { get; set; }
        public List<CreditsEntryDTO> Entries { get; set; } = new();
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();

        public bool HasContent => Entries?.Count > 0;
        public bool HasFormatting => EmptyLines?.Count > 0;
    }

    public class CreditsEntryDTO
    {
        public string Text { get; set; }
        public List<CreditsEmptyLineDTO> EmptyLines { get; set; } = new();

        public bool HasFormatting => EmptyLines?.Count > 0;
    }

    public class CreditsEmptyLineDTO
    {
        // 空元素，用于格式化
    }

    public class CreditsLoadFromFileDTO
    {
        public string Name { get; set; }
        public string PlatformSpecific { get; set; }
        public string ConsoleSpecific { get; set; }

        // 业务逻辑方法
        public bool IsPlatformSpecific => !string.IsNullOrEmpty(PlatformSpecific);
        public bool IsConsoleSpecific => !string.IsNullOrEmpty(ConsoleSpecific);
    }

    public class CreditsImageDTO
    {
        public string Text { get; set; }
    }
}