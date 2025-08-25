using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class SoundFilesMapper
    {
        public static SoundFilesDTO ToDTO(SoundFilesDO source)
        {
            if (source == null) return null;

            return new SoundFilesDTO
            {
                Type = source.Type,
                BankFiles = BankFilesToDTO(source.BankFiles),
                AssetFiles = AssetFilesToDTO(source.AssetFiles)
            };
        }

        public static SoundFilesDO ToDO(SoundFilesDTO source)
        {
            if (source == null) return null;

            return new SoundFilesDO
            {
                Type = source.Type,
                BankFiles = BankFilesToDo(source.BankFiles),
                AssetFiles = AssetFilesToDo(source.AssetFiles),
                HasBankFiles = source.BankFiles != null && source.BankFiles.File.Count > 0,
                HasAssetFiles = source.AssetFiles != null && source.AssetFiles.File.Count > 0
            };
        }

        public static BankFilesDTO BankFilesToDTO(BankFilesDO source)
        {
            if (source == null) return null;

            return new BankFilesDTO
            {
                File = source.File?.Select(SoundFileToDTO).ToList() ?? new List<SoundFileDTO>()
            };
        }

        public static BankFilesDO BankFilesToDo(BankFilesDTO source)
        {
            if (source == null) return null;

            return new BankFilesDO
            {
                File = source.File?.Select(SoundFileToDo).ToList() ?? new List<SoundFileDO>()
            };
        }

        public static AssetFilesDTO AssetFilesToDTO(AssetFilesDO source)
        {
            if (source == null) return null;

            return new AssetFilesDTO
            {
                File = source.File?.Select(SoundFileToDTO).ToList() ?? new List<SoundFileDTO>()
            };
        }

        public static AssetFilesDO AssetFilesToDo(AssetFilesDTO source)
        {
            if (source == null) return null;

            return new AssetFilesDO
            {
                File = source.File?.Select(SoundFileToDo).ToList() ?? new List<SoundFileDO>()
            };
        }

        public static SoundFileDTO SoundFileToDTO(SoundFileDO source)
        {
            if (source == null) return null;

            return new SoundFileDTO
            {
                Name = source.Name,
                DecompressSamples = source.DecompressSamples
            };
        }

        public static SoundFileDO SoundFileToDo(SoundFileDTO source)
        {
            if (source == null) return null;

            return new SoundFileDO
            {
                Name = source.Name,
                DecompressSamples = source.DecompressSamples
            };
        }
    }
}